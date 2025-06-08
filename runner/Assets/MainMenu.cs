using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Data data;
    public AudioManager audioManager;
    public GameObject transition;
    private Image img;
    public float transitionAmt;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        transitionAmt = 0f;
        string loadedData = SaveSystem.Load("save");
        if (loadedData != null)
        {
            data = JsonUtility.FromJson<Data>(loadedData);
        }
        else
        {
            data = new Data();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playGame()
    {
        transition.SetActive(true);
        playTransition();
    }

    public void playTransition()
    {
        StartCoroutine(Transition());

        //if(transitionAmt >= 1) SceneManager.LoadScene(1);
    }

    private IEnumerator Transition()
    {
        img.fillAmount = 0f;
        img = transition.GetComponent<Image>();
        //transition
        while (transitionAmt <= 1)
        {
            img.fillAmount = transitionAmt;
            transitionAmt += Time.deltaTime * 2.2f;
        }
        if(transitionAmt >=1) transition.SetActive(false);
        yield return null;
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

    public void QuitGame()
    {
        Application.Quit();
    }
    public Data SaveData(Data data)
    {
        //save
        string saveString = JsonUtility.ToJson(data);
        SaveSystem.Save("save", saveString);
        return data;
    }
}
