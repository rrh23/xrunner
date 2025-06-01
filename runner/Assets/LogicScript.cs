using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    //public int playerScore;
    //public TMP_Text text;
    public GameObject player;
    public GameObject gameOverScreen;
    public AudioManager audioManager;
    public Spawner spawner;

    //increase score


    public void GameOver()
    {
        Destroy(player);
        gameOverScreen.SetActive(true);
        audioManager.stopBGM();
        spawner.isSpawning = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
