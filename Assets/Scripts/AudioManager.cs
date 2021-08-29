using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("World")]
    [SerializeField] private AudioSource ambRain;
    [SerializeField] private AudioMixerSnapshot outsideSnapshot;
    [SerializeField] private AudioMixerSnapshot insideSnapshot;
    [SerializeField] private float transitionTime;
    [Header("UI")]
    [SerializeField] private AudioSource sFXButtonSelect_High;
    [SerializeField] private AudioSource sFXButtonSelect_Low;

    //Pauses and unpauses the rain when the player pauses
    public void PauseRain()
    {
        ambRain.Pause();
    }

    public void UnpauseRain()
    {
        ambRain.Play();
    }

    //Controls the lowpass on ambient noise, when the player enters buildings
    public void GoInside()
    {
        insideSnapshot.TransitionTo(transitionTime);
    }

    //Returns sounds to their original level, when the player leaves buildings
    public void GoOutside()
    {
        outsideSnapshot.TransitionTo(transitionTime);
    }

    public void SFXSelectButton_High()
    {
        sFXButtonSelect_High.Play();
    }

    public void SFXSelectButton_Low()
    {
        sFXButtonSelect_Low.Play();
    }
}
