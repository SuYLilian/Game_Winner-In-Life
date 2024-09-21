using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmAudio, sfAudio;
    public AudioClip[] sfClip;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void PlayClip_SF(AudioClip _clip)
    {
        sfAudio.PlayOneShot(_clip);
    }
}
