using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace TCP_IP_SERVER_TESTER
{
	class Program
	{
		//initialize server socket type 
		private static readonly Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		// Client Socket List
		private static readonly List<Socket> _clientsocket = new List<Socket>();
		//recived buffer size
		private const int BUFFER_SIZE = 2048;
		// connection port server and client
		private const int PORT = 5555;
		// recived byte array 
		private static readonly byte[] _buffer = new byte[BUFFER_SIZE];

		static void Main(string[] args)
		{
			
		}
		// Server Working Start here
		private static void SetupServer()
		{
			Console.WriteLine("Setting up server...");
			_serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
			_serverSocket.Listen(5);
		}
	}
}
