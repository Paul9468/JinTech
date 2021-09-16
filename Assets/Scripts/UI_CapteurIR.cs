using UnityEngine.UI;
using UnityEngine;

public class UI_CapteurIR : MonoBehaviour
{
    public CapteurIR IR;
    private RawImage im;

    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IR.touch) im.color = Color.HSVToRGB(0.3f*(IR.distanceTouch/IR.distanceRay), 1f, 1f);
        else im.color = Color.black;
    }
}
