using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.IO;

namespace DefaultNamespace
{
     class MainClass
	{
	 public static void Main(string[] args)
	 {
        //Get the IP address and convert it to string
        String strHostName = string.Empty;
        strHostName = Dns.GetHostName();
        IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
        IPAddress[] addr = ipEntry.AddressList;
        string ipAddress = addr[1].ToString();
        
        //Initialize the sending buffer
        byte[] chaine = System.Text.Encoding.UTF8.GetBytes("Testp0");
        /*Testing part
        string s = System.Text.Encoding.UTF8.GetString(chaine);
        Console.Write(s);
        */
        //Initialize the socket
		Socket ClientServer = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //Connect to the server on listened address and port.I replace in this code the IP address text chain by ServerAddress. The program could be configured to know automatically the machine ip address.
        ClientServer.Connect(ipAddress,8000);
        //Send text. One letter at a time. It would be improved in various ways.
        ClientServer.Send(chaine, SocketFlags.None);

        //ClientServer.Send(chaine);
        //ClientServer.Close();
	 }
    }
}