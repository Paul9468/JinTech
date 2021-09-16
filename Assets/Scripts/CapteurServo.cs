using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapteurServo : MonoBehaviour
{
    public CapteurIR Capteur1;
    public CapteurIR Capteur2;
    public CapteurLidar Lidar;
    //public RoueCodeuse1
    //public RoueCodeuse2

    public TCPServer server;

    private string separator = " ";
    private string terminator = "\n";

    /* Tests des Fonctions
    public bool test;
    private void Update()
    {
        if (test) SendIRMessage(1);
    }
    */


    public void SendIRMessage(int id)
    {
        //Création du de la variable du message mise à 0 (null)
        string msg = null;
        if (id == 0)
        {
            //Initialisation du Message avec le header du Capteur n°1
            msg += "IR";
            msg += Capteur1.distanceTouch;
        }
        else
        {
            //Initialisation du Message avec le header du Capteur n°2
            msg += "RI";
            msg += Capteur2.distanceTouch;
        }
        msg += terminator;
        Debug.Log(msg);
        server.SendAMessage(msg);
    }

    public void SendLidarMessage()
    {
        //Initialisation du Message avec le header du Lidar
        string msg = "!!"; //en C : { 0x21, 0x21 } et 0x21 correspond au code ASCII pour '!';
        for (int i=0; i<Lidar.resolution; i++)
        {
            if (Lidar.distanceTab[i] > 0)
            {
                // Il faut enregistrer l'angle et la distance
                msg += Lidar.distanceTab[i] + separator + (i * Lidar.angleRot) + separator; //potentiellement un *-1 ou un -Lidar.angleMax pour avoir la bonne origine?
            }
        }
        msg = msg.Remove(msg.Length - 1) + terminator;
        //Debug.Log(msg);
        server.SendAMessage(msg);
    }
}
