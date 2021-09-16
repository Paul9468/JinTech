using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoueCodeuse : MonoBehaviour
{

    [Header("Components")]
    public WheelCollider left;
    public WheelCollider right;
    public Rigidbody robot;

    [Header("Variables")]
    public float rcDeplacement;
    public float rcRotation;
    private float Speed;
    private float Rotation;

    // Start is called before the first frame update
    void Start()
    {
        rcDeplacement = 0;
        rcRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Speed = (left.rpm + right.rpm) / (360 * 2);
        if (GetComponent<Rigidbody>().velocity.magnitude > 0.1) // Ce if permet d'eviter d'incrementer la roue lorsque Speed n'est qu'une erreur de calcul 
        {                                                        // entre les deux roues, ou lorsque le robot avance dans un mur
            rcDeplacement += (left.rpm + right.rpm) / (360 * 2); // 360 frames par minutes
        }
        Rotation = (left.rpm - right.rpm) / 360;
        if (Rotation > 0.01 || Rotation < -0.01) //Voir commentaire précedent
        {
            rcRotation += (left.rpm - right.rpm) / 360; // 360 frames par minutes
        }
    }
}
