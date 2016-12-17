using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.IO;
using System.Globalization;

namespace QuickSharp
{
    public class Program
    {
        private static void Main()
        {
            //Initialize the IP address and port manually for now. I replace in this code the IP address text chain by ServerAddress. The program could be configured to know automatically the machine ip address.
            string ipAddress = ServerAddress;
            int port = 8000;
            //Initialize the buffer for TCP connection
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(" ");
            //Log sentences to write down into the console the connection address and port
            Console.WriteLine("IP="+ipAddress + ":" + port);
            //Socket initialization
            Socket S = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            Socket list = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            //Binding the socket to the IP address and port previously defined
            IPAddress ipClient;
            IPAddress.TryParse(ipAddress, out ipClient);
            IPEndPoint ipe = new IPEndPoint(ipClient, port);
            S.Bind(ipe);
            //Setup the socket to listen on port defined before. Initialize the length of queue at 10.
            S.Listen(10);
            //Loop to accept clients connections (simultaneous connections is possible). Exchange with the client and then close the client connection.
            while(true)
            {
                list=S.Accept();
                //Initialize the reception buffer
                string c = " ";
                while(true)
                {
                    //Read the buffer
                    list.Receive(bytes);
                    //Text received chararcter by character. In the future we plan to try an exchange of other orders.
                    string t = System.Text.Encoding.UTF8.GetString(bytes);
                    //We put an character '0' on the client side to define a text end. If we receive a '0' character we stop reading the buffer.
                    //This part would be improved.
                    if(String.Compare(t,"0") == 0){ break;}
                    //Rebuild initial text character by character.
                    c = c + t;
                }
                //Write down the complete text received
                Console.WriteLine(c);
                //Close the client connection
                list.Close();
            }
            //S.Close();
          }
     }
}