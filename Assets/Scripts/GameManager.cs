using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] objectsToKeep;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(gameObject);
            return;
        }
        KeepOnSimulation();
        SetVolume(PlayerPrefs.GetInt("slidVolume"));
    }


    public void KeepOnSimulation()
    {
        foreach (var element in objectsToKeep)
        {
            DontDestroyOnLoad(element);
        }
    }

    public void SetVolume(float vol)
    {
        GetComponent<AudioSource>().volume = vol / 100;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
