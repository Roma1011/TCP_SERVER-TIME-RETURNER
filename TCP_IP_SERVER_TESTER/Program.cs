using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
			_serverSocket.BeginAccept(AcceptCallBack, null);
			Console.WriteLine("Server setup complete");
		}
		//Accept connections
		private static void AcceptCallBack(IAsyncResult ar)
		{
			Socket socket;

			try
			{
				socket = _serverSocket.EndAccept(ar);
			}
			catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
			{
				return;
			}

			_clientsocket.Add(socket);
			socket.BeginReceive(_buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
			Console.WriteLine("Client connected, waiting for request...");
			_serverSocket.BeginAccept(AcceptCallBack, null);
		}
		private static void ReceiveCallback(IAsyncResult ar)
		{
			Socket current = (Socket)ar.AsyncState;
			int received;

			try
			{
				received = current.EndReceive(ar);
			}
			catch (SocketException)
			{
				Console.WriteLine("Client forcefully disconnected");
				// Don't shutdown because the socket may be disposed and its disconnected anyway.
				current.Close();
				_clientsocket.Remove(current);
				return;
			}
			byte[] recBuf = new byte[received];
			Array.Copy(_buffer, recBuf, received);
			string text = Encoding.ASCII.GetString(recBuf);
			Console.WriteLine("Received Text: " + text);

			if (text.ToLower() == "get time") // Client requested time
			{
				SendTimeforClient(current);
			}
			else if(text.ToLower() == "exit")// Client wants to exit
			{
				Exit(current);
			}
		}
		private static void SendTimeforClient(Socket CurSocket)
		{
			Console.WriteLine("Text is a get time request");
			byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
			CurSocket.Send(data);
			Console.WriteLine("Time sent to client");
		}
		private static void Exit(Socket CurSocket)
		{
			// Always Shutdown before closing
			CurSocket.Shutdown(SocketShutdown.Both);
			CurSocket.Close();
			_clientsocket.Remove(CurSocket);
			Console.WriteLine("Client disconnected");
			return;
		}
	}
}
