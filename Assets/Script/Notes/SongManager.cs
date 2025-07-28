using System.Collections;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Script.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Notes
{
  public class SongManager : MonoBehaviour
  {
    public static SongManager Instance;
    // obtenemos el audiosourceBGM del SoundManager
    private AudioSource soundManagerAudioSource;
    private AudioSource menuSource;

    [SerializeField] private Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds



    // Ruta de los archivos MIDI
    private const string DirectoryFile = "/StreamingAssets/MIDI_Files/";

    public int inputDelayInMilliseconds;

    [SerializeField] private string fileLocation;

    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;

    public float noteDespawnY => noteTapY - (noteSpawnY - noteTapY);

    public static MidiFile midiFile;

    void Start()
    {
      if (SoundManager.Instance != null)
      {
        // definimos que queremos usar el audiosource de la música de fondo
        soundManagerAudioSource = SoundManager.Instance.GetBgmSource();
        soundManagerAudioSource.Pause(); // Pausa la música de fondo
        menuSource = SoundManager.Instance.GetMenuSource();
        menuSource.Play();
        StartCoroutine(SoundManager.Instance.AdjustVolumeOverTime(menuSource, 0.1f, 0.5f));
        GlobalScore.songStarted = false;
      }
      else
      {
        Debug.LogError("SoundManager no se ha inicializado");
      }

      if (Instance == null)
      {
        Instance = this;
      }
      ReadFromFile();
    }

    private void ReadFromFile()
    {
      try
      {
        string fullPath = Application.dataPath + DirectoryFile + fileLocation;
        if (System.IO.File.Exists(fullPath))
        {
          midiFile = MidiFile.Read(fullPath);
          GetDataFromMidi();
        }
        else
        {
          Debug.LogError($"MIDI file not found: {fullPath}");
        }
      }
      catch (System.Exception e)
      {
        Debug.LogError($"Error reading MIDI file: {e.Message}");
      }
    }

    private void GetDataFromMidi()
    {
      var notes = midiFile.GetNotes();
      var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
      notes.CopyTo(array, 0);

      foreach (var lane in lanes) lane.SetTimeStamps(array);

      GlobalScore.totalNotes = notes.Count;
      //Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void InitSong()
    {
      Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong()
    {
      soundManagerAudioSource.Play();
      menuSource.Stop();
      GlobalScore.songStarted = true;

      StartCoroutine(TimeAfterChangeScene());
    }

    public void ChangeScene()
    {
      Debug.Log("Entrando en ChangeScene");
      SceneManager.LoadScene("ScoreScreen");
    }

    public static double GetAudioSourceTime()
    {
      return (double)Instance.soundManagerAudioSource.timeSamples / Instance.soundManagerAudioSource.clip.frequency;
    }

    private IEnumerator TimeAfterChangeScene()
    {

      while (soundManagerAudioSource.isPlaying != MenuPause.IsPaused)
      {
        yield return null;
      }

      Debug.Log("Audio ha terminado de reproducirse");

      yield return new WaitForSeconds(1f);
      ChangeScene();
    }
  }
}