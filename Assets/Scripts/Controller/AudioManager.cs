using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSorce;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backGround;
    public AudioClip coinClip;  
    public AudioClip walkClip;

    void Start()
    {
        musicSorce.clip = backGround;
        musicSorce.loop = true;
        musicSorce.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
