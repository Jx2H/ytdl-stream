using System;
using System.IO;
using System.Net;
using System.Collections.Specialized;
// using System.Text;
// using System.Collections.Generic;
using System.Diagnostics;

namespace ytdl_stream
{
    class Program
    {
        HttpListener listener;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Program w = new Program();
            Console.ReadLine();
        }

        public Program()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:424/");
            listener.Start();
            listener.BeginGetContext(OnContext, null);
        }

        void ResSend(HttpListenerResponse res, string m)
        {
            res.ContentType = "Content-Type: text/html; charset=utf-8";
            using (StreamWriter w = new StreamWriter(res.OutputStream))
                w.Write(m);
            res.Close();
        }

        void OnContext(IAsyncResult ar)
        {
            HttpListenerContext client = listener.EndGetContext(ar);
            Console.WriteLine("접속시도");
            listener.BeginGetContext(OnContext, null);
            HttpListenerRequest req = client.Request;
            HttpListenerResponse res = client.Response;
            NameValueCollection query = req.QueryString;

            System.Console.WriteLine(req.UserAgent);

            if (query["q"] == null)
            {
                ResSend(res, "Youtube Audio Stream Converter\nPowered by .NET Core\n\n q 매개변수를 찾을 수 없습니다.");
                return;
            }
            if (query["q"] == "")
            {
                ResSend(res, "값이 유효하지 않습니다.");
                return;
            }

            var a = Process.Start(new ProcessStartInfo {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = "cmd",
                Arguments = $"/c youtube-dl -f bestaudio --encoding UTF8 --audio-format webm -o - https://www.youtube.com/watch?v={query["q"]}"
            });
            a.WaitForExit(3000);
            MemoryStream ms = new MemoryStream();
            a.StandardOutput.BaseStream.CopyTo(ms);
            Console.WriteLine(ms.Length.ToString());
            a.Kill();

            res.ContentType = "audio/webm";
            res.ContentLength64 = ms.Length;
            res.StatusCode = 200;
            res.AddHeader("Content-disposition", $"attachment; filename={query["q"]}.webm");
            res.AddHeader("Server", "");

            try
            {
                byte[] fe = ms.ToArray();
                res.OutputStream.Write(fe, 0, fe.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                res.StatusCode = 410;
                ResSend(res, "어.... 있었는데요, 없었습니다. \n\n"+ex.ToString());
                return;
            }
        }
    }
}
