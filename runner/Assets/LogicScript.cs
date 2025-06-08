using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] public TextMeshProUGUI gameOverScore;
    [SerializeField] public TextMeshProUGUI gameOverHighscore;
    //public int playerScore;
    //public TMP_Text text;
    public GameObject player;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject transition;
    public GameObject playerCanvas;
    public AudioManager audioManager;
    public AudioSource bgmSource;
    public Spawner spawner;

    public static LogicScript Instance;
    public Data data;

    //public Transform target;
    //private float speed = 100f;
    public bool isEDCollected;
    public bool isPaused;

    private Image img, playerimg;
    public float transitionAmt;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public float currentScore;
    public bool isPlaying = false;

    private void Start()
    {
        player.SetActive(true);
        isPlaying = true;
        currentScore = 0;

        string loadedData = SaveSystem.Load("save");
        if(loadedData != null )
        {
            data = JsonUtility.FromJson<Data>(loadedData);
        }
        else
        {
            data = new Data();
        }

        transition.SetActive(true);
        img = transition.GetComponent<Image>();
        playerimg = playerCanvas.GetComponentInChildren<Image>();
        transitionAmt = 1;

        //set volume = settings
        Settings();
    }
    private void Update()
    {
        //transition
        if (transitionAmt >= 0)
        {
            playerimg.color = Color.clear;
            img.fillAmount = transitionAmt;
            transitionAmt -= Time.deltaTime * 2.2f;
        }
        else
        {
            transition.SetActive (false);

            //super janky method of turning on the health & stamina bars after the transition
            //because the layering behaves kinda weirdly
            //idk
            //playerCanvas.transform.position = new Vector3(transform.position.x, transform.position.y, -2);
            playerimg.color = Color.white;
        }

        if (isPlaying)
        {
            currentScore += Time.deltaTime;
        }

        //if (target != null) 
        //{
        //    target.transform.Translate(Vector3.left * (speed * 1) * Time.deltaTime);
        //}

        if (isPaused)
        {
            // Pause the game
            Time.timeScale = 0f;
            bgmSource.Pause();
            pauseScreen.SetActive(true);
            //Debug.Log("Game Paused");
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MainMenu();
            }

        }
        else if (!isPaused)
        {
            // Resume the game
            Time.timeScale = 1f;
            bgmSource.UnPause();
            isPaused = false;
            pauseScreen.SetActive(false);
            //Debug.Log("Game Resumed");
        }
    }
    private void OnGUI()
    {
        score.text = RoundedScore();
    }

    public string RoundedScore()
    {
        return Mathf.RoundToInt(currentScore).ToString();
    }

    public string RoundedHighScore()
    {
        return Mathf.RoundToInt(data.highscore).ToString();
    }

    public void GameOver()
    {
        player.SetActive(false);
        gameOverScreen.SetActive(true);
        audioManager.stopBGM();
        isPlaying = false;

        //update highscore
        if (data.highscore < currentScore)
        {
            data.highscore = currentScore;
            SaveData(data);
        }

        //save scores
        gameOverScore.text = "Score: " + RoundedScore();
        gameOverHighscore.text = "Highscore: " + RoundedHighScore();

    }
    
    public void Resume()
    {
        isPaused = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Pause()
    {
        isPaused = true;
        
    }


    public void SaveVolumeData()
    {
        //save pref to data
        data.SFXvolume = audioManager.SFXSlider.value;
        data.BGMvolume = audioManager.BGMSlider.value;
        SaveData(data);
    }

    public void Settings()
    {
        //load the volume's data
        audioManager.BGMSlider.value = data.BGMvolume;
        audioManager.SFXSlider.value = data.SFXvolume;
    }

    public Data SaveData(Data data)
    {
        //save
        string saveString = JsonUtility.ToJson(data);
        SaveSystem.Save("save", saveString);
        return data;
    }
}
