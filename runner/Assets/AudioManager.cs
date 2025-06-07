using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip bgm01;
    //public AudioClip bgm02;
    //public AudioClip jump;
    //public AudioClip crouch;
    //public AudioClip die;

    public AudioMixer AudioMixer;
    public UnityEngine.UI.Slider BGMSlider;
    public UnityEngine.UI.Slider SFXSlider;

    public Data data;
    public LogicScript ls;

    private bool isStopping;
    private float stopDuration = 1.0f;

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

        ls = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
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

            // Gradually decrease pitch over time
            BGMSource.pitch = Mathf.Lerp(startPitch, 0.0f, t);

            // Optionally decrease volume (for realism)
            BGMSource.volume = Mathf.Lerp(1.0f, 0.0f, t);

            yield return null;
        }

        BGMSource.pitch = 0.0f;
        BGMSource.Stop();
        isStopping = false;
    }
}
