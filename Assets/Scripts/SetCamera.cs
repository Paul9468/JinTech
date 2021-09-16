using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamera : MonoBehaviour
{
    public int idCam = 0;
    GameObject[] cams;

    private void Start()
    {
        cams = new GameObject[transform.childCount];
        for (int i = 0; i< cams.Length; i++)
        {
            cams[i] = transform.GetChild(i).gameObject;
            cams[i].SetActive(false);
        }
        cams[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ChangeCamera")) nextCam();
    }

    private void nextCam()
    {
        cams[idCam].SetActive(false);
        idCam++;
        if(idCam >= cams.Length)
        {
            idCam = 0;
        }
        cams[idCam].SetActive(true);
    }
}
