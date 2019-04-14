using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Will be implemented later
public class AudioManager : MonoBehaviour
{
    public AudioClip mainTheme;

    public AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayMaintheme()
    {
        source.Play();
    }
}
