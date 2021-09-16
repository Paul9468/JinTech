using UnityEngine;
using UnityEngine.UI;

public class Bras_UI : MonoBehaviour
{
    [SerializeField]
    private Bras bras = null;
    private Ventouse vent;
    private RawImage raw;
    [SerializeField]
    private RawImage VentouseIndic = null;

    // Start is called before the first frame update
    void Start()
    {
        raw = GetComponent<RawImage>();
        vent = bras.Module;
    }

    // Update is called once per frame
    void Update()
    {
        Bras_Color();
        Ventouse_Color();

    }

    void Bras_Color()
    {
        if (bras.etat)  // Bras Baissé
        {
            if (bras.positionBasse)
            {
                raw.color = new Color32(210, 90, 90, 255);   //Rouge
            }
            else
            {
                raw.color = new Color32(212, 183, 104, 255);   //Ocre
            }
        }
        else            // Bras levé
        {
            raw.color = new Color32(103, 112, 212, 255);       // Bleu
        }
    }

    void Ventouse_Color()
    {
        if (vent.aspire)  // Ventouse active
        {
            if (vent.occupe)
            {
                VentouseIndic.color = new Color32(255, 100, 0, 255);   //Rouge
            }
            else
            {
                VentouseIndic.color = new Color32(0, 255, 25, 255);   //Vert
            }
        }
        else            // Ventouse inactive
        {
            VentouseIndic.color = new Color32(188, 188, 188, 255);       // Gris
        }
    }
}
