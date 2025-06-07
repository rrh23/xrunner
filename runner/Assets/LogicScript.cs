using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] public TextMeshProUGUI gameOverScore;
    [SerializeField] public TextMeshProUGUI gameOverHighscore;
    //public int playerScore;
    //public TMP_Text text;
    public GameObject player;
    public GameObject gameOverScreen;
    public AudioManager audioManager;
    public Spawner spawner;

    public static LogicScript Instance;
    public Data data;

    //public Transform target;
    public float speed = 100f;
    public bool isEDCollected;

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
        data = new Data();
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
    }
    private void Update()
    {
        if (isPlaying)
        {
            currentScore += Time.deltaTime;
        }

        //if (target != null) 
        //{
        //    target.transform.Translate(Vector3.left * (speed * 1) * Time.deltaTime);
        //}
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

            //save
            string saveString = JsonUtility.ToJson(data);
            SaveSystem.Save("save", saveString);
        }

        //save scores
        gameOverScore.text = "Score: " + RoundedScore();
        gameOverHighscore.text = "Highscore: " + RoundedHighScore();

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
