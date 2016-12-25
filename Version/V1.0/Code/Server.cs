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
        public static void ConnectToSql()
        {
            System.Data.SqlClient.SqlConnection conn = 
            new System.Data.SqlClient.SqlConnection ();
            //Replace PC_NAME\\INSTANCE_NAME and DB_NAME by what you are using
            conn.ConnectionString = "integrated security=SSPI;data source=PC_NAME\\SQLEXPRESS;" + "persist security info=False;initial catalog=DB_NAME";
            try
            {
                conn.Open();
                Console.WriteLine("You are connected to the DB");
                // Insert code to process data.
            }
            catch (Exception e) 
            {
                Console.WriteLine("Failed to connect to data source: {0}", e.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static void connexion(string c,Socket list,string path, string o)
        {
            int connexion = 0;
            string enterlogin = " ";
            byte[] enterloginb = System.Text.Encoding.UTF8.GetBytes(" ");
            string recep = " ";
            int first = 0;
            byte[] recenterloginb = System.Text.Encoding.UTF8.GetBytes(" ");
            while(connexion != 1)
            {
               if(first == 0)
               {
                   enterlogin = "enterlogin;";
                   enterloginb = System.Text.Encoding.UTF8.GetBytes(enterlogin);
                   list.Send(enterloginb, SocketFlags.None);
                   recenterloginb = System.Text.Encoding.UTF8.GetBytes(" ");
                   first = 1;
               }
               while(true)
               {
                   //Read the buffer
                    list.Receive(recenterloginb);
                    //Text received chararcter by character. In the future we plan to try an exchange of other orders.
                    string t = System.Text.Encoding.UTF8.GetString(recenterloginb);
                    //We put an character ";" on the client side to define a text end. If we receive a ";" character we stop reading the buffer.
                    //This part would be improved.
                    if(String.Compare(t,";") == 0){ break;}
                    //Rebuild initial text character by character.
                    recep = recep + t;
                }
                recep = recep + ";";
                string[] v = recep.Split(':');
                string log = v[0].Replace(" ","");
                string p = v[1].Replace(" ","");
                string ps = v[1].Replace(";","");
                ConnectToSql();
                if(String.Compare(log,"login")==0 && String.Compare(ps,"pass")==0)
                {
                    using (StreamWriter sw = File.AppendText(path)) 
                    {
                        sw.Write(o);
                        sw.WriteLine(" : Log on as " + log);
                    }
                    connexion = 1;
                    enterlogin = "Connexion OK;";
                    enterloginb = System.Text.Encoding.UTF8.GetBytes(enterlogin);
                    list.Send(enterloginb, SocketFlags.None);
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path)) 
                    {
                        sw.Write(o);
                        sw.WriteLine(" : Connexion failed as " + log);
                    }
                    enterlogin = " Connexion NOK;";
                    enterloginb = System.Text.Encoding.UTF8.GetBytes(enterlogin);
                    list.Send(enterloginb, SocketFlags.None);
                    recep =" ";
                }
             }    
        }
        public static string calculation(string s,string path, string o)
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
                    using (StreamWriter sw = File.AppendText(path)) 
                    {
                        sw.Write(o);
                        sw.WriteLine(" : Intermediate :{0}, current : {1}",intermediate,u);
                    }
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
                using (StreamWriter sw = File.AppendText(path)) 
                {
                    sw.Write(o);
                    sw.WriteLine(" : Sending the result.");
                }
                return r;
        }
        public static void Main(string[] args)
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            Console.WriteLine(date);
            string o = DateTime.Now.ToString("HH:mm:ss tt");
            string d = DateTime.Now.ToString("ddMMyyyy");
            string filename = "log_server_" + d + ".txt";
            string p =@".\log";
            try 
            {
                
                // Try to create the directory.
                Directory.CreateDirectory(p);
            } 
            catch (Exception e) 
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            } 
            string path = @".\log\" + filename;
            if (!File.Exists(path)) 
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path)) 
                {
        
                }	
            }
            //Get the current host name from the system
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            //Get a list of IP addresses [addr variable]. The index 0 of addr is the 1st interface and the index 1 is the second interface and so on.
            //You can see the interface details with the ipconfig command line in a cmd window.
            //Select the interface you want to connect through and enter the index matching with this interface in the below line "string ipAddress = addr[index].ToString();"
            IPAddress[] addr = ipEntry.AddressList;
            //string ipAddress = addr[1].ToString();
            string ipAddress = "127.0.0.1";
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
                if(String.Compare(c," Connect;")==0)
                {
                    connexion(c,list,path,o);
                }
                if(String.Compare(c," Calc;")==0)
                {
                    string ask = " ";
                    ask = " ASK;";
                    byte[] askb = System.Text.Encoding.UTF8.GetBytes(ask);
                    list.Send(askb, SocketFlags.None);
                    while(true)
                    {
                        //Initialize the reception buffer
                        c = " ";
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
                        string checkc = c.Replace(" ","");
                        if(String.Compare(checkc,"STOP;") == 0)
                        {
                            break; 
                        }
                        //Call the calculation function. Return a string variable and need a string as a parameter.
                        string rCalc = calculation(c,path,o);
                        //Convert the result to the buffer format
                        byte[] res1 = System.Text.Encoding.UTF8.GetBytes(rCalc);
                        using (StreamWriter sw = File.AppendText(path)) 
                        {
                            sw.Write(o);
                            sw.WriteLine(" : " + rCalc);
                        }
                        //Send the result
                        list.Send(res1, SocketFlags.None);
                        using (StreamWriter sw = File.AppendText(path)) 
                        {
                            sw.Write(o);
                            sw.WriteLine(" : Result sent");
                        }
                    }
                }
                //Close the socket connection
                list.Close();
            }
          }
     }
}
