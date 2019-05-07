using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAssets : MonoBehaviour
{
    public Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
    public AudioClip shootSound;

    private void Awake()
    {
        shootSound = (AudioClip)Resources.Load("Assets/sounds/shootSound.mp3", typeof(AudioClip));
        sounds.Add("shootSound", shootSound);

    }
}
