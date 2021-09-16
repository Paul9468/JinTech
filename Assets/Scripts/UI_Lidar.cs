using UnityEngine.UI;
using UnityEngine;

public class UI_Lidar : MonoBehaviour
{
    public GameObject UIPrefab;
    public CapteurLidar Lidar;
    private GameObject UILid;
    private Image[] imTab;

    private Color32 couleurVide = new Color32(10, 10, 10, 50);

    private void Start()
    {

        imTab = new Image[Lidar.resolution];
        for (int i=0; i < Lidar.resolution; i++)
        {
            UILid = Instantiate(UIPrefab, transform);
            Debug.Log(Lidar.angleRot + " " + Lidar.angleMax + " " + Lidar.angleRot / (float)(Lidar.angleMax * 2));
            UILid.GetComponent<Image>().fillAmount = Lidar.angleRot / (float)(Lidar.angleMax * 2);
            UILid.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, Lidar.angleMax - i * Lidar.angleRot));
            imTab[i] = UILid.GetComponent<Image>();
        }
    }

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (Lidar.distanceTab[i] > 0)
            {
                imTab[i].color = Color.HSVToRGB(0.3f * (Lidar.distanceTab[i] / Lidar.distanceRay), 1f, 1f);
            }
            else imTab[i].color = Color.black; //= couleurVide;
        }
    }
}
