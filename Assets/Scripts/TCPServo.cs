using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TCPServo : MonoBehaviour
{
    TCPServer server;
    private char[] separateur = { ' ' };
    //si il faut ajouter un character spécial pour fermer le message ca se fera dans la fonction SendAMessage de TCPServer
    Queue<string[]> commandeListList = new Queue<string[]>();
    public int nbMaxOrdreFrame = 3;    // limitation pour eviter qui s'il a plus d'1 ordre par frame pendant une longue durée on ne reste pas bloqué sur un while mais qu'on puisse qd mm en executer plusieurs à la mm frame

    [Header("Robot et capteurs")]    
    public Robot robot;
    public CapteurServo capteurs;
    

    // Set up
    void Start()
    {
        server = GetComponent<TCPServer>();
    }
    // Routine : si la queue est non vide, il y a un ordre à l'executer : on le fait + dans tous les cas on envoie la position
    private void Update()
    {
        for (int i =0; commandeListList.Count > 0 && i < nbMaxOrdreFrame; i++)
        {
            ApplyMessage();
        }
        xyo();
    }



    // méthode appelée losrque le message s est reçu pour pour le décrypter
    public void ReadMessage(string s)
    {
        Debug.Log("client message received as: " + s);
        DecrypteMessageTerminator(s);
    }
    // méthode appelée lorsque la queue est non vide pour executer l'ordre qui va bien
    private void ApplyMessage() {
        string[] commandeList = commandeListList.Dequeue();
        switch (commandeList[0])    //cet élément est la commande, il existe toujours (sinon c'est que s=null, ce qui est impossible)
        {
            default:
                Debug.LogError("Message du haut niveau non reconnu/implémenté : " + commandeList[0]);
                break;
            case "ping":
                ping();
                break;
            //[...]
            case "xyo":
                xyo();
                break;
            case "d":
                if (commandeList.Length == 3)
                {
                    d(commandeList[1], commandeList[2]);
                }
                else
                {
                    d(commandeList[1]);
                }
                break;
            case "t":
                t(commandeList[1]);
                break;
            case "goto":
                Goto(commandeList[1], commandeList[2], commandeList[3]);
                break;
            //[...]
            case "stop":
                stop();
                break;
            case "cx":
                cx(commandeList[1]);
                break;
            case "cy":
                cy(commandeList[1]);
                break;
            case "co":
                co(commandeList[1]);
                break;
            case "cxyo":
                cxyo(commandeList[1], commandeList[2], commandeList[3]);
                break;
            //[...]
            case "av":
                av();
                break;
            case "rc":
                rc();
                break;
            case "td":
                td();
                break;
            case "tg":
                tg();
                break;
            //[...]
            case "maxtr":
                maxtr(commandeList[1]);
                break;
            case "maxro":
                Debug.Log("Case fonctionnel");
                maxro(commandeList[1]);
                break;
            case "maxtrro":
                maxtrro(commandeList[1], commandeList[2]);
                break;
            //[...]
            case "BrasOut":
                brasOut(commandeList[1]);
                break;
            case "BrasIn":
                brasIn(commandeList[1]);
                break;
            //[...]
            case "Suck":
                suck(commandeList[1], commandeList[2]);
                break;
            //[...]
            case "BrasStock":
                brasStock(commandeList[1]);
                break;
            case "BrasEcueil":
                brasEcueil(commandeList[1]);
                break;
            case "BrasDepot":
                brasDepot(commandeList[1]);
                break;
            //[...]
            case "lectureSICK":
                lectureSICK();
                break;
            case "testSICK":
                testSICK(commandeList[1]);
                break;
            //[...]
            case "endMatch":
                endMatch();
                break;
        }
        Debug.Log("Fin de la gestion du message");
    }

    // retourne une liste dont les élements sont 0:Commande ; 1...:Arguments(optionnels)
    private void DecrypteMessage(string msg)
    {
        string[] list = msg.Split(separateur);
        //list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1); //enlève le dernier caractere ('\n')
       
        //On vérifie si on doit confirmer la réception de l'ordre
        if (list[0][0].Equals('!'))
        {
            Debug.Log("Accusé de reception demandé");
            list[0] = list[0].Remove(0, 1);
            ConfirmOrder(list[0]);
        }
        commandeListList.Enqueue(list);

    }
    private void DecrypteMessageTerminator(string msg)
    {
        char[] terminators = { '\n' };
        string[] list = msg.Split(terminators);
        Debug.Log(list.Length + " :" + list[0] + ";" + list[list.Length - 1] + ";");
        for (int i = 0; i < list.Length - 1; i++)
        {
            Debug.Log("CountMSg");
            DecrypteMessage(list[i]);
        }
    }
    // envoie l'accusé de réception de l'ordre order
    private void ConfirmOrder(string order)
    {
        server.SendAMessage("@BconfirmOrder" + separateur[0] + order+ "\n"); // "@B est le channel id
    }
    public void ConfirmPosition()
    {
        server.SendAMessage("@BstoppedMoving\n");
    }


    #region Méthodes de conversion de float
    static public string ConvertDistFTS(float f)    // les distances d'Intech sont données en mm alors que nous utilisons des m : m->mm
    {
        return ConvertFTS(f * 1000);
    }
    private float ConvertDistSTF(string sf)         //                                                                             mm->m
    {
        return ConvertSTF(sf) / 1000;
    }
    static public string ConvertAngleFTS(float f)    // les angles d'Intech sont données en rad alors que nous utilisons des deg : deg->rad
    {
        return ConvertFTS(f * Mathf.PI / 180);
    }
    private float ConvertAngleSTF(string sf)         //                                                                            rad->deg
    {
        return ConvertSTF(sf) * 180 / Mathf.PI;
    }

    // Converti un float en string traduisible par IDEA
    static public string ConvertFTS(float f)
    {
        return f.ToString(CultureInfo.InvariantCulture.NumberFormat);
    }
    // converti un string en float (on utilise cette méthode car sinon on a un problème de format avec la virgule
    private float ConvertSTF(string sf)
    {
        return float.Parse(sf, CultureInfo.InvariantCulture.NumberFormat);
    }
    #endregion



    #region méthodes par ordre
    private void ping()
    {
        server.SendAMessage("pong");
    }
    //No compredno ??
    /* j : ?
     * f : -
     */
    private void xyo()
    {
        server.SendAMessage("@P" + ConvertDistFTS(robot.transform.position.x) + separateur[0] + ConvertDistFTS(robot.transform.position.z) + separateur[0] + ConvertAngleFTS(90-robot.transform.eulerAngles.y) + "\n");
    }
    private void d(string _dS, string _ewiS = null)
    {
        float _d = ConvertDistSTF(_dS);
        if (_ewiS != null)
        {
            bool _ewi = bool.Parse(_ewiS);
        }
        robot.Déplacer(_d);            
        Debug.Log($"Le robot doit avancer de {_d} m");
        // Faire avancer le robot de la bonne distance
    }   //TODO ####################### 
    private void t(string _aS)
    {
        Debug.Log($"Le robot doit tourner de {_aS} rad");
        float _a = ConvertAngleSTF(_aS);
        _a=_a-(90-robot.transform.eulerAngles.y);
        while(_a > 180)
        {
            _a -= 360;
        }
        while (_a < -180)
        {
            _a += 360;
        }
        Debug.Log($"Le robot doit tourner de {_a} deg");
        robot.Tourner(_a);
    }
    private void Goto(string _xS, string _yS, string _sS)
    {
        float _x = ConvertDistSTF(_xS);
        float _y = ConvertDistSTF(_yS);
        bool _s = bool.Parse(_sS);
        Debug.Log($"Le robot doit aller à la position ({_x},{_y}) donnée en mm");
        // Faire aller le robot à la bonne position
    }   //TODO #######################   //Majuscule car goto existe deja
//No compredno ??
    /* followTrajectory : ?
      */
    private void stop()
    {
        robot.Arreter();
    }
    private void cx(string _xS)
    {
        float _x = ConvertDistSTF(_xS);
        robot.transform.position = new Vector3(_x, robot.transform.position.y, robot.transform.position.z);
    }
    private void cy(string _yS)
    {
        float _y = ConvertDistSTF(_yS);
        robot.transform.position = new Vector3(robot.transform.position.x, robot.transform.position.y, _y);
    }
    private void co(string _oS)
    {
        float _o = ConvertAngleSTF(_oS)+90;
        robot.transform.rotation = Quaternion.Euler(robot.transform.eulerAngles.x, _o, robot.transform.eulerAngles.z);
    }
    private void cxyo(string _xS, string _yS, string _oS)
    {
        float _x = ConvertDistSTF(_xS);
        float _y = ConvertDistSTF(_yS);
        float _o = ConvertAngleSTF(_oS)+90;
        robot.transform.position = new Vector3(_x, robot.transform.position.y, _y);
        robot.transform.rotation = Quaternion.Euler(robot.transform.eulerAngles.x, _o, robot.transform.eulerAngles.z);
    }
//TODO #######
    /* ctv : -
     * crv : -
     * ctrv : -
     * efm : -
     * dfm : -
     * ct0 : -
     * ct1 : -
     * cr0 : -
     * cv0 : -
     * cv1 : -
     * cod : TODO #########################
     * pfdebug : -
     * rawpwn : -
     * getpwn : -
     * errors : -
     * rawspeed : -
     * rawposdata : -
     * reseteth : -
     * disableTorque : -
     * enableTorque : -
     * montlhery : -
     */
    private void av()
    {
        robot.Avancer();
    }
    private void rc()
    {
        robot.Reculer();
    }
    private void td()
    {
        robot.TournerD();
    }
    private void tg()
    {
        robot.TournerG();
    }
    /* sstop : - 
     */
    private void maxtr(string _mvtS)
    {
        float _mvt = ConvertDistSTF(_mvtS);
        Debug.Log($"La vitesse de translation max du robot doit être de {_mvt} mm/s");
        // Changer la vitesse de translation max du robot
    }
    private void maxro(string _mvrS)
    {
        Debug.Log("Argument du maxro +" + _mvrS + "+");
        //float _mvr = float.Parse(_mvrS);
        float _mvr = ConvertAngleSTF(_mvrS);
        Debug.Log($"La vitesse de rotation max du robot doit être de {_mvr} mm/s");
        // Changer la vitesse de rotation max du robot
    }
    private void maxtrro(string _mvtS, string _mvrS)
    {
        float _mvt = ConvertDistSTF(_mvtS);
        float _mvr = ConvertAngleSTF(_mvrS);
        Debug.Log($"La vitesse de translation max du robot doit être de {_mvt} mm/s...");
        Debug.Log($"... et la vitesse de rotation max du robot doit être de {_mvr} mm/s");
        // Changer la vitesse de translation et de rotation max du robot
    }
    /* trstop : -
     * rostop : -
     * toggle : -
     * displayAsserv : -
     * kpt : -
     * kdt : -
     * kit : -
     * kpr : -
     * kir : -
     * kdr : -
     * kpg : -
     * kig : -
     * kdg : -
     * kpd : -
     * kid : -
     * kdd : -
     * nh : ?
     * eh : ?
     * dh : ?
     * demo : -
     * ptpdemo : -
     * ptpdemoseq : -
     * XLm : -
     * XLs : -
     * posBras : -
     */
    private void brasOut(string _c)
    {
        if (_c.Equals("right"))
        {
            robot.Servo.ActionAilette(2, 2);
        }
        else //(_c.Equals("left"))
        {
            robot.Servo.ActionAilette(2, 1);
        }
    }
    private void brasIn(string _c)
    {
        if (_c.Equals("right"))
        {
            robot.Servo.ActionAilette(1, 2);
        }
        else //(_c.Equals("left"))
        {
            robot.Servo.ActionAilette(1, 1);
        }
    }
    /* torqueBras : -
     * torqueXL : -
     * Valve : -
     */
    private void suck(string _pS, string _oS)
    {
        int _p = int.Parse(_pS);
        bool _o = bool.Parse(_oS);
        if (_o) robot.Servo.ActionBras(6,_p);   //on veut la ventouse activée
        else robot.Servo.ActionBras(7, _p);   //on veut la ventouse desactivée
    }
//No compredno ??
    /* FlagUp : ?
     * FlagDown : ?
     * DiodeOn : pas de diode sur unity
     * DiodeOff : pas de diode sur unity
     * LiftUp : pour le robot primaire
     * LiftDown : pour le robot primaire
     * Gate : pour le robot primaire
     */
    private void brasStock(string _bS)
    {
        int _b = int.Parse(_bS);
        robot.Servo.ActionBras(3, _b);
    }
    private void brasEcueil(string _bS)
    {
        int _b = int.Parse(_bS);
        robot.Servo.ActionBras(4, _b);
    }
    private void brasDepot(string _bS)
    {
        int _b = int.Parse(_bS);
        robot.Servo.ActionBras(5, _b);
    }
//No compredno ??
    /* grnd : - 
     * oust : Ordre pour le Robot Primaire
     */
    private void lectureSICK()
    {
        testSICK("1");
        testSICK("2");
    }
    private void testSICK(string _irS)
    {
        int _ir = int.Parse(_irS);
        if (_ir == 1)
            server.SendAMessage("@A" + capteurs.Capteur1.distanceTouch + "\n");
        else
            server.SendAMessage("@A" + capteurs.Capteur2.distanceTouch + "\n");
    }
    /*
     * rangeSICK : ?
     * waitJumper : ?
     */
    private void endMatch()
    {
        Debug.Log("Fin du match : Simulation terminée");
        //Changer la scène vers le menu de fin
    }
    #endregion
}
