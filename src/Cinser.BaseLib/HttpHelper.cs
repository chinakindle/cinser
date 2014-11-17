using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Cinser.BaseLib
{

    public class HttpHelper
    {
        public static readonly string AgentFireFox = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2.23) Gecko/20110920 Firefox/3.6.23";
        public static readonly string AgentIE10 = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0;)";
        public static readonly string AgentIE6 = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        public static readonly string AgentIE7 = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; Trident/4.0; .NET CLR 1.1.4322; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
        public static readonly string AgentIE9 = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; BOIE9;ZHCN)";
        public static readonly string DefaultAccept = "*/*";
        public static readonly string DefaultContentType = "text/html; charset=utf-8";
        public static readonly int DefaultTimeOut = 0x7530;
        public static readonly string DefaultUserAgent = AgentIE10;
        public static WebProxy proxy = null;

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public static HttpWebRequest CreateGetHttpResponse(string url, int? timeout, string userAgent, string accept, CookieContainer cookies, string referer)
        {
            Debug.WriteLine("CreateGetHttpResponse url:" + url);
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.KeepAlive = true;
            request.Accept = DefaultAccept;
            request.UserAgent = DefaultUserAgent;
            request.Headers.Add("Accept-Language: zh-CN");
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            if (null != proxy)
            {
                request.Proxy = proxy;
            }
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (!string.IsNullOrEmpty(accept))
            {
                request.Accept = accept;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            else
            {
                request.Timeout = DefaultTimeOut;
            }
            if (cookies != null)
            {
                request.CookieContainer = cookies;
            }
            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }
            return request;
        }

        public static HttpWebResponse CreatePostHttpResponse(string url, List<KeyValuePair<string, string>> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieContainer cookies, string referer)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(HttpHelper.CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Language: zh-cn");
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            if (null != proxy)
            {
                request.Proxy = proxy;
            }
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            else
            {
                request.Timeout = DefaultTimeOut;
            }
            if (cookies != null)
            {
                request.CookieContainer = cookies;
            }
            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }
            if ((parameters != null) && (parameters.Count != 0))
            {
                byte[] bytes = requestEncoding.GetBytes(MergeParam(parameters).ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            return (request.GetResponse() as HttpWebResponse);
        }

        private static string GetResponseString(HttpWebResponse response, Encoding encoding)
        {
            string str = string.Empty;
            if (response != null)
            {
                Stream responseStream = response.GetResponseStream();
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                }
                StreamReader reader = null;
                if (null == encoding)
                {
                    reader = new StreamReader(responseStream);
                }
                else
                {
                    reader = new StreamReader(responseStream, encoding);
                }
                str = reader.ReadToEnd();
                if (null != reader)
                {
                    reader.Close();
                }
                if (null != responseStream)
                {
                    responseStream.Close();
                }
            }
            return str;
        }

        public static string GetString(string url)
        {
            return GetString(url, null);
        }

        public static string GetString(string url, CookieContainer cookieContainer)
        {
            return GetString(url, cookieContainer, string.Empty);
        }

        public static string GetString(string url, CookieContainer cookieContainer, string referer)
        {
            return GetString(url, null, cookieContainer, referer, null);
        }

        public static string GetString(string url, int? timeout, CookieContainer cookieContainer, string referer)
        {
            return GetString(url, timeout, cookieContainer, referer, null);
        }

        public static string GetString(string url, int? timeout, CookieContainer cookies, string referer, Encoding encoding)
        {
            string message = string.Empty;
            HttpWebRequest request = CreateGetHttpResponse(url, timeout, null, null, cookies, referer);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                message = GetResponseString(response, encoding);
                Debug.WriteLine(message);
            }
            catch (Exception)
            {
            }
            return message;
        }

        public static string GetStringByEncoding(string url, Encoding encoding)
        {
            return GetString(url, null, null, null, encoding);
        }

        public static Stream GetVerificationCode(string url, int? timeout, string accept, string refrere, CookieContainer cookies)
        {
            Stream responseStream = null;
            HttpWebRequest request = CreateGetHttpResponse(url, timeout, null, accept, cookies, refrere);
            string cookieHeader = string.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                responseStream = response.GetResponseStream();
                cookieHeader = response.Headers.Get("Set-Cookie") ?? string.Empty;
                cookies.SetCookies(new Uri(url), cookieHeader);
            }
            catch (Exception)
            {
            }
            return responseStream;
        }

        public static string GetWebValueByCond(string webResult, string startMark, string endMark)
        {
            string str = string.Empty;
            if (webResult.IndexOf(startMark) > -1)
            {
                int startIndex = webResult.IndexOf(startMark) + startMark.Length;
                int index = webResult.IndexOf(endMark, startIndex);
                if (startIndex > -1)
                {
                    str = webResult.Substring(startIndex, index - startIndex);
                }
            }
            return str;
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
        public static StringBuilder MergeParam(List<KeyValuePair<string, string>> parameters)
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            foreach (KeyValuePair<string, string> pair in parameters)
            {
                if (num > 0)
                {
                    builder.AppendFormat("&{0}={1}", pair.Key, pair.Value);
                }
                else
                {
                    builder.AppendFormat("{0}={1}", pair.Key, pair.Value);
                }
                num++;
            }
            return builder;
        }

        public static void OpenIE(string uri)
        {
            Process.Start("IExplore.exe", uri);
        }

        public static string PostString(string url, List<KeyValuePair<string, string>> parameters, int? timeout, string userAgent, Encoding encoding, CookieContainer cookies, string referer)
        {
            string message = string.Empty;
            try
            {
                message = GetResponseString(CreatePostHttpResponse(url, parameters, timeout, userAgent, encoding, cookies, referer), null);
                Debug.WriteLine(message);
            }
            catch (Exception)
            {
            }
            return message;
        }

        public static string ReplaceHTMLAttributes(string html)
        {
            html = Regex.Replace(html, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "-->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<!--.*", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(iexcl|#161);", "\x00a1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(cent|#162);", "\x00a2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(pound|#163);", "\x00a3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&(copy|#169);", "\x00a9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            html.Replace("<", "");
            html.Replace(">", "");
            return html;
        }

        public void DownHttpFile(string filePath, Stream saveFileStream)
        {
            WebRequest request = WebRequest.Create(filePath);

            request.BeginGetResponse((responseAsyncCallBack) =>
            {
                using (Stream openFileStream = saveFileStream)
                {
                    #region 获取response bytes
                    WebResponse response = request.EndGetResponse(responseAsyncCallBack);
                    Stream responseStream = response.GetResponseStream();
                    Byte[] bufferBytes = new Byte[responseStream.Length];
                    responseStream.Read(bufferBytes, 0, bufferBytes.Length);
                    #endregion
                    openFileStream.Write(bufferBytes, 0, bufferBytes.Length);
                    openFileStream.Flush();
                }
            }, null);
        }
    }
}
