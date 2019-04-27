using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
//using UnityEngine.UI;


// Client app is the one sending messages to a Server/listener.   
// Both listener and client can send messages back and forth once a   
// communication is established.  



public class SocketClient
    {
        public static int Main(String[] args)
        {

            
            StartClient();
            return 0;
        }

    public static bool isConnected()
    {
        return true;//return true once the client recieves a message from the listener
    }


    public static void StartClient()
    {
        byte[] bytes = new byte[1024];

    
       // var vc = new VerifyingConnection();


            try
            {
            //var test2 = new VerifyingConnection();
                //VerifyingConnection vc = GetCompoment

                //new verifyingConnection instance to call from client once the client has connected


                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  


                //get username/email to add to IPHostEntry
                //13.59.229.139 IP Address to website
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.    
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);//using TCP

                //Console.WriteLine("outisde of try statement");
                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);


                    Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.    

                    //byte[] chatMessage; 
                    //variable for chat strings

                    byte[] msg = Encoding.ASCII.GetBytes("sending message to listener test");

                    //get the string from the chat box script and and put it in msg;

                    // Send the data through the socket.    
                    int bytesSent = sender.Send(msg);//send data break point

                    // Receive the response from the remote device.    
                    int bytesRec = sender.Receive(bytes);
                    

                    Console.WriteLine("Echoed test = {0}, from listener",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));
                isConnected();

                    // VerifyingConnection.
                   //test2.testingScriptAccess();
                   


                    // Release the socket.    
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                    //exit out of socket
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }


//namespace verifying connection bracket