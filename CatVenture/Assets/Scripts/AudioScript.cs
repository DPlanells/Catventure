using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public  class AudioScript : MonoBehaviour
{

    public AudioSource audio;
    private AudioSource[] ListaAudio;


    public void OnTriggerEnter(Collider other)
    {
        ListaAudio = FindObjectsOfType<AudioSource>();
        Debug.Log("cambio musica");

        foreach (AudioSource a in ListaAudio) { 
            if(a.name != audio.name) { StartCoroutine(FadeOut(a, 10)); }
             }

        StartCoroutine(FadeIn(audio, 10));


    }

   
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        Debug.Log("Dentro out");
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 0.6f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 0.5f;
    }
}

