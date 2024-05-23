﻿using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Script.Interface
{
  public class SoundManager : MonoBehaviour
  {
    [Header("Sliders")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    
    [Header("AudioSource")]
    [SerializeField] private AudioSource bgmSource; // AudioSource para la música de fondo
    [SerializeField] private AudioSource sfxSource; // AudioSource para los efectos de sonido
    [SerializeField] private AudioSource audioSource;
    
    [Header("Musica asignables")]
    [SerializeField] private AudioClip menuMUSIC; // Música del menú
    [SerializeField] private AudioClip gameMUSIC; // Música del juego
    [SerializeField] private AudioClip pauseMUSIC; // Música de pausa y loadingScreen
    
    [Header("Efectos de sonido")]
    [SerializeField] private AudioClip clickSOUND; // sonido de click jijija
    [SerializeField] private AudioClip voiceSOUND; // proximamente supongo
    
    [Header("Variables de Valor")]
    private float bgmVolume;
    private float sfxVolume;
    
    public static SoundManager Instance { get; private set; }
    public AudioSource GetAudioSource() { return audioSource; }
    
    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
      }
      
      audioSource = GetComponent<AudioSource>();
      if (audioSource == null)
      {
        audioSource = gameObject.AddComponent<AudioSource>();
      }
      
      audioSource.clip = clickSOUND;
      SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void Start()
    {
      // Valores iniciales de los sliders
      bgmVolume = PlayerPrefs.GetFloat("BGM", 0.8f);
      sfxVolume = PlayerPrefs.GetFloat("SFX", 0.8f);
      
      bgmSlider.value = bgmVolume;
      sfxSlider.value = sfxVolume;

      bgmSource.volume = bgmVolume;
      sfxSource.volume = sfxVolume;
    }

    public void ChangeBGMVolume(float value)
    {
      // Cambia el volumen de la música de fondo
      PlayerPrefs.SetFloat("BGM", value);
      bgmSource.volume = value;
      PlayerPrefs.Save();
    }

    public void ChangeSFXVolume(float value)
    {
      // Volumen de los efectos de sonido
      PlayerPrefs.SetFloat("SFX", value);
      sfxSource.volume = value;
      PlayerPrefs.Save();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      // Cambia la música de fondo dependiendo de la escena
      switch (scene.name)
      {
        case "MenuPrincipal":
          bgmSource.clip = menuMUSIC;
          bgmSource.loop = true;
          bgmSource.Play();
          break;
        case "Game":
          bgmSource.clip = gameMUSIC;
          break;
        case "Credits":
          bgmSource.clip = menuMUSIC;
          bgmSource.Play();
          break;
        default:
          Debug.Log("No se ha asignado música a la escena");
          break;
      }
      
    }
    
    public AudioSource GetBgmSource() 
    {
      return bgmSource;
    }

    public void PlayPauseMusic()
    {
      bgmSource.clip = pauseMUSIC;
      bgmSource.loop = true;
      bgmSource.Play();
    }

    public void PlayClickSound()
    {
      audioSource.PlayOneShot(clickSOUND);
    }
  }
}