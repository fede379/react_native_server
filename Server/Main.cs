using System;
using System.Diagnostics;
using Microsoft.ReactNative.Managed;

namespace publicala_desktop.Server
{
    [ReactModule("Server")]
    class Main
    {
        [ReactMethod("start")]
        public void StartServer()
        {
            Debug.WriteLine("Starting server in port 8088");
            HTTPServer server = new HTTPServer(8088);

            server.Start();
        }
    }
}
