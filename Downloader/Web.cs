﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Downloader
{
    public class Web
    {
        HTTPGet GET = new HTTPGet();
        public string HttpGet(string url)
        {
            GET.Request(url);
            return GET.EscapedBody;
        }
    }

    public class HTTPGet
    {
        private HttpWebRequest request;
        private HttpWebResponse response;

        private string responseBody;
        private string escapedBody;
        private int statusCode;
        private double responseTime;

        public string ResponseBody { get { return responseBody; } }
        public string EscapedBody { get { return GetEscapedBody(); } }
        public int StatusCode { get { return statusCode; } }
        public double ResponseTime { get { return responseTime; } }
        public string Headers { get { return GetHeaders(); } }
        public string StatusLine { get { return GetStatusLine(); } }

        public void Request(string url)
        {
            Stopwatch timer = new Stopwatch();
            StringBuilder respBody = new StringBuilder();

            this.request = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                timer.Start();
                this.response = (HttpWebResponse)this.request.GetResponse();
                byte[] buf = new byte[8192];
                Stream respStream = this.response.GetResponseStream();
                int count = 0;
                do
                {
                    count = respStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                        respBody.Append(Encoding.ASCII.GetString(buf, 0, count));
                }
                while (count > 0);
                timer.Stop();

                this.responseBody = respBody.ToString();
                this.statusCode = (int)(HttpStatusCode)this.response.StatusCode;
                this.responseTime = timer.ElapsedMilliseconds / 1000.0;
            }
            catch (WebException ex)
            {
                this.response = (HttpWebResponse)ex.Response;
                this.responseBody = "No Server Response";
                this.escapedBody = "No Server Response";
                this.responseTime = 0.0;
            }
        }

        private string GetEscapedBody()
        {  // HTML escaped chars
            string escapedBody = responseBody;
            escapedBody = escapedBody.Replace("&", "&");
            escapedBody = escapedBody.Replace("<", "<");
            escapedBody = escapedBody.Replace(">", ">");
            escapedBody = escapedBody.Replace("'", "'");
            escapedBody = escapedBody.Replace("\"", "\"");
            this.escapedBody = escapedBody;

            return escapedBody;
        }

        private string GetHeaders()
        {
            if (response == null)
                return "No Server Response";
            else
            {
                StringBuilder headers = new StringBuilder();
                for (int i = 0; i < this.response.Headers.Count; ++i)
                    headers.Append(String.Format("{0}: {1}\n",
                        response.Headers.Keys[i], response.Headers[i]));

                return headers.ToString();
            }
        }

        private string GetStatusLine()
        {
            if (response == null)
                return "No Server Response";
            else
                return String.Format("HTTP/{0} {1} {2}", response.ProtocolVersion,
                    (int)response.StatusCode, response.StatusDescription);
        }
    }

}
