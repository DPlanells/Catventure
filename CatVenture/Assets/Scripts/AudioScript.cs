using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public  class AudioScript : MonoBehaviour
{

    public AudioSource audio1;
    public AudioSource audio2;


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("cambio musica");
        StartCoroutine(FadeOut(audio1, 3));
        StartCoroutine(FadeIn(audio2, 3));

    }
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        Debug.Log("Dentro out");
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            Debug.Log("Dentro while");
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

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }
}

