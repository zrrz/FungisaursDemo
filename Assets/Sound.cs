using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound{

    public string name;

    public AudioClip clip;

    [Range (0f, 1f)]
    public float volume = 1f;
    [Range (.1f, 3f)]
    public float pitch = 1f;

    public bool doRandomPitch;
    public float pitchRange;

    public bool loop;

    [System.NonSerialized]
    public AudioSource source;

    public enum SoundType
    {
        Music, FX, UI
    }

    public SoundType soundType;

}
