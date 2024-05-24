using UnityEngine;

namespace Script.Player
{
  public class ScoreManager : MonoBehaviour
  {
    private static ScoreManager Instance; // Singleton instance of the ScoreManager class
    public AudioSource hitSFX; // Sound effect for a successful hit
    public AudioSource missSFX; // Sound effect for a miss
    public TMPro.TextMeshProUGUI currentComboText; // TextMeshPro component for displaying the current combo
    public TMPro.TextMeshProUGUI scoreText; // TextMeshPro component for displaying the score
    public GameObject playerPrefab;
    static int comboScore; // Current combo score
    static int Score; // Total score

    private void Start()
    {
      Instance = this;
      comboScore = 0;
      Score = 0;
    }

    
    // Se llama cuando se acierta una nota
    public static void Hit()
    {
      Animator comboAnimation = Instance.currentComboText.GetComponent<Animator>();
      Animator playerAnimation = Instance.playerPrefab.GetComponent<Animator>();
      comboScore += 1;
      Score += 10 * (comboScore + 1);
      Instance.hitSFX.Play();

      // Play Combo animation once
      if (comboAnimation != null)
      {
        comboAnimation.Play("Combo_hit", -1, 0f); // Play the combo animation from the beginning
      }

      if (playerAnimation != null)
      {
        playerAnimation.Play("Rana_success", -1, 0f);
      }
    }

    // Se llama cuando se falla una nota
    public static void Miss(bool isInputMiss)
    {
      // Play Player miss animation
      Animator playerAnimation = Instance.playerPrefab.GetComponent<Animator>();
      if (playerAnimation != null)
      {
        if (isInputMiss)
          playerAnimation.Play("Rana_fail", -1, 0f);
        else
          playerAnimation.Play("Rana_fail_miss", -1, 0f);
      }

      comboScore = 0;
      Instance.missSFX.Play();
    }

    private void Update()
    {
      currentComboText.text = comboScore.ToString(); // Update the combo text display
      scoreText.text = Score.ToString(); // Update the score text display
    }
  }
}