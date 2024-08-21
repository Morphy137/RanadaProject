
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Interface
{
  public class SceneLoader : MonoBehaviour
  {
    public static SceneLoader Instance;

    public void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
        DontDestroyOnLoad(gameObject);
      }
      else
      {
        Destroy(gameObject);
      }
    }

    // public void ChangeToScoreScene()
    // {
    //   SceneManager.LoadScene("ScoreScreen");
    // }
  }
}