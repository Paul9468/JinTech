using UnityEngine.UI;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public KeyCode manuel = KeyCode.Space;
    [SerializeField]
    private Robot robot = null;
    [SerializeField]
    TCPServer server = null;
    
    private RawImage UI_ctrl;
    private RawImage UI_swap;
    private RawImage UI_code;
    //private RawImage UI_backText;
    //private Text UI_text;
    private GameObject UI_ControlTxt;


    private void Start()
    {
        //text = GetComponent<Text>();
        UI_ctrl = transform.GetChild(0).GetComponent<RawImage>();
        UI_swap = transform.GetChild(1).GetComponent<RawImage>();
        UI_code = transform.GetChild(2).GetComponent<RawImage>();
        //UI_backText = transform.GetChild(3).GetComponent<RawImage>();
        //UI_text = UI_backText.transform.GetComponentInChildren<Text>();
        UI_ControlTxt = transform.GetChild(3).gameObject;
        server.enabled = false; // Par défaut on part sur de la commande manuelle
    }

    void Update()
    {
        if (Input.GetKeyDown(manuel))
        {
            ChangeMode();
        }
    }

    public void ChangeMode()
    {
        bool man = !robot.commandeManuelle;
        robot.commandeManuelle = man;
        robot.Servo.commandeManuelle = man;
        //UI_text.enabled = man;
        //UI_backText.enabled = man;
        UI_ControlTxt.SetActive(man);
        if (robot.commandeManuelle)
        {
            UI_ctrl.color = Color.white;
            UI_code.color = new Color32(0, 0, 0, 100);
            UI_swap.uvRect = new Rect(0, 0, -1, -1);
        }
        else
        {
            UI_ctrl.color = new Color32(0, 0, 0, 100);
            UI_code.color = Color.white;
            UI_swap.uvRect = new Rect(0, 0, 1, 1);
        }
        server.enabled = !man;
    }
}
