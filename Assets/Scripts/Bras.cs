using UnityEngine;

public class Bras : MonoBehaviour
{
    public KeyCode toucheRotation;
    public KeyCode toucheAngle;
    public KeyCode toucheAction;

    [SerializeField]
    private GameObject Cible = null;                //Si ne marche plus quand prefab : someGameObject = GameObject.Find ("GameObjectName");
    [SerializeField]
    private GameObject BrasBas = null;
    [SerializeField]
    private GameObject BrasBasPivot = null;
    [SerializeField]
    private GameObject BrasHaut = null;
    [SerializeField]
    private GameObject BrasHautPivot = null;
    [SerializeField]
    private GameObject Collision60deg = null;
    [SerializeField]
    private GameObject Collision0deg = null;

    public Ventouse Module;

    private Quaternion RotBase;             // Rotation de la Base du bras (qui héberge ce script)
    private Vector3 BHaut_Anchor;           // Position du bras haut
    private Vector3 BBas_Anchor;            // Position du bras bas (celui sur lequel on applique la rotation)


    private float RotX;                      // Valeur de l'angle du bras par rapport à sa position initiale

    //private int PositionAngle = 0;
    private int PuissanceAngulaire = 1000;

    public bool etat = false;                // false = bras levé ; true = bras baissé
    public bool positionBasse = false;       // false = position basse à -20° (angleBas1) ; true = position basse à -40° (angleBas2)

    // -60 = 0°
    public float angle;                      // Valeur de l'angle du bras par rapport au sol
    private int angleBas;                    
    private int angleBas1 = -80;
    private int angleBas2 = -100;
    private int angleHaut = 4;               // en dessous de -7 (exclu) on a des problèmes pour descendre le bras...


    // --------------------------------------- Méthodes Unity ------------------------------------------------------------
    void Awake()
    {
        angleBas = angleBas1;
    }


    
    void Update()
    {
        RotBase = GetComponent<Transform>().rotation;
        BHaut_Anchor = BrasHautPivot.GetComponent<Transform>().position;
        BBas_Anchor = BrasBasPivot.GetComponent<Transform>().position;

        GetRealRot();
        Cap_Rot();
        MAJ_Rots_Poss();
        Commande_Rot();
        //Controller();

        AfficheAngle();
    }



    // --------------------------------------- Autres Méthodes ------------------------------------------------------------

    // permet de connaître le véritable angle du bras (parce que la conversion en eulerAngles ne marche pas super bien)
    void GetRealRot()
    {
        if (Collision60deg.GetComponent<CollisionDetect>().Trig)
        {
            //PositionAngle = 0;
            RotX = -90+(-90 - (BrasBas.transform.localRotation.eulerAngles.x - 360));
        }
        else if (Collision0deg.GetComponent<CollisionDetect>().Trig)
        {
            //PositionAngle = 2;
            RotX = -(BrasBas.transform.localRotation.eulerAngles.x - 360);
        }
        else
        {
            //PositionAngle = 1;
            RotX = BrasBas.transform.localRotation.eulerAngles.x - 360;
        }
    }

    // Verrouille le bras dans la bonne plage d'angles
    void Cap_Rot()
    {
        if ((RotX < angleBas - 1) && (etat))
        {
            BrasBas.GetComponent<Transform>().localRotation = Quaternion.AngleAxis(360 + angleBas, Vector3.right);
            RotX = angleBas;
        }
        else if ((RotX > angleHaut + 1) && !(etat))
        {
            BrasBas.GetComponent<Transform>().localRotation = Quaternion.AngleAxis(-angleHaut, Vector3.left);
            RotX = angleHaut;
        }
    }

    // mets à jour la rotation de l'ensemble des composants du bras
    void MAJ_Rots_Poss()
    {
        BrasBas.GetComponent<Transform>().position = BBas_Anchor;
        BrasHaut.GetComponent<Transform>().position = BHaut_Anchor;
        Cible.transform.rotation = RotBase;
    }

    // Applique la roation pour tourner le bras
    void Commande_Rot()
    {
        if (etat)
        {   
            // Vers le bas
            BrasBas.GetComponent<Rigidbody>().AddTorque(PuissanceAngulaire * - GetComponent<Transform>().right);
        }
        else {
            // Vers le haut
            BrasBas.GetComponent<Rigidbody>().AddTorque(PuissanceAngulaire * GetComponent<Transform>().right);
        }
    }

    // Gère les différents inputs clavier
    void Controller()
    {
        // Baisser ou lever le bras
        if (Input.GetKeyDown(toucheRotation))
        {
            BrasBas.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            BrasBas.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (etat)
            {
                BrasBas.GetComponent<Rigidbody>().AddTorque(PuissanceAngulaire * GetComponent<Transform>().right);
            }
            else
            {
                BrasBas.GetComponent<Rigidbody>().AddTorque(PuissanceAngulaire * -GetComponent<Transform>().right);
            }
            etat = !(etat);
        }

        // Changer la valeur de la position basse
        if (Input.GetKeyDown(toucheAngle))
        {
            if (positionBasse)
            {
                positionBasse = false;
                angleBas = angleBas1;
            }
            else
            {
                positionBasse = true;
                angleBas = angleBas2;
            }
        }
    }

    // Calcul de la valeur exacte de l'angle du bras par rapport a la base
    void AfficheAngle()
    {
        angle = RotX + 60;
        while (angle >= 360)
        {
            angle -= 360;
        }
    }

    public void BaisserBras()
    {
        BrasBas.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        BrasBas.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (etat)
        {
            BrasBas.GetComponent<Rigidbody>().AddTorque(PuissanceAngulaire * GetComponent<Transform>().right);
        }
        else
        {
            BrasBas.GetComponent<Rigidbody>().AddTorque(PuissanceAngulaire * -GetComponent<Transform>().right);
        }
        etat = !(etat);
    }

    public void ChangerNiveau()
    {
        if (positionBasse)
        {
            positionBasse = false;
            angleBas = angleBas1;
        }
        else
        {
            positionBasse = true;
            angleBas = angleBas2;
        }
    }

    public void ActiverModule()
    {
        if (Module != null)
        {
            Module.Aspirer();
        }
    }
}
