﻿using System;

namespace publicala_desktop.Server
{
    class Request
    {
        public String Type { get; set; }
        public String URL { get; set; }
        public String Host { get; set; }


        private Request(String type, String url, String host)
        {
            Type = type;
            URL = url;
            Host = host;
        }

        public static Request getRequest(String request)
        {
            if (String.IsNullOrEmpty(request))
            {
                return null;
            }

            String[] tokens = request.Split(" ");
            String type = tokens[0];
            String url = tokens[1];
            String host = tokens[4];
            return new Request(type, url, host);
        }
    }
}
