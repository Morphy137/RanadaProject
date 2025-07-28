using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using Script.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Notes
{
  /// <summary>
  /// Gestiona una pista individual del juego de ritmo, controlando la generación de notas, 
  /// detección de entrada del jugador y evaluación de precisión.
  /// Maneja tanto las notas musicales como los elementos de comida asociados.
  /// </summary>
  public class Lane : MonoBehaviour
  {
    #region Serialized Fields
    [Header("Configuración de Notas")]
    [Tooltip("Restricción de nota musical específica para esta pista")]
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;

    [Tooltip("Prefab de la nota que se generará en esta pista")]
    public GameObject notePrefab;

    [Header("Prefabs de Evaluación")]
    [Tooltip("Prefab de texto flotante para golpes perfectos")]
    public GameObject PerfectPrefab;

    [Tooltip("Prefab de texto flotante para golpes excelentes")]
    public GameObject GreatPrefab;

    [Tooltip("Prefab de texto flotante para golpes buenos")]
    public GameObject GoodPrefab;

    [Tooltip("Prefab de texto flotante para golpes fallidos")]
    public GameObject MissPrefab;

    [Header("Efectos Visuales")]
    [Tooltip("Prefab del círculo de efecto visual al golpear")]
    public GameObject circlePrefab;

    [Header("Sistema de Comida")]
    [Tooltip("Lista de prefabs de comida que aparecen con las notas")]
    [SerializeField] private List<GameObject> foodPrefabs;

    [Header("Configuración Visual")]
    [Tooltip("Opciones de sprites aleatorios para las notas")]
    public Sprite[] spriteOptions;
    #endregion

    #region Public Fields
    /// <summary>Lista de todas las notas activas en esta pista</summary>
    public List<Note> notes = new();

    /// <summary>Lista de todos los elementos de comida activos en esta pista</summary>
    public List<Note> foods = new();

    /// <summary>Lista de marcas de tiempo para la generación de notas</summary>
    public List<double> timeStamps = new();
    #endregion

    #region Private Fields
    /// <summary>Posición donde aparecen los textos de calificación</summary>
    private Vector3 ratingSpawnPos = new(-12f, 2f, 0f);

    /// <summary>Índice actual para la generación de notas</summary>
    private int spawnIndex;

    /// <summary>Índice actual para la evaluación de entrada</summary>
    private int inputIndex;

    /// <summary>Indica si el cooldown de entrada está activo</summary>
    private bool isCooldownActive;

    /// <summary>Duración del cooldown entre entradas</summary>
    private float cooldownDuration = 0.5f;

    /// <summary>Temporizador actual del cooldown</summary>
    private float cooldownTimer;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Actualiza continuamente el sistema de generación de notas.
    /// </summary>
    void Update()
    {
      HandleNoteSpawning();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Configura las marcas de tiempo para esta pista basándose en un arreglo de notas MIDI.
    /// Filtra las notas que coinciden con la restricción de nota de esta pista.
    /// </summary>
    /// <param name="array">Arreglo de notas MIDI para procesar</param>
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
      foreach (var note in array)
      {
        if (note.NoteName == noteRestriction)
        {
          var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
          timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
        }
      }
    }

    /// <summary>
    /// Maneja la entrada de tecla presionada para esta pista.
    /// </summary>
    public void OnInputPressed()
    {
      HandleNoteInput(true);
    }

    /// <summary>
    /// Maneja la entrada de tecla liberada para esta pista.
    /// </summary>
    public void OnInputReleased()
    {
      HandleNoteInput(false);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Gestiona la generación automática de notas basándose en las marcas de tiempo y el tiempo actual de la canción.
    /// Crea tanto las notas musicales como los elementos de comida asociados.
    /// </summary>
    private void HandleNoteSpawning()
    {
      if (spawnIndex < timeStamps.Count)
      {
        if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
        {
          var note = Instantiate(notePrefab, transform);
          notes.Add(note.GetComponent<Note>());
          note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];

          GameObject foodPrefab = foodPrefabs[UnityEngine.Random.Range(0, foodPrefabs.Count)];
          var food = Instantiate(foodPrefab, new Vector3(note.transform.position.x, -8.5f, note.transform.position.z), Quaternion.identity);
          var foodNote = food.GetComponentInChildren<Note>();
          foods.Add(foodNote);
          foodNote.assignedTime = (float)timeStamps[spawnIndex];

          // Set the sprite of the note
          if (spriteOptions.Length > 0)
          {
            int randomIndex = UnityEngine.Random.Range(0, spriteOptions.Length);
            note.GetComponent<SpriteRenderer>().sprite = spriteOptions[randomIndex];
            note.GetComponent<SpriteRenderer>().sortingOrder = 10;
          }
          spawnIndex++;
        }
      }
    }

    /// <summary>
    /// Procesa la entrada del jugador y evalúa la precisión del golpe contra las notas programadas.
    /// Maneja tanto los golpes exitosos como los fallos, aplicando cooldowns para prevenir spam.
    /// </summary>
    /// <param name="inputPressed">Indica si la entrada fue presionada (true) o liberada (false)</param>
    private void HandleNoteInput(bool inputPressed)
    {
      if (!GlobalScore.songStarted)
        return;
      if (inputIndex < timeStamps.Count)
      {
        double timeStamp = timeStamps[inputIndex];
        double marginOfError = SongManager.Instance.marginOfError;
        double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

        if (inputPressed)
        {
          if (Math.Abs(audioTime - timeStamp) < marginOfError)
          {
            Animator foodAnimation = foods[inputIndex].GetComponent<Animator>();
            Hit(notes[inputIndex].gameObject.transform.position.x + 4);
            Debug.Log($"Hit on {inputIndex} note");
            Destroy(notes[inputIndex].gameObject);
            foods[inputIndex].movementEnabled = false;
            StartCoroutine(DestroyAfterDelay(foods[inputIndex].gameObject, 0.20f));
            inputIndex++;
          }
          else if (!isCooldownActive)
          {
            Miss(true);
            Debug.Log($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
            isCooldownActive = true;
            cooldownTimer = cooldownDuration;
          }
        }

        if (timeStamp + marginOfError <= audioTime)
        {
          Miss(false);
          Debug.Log($"Missed {inputIndex} note");
          inputIndex++;
        }

        if (isCooldownActive)
        {
          cooldownTimer -= Time.deltaTime;
          if (cooldownTimer <= 0)
          {
            isCooldownActive = false;
          }
        }
      }
    }

    /// <summary>
    /// Procesa un golpe exitoso de nota, evaluando la precisión y creando efectos visuales apropiados.
    /// Actualiza las estadísticas globales y reproduce animaciones de feedback.
    /// </summary>
    /// <param name="position">Posición relativa del golpe para determinar la precisión</param>
    private void Hit(float position)
    {
      ScoreManager.Hit();
      GameObject prefab;
      Animator circleAnimation = circlePrefab.GetComponent<Animator>();

      if (circleAnimation != null)
      {
        circleAnimation.Play("hit", -1, 0f);
      }

      if (position <= 0.5 && position >= -0.5)
      {
        GlobalScore.perfectHits++;
        prefab = Instantiate(PerfectPrefab, ratingSpawnPos, Quaternion.identity);
      }
      else if (position is <= 1 and >= -1)
      {
        GlobalScore.greatHits++;
        prefab = Instantiate(GreatPrefab, ratingSpawnPos, Quaternion.identity);
      }
      else
      {
        GlobalScore.gooodHits++;
        prefab = Instantiate(GoodPrefab, ratingSpawnPos, Quaternion.identity);
      }

      StartCoroutine(DestroyAfterDelay(prefab, 3f));
    }

    /// <summary>
    /// Procesa un fallo de nota, actualizando estadísticas y mostrando feedback visual apropiado.
    /// </summary>
    /// <param name="isInputMiss">Indica si el fallo fue por entrada incorrecta (true) o por tiempo agotado (false)</param>
    private void Miss(bool isInputMiss)
    {
      ScoreManager.Miss(isInputMiss);
      GlobalScore.missesHit++;
      // Floating Text for Miss
      GameObject prefab = Instantiate(MissPrefab, ratingSpawnPos, Quaternion.identity);
      StartCoroutine(DestroyAfterDelay(prefab, 3f));
    }

    /// <summary>
    /// Anima un prefab con escalado y movimiento suaves antes de destruirlo.
    /// Método auxiliar para efectos visuales de feedback.
    /// </summary>
    /// <param name="prefab">GameObject a animar</param>
    /// <returns>Corrutina que maneja la animación</returns>
    private IEnumerator AnimatePrefab(GameObject prefab)
    {
      float duration = 1f; // Duración de la animación en segundos
      float elapsed = 0f; // Tiempo transcurrido desde el inicio de la animación

      Vector3 initialScale = prefab.transform.localScale; // Escala inicial del prefab
      Vector3 targetScale = initialScale * 1.4f; // Escala final del prefab

      Vector3 initialPosition = prefab.transform.position; // Posición inicial del prefab
      Vector3 targetPosition = initialPosition + new Vector3(2, 2, 0); // Posición final del prefab

      while (elapsed < duration)
      {
        elapsed += Time.deltaTime; // Actualiza el tiempo transcurrido
        float t = elapsed / duration; // Calcula la fracción del tiempo transcurrido

        // Interpola la escala y la posición del prefab
        prefab.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
        prefab.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

        yield return null; // Espera hasta el próximo frame
      }

      // Asegura que la escala y la posición del prefab sean exactamente las deseadas al final de la animación
      prefab.transform.localScale = targetScale;
      prefab.transform.position = targetPosition;

      // Destruye el prefab
      Destroy(prefab);
    }

    /// <summary>
    /// Destruye un GameObject después de un retraso especificado.
    /// Incluye verificación de nulidad para prevenir errores.
    /// </summary>
    /// <param name="gameObjectToDestroy">GameObject a destruir</param>
    /// <param name="delay">Retraso en segundos antes de la destrucción</param>
    /// <returns>Corrutina que maneja el retraso y destrucción</returns>
    private IEnumerator DestroyAfterDelay(GameObject gameObjectToDestroy, float delay)
    {
      // Wait for the specified delay
      yield return new WaitForSeconds(delay);

      // Check if the GameObject still exists before attempting to destroy it
      if (gameObjectToDestroy != null)
      {
        Destroy(gameObjectToDestroy);
      }
    }
    #endregion
  }
}