using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Data data;
    public AudioManager audioManager;
    public GameObject transition;
    public GameObject postProcessing;
    private Image img;
    public float transitionAmt;
    public Bloom bloom;
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
        Volume volume = postProcessing.GetComponent<Volume>();
    }

    public void playGame()
    {
        transition.SetActive(true);
        playTransition();
    }

    public void playTransition()
    {
        StartCoroutine(Transition());
    }

    public void turnOnBloom()
    {
        var volume = postProcessing.GetComponent<Volume>();
        if (volume.profile.TryGet<Bloom>(out var bloom))
        {
            bloom.intensity.value = 9.3f;
        }
    }
    public void turnOffBloom()
    {
        var volume = postProcessing.GetComponent<Volume>();
        if (volume.profile.TryGet<Bloom>(out var bloom))
        {
            bloom.intensity.value = 0f;
        }
    }

    private IEnumerator Transition()
    {
        img = transition.GetComponent<Image>();
        img.fillAmount = 0f;
        transitionAmt = 0f;
        while (transitionAmt <= 1)
        {
            img.fillAmount = transitionAmt;
            transitionAmt += Time.deltaTime * 2.2f;
            yield return null;
        }
        img.fillAmount = 1f;
        //transition.SetActive(false);
        SceneManager.LoadScene(1);
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
