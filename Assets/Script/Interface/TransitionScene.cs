using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Script.Interface
{
    /// <summary>
    /// Controla las transiciones visuales entre escenas usando una imagen que se escala.
    /// Implementa un efecto de zoom in/out para suavizar el cambio entre escenas.
    /// </summary>
    public class SceneTransition : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Configuración de transición")]
        public Canvas transitionCanvas;
        public Image transitionImage;
        public float transitionDuration = 1.0f;
        public Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);
        public Vector3 maxScale = new Vector3(5.0f, 5.0f, 5.0f);
        #endregion

        #region Private Fields
        private static SceneTransition instance;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// Inicializa el singleton y configura el Canvas para persistir entre escenas.
        /// </summary>
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(transitionCanvas.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Configura el estado inicial de la imagen de transición.
        /// </summary>
        void Start()
        {
            transitionImage.rectTransform.localScale = startScale;
            transitionImage.enabled = true;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Inicia una transición hacia una nueva escena con efecto visual.
        /// </summary>
        /// <param name="sceneName">Nombre de la escena a cargar</param>
        public void StartSceneTransition(string sceneName)
        {
            SoundManager.Instance.ResetAudioSource();
            StartCoroutine(Transition(sceneName));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Corrutina que maneja el proceso completo de transición entre escenas.
        /// </summary>
        /// <param name="sceneName">Nombre de la escena destino</param>
        IEnumerator Transition(string sceneName)
        {
            yield return StartCoroutine(ScaleImage(maxScale, transitionDuration));

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }

            asyncLoad.allowSceneActivation = true;
            yield return null;
            yield return StartCoroutine(ScaleImage(Vector3.zero, transitionDuration));
        }

        /// <summary>
        /// Anima el escalado de la imagen de transición con suavizado.
        /// </summary>
        /// <param name="targetScale">Escala objetivo</param>
        /// <param name="duration">Duración de la animación</param>
        IEnumerator ScaleImage(Vector3 targetScale, float duration)
        {
            Vector3 initialScale = transitionImage.rectTransform.localScale;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                float scaleFactor = Mathf.SmoothStep(0f, 1f, t);
                transitionImage.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, scaleFactor);
                yield return null;
            }

            transitionImage.rectTransform.localScale = targetScale;

            if (targetScale == Vector3.zero)
            {
                transitionImage.enabled = false;
            }
        }
        #endregion
    }
}
