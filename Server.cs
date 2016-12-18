using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.IO;
using System.Globalization;

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
                    //We put an character ";" on the client side to define a text end. If we receive a ";" character we stop reading the buffer.
                    //This part would be improved.
                    if(String.Compare(t,";") == 0){ break;}
                    //Rebuild initial text character by character.
                    c = c + t;
                }
                //Calculation operations.
                //Delete space character of c
                string CSpc = c.Replace(" ","");
                string Wint = " ";
                int operat = -6;
                //At the end of the loop we have the same text without letter.
                foreach(char u in CSpc)
                {
                    //We determine the operation that the user want to do
                    string lulu = u.ToString();
                    if(String.Compare(lulu,"+") == 0)
                    {
                        operat=1;
                    }
                    if(String.Compare(lulu,"-") == 0)
                    {
                        operat=2;
                    }
                    if(String.Compare(lulu,"*") == 0)
                    {
                        operat=3;
                    }
                    if(String.Compare(lulu,"/") == 0)
                    {
                        operat=4;
                    }
                    if(!Char.IsLetter(u))
                    {
                        Wint=Wint+u;
                    }
                }
                //Divide Wint into two pieces with operation character for delimiter.
                //For now we develop the addition code but we will develop more operations with more modularity.
                //The modularity will help to have a better vision of the code and simpler code.
                string[] v = { " " };
                if (operat==1)
                {
                    v = Wint.Split('+');
                }
                if (operat==2)
                {
                    v = Wint.Split('-');
                }
                if (operat==3)
                {
                    v = Wint.Split('*');
                }
                if (operat==4)
                {
                    v = Wint.Split('/');
                }
                string first = v[0];
                string second = v[1];
                //Convert text received to int and calculations
                int res = 0;
                if (operat==1)
                {
                    res = Convert.ToInt32(first) + Convert.ToInt32(second);
                }
                if (operat==2)
                {
                    res = Convert.ToInt32(first) - Convert.ToInt32(second);
                }
                if (operat==3)
                {
                    res = Convert.ToInt32(first) * Convert.ToInt32(second);
                }
                if (operat==4)
                {
                    res = Convert.ToInt32(first) / Convert.ToInt32(second);
                }
                //Prepare the sending of the result. Convert result from int to string to send it
                string r = res.ToString();
                //Add the end character for result transmission through the socket
                r = r + ";";
                Console.WriteLine("Sending the result.");
                //Convert the result to the buffer format
                byte[] res1 = System.Text.Encoding.UTF8.GetBytes(r);
                //Send the result
                list.Send(res1, SocketFlags.None);
                Console.WriteLine("Result sent");
                //Close the client connection
                list.Close();
            }
          }
     }
}
