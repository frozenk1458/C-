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
        public static string calculation(string s)
        {//Function to calculate
                //Delete space character of c and build a string CSpc
                string CSpc = s.Replace(" ","");
                //Int to determine the operator to do
                int operat = 0;
                //Variable to store result
                int res = 0;
                //The calculation loop
                foreach(char u in CSpc)
                {
                    //We determine the operation that the user want to do and store the type into the variable lulu
                    string lulu = u.ToString();
                    //If the current character is not a number it would be an operator character so we analyse it
                    if(!Char.IsNumber(u))
                    {   
                        //If the operation is an addition the operator variable is set to 1
                        if(String.Compare(lulu,"+") == 0)
                        {
                            operat = 1;
                        }
                        //If the operation is an subtraction the operator variable is set to 2
                        if(String.Compare(lulu,"-") == 0)
                        {
                            operat = 2;
                        }
                        //If the operation is an multiplication the operator variable is set to 3
                        if(String.Compare(lulu,"*") == 0)
                        {
                            operat = 3;
                        }
                        //If the operation is an division the operator variable is set to 4
                        if(String.Compare(lulu,"/") == 0)
                        {
                            operat = 4;
                        }
                    }
                    //If the current character we apply the current operator to calculate the final result or an intermediate result
                    if(Char.IsNumber(u))
                    {
                        //ri is the variable to store the numeric value (int format) of current character read
                        int ri = (int)Char.GetNumericValue(u);
                        //If the variable operat value is 0, it is because no operator has been detected so we just add to the result
                        if(operat == 0) res = res + ri;
                        //We calculate the intermediate result according to the operator found
                        if(operat == 1)
                        res = res + ri;
                        if(operat == 2)
                        res = res - ri;
                        if(operat == 3)
                        res = res * ri;
                        if(operat == 4)
                        res = res / ri;
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
                //Call the calculation function. Return a string variable and need a string as a parameter.
                string rCalc = calculation(c);
                //Convert the result to the buffer format
                byte[] res1 = System.Text.Encoding.UTF8.GetBytes(rCalc);
                //Send the result
                list.Send(res1, SocketFlags.None);
                Console.WriteLine("Result sent");
                //Close the socket connection
                list.Close();
            }
          }
     }
}
