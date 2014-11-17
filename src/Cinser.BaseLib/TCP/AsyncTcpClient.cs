using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Cinser.BaseLib.TCP
{
    /// <summary>
    /// 基于IAsyncResult的异步设计模式的TCP通讯客户端公共类
    /// 创建时间: 2013-8-1
    /// 作    者: 伍新生
    /// </summary>
    public class AsyncTcpClient
    {
        protected TcpClient client;
        protected NetworkStream networkStream;

        //用于线程同步，初始状态设为非终止状态，使用手动重置方式        
        protected EventWaitHandle allDone = new EventWaitHandle(false, EventResetMode.ManualReset);
        
        /// <summary>
        /// 服务端IPEndPoint接口
        /// </summary>
        protected IPAddress ServerIP;

        /// <summary>
        /// 服务器监听端口
        /// </summary>
        protected int Port;

        /// <summary>
        /// 当前状态
        /// </summary>
        public StateType State;

        /// <summary>
        /// 状态类型
        /// </summary>
        public enum StateType
        {
            /// <summary>
            /// 连接失败
            /// </summary>
            FaildConnected = 0,
            /// <summary>
            /// 已连接
            /// </summary>
            Connected = 1,
            /// <summary>
            /// 连接停止
            /// </summary>
            Stoped = 2,
            /// <summary>
            /// 初始化失败
            /// </summary>
            InitFailed = 3,
            /// <summary>
            /// 正在连接
            /// </summary>
            Connecting
        }

        /// <summary>
        /// 发生异常事件委托对象
        /// </summary>
        public delegate void ExceptionHandle(object sender, Exception ex);

        /// <summary>
        /// 发生异常事件
        /// </summary>
        public event ExceptionHandle ExceptionHappened;

        /// <summary>
        /// 捕获到新消息时的事件委托对象
        /// </summary>
        public delegate void NewMessageHandle(byte[] msgContent, TcpCommon.MessageType msgType);

        /// <summary>
        /// 捕获到新消息事件
        /// </summary>
        public event NewMessageHandle CatchNewMessage;

        /// <summary>
        /// 与服务器断开连接监测委托
        /// </summary>
        public delegate void ServerMissedHandle();

        /// <summary>
        /// 与服务器断开连接触发事件
        /// </summary>
        public event ServerMissedHandle ServerMissed;

        /// <summary>
        /// 状态改变事件委托
        /// </summary>
        public delegate void StateChangedHandle(StateType oldState, StateType newState);

        /// <summary>
        /// 状态改变事件
        /// </summary>
        public event StateChangedHandle StateChanged;

        public AsyncTcpClient(string serverIp, int port)
        {
            if (IPAddress.TryParse(serverIp, out this.ServerIP))
            {
                this.Port = port;
            }
            else
            {
                TriggerStateChanged(StateType.InitFailed);
                TriggerException(new Exception("您设置的服务器IP地址(" + serverIp + ")不正确."));
            }
        }

        /// <summary>
        /// 连接至服务器
        /// </summary>
        public virtual void Connect()
        {
            if (State != StateType.Connecting)
            {
                //使用IPv4
                client = new TcpClient(AddressFamily.InterNetwork);
                //创建一个委托，让其引用在异步操作完成时调用的回调方法
                AsyncCallback requestCallback = new AsyncCallback(RequestCallback);
                //将事件的状态设为非终止状态
                allDone.Reset();
                //开始一个对远程主机的异步请求
                client.BeginConnect(ServerIP, Port, requestCallback, client);
                TriggerStateChanged(StateType.Connecting);

                //阻塞当前线程，即窗体界面不再响应任何用户操作，等待BeginConnect完成
                //这样做的目的是为了与服务器连接有结果（成功或失败）时，才能继续
                //当BeginConnect完成时，会自动调用RequestCallback，
                //通过在RequestCallback中调用Set方法解除阻塞
                //allDone.WaitOne();
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public virtual void Disconnect()
        {
            TriggerStateChanged(StateType.Stoped);
            allDone.Set();
            System.Threading.Thread.Sleep(2000);
            client.Close();
        }

        private void RequestCallback(IAsyncResult ar)
        {
            //异步操作能执行到此处，说明调用BeginConnect已经完成，
            //并得到了IAsyncResult类型的状态参数ar，但BeginConnect尚未结束
            //此时需要解除阻塞,以便能调用EndConnect
            allDone.Set();
            //调用Set后，事件状态变为终止状态，当前线程继续，
            //buttonConnect_Click执行结束，
            //同时窗体界面可以响应用户操作
            try
            {
                //获取连接成功后得到的状态参数
                client = (TcpClient)ar.AsyncState;
                //异步接受传入的连接尝试，使BeginConnect正常结束
                client.EndConnect(ar);

                TriggerStateChanged(StateType.Connected);
                //获取接收和发送数据的网络流
                networkStream = client.GetStream();
                //异步接收服务器发送的数据，BeginRead完成后，会自动调用ReadCallback
                ReadObject readObject = new ReadObject(networkStream, client.ReceiveBufferSize);
                networkStream.BeginRead(readObject.bytes, 0, readObject.bytes.Length, ReadCallback, readObject);
            }
            catch (Exception err)
            {
                TriggerStateChanged(StateType.FaildConnected);
                if (ServerMissed != null)
                {
                    ServerMissed();
                }
                return;
            }
        }
        private void ReadCallback(IAsyncResult ar)
        {
            //异步操作能执行到此处，说明调用BeginRead已经完成
            try
            {
                ReadObject readObject = (ReadObject)ar.AsyncState;
                int count = readObject.netStream.EndRead(ar);
                if (CatchNewMessage != null)
                {
                    TcpCommon.MessageType msgType = (TcpCommon.MessageType)TcpCommon.TakeOutMessageType(ref readObject.bytes);
                    CatchNewMessage(readObject.bytes, msgType);
                }
                if (State == StateType.Connected)
                {
                    //重新调用BeginRead进行异步读取
                    readObject = new ReadObject(networkStream, client.ReceiveBufferSize);
                    networkStream.BeginRead(readObject.bytes, 0, readObject.bytes.Length, ReadCallback, readObject);
                }
            }
            catch (Exception err)
            {
                TriggerStateChanged(StateType.FaildConnected);
                if(ServerMissed!=null)
                    ServerMissed();
            }
        }

        public void SendMessage(string content)
        {
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(content);
            SendMessage(byteData, TcpCommon.MessageType.Text);
        }

        public void SendMessage(System.IO.Stream content,TcpCommon.MessageType msgType)
        {
            byte[] byteData = new byte[content.ReadByte()];
            content.Read(byteData, 0, byteData.Length);
            SendMessage(byteData, msgType);
        }

        public void SendMessage(byte[] byteData, TcpCommon.MessageType msgType)
        {
            try
            {
                TcpCommon.AddMessageType(msgType, ref byteData);
                networkStream.BeginWrite(byteData, 0, byteData.Length, new AsyncCallback(SendCallback), networkStream);
                networkStream.Flush();
            }
            catch (Exception err)
            {
                TriggerException("SendMessage", err);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                networkStream.EndWrite(ar);
            }
            catch (Exception err)
            {
                TriggerException("SendMessage", err);
            }
        }

        //用于回调参数
        public class ReadObject
        {
            public NetworkStream netStream;
            public byte[] bytes;
            public ReadObject(NetworkStream netStream, int bufferSize)
            {
                this.netStream = netStream;
                bytes = new byte[bufferSize];
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

        private void TriggerStateChanged(StateType newSate)
        {
            StateType oldState = State;
            State = newSate;
            if (StateChanged != null)
            {
                StateChanged(oldState, newSate);
            }
        }
    }
}
