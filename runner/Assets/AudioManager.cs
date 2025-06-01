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

    private void Start()
    {
        BGMSource.clip = bgm01;
        BGMSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
