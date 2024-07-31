using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject playButton;
    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private Slider musicSlider;

    private AudioSource audioController;

    void Start()
    {
        audioController = FindObjectOfType<AudioSource>();

        if (File.Exists(Application.persistentDataPath + "/data.json"))
        {
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void SetOptionsVisibility(bool visible)
    {
        if (visible)
        {
            musicSlider.value = PlayerPrefs.GetFloat("volume", 0.5F);
        }
        else
        {
            PlayerPrefs.SetFloat("volume", musicSlider.value);
            PlayerPrefs.Save();
        }

        optionsPanel.SetActive(visible);
    }

    public void SetMusicVolume()
    {
        audioController.volume = musicSlider.value;
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
