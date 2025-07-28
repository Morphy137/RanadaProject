using System;
using Script.Animation;
using UnityEngine;

namespace Script.Player
{
  /// <summary>
  /// Gestor central del sistema de puntuación y combos del juego de ritmo.
  /// Maneja la lógica de scoring, efectos de sonido, animaciones de feedback
  /// y actualización de la interfaz de usuario en tiempo real.
  /// Implementa el patrón singleton para acceso global.
  /// </summary>
  public class ScoreManager : MonoBehaviour
  {
    #region Singleton
    /// <summary>Instancia singleton del ScoreManager</summary>
    private static ScoreManager Instance;
    #endregion

    #region Serialized Fields
    [Header("Efectos de Audio")]
    [Tooltip("AudioSource para sonidos de golpe exitoso")]
    public AudioSource hitSFX;

    [Tooltip("AudioSource para sonidos de fallo")]
    public AudioSource missSFX;

    [Header("Interfaz de Usuario")]
    [Tooltip("Texto que muestra el combo actual del jugador")]
    public TMPro.TextMeshProUGUI currentComboText;

    [Tooltip("Texto que muestra la puntuación actual")]
    public TMPro.TextMeshProUGUI scoreText;

    [Header("Animaciones del Jugador")]
    [Tooltip("Prefab del jugador para reproducir animaciones de feedback")]
    public GameObject playerPrefab;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Inicializa el singleton y gestiona la persistencia de la instancia.
    /// </summary>
    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
      }
      else
      {
        Destroy(gameObject);
      }
    }

    /// <summary>
    /// Reinicia todas las estadísticas globales al comenzar una nueva partida.
    /// </summary>
    private void Start()
    {
      GlobalScore.score = 0;
      GlobalScore.totalCombo = 0;
      GlobalScore.currentCombo = 0;
      GlobalScore.highestCombo = 0;
      GlobalScore.missesHit = 0;
      GlobalScore.perfectHits = 0;
      GlobalScore.greatHits = 0;
      GlobalScore.gooodHits = 0;
    }

    /// <summary>
    /// Actualiza la interfaz de usuario con los valores actuales de combo y puntuación.
    /// </summary>
    private void Update()
    {
      if (GlobalScore.currentCombo >= 5)
      {
        currentComboText.text = GlobalScore.currentCombo.ToString(); // Update the combo text display
      }
      else
      {
        currentComboText.text = "0"; // Hide the combo text display
      }

      scoreText.text = GlobalScore.score.ToString(); // Update the score text display
    }
    #endregion

    #region Public Static Methods
    /// <summary>
    /// Procesa un golpe exitoso de nota, actualizando puntuación, combos y reproduciendo efectos.
    /// Calcula bonificaciones basadas en combos y activa animaciones de feedback positivo.
    /// </summary>
    public static void Hit()
    {
      Animator comboAnimation = Instance.currentComboText.GetComponent<Animator>();
      Animator playerAnimation = Instance.playerPrefab.GetComponent<Animator>();

      // Actualizar combo y estadísticas
      GlobalScore.currentCombo += 1;
      GlobalScore.highestCombo = GlobalScore.currentCombo > GlobalScore.highestCombo
        ? GlobalScore.currentCombo : GlobalScore.highestCombo;

      // Calcular puntuación con bonificación de combo
      if (GlobalScore.currentCombo >= 5)
      {
        GlobalScore.totalCombo += 1;
        GlobalScore.score += 10 * GlobalScore.currentCombo;
      }
      else
      {
        GlobalScore.score += 10;
      }

      // Activar efectos visuales y de audio
      ScorePulse.Instance.Pulse();
      Instance.hitSFX.Play();

      // Reproducir animaciones de feedback
      if (comboAnimation != null)
      {
        comboAnimation.Play("Combo_hit", -1, 0f); // Play the combo animation from the beginning
      }

      if (playerAnimation != null)
      {
        playerAnimation.Play("Rana_success", -1, 0f);
      }
    }

    /// <summary>
    /// Procesa un fallo de nota, reiniciando el combo y reproduciendo efectos de feedback negativo.
    /// Diferencia entre fallos por entrada incorrecta y fallos por tiempo agotado.
    /// </summary>
    /// <param name="isInputMiss">True si el fallo fue por entrada incorrecta, false si fue por tiempo agotado</param>
    public static void Miss(bool isInputMiss)
    {
      // Reproducir animación de fallo del jugador
      Animator playerAnimation = Instance.playerPrefab.GetComponent<Animator>();
      if (playerAnimation != null)
      {
        if (isInputMiss)
          playerAnimation.Play("Rana_fail", -1, 0f);
        else
          playerAnimation.Play("Rana_fail_miss", -1, 0f);
      }

      // Reiniciar combo y reproducir sonido de fallo
      GlobalScore.currentCombo = 0;
      Instance.missSFX.Play();
    }
    #endregion
  }
}