using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Animation
{
  public class ScorePulse : MonoBehaviour
  {
    [SerializeField] private float pulseSpeed = 1.5f; // velocidad del pulso
    [SerializeField] private float maxScale = 1.2f; // escala máxima del pulso
    public static ScorePulse Instance;
    private bool isPulsing;

    // Lista de GameObjects a los que se les aplicará el efecto
    [SerializeField] private List<GameObject> gameObjectsToPulse = new List<GameObject>();

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

    public void Pulse()
    {
      if (!isPulsing)
      {
        // Aplica el efecto a todos los GameObjects en la lista
        foreach (var gameObject in gameObjectsToPulse)
        {
          StartCoroutine(PulseEffect(gameObject));
        }
      }
    }

    private IEnumerator PulseEffect(GameObject gameObjectToPulse)
    {
      isPulsing = true;
      Vector3 originalScale = gameObjectToPulse.transform.localScale;
      Vector3 destinationScale = new Vector3(maxScale, maxScale, maxScale);

      // agrandar
      float t = 0.0f;
      while (t <= 1.0f)
      {
        t += Time.deltaTime * pulseSpeed;
        gameObjectToPulse.transform.localScale = Vector3.Lerp(originalScale, destinationScale, t);
        yield return null;
      }

      // reducir
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
  }
}