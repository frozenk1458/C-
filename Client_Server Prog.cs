/*******************************************/
/*******                              ******/
/*******   Author : Frozenk           ******/
/*******                              ******/
/*******   Client/Server              ******/
/*******   Simple text transmission   ******/
/*******************************************/


using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace HelloWorld
{
    public class menu
    {
        /*User interface for navigate through functionnality*/
        public void Menu()
        {
                Console.WriteLine("We are in the menu class");
        }
    }
    
    public class S
    {
        public Socket ConnectSocket(string server, int port)
        {
           /*Declaration*/
           Socket s = null;
           IPHostEntry hostEntry = null;

           // Get host related information.
           hostEntry = Dns.GetHostEntry(server);

        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
        // an exception that occurs when the host IP Address is not compatible with the address family
        // (typical in the IPv6 case).
        foreach(IPAddress address in hostEntry.AddressList)
        {
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket = 
                new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            tempSocket.Connect(ipe);

            if(tempSocket.Connected)
            {
                s = tempSocket;
                break;
            }
            else
            {
                continue;
            }
        }
        return s;
        }
        public String Sock()
        {
            /*Initatialization Socket and variables*/
       
            string host;
            host = Dns.GetHostName();
            int port = 5000;
            string TextSend = "This is a simple test. \r\n\r\n";
            Byte[] bytesSent = Encoding.ASCII.GetBytes(TextSend);
            Byte[] bytesReceived =new Byte[256];
            
            //Create a connection
            Socket s = ConnectSocket(host,port);
            
            //Error message in case the socket is not open
            if (s == null)
                return ("Connection failed");
                
            // Send text to the server
            s.Send(bytesSent,bytesSent.Length, 0);
            
            //Receive the text 
            int bytes = 0;
            string txt = " \r\n";
            
            //The following will block until the text is transmitted
            do
            {
                bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                txt = txt + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
            }
            while (bytes > 0);
            
            return txt;
        }
    }
    
    public class Hello
    {
        private static void Main(string[] args)
        {
            /*Declaration*/
            menu m = new menu();
            /*Display the first page*/
            m.Menu();
            /*Call Socket Initialization*/
            S p = new S();
            p.Sock();
            /*Wait for user action*/
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
