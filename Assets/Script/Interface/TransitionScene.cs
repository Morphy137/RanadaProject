using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Script.Interface;

public class SceneTransition : MonoBehaviour
{
    public Canvas transitionCanvas; // Canvas que contiene la imagen de transición
    public Image transitionImage; // La imagen para la transición
    public float transitionDuration = 1.0f; // Duración de la transición
    public Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f); // Tamaño inicial pequeño
    public Vector3 maxScale = new Vector3(5.0f, 5.0f, 5.0f); // Tamaño máximo para cubrir la pantalla

    private static SceneTransition instance;

    void Awake()
    {
        // Asegurarnos de que el Canvas persista entre escenas
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transitionCanvas.gameObject); // No destruir el Canvas
        }
        else
        {
            Destroy(gameObject); // Destruir cualquier duplicado
        }
    }

    void Start()
    {
        // Inicialmente, la imagen es pequeña y visible
        transitionImage.rectTransform.localScale = startScale;
        transitionImage.enabled = true; // Hacer que la imagen esté visible
    }

    public void StartSceneTransition(string sceneName)
    {
        SoundManager.Instance.ResetAudioSource();
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        // 1. Agrandar la imagen hasta cubrir la pantalla completamente con aceleración/desaceleración
        yield return StartCoroutine(ScaleImage(maxScale, transitionDuration));

        // 2. Cargar la nueva escena en segundo plano sin activarla de inmediato
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // 3. Esperar a que la escena esté completamente cargada (progreso >= 90%)
        while (asyncLoad.progress < 0.9f)
        {
            yield return null; // Continuar esperando hasta que se cargue la escena
        }

        // 4. Activar la nueva escena
        asyncLoad.allowSceneActivation = true;

        // 5. Esperar un frame para asegurarnos de que la escena esté visible
        yield return null;

        // 6. Achicar la imagen desde el tamaño máximo hasta desaparecer (escala 0) con aceleración/desaceleración
        yield return StartCoroutine(ScaleImage(Vector3.zero, transitionDuration));
    }

    IEnumerator ScaleImage(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transitionImage.rectTransform.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Usamos Mathf.SmoothStep para aplicar aceleración y desaceleración
            float scaleFactor = Mathf.SmoothStep(0f, 1f, t);

            transitionImage.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, scaleFactor);
            yield return null;
        }

        // Aseguramos que la imagen tenga el tamaño correcto al final
        transitionImage.rectTransform.localScale = targetScale;

        // Deshabilitamos la imagen solo si se ha achicado completamente
        if (targetScale == Vector3.zero)
        {
            transitionImage.enabled = false; // Ocultar la imagen al finalizar
        }
    }
}
