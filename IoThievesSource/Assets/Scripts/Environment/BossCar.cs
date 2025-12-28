using System;
using System.Collections;
using UnityEngine;

public class BossCar : MonoBehaviour
{
    [SerializeField]
    private Animator carAnimator;
    [SerializeField]
    private AudioSource startingCarAudioSource;
    [SerializeField]
    private AudioSource runningCarAudioSource;

    public void InteractWithCar()
    {
        print("interacting with car");
        carAnimator.SetBool("IsStarting", true);
        startingCarAudioSource.Play();
    }

    private void Awake()
    {
        print("awake");
        // testing
        StartCoroutine(PlayInteractWithCar());
    }

    IEnumerator PlayInteractWithCar()
    {
        print("enumerator");
        yield return new WaitForSeconds(1f);
        startingCarAudioSource.Play();
        carAnimator.SetBool("IsStarting", true);
        yield return new WaitForSeconds(3f);
        // carAnimator.SetBool("IsStarting", false);
        runningCarAudioSource.Play();
        // yield return new WaitForSeconds(2f);
        // carAnimator.SetBool("IsStarting", false);
    }
}
