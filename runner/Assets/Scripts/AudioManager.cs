using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource BGMSource;
    [SerializeField] public AudioSource SFXSource;
    [SerializeField] public AudioSource FlySource;

    public AudioClip bgm01;
    //public AudioClip bgm02;
    public AudioClip fly, crouch, heal, death, click, hurt;

    public AudioMixer AudioMixer;
    public UnityEngine.UI.Slider BGMSlider;
    public UnityEngine.UI.Slider SFXSlider;

    public Data data;

    private bool isStopping;
    private float stopDuration = 1.0f;
    private float fadeDuration = 0.3f;
    public Coroutine fadeCoroutine;

    private void Start()
    {
        //[LOAD DATA]
        string loadedData = SaveSystem.Load("save");
        if (loadedData != null)
        {
            data = JsonUtility.FromJson<Data>(loadedData);
        }
        else
        {
            data = new Data();
        }

        //set volume to the one saved in data
        BGMSource.volume = data.BGMvolume;
        SFXSource.volume = data.SFXvolume;

        BGMSource.clip = bgm01;
        BGMSource.Play();

        //ls = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    public void startFade()
    {
        fadeCoroutine = StartCoroutine(FadeOut());
    }

    public void playSound(AudioClip soundName)
    {
        //initialize volumes
        BGMSource.volume = data.BGMvolume;
        SFXSource.volume = data.SFXvolume;
        FlySource.volume = 1f;
        
        if (soundName == fly)
        {
            FlySource.clip = fly;
            FlySource.Play();
        }
        else
        {
            SFXSource.clip = soundName;
            SFXSource.PlayOneShot(soundName);
        }

        if (!soundName) return;
    }
    
    public void stopBGM()
    {
        TapeStop();
    }

    public void SetBGMVolume()
    {
        float volume = BGMSlider.value;
        AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void TapeStop()
    {
        if (!isStopping)
            StartCoroutine(TapeStopCoroutine());
    }

    private IEnumerator TapeStopCoroutine()
    {
        isStopping = true;

        float time = 0f;
        float startPitch = BGMSource.pitch;

        while (time < stopDuration)
        {
            time += Time.deltaTime;
            float t = time / stopDuration;

            BGMSource.pitch = Mathf.Lerp(startPitch, 0.0f, t);
            BGMSource.volume = Mathf.Lerp(data.BGMvolume, 0.0f, t);

            yield return null;
        }

        BGMSource.pitch = 0.0f;
        BGMSource.Stop();
        isStopping = false;
    }
    
    IEnumerator FadeOut()
    {
        // SFXSource.volume = data.SFXvolume;
        float startVolume = FlySource.volume;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            FlySource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        FlySource.Stop();
        FlySource.volume = 1f;
        fadeCoroutine = null;
    }
}
