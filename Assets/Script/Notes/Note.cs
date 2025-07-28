using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Script.Notes
{
    /// <summary>
    /// Controla el comportamiento y movimiento de las notas musicales individuales en el juego de ritmo.
    /// Gestiona la sincronización temporal, el movimiento visual y las transiciones de estado de las notas.
    /// </summary>
    public class Note : MonoBehaviour
    {
        #region Private Fields
        /// <summary>Tiempo en que fue instanciada la nota</summary>
        double timeInstantiated;

        /// <summary>Tiempo asignado específico para esta nota en la canción</summary>
        public float assignedTime;

        /// <summary>Indica si el movimiento automático de la nota está habilitado</summary>
        public bool movementEnabled = true;

        /// <summary>Indica si la nota está actualmente en proceso de animación</summary>
        private bool isMoving = false;

        /// <summary>Posición objetivo para la animación de la nota cuando es golpeada</summary>
        private Vector3 targetPosition = new(-10.87f, -2.17f, 0f);

        /// <summary>Escala objetivo para la animación de la nota cuando es golpeada</summary>
        private Vector3 targetScale = new(0.50f, 0.50f, 0.50f);
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa el tiempo de instanciación de la nota para cálculos de sincronización.
        /// </summary>
        void Start()
        {
            timeInstantiated = SongManager.GetAudioSourceTime(); // Get the current time of the audio source
        }

        /// <summary>
        /// Actualiza la posición de la nota basándose en el tiempo transcurrido y su estado de movimiento.
        /// Controla tanto el movimiento automático como las animaciones especiales.
        /// </summary>
        void Update()
        {
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated; // Get the time since the note was instantiated
            float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

            if (t > 1)
            {
                Destroy(gameObject);
            }
            else if (!movementEnabled && !isMoving)
            {
                StartCoroutine(MoveAndScaleToPosition(targetPosition, targetScale, 0.2f));
            }
            else if (movementEnabled)
            {
                transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnY,
                    Vector3.right * SongManager.Instance.noteDespawnY, t);
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Corrutina que anima suavemente la nota hacia una posición y escala específicas.
        /// Utilizada cuando la nota es golpeada exitosamente para crear efectos visuales.
        /// </summary>
        /// <param name="targetPosition">Posición objetivo del movimiento</param>
        /// <param name="targetScale">Escala objetivo del movimiento</param>
        /// <param name="duration">Duración de la animación en segundos</param>
        /// <returns>Corrutina que maneja la animación</returns>
        private IEnumerator MoveAndScaleToPosition(Vector3 targetPosition, Vector3 targetScale, float duration)
        {
            isMoving = true;
            Vector3 startPosition = transform.position;
            Vector3 startScale = transform.localScale;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition; // Ensure the final position is accurate
            transform.localScale = targetScale; // Ensure the final scale is accurate
            isMoving = false;
        }
        #endregion
    }
}
