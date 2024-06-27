using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // Reference to the AudioMixer component
    [SerializeField] private Slider musicSlider; // Reference to the volume slider 
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if(PlayerPrefs.HasKey("musicValue"))
        {
            LoadVolume();
        }
        else
        {          
            SetMusicVolume();
            SetSfxVolume();
        }   

    }



    public void SetMusicVolume()
    {
        float volumeValue = musicSlider.value; // Get the value of the volume slider
        audioMixer.SetFloat("music", Mathf.Log10(volumeValue) * 20 ); // Set the volume of the AudioMixer
        PlayerPrefs.SetFloat("musicValue", volumeValue); // Save the volume value to PlayerPrefs
    }

    public void SetSfxVolume()
    {
        float volumeValue = sfxSlider.value; // Get the value of the volume slider
        audioMixer.SetFloat("sfx", Mathf.Log10(volumeValue) * 20); // Set the volume of the AudioMixer
        PlayerPrefs.SetFloat("sfxValue", volumeValue); // Save the volume value to PlayerPrefs
    }

    public void LoadVolume()    
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicValue"); // Get the volume value from PlayerPrefs
        sfxSlider.value = PlayerPrefs.GetFloat("sfxValue");
        SetMusicVolume(); // Set the volume using the volume slider value
        SetSfxVolume();
    }   
}
