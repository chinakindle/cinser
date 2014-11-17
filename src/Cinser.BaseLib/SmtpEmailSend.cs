namespace Cinser.BaseLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Runtime.InteropServices;
    using System.Text;

    public class SmtpEmailSend
    {
        private static MailAddress MailAddress_from = null;
        private static MailAddress MailAddress_to = null;
        private static MailMessage MailMessage_Mai = new MailMessage();
        private static System.Net.Mail.SmtpClient SmtpClient = null;

        public static bool Attachment_MaiInit(string path, out string msg)
        {
            try
            {
                msg = string.Empty;
                FileStream stream = new FileStream(path, FileMode.Open);
                string name = stream.Name;
                int num = (int) ((stream.Length / 0x400) / 0x400);
                stream.Close();
                if (num > 3)
                {
                    msg = "文件长度不能大于3M！你选择的文件大小为" + num.ToString() + "M";
                    return false;
                }
                return true;
            }
            catch (IOException)
            {
                msg = "检测附件大小时发生异常";
                return false;
            }
        }

        public static bool SendEmail(List<string> strMailAddressTo, string title, string content, string smtpAddress, int smtpPort, string sendEmailFrom, string sendEmailFromPwd, string showNickName, string filePath, out string msg, SendCompletedEventHandler callback)
        {
            Exception exception;
            bool flag = false;
            msg = string.Empty;
            try
            {
                setSmtpClient(smtpAddress, smtpPort);
            }
            catch (Exception exception1)
            {
                exception = exception1;
                msg = "请确定SMTP服务名是否正确！";
                return flag;
            }
            try
            {
                setAddressform(sendEmailFrom, sendEmailFromPwd, showNickName);
            }
            catch (Exception exception2)
            {
                exception = exception2;
                msg = "请确定发件邮箱地址和密码的正确性！";
                return flag;
            }
            MailMessage_Mai.To.Clear();
            foreach (string str in strMailAddressTo)
            {
                MailAddress_to = new MailAddress(str);
                MailMessage_Mai.To.Add(MailAddress_to);
            }
            MailMessage_Mai.From = MailAddress_from;
            MailMessage_Mai.Subject = title;
            MailMessage_Mai.SubjectEncoding = Encoding.UTF8;
            MailMessage_Mai.Body = content;
            MailMessage_Mai.BodyEncoding = Encoding.UTF8;
            MailMessage_Mai.Priority = MailPriority.High;
            MailMessage_Mai.IsBodyHtml = false;
            MailMessage_Mai.Attachments.Clear();
            if (!string.IsNullOrEmpty(filePath))
            {
                MailMessage_Mai.Attachments.Add(new Attachment(filePath));
            }
            SmtpClient.SendCompleted += new SendCompletedEventHandler(callback.Invoke);
            SmtpClient.SendAsync(MailMessage_Mai, "000000000");
            return true;
        }

        public static void setAddressform(string strMailAddress, string strMailPwd, string strNickName)
        {
            NetworkCredential credential = new NetworkCredential(strMailAddress, strMailPwd);
            MailAddress_from = new MailAddress(strMailAddress, strNickName);
            SmtpClient.Credentials = new NetworkCredential(MailAddress_from.Address, strMailPwd);
        }

        public static void setSmtpClient(string ServerHost, int Port)
        {
            SmtpClient = new System.Net.Mail.SmtpClient();
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpClient.Host = ServerHost;
            SmtpClient.Port = Port;
            SmtpClient.Timeout = 0;
        }
    }
}

