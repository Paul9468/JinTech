using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapteurLidar : CapteurIR
{
    [SerializeField]
    Transform originParent = null;
    [SerializeField]
    public int resolution = 500;   // nombre de ray

    public bool globalTouch = false;
    public float[] distanceTab;

    public int angleMax = 135;
    public float angleRot;
    private Quaternion rotInitiale;



    private void Awake()
    {
        resolution = PlayerPrefs.GetInt("resLidar");
        distanceTab = new float[resolution];
        rotInitiale = Quaternion.Euler(originParent.localRotation.eulerAngles - originParent.up * angleMax);
        angleRot = (float)(angleMax*2)/resolution;
    }



    // Update is called once per frame
    void Update()
    {
        globalTouch = false;
        originParent.localRotation = rotInitiale;
        for (int i = 0; i< resolution; i++)
        {
            originParent.localRotation = Quaternion.Euler(originParent.localRotation.eulerAngles + originParent.up * angleRot);
            UseRaycast();
            DrawRaycast();            
            if (touch)
            {
                globalTouch = true;
                distanceTab[i] = distanceTouch;
            }
            else
            {
                distanceTab[i] = 0;
            }
        }
    }
}
