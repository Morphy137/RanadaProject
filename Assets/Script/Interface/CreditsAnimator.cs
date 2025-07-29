using UnityEngine;
using UnityEngine.Events;

namespace Script.Interface
{
    /// <summary>
    /// Controla la animación de desplazamiento de los créditos del juego.
    /// Gestiona el movimiento vertical automático del texto de créditos y el logo.
    /// </summary>
    public class CreditsAnimator : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Elementos de UI")]
        [Tooltip("GameObject padre que contiene todos los elementos de créditos (texto y logo)")]
        public RectTransform creditsContainer;

        [Header("Configuración de Animación")]
        [Tooltip("Velocidad de desplazamiento vertical en unidades por segundo")]
        public float speed = 50f;

        [Tooltip("Si está marcado, la animación comenzará automáticamente al iniciar")]
        public bool autoStart = false;

        [Tooltip("Si está marcado, la animación se repetirá en bucle infinito")]
        public bool loop = false;

        [Header("Efectos Visuales")]
        [Tooltip("Curva de animación para variar la velocidad durante el movimiento")]
        public AnimationCurve speedCurve = AnimationCurve.Linear(0, 1, 1, 1);

        [Tooltip("Si está marcado, aplicará fade in/out temporal durante la animación")]
        public bool fadeInOut = false;

        [Header("Control de Tiempo")]
        [Tooltip("Tiempo en segundos para detener automáticamente (0 = nunca parar)")]
        public float autoStopAfterSeconds = 0f;

        [Header("Configuración de Loop")]
        [Tooltip("Altura máxima antes de reiniciar posición (solo con loop activado)")]
        public float maxHeight = 1000f;

        [Tooltip("Posición Y donde reaparecerán los créditos en modo loop (abajo de la pantalla)")]
        public float loopRestartPositionY = -300f;

        [Header("Audio")]
        [Tooltip("AudioSource para reproducir música durante los créditos")]
        public AudioSource creditsMusic;

        [Tooltip("Si está marcado, reproducirá música automáticamente al iniciar")]
        public bool playMusicOnStart = true;

        [Header("Eventos")]
        [Tooltip("Evento que se ejecuta cuando inician los créditos")]
        public UnityEvent OnCreditsStart;

        [Tooltip("Evento que se ejecuta cuando terminan los créditos")]
        public UnityEvent OnCreditsEnd;
        #endregion

        #region Private Fields
        /// <summary>Posición inicial del contenedor de créditos</summary>
        private Vector3 initialPosition;

        /// <summary>Indica si los créditos están actualmente en movimiento</summary>
        private bool isCreditsRolling = false;

        /// <summary>Tiempo transcurrido desde el inicio de la animación</summary>
        private float animationTime = 0f;

        /// <summary>CanvasGroup para controlar el fade del contenedor completo</summary>
        private CanvasGroup creditsCanvasGroup;

        /// <summary>Opacidad inicial del contenedor</summary>
        private float initialAlpha = 1f;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa la posición original del contenedor de créditos al despertar el componente.
        /// </summary>
        void Awake()
        {
            // Guardar la posición inicial del contenedor de créditos
            if (creditsContainer != null)
            {
                initialPosition = creditsContainer.localPosition;

                // Obtener o crear el CanvasGroup del contenedor
                creditsCanvasGroup = creditsContainer.GetComponent<CanvasGroup>();
                if (creditsCanvasGroup == null)
                {
                    creditsCanvasGroup = creditsContainer.gameObject.AddComponent<CanvasGroup>();
                    Debug.Log("CanvasGroup agregado automáticamente al contenedor de créditos");
                }

                initialAlpha = creditsCanvasGroup.alpha;
            }
            else
            {
                Debug.LogWarning("Credits Container no está asignado en " + gameObject.name);
            }

            Debug.Log("Posición inicial del contenedor de créditos guardada: " + initialPosition);
        }

        /// <summary>
        /// Inicia la animación automáticamente si está configurado para hacerlo.
        /// </summary>
        void Start()
        {
            if (autoStart)
            {
                StartAnimation();
            }
        }

        /// <summary>
        /// Actualiza la posición de los elementos durante la animación de créditos.
        /// Se ejecuta cada frame cuando los créditos están en movimiento.
        /// </summary>
        void Update()
        {
            if (isCreditsRolling)
            {
                // Incrementar el tiempo de animación
                animationTime += Time.deltaTime;

                // Evaluar la curva de velocidad basada en el tiempo normalizado
                float normalizedTime = autoStopAfterSeconds > 0 ? animationTime / autoStopAfterSeconds : animationTime * 0.1f;
                float speedMultiplier = speedCurve.Evaluate(normalizedTime);

                // Calcular el desplazamiento para este frame
                Vector3 movement = Vector3.up * speed * speedMultiplier * Time.deltaTime;

                // Mover el contenedor completo hacia arriba
                if (creditsContainer != null)
                    creditsContainer.localPosition += movement;

                // Aplicar fade in/out si está habilitado
                if (fadeInOut)
                {
                    ApplyFadeEffect(normalizedTime);
                }

                // Verificar si necesita reiniciar en modo loop
                if (loop && creditsContainer != null && creditsContainer.localPosition.y > maxHeight)
                {
                    ResetToLoopPosition();
                }

                // Verificar auto-stop
                if (autoStopAfterSeconds > 0 && animationTime >= autoStopAfterSeconds)
                {
                    StopAnimation();
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Inicia la animación de los créditos desde sus posiciones iniciales.
        /// Restablece las posiciones y activa el movimiento automático.
        /// </summary>
        public void StartAnimation()
        {
            ResetToInitialPosition();
            animationTime = 0f;
            isCreditsRolling = true;

            // Reproducir música si está configurado
            if (playMusicOnStart && creditsMusic != null)
            {
                creditsMusic.Play();
            }

            // Invocar evento de inicio
            OnCreditsStart?.Invoke();

            Debug.Log("Animación de créditos iniciada");
        }

        /// <summary>
        /// Detiene inmediatamente la animación de los créditos.
        /// Los elementos permanecen en su posición actual.
        /// </summary>
        public void StopAnimation()
        {
            isCreditsRolling = false;

            // Detener música si está reproduciéndose
            if (creditsMusic != null && creditsMusic.isPlaying)
            {
                creditsMusic.Stop();
            }

            // Invocar evento de fin
            OnCreditsEnd?.Invoke();

            Debug.Log("Animación de créditos detenida");
        }

        /// <summary>
        /// Pausa o reanuda la animación de créditos.
        /// </summary>
        public void ToggleAnimation()
        {
            isCreditsRolling = !isCreditsRolling;
            Debug.Log("Animación de créditos " + (isCreditsRolling ? "reanudada" : "pausada"));
        }

        /// <summary>
        /// Restablece los elementos a sus posiciones iniciales sin iniciar la animación.
        /// </summary>
        public void ResetToInitialPosition()
        {
            if (creditsContainer != null)
                creditsContainer.localPosition = initialPosition;

            // Restablecer opacidad inicial si hay fade
            if (fadeInOut && creditsCanvasGroup != null)
            {
                creditsCanvasGroup.alpha = initialAlpha;
            }

            animationTime = 0f;
            Debug.Log("Créditos restablecidos a posición inicial");
        }

        /// <summary>
        /// Cambia la velocidad de la animación durante la ejecución.
        /// </summary>
        /// <param name="newSpeed">Nueva velocidad en unidades por segundo</param>
        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
            Debug.Log("Velocidad de animación cambiada a: " + newSpeed);
        }

        /// <summary>
        /// Verifica si la animación está actualmente en ejecución.
        /// </summary>
        /// <returns>True si la animación está corriendo, false si está detenida</returns>
        public bool IsAnimating()
        {
            return isCreditsRolling;
        }

        /// <summary>
        /// Obtiene el tiempo transcurrido desde el inicio de la animación.
        /// </summary>
        /// <returns>Tiempo en segundos desde que inició la animación</returns>
        public float GetAnimationTime()
        {
            return animationTime;
        }

        /// <summary>
        /// Pausa o reanuda la música de créditos.
        /// </summary>
        public void ToggleMusic()
        {
            if (creditsMusic != null)
            {
                if (creditsMusic.isPlaying)
                    creditsMusic.Pause();
                else
                    creditsMusic.UnPause();
            }
        }

        /// <summary>
        /// Cambia la posición de reinicio del loop durante runtime.
        /// </summary>
        /// <param name="newYPosition">Nueva posición Y donde reaparecerán los créditos</param>
        public void SetLoopRestartPosition(float newYPosition)
        {
            loopRestartPositionY = newYPosition;
            Debug.Log("Posición de reinicio del loop cambiada a: " + newYPosition);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Aplica efectos de fade in/out basados en el tiempo normalizado.
        /// </summary>
        /// <param name="normalizedTime">Tiempo normalizado de la animación (0-1)</param>
        private void ApplyFadeEffect(float normalizedTime)
        {
            if (creditsCanvasGroup == null)
                return;

            // Crear curva de fade: fade in los primeros 20%, opaco en el medio, fade out los últimos 20%
            float alpha = 1f;

            if (normalizedTime < 0.2f)
            {
                // Fade in
                alpha = normalizedTime / 0.2f;
            }
            else if (normalizedTime > 0.8f)
            {
                // Fade out
                alpha = (1f - normalizedTime) / 0.2f;
            }

            // Aplicar alpha al contenedor completo
            creditsCanvasGroup.alpha = alpha * initialAlpha;
        }

        /// <summary>
        /// Restablece el contenedor a la posición de reinicio del loop (abajo de la pantalla).
        /// </summary>
        private void ResetToLoopPosition()
        {
            if (creditsContainer != null)
            {
                Vector3 newPosition = creditsContainer.localPosition;
                newPosition.y = loopRestartPositionY;
                creditsContainer.localPosition = newPosition;
            }

            Debug.Log("Créditos reiniciados en posición de loop: Y = " + loopRestartPositionY);
        }
        #endregion
    }
}