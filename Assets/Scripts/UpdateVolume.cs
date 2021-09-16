using UnityEngine.UI;
using UnityEngine;

public class UpdateVolume : MonoBehaviour
{
    GameManager gameManager;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        gameManager.SetVolume(slider.value);
    }
}
