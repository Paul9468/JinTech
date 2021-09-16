using UnityEngine.UI;
using UnityEngine;

public class UI_Ailette : MonoBehaviour
{
    [SerializeField]
    private Ailette Ailette = null;
    private RectTransform Rect;

    [SerializeField]
    private int orientation = 1; //vers la droite = 1

    // Start is called before the first frame update
    void Start()
    {
        Rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Ailette.etat)
        {
            Rect.anchoredPosition = new Vector3(15 * orientation, 0);          
            Rect.sizeDelta = new Vector3(50, 15);
        }
        else
        {
            Rect.anchoredPosition = new Vector3(-5 * orientation, 0);            
            Rect.sizeDelta = new Vector3(10, 15);
        }
    }
}
