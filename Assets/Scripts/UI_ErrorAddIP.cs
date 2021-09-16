using UnityEngine.UI;
using UnityEngine;

public class UI_ErrorAddIP : MonoBehaviour
{
    public bool error;

    // Update is called once per frame
    void Update()
    {
        if (error)
        {
            GetComponent<Text>().enabled = true;
        }
    }
}
