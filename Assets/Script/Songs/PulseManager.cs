using System;
using Script.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace Script.Songs
{
  public class PulseManager : MonoBehaviour
  {
    [SerializeField] private float bpm;
    [SerializeField] private Intervals[] intervals;

    private PulseManager instance;
    private AudioSource soundManagerAudioSource;

    private void Awake()
    {
      if (instance == null)
      {
        instance = this;
      }
      
      soundManagerAudioSource = SoundManager.Instance.GetBgmSource();
    }

    private void Update()
    {
      foreach (var interval in intervals)
      {
        float sampledTime = (soundManagerAudioSource.timeSamples / (soundManagerAudioSource.clip.frequency * interval.GetIntervalLength(bpm)));
        interval.CheckForNewInterval(sampledTime);
      }
    }
  }

  [System.Serializable]
  public class Intervals
  {
    [SerializeField] private float steps;
    [SerializeField] private UnityEvent trigger;
    private int lastInterval;

    public float GetIntervalLength(float bpm)
    {
      return 60 / (bpm * steps);
    }

    public void CheckForNewInterval(float interval)
    {
      // redondea hacia abajo el intervalo
      if (Mathf.FloorToInt(interval) != lastInterval)
      {
        lastInterval = Mathf.FloorToInt(interval);
        trigger.Invoke();
      }
    }
  }
}