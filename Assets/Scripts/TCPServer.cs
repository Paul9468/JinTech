using System;
using System.Collections; 
using System.Collections.Generic; 
using System.Net; 
using System.Net.Sockets; 
using System.Text; 
using System.Threading; 
using UnityEngine;  

public class TCPServer : MonoBehaviour {  

	public Robot robot;	
	#region private members TCP	
	/// <summary> 	
	/// TCPListener to listen for incomming TCP connection 	
	/// requests. 	
	/// </summary> 	
	private TcpListener tcpListener; 
	/// <summary> 
	/// Background thread for TcpServer workload. 	
	/// </summary> 	
	private Thread tcpListenerThread;  	
	/// <summary> 	
	/// Create handle to connected tcp client. 	
	/// </summary> 	
	private TcpClient connectedTcpClient;
    #endregion
    private TCPServo servo;
    public string ipAdress = "127.0.0.1";
    public int port = 8052;
    public UI_ErrorAddIP ErrorAddText;
    
	// Use this for initialization
	void Start () {
        //On recup�re le servo avant de d�marrer le serveur (pour pouvoir g�rer les messages)
        servo = GetComponent<TCPServo>();

        // Start TcpServer background thread
        Debug.Log("Server started");
		tcpListenerThread = new Thread (new ThreadStart(ListenForIncommingRequests)); 		
		tcpListenerThread.IsBackground = true; 		
		tcpListenerThread.Start(); 	
	}  	


    #region Partie pour les m�thodes techniques
    /// <summary> 	
    /// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
    /// </summary> 	
    private void ListenForIncommingRequests () { 		
		try { 			
			// Create listener on localhost port 8052. 			
			tcpListener = new TcpListener(IPAddress.Parse(ipAdress), port); 			
			tcpListener.Start();              
			Debug.Log("Server is listening");              
			Byte[] bytes = new Byte[1024];  			
			while (true) { 				
				using (connectedTcpClient = tcpListener.AcceptTcpClient()) { 					
					// Get a stream object for reading 					
					using (NetworkStream stream = connectedTcpClient.GetStream()) { 						
						int length; 						
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 							
							var incommingData = new byte[length]; 							
							Array.Copy(bytes, 0, incommingData, 0, length);  							
							// Convert byte array to string message. 							
							string clientMessage = Encoding.ASCII.GetString(incommingData);
                            servo.ReadMessage(clientMessage);   //######################################################## On Envoie le message re�u au servo qui va le traiter
                        } 					
					} 				
				} 			
			} 		
		} 		
		catch (SocketException socketException) {
            ErrorAddText.error = true;
            Debug.Log("SocketException " + socketException.ToString()); 		
		}     
	}  	
	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary> 	
	public void SendAMessage(string serverMessage)              //######################################################## On appele cette methode avec en argument le string du message � envoyer
    { 		
		if (connectedTcpClient == null) {             
			return;         
		}  		
		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream(); 			
			if (stream.CanWrite) {
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage); 				
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);

                if (!serverMessage.Contains("@P"))
                {
                    Debug.Log("Message envoy� : " + serverMessage);
                }
                else
                {
                    Debug.Log("Message envoy� : @P ...");
                }				         
			}       
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 	
	}
    #endregion
}