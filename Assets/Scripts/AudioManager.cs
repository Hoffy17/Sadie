using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerSnapshot outsideSnapshot;
    [SerializeField] private AudioMixerSnapshot insideSnapshot;
    [SerializeField] private float transitionTime;

    public void GoInside()
    {
        insideSnapshot.TransitionTo(transitionTime);
    }

    public void GoOutside()
    {
        outsideSnapshot.TransitionTo(transitionTime);
    }
}
