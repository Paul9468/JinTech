using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject UILidar = null;
    [SerializeField]
    private GameObject FlagManager = null;
    [SerializeField]
    private GameObject UIFlag = null;
    [SerializeField]
    private GameObject GobeletManager = null;
    [SerializeField]
    private TCPServer Serveur = null;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("affLidar") == 0) Destroy(UILidar);
        if (PlayerPrefs.GetInt("actFlag") == 0) { Destroy(UIFlag);  Destroy(FlagManager);}
        if (PlayerPrefs.GetInt("actGobelet") == 0) Destroy(GobeletManager);
        Serveur.ipAdress = PlayerPrefs.GetString("addIP");
        Serveur.port = PlayerPrefs.GetInt("port");
        Serveur.gameObject.GetComponent<TCPServo>().nbMaxOrdreFrame = PlayerPrefs.GetInt("ordre");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
