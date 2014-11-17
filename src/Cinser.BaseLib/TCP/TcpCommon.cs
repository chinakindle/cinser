using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinser.BaseLib.TCP
{
    public class TcpCommon
    {
        /// <summary>
        /// 消息类型枚举
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// 文本
            /// </summary>
            Text = 0,
            /// <summary>
            /// 图像文件
            /// </summary>
            Image,
            /// <summary>
            /// 声频文件
            /// </summary>
            Audio,
            /// <summary>
            /// 视频文件
            /// </summary>
            Vidio,
            /// <summary>
            /// 压缩文件
            /// </summary>
            RAR,
            /// <summary>
            /// 其它文件
            /// </summary>
            File,
            /// <summary>
            /// 不设置消息类型
            /// </summary>
            NotSet
        }

        /// <summary>
        /// MessageType的分隔符，使得所有发送消息都为byte[]{MessageType + MessageTypeSpliter + content},用于读取时解析消息类型
        /// </summary>
        public static byte[] MessageTypeSpliter = new byte[] { 42 };


        public static void AddMessageType(MessageType msgType, ref byte[] content)
        {
            byte[] newContent = null;
            string type = ((int)msgType).ToString();
            byte[] typeByte = TextToByteConverter(type);
            newContent = new byte[content.Length + typeByte.Length + MessageTypeSpliter.Length];

            int newContentIndex = 0;
            foreach (var item in typeByte)
            {
                newContent[newContentIndex++] = item;
            }

            newContent[newContentIndex++] = MessageTypeSpliter[0];

            foreach (var item in content)
            {
                newContent[newContentIndex++] = item;
            }
            content = newContent;
        }

        public static MessageType TakeOutMessageType(ref byte[] content)
        {
            MessageType msgType = MessageType.NotSet;

            byte[] typeByte;
            int typeLenght = 0;

            byte[] newContent;

            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == MessageTypeSpliter[0])
                {
                    typeLenght = i;
                    break;
                }
            }

            typeByte = new byte[typeLenght];
            newContent = new byte[content.Length - typeByte.Length - 1];

            for (int i = 0; i < content.Length; i++)
            {
                if (i < typeLenght)
                {
                    typeByte[i] = content[i];
                }
                else if (i > typeLenght)
                {
                    newContent[i - typeLenght - 1] = content[i];
                }
            }

            content = newContent;

            int typeEnumValue = -1;
            if (int.TryParse(ByteToTextConverter(typeByte), out typeEnumValue))
            {
                string[] arr = Enum.GetNames(typeof(MessageType));

                for (int i = 0; i < arr.Length; i++)
                {
                    msgType = (MessageType)Enum.Parse(typeof(MessageType), arr[i], true);
                    if ((int)(msgType) == typeEnumValue)
                    {
                        break;
                    }
                }
            }
            return msgType;
        }

        public static byte[] TextToByteConverter(string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        public static string ByteToTextConverter(byte[] bt)
        {
            return System.Text.Encoding.UTF8.GetString(bt);
        }
    }
}
