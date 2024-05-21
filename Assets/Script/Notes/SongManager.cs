using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Script.Interface;
using UnityEngine;

namespace Script.Notes
{
  public class SongManager : MonoBehaviour
  {
    public static SongManager Instance;
    // obtenemos el audiosourceBGM del SoundManager
    private AudioSource soundManagerAudioSource;
    
    [SerializeField] private Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds
    
    // Ruta de los archivos MIDI
    private const string DirectoryFile = "/Audio/MIDI_Files/";

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
      midiFile = MidiFile.Read(Application.dataPath + DirectoryFile + fileLocation);
      GetDataFromMidi();
    }

    private void GetDataFromMidi()
    {
      var notes = midiFile.GetNotes();
      var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
      notes.CopyTo(array, 0);

      foreach (var lane in lanes) lane.SetTimeStamps(array);

      Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong()
    {
      soundManagerAudioSource.Play();
    }

    public static double GetAudioSourceTime()
    {
      return (double)Instance.soundManagerAudioSource.timeSamples / Instance.soundManagerAudioSource.clip.frequency;
    }
  }
}