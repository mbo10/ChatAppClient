using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ChatAppClient
{
    class Client
    {
        static void Main(string[] args)
        {
            byte[] serverMessage = new byte[1024]; 

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.5"), 8080);

            Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                connectionSocket.Connect(endpoint);
            }
            catch (SocketException exception)
            {
                Console.WriteLine("Exception thrown: Connection failed.");

                Console.WriteLine(exception.ToString());

                return;
            }

            int msgReceiver = connectionSocket.Receive(serverMessage);

            string serverMsgString = Encoding.UTF8.GetString(serverMessage, 0, msgReceiver);

            Console.WriteLine(serverMsgString);

            while (true)
            {
                string clientMessage = Console.ReadLine();

                Console.SetCursorPosition(0, Console.CursorTop - 1);

                if (clientMessage == "goodbye")

                    break;

                Console.WriteLine("Client(You): " + clientMessage);

                connectionSocket.Send(Encoding.UTF8.GetBytes(clientMessage));

                msgReceiver = connectionSocket.Receive(serverMessage);

                serverMsgString = Encoding.UTF8.GetString(serverMessage, 0, msgReceiver);

                Console.WriteLine("Server: " + serverMsgString);
            }

            Console.WriteLine("Exiting server...");

            connectionSocket.Shutdown(SocketShutdown.Both);

            connectionSocket.Close();

            Console.WriteLine("Connection Closed!");
        }
    }
}
