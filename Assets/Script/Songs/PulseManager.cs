using System;
using Script.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace Script.Songs
{
  /// <summary>
  /// Gestor principal del sistema de pulso musical sincronizado con BPM.
  /// Controla intervalos temporales basados en el tempo de la música y ejecuta eventos
  /// cuando se detectan nuevos beats o intervalos musicales.
  /// Implementa el patrón singleton para acceso global.
  /// </summary>
  public class PulseManager : MonoBehaviour
  {
    #region Serialized Fields
    [Header("Configuración de Tempo")]
    [Tooltip("Beats por minuto (BPM) de la canción actual")]
    [SerializeField] private float bpm;

    [Tooltip("Array de intervalos que se ejecutarán en diferentes momentos del beat")]
    [SerializeField] private Intervals[] intervals;
    #endregion

    #region Private Fields
    /// <summary>Instancia singleton del PulseManager</summary>
    private PulseManager instance;

    /// <summary>AudioSource obtenido del SoundManager para sincronización temporal</summary>
    private AudioSource soundManagerAudioSource;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Inicializa el singleton y obtiene la referencia al AudioSource del SoundManager.
    /// </summary>
    private void Awake()
    {
      if (instance == null)
      {
        instance = this;
      }

      soundManagerAudioSource = SoundManager.Instance.GetBgmSource();
    }

    /// <summary>
    /// Actualiza continuamente todos los intervalos, verificando si han ocurrido nuevos beats.
    /// Calcula el tiempo muestreado basándose en el AudioSource y los intervalos configurados.
    /// </summary>
    private void Update()
    {
      foreach (var interval in intervals)
      {
        float sampledTime = (soundManagerAudioSource.timeSamples / (soundManagerAudioSource.clip.frequency * interval.GetIntervalLength(bpm)));
        interval.CheckForNewInterval(sampledTime);
      }
    }
    #endregion
  }

  /// <summary>
  /// Representa un intervalo musical específico que puede ejecutar eventos cuando se detecta un nuevo beat.
  /// Configurable mediante steps para diferentes divisiones del tempo (1/4, 1/8, 1/16, etc.).
  /// </summary>
  [System.Serializable]
  public class Intervals
  {
    #region Serialized Fields
    [Header("Configuración de Intervalo")]
    [Tooltip("Número de pasos por beat (1 = quarter note, 2 = eighth note, 4 = sixteenth note)")]
    [SerializeField] private float steps;

    [Tooltip("Evento que se ejecuta cuando se detecta un nuevo intervalo")]
    [SerializeField] private UnityEvent trigger;
    #endregion

    #region Private Fields
    /// <summary>Último intervalo procesado para evitar ejecuciones duplicadas</summary>
    private int lastInterval;
    #endregion

    #region Public Methods
    /// <summary>
    /// Calcula la duración de un intervalo basándose en el BPM y los pasos configurados.
    /// </summary>
    /// <param name="bpm">Beats por minuto de la canción</param>
    /// <returns>Duración del intervalo en segundos</returns>
    public float GetIntervalLength(float bpm)
    {
      return 60 / (bpm * steps);
    }

    /// <summary>
    /// Verifica si ha ocurrido un nuevo intervalo y ejecuta el evento correspondiente.
    /// Utiliza Floor para detectar cambios de intervalo entero.
    /// </summary>
    /// <param name="interval">Tiempo del intervalo actual calculado</param>
    public void CheckForNewInterval(float interval)
    {
      // redondea hacia abajo el intervalo
      if (Mathf.FloorToInt(interval) != lastInterval)
      {
        lastInterval = Mathf.FloorToInt(interval);
        trigger.Invoke();
      }
    }
    #endregion
  }
}