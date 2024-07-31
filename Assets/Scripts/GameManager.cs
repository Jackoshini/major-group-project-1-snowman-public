using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool paused = false;
    [SerializeField] private Canvas pauseScreen;
    [SerializeField] private Canvas loseScreen;
    [SerializeField] private GameObject hud;
    [SerializeField] private UnityEngine.UI.Image heart1;
    [SerializeField] private UnityEngine.UI.Image heart2;
    [SerializeField] private UnityEngine.UI.Image heart3;
    [SerializeField] private Sprite noHp;
    [SerializeField] private UnityEngine.UI.Image xpBar;
    [SerializeField] private UnityEngine.UI.Image xpHighlight;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI timer;

    private string savePath;
    private float gameTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        hud.GetComponent<Canvas>().enabled = true;
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume", 0.5F);
        SetPauseVisibility(false);

        savePath = Application.persistentDataPath + "/data.json";

        if (File.Exists(Application.persistentDataPath + "/data.json"))
        {
            using StreamReader reader = new(Application.persistentDataPath + "/data.json");
            LevelData data = LevelData.Load(reader.ReadToEnd());
            gameTime = data.CurrentTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        updateTime();
        updateXpBar();
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetPauseVisibility(!paused);

        }
        if (PlayerMovment.Instance.health <= 2)
        {
            heart3.sprite = noHp;
        }
        if (PlayerMovment.Instance.health <= 1)
        {
            heart2.sprite = noHp;
        }
        if (PlayerMovment.Instance.health == 0)
        {
            heart1.sprite = noHp;
        }
        if (PlayerMovment.Instance.health == 0)
        {
            Time.timeScale = 0;
            loseScreen.enabled = true;

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
        }
        if (Time.timeScale == 0)
        {
            gameObject.GetComponent<AudioSource>().Pause();
        }
        else
        {
            gameObject.GetComponent<AudioSource>().UnPause();
        }
    }

    public void SetPauseVisibility(bool visible)
    {
        if (PlayerMovment.Instance.health <= 0)
        {
            return;
        }

        paused = visible;
        Time.timeScale = visible ? 0 : 1;
        pauseScreen.enabled = visible;
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveAndQuit()
    {
        LevelData data = new()
        {
            PlayerLocation = FindObjectOfType<PlayerMovment>().transform.position,
            PlayerHealth = FindObjectOfType<PlayerMovment>().health,
            PlayerDamage = FindObjectOfType<PlayerMovment>().damage,
            PlayerLevel = FindObjectOfType<PlayerMovment>().level,
            PlayerXpToNextLevel = FindObjectOfType<PlayerMovment>().xpToNextLevel,
            PlayerCurrentXp = FindObjectOfType<PlayerMovment>().currentXp,
            CurrentTime = gameTime,
            SkeletonMaxHp = PlayerMovment.Instance.gameObject.GetComponentInChildren<EnemySpawning>().skeltonHealth,
            NecromancerMaxHp = PlayerMovment.Instance.gameObject.GetComponentInChildren<EnemySpawning>().necromancerHealth,
            SkeletonGroupSize = PlayerMovment.Instance.gameObject.GetComponentInChildren<EnemySpawning>().skeletonGroup,
            NecromancerGroupSize = PlayerMovment.Instance.gameObject.GetComponentInChildren<EnemySpawning>().necromancerGroup
        };

        foreach (var skeleton in FindObjectsOfType<Skeleton>())
        {
            data.SkeletonLocations.Add(skeleton.transform.position);
        }

        foreach (var necromancer in FindObjectsOfType<Necromancer>())
        {
            data.NecromancerLocations.Add(necromancer.transform.position);
        }

        using StreamWriter saveFile = new(savePath, false);
        string json = JsonUtility.ToJson(data);
        saveFile.Write(json);

        Quit();
    }

    private void updateTime()
    {

        float minutes = Mathf.FloorToInt(gameTime / 60);
        float seconds = Mathf.FloorToInt(gameTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void updateXpBar()
    {
        float curXp = PlayerMovment.Instance.currentXp;
        float xpNeeded = PlayerMovment.Instance.xpToNextLevel;
        float xpPercent = curXp / xpNeeded;
        xpBar.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 510 - (xpPercent * 500), xpPercent * 500);
        xpHighlight.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 510 - (xpPercent * 500), xpPercent * 500);
        level.text = PlayerMovment.Instance.level.ToString();
    }
}
