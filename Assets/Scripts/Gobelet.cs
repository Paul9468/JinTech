using UnityEngine;

public class Gobelet : MonoBehaviour
{
    [SerializeField]
    private int colorId = 0; // 0 : rouge, 1 : vert
    private Color[] colors = { new Color(0.8f,0.1f,0.1f), new Color(0f,0.4f,0.1f) };
    private Vector3 posInit;
    private Quaternion rotinit;
    private bool ghosted = false;



    // enregistre la position et rotation d'origine + donne la bonne couleur au gobelet
    void Start()
    {
        posInit = transform.position;
        rotinit = transform.rotation;
        transform.GetChild(0).GetComponent<Renderer>().material.color = colors[colorId];
    }

    // permet de remettre le gobelet à sa position et avec sa rotation d'origine + de passer en mode ghost (c'est-à-dire sans physique)
    void Update()
    {
        if (Input.GetButtonDown("ReinitGobelet"))
        {
            transform.position = posInit;
            transform.rotation = rotinit;
        }
        if (Input.GetButtonDown("GhostGobelet"))
        {
            GetComponent<MeshCollider>().enabled = ghosted;     //doit être faux quand on veut le comportement ghost (pas de boite de collision)
            ghosted = !ghosted;                                 //devient vrai quand on a le comportement ghost
            GetComponent<Rigidbody>().isKinematic = ghosted;    //doit être vrai quand on veut le comportement ghost (objet immobile)
        }
    }
}
