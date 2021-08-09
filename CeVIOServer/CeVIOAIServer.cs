using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;

namespace CeVIOServer
{
    public class CeVIOAIServer
    {
        public static HttpListener listener;
        public static CeVIODriver cdriver;
        public static int requestCount = 0;


        public static void Start(string prefix, string cast, string userdic_path, string dic_path)
        {
            listener = new HttpListener();
            cdriver = new CeVIODriver(cast, userdic_path, dic_path);
            listener.Prefixes.Add(prefix);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", prefix);
        }

        public static void Main(string prefix, string cast, string userdic_path, string dic_path)
        {
            Start(prefix, cast, userdic_path, dic_path);
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();
            Stop();
        }

        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                try
                {
                    // Will wait here until we hear from a connection
                    HttpListenerContext ctx = await listener.GetContextAsync();

                    // Peel out the requests and response objects
                    HttpListenerRequest req = ctx.Request;
                    HttpListenerResponse resp = ctx.Response;

                    // Print out some info about the request
                    Console.WriteLine("Request #: {0}", ++requestCount);
                    Console.WriteLine(req.Url.ToString());
                    Console.WriteLine(req.HttpMethod);
                    Console.WriteLine(req.UserHostName);
                    Console.WriteLine(req.UserAgent);
                    Console.WriteLine();

                    // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                    if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                    {
                        Console.WriteLine("Shutdown requested");
                        runServer = false;
                    }

                    // メッセージ送信部
                    StreamReader reader = new StreamReader(req.InputStream, Encoding.UTF8);
                    string req_message = reader.ReadToEnd();
                    Console.WriteLine(req_message);

                    resp.StatusCode = (int)HttpStatusCode.OK;
                    string wav_path = cdriver.CreateWAV(req_message);
                    using (FileStream fs = new FileStream(wav_path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        try
                        {
                            long FileSize;
                            FileSize = fs.Length;

                            byte[] Buffer = new byte[(int)FileSize];
                            fs.Read(Buffer, 0, (int)FileSize);
                            resp.AddHeader("Content-Type", "audio/wave");
                            await resp.OutputStream.WriteAsync(Buffer, 0, (int)FileSize);
                        }
                        catch
                        {
                            fs.Close();
                        }

                    }
                    resp.Close();
                }
                catch(Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }


        static void ReturnInternalError(HttpListenerResponse response, Exception cause)
        {
            Console.Error.WriteLine(cause);
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.ContentType = "text/plain";
            try
            {
                using (var writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
                    writer.Write(cause.ToString());
                response.Close();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                response.Abort();
            }
        }

        public static void Stop()
        {
            listener.Stop();
            listener.Close();
        }
    }
}
