using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIPause : MonoBehaviour
{
    bool pause = false;
    GameObject pauseHolder = null;
    [SerializeField]
    GameObject FirstButton = null;
    [SerializeField]
    Animator FadeSystem = null;

    private void Start()
    {
        pauseHolder = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (pause) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pause = true;
        Time.timeScale = 0;
        pauseHolder.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FirstButton);
    }
    public void Resume()
    {
        pause = false;
        Time.timeScale = 1;
        pauseHolder.SetActive(false);
    }
    public void LoadScene(int idScene)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadSceneCorout(idScene));
    }
    IEnumerator LoadSceneCorout(int idScene)
    {
        FadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(idScene);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
