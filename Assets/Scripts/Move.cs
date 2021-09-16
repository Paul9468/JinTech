using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //Speed
    public float mspeed = 10f;
    public float rspeed = 100f;
    private float temp = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Création de 2 nouveaux vecteurs de déplacement
        Vector3 move = new Vector3();
        temp = 0;

        // Récupération des touches haut et bas
        if (Input.GetKey(KeyCode.UpArrow))
            move.z += 1f;
        if (Input.GetKey(KeyCode.DownArrow))
            move.z -= 1f;

        // Récupération des touches gauche et droite
        if (Input.GetKey(KeyCode.LeftArrow))
            temp = 1;
        if (Input.GetKey(KeyCode.RightArrow))
            temp = -1;

        // Modif des vitesses
        if (Input.GetKey(KeyCode.A))
        {
            mspeed = 10f;
            rspeed = 100f;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            mspeed = 100f;
            rspeed = 200f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            mspeed = 500f;
            rspeed = 500f;
        }



        // On applique le mouvement à l'objet
        transform.Translate(move * mspeed * Time.deltaTime);
        transform.Rotate(0, temp * rspeed * Time.deltaTime, 0);
    }
}
