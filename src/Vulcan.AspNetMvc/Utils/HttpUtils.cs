using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Net;
using System.IO;

namespace Vulcan.AspNetMvc.Utils
{
    public static class HttpUtils
    {
        public static string Post(string url, SortedDictionary<string, string> data, Encoding encoding)
        {
            string strdata = DictToStr(data, "&", encoding);
            return Net.HttpRequest(url, strdata, "POST", 60000, encoding);
        }

        public static string Get(string url, SortedDictionary<string, string> data, Encoding encoding)
        {
            string strdata = DictToStr(data, "&", encoding);
            return Net.HttpRequest(url, strdata, "GET", 60000, encoding);
        }

        /// <summary>
        /// 字典转字符串
        /// </summary>
        /// <param name="dict">字典类型数据</param>
        /// <param name="str_join">连接字符串</param>
        /// <param name="coding">url编码</param>
        /// <returns>字典数据的key和value组成的字符串</returns>
        private static string DictToStr(SortedDictionary<string, string> dict, string str_join, Encoding coding)
        {
            //连接字符串
            str_join = str_join == null ? "&" : str_join;
            StringBuilder result = new StringBuilder();
            string value = string.Empty;
            int i = 0;
            foreach (KeyValuePair<string, string> kv in dict)
            {
                value = HttpUtility.UrlEncode(kv.Value, coding);
                result.AppendFormat("{0}{1}={2}", i > 0 ? str_join : "", kv.Key, value);
                i++;
            }
            return result.ToString();
        }
    }

    internal static class HttpVerbs
    {

        public const string POST = "POST";
        public const string GET = "GET";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
        public const string HEAD = "HEAD";
        //public const string POST = "POST";
    }
    /**/
    /// <summary>
    /// Net : 提供静态方法，对常用的网络操作进行封装
    /// </summary>
    internal sealed class Net
    {

        /// <summary>
        /// 返回URL内容,带POST数据提交
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="data">数据</param>
        /// <param name="method">GET/POST(默认)</param>
        /// <param name="timeout">超时时间（以毫秒为单位）</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static string HttpRequest(string url, string data, string method, int timeout, Encoding encoding)
        {
            string res = string.Empty;
            //Encoding encoding = Encoding.GetEncoding("utf-8");

            //请求
            WebRequest webRequest = null;
            Stream postStream = null;

            //响应
            WebResponse webResponse = null;
            StreamReader streamReader = null;

            try
            {
                if (method == "GET")
                {
                    if (url.IndexOf("?") > 0)
                    {
                        url = url + '&' + data;
                    }
                    else
                    {
                        url = url + '?' + data;
                    }
                }
                //请求
                webRequest = WebRequest.Create(url);
                webRequest.Method = string.IsNullOrEmpty(method) ? "POST" : method;
                webRequest.Timeout = timeout;

                if (method == "POST")
                {
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    byte[] postData = encoding.GetBytes(data);
                    webRequest.ContentLength = postData.Length;
                    postStream = webRequest.GetRequestStream();
                    postStream.Write(postData, 0, postData.Length);
                }
                //响应
                webResponse = webRequest.GetResponse();
                streamReader = new StreamReader(webResponse.GetResponseStream(), encoding);
                res = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                {
                    if (response != null)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            res = new StreamReader(responseStream).ReadToEnd();
                            res = ":(" + res;
                        }
                    }
                    else
                    {
                        res = ":(我猜是连接超时";
                    }
                }
            }
            catch (Exception ex)
            {
                res = ":("+ex.Message;
            }
            finally
            {
                if (postStream != null)
                {
                    postStream.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }

            return res;
        }


        public static string Upload(string url, byte[] fileDatas, string fileName, NameValueCollection postParam)
        {
            string result = string.Empty;

            HttpWebRequest requestToServer = (HttpWebRequest)WebRequest.Create(url);

            // Define a boundary string
            string boundaryString = RondamBoundaryText(); //"----SomeRandomText"; 

            // Turn off the buffering of data to be written, to prevent
            // OutOfMemoryException when sending data
            requestToServer.AllowWriteStreamBuffering = false;//false适合传送大数据，但此时必须设置ContentLength
            // Specify that request is a HTTP post
            requestToServer.Method = WebRequestMethods.Http.Post;
            // Specify that the content type is a multipart request
            requestToServer.ContentType
                = "multipart/form-data; boundary=" + boundaryString;
            // Turn off keep alive
            requestToServer.KeepAlive = false;


            ASCIIEncoding ascii = new ASCIIEncoding();
            UTF8Encoding utf8 = new UTF8Encoding();

            string boundaryStringLine = "\r\n--" + boundaryString + "\r\n";
            byte[] boundaryStringLineBytes = ascii.GetBytes(boundaryStringLine);

            string lastBoundaryStringLine = "\r\n--" + boundaryString + "--\r\n";
            byte[] lastBoundaryStringLineBytes = ascii.GetBytes(lastBoundaryStringLine);

            long totalRequestBodySize = 0;
            List<byte[]> postDatas = new List<byte[]>();

            //byte[] myFileDescriptionContentDispositionBytes = ConvertPostData("myFileDescription", "A sample file description");
            //totalRequestBodySize += boundaryStringLineBytes.Length + myFileDescriptionContentDispositionBytes.Length;
            //postDatas.Add(boundaryStringLineBytes);
            //postDatas.Add(myFileDescriptionContentDispositionBytes);

            if (postParam != null)
            {
                foreach (string key in postParam.Keys)
                {
                    var tempData = ConvertPostData(key, postParam[key]);
                    totalRequestBodySize += boundaryStringLineBytes.Length + tempData.Length;
                    postDatas.Add(boundaryStringLineBytes);
                    postDatas.Add(tempData);
                }
            }

            // Get the byte array of the string part of the myFile content
            // disposition
            string myFileContentDisposition = String.Format(
                "Content-Disposition: form-data;name=\"{0}\"; "
                 + "filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n",
                "userfile", Path.GetFileName(fileName), Path.GetExtension(fileName));
            byte[] myFileContentDispositionBytes =
                utf8.GetBytes(myFileContentDisposition);

            postDatas.Add(boundaryStringLineBytes);
            postDatas.Add(myFileContentDispositionBytes);



            //FileInfo fileInfo = new FileInfo(fileUrl);
            totalRequestBodySize += boundaryStringLineBytes.Length + myFileContentDispositionBytes.Length;
            totalRequestBodySize += fileDatas.Length;
            totalRequestBodySize += lastBoundaryStringLineBytes.Length;

            // And indicate the value as the HTTP request content length
            requestToServer.ContentLength = totalRequestBodySize;



            #region
            // Write the http request body directly to the server
            using (Stream s = requestToServer.GetRequestStream())
            {
                foreach (var item in postDatas)
                {
                    s.Write(item, 0, item.Length);
                }

                // Send the file binaries over to the server, in 1024 bytes chunk
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                int residueDataLength = fileDatas.Length;
                while (residueDataLength / buffer.Length >= 1)
                {

                    Array.Copy(fileDatas, bytesRead, buffer, 0, buffer.Length);
                    s.Write(buffer, 0, buffer.Length);
                    bytesRead += buffer.Length;
                    residueDataLength -= buffer.Length;
                } // end while
                if (residueDataLength > 0)
                {
                    s.Write(fileDatas, bytesRead, residueDataLength);
                }


                // Send the last part of the HTTP request body
                s.Write(lastBoundaryStringLineBytes, 0, lastBoundaryStringLineBytes.Length);
            } // end using
            #endregion

            WebResponse wresp = null;
            try
            {
                wresp = requestToServer.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                result = reader2.ReadToEnd();
            }
            catch (Exception)
            {
                //TODO:异常处理
                if (wresp != null)
                {
                    wresp.Close();
                }
            }
            return result;

        }

        private static string RondamBoundaryText()
        {
            //键	值boundary=---------------------------7dec4363065c

            return string.Format("---------------------------{0}", Guid.NewGuid().ToString("N").Substring(0, 12));
        }
        private static string FormDataTemplete = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

        private static byte[] ConvertPostData(string key, object value)
        {
            string formitem = String.Format(FormDataTemplete, key, value);
            return System.Text.Encoding.UTF8.GetBytes(formitem);
        }

    }
}
