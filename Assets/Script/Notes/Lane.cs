using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using Script.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Notes
{
  public class Lane : MonoBehaviour
  {
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public GameObject notePrefab;
    public List<Note> notes = new();
    public List<Note> foods = new();
    public List<double> timeStamps = new();

    // Floating Text Prefabs
    public GameObject PerfectPrefab;
    public GameObject GreatPrefab;
    public GameObject GoodPrefab;
    public GameObject MissPrefab;
    // Circle Prefab
    public GameObject circlePrefab;
    // Food Prefab
    [SerializeField] private List<GameObject> foodPrefabs; // Lista de prefabs de comida
    
    private Vector3 ratingSpawnPos = new(-12f, 2f, 0f);
    
    // Sprite Options
    public Sprite[] spriteOptions;

    private int spawnIndex;
    private int inputIndex;

    // Coldown for the input
    private bool isCooldownActive;
    private float cooldownDuration = 0.5f;
    private float cooldownTimer;

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

    void Update()
    {
      HandleNoteSpawning();
    }

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

    private void HandleNoteInput(bool inputPressed)
    {
      if (inputIndex < timeStamps.Count)
      {
        double timeStamp = timeStamps[inputIndex];
        double marginOfError = SongManager.Instance.marginOfError;
        double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

        if (inputPressed)
        {
          if (Math.Abs(audioTime - timeStamp) < marginOfError)
          {
            Hit(notes[inputIndex].gameObject.transform.position.x + 4);
            Debug.Log($"Hit on {inputIndex} note");
            Destroy(notes[inputIndex].gameObject);
            Destroy(foods[inputIndex].gameObject);
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
    
    public void OnInputPressed()
    {
      HandleNoteInput(true);
    }

    public void OnInputReleased()
    {
      HandleNoteInput(false);
    }
    
    /*
     * Handle the hit event when the note is hit
     * @param position: The position of the note
     */
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
        //StartCoroutine(AnimatePrefab(prefab));
      }
      else if (position is <= 1 and >= -1)
      {
        GlobalScore.greatHits++;
        prefab = Instantiate(GreatPrefab, ratingSpawnPos, Quaternion.identity);
        //StartCoroutine(AnimatePrefab(prefab));
      }
      else
      {
        GlobalScore.gooodHits++;
        prefab = Instantiate(GoodPrefab, ratingSpawnPos, Quaternion.identity);
        //StartCoroutine(AnimatePrefab(prefab));
      }

      StartCoroutine(DestroyAfterDelay(prefab, 3f));
    }

    /*
     * Handle the miss event when the note is missed
     */
    private void Miss(bool isInputMiss)
    {
      ScoreManager.Miss(isInputMiss);
      GlobalScore.missesHit++;
      // Floating Text for Miss
      GameObject prefab = Instantiate(MissPrefab, ratingSpawnPos, Quaternion.identity);
      //StartCoroutine(AnimatePrefab(prefab));
      StartCoroutine(DestroyAfterDelay(prefab, 3f));
      //if (inputIndex >= 15) {
        //SceneManager.LoadScene("ScoreScreen");
      //}
    }

    
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
  }
}