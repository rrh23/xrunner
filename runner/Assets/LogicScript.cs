using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    //public int playerScore;
    //public TMP_Text text;
    public GameObject player;
    public GameObject gameOverScreen;
    public AudioManager audioManager;
    public Spawner spawner;

    public static LogicScript Instance;
    public SaveData saveData;

    public Transform target;
    public float speed = 100f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public float currentScore;
    public bool isPlaying = false;

    private void Start()
    {
        isPlaying = true;
        saveData = new SaveData();
        currentScore = 0;
    }
    private void Update()
    {
        if (isPlaying)
        {
            currentScore += Time.deltaTime;
        }

        if (target != null) 
        {
            target.transform.Translate(Vector3.left * (speed * 100) * Time.deltaTime);
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

    public void GameOver()
    {
        Destroy(player);
        gameOverScreen.SetActive(true);
        audioManager.stopBGM();
        isPlaying = false;

        //update highscore
        if (saveData.highscore < currentScore)
        {
            saveData.highscore = currentScore;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
