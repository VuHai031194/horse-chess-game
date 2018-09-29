using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundController : MonoBehaviour {

    public static SoundController main;

    [HideInInspector]
    public bool isSound;
    
    public AudioSource audioSound;
    public AudioSource audioMusic;

    public Image[] imgSounds;
    public Sprite sprSoundOn, sprSoundOff;

    public AudioClip audBackground, audPress, audClock, audMove;

    void Awake()
    {
        main = this;
        if (PlayerPrefs.GetInt("SoundData") == 0)
        {
            isSound = true;
            PlayerPrefs.SetInt("SoundData", 1);
        }
        else
        {
            isSound = PlayerPrefs.GetInt("Sound") == 1 ? true : false;
        }

        if (isSound)
        {
            SoundPlay("background");
        }

        SetSound();
    }

    public void SoundPlay(string soundName)
    {
        switch (soundName)
        {
            case "background":
                //audioSound.Stop();
                audioMusic.clip = audBackground;
                audioMusic.volume = 0.2f;
                break;
            case "clock":
                audioMusic.clip = audClock;
                audioMusic.volume = 0.2f;
                break;
            case "press":
                audioSound.clip = audPress;
                audioSound.volume = 0.1f;
                break;
            case "move":
                audioSound.clip = audMove;
                audioSound.volume = 0.7f;
                break;
            default: Debug.LogError("Not Exits");
                break;
        }

        if (isSound)
        {
            if (soundName.Equals("background") || soundName.Equals("clock"))
            {
                audioMusic.loop = true;
                audioMusic.time = 0;
                audioMusic.Play();
            }
            else
            {
                audioSound.loop = false;
                audioSound.Play();
            }  
        }
        
    }

    public void SetSound()
    {
        for (int i = 0; i < imgSounds.Length; i++)
            imgSounds[i].sprite = isSound ? sprSoundOn : sprSoundOff;

        RunSound(isSound);

        PlayerPrefs.SetInt("Sound", isSound ? 1 : 0);
    }

    public void OnSoundClick()
    {
        isSound = !isSound;
        SetSound();
    }

    public void RunSound()
    {
        RunSound(true);
    }

    public void PauseSound()
    {
        RunSound(false);
    }

    //[System.Obsolete("Use RunSound Instead")]
    public void RunSound(bool isPlay)
    {
        if (isPlay)
        {
            audioMusic.volume = 0.3f;
            audioSound.volume = 0.5f;
        }
        else
        {
            audioMusic.volume = 0;
            audioSound.volume = 0;
        }
    }
}
