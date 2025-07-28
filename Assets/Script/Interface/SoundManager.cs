using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Script.Interface
{
  /// <summary>
  /// Gestor centralizado de audio del juego que maneja música de fondo, efectos de sonido y configuraciones de volumen.
  /// Implementa el patrón singleton y persiste entre escenas para mantener coherencia de audio.
  /// Controla automáticamente la música según la escena activa y proporciona métodos para ajustes de volumen.
  /// </summary>
  public class SoundManager : MonoBehaviour
  {
    #region Serialized Fields
    [Header("Controles de UI")]
    [Tooltip("Slider para controlar el volumen de la música de fondo")]
    [SerializeField] private Slider bgmSlider;

    [Tooltip("Slider para controlar el volumen de los efectos de sonido")]
    [SerializeField] private Slider sfxSlider;

    [Header("Fuentes de Audio")]
    [Tooltip("AudioSource dedicado para la música de fondo del juego")]
    [SerializeField] private AudioSource bgmSource;

    [Tooltip("AudioSource dedicado para efectos de sonido")]
    [SerializeField] private AudioSource sfxSource;

    [Tooltip("AudioSource dedicado para música de menús y pausas")]
    [SerializeField] private AudioSource menuSource;

    [Tooltip("AudioSource adicional para sonidos de interfaz como clicks")]
    [SerializeField] private AudioSource audioSource;

    [Header("Clips de Música")]
    [Tooltip("Música que se reproduce en los menús del juego")]
    [SerializeField] private AudioClip menuMUSIC;

    [Tooltip("Música principal que se reproduce durante el gameplay")]
    [SerializeField] private AudioClip gameMUSIC;

    [Tooltip("Música que se reproduce durante pausas y pantallas de carga")]
    [SerializeField] private AudioClip pauseMUSIC;

    [Header("Efectos de Sonido")]
    [Tooltip("Sonido que se reproduce al hacer click en elementos de UI")]
    [SerializeField] private AudioClip clickSOUND;

    [Tooltip("Sonido de voz (reservado para futuras implementaciones)")]
    [SerializeField] private AudioClip voiceSOUND;

    [Tooltip("Sonido que se reproduce al pasar el cursor sobre elementos interactivos")]
    [SerializeField] private AudioClip hoverSOUND;
    #endregion

    #region Private Fields
    /// <summary>Valor actual del volumen de música de fondo</summary>
    private float bgmVolume;

    /// <summary>Valor actual del volumen de efectos de sonido</summary>
    private float sfxVolume;
    #endregion

    #region Properties
    /// <summary>Instancia singleton del SoundManager para acceso global</summary>
    public static SoundManager Instance { get; private set; }

    /// <summary>Obtiene la referencia al AudioSource del menú</summary>
    public AudioSource GetMenuSource() { return menuSource; }

    /// <summary>Obtiene la referencia al AudioSource de música de fondo</summary>
    public AudioSource GetBgmSource() { return bgmSource; }

    /// <summary>Obtiene la referencia al AudioSource de efectos de sonido</summary>
    public AudioSource GetSfxSource() { return sfxSource; }

    /// <summary>Obtiene la referencia al AudioSource adicional</summary>
    public AudioSource AudioSource() { return audioSource; }
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Inicializa el singleton, configura los AudioSources y se suscribe a eventos de cambio de escena.
    /// Asigna clips de audio predeterminados y prepara el sistema de audio.
    /// </summary>
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

      // Asignamos la música predefinida
      menuSource.clip = pauseMUSIC;
      audioSource.clip = clickSOUND;

      SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Inicializa los valores de volumen desde PlayerPrefs y configura los sliders de UI.
    /// Aplica los valores guardados a los AudioSources correspondientes.
    /// </summary>
    private void Start()
    {
      // Valores iniciales de los sliders desde configuración guardada
      bgmVolume = PlayerPrefs.GetFloat("BGM", 0.8f);
      sfxVolume = PlayerPrefs.GetFloat("SFX", 0.8f);

      bgmSlider.value = bgmVolume;
      sfxSlider.value = sfxVolume;

      bgmSource.volume = bgmVolume;
      sfxSource.volume = sfxVolume;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Maneja el cambio automático de música cuando se carga una nueva escena.
    /// Asigna la música apropiada según el nombre de la escena cargada.
    /// </summary>
    /// <param name="scene">Escena que se ha cargado</param>
    /// <param name="mode">Modo de carga de la escena</param>
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
          bgmSource.loop = false;
          bgmSource.Play();
          break;
        case "Credits":
          bgmSource.clip = menuMUSIC;
          bgmSource.loop = true;
          bgmSource.Play();
          break;
        case "ScoreScreen":
          bgmSource.clip = menuMUSIC;
          bgmSource.loop = true;
          bgmSource.Play();
          break;
        default:
          Debug.Log("No se ha asignado música a la escena");
          break;
      }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Cambia el volumen de la música de fondo y guarda la configuración.
    /// </summary>
    /// <param name="value">Nuevo valor de volumen (0.0 - 1.0)</param>
    public void ChangeBGMVolume(float value)
    {
      // Cambia el volumen de la música de fondo
      PlayerPrefs.SetFloat("BGM", value);
      bgmSource.volume = value;
      PlayerPrefs.Save();
    }

    /// <summary>
    /// Cambia el volumen de los efectos de sonido y guarda la configuración.
    /// </summary>
    /// <param name="value">Nuevo valor de volumen (0.0 - 1.0)</param>
    public void ChangeSFXVolume(float value)
    {
      // Volumen de los efectos de sonido
      PlayerPrefs.SetFloat("SFX", value);
      sfxSource.volume = value;
      PlayerPrefs.Save();
    }

    /// <summary>
    /// Reproduce la música de pausa configurada.
    /// </summary>
    public void PlayPauseMusic()
    {
      audioSource.clip = pauseMUSIC;
      bgmSource.loop = true;
      bgmSource.Play();
    }

    /// <summary>
    /// Reproduce el sonido de click en los elementos de UI.
    /// </summary>
    public void PlayClickSound()
    {
      sfxSource.PlayOneShot(clickSOUND);
    }

    /// <summary>
    /// Reproduce el sonido de hover cuando el cursor pasa sobre elementos interactivos.
    /// </summary>
    public void PlayHoverSound()
    {
      sfxSource.PlayOneShot(hoverSOUND);
    }

    /// <summary>
    /// Reinicia el AudioSource de música de fondo, deteniéndolo y volviéndolo a reproducir.
    /// </summary>
    public void ResetAudioSource()
    {
      if (bgmSource != null)
      {
        bgmSource.Stop();
        bgmSource.Play();
      }
    }

    /// <summary>
    /// Obtiene la duración total del clip de música del juego.
    /// </summary>
    /// <returns>Duración en segundos del audio clip del juego</returns>
    public float GetLengthGameAudioClip()
    {
      return gameMUSIC.length;
    }

    /// <summary>
    /// Ajusta gradualmente el volumen de un AudioSource a lo largo del tiempo.
    /// </summary>
    /// <param name="audioSource">AudioSource cuyo volumen se modificará</param>
    /// <param name="volumeChangeSpeed">Velocidad de cambio (positiva para aumentar, negativa para disminuir)</param>
    /// <param name="targetVolume">Volumen objetivo a alcanzar</param>
    /// <returns>Corrutina que maneja el cambio gradual de volumen</returns>
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
    #endregion
  }
}