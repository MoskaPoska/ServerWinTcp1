using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 11000);
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(endPoint);

                using (NetworkStream stream = tcpClient.GetStream())
                {
                    byte[] sendbuff = ReceiveScreen();
                    stream.Write(sendbuff);
                }

                tcpClient.Close();
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static byte[] ReceiveScreen()
        {
            ScreenCapture screenCapture = new ScreenCapture();
            Image image = screenCapture.CaptureScreen();
            byte[] buff = null;

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg);
                buff = stream.ToArray();
            }

            return buff;
        }
    }
}