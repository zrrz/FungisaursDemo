using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

    Dictionary<string, Sound> soundMap;

    AudioSource backgroundMusic;
    AudioSource uiSounds;
    AudioSource gameSounds;

    public bool muteSoundFX;
    public bool muteBackgroundMusic;
    
    void Awake () {

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
	}

    private void Start() {
        soundMap = new Dictionary<string, Sound>();

        backgroundMusic = gameObject.AddComponent<AudioSource>();
        uiSounds = gameObject.AddComponent<AudioSource>();
        gameSounds = gameObject.AddComponent<AudioSource>();

        foreach (Sound sound in sounds) {
            switch(sound.soundType) {
                case Sound.SoundType.Music:
                    sound.source = backgroundMusic;
                    break;
                case Sound.SoundType.FX:
                    sound.source = gameSounds;
                    break;
                case Sound.SoundType.UI:
                    sound.source = uiSounds;
                    break;
            }
            
            soundMap.Add(sound.name, sound);

            if (PlayerPrefs.HasKey("muteSoundFX")) {
                muteSoundFX = (PlayerPrefs.GetInt("muteSoundFX") == 1);
            } else {
                PlayerPrefs.SetInt("muteSoundFX", 0);
				PlayerPrefs.Save();
            }

            if (PlayerPrefs.HasKey("muteBackgroundMusic"))
            {
                muteBackgroundMusic = (PlayerPrefs.GetInt("muteBackgroundMusic") == 1);
            } else {
                PlayerPrefs.SetInt("muteBackgroundMusic", 0);
                PlayerPrefs.Save();
            }
        }

        Play("LoadingScreen_Theme");
    }

    public void Play (string name) {
        //Sound s = Array.Find(sounds, sound => sound.name == name);
        // if (s == null) {
        //     Debug.LogWarning("Sound: " + name + " not found!");
        // }
        Sound sound = soundMap[name];

        if(sound.soundType == Sound.SoundType.Music && muteBackgroundMusic == true) {
            return;
        } else if(sound.soundType == Sound.SoundType.FX && muteSoundFX == true) {
            return;
        } else if(sound.soundType == Sound.SoundType.UI && muteSoundFX == true) {
            return;
        }

        sound.source.clip = sound.clip;

        sound.source.volume = sound.volume;
        if(sound.doRandomPitch == true) {
            sound.source.pitch = sound.pitch + UnityEngine.Random.Range(-sound.pitchRange, sound.pitchRange);
        } else {
            sound.source.pitch = sound.pitch;
        }
        sound.source.loop = sound.loop;
        sound.source.Play();
    }

    public void ToggleMuteMusic() {
        muteBackgroundMusic = !muteBackgroundMusic;

        PlayerPrefs.SetInt("muteBackgroundMusic", 0);
        PlayerPrefs.Save();
    }

    public void ToggleMuteSoundFX() {
        muteSoundFX = !muteSoundFX;

        PlayerPrefs.SetInt("muteSoundFX", 0);
        PlayerPrefs.Save();
    }
}
