using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerWinTcp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Process.Start("Client.exe");
        }

        Task task;
        private void button2_Click(object sender, EventArgs e)
        {
            if (task != null)
                return;
            task = Task.Run(() =>
            {
                try
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 11000);
                    TcpListener listener = new TcpListener(endPoint);
                    listener.Start();
                    TcpClient tcpClient = listener.AcceptTcpClient(); 
                    using (NetworkStream stream = tcpClient.GetStream())
                    {
                        byte[] buffer = new byte[111000];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length); 
                        Image image = Image.FromStream(new MemoryStream(buffer, 0, bytesRead));
                        Bitmap bitmap = new Bitmap(image, pictureBox1.ClientSize);
                        pictureBox1.Invoke(new Action(() => pictureBox1.Image = bitmap));
                    }
                    tcpClient.Close();
                    listener.Stop();
                }
                catch (SocketException ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }
            });
            Text = "Server is working";
        }
    }
}