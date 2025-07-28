using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Songs
{
  /// <summary>
  /// Componente que hace que un objeto pulse/escale al ritmo de la música.
  /// Proporciona efectos visuales sincronizados con el beat, escalando temporalmente
  /// el objeto y luego regresándolo suavemente a su tamaño original.
  /// </summary>
  public class PulseToTheBeat : MonoBehaviour
  {
    #region Serialized Fields
    [Header("Configuración de Pulso")]
    [Tooltip("Activar para usar un beat de prueba automático (solo testing)")]
    [SerializeField] bool useTestBeat;

    [Tooltip("Multiplicador de escala cuando ocurre el pulso (1.15 = 15% más grande)")]
    [SerializeField] float pulseSize = 1.15f;

    [Tooltip("Velocidad de retorno al tamaño original después del pulso")]
    [SerializeField] float returnSpeed = 5f;
    #endregion

    #region Private Fields
    /// <summary>Escala inicial del objeto al comenzar</summary>
    private Vector3 startSize;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Inicializa la escala de referencia y opcionalmente inicia el beat de prueba.
    /// </summary>
    private void Start()
    {
      startSize = transform.localScale;
      if (useTestBeat)
      {
        StartCoroutine(TestBeat());
      }
    }

    /// <summary>
    /// Interpola suavemente la escala del objeto de vuelta al tamaño original.
    /// Se ejecuta continuamente para crear el efecto de retorno gradual.
    /// </summary>
    private void Update()
    {
      transform.localScale = Vector3.Lerp(transform.localScale, startSize, returnSpeed * Time.deltaTime);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Ejecuta un pulso inmediato, escalando el objeto al tamaño configurado.
    /// Este método debe ser llamado por el PulseManager o eventos externos.
    /// </summary>
    public void Pulse()
    {
      transform.localScale = startSize * pulseSize;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Corrutina de prueba que ejecuta pulsos automáticos cada segundo.
    /// Solo se usa para testing cuando useTestBeat está activado.
    /// </summary>
    /// <returns>Corrutina que ejecuta pulsos de prueba</returns>
    IEnumerator TestBeat()
    {
      while (true)
      {
        yield return new WaitForSeconds(1f);
        Pulse();
      }
    }
    #endregion
  }
}