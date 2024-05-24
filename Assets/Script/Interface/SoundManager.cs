using System;
using System.Collections;
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
    [SerializeField] private AudioSource menuSource; // AudioSource para el menú
    [SerializeField] private AudioSource audioSource; // extra click sound
    
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
    
    //Getter
    public static SoundManager Instance { get; private set; }
    public AudioSource GetMenuSource() { return menuSource; }
    public AudioSource GetBgmSource() { return bgmSource; }
    public AudioSource GetSfxSource() { return sfxSource; }
    public AudioSource AudioSource() { return audioSource; }
    
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
      
      // Asignamos la musica predefinida
      menuSource.clip = pauseMUSIC;
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
          bgmSource.loop = true;
          bgmSource.Play();
          break;
        default:
          Debug.Log("No se ha asignado música a la escena");
          break;
      }
      
    }

    public void PlayPauseMusic()
    {
      audioSource.clip = pauseMUSIC;
      bgmSource.loop = true;
      bgmSource.Play();
    }

    public void PlayClickSound()
    {
      sfxSource.PlayOneShot(clickSOUND);
    }
    
    public IEnumerator AdjustVolumeOverTime(AudioSource audioSource, float volumeChangeSpeed, float targetVolume)
    {
      // Si volumeChangeSpeed es positivo, aumenta el volumen
      // Si volumeChangeSpeed es negativo, disminuye el volumen
      while ((volumeChangeSpeed > 0 && audioSource.volume < targetVolume) || (volumeChangeSpeed < 0 && audioSource.volume > targetVolume))
      {
        audioSource.volume += volumeChangeSpeed * Time.unscaledDeltaTime;
        yield return null;
      }

      // Asegúrate de que el volumen llegue a targetVolume
      audioSource.volume = targetVolume;
    }
  }
}