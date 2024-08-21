
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Notes
{
  public class ScorePlayerStats : MonoBehaviour
  {
    [SerializeField] private Image setRanking;
    [SerializeField] private List<Sprite> rankingScore; // 0 is S, 1 is A, 2 is B, 3 is C, 4 is D, 5 is E, 6 is F

    [Space(10)]
    [SerializeField] private Image setModePet;
    [SerializeField] private List<Sprite> petSprites; // 0 is Perfect, 1 is Great, 2 is Bad

    [Space(10)]
    [SerializeField] private Image setModeDog;
    [SerializeField] private List<Sprite> dogSprites;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI uyuuuyText;
    [SerializeField] private TextMeshProUGUI bakanText;
    [SerializeField] private TextMeshProUGUI wenaText;
    [SerializeField] private TextMeshProUGUI puchaText;

    [Space(10)]
    [SerializeField] private AudioSource rankAudioSource;
    [SerializeField] private AudioClip sRankClip;
    [SerializeField] private AudioClip aRankClip;
    [SerializeField] private AudioClip bRankClip;
    [SerializeField] private AudioClip cRankClip;

    [Space(10)]
    [SerializeField] private AudioSource scoreAudioSource;
    [SerializeField] private AudioClip scoreClip;

    private int puchaScore = GlobalScore.missesHit;
    private int wenaScore = GlobalScore.gooodHits;
    private int bakanScore = GlobalScore.greatHits;
    private int uyuuuyScore = GlobalScore.perfectHits;

    private void Start()
    {
      if (setModePet == null || petSprites == null || dogSprites == null || scoreText == null || setRanking == null || rankingScore == null)
      {
        Debug.LogError("One or more serialized fields are not assigned in the Inspector.");
        return;
      }

      List<TextMeshProUGUI> textFields = new List<TextMeshProUGUI> { uyuuuyText, bakanText, wenaText, puchaText };
      List<int> values = new List<int> { uyuuuyScore, bakanScore, wenaScore, puchaScore };

      scoreAudioSource.volume = 0.3f;

      StartCoroutine(UpdateOverTime(0, GlobalScore.score, 2.5f));

      StartCoroutine(UpdateTextsInOrder(textFields, values, 0.3f)); // 0.2 segundos de retraso entre cada actualización

      StartCoroutine(UpdateImagePet(GlobalScore.score));
    }

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
        case 4:
          rankAudioSource.PlayOneShot(bRankClip);//C
          break;
        case 5:
          rankAudioSource.PlayOneShot(cRankClip);//D
          break;
        case 6:
          rankAudioSource.PlayOneShot(cRankClip);//E
          break;
        case 7:
          rankAudioSource.PlayOneShot(cRankClip);//F
          break;
        default:
          break;
      }
    }

    private IEnumerator UpdateOverTime(int startScore, int endScore, float duration)
    {
      float elapsedTime = 0f;
      while (elapsedTime < duration)
      {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, endScore, t));
        scoreText.text = currentScore.ToString();
        yield return null;
      }
      scoreAudioSource.PlayOneShot(scoreClip);
      scoreText.text = endScore.ToString(); // Asegurarse de que el score final sea exacto
      StartCoroutine(UpdateRanking(GlobalScore.score));
    }

    private IEnumerator UpdateRanking(int finalScore)
    {
      yield return new WaitForSeconds(0.5f); // Esperar un segundo antes de cambiar la imagen

      int targetIndex;
      if (finalScore >= 70000)
      {
        targetIndex = 0; // S
      }
      else if (finalScore >= 60000)
      {
        targetIndex = 1; // A
      }
      else if (finalScore >= 50000)
      {
        targetIndex = 2; // B
      }
      else if (finalScore >= 40000)
      {
        targetIndex = 3; // C
      }
      else if (finalScore >= 30000)
      {
        targetIndex = 4; // D
      }
      else if (finalScore >= 20000)
      {
        targetIndex = 5; // E
      }
      else
      {
        targetIndex = 6; // F
      }

      for (int i = rankingScore.Count - 1; i >= targetIndex; i--)
      {
        setRanking.sprite = rankingScore[i];
        yield return new WaitForSeconds(0.2f); // Esperar medio segundo entre cada cambio de imagen
      }

      yield return new WaitForSeconds(0.3f);
      PlayRankingSound(targetIndex);
    }

    private IEnumerator UpdateImagePet(int finalScore)
    {
      int targetPet = 2;
      int targetDog = 5;

      if (finalScore >= 70000)
      {
        targetPet = 0; // S
        targetDog = 0;
      }
      else if (finalScore >= 60000)
      {
        targetPet = 1; // A
        targetDog = 1;
      }
      else if (finalScore >= 50000)
      {
        targetPet = 1; // B
        targetDog = 2;
      }
      else if (finalScore >= 40000)
      {
        targetPet = 1; // C
        targetDog = 3;
      }
      else if (finalScore >= 30000)
      {
        targetPet = 2; // D
        targetDog = 3;
      }
      else if (finalScore >= 20000)
      {
        targetPet = 2; // E
        targetDog = 4;
      }
      else if (finalScore >= 10000)
      {
        targetPet = 2; // F
        targetDog = 5;
      }

      List<int> petIndices = new List<int>();
      List<int> dogIndices = new List<int>();

      for (int i = petSprites.Count - 1; i >= targetPet; i--)
      {
        petIndices.Add(i);
      }

      for (int i = dogSprites.Count - 1; i >= targetDog; i--)
      {
        dogIndices.Add(i);
      }

      foreach (int i in petIndices)
      {
        setModePet.sprite = petSprites[i];
        yield return new WaitForSeconds(0.3f); // Esperar medio segundo entre cada cambio de imagen
      }

      foreach (int i in dogIndices)
      {
        setModeDog.sprite = dogSprites[i];
        yield return new WaitForSeconds(0.3f); // Esperar medio segundo entre cada cambio de imagen
      }
    }

    private IEnumerator UpdateTextsInOrder(List<TextMeshProUGUI> textFields, List<int> values, float delay)
    {
      for (int i = textFields.Count - 1; i >= 0; i--)
      {
        textFields[i].text = values[i].ToString();
        scoreAudioSource.PlayOneShot(scoreClip);
        yield return new WaitForSeconds(delay);
      }
    }
  }
}