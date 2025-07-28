
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Script.Player;

namespace Script.Notes
{
  /// <summary>
  /// Gestiona la pantalla de estadísticas del jugador al final de una canción.
  /// Controla la animación de puntuaciones, rankings, sprites de mascotas y efectos de audio.
  /// Proporciona feedback visual y auditivo basado en el rendimiento del jugador.
  /// </summary>
  public class ScorePlayerStats : MonoBehaviour
  {
    #region Serialized Fields
    [Header("Sistema de Ranking")]
    [Tooltip("Imagen donde se muestra el ranking obtenido")]
    [SerializeField] private Image setRanking;

    [Tooltip("Lista de sprites de ranking: 0=S, 1=A, 2=B, 3=C, 4=D, 5=E, 6=F")]
    [SerializeField] private List<Sprite> rankingScore;

    [Space(10)]
    [Header("Mascotas - Pet")]
    [Tooltip("Imagen de la mascota principal")]
    [SerializeField] private Image setModePet;

    [Tooltip("Lista de sprites de la mascota: 0=Perfect, 1=Great, 2=Bad")]
    [SerializeField] private List<Sprite> petSprites;

    [Space(10)]
    [Header("Mascotas - Dog")]
    [Tooltip("Imagen de la mascota secundaria (perro)")]
    [SerializeField] private Image setModeDog;

    [Tooltip("Lista de sprites del perro en diferentes estados")]
    [SerializeField] private List<Sprite> dogSprites;

    [Space(10)]
    [Header("Interfaz de Puntuación")]
    [Tooltip("Texto que muestra la puntuación final")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Space(10)]
    [Header("Estadísticas de Precisión")]
    [Tooltip("Texto para mostrar golpes perfectos (uyuuuy)")]
    [SerializeField] private TextMeshProUGUI uyuuuyText;

    [Tooltip("Texto para mostrar golpes excelentes (bakan)")]
    [SerializeField] private TextMeshProUGUI bakanText;

    [Tooltip("Texto para mostrar golpes buenos (wena)")]
    [SerializeField] private TextMeshProUGUI wenaText;

    [Tooltip("Texto para mostrar golpes fallidos (pucha)")]
    [SerializeField] private TextMeshProUGUI puchaText;

    [Space(10)]
    [Header("Audio de Ranking")]
    [Tooltip("AudioSource para sonidos de ranking")]
    [SerializeField] private AudioSource rankAudioSource;

    [Tooltip("Clip de audio para ranking S")]
    [SerializeField] private AudioClip sRankClip;

    [Tooltip("Clip de audio para ranking A")]
    [SerializeField] private AudioClip aRankClip;

    [Tooltip("Clip de audio para ranking B")]
    [SerializeField] private AudioClip bRankClip;

    [Tooltip("Clip de audio para ranking C")]
    [SerializeField] private AudioClip cRankClip;

    [Space(10)]
    [Header("Audio de Puntuación")]
    [Tooltip("AudioSource para efectos de sonido de puntuación")]
    [SerializeField] private AudioSource scoreAudioSource;

    [Tooltip("Clip de sonido que se reproduce al mostrar estadísticas")]
    [SerializeField] private AudioClip scoreClip;
    #endregion

    #region Private Fields
    /// <summary>Número de golpes fallidos del jugador</summary>
    private int puchaScore;

    /// <summary>Número de golpes buenos del jugador</summary>
    private int wenaScore;

    /// <summary>Número de golpes excelentes del jugador</summary>
    private int bakanScore;

    /// <summary>Número de golpes perfectos del jugador</summary>
    private int uyuuuyScore;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Inicializa las estadísticas del jugador y comienza las animaciones de presentación.
    /// Verifica la integridad de las referencias y configura los valores desde GlobalScore.
    /// </summary>
    private void Start()
    {
      if (setModePet == null || petSprites == null || dogSprites == null || scoreText == null || setRanking == null || rankingScore == null)
      {
        Debug.LogError("One or more serialized fields are not assigned in the Inspector.");
        return;
      }

      // Asignar valores desde GlobalScore DESPUÉS de que se haya inicializado
      puchaScore = GlobalScore.missesHit;
      wenaScore = GlobalScore.gooodHits;
      bakanScore = GlobalScore.greatHits;
      uyuuuyScore = GlobalScore.perfectHits;

      List<TextMeshProUGUI> textFields = new List<TextMeshProUGUI> { uyuuuyText, bakanText, wenaText, puchaText };
      List<int> values = new List<int> { uyuuuyScore, bakanScore, wenaScore, puchaScore };

      scoreAudioSource.volume = 0.3f;

      // Debug para verificar el score
      Debug.Log($"Score obtenido en ScorePlayerStats: {GlobalScore.score}");
      Debug.Log($"Perfect hits: {GlobalScore.perfectHits}, Great hits: {GlobalScore.greatHits}");
      Debug.Log($"Good hits: {GlobalScore.gooodHits}, Miss hits: {GlobalScore.missesHit}");

      // Iniciar todas las animaciones de forma sincronizada
      StartCoroutine(UpdateSynchronized(0, GlobalScore.score, 2.5f));
      StartCoroutine(UpdateTextsInOrder(textFields, values, 0.3f));
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Reproduce el sonido correspondiente al ranking obtenido.
    /// </summary>
    /// <param name="targetIndex">Índice del ranking (0=S, 1=A, 2=B, 3=C, 4=D, 5=E, 6=F)</param>
    private void PlayRankingSound(int targetIndex)
    {
      // 0 is S, 1 is A, 2 is B, 3 is C, 4 is D, 5 is E, 6 is F
      switch (targetIndex)
      {
        case 0:
          rankAudioSource.PlayOneShot(sRankClip); //S
          break;
        case 1:
          rankAudioSource.PlayOneShot(aRankClip);//A
          break;
        case 2:
          rankAudioSource.PlayOneShot(bRankClip);//B
          break;
        case 3:
          rankAudioSource.PlayOneShot(bRankClip);//C
          break;
        case 4:
          rankAudioSource.PlayOneShot(cRankClip);//D
          break;
        case 5:
          rankAudioSource.PlayOneShot(cRankClip);//E
          break;
        case 6:
          rankAudioSource.PlayOneShot(cRankClip);//F
          break;
        default:
          break;
      }
    }

    /// <summary>
    /// Corrutina principal que sincroniza la animación de puntuación, ranking y sprites de mascotas.
    /// Actualiza todos los elementos visuales de forma coordinada durante la duración especificada.
    /// </summary>
    /// <param name="startScore">Puntuación inicial de la animación</param>
    /// <param name="endScore">Puntuación final objetivo</param>
    /// <param name="duration">Duración total de la animación en segundos</param>
    /// <returns>Corrutina que maneja la animación sincronizada</returns>
    private IEnumerator UpdateSynchronized(int startScore, int endScore, float duration)
    {
      Debug.Log($"UpdateSynchronized iniciado: desde {startScore} hasta {endScore} en {duration} segundos");

      float elapsedTime = 0f;
      int lastRankingIndex = -1;
      int lastPetIndex = -1;
      int lastDogIndex = -1;

      while (elapsedTime < duration)
      {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, endScore, t));

        // Actualizar score
        scoreText.text = currentScore.ToString();

        // Determinar ranking actual basado en el score actual
        int currentRankingIndex = GetRankingIndex(currentScore);
        if (currentRankingIndex != lastRankingIndex)
        {
          if (currentRankingIndex < rankingScore.Count)
          {
            setRanking.sprite = rankingScore[currentRankingIndex];
            Debug.Log($"Ranking cambiado a: {GetRankingLetter(currentRankingIndex)} (índice: {currentRankingIndex})");
          }
          lastRankingIndex = currentRankingIndex;
        }

        // Determinar sprites de mascotas basado en el score actual
        var petIndices = GetPetIndices(currentScore);
        int currentPetIndex = petIndices.pet;
        int currentDogIndex = petIndices.dog;

        if (currentPetIndex != lastPetIndex && currentPetIndex < petSprites.Count)
        {
          setModePet.sprite = petSprites[currentPetIndex];
          lastPetIndex = currentPetIndex;
        }

        if (currentDogIndex != lastDogIndex && currentDogIndex < dogSprites.Count)
        {
          setModeDog.sprite = dogSprites[currentDogIndex];
          lastDogIndex = currentDogIndex;
        }

        yield return null;
      }

      // Asegurar valores finales exactos
      scoreText.text = endScore.ToString();
      int finalRankingIndex = GetRankingIndex(endScore);

      Debug.Log($"Score final: {endScore}, Ranking final: {GetRankingLetter(finalRankingIndex)}");

      if (finalRankingIndex < rankingScore.Count)
        setRanking.sprite = rankingScore[finalRankingIndex];

      var finalPetIndices = GetPetIndices(endScore);
      if (finalPetIndices.pet < petSprites.Count)
        setModePet.sprite = petSprites[finalPetIndices.pet];
      if (finalPetIndices.dog < dogSprites.Count)
        setModeDog.sprite = dogSprites[finalPetIndices.dog];

      // Reproducir sonidos finales
      scoreAudioSource.PlayOneShot(scoreClip);
      yield return new WaitForSeconds(0.3f);
      PlayRankingSound(finalRankingIndex);
    }

    /// <summary>
    /// Convierte un índice de ranking a su letra correspondiente para debug.
    /// </summary>
    /// <param name="index">Índice del ranking</param>
    /// <returns>Letra del ranking (S, A, B, C, D, E, F)</returns>
    private string GetRankingLetter(int index)
    {
      switch (index)
      {
        case 0: return "S";
        case 1: return "A";
        case 2: return "B";
        case 3: return "C";
        case 4: return "D";
        case 5: return "E";
        case 6: return "F";
        default: return "?";
      }
    }

    /// <summary>
    /// Determina el índice de ranking basado en la puntuación obtenida.
    /// </summary>
    /// <param name="score">Puntuación del jugador</param>
    /// <returns>Índice del ranking correspondiente</returns>
    private int GetRankingIndex(int score)
    {
      if (score >= 70000) return 0; // S
      else if (score >= 60000) return 1; // A
      else if (score >= 50000) return 2; // B
      else if (score >= 40000) return 3; // C
      else if (score >= 30000) return 4; // D
      else if (score >= 20000) return 5; // E
      else return 6; // F
    }

    /// <summary>
    /// Determina los índices de sprites de mascotas basado en la puntuación.
    /// </summary>
    /// <param name="score">Puntuación del jugador</param>
    /// <returns>Tupla con los índices de pet y dog</returns>
    private (int pet, int dog) GetPetIndices(int score)
    {
      if (score >= 70000) return (0, 0); // S
      else if (score >= 60000) return (1, 1); // A
      else if (score >= 50000) return (1, 2); // B
      else if (score >= 40000) return (1, 3); // C
      else if (score >= 30000) return (2, 3); // D
      else if (score >= 20000) return (2, 4); // E
      else if (score >= 10000) return (2, 5); // F
      else return (2, 5); // F (menos de 10k)
    }

    /// <summary>
    /// Anima la aparición secuencial de los textos de estadísticas con efectos de sonido.
    /// </summary>
    /// <param name="textFields">Lista de campos de texto a actualizar</param>
    /// <param name="values">Lista de valores correspondientes</param>
    /// <param name="delay">Retraso entre cada actualización</param>
    /// <returns>Corrutina que maneja la animación secuencial</returns>
    private IEnumerator UpdateTextsInOrder(List<TextMeshProUGUI> textFields, List<int> values, float delay)
    {
      for (int i = textFields.Count - 1; i >= 0; i--)
      {
        textFields[i].text = values[i].ToString();
        scoreAudioSource.PlayOneShot(scoreClip);
        yield return new WaitForSeconds(delay);
      }
    }
    #endregion
  }
}