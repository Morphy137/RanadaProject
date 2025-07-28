using System.Collections;
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
    [SerializeField] private AudioClip hoverSOUND;

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
      // Improved Singleton pattern for Unity 6
      if (Instance == null)
      {
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
      }
      else if (Instance != this)
      {
        Destroy(gameObject); // Destroy duplicate instances
        return;
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

    private void Start()
    {
      // Cargar y aplicar valores guardados
      LoadVolumeSettings();

      // Conectar sliders si existen en la escena actual
      ConnectSliders();
    }

    private void LoadVolumeSettings()
    {
      // Cargar valores guardados
      bgmVolume = PlayerPrefs.GetFloat("BGM", 0.8f);
      sfxVolume = PlayerPrefs.GetFloat("SFX", 0.8f);

      // Aplicar volúmenes a los AudioSources
      if (bgmSource != null) bgmSource.volume = bgmVolume;
      if (sfxSource != null) sfxSource.volume = sfxVolume;
      if (menuSource != null) menuSource.volume = bgmVolume;
      if (audioSource != null) audioSource.volume = sfxVolume;
    }

    private void ConnectSliders()
    {
      // Resetear referencias de sliders al cambiar de escena
      bgmSlider = null;
      sfxSlider = null;

      // Buscar sliders en toda la jerarquía, incluyendo objetos inactivos
      bgmSlider = FindSliderInHierarchy("SliderBGM");
      sfxSlider = FindSliderInHierarchy("SliderSFX");

      // También buscar por nombres alternativos comunes
      if (bgmSlider == null)
      {
        bgmSlider = FindSliderInHierarchy("MusicSlider") ?? FindSliderInHierarchy("BGM");
      }

      if (sfxSlider == null)
      {
        sfxSlider = FindSliderInHierarchy("SoundSlider") ?? FindSliderInHierarchy("SFX");
      }

      // Actualizar valores de sliders si existen
      if (bgmSlider != null)
      {
        bgmSlider.value = bgmVolume;
        bgmSlider.onValueChanged.RemoveAllListeners();
        bgmSlider.onValueChanged.AddListener(ChangeBGMVolume);
        Debug.Log("BGM Slider conectado correctamente");
      }
      else
      {
        Debug.LogWarning("BGM Slider no encontrado en la escena");
      }

      if (sfxSlider != null)
      {
        sfxSlider.value = sfxVolume;
        sfxSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        Debug.Log("SFX Slider conectado correctamente");
      }
      else
      {
        Debug.LogWarning("SFX Slider no encontrado en la escena");
      }
    }

    private Slider FindSliderInHierarchy(string sliderName)
    {
      // Buscar en todos los objetos, incluyendo inactivos
      GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

      foreach (GameObject obj in allObjects)
      {
        // Solo buscar en objetos de la escena actual (no assets o prefabs)
        if (obj.scene.isLoaded && obj.name.Contains(sliderName))
        {
          Slider slider = obj.GetComponent<Slider>();
          if (slider != null)
          {
            Debug.Log($"Slider encontrado: {obj.name} en {GetGameObjectPath(obj)}");
            return slider;
          }
        }
      }

      // Búsqueda alternativa usando FindObjectsByType (Unity 6)
      Slider[] allSliders = FindObjectsByType<Slider>(FindObjectsSortMode.None);
      foreach (Slider slider in allSliders)
      {
        if (slider.gameObject.name.Contains(sliderName))
        {
          Debug.Log($"Slider encontrado (método alternativo): {slider.gameObject.name}");
          return slider;
        }
      }

      return null;
    }

    private string GetGameObjectPath(GameObject obj)
    {
      string path = obj.name;
      Transform parent = obj.transform.parent;

      while (parent != null)
      {
        path = parent.name + "/" + path;
        parent = parent.parent;
      }

      return path;
    }

    public void ChangeBGMVolume(float value)
    {
      // Actualizar variable local
      bgmVolume = value;

      // Aplicar a todos los AudioSources de música
      if (bgmSource != null) bgmSource.volume = value;
      if (menuSource != null) menuSource.volume = value;

      // Guardar en PlayerPrefs
      PlayerPrefs.SetFloat("BGM", value);
      PlayerPrefs.Save();

      Debug.Log($"BGM Volume changed to: {value}");
    }

    public void ChangeSFXVolume(float value)
    {
      // Actualizar variable local
      sfxVolume = value;

      // Aplicar a todos los AudioSources de efectos
      if (sfxSource != null) sfxSource.volume = value;
      if (audioSource != null) audioSource.volume = value;

      // Guardar en PlayerPrefs
      PlayerPrefs.SetFloat("SFX", value);
      PlayerPrefs.Save();

      Debug.Log($"SFX Volume changed to: {value}");
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      // Cargar configuración de volumen en cada escena
      LoadVolumeSettings();

      // Reconectar sliders en la nueva escena
      StartCoroutine(ConnectSlidersDelayed());

      // Cambia la música de fondo dependiendo de la escena
      switch (scene.name)
      {
        case "MenuPrincipal":
          if (bgmSource != null)
          {
            bgmSource.clip = menuMUSIC;
            bgmSource.loop = true;
            bgmSource.volume = bgmVolume; // Aplicar volumen guardado
            bgmSource.Play();
          }
          break;
        case "Game":
          if (bgmSource != null)
          {
            bgmSource.clip = gameMUSIC;
            bgmSource.loop = false;
            bgmSource.volume = bgmVolume; // Aplicar volumen guardado
            bgmSource.Play();
          }
          break;
        case "Credits":
          if (bgmSource != null)
          {
            bgmSource.clip = menuMUSIC;
            bgmSource.loop = true;
            bgmSource.volume = bgmVolume; // Aplicar volumen guardado
            bgmSource.Play();
          }
          break;
        case "ScoreScreen":
          if (bgmSource != null)
          {
            bgmSource.clip = menuMUSIC;
            bgmSource.loop = true;
            bgmSource.volume = bgmVolume; // Aplicar volumen guardado
            bgmSource.Play();
          }
          break;
        default:
          Debug.Log("No se ha asignado música a la escena");
          break;
      }
    }

    private IEnumerator ConnectSlidersDelayed()
    {
      // Esperar más tiempo para que Manager1 y otros objetos se inicialicen
      yield return new WaitForSeconds(0.1f);
      ConnectSliders();

      // Si no se encontraron sliders, intentar de nuevo después de un delay adicional
      if (!AreSlidersConnected())
      {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Reintentando conexión de sliders...");
        ConnectSliders();
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

    public void PlayHoverSound()
    {
      sfxSource.PlayOneShot(hoverSOUND);
    }

    public void ResetAudioSource()
    {
      if (bgmSource != null)
      {
        bgmSource.Stop();
        bgmSource.Play();
      }
    }

    public float GetLengthGameAudioClip()
    {
      return gameMUSIC.length;
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

    // Método público para reconectar sliders desde otros scripts
    public void RefreshSliders()
    {
      ConnectSliders();
    }

    // Método específico para cuando Manager1 se inicializa
    public void OnManagerInitialized()
    {
      StartCoroutine(ConnectSlidersDelayed());
    }

    // Método para verificar si los sliders están conectados
    public bool AreSlidersConnected()
    {
      return bgmSlider != null && sfxSlider != null;
    }

    private void OnDestroy()
    {
      // Cleanup para evitar memory leaks
      SceneManager.sceneLoaded -= OnSceneLoaded;

      if (bgmSlider != null)
        bgmSlider.onValueChanged.RemoveAllListeners();

      if (sfxSlider != null)
        sfxSlider.onValueChanged.RemoveAllListeners();
    }
  }
}