using System;
using System.IO;
using System.Net.Sockets;

namespace publicala_desktop.Server
{
    class Response
    {
        private Byte[] Data = null;
        private String Status;
        private String ContentType;

        private Response(String status, String contentType, Byte[] data)
        {
            Status = status;
            ContentType = contentType;
            Data = data;
        }

        public static Response From(Request request)
        {
            if (request == null)
            {
                return MakeBadRequest();
            }

            if (request.Type == "GET")
            {
                String path = Environment.CurrentDirectory + HTTPServer.WWW_DIR + request.URL;
                FileInfo file = new FileInfo(path);
                if (!file.Exists)
                {
                    return MakePageNotFound();
                }
                if (file.Extension.Contains("."))
                {
                    return MakeFromFile(file);
                } else
                {
                    DirectoryInfo dir = new DirectoryInfo(file + "/");
                    if (!dir.Exists)
                    {
                        return MakePageNotFound();
                    }
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo f in files)
                    {
                        String name = f.Name;
                        if(name.Contains("default.htm") || name.Contains("default.html") || name.Contains("index.htm") || name.Contains("index.html"))
                        {
                            return MakeFromFile(f);
                        }
                    }
                }
            } else
            {
                return MakeMethodNotAllowed();
            }
            return MakePageNotFound();
        }

        private static Response MakeFromFile(FileInfo file)
        { 
            FileStream fs = file.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] data = new byte[fs.Length];
            reader.Read(data, 0, data.Length);
            fs.Close();
            return new Response("200 OK", "text/html", data);
        }

        private static Response MakeBadRequest()
        {
            String path = Environment.CurrentDirectory + HTTPServer.WWW_DIR + "400.html";
            FileInfo file = new FileInfo(path);
            FileStream fs = file.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] data = new byte[fs.Length];
            reader.Read(data, 0, data.Length);
            fs.Close();
            return new Response("400 Bad Request", "text/html", data);
        }

        private static Response MakePageNotFound()
        {
            String path = Environment.CurrentDirectory + HTTPServer.WWW_DIR + "404.html";
            FileInfo file = new FileInfo(path);
            FileStream fs = file.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] data = new byte[fs.Length];
            reader.Read(data, 0, data.Length);
            fs.Close();
            return new Response("404 Page Not Found", "text/html", data);
        }

        private static Response MakeMethodNotAllowed()
        {
            String path = Environment.CurrentDirectory + HTTPServer.WWW_DIR + "405.html";
            FileInfo file = new FileInfo(path);
            FileStream fs = file.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] data = new byte[fs.Length];
            reader.Read(data, 0, data.Length);
            fs.Close();
            return new Response("405 Method Not Allowed", "text/html", data);
        }

        public void Post(NetworkStream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(String.Format("{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n", HTTPServer.HTTP_VERSION, Status, HTTPServer.NAME, ContentType, Data.Length));
            writer.Flush();
            stream.Write(Data, 0, Data.Length);
        }
    }
}
