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
        //Get the current host name from the system
        String strHostName = string.Empty;
        strHostName = Dns.GetHostName();
        IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
        //Get a list of IP addresses. Currently I have 2 IP interfaces so I have 2 IP addresses.
        //Here I have chosen to use the second. The index 0 of IP address tab is the 1st interface and the index 1 is the second interface and so on.
        //You can see the interface details with the ipconfig command line in a cmd windows
        IPAddress[] addr = ipEntry.AddressList; 
        string ipAddress = addr[0].ToString();
        
        //Ask the user to enter a text line
        string rline;
        rline = Console.ReadLine();
        //Put at the end of the line entered by the user ";" which is the caracter end of the buffer
        rline = rline + ";";
        byte[] chaine = System.Text.Encoding.UTF8.GetBytes(rline);
        
        //Initialize the socket
        Socket ClientServer = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //Connect to the server on listened address and port.
        ClientServer.Connect(ipAddress,8000);
        //Send text. One letter at a time. It would be improved in various ways.
        ClientServer.Send(chaine, SocketFlags.None);
        
        ClientServer.Close();
	 }
    }
}
