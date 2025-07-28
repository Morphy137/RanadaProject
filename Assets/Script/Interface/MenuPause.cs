using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

namespace Script.Interface
{
    /// <summary>
    /// Gestiona el sistema de pausa del juego incluyendo menús de pausa, cuenta regresiva de reanudación y control de audio.
    /// Implementa el patrón singleton para acceso global y maneja la transición suave entre estados de pausa y juego activo.
    /// Controla la música de fondo, efectos de volumen y la interfaz de usuario relacionada con la pausa.
    /// </summary>
    public class MenuPause : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Objetos de la interfaz")]
        [Tooltip("Botón de pausa visible durante el gameplay")]
        [SerializeField] private GameObject pauseButton;

        [Tooltip("Panel principal del menú de pausa")]
        [SerializeField] private GameObject menuPause;

        [Tooltip("Panel de opciones dentro del menú de pausa")]
        [SerializeField] private GameObject menuOption;

        [Tooltip("Panel que muestra la cuenta regresiva al reanudar")]
        [SerializeField] private GameObject countDownTimer;

        [Header("Texto en pantalla")]
        [Tooltip("Texto que muestra la cuenta regresiva numérica")]
        [SerializeField] private TextMeshProUGUI countdownText;

        [Tooltip("Duración en segundos de la cuenta regresiva antes de reanudar")]
        [SerializeField] private float resumeDelay = 3f;
        #endregion

        #region Private Fields
        /// <summary>Referencia al AudioSource de música de fondo</summary>
        private AudioSource bgmSource;

        /// <summary>Referencia al AudioSource de música de menú</summary>
        private AudioSource menuSource;

        /// <summary>Corrutina activa para ajuste de volumen</summary>
        private Coroutine volumeCoroutine;

        /// <summary>Estado actual de pausa del juego</summary>
        private static bool _isPaused;
        #endregion

        #region Properties
        /// <summary>Indica si el juego está actualmente pausado</summary>
        public static bool IsPaused => _isPaused;

        /// <summary>Instancia singleton del MenuPause para acceso global</summary>
        public static MenuPause Instance { get; private set; }
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa el patrón singleton asegurando una única instancia del MenuPause.
        /// Destruye duplicados si ya existe una instancia.
        /// </summary>
        private void Awake()
        {
            // Inicializamos la instancia singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject); // Si ya existe una instancia, destruye el duplicado
            }
        }

        /// <summary>
        /// Obtiene las referencias de los AudioSources del SoundManager al inicio.
        /// </summary>
        private void Start()
        {
            menuSource = SoundManager.Instance.GetMenuSource();
            bgmSource = SoundManager.Instance.GetBgmSource();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Alterna entre pausar y reanudar el juego según el estado actual.
        /// </summary>
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

        /// <summary>
        /// Pausa el juego, detiene el tiempo, pausa la música de fondo y muestra el menú de pausa.
        /// Inicia la reproducción de música de menú con efecto de fade-in.
        /// </summary>
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

        /// <summary>
        /// Inicia el proceso de reanudación del juego con cuenta regresiva.
        /// Oculta menús, reduce el volumen de la música de menú y activa el temporizador visual.
        /// </summary>
        public void Resume()
        {
            menuOption.SetActive(false);
            menuPause.SetActive(false);
            countDownTimer.SetActive(true);
            volumeCoroutine = StartCoroutine(SoundManager.Instance.AdjustVolumeOverTime(menuSource, -0.4f, 0));
            StartCoroutine(ResumeAfterDelay(resumeDelay));
        }

        /// <summary>
        /// Reinicia el nivel actual, restableciendo el estado del juego y deteniendo toda la música.
        /// Recarga la escena activa desde el principio.
        /// </summary>
        public void Restart()
        {
            _isPaused = false;
            Time.timeScale = 1;

            //Musica detener
            menuSource.Stop();
            bgmSource.Stop();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Sale del juego y regresa al menú principal.
        /// Detiene toda la música y restablece el estado del juego antes de cambiar de escena.
        /// </summary>
        public void Quit()
        {
            menuSource.Stop();
            _isPaused = false;
            Time.timeScale = 1;

            // Carga el menú principal
            SceneManager.LoadScene("MenuPrincipal");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Corrutina que maneja la cuenta regresiva visual antes de reanudar el juego.
        /// Muestra números descendentes y luego un mensaje de reanudación antes de reactivar el gameplay.
        /// </summary>
        /// <param name="delay">Tiempo en segundos de la cuenta regresiva</param>
        /// <returns>Corrutina que maneja la cuenta regresiva y reanudación</returns>
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

            countdownText.text = "DALE!";
            yield return new WaitForSecondsRealtime(0.5f);

            countdownText.gameObject.SetActive(false);

            _isPaused = false;
            Time.timeScale = 1; // resume

            // Musica
            bgmSource.UnPause();
            menuSource.Stop();

            // Solución bug de volumen disminuyendo infinito
            if (volumeCoroutine != null)
            {
                StopCoroutine(volumeCoroutine);
                volumeCoroutine = null;
            }

            // Menu
            menuPause.SetActive(false);
            pauseButton.SetActive(true);
        }
        #endregion
    }
}
