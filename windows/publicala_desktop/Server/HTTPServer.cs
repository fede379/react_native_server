using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace publicala_desktop.Server
{
    public class HTTPServer
    {
        public const String WWW_DIR = "/Server/www/";
        public const String HTTP_VERSION = "HTTP/1.1";
        public const String NAME = "Publicala Server v1.0";
        private bool running = false;
        private TcpListener listener;

        public HTTPServer(int port)
        {
            if (listener == null)
            {
                listener = new TcpListener(IPAddress.Any, port);
            }
        }

        public void Start()
        {
            if (!running)
            {
                Thread serverThread = new Thread(new ThreadStart(Run));
                serverThread.Start();
            }
        }

        private void Run()
        {
            running = true;
            listener.Start();

            while(running)
            {
                Debug.WriteLine("Waiting for connections...");

                TcpClient tcpClient = listener.AcceptTcpClient();

                Debug.WriteLine("Connection accepted");

                handleClient(tcpClient);

                tcpClient.Close();
            }

            running = false;
            listener.Stop();
        }

        private void handleClient(TcpClient tcpClient)
        {
            StreamReader reader = new StreamReader(tcpClient.GetStream());

            String message = "";
            while (reader.Peek() != -1)
            {
                message += reader.ReadLine() + "\n";
            }

            Debug.WriteLine("Request: \n" + message);

            Request req = Request.getRequest(message);
            Response res = Response.From(req);
            res.Post(tcpClient.GetStream());
        }
    }
}
