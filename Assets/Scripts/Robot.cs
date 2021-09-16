using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    //Speed
    [Header("Serveur")]
    public bool commandeManuelle = true;
    public TCPServo server;

    [Header("Components")]
    public WheelCollider left;
    public WheelCollider right;
    public Transform meshleft;
    public Transform meshright;
    public BrasServo Servo;
    public GameObject[] listBrasADesactiver;

    [Header("Speed parameters")]
    public float Torque;
    public float Speed;
    public float MaxSpeed;
    public int Brake;
    public float CoefAcceleration;
    public float WheelAngleMax;
    public float Distanceaparcourir;
    public int mvmt;
    public float dist;
    private Quaternion RotQ;
    public int doitavancer;
    public int doitreculer; 
    public int doittournerD;
    public int doittournerG;
    public float diffangl;
    public float diffanglparcourir;
    public int nbtourD;
    public int nbtourG;
    public float roat;
    public float roatinit;

    [Header("UI Elements")]
    public Text TxtSpeed;
    public Text TxtWheelL;
    public Text TxtWheelR;

    public Text Distance;
    public Text Distance2;

    //GOTO
    public Vector3 robotInit;
    public Vector3 robotPos;
    public Vector3 robotInitRot;
    public Vector3 robotRot;

    public Vector3 localforward;
    public Vector3 gotto;

    public Rigidbody robot;


    void Start()
    {
        mvmt=0;
        
        
        
//        Servo = transform.GetComponentInChildren<BrasServo>();
//        if (Servo == null)
//        {
//            Debug.Log("Erreur : le BrasServo du robot n'a pas été trouvé");
//        }
//        else
//        {
//            Debug.Log($"BrasServo : {Servo.gameObject.name}");
//        }
    }

    void Update()
    {
        //Affichage vitesse
        robot = GetComponent<Rigidbody>();
        Speed = GetComponent<Rigidbody>().velocity.magnitude;
        TxtSpeed.text = "Speed : " + (int)Speed + " m/s";
        // robotPos= robot.transform.position;
        TxtWheelL.text = "Wheel L : " + (int)left.rpm*60/360;
        TxtWheelR.text = "Wheel R : " + (int)right.rpm * 60 / 360;
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        
        if (doitavancer==1) 
        {
            DistanceParcourue();
            Avancer();
        }

        if (doitreculer==1) 
        {
            DistanceParcourue();
            Reculer();
        }

        if (doittournerD==1) 
        {
            RotationParcourueD();
            robotRot= robot.transform.rotation.eulerAngles;
            if (robotRot[1]>=358) {
                nbtourD=1;
            }
            TournerG();
        }

        if (doittournerG==1) 
        {
            RotationParcourueG();
            
            robotRot= robot.transform.rotation.eulerAngles;
            if (robotRot[1]<=2) {
                nbtourG=1;
            }
            TournerD();
        }

        if (commandeManuelle) 
        {
                        if (Input.GetAxis("Vertical") > 0)
            {
                Avancer();
            }
            else        // Décélération        
            {
                Arreter();
                if (Input.GetAxis("Vertical") < 0)   { Reculer(); }
                if (Input.GetAxis("Horizontal") > 0) { TournerG(); }
                if (Input.GetAxis("Horizontal") < 0) { TournerD(); }
           }
        
        }
        
        if (mvmt==0 ) // mvmt==0 si jamais on utiliser la méthode en commande manuel commandeManuelle
        {

        robotInit= robot.transform.position;
        robotInitRot= robot.transform.rotation.eulerAngles;
        Distanceaparcourir=1;
        diffanglparcourir=10;

            if (Input.GetAxis("Vertical") > 0)
            {
                Avance(Distanceaparcourir);
            }
            else        // Décélération        
            {
                Arreter();
                if (Input.GetAxis("Vertical") < 0)   { Reculer(Distanceaparcourir); }
                if (Input.GetAxis("Horizontal") > 0) { TournerD(diffanglparcourir); }
                if (Input.GetAxis("Horizontal") < 0) { TournerG(diffanglparcourir); }
            }
        }
        // Direction
       meshleft.Rotate(left.rpm*60/360,0,0);
       meshright.Rotate(right.rpm*60/360,0,0);        
    }

    #region Méthodes de déplacement basiques (sans indication de distance/angle)

    public void Avancer() 
    {
        left.brakeTorque = 0;
        right.brakeTorque = 0;
        left.motorTorque =  Torque * CoefAcceleration * Time.deltaTime /2 ; //Input.GetAxis("Vertical")
        right.motorTorque = Torque * CoefAcceleration * Time.deltaTime /2;
    }


    public void Reculer()
    {        
        left.brakeTorque = 0;
        right.brakeTorque = 0;
        left.motorTorque =  (-1)*Torque * CoefAcceleration * Time.deltaTime /2;
        right.motorTorque = (-1)*Torque * CoefAcceleration * Time.deltaTime /2;                   
    }

    public void TournerD()
    {
        left.brakeTorque = 0;
        right.brakeTorque = 0;
        left.motorTorque = Torque * CoefAcceleration * Time.deltaTime * (-1) /2;
        right.motorTorque =  Torque * CoefAcceleration * Time.deltaTime /2;
        meshleft.localEulerAngles=new Vector3(meshleft.localEulerAngles.x,left.steerAngle-meshleft.localEulerAngles.z,meshleft.localEulerAngles.z);
        meshright.localEulerAngles=new Vector3(meshright.localEulerAngles.x,right.steerAngle-meshright.localEulerAngles.z,meshright.localEulerAngles.z);

    }

    public void TournerG()
    { 
        left.brakeTorque = 0;
        right.brakeTorque = 0;
        right.motorTorque =  Torque * CoefAcceleration * Time.deltaTime * (-1) /2;
        left.motorTorque =   Torque * CoefAcceleration * Time.deltaTime /2;
        meshleft.localEulerAngles=new Vector3(meshleft.localEulerAngles.x,left.steerAngle-meshleft.localEulerAngles.z,meshleft.localEulerAngles.z);
        meshright.localEulerAngles=new Vector3(meshright.localEulerAngles.x,right.steerAngle-meshright.localEulerAngles.z,meshright.localEulerAngles.z);  
    }

    public void Arreter()
    {
        left.motorTorque = 0;
        right.motorTorque = 0;
        left.brakeTorque = Brake * CoefAcceleration * Time.deltaTime;
        right.brakeTorque = Brake * CoefAcceleration * Time.deltaTime;
    }
    #endregion

    // méthodes appelées par les packets du serveur
    public void Mouvement(int _move)
    {
        if (!commandeManuelle) { 
            if (_move == 1)
            {
                robotInit= robot.transform.position;
                Avance(Distanceaparcourir);
                Debug.Log("En avant");
            }
            else if (_move == 2)
            {
                robotInit= robot.transform.position;
                Reculer(Distanceaparcourir);
                Debug.Log("En arrière");
            }
            else if (_move == 3)
            {
                TournerD();
                Debug.Log("A droite");
            }
            else if (_move == 4)
            {
                TournerG();
                Debug.Log("A Gauche");
            }
        }
    }

    #region méthodes chronomètre (sécurité quand demmande un déplacement)
    IEnumerator Chrono(float duree)
    {
        Debug.Log("Chrono lancé pour : " + duree);
        float margeMVt = 2f;
        //disableBrasPhysic();
        yield return new WaitForSeconds(duree + margeMVt);
        /*mvmt = 0;
        doitavancer = 0;
        doitreculer = 0;
        doittournerD = 0;
        doittournerG = 0;
        server.ConfirmPosition();*/
        Debug.Log("Chrono fini pour : " + duree);
        stopMvt();
    }
    float getTimeFromDist(float dist)
    {
        float speedTransConst = 2/4;   //vitesse de 2m/s
        return dist / speedTransConst;
    }
    float getTimeFromAngle(float dist)
    {
        float speedRotConst = 120;   //vitesse de 120°/s
        return dist / speedRotConst;
    }
    void enableBrasPhysic(bool enable)
    {/*
        foreach (var mc in Servo.transform.GetComponentsInChildren<MeshCollider>())
        {
            mc.enabled = enable;
        }
        */
        foreach (var b in listBrasADesactiver)
        {
            b.SetActive(enable);
        }
        //Servo.gameObject.SetActive(enable);
    }
    public void stopMvt()
    {
        /*foreach(var mc in Servo.transform.GetComponentsInChildren<MeshCollider>())
        {
            //mc.enabled = true;
        }*/
        enableBrasPhysic(true);
        mvmt = 0;
        doitavancer = 0;
        doitreculer = 0;
        doittournerD = 0;
        doittournerG = 0;
        server.ConfirmPosition();
    }
    #endregion


    #region Méthodes de déplacement avec indication
    public void Tourner(float angl)
    {
        enableBrasPhysic(false);
        Debug.Log("début"+ robot.transform.eulerAngles);
        if (angl < 0)
        {
            TournerD(Mathf.Abs(angl));
        }
        else
        {
            TournerG(Mathf.Abs(angl));
        }
    }

    public void Déplacer( float d ) 
    {
        enableBrasPhysic(false);
        if ( d>0)
        {
            Avance(d);
        }
        else
        {
            Reculer(Mathf.Abs(d));
        }
    }
    public void Avance( float Distanceaparcourir )
    {
        StartCoroutine(AvanceRoutine(Distanceaparcourir));
    } 
    IEnumerator AvanceRoutine (float Distanceaparcourir)
    {   
        doitavancer=1;
        mvmt=1;
      //  Coroutine chrono = StartCoroutine(Chrono(getTimeFromDist(Distanceaparcourir)));
        yield return new WaitWhile(()=> dist < Distanceaparcourir);
        dist=0;
      //  StopCoroutine(chrono);
        /*mvmt =0;   
        doitavancer=0;
        server.ConfirmPosition();*/
        stopMvt();
    }    

    public void Reculer( float Distanceaparcourir )
    {
        StartCoroutine(ReculerRoutine(Distanceaparcourir));
    } 
    IEnumerator ReculerRoutine (float Distanceaparcourir)
    {   
        doitreculer=1;
        mvmt=1;
      //  Coroutine chrono = StartCoroutine(Chrono(getTimeFromDist(Distanceaparcourir)));
        yield return new WaitWhile(()=> dist < Distanceaparcourir);
        dist=0;
      //  StopCoroutine(chrono);
        /*mvmt =0;   
        doitreculer=0;
        server.ConfirmPosition();*/
        stopMvt();
    }    

    public void TournerD( float diffanglparcourir )
    {
        diffangl=0;
        StartCoroutine(TournerDRoutine(diffanglparcourir));
        
    } 
    IEnumerator TournerDRoutine (float diffanglparcourir)
    {   
        doittournerD=1;
        mvmt=1;
      //  Coroutine chrono = StartCoroutine(Chrono(getTimeFromAngle(Distanceaparcourir)));
        yield return new WaitWhile(()=> diffangl < diffanglparcourir);
        diffangl=0;
       // StopCoroutine(chrono);
        nbtourD =0;
        mvmt=0;
        doittournerD = 0;
        Debug.Log("fin"+robot.transform.eulerAngles);
        stopMvt();
        //server.ConfirmPosition();
    }  

    public void TournerG( float diffanglparcourir )
    {
        diffangl=0;
        StartCoroutine(TournerGRoutine(diffanglparcourir));
        
    } 
    IEnumerator TournerGRoutine (float diffanglparcourir)
    {   
        doittournerG=1;
        mvmt=1;
        //Coroutine chrono = StartCoroutine(Chrono(getTimeFromAngle(Distanceaparcourir)));
        yield return new WaitWhile(()=> diffangl < diffanglparcourir);
        diffangl=0;
        //StopCoroutine(chrono);
        nbtourG =0;
        mvmt=0;   
        doittournerG=0;
        Debug.Log("fin"+robot.transform.eulerAngles);
        stopMvt();
        //server.ConfirmPosition();
    }
    #endregion

    #region Dist/Rot parcourues
    public void DistanceParcourue() 
    {
        robot = GetComponent<Rigidbody>();
        robotPos= robot.transform.position;
        Distance.text = "Distance : " + robotInit + " m";
        dist = Vector3.Distance(robotInit, robotPos);
        Distance2.text = "Distance : " + dist + " m";       
    }

    public void RotationParcourueD() 
    {
        robot = GetComponent<Rigidbody>();
        robotRot= robot.transform.rotation.eulerAngles;
        roat = robotRot[1]+360*nbtourD;
        if ( robotRot[1]<358 ) 
        {   diffangl = roat - robotInitRot[1]; }
        else 
        {   diffangl = robotRot[1] - robotInitRot[1];   }
        Distance.text = " test " + roat ;
        Distance2.text = " Angle : " + diffangl ;       
    }

    public void RotationParcourueG() 
    {
        robot = GetComponent<Rigidbody>();
        robotRot= robot.transform.rotation.eulerAngles;
        roatinit = robotInitRot[1]+360*nbtourG;
        if ( robotRot[1]>2 ) 
        {   diffangl = roatinit - robotRot[1] ; }
        else 
        {   diffangl = robotInitRot[1] - robotInitRot[1] ;  }

        Distance.text = "test"+ roat;
        Distance2.text = "Distance : " + diffangl;
    //    Distance.text = "test"+roat ;
    //    Distance2.text = "Angle : " + diffangl ;       
    }
    #endregion


    // Méthode goto
    void got ( float _xS,float _yS) 
    {
        localforward = robot.transform.forward;
       // robotRot= robot.transform.rotation.eulerAngles;
        gotto = new Vector3((_xS-robot.transform.position[0]),(_yS-robot.transform.position[2]));
        
      //Tourner(Vector2.Angle(localforward,gotto));
     // if ( _yS>robot.transform.position[2]) 
    // {
           TournerG(Vector2.Angle(localforward,gotto));
     //  }
     //  else 
      // {
      //     TournerG(Vector2.Angle(localforward,gotto));
      // }
       Debug.Log("test");
       gotto = new Vector3((_xS-robot.transform.position[0]),(_yS-robot.transform.position[2]));
       Avance(gotto.magnitude);
       // Avance ( Mathf.Pow( (_xS-robot.transform.position[0]), 2f)+Mathf.Pow( (_yS-robot.transform.position[2]),2f ));   
    }
}







