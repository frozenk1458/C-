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
        public static string connexion(string s)
        {
            if(String.Compare(s," login;") == 0 )
            {
                Console.WriteLine("Login OK");
                return "Connection OK;";
            }
            else
            {
                Console.WriteLine("Login NOK");
                return "Connection NOK;";
            }
        }
        public static string calculation(string s)
        {//Function to calculate
                //Delete space character of c and build a string CSpc
                string CSpc = s.Replace(" ","");
                CSpc = CSpc +";";
                //Variable to store result
                int res = 0;
                //It is a variable to store the operand and its sign
                string intermediate = " ";
                //lulu and operat are test variables to define if the current character of the loop is the operator + or - or * or /
                string lulu = " ";
                string operat = " ";
                //The calculation loop
                foreach(char u in CSpc)
                {
                    Console.WriteLine("Intermediate :{0}, current : {1}",intermediate,u);
                    //If the current character is a number we had it to intermediate to solve the problem of an operand with several number
                    if(Char.IsNumber(u))
                    {
                        intermediate = intermediate + Char.ToString(u);
                    }
                    if(!Char.IsNumber(u))
                    {
                        lulu = u.ToString();
                        //Case of the + operator
                        if(String.Compare(lulu,"+") == 0)
                        {
                            //Case of an addition. Then we set again the variable intermediate to a simple space for the next loop
                            if(String.Compare(intermediate," ") != 0)
                            {
                                res = res + Convert.ToInt32(intermediate);
                                operat = "+";
                                intermediate = " ";
                            }
                            //If we are in the begin of the calculation, there is nothing store. So the first character if it is not a number is likely an + or - sign
                            //The case below is the + sign. We build the first operand with the sign met.
                            if(String.Compare(intermediate," ") == 0)
                            intermediate = intermediate + "+";
                        }
                        if(String.Compare(lulu,"-") == 0)
                        {
                            //Case of a subtraction. Then we set again the variable intermediate to a simple space for the next loop
                            if(String.Compare(intermediate," ") != 0)
                            {
                                res = res - Convert.ToInt32(intermediate);
                                 operat = "-";
                                 intermediate = " ";
                            }
                            //If we are in the begin of the calculation, there is nothing store. So the first character if it is not a number is likely an + or - sign
                            //The case below is the - sign. We build the first operand with the sign met.
                            if(String.Compare(intermediate," ") == 0)
                            intermediate = intermediate + "-";
                        }
                        if(String.Compare(lulu,"*") == 0)
                        {
                            //Case of a multiplication. Then we set again the variable intermediate to a simple space for the next loop
                            if(String.Compare(intermediate," ") != 0)
                            {
                                res = res * Convert.ToInt32(intermediate);
                                operat = "*";
                            }
                            //Set the variable intermediate set to a simple space for the next loop
                            intermediate = " ";
                        }
                        if(String.Compare(lulu,"/") == 0)
                        {
                            //Case of a division. Then we set again the variable intermediate a simple space for the next loop
                            if(String.Compare(intermediate," ") != 0)
                            {
                                res = res / Convert.ToInt32(intermediate);
                                operat = "/";
                            }
                            //Set the variable intermediate set to a simple space for the next loop
                            intermediate = " ";
                        }
                    }
                    //Case where we arrive at the end of the calculation symbolized by ";"
                    if(String.Compare(Char.ToString(u),";") == 0)
                    {
                        //In this case we take the last operator and we apply it to the last number of the calculation
                       if(String.Compare(operat,"+") == 0)
                       {
                        //Case of an addition   
                        res = res + Convert.ToInt32(intermediate);
                        //We exit the loop
                        break;
                       }
                       if(String.Compare(operat,"-") == 0)
                       {
                        //Case of a subtraction
                        res = res - Convert.ToInt32(intermediate);
                        //We exit the loop
                       break;
                        }
                       if(String.Compare(operat,"*") == 0)
                       {
                        //Case of a multiplication
                        res = res * Convert.ToInt32(intermediate);
                        //We exit the loop
                        break;
                       }
                       if(String.Compare(operat,"/") == 0)
                       {
                        //Case of a division
                        res = res / Convert.ToInt32(intermediate);
                        //We exit the loop
                        break;
                       }
                    }
                }
                //Prepare the sending of the result. Convert result from int to string to send it
                string r = res.ToString();
                //Add the end character for result transmission through the socket
                r = r + ";";
                Console.WriteLine("Sending the result.");
                return r;
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
            
            int port = 3000;
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
                c = c + ";";
                Console.Clear();
                Console.WriteLine(c);
                if(String.Compare(c," Connect;")==0 || String.Compare(c," login;")==0)
                {
                    //Call the calculation function. Return a string variable and need a string as a parameter.
                    string co = calculation(c);
                    co = connexion(c);
                    Console.WriteLine("Cob : {0}",co);
                    byte[] cob = System.Text.Encoding.UTF8.GetBytes(co);
                    //Send the result
                    list.Send(cob, SocketFlags.None);
                }
                else if(String.Compare(c," Calc;")!=0 || String.Compare(c," Connect;")!=0)
                {
                    //Call the calculation function. Return a string variable and need a string as a parameter.
                    string rCalc = calculation(c);
                    //Convert the result to the buffer format
                    byte[] res1 = System.Text.Encoding.UTF8.GetBytes(rCalc);
                    Console.WriteLine(rCalc);
                    //Send the result
                    list.Send(res1, SocketFlags.None);
                    Console.WriteLine("Result sent");
                }
                //Close the socket connection
                list.Close();
            }
          }
     }
}
