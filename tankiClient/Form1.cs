using Syusing System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace tankiServer
{
    public class ServerObject
    {
        int port = 8888;
        static TcpListener tcpListener;
 
        List<ClientObject> clients = new List<ClientObject>();

        protected internal void AddConnection(ClientObject client)
        {
            clients.Add(client);
        }
        protected internal void RemoveConnection(string id)
        {
            ClientObject client = clients.FirstOrDefault(x => x.Id == id);
            if (client != null)
                clients.Remove(client);
        }

        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();

                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Server start!");
                Console.BackgroundColor = ConsoleColor.Black;

                while (true)
                {
                    TcpClient tmp = tcpListener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(tmp, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }
        protected internal void BroadcastMsg(string msg, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(msg);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id)
                    clients[i].Stream.Write(data, 0, data.Length);
            }
        }
        protected internal void Disconnect()
        {
            tcpListener.Stop();
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0);
        }
    }
    public class ClientObject
    {
        public string Id { get; set; }
        public NetworkStream Stream { get; set; }
        string Name;
        TcpClient client;
        private ServerObject server;

        public ClientObject(TcpClient client, ServerObject serverObject)
        {
            this.client = client;
            this.server = serverObject;
            this.Id = Guid.NewGuid().ToString();
            server.AddConnection(this);
        }

        internal void Process()
        {
            try
            {
                Stream = client.GetStream();
                string msg = GetMsg();
                Name = msg;

                msg = Name + " вошел в игру";
                server.BroadcastMsg(msg, this.Id);
                Console.WriteLine(msg);

                while (true)
                {
                    try
                    {
                        msg = GetMsg();
                        Console.WriteLine(Name + ": " + msg);
                        server.BroadcastMsg(msg, this.Id);
                    }
                    catch (Exception ex)
                    {
                        msg = Name + " покинул игру";
                        Console.WriteLine(msg);
                        server.BroadcastMsg(msg, this.Id);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        private string GetMsg()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;

            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (Stream.DataAvailable);

            return builder.ToString();
        }

        internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
    class Program
    {
        static ServerObject server;
        static Thread listenThread;

        static void Main(string[] args)
        {
            try
            {
                server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                server.Disconnect();
            }
        }
    }
}
