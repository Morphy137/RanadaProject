using UnityEngine;

namespace Script.Interface
{
  public class DontDestroyObject : MonoBehaviour
  {
    private static DontDestroyObject instance;

    private void Awake()
    {
      if (instance == null)
      {
        instance = this;
        DontDestroyOnLoad(gameObject);
      }
      else
      {
        Destroy(gameObject);
      }
    }
  }
}