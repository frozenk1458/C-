﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.IO;

namespace DefaultNamespace
{
     class MainClass
	{
     public static Socket Send(string c, string ipAddress)
     {
         if(String.Compare(c," ") == 0)
            Console.WriteLine("\nYou can enter a command line. Please read the command line list file.");
            //Ask the user to enter a text line
            string rline;
            rline = Console.ReadLine();
            //Put at the end of the line entered by the user ";" which is the caracter end of the buffer
            rline = rline + ";";
            byte[] chaine = System.Text.Encoding.UTF8.GetBytes(rline);
        
            //Initialize the socket
            Socket ClientServer = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            //Connect to the server on listened address and port.
            ClientServer.Connect(ipAddress,3000);
            //Send text. One letter at a time. It would be improved in various ways.
            ClientServer.Send(chaine, SocketFlags.None);
            rline = " ";
            return ClientServer;
     }
     public static string Receive(string c, Socket w)
     {
         while(true)
            {   
                //Reception buffer
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(" ");
                w.Receive(bytes);
                string t = System.Text.Encoding.UTF8.GetString(bytes);
                //; is the end character
                if(String.Compare(t,";") == 0){ break;}
                //Rebuild the text sent
                c = c + t;
            }
            return c;
     }
     public static void Display(string c)
     {
            //Display the result sent by the server
            Console.WriteLine("Result : {0}",c);
            if(String.Compare(c," Connection NOK") ==0)
            {
                Console.Clear();
                System.Environment.Exit(1);
            }
     }
     
	 public static void Main(string[] args)
	 {
        //Get the current host name from the system
        String strHostName = string.Empty;
        strHostName = Dns.GetHostName();
        IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
        //Get a list of IP addresses [addr variable]. The index 0 of addr is the 1st interface and the index 1 is the second interface and so on.
        //You can see the interface details with the ipconfig command line in a cmd window.
        //Select the interface you want to connect through and enter the index matching with this interface in the below line "string ipAddress = addr[index].ToString();"
        IPAddress[] addr = ipEntry.AddressList; 
        string ipAddress = addr[0].ToString();
        Console.WriteLine("Welcome !!!");
        Thread.Sleep(1000);
        Console.Clear();
        string c = " ";
        //Infinite client loop
        while(true)
        {
            c=" ";
            Socket w = Send(c,ipAddress);
            c = Receive(c,w);
            Display(c);
            //Close the socket connection
            w.Close();
        }
	 }
    }
}
