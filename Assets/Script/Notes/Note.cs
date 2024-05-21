using UnityEngine;

namespace Script.Notes
{
  public class Note : MonoBehaviour
  {
    double timeInstantiated;
    public float assignedTime;

    void Start()
    {
      timeInstantiated = SongManager.GetAudioSourceTime(); // Get the current time of the audio source
    }

    void Update()
    {
      double timeSinceInstantiated =
        SongManager.GetAudioSourceTime() - timeInstantiated; // Get the time since the note was instantiated
      float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

      if (t > 1)
      {
        Destroy(gameObject);
      }
      else
      {
        transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnY,
          Vector3.right * SongManager.Instance.noteDespawnY, t);
        GetComponent<SpriteRenderer>().enabled = true;
      }
    }
  }
}