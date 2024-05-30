using System.Collections;
using UnityEngine;

namespace Script.Animation
{
  public class ScorePulse : MonoBehaviour
  {
    [SerializeField] private float pulseSpeed = 1.5f; // velocidad del pulso
    [SerializeField] private float maxScale = 1.2f; // escala máxima del pulso
    public static ScorePulse Instance;
    private bool isPulsing;
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
        StartCoroutine(PulseEffect());
      }
    }

    private IEnumerator PulseEffect()
    {
      isPulsing = true;
      Vector3 originalScale = transform.localScale;
      Vector3 destinationScale = new Vector3(maxScale, maxScale, maxScale);

      // agrandar
      float t = 0.0f;
      while (t <= 1.0f)
      {
        t += Time.deltaTime * pulseSpeed;
        transform.localScale = Vector3.Lerp(originalScale, destinationScale, t);
        yield return null;
      }

      // reducir
      t = 0.0f;
      while (t <= 1.0f)
      {
        t += Time.deltaTime * pulseSpeed;
        transform.localScale = Vector3.Lerp(destinationScale, originalScale, t);
        yield return null;
      }

      transform.localScale = originalScale;
      isPulsing = false;
    }
  }
}