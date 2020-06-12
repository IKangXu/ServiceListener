
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace project_install.Common
{
    class Server
    {
        public bool running = false;

        private int timeout = 8;
        private Encoding charEncoder = Encoding.UTF8;
        private Socket serverSocket;
        private string contentPath;

        private Dictionary<string, string> extensions = new Dictionary<string, string>()
        { 
            //{ "extension", "content type" }
            { "html", "text/html" },
            { "htm", "text/html" },
            { "shtml", "text/html" },
            { "css", "text/css" },
            { "xml", "text/xml" },
            { "gif", "image/gif" },
            { "jpeg", "image/jpeg" },
            { "jpg", "image/jpeg" },
            { "js", "application/javascript" },
            { "atom", "application/atom+xml" },
            { "rss", "application/rss+xml" },
            { "mml", "text/mathml" },
            { "txt", "text/plain" },
            { "jad", "text/vnd.sun.j2me.app-descriptor" },
            { "wml", "text/vnd.wap.wml" },
            { "htc", "text/x-component" },
            { "png", "image/png" },
            { "svg", "image/svg+xml" },
            { "svgz", "image/svg+xml" },
            { "tif", "image/tiff" },
            { "tiff", "image/tiff" },
            { "wbmp", "image/vnd.wap.wbmp" },
            { "webp", "image/webp" },
            { "ico", "image/x-icon" },
            { "jng", "image/x-jng" },
            { "bmp", "image/x-ms-bmp" },
            { "woff", "font/woff" },
            { "woff2", "font/woff2" },
            { "jar", "application/java-archive" },
            { "war", "application/java-archive" },
            { "ear", "application/java-archive" },
            { "json", "application/json" },
            { "hqx", "application/mac-binhex40" },
            { "doc", "application/msword" },
            { "pdf", "application/pdf" },
            { "ps", "application/postscript" },
            { "eps", "application/postscript" },
            { "ai", "application/postscript" },
            { "rtf", "application/rtf" },
            { "m3u8", "application/vnd.apple.mpegurl" },
            { "kml", "application/vnd.google-earth.kml+xml" },
            { "kmz", "application/vnd.google-earth.kmz" },
            { "xls", "application/vnd.ms-excel" },
            { "eot", "application/vnd.ms-fontobject" },
            { "ppt", "application/vnd.ms-powerpoint" },
            { "odg", "application/vnd.oasis.opendocument.graphics" },
            { "odp", "application/vnd.oasis.opendocument.presentation" },
            { "ods", "application/vnd.oasis.opendocument.spreadsheet" },
            { "odt", "application/vnd.oasis.opendocument.text" },
            { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { "wmlc", "application/vnd.wap.wmlc" },
            { "7z", "application/x-7z-compressed" },
            { "cco", "application/x-cocoa" },
            { "jardiff", "application/x-java-archive-diff" },
            { "jnlp", "application/x-java-jnlp-file" },
            { "run", "application/x-makeself" },
            { "pl", "application/x-perl" },
            { "pm", "application/x-perl" },
            { "prc", "application/x-pilot" },
            { "pdb", "application/x-pilot" },
            { "rar", "application/x-rar-compressed" },
            { "rpm", "application/x-redhat-package-manager" },
            { "sea", "application/x-sea" },
            { "swf", "application/x-shockwave-flash" },
            { "sit", "application/x-stuffit" },
            { "tcl", "application/x-tcl" },
            { "tk", "application/x-tcl" },
            { "der", "application/x-x509-ca-cert" },
            { "pem", "application/x-x509-ca-cert" },
            { "crt", "application/x-x509-ca-cert" },
            { "xpi", "application/x-xpinstall" },
            { "xhtml", "application/xhtml+xml" },
            { "xspf", "application/xspf+xml" },
            { "zip", "application/zip" },
            { "bin", "application/octet-stream" },
            { "exe", "application/octet-stream" },
            { "dll", "application/octet-stream" },
            { "deb", "application/octet-stream" },
            { "dmg", "application/octet-stream" },
            { "iso", "application/octet-stream" },
            { "img", "application/octet-stream" },
            { "msi", "application/octet-stream" },
            { "msp", "application/octet-stream" },
            { "msm", "application/octet-stream" },
            { "mid", "audio/midi" },
            { "midi", "audio/midi" },
            { "kar", "audio/midi" },
            { "mp3", "audio/mpeg" },
            { "ogg", "audio/ogg" },
            { "m4a", "audio/x-m4a" },
            { "ra", "audio/x-realaudio" },
            { "3gpp", "video/3gpp" },
            { "3gp", "video/3gpp" },
            { "ts", "video/mp2t" },
            { "mp4", "video/mp4" },
            { "mpeg", "video/mpeg" },
            { "mpg", "video/mpeg" },
            { "mov", "video/quicktime" },
            { "webm", "video/webm" },
            { "flv", "video/x-flv" },
            { "m4v", "video/x-m4v" },
            { "mng", "video/x-mng" },
            { "asx", "video/x-ms-asf" },
            { "asf", "video/x-ms-asf" },
            { "wmv", "video/x-ms-wmv" },
            { "avi", "video/x-msvideo" }
        };

        public bool Start(IPAddress ipAddress, int port, int maxNOfCon, string contentPath)
        {
            if (running) return false;

            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(ipAddress, port));
                serverSocket.Listen(maxNOfCon);
                serverSocket.ReceiveTimeout = timeout;
                serverSocket.SendTimeout = timeout;
                running = true;
                this.contentPath = contentPath;
            }
            catch { return false; }

            Thread requestListenerT = new Thread(() =>
            {
                while (running)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = serverSocket.Accept();
                        Thread requestHandler = new Thread(() =>
                        {
                            clientSocket.ReceiveTimeout = timeout;
                            clientSocket.SendTimeout = timeout;
                            try { HandleTheRequest(clientSocket); }
                            catch
                            {
                                try { clientSocket.Close(); } catch { }
                            }
                        });
                        requestHandler.Start();
                    }
                    catch { }
                }
            });
            requestListenerT.Start();

            return true;
        }

        public void Stop()
        {
            if (running)
            {
                running = false;
                try { serverSocket.Close(); }
                catch { }
                serverSocket = null;
            }
        }

        private void HandleTheRequest(Socket clientSocket)
        {
            byte[] buffer = new byte[10240];
            int receivedBCount = clientSocket.Receive(buffer);
            string strReceived = charEncoder.GetString(buffer, 0, receivedBCount);

            // Parse the method of the request
            string httpMethod = strReceived.Substring(0, strReceived.IndexOf(" "));

            int start = strReceived.IndexOf(httpMethod) + httpMethod.Length + 1;
            int length = strReceived.LastIndexOf("HTTP") - start - 1;
            string requestedUrl = strReceived.Substring(start, length);

            string requestedFile;
            if (httpMethod.Equals("GET") || httpMethod.Equals("POST"))
                requestedFile = requestedUrl.Split('?')[0];
            else
            {
                NotImplemented(clientSocket);
                return;
            }

            requestedFile = HttpUtility.UrlDecode(requestedFile.Replace("/", "\\").Replace("\\..", ""), charEncoder);// requestedFile.Replace("/", "\\").Replace("\\..", "");
            start = requestedFile.LastIndexOf('.') + 1;
            if (start > 0)
            {
                length = requestedFile.Length - start;
                string extension = requestedFile.Substring(start, length);
                if (extensions.ContainsKey(extension))
                {
                    if (File.Exists(contentPath + requestedFile))
                    { SendOkResponse(clientSocket, File.ReadAllBytes(contentPath + requestedFile), extensions[extension]); }
                    else
                    { NotFound(clientSocket); }
                }
                else
                {
                    if (File.Exists(contentPath + requestedFile))
                    { SendOkResponse(clientSocket, File.ReadAllBytes(contentPath + requestedFile), "application/octet-stream"); }
                    else
                    { NotFound(clientSocket); }
                }
            }
            else
            {
                if (requestedFile.Substring(length - 1, 1) != "\\")
                { requestedFile += "\\"; }
                if (File.Exists(contentPath + requestedFile + "index.htm"))
                { SendOkResponse(clientSocket, File.ReadAllBytes(contentPath + requestedFile + "\\index.htm"), "text/html"); }
                else if (File.Exists(contentPath + requestedFile + "index.html"))
                { SendOkResponse(clientSocket, File.ReadAllBytes(contentPath + requestedFile + "\\index.html"), "text/html"); }
                else
                { NotFound(clientSocket); }
            }
        }

        private void NotImplemented(Socket clientSocket)
        {
            SendResponse(clientSocket, "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body><h2>Atasoy Simple Web Server</h2><div>501 - Method Not Implemented</div></body></html>", "501 Not Implemented", "text/html");
        }

        private void NotFound(Socket clientSocket)
        {
            SendResponse(clientSocket, "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body><h2>Atasoy Simple Web Server</h2><div>404 - Not Found</div></body></html>", "404 Not Found", "text/html");
        }

        private void SendOkResponse(Socket clientSocket, byte[] bContent, string contentType)
        {
            SendResponse(clientSocket, bContent, "200 OK", contentType);
        }
        private void SendResponse(Socket clientSocket, string strContent, string responseCode, string contentType)
        {
            byte[] bContent = charEncoder.GetBytes(strContent);
            SendResponse(clientSocket, bContent, responseCode, contentType);
        }

        private void SendResponse(Socket clientSocket, byte[] bContent, string responseCode, string contentType)
        {
            try
            {
                byte[] bHeader = charEncoder.GetBytes(
                                    "HTTP/1.1 " + responseCode + "\r\n"
                                  + "Server: Atasoy Simple Web Server\r\n"
                                  + "Content-Length: " + bContent.Length.ToString() + "\r\n"
                                  + "Connection: close\r\n"
                                  + "Content-Type: " + contentType + "\r\n\r\n");
                clientSocket.Send(bHeader);
                clientSocket.Send(bContent);
                clientSocket.Close();
            }
            catch { }
        }
    }
}
