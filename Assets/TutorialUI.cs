using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using Script.Notes;

namespace Script.Interface
{
    /// <summary>
    /// Controlador de la interfaz de tutorial que gestiona la transición del estado inicial
    /// del juego hacia el gameplay activo. Utiliza el nuevo Input System de Unity para
    /// detectar cualquier entrada del jugador y activar el juego.
    /// </summary>
    public class PauseUntilKeyPress : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Canvas de Interfaz")]
        [Tooltip("Canvas que contiene el mensaje 'Presiona Para Continuar'")]
        public Canvas tutorialCanvas;

        [Tooltip("Canvas que muestra la puntuación durante el juego")]
        public Canvas ScoreUI;

        [Tooltip("Canvas que contiene el botón de pausa")]
        public Canvas PauseButton;
        #endregion

        #region Private Fields
        /// <summary>Indica si el juego ya ha sido iniciado</summary>
        private bool isStart;

        /// <summary>Referencia al sistema de entrada del jugador</summary>
        private PlayerInput playerInput;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa el estado del tutorial y configura las referencias del Input System.
        /// </summary>
        void Start()
        {
            isStart = false;
            ScoreUI.enabled = false;
            PauseButton.enabled = false;

            // Configurar el Input System para detectar cualquier entrada
            playerInput = new PlayerInput();

            // Suscribirse a todos los eventos de entrada disponibles
            playerInput.Player.Lane1Key.performed += OnAnyInput;
            playerInput.Player.Lane2Key.performed += OnAnyInput;
            playerInput.Player.Click.performed += OnAnyInput;
            playerInput.Player.Pause.performed += OnAnyInput;

            playerInput.Player.Enable();
        }

        /// <summary>
        /// Limpia las suscripciones al Input System cuando el objeto se destruye.
        /// </summary>
        void OnDestroy()
        {
            if (playerInput != null)
            {
                playerInput.Player.Lane1Key.performed -= OnAnyInput;
                playerInput.Player.Lane2Key.performed -= OnAnyInput;
                playerInput.Player.Click.performed -= OnAnyInput;
                playerInput.Player.Pause.performed -= OnAnyInput;

                playerInput.Player.Disable();
                playerInput.Dispose();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Maneja cualquier entrada del jugador durante el estado de tutorial.
        /// Se ejecuta cuando se detecta cualquier acción del Input System.
        /// </summary>
        /// <param name="context">Contexto de la acción de entrada</param>
        private void OnAnyInput(InputAction.CallbackContext context)
        {
            if (!isStart)
            {
                StartGame();
            }
        }

        /// <summary>
        /// Inicia el juego ocultando el tutorial y activando la interfaz de gameplay.
        /// Habilita los canvas necesarios e inicia la reproducción de la canción.
        /// </summary>
        void StartGame()
        {
            // Marcar el juego como iniciado
            isStart = true;

            // Gestionar visibilidad de los canvas
            tutorialCanvas.enabled = false;
            ScoreUI.enabled = true;
            PauseButton.enabled = true;

            // Iniciar la canción
            SongManager.Instance.InitSong();

            // Limpiar las suscripciones del Input System ya que no las necesitamos más
            if (playerInput != null)
            {
                playerInput.Player.Lane1Key.performed -= OnAnyInput;
                playerInput.Player.Lane2Key.performed -= OnAnyInput;
                playerInput.Player.Click.performed -= OnAnyInput;
                playerInput.Player.Pause.performed -= OnAnyInput;

                playerInput.Player.Disable();
                playerInput.Dispose();
                playerInput = null;
            }
        }
        #endregion
    }
}
