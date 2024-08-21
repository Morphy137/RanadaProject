
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

namespace Script.Interface
{
  public class MenuPause : MonoBehaviour
  {
    [Header("Objetos de la interfaz")]
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject menuPause;
    [SerializeField] private GameObject menuOption;
    [SerializeField] private GameObject countDownTimer;
    
    private AudioSource bgmSource;
    private AudioSource menuSource;
    private Coroutine volumeCoroutine;
    
    [Header("Texto en pantalla")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float resumeDelay = 3f;
    
    private bool _isPaused;

    private void Start()
    {
      menuSource = SoundManager.Instance.GetMenuSource();
      bgmSource = SoundManager.Instance.GetBgmSource();
    }
    

    public void TogglePause()
    {
      if (_isPaused)
      {
        Resume();
      }
      else
      {
        Pause();
      }
    }
    
    public void Pause()
    {
      _isPaused = true;
      Time.timeScale = 0; // pause el juego
      bgmSource.Pause(); // Pausa la música de fondo
      
      // Musica del menu de pausa
      menuSource.volume = 0; // Asegúrate de que el volumen inicial es 0
      menuSource.Play();
      StartCoroutine(SoundManager.Instance.AdjustVolumeOverTime(menuSource, 0.1f, 0.5f));      
      
      // Muestra el menú de pausa
      menuPause.SetActive(true);
      pauseButton.SetActive(false);
    }

    public void Resume()
    {
      menuOption.SetActive(false);
      menuPause.SetActive(false);
      countDownTimer.SetActive(true);
      volumeCoroutine = StartCoroutine(SoundManager.Instance.AdjustVolumeOverTime(menuSource, -0.4f, 0));
      StartCoroutine(ResumeAfterDelay(resumeDelay));
    }

    private IEnumerator ResumeAfterDelay(float delay)
    {
      countdownText.gameObject.SetActive(true);
      
      float countdown = delay;
      while (countdown > 0)
      {
        countdownText.text = countdown.ToString("0");
        yield return new WaitForSecondsRealtime(1f);
        countdown--;
      }
      
      countdownText.text = "DALEE!";
      yield return new WaitForSecondsRealtime(0.5f);
      
      countdownText.gameObject.SetActive(false);
      
      _isPaused = false;
      Time.timeScale = 1; // resume
      
      // Musica
      bgmSource.UnPause();
      menuSource.Stop();
      
      // Solucion bug de volumen disminuyendo infinito
      if (volumeCoroutine != null)
      {
        StopCoroutine(volumeCoroutine);
        volumeCoroutine = null;
      }
      
      // Menu
      menuPause.SetActive(false);
      pauseButton.SetActive(true);
    }
    
    public void Restart()
    {
      _isPaused = false;
      Time.timeScale = 1;
      
      //Musica detener
      menuSource.Stop();
      bgmSource.Stop();
      
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void Quit()
    {
      menuSource.Stop();
      _isPaused = false;
      Time.timeScale = 1;
      
      // Carga el menú principal
      SceneManager.LoadScene("MenuPrincipal");
    }
  }
}