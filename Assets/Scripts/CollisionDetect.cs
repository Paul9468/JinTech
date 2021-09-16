using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    public bool Trig;

    // Start is called before the first frame update
    void Start()
    {
        Trig = false;
    }

    void OnTriggerEnter()
    {
        Trig = true;
    }

    void OnTriggerExit()
    {
        Trig = false;
    }






    // Update is called once per frame
    void Update()
    {
        
    }
}
