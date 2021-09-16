using UnityEngine;

public class Ailette : MonoBehaviour
{
    public KeyCode toucheAbaissement;

    [SerializeField]
    private Transform ailette = null;
    private Rigidbody ailetteRb;

    public float angle;
    public bool etat; //false : ailette relevée

    [SerializeField]
    private int orientation = 1; // =1 => ailette tombe vers la droite soit vers 90° ; =-1 => vers -90°
    private int angleCap = 90;
    private int PuissanceAngulaire = 500;



    private void Start()
    {
        ailetteRb = ailette.gameObject.GetComponent<Rigidbody>();
    }


    void Update()
    {
        angleGet();
        capRot();
        //if (Input.GetKeyDown(toucheAbaissement)) { Controller(); }
    }

    private void FixedUpdate()
    {
        Commande_Rot();
    }

    // Donne la valeur de l'angle de l'ailette dans angle
    void angleGet()
    {
        angle = ailette.localRotation.eulerAngles.z;
        while (angle >= 180)
        {
            angle -= 360;
        }
        while (angle <= -180)
        {
            angle += 360;
        }
    }

    void Commande_Rot()
    {
        if (etat)
        {
            // Vers le bas
            ailetteRb.AddTorque(PuissanceAngulaire * orientation * transform.forward);
        }
        else
        {
            // Vers le haut
            ailetteRb.AddTorque(PuissanceAngulaire * orientation * -transform.forward);
        }
    }

    void capRot()
    {
        if ((angle * orientation < 0.1f))   // && !(etat)
        {
            angle = 0.1f * orientation;
            ailette.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if ((angle * orientation > angleCap))  // && (etat)
        {
            angle = angleCap * orientation;
            ailette.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public void Action()
    {
        // Baisser ou lever le bras
        ailetteRb.angularVelocity = Vector3.zero;
        ailetteRb.velocity = Vector3.zero;
        if (etat)
        {
            ailetteRb.AddTorque(PuissanceAngulaire * orientation * -transform.forward);
        }
        else
        {
            ailetteRb.AddTorque(PuissanceAngulaire * orientation * transform.forward);
        }
        etat = !(etat);
    }
}
