using UnityEngine;
using UnityEngine.InputSystem;
using Script.Notes;

namespace Script.Interface
{
    /// <summary>
    /// Controlador universal para transiciones activadas por cualquier entrada del jugador.
    /// Permite configurar listas de GameObjects que se activan/desactivan simultáneamente
    /// cuando se detecta cualquier input. Útil para "Presiona cualquier tecla para continuar".
    /// </summary>
    public class InputTransitionController : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Configuración de GameObjects")]
        [Tooltip("Lista de GameObjects que se DESACTIVARÁN cuando se detecte una entrada")]
        public GameObject[] objectsToDeactivate;

        [Tooltip("Lista de GameObjects que se ACTIVARÁN cuando se detecte una entrada")]
        public GameObject[] objectsToActivate;

        [Header("Configuración Adicional")]
        [Tooltip("Si está marcado, iniciará automáticamente SongManager.Instance.InitSong()")]
        public bool initSongOnTransition = true;

        [Tooltip("Si está marcado, el script se ejecutará solo una vez y luego se deshabilitará")]
        public bool executeOnlyOnce = true;
        #endregion

        #region Private Fields
        /// <summary>Indica si la transición ya ha sido ejecutada</summary>
        private bool transitionExecuted = false;

        /// <summary>Referencia al sistema de entrada del jugador</summary>
        private PlayerInput playerInput;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa el sistema y configura el Input System para detectar cualquier entrada.
        /// </summary>
        void Start()
        {
            transitionExecuted = false;
            SetupInputSystem();
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
        /// Configura el Input System para detectar cualquier entrada del jugador.
        /// </summary>
        private void SetupInputSystem()
        {
            try
            {
                playerInput = new PlayerInput();

                // Suscribirse a todos los eventos de entrada disponibles
                playerInput.Player.Lane1Key.performed += OnAnyInput;
                playerInput.Player.Lane2Key.performed += OnAnyInput;
                playerInput.Player.Click.performed += OnAnyInput;
                playerInput.Player.Pause.performed += OnAnyInput;

                playerInput.Player.Enable();
                Debug.Log("Input System configurado para transición");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error configurando Input System: " + e.Message);
            }
        }

        /// <summary>
        /// Maneja cualquier entrada del jugador durante el estado de espera.
        /// Se ejecuta cuando se detecta cualquier acción del Input System.
        /// </summary>
        /// <param name="context">Contexto de la acción de entrada</param>
        private void OnAnyInput(InputAction.CallbackContext context)
        {
            if (!transitionExecuted)
            {
                ExecuteTransition();
            }
        }

        /// <summary>
        /// Ejecuta la transición activando/desactivando los GameObjects configurados.
        /// </summary>
        private void ExecuteTransition()
        {
            // Marcar como ejecutado si se configura para ejecutar solo una vez
            if (executeOnlyOnce)
            {
                transitionExecuted = true;
            }

            // Desactivar GameObjects en la lista de desactivación
            foreach (GameObject obj in objectsToDeactivate)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    Debug.Log($"Desactivado: {obj.name}");
                }
            }

            // Activar GameObjects en la lista de activación
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                    Debug.Log($"Activado: {obj.name}");
                }
            }

            // Iniciar canción si está configurado
            if (initSongOnTransition && SongManager.Instance != null)
            {
                SongManager.Instance.InitSong();
                Debug.Log("Canción iniciada");
            }

            // Limpiar Input System si se ejecuta solo una vez
            if (executeOnlyOnce)
            {
                CleanupInputSystem();
            }

            Debug.Log("Transición ejecutada correctamente");
        }

        /// <summary>
        /// Limpia las suscripciones del Input System.
        /// </summary>
        private void CleanupInputSystem()
        {
            if (playerInput != null)
            {
                try
                {
                    playerInput.Player.Lane1Key.performed -= OnAnyInput;
                    playerInput.Player.Lane2Key.performed -= OnAnyInput;
                    playerInput.Player.Click.performed -= OnAnyInput;
                    playerInput.Player.Pause.performed -= OnAnyInput;

                    playerInput.Player.Disable();
                    playerInput.Dispose();
                    playerInput = null;
                    Debug.Log("Input System limpiado");
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error limpiando Input System: " + e.Message);
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Método público para ejecutar la transición manualmente desde otros scripts o eventos UI.
        /// </summary>
        public void TriggerTransition()
        {
            if (!executeOnlyOnce || !transitionExecuted)
            {
                ExecuteTransition();
            }
        }

        /// <summary>
        /// Reinicia el estado del script para poder ejecutar la transición nuevamente.
        /// </summary>
        public void ResetTransition()
        {
            transitionExecuted = false;
            if (playerInput == null)
            {
                SetupInputSystem();
            }
        }
        #endregion
    }
}
