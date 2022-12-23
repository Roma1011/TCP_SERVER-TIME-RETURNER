using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Server;

namespace TCP_IP_SERVER_TESTER
{
	class Program
	{
		static void Main(string[] args)
		{
			ServerData.SetupServer();
			Console.ReadLine();
			ServerData.CloseAllSockets();
		}
	}
}
