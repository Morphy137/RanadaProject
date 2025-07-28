using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Animation
{
  /// <summary>
  /// Controla el efecto de pulso visual en elementos de la interfaz cuando el jugador obtiene puntos.
  /// </summary>
  public class ScorePulse : MonoBehaviour
  {
    #region Serialized Fields
    [SerializeField] private float pulseSpeed = 1.5f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private List<GameObject> gameObjectsToPulse = new List<GameObject>();
    #endregion

    #region Private Fields
    private bool isPulsing;
    #endregion

    #region Public Properties
    /// <summary>
    /// Instancia singleton del ScorePulse.
    /// </summary>
    public static ScorePulse Instance { get; private set; }
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Inicializa la instancia singleton.
    /// </summary>
    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
      }
      else
      {
        Debug.LogError("Multiple instances of ScorePulse!");
      }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Ejecuta el efecto de pulso en todos los GameObjects configurados.
    /// </summary>
    public void Pulse()
    {
      if (!isPulsing)
      {
        foreach (var gameObject in gameObjectsToPulse)
        {
          StartCoroutine(PulseEffect(gameObject));
        }
      }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Corrutina que ejecuta el efecto de pulso: agranda y luego reduce el objeto a su tamaño original.
    /// </summary>
    /// <param name="gameObjectToPulse">Objeto al que aplicar el efecto</param>
    private IEnumerator PulseEffect(GameObject gameObjectToPulse)
    {
      isPulsing = true;
      Vector3 originalScale = gameObjectToPulse.transform.localScale;
      Vector3 destinationScale = new Vector3(maxScale, maxScale, maxScale);

      // Fase de agrandamiento
      float t = 0.0f;
      while (t <= 1.0f)
      {
        t += Time.deltaTime * pulseSpeed;
        gameObjectToPulse.transform.localScale = Vector3.Lerp(originalScale, destinationScale, t);
        yield return null;
      }

      // Fase de reducción
      t = 0.0f;
      while (t <= 1.0f)
      {
        t += Time.deltaTime * pulseSpeed;
        gameObjectToPulse.transform.localScale = Vector3.Lerp(destinationScale, originalScale, t);
        yield return null;
      }

      gameObjectToPulse.transform.localScale = originalScale;
      isPulsing = false;
    }
    #endregion
  }
}