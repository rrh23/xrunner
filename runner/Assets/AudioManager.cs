using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip bgm01;
    //public AudioClip bgm02;
    //public AudioClip jump;
    //public AudioClip crouch;
    //public AudioClip die;


    private bool isStopping;
    private float stopDuration = 1.0f;

    private void Start()
    {
        BGMSource.clip = bgm01;
        BGMSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void stopBGM()
    {
        BGMSource.clip = bgm01;
        BGMSource.Stop();
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
