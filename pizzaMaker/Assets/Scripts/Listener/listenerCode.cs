using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;


// Socket Listener acts as a server and listens to the incoming   
// messages on the specified port and protocol.  
public class SocketListener
{
    //var gameMenu = GameMenuSetup.Instance();
    //var dbCon = DBConnection.Instance();

    //var dbCon = DBConnection.Instance();
    //dbCon.DatabaseName = "googleaidb";


    public static int Main(String[] args)
    {
        //before starting server, make an instance of database to get character status
        StartServer();
        return 0;
    }
    public static bool isConnected()
    {
        return true;
    }

    public static void StartServer()
    {
        // Get Host IP Address that is used to establish a connection  
        // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
        // If a host has multiple addresses, you will get a list of addresses  
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        //Console.WriteLine("outisde of try statement");
        try
        {
            
           // var dbCon = DBConnection.Instance();
            //dbCon.DatabaseName = "googleaidb";


            // Create a Socket that will use Tcp protocol      
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // A Socket must be associated with an endpoint using the Bind method  
            listener.Bind(localEndPoint);
            // Specify how many requests a Socket can listen before it gives Server busy response.  
            // We will listen 10 requests at a time  
            listener.Listen(10);

            //get the status of the listener/client from database
          //  var dbCon = DBConnection.Instance();
          //currently cant make an instance of the Database or database connection
          //access database and check if two users have the same value


          // Console.WriteLine("Waiting for a connection from client");
            Socket handler = listener.Accept();

            // Incoming data from the client. aka other player    
            string data = null;
            byte[] bytes = null;


            //Console.WriteLine("entrance of true statement");

                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);



           // Console.WriteLine("outisde of true statement");
            //data is the message that is sended, will need to keep track with bytesRec: which the client recieves in the client.cs file

            Console.WriteLine("Text received : {0}, from client", data);

            byte[] msg = Encoding.ASCII.GetBytes(data);
            handler.Send(msg);



            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            //shut down handler
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\n Press any key to continue...");
        Console.ReadKey();
    }
}