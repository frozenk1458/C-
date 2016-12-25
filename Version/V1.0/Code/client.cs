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
     public static Socket Send(string c, string ipAddress, int connexion)
     {
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            Console.WriteLine(date);
            string d = DateTime.Now.ToString("ddMMyyyy");
            string filename = "log_client_" + d + ".txt";
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

            string rline = " ";
            int first=0;
            while(String.Compare(rline,"Connect") != 0 && String.Compare(rline,"Calc") != 0)
            {
                Console.WriteLine("\nYou can enter a command line. Please read the command line list file.");
                //Ask the user to enter a text line
                rline = Console.ReadLine();
                if(String.Compare(rline,"Connect") == 0)break; 
                if(String.Compare(rline,"Connect") != 0 || String.Compare(rline,"Calc") != 0 && first == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Command Invalid. Please refer to command line list file.");
                }
                if(String.Compare(rline,"Calc") == 0 && connexion !=1)
                {
                    Console.Clear();
                    Console.WriteLine("You are not connected so you cannot Calc. Please enter the command \"Connect\" first.");
                    rline = " ";
                }
                first = 1;
            }
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
     public static int Connect(string c,Socket ClientServer, int connexion)
     {
            if(String.Compare(c," enterlogin") ==0)
            {
                Console.WriteLine("\nPlease enter your login.");
                //Ask the user to enter a text line
                string rline = " ";
                rline = Console.ReadLine();
                rline = rline + ":";
                Console.WriteLine("\nPlease enter your password.");
                rline = rline + Console.ReadLine();
                //Put at the end of the line entered by the user ";" which is the caracter end of the buffer
                rline = rline + ";";
                byte[] chaine = System.Text.Encoding.UTF8.GetBytes(rline);
                ClientServer.Send(chaine, SocketFlags.None);
                c= " ";
            }
            string recep = " ";
            string CSpc = " ";
            while(String.Compare(CSpc,"ConnexionOK") !=0)
            {
            recep = Receive(c,ClientServer);
            CSpc = recep.Replace(" ","");
            if(String.Compare(CSpc,"ConnexionOK") ==0)
            {
                Console.Clear();
                Console.WriteLine("Your connexion is secured now!");
                connexion = 1;
            }
            if(String.Compare(CSpc,"ConnexionNOK") ==0)
            {
                Console.Clear();
                Console.WriteLine("You entered a wrong login or password. Please try again");
                Console.WriteLine("\nPlease enter your login.");
                //Ask the user to enter a text line
                string rline = " ";
                rline = Console.ReadLine();
                rline = rline + ":";
                Console.WriteLine("\nPlease enter your password.");
                rline = rline + Console.ReadLine();
                //Put at the end of the line entered by the user ";" which is the caracter end of the buffer
                rline = rline + ";";
                byte[] chaine = System.Text.Encoding.UTF8.GetBytes(rline);
                ClientServer.Send(chaine, SocketFlags.None);
                c= " ";
            }
         }
         return connexion;
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
        //string ipAddress = addr[1].ToString();
        string ipAddress = "127.0.0.1";
        Console.WriteLine("Welcome !!!");
        Thread.Sleep(1000);
        Console.Clear();
        string c = " ";
        int connexion = 0;
        string calt = " ";
        //Infinite client loop
        while(true)
        {
            c=" ";
            Socket w = Send(c,ipAddress,connexion);
            c = Receive(c,w);
            string h = c.Replace(" ","");
            if(String.Compare(h,"ASK") == 0)
            {
                string stop = " ";
                while(String.Compare(stop,"STOP;") != 0)
                {
                    string ca = " ";
                    Console.WriteLine("\nEnter the calculation you want to do");
                    //Ask the user to enter a text line
                    ca = Console.ReadLine();
                    ca = ca + ";";
                    byte[] chaine = System.Text.Encoding.UTF8.GetBytes(ca);
                    w.Send(chaine, SocketFlags.None);
                    calt = Receive(calt,w);
                    Console.WriteLine("Result : {0}",calt);
                    Console.WriteLine("If you want to stop your calculation, please enter the word \"STOP\". Enter anything else to continue.");
                    stop = Console.ReadLine();
                    stop = stop + ";";
                    byte[] stopp = System.Text.Encoding.UTF8.GetBytes(stop);
                    w.Send(stopp, SocketFlags.None);
                }
            }
            if(connexion !=1)
            connexion = Connect(c,w,connexion);
            //Close the socket connection
            w.Close();
        }
	 }
    }
}
