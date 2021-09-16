using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrasServo : MonoBehaviour
{
    [SerializeField]
    private int idCourant=0;
    private bool aileCourante = true; // true = ailette 1
    public bool commandeManuelle = true;

    [Header("Les Bras")]
    [SerializeField]
    private Bras[] bras = null;

    

    //----------------------------------------

    [Header("Les Ailettes")]
    [SerializeField]
    private Ailette[] ailettes = null;


    //----------------------------------------

    [Header("UI Elements")]
    [SerializeField]
    private Text[] texts = null;
    [SerializeField]
    private RawImage[] ailettesUI = null;





    private void Start()
    {
        //setMass(transform);
        
        // Méthodes en lien avec l'UI
        HighlightBrasActif();
        HighlightAiletteActif();
    }

    void Update()
    {
        if (commandeManuelle)
        {
            SetActif();
            Action();
        }
    }




    private void setMass(Transform Root)
    {
        foreach (Transform child in Root)
        {
            Debug.Log(Root.gameObject.name + " -> " + child.gameObject.name);
            if(child.GetComponent<Rigidbody>() != null)
            {
                child.GetComponent<Rigidbody>().mass = 0.001f;
            }
            if (child.childCount != 0)
            {
                setMass(child);
            }
        }
    }

    private void SetActif()
    {
        // Bras
        if (Input.GetButtonDown("SetActifBras1")) { idCourant = 0; HighlightBrasActif(); }
        else if (Input.GetButtonDown("SetActifBras2")) { idCourant = 1; HighlightBrasActif(); }
        else if (Input.GetButtonDown("SetActifBras3")) { idCourant = 2; HighlightBrasActif(); }
        else if (Input.GetButtonDown("SetActifBras4")) { idCourant = 3; HighlightBrasActif(); }
        else if (Input.GetButtonDown("SetActifBras5")) { idCourant = 4; HighlightBrasActif(); }
        else if (Input.GetButtonDown("SetActifBras6")) { idCourant = 5; HighlightBrasActif(); }

        // Ailettes
            if (Input.GetButtonDown("ChangeActifAilette")) { aileCourante = !aileCourante; HighlightAiletteActif(); }
    }

    private void Action()
    {
        // Bras
        if (Input.GetButtonDown("BaisserBrasTous"))
        {
            foreach (Bras b in bras)
            {
                b.BaisserBras();
            }
        }
        if (Input.GetButtonDown("BasBrasTous"))
        {
            foreach (Bras b in bras)
            {
                b.ChangerNiveau();
            }
        }
        if (Input.GetButtonDown("VentouseTous"))
        {
            foreach (Bras b in bras)
            {
                b.ActiverModule();
            }
        }
        if (Input.GetButtonDown("BaisserBrasUn"))
        {
            bras[idCourant].BaisserBras();
        }
        if (Input.GetButtonDown("BasBrasUn"))
        {
            bras[idCourant].ChangerNiveau();
        }
        if (Input.GetButtonDown("VentouseUn"))
        {
            bras[idCourant].ActiverModule();
        }

        // Ailette
        if (Input.GetButtonDown("BaisserAiletteTous"))
        {
            ailettes[0].Action();
            ailettes[1].Action();
        }
        if (Input.GetButtonDown("BaisserAiletteUn"))
        {
            if (aileCourante)
            {
                ailettes[0].Action();
            }
            else
            {
                ailettes[1].Action();
            }
        }
    }

    private void HighlightBrasActif()
    {
        foreach(Text t in texts)
        {
            t.color = Color.black;
        }
        texts[idCourant].color = Color.white;
    }

    private void HighlightAiletteActif()
    {
        if (aileCourante)
        {
            ailettesUI[0].color = new Color32(175, 175, 225, 255);
            ailettesUI[1].color = new Color32(103, 112, 212, 255);
        }
        else
        {            
            ailettesUI[0].color = new Color32(103, 112, 212, 255);
            ailettesUI[1].color = new Color32(175, 175, 225, 255);
        }
    }


    //Méthodes de Serveur
    public void ActionBras(int _action, int _bras)
    {
        Debug.Log($"Action n° {_action} sur le bras n°{_bras}");
        if( _bras == 0 )
        {
            foreach (Bras b in bras)
            {
                ActionSurUnBras(_action, b);
            }
        }
        else if( _bras < 7 )    //_bras compris entre 1 et 6 inclus
        {
            ActionSurUnBras(_action, bras[_bras-1]);
        }
    }

    private void ActionSurUnBras(int _action, Bras _b)
    {
        /* Action :
         * 0 = baisser/lever bras
         * 1 = changer position basse
         * 2 = ventouse
         * 3 = mettre le bras en position haute
         * 4 = mettre le bras en position milieu
         * 5 = mettre le bras en position basse
         * 6 = activer la ventouse
         * 7 = désactiver la ventouse
         */
        if (_action == 0)           //baisser/lever bras
        {
            _b.BaisserBras();
        }
        else if (_action == 1)      //changer position basse
        {
            _b.ChangerNiveau();
        }
        else if (_action == 2)      //ventouse
        {
            _b.ActiverModule();
        }
        else if (_action == 3)      //mettre le bras en position haute
        {
            if (_b.etat)
            {
                _b.BaisserBras();
            }
        }
        else if (_action == 4)      //mettre le bras en position milieu
        {
            if (!_b.etat)
            {
                if (_b.positionBasse)
                {
                    _b.ChangerNiveau();
                }
                _b.BaisserBras();
            }
        }
        else if (_action == 5)      //mettre le bras en position basse
        {
            if (!_b.etat)
            {
                if (!_b.positionBasse)
                {
                    _b.ChangerNiveau();
                }
                _b.BaisserBras();
            }
        }
        else if (_action == 6)      //activer la ventouse
        {
            if (!_b.Module.aspire)
            {
                _b.ActiverModule();
            }
        }
        else if (_action == 7)      //désactiver la ventouse
        {
            if (_b.Module.aspire)
            {
                _b.ActiverModule();
            }
        }
    }

    public void ActionAilette(int _action, int _ailette)
    {
        Debug.Log($"Action sur l'ailette n°{_ailette}");
        if (_ailette == 0)
        {
            ActionSurUneAilette(_action, ailettes[1]);
            ActionSurUneAilette(_action, ailettes[0]);
        }
        else if (_ailette < 3)
        {
            ActionSurUneAilette(_action, ailettes[_ailette - 1]);
        }
    }

    public void ActionSurUneAilette(int _action, Ailette _a)
    {
        /* Action :
         * 0 = baisser/lever l'ailette
         * 1 = lever l'ailette
         * 2 = baisser l'ailette
         */
        if (_action == 0)           //baisser/lever bras
        {
            _a.Action();
        }
        else if (_action == 1)      //lever bras
        {
            if (_a.etat)
            {
                _a.Action();
            }
        }
        else if (_action == 2)      //baisser bras
        {
            if (!_a.etat)
            {
                _a.Action();
            }
        }
    }
}
