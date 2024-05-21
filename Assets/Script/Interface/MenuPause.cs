using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

namespace Script.Interface
{
  public class MenuPause : MonoBehaviour
  {
    [Header("Objetos de la interfaz")]
    [SerializeField] private GameObject menuGame;
    [SerializeField] private GameObject menuPause;
    [SerializeField] private GameObject menuOption;
    [SerializeField] private GameObject countDownTimer;
    
    [Header("Cancion para el manu de pausa")]
    [SerializeField] private AudioSource audioSource;
    
    [Header("Texto en pantalla")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float resumeDelay = 3f;
    
    private bool _isPaused;
    
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
      Time.timeScale = 0; // pause
      audioSource.Pause();
      menuGame.SetActive(true);
    }

    public void Resume()
    {
      menuOption.SetActive(false);
      menuPause.SetActive(false);
      countDownTimer.SetActive(true);
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
      audioSource.UnPause();
      menuGame.SetActive(false);
    }
    
    public void Restart()
    {
      _isPaused = false;
      Time.timeScale = 1;
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void Quit()
    {
      // Carga el menú principal
      SceneManager.LoadScene("MenuPrincipal");
    }
  }
}