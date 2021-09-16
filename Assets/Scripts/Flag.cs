using UnityEngine.UI;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public bool releve;

    private Transform enfant;
    private Vector3 RotCible;
    private Rigidbody rb;

    [SerializeField]
    private RawImage UI_flag = null;


    void Start()
    {
        releve = false;
        enfant = GetComponent<Transform>().GetChild(1);
        rb = enfant.GetComponent<Rigidbody>();        
    }
    
    void Update()
    {
        RotCible = enfant.eulerAngles;
        // releve indique si le drapeau est relevé : double utilité car permet aux auters obj de savoir si le drapeau est levé + pas de vérif sur la rotation une fois le drapeau levé
        if (!(releve))
        {
            if ((RotCible.x > 357) || (RotCible.x < 200))
            {                        
                releve = true;
                UI_flag.color = Color.white;
                rb.isKinematic = true;
                //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }
        else if (Input.GetButtonDown("ReinitFlag"))
        {
            releve = false;
            enfant.localEulerAngles = new Vector3(-90, 0, 90);
            UI_flag.color = new Color32(0,0,0, 100);
            rb.isKinematic = false;
        }
    }
}