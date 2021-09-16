using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("Changements de Scene" +
        "")]
    public int sceneToOpen = 1;
    public Animator FadeSystem;

    [Header("Interactables Menu")]
    public InputField resLidar;
    public Toggle affLidar;
    public Toggle actFlag;
    public Toggle actGobelet;
    public Slider slidVolume;
    public InputField addIP;
    public InputField port;
    public InputField ordre;
    public GameObject aideButton;
    public GameObject aide;


    private void Awake()
    {
        if (PlayerPrefs.GetInt("resLidar") == 500 || PlayerPrefs.GetInt("resLidar") == 0) resLidar.text = null;
        else resLidar.text = PlayerPrefs.GetInt("resLidar").ToString();
        if (PlayerPrefs.GetInt("affLidar") == 1) affLidar.isOn = true;
        else affLidar.isOn = false;
        if (PlayerPrefs.GetInt("actFlag") == 1) actFlag.isOn = true;
        else actFlag.isOn = false;
        if (PlayerPrefs.GetInt("actGobelet") == 1) actGobelet.isOn = true;
        else actGobelet.isOn = false;
        slidVolume.value = PlayerPrefs.GetInt("slidVolume");
        if (PlayerPrefs.GetString("addIP").Length == 0 || PlayerPrefs.GetString("addIP").Equals("127.0.0.1")) addIP.text = null;
        else addIP.text = PlayerPrefs.GetString("addIP");
        if (PlayerPrefs.GetInt("port") == 13500 || PlayerPrefs.GetInt("port") == 0) port.text = null;
        else port.text = PlayerPrefs.GetInt("port").ToString();
        if (PlayerPrefs.GetInt("ordre") == 3 || PlayerPrefs.GetInt("ordre") <= 0) ordre.text = null;
        else ordre.text = PlayerPrefs.GetInt("ordre").ToString();
    }


    public void StartSimulation()
    {
        Debug.Log("Debut de la Simulation");
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        SaveSettings();
        FadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToOpen);
    }

    private void SaveSettings()
    {
        GameManager man = GameManager.instance;

        #region Save resLidar
        if (resLidar.text.Equals(null)) PlayerPrefs.SetInt("resLidar", 500);
        else
        {
            try
            {
                PlayerPrefs.SetInt("resLidar", int.Parse(resLidar.text));
            }
            catch
            {
                PlayerPrefs.SetInt("resLidar", 500);
            }
        }
        resLidar.interactable = false;
        #endregion
        #region Save affLidar
        if (affLidar.isOn) PlayerPrefs.SetInt("affLidar", 1);
        else PlayerPrefs.SetInt("affLidar", 0);
        affLidar.interactable = false;
        #endregion
        #region Save actFlag
        if (actFlag.isOn) PlayerPrefs.SetInt("actFlag", 1);
        else PlayerPrefs.SetInt("actFlag", 0);
        actFlag.interactable = false;
        #endregion
        #region Save actGobelet
        if (actGobelet.isOn) PlayerPrefs.SetInt("actGobelet", 1);
        else PlayerPrefs.SetInt("actGobelet", 0);
        actGobelet.interactable = false;
        #endregion
        #region Save slidVolume
        PlayerPrefs.SetInt("slidVolume", (int)slidVolume.value);
        slidVolume.interactable = false;
        #endregion
        #region Save addIP
        if (addIP.text.Length == 0) PlayerPrefs.SetString("addIP", "127.0.0.1");
        else
        {
            PlayerPrefs.SetString("addIP", addIP.text);
        }
        addIP.interactable = false;
        #endregion
        #region Save port
        if (port.text.Equals(null)) PlayerPrefs.SetInt("port", 13500);
        else
        {
            try
            {
                PlayerPrefs.SetInt("port", int.Parse(port.text));
            }
            catch
            {
                PlayerPrefs.SetInt("port", 13500);
            }
        }
        port.interactable = false;
        #endregion
        #region Save ordre
        if (ordre.text.Equals(null)) PlayerPrefs.SetInt("ordre", 3);
        else
        {
            try
            {
                int nb = int.Parse(ordre.text);
                if (nb <= 0)
                    nb = 3;
                PlayerPrefs.SetInt("ordre", nb);
            }
            catch
            {
                PlayerPrefs.SetInt("ordre", 3);
            }
        }
        ordre.interactable = false;
        #endregion

        // Check Saves
        Debug.Log(PlayerPrefs.GetInt("resLidar"));
        Debug.Log(PlayerPrefs.GetInt("affLidar"));
        Debug.Log(PlayerPrefs.GetInt("actFlag"));
        Debug.Log(PlayerPrefs.GetInt("actGobelet"));
        Debug.Log(PlayerPrefs.GetInt("slidVolume"));
        Debug.Log(PlayerPrefs.GetString("addIP"));
        Debug.Log(PlayerPrefs.GetInt("port"));
        Debug.Log(PlayerPrefs.GetInt("ordre"));
    }

    public void ShowAide()
    {
        aide.SetActive(!aide.activeSelf);
        if (aide.activeSelf) EventSystem.current.SetSelectedGameObject(aide);    //On affiche l'aide : on veut pas que le joueur aille sur un autre boutton
        else EventSystem.current.SetSelectedGameObject(aideButton);
    }
}
