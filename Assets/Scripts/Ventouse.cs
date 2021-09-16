using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventouse : MonoBehaviour
{
    public KeyCode toucheAspire;

    public bool aspire = false;
    public bool occupe = false;
    public Renderer etendu;
    public Renderer repli;
    public Transform anchorAspire;

    public Transform objAspire; 

    // Start is called before the first frame update
    void Start()
    {
        etendu = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        repli = transform.GetChild(1).gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(toucheAspire))  { Aspirer(); }       
        AfficheVentouse();
    }


    // Lache l'objet tenu si il y en a un
    void lacherPrise()
    {
        if (occupe)
        {
            occupe = false;
            objAspire.parent = null;
            objAspire.GetComponent<Rigidbody>().isKinematic = false;
            objAspire.GetComponent<Gobelet>().enabled = true;

        }
        aspire = false;
    }

    // Si un objet rentre dans le champs aspirable on essaie de l'aspirer
    /*
    private void OnCollisionStay(Collision collisionInfo)
    {
        if (aspire & !occupe) //si on aspire mais qu'on ne tient toujours rien
        {
            Collider other = collisionInfo.collider;
            if (other.CompareTag("Aspirable"))
            {
                occupe = true;
                objAspire = other.transform;
                objAspire.parent = transform;
                //objAspire.localPosition = new Vector3(other.transform.localPosition.x, other.transform.localPosition.y, other.transform.localPosition.z + 0.1f);
                objAspire.GetComponent<Rigidbody>().isKinematic = true;
                objAspire.GetComponent<Gobelet>().enabled = false;
            }
        }
    }*/
    
    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"Trigger : {other.gameObject.name}");
        if (aspire & !occupe) //si on aspire mais qu'on ne tient toujours rien
        {
            if (other.CompareTag("Aspirable"))
            {
                occupe = true;
                objAspire = other.transform;
                objAspire.parent = transform;
                objAspire.position = anchorAspire.position;
                objAspire.rotation = anchorAspire.rotation;
                //objAspire.localPosition = new Vector3(other.transform.localPosition.x, other.transform.localPosition.y, other.transform.localPosition.z + 0.1f);
                objAspire.GetComponent<Rigidbody>().isKinematic = true;
                objAspire.GetComponent<Gobelet>().enabled = false;
            }
        }
    }
    

    // Affiche la bonne ventouse (repliée ou etendue)
    void AfficheVentouse()
    {
        if (occupe)
        {
            etendu.enabled = false;
            repli.enabled = true;
        }
        else
        {
            etendu.enabled = true;
            repli.enabled = false;
        }
    }

    public void Aspirer()
    {
        if (aspire)
        {
            lacherPrise();
        }
        else
        {
            aspire = true;
        }
    }
}
