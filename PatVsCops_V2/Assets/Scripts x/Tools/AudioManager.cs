using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip collectDiamond;


    void PlayAudioOneShot(AudioClip audioP)
    {
        audioSource.PlayOneShot(audioP);
    }
    
    public void PlayCollectDiamond()
    {
        PlayAudioOneShot(collectDiamond);
    }




    public void VolumeZero()
    {
        audioSource.volume = 0.0f;
    }
    public void VolumeOne()
    {
        audioSource.volume = 1.0f;
    }

}
