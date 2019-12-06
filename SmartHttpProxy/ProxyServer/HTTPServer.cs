using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace SmartHttpProxy
{
    class HTTPServer
    {
        private X509Certificate _certificate = new X509Certificate2(Sol.CetrificatePath);
        
        private IPAddress _listeningIP = 
            IPAddress.Parse(new System.Net.WebClient().DownloadString(Sol.LinkToGetIp));
        
        private const Int16 _bufferSize = 8192;

        private Int32 _listeningPort = Sol.ListeningPort;

        private TcpListener _listener;
        private Thread _listenerThread;

        public void Start () {
            _listener = new TcpListener (_listeningIP, _listeningPort);
            
            Debug.PrintInformation($"Started at - {_listeningIP} - {_listeningPort}");
            _listener.Start(100);
            _listenerThread = new Thread (() => Listen (_listener));
            _listenerThread.Start ();
        }

         public void Stop () {
            //stop listening for incoming connections
            _listener.Stop ();
            //wait for server to finish processing current connections...
            _listenerThread.Abort ();
            _listenerThread.Join ();
        }

        private static void Listen (Object obj) {
            TcpListener listener = (TcpListener) obj;
            try {
                while (true) {
                    var client = listener.AcceptTcpClient();
                    while (!ThreadPool.QueueUserWorkItem (new WaitCallback (ProcessClient), client));
                }
            } catch (ThreadAbortException) { } catch (SocketException) { }
        }

        private static void ProcessHttpRequest(TcpClient client)
        {
            #region streams
            var clientStream = client.GetStream();
            var sslStream = new SslStream(clientStream, false);
            var clientStreamReader = new StreamReader(clientStream);
            #endregion
            var httpCommand = string.Empty;
            if(!IsRequestValid(clientStream, sslStream, clientStreamReader, out httpCommand)) return;

            var splitBuffer = httpCommand.Split(Symbols.SpaceSplit, 3);

            var method = splitBuffer[0];
            var remoteUri = splitBuffer[1];
            var version = new Version(1, 0);

        }

        private static bool IsRequestValid(
            NetworkStream clientStream,
            SslStream sslStream, 
            StreamReader clientStreamReader,
            out string httpCommand
            )
        {
            httpCommand = clientStreamReader.ReadLine();

            if (String.IsNullOrEmpty(httpCommand))
            {
                clientStream.Close();
                sslStream.Close();
                clientStreamReader.Close();

                return false;
            }
            return true;
        }


        private static void ProcessClient (Object obj) {
            Socket client = (Socket) obj;
            try {
                NetworkStream ns = new NetworkStream (client);
                //RECEIVE CLIENT DATA
                byte[] buffer = new byte[_bufferSize];
                int rec = 0, sent = 0, transferred = 0, rport = 0;
                string data = "";
                do {
                    rec = ns.Read (buffer, 0, buffer.Length);
                    data += Encoding.ASCII.GetString (buffer, 0, rec);
                } while (rec == buffer.Length);
                string html = System.Text.Encoding.UTF8.GetString(buffer);
                Console.WriteLine(html);
                //do your processing here
            } catch (Exception ex) {
                //handle exception
            } finally {
                client.Close ();
            }
        }


    }
}
