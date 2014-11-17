using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Cinser.BaseLib.TCP
{
    /// <summary>
    /// 基于IAsyncResult的异步设计模式的TCP通讯服务端公共类
    /// 创建时间: 2013-8-1
    /// 作    者: 伍新生
    /// </summary>
    public class AsyncTcpServer
    {
        protected TcpListener listener;

        //用于线程同步，初始状态设为非终止状态，使用手动重置方式
        protected EventWaitHandle allDone =  new EventWaitHandle(false, EventResetMode.ManualReset);
        
        /// <summary>
        /// 当前连接了本服务器的客户端
        /// </summary>
        public List<ReadWriteObject> clients = new List<ReadWriteObject>();

        /// <summary>
        /// 捕获新连接的事件委托对象
        /// </summary>
        public delegate void ClientConnectHandle(TcpClient sender);
        
        /// <summary>
        /// 捕获到新的连接事件时触发的事件
        /// </summary>
        public event ClientConnectHandle CatchClientConnect;

        /// <summary>
        /// 捕获到连接退出时的事件委托对象
        /// </summary>
        public delegate void QuitConnectHandle(TcpClient sender);

        /// <summary>
        /// 捕获到连接退出时触发的事件
        /// </summary>
        public event QuitConnectHandle CatchQuitConnect;

        /// <summary>
        /// 捕获到新消息时的事件委托对象
        /// </summary>
        public delegate void NewMessageHandle(TcpClient sender, byte[] msgContent, TcpCommon.MessageType msgType);

        /// <summary>
        /// 捕获到新消息事件
        /// </summary>
        public event NewMessageHandle CatchNewMessage;

        /// <summary>
        /// 发生异常事件委托对象
        /// </summary>
        public delegate void ExceptionHandle(object sender, Exception ex);

        /// <summary>
        /// 发生异常事件
        /// </summary>
        public event ExceptionHandle ExceptionHappened;

        /// <summary>
        /// 当前状态
        /// </summary>
        public StateType State;

        /// <summary>
        /// 状态类型
        /// </summary>
        public enum StateType
        {
            // 摘要:监听未启动
            NotStart = 0,
            // 摘要:监听已启动
            Started = 1,
            // 摘要:监听停止
            Stoped = 2,
            // 摘要:初始化失败
            InitFailed = 3
        }

        /// <summary>
        /// 服务端IPEndPoint接口
        /// </summary>
        protected IPEndPoint ServerIEP;

        /// <summary>
        /// 根据IP,端口初始化服务端
        /// </summary>
        public AsyncTcpServer(string serverIp, int port)
        {
            IPAddress ip;
            if (IPAddress.TryParse(serverIp, out ip))
            {
                ServerIEP = new IPEndPoint(ip, port);
            }
            else
            {
                this.State = StateType.InitFailed;
                TriggerException( new Exception("您设置的服务器IP地址(" + serverIp + ")不正确."));
            }
        }
        
        /// <summary>
        /// 通过析构函数初始化连接状态
        /// </summary>
        ~AsyncTcpServer()
        {
            this.State = StateType.NotStart;
        }


        /// <summary>
        /// 启动监听
        /// </summary>
        public virtual void Start()
        {
            //由于服务器要为多个客户服务，所以需要创建一个线程监听客户端连接请求
            ThreadStart ts = new ThreadStart(AcceptConnect);
            Thread myThread = new Thread(ts);

            this.State = StateType.Started;
            myThread.Start();
        }

        public virtual void Restart()
        {
            Stop();
            Start();
        }

        public virtual void Stop()
        {
            //使线程自动结束
            this.State = StateType.Stoped;
            //将事件状态设置为终止状态，允许一个或者多个等待线程继续
            //从而使线程正常结束
            allDone.Set();
            System.Threading.Thread.Sleep(2000);
            listener.Stop();
        }

        private void AcceptConnect()
        {
            try
            {
                listener = null;
                listener = new TcpListener(ServerIEP);
                listener.Start();
                while (State == StateType.Started)
                {
                    //将事件的状态设为非终止
                    allDone.Reset();
                    //引用在异步操作完成时调用的回调方法
                    AsyncCallback callback = new AsyncCallback(AcceptTcpClientCallback);

                    //开始一个异步操作接受传入的连接尝试
                    listener.BeginAcceptTcpClient(callback, listener);
                    //阻塞当前线程，直到收到客户连接信号
                    allDone.WaitOne();
                }
            }
            catch (Exception err)
            {
                TriggerException("AcceptConnect", err);
                return;
            }
        }


        /// <summary>
        /// 实时捕获客户端传回来的结果
        /// </summary>
        /// <param name="ar">ar是IAsyncResult类型的接口，表示异步操作的状态
        /// 是由listener.BeginAcceptTcpClient(callback, listener)传递过来的</param>
        private void AcceptTcpClientCallback(IAsyncResult ar)
        {
            try
            {
                //将事件状态设为终止状态，允许一个或多个等待线程继续
                allDone.Set();
                TcpListener myListener = (TcpListener)ar.AsyncState;
                //异步接收传入的连接，并创建新的TcpClient对象处理远程主机通信
                TcpClient client = myListener.EndAcceptTcpClient(ar);

                //触发捕获到新连接事件
                if (CatchClientConnect != null)
                    CatchClientConnect(client);
                
                ReadWriteObject readWriteObject = new ReadWriteObject(client);

                if (clients.Contains(readWriteObject) == false)
                    clients.Add(readWriteObject);

                readWriteObject.netStream.BeginRead(readWriteObject.readBytes,
                    0, readWriteObject.readBytes.Length, ReadCallback, readWriteObject);
            }
            catch (Exception err)
            {
                TriggerException("AcceptTcpClientCallback", err);
                return;
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            ReadWriteObject readWriteObject = (ReadWriteObject)ar.AsyncState;
            try
            {
                int count = readWriteObject.netStream.EndRead(ar);

                //触发捕获到新消息的事件
                if (CatchNewMessage != null)
                {
                    TcpCommon.MessageType msgType = TcpCommon.TakeOutMessageType(ref readWriteObject.readBytes);
                    CatchNewMessage(readWriteObject.client, readWriteObject.readBytes, msgType);
                }

                if (State == StateType.Started)
                {
                    readWriteObject.InitReadArray();
                    readWriteObject.netStream.BeginRead(readWriteObject.readBytes,
                        0, readWriteObject.readBytes.Length, ReadCallback, readWriteObject);
                }
            }
            catch (Exception err)
            {
                clients.Remove(readWriteObject);
                if (CatchQuitConnect != null)
                {
                    CatchQuitConnect(readWriteObject.client);
                }
            }
        }

        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        public virtual void SendMessage(ReadWriteObject readWriteObject, string str)
        {
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(str);
            SendMessage(readWriteObject, byteData, TcpCommon.MessageType.Text);
        }

        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        public virtual void SendMessage(ReadWriteObject readWriteObject, System.IO.Stream content, TcpCommon.MessageType msgType)
        {
            byte[] byteData = new byte[content.ReadByte()];
            content.Read(byteData, 0, content.ReadByte());
            SendMessage(readWriteObject, byteData, msgType);
        }

        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        public virtual void SendMessage(ReadWriteObject readWriteObject, byte[] content, TcpCommon.MessageType msgType)
        {
            try
            {
                readWriteObject.writeBytes = content;
                TcpCommon.AddMessageType(msgType, ref readWriteObject.writeBytes);
                readWriteObject.netStream.BeginWrite(readWriteObject.writeBytes,
                    0, readWriteObject.writeBytes.Length,
                    new AsyncCallback(SendCallback), readWriteObject);
                readWriteObject.netStream.Flush();
            }
            catch (Exception err)
            {
                TriggerException("SendMessage", err);
            }
        }

        /// <summary>
        /// 向所有客户端发送消息
        /// </summary>
        public virtual void SendMessage(string str)
        {
            foreach (var readWriteObject in clients)
            {
                SendMessage(readWriteObject, str);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            ReadWriteObject readWriteObject = (ReadWriteObject)ar.AsyncState;
            try
            {
                readWriteObject.netStream.EndWrite(ar);
            }
            catch (Exception err)
            {
                clients.Remove(readWriteObject);
                TriggerException("SendCallback", err);
            }
        }

        public class ReadWriteObject
        {
            public TcpClient client;
            public NetworkStream netStream;
            public byte[] readBytes;
            public byte[] writeBytes;
            
            public ReadWriteObject(TcpClient client)
            {
                this.client = client;
                netStream = client.GetStream();
                readBytes = new byte[client.ReceiveBufferSize];
                writeBytes = new byte[client.SendBufferSize];
            }

            public void InitReadArray()
            {
                readBytes = new byte[client.ReceiveBufferSize];
            }

            public void InitWriteArray()
            {
                writeBytes = new byte[client.SendBufferSize];
            }
        }

        private void TriggerException(Exception ex)
        {
            if (ExceptionHappened != null)
            {
                ExceptionHappened(null, ex);
            }
        }

        private void TriggerException(object sender, Exception ex)
        {
            if (ExceptionHappened != null)
            {
                ExceptionHappened(sender, ex);
            }
        }

    }
}
