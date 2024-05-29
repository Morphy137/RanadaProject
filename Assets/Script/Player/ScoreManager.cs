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

    private void Start()
    {
      Instance = this;
      GlobalScore.score = 0;
      GlobalScore.totalCombo = 0;
      GlobalScore.currentCombo = 0;
      GlobalScore.highestCombo = 0;
      GlobalScore.missesHit = 0;
      GlobalScore.perfectHits = 0;
      GlobalScore.greatHits = 0;
      GlobalScore.gooodHits = 0;
    }

    
    // Se llama cuando se acierta una nota
    public static void Hit()
    {
      Animator comboAnimation = Instance.currentComboText.GetComponent<Animator>();
      Animator playerAnimation = Instance.playerPrefab.GetComponent<Animator>();
      GlobalScore.totalCombo += 1;
      GlobalScore.currentCombo += 1;
      GlobalScore.highestCombo = GlobalScore.currentCombo > GlobalScore.highestCombo ? GlobalScore.currentCombo : GlobalScore.highestCombo;
      GlobalScore.score += 10 * (GlobalScore.currentCombo + 1);
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

      GlobalScore.currentCombo = 0;
      Instance.missSFX.Play();
    }

    private void Update()
    {
      currentComboText.text = GlobalScore.currentCombo.ToString(); // Update the combo text display
      scoreText.text = GlobalScore.score.ToString(); // Update the score text display
    }
  }
}