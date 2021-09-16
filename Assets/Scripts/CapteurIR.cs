using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapteurIR : MonoBehaviour
{
    public bool showRay = true;
    public int distanceRay = 3;
    

    [SerializeField]
    protected Transform origin;

    protected RaycastHit hit;
    public bool touch;
    public float distanceTouch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawRaycast();
        UseRaycast();
    }

    protected void DrawRaycast()
    {
        if (showRay) Debug.DrawRay(origin.position, origin.right, Color.red);
    }

    protected void UseRaycast()
    {
        if (Physics.Raycast(origin.position, origin.right, out hit, distanceRay))
        {
            if (hit.transform.CompareTag("Environnement") || hit.transform.CompareTag("Aspirable"))
            {
                touch = true;
                distanceTouch = hit.distance;
                return;
            }
        }
        touch = false;  // on arrive à cette ligne que si le raycast n'a rien trouvé
    }

    public string ReturnRayResult()
    {
        string msg = "";
        return msg;
    }
}
