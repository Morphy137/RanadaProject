
using System.Collections;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Script.Interface;
using Script.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Notes
{
  /// <summary>
  /// Gestor principal del sistema de canciones en el juego de ritmo.
  /// Controla la carga y reproducción de archivos MIDI, maneja la sincronización de audio,
  /// y gestiona las transiciones entre estados del juego y escenas.
  /// Implementa el patrón singleton para acceso global.
  /// </summary>
  public class SongManager : MonoBehaviour
  {
    #region Singleton
    /// <summary>Instancia singleton del SongManager</summary>
    public static SongManager Instance;
    #endregion

    #region Private Fields
    /// <summary>AudioSource de música de fondo obtenido del SoundManager</summary>
    private AudioSource soundManagerAudioSource;

    /// <summary>AudioSource de música de menú obtenido del SoundManager</summary>
    private AudioSource menuSource;
    #endregion

    #region Serialized Fields
    [Header("Configuración de Pistas")]
    [Tooltip("Array de pistas (lanes) que manejarán las notas")]
    [SerializeField] private Lane[] lanes;

    [Header("Configuración de Tiempo")]
    [Tooltip("Retraso en segundos antes de iniciar la canción")]
    public float songDelayInSeconds;

    [Tooltip("Margen de error en segundos para la detección de golpes")]
    public double marginOfError;

    [Tooltip("Retraso de entrada en milisegundos para compensar latencia")]
    public int inputDelayInMilliseconds;

    [Header("Configuración de Notas")]
    [Tooltip("Tiempo en segundos que toma una nota en moverse por la pantalla")]
    public float noteTime;

    [Tooltip("Posición Y donde aparecen las notas")]
    public float noteSpawnY;

    [Tooltip("Posición Y donde deben ser golpeadas las notas")]
    public float noteTapY;

    [Header("Archivo MIDI")]
    [Tooltip("Nombre del archivo MIDI a cargar (sin extensión)")]
    [SerializeField] private string fileLocation;
    #endregion

    #region Properties
    /// <summary>Posición Y donde las notas desaparecen después de ser perdidas</summary>
    public float noteDespawnY => noteTapY - (noteSpawnY - noteTapY);

    /// <summary>Archivo MIDI cargado actualmente</summary>
    public static MidiFile midiFile;
    #endregion

    #region Constants
    /// <summary>Ruta del directorio donde se encuentran los archivos MIDI</summary>
    private const string DirectoryFile = "/StreamingAssets/MIDI_Files/";
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Inicializa el SongManager, establece las referencias de audio y carga el archivo MIDI.
    /// Configura el sistema de audio y prepara el estado inicial del juego.
    /// </summary>
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
    #endregion

    #region Public Methods
    /// <summary>
    /// Inicia el proceso de reproducción de la canción con el retraso configurado.
    /// </summary>
    public void InitSong()
    {
      Invoke(nameof(StartSong), songDelayInSeconds);
    }

    /// <summary>
    /// Comienza la reproducción de la canción y activa el estado de juego.
    /// Detiene la música del menú y inicia el monitoreo para el cambio de escena.
    /// </summary>
    public void StartSong()
    {
      soundManagerAudioSource.Play();
      menuSource.Stop();
      GlobalScore.songStarted = true;

      StartCoroutine(TimeAfterChangeScene());
    }

    /// <summary>
    /// Cambia a la escena de puntuación al finalizar la canción.
    /// </summary>
    public void ChangeScene()
    {
      Debug.Log("Entrando en ChangeScene");
      SceneManager.LoadScene("ScoreScreen");
    }

    /// <summary>
    /// Obtiene el tiempo actual de reproducción del AudioSource en segundos.
    /// </summary>
    /// <returns>Tiempo de reproducción actual en segundos</returns>
    public static double GetAudioSourceTime()
    {
      return (double)Instance.soundManagerAudioSource.timeSamples / Instance.soundManagerAudioSource.clip.frequency;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Lee y carga el archivo MIDI especificado desde el directorio de StreamingAssets.
    /// </summary>
    private void ReadFromFile()
    {
      midiFile = MidiFile.Read(Application.dataPath + DirectoryFile + fileLocation);
      GetDataFromMidi();
    }

    /// <summary>
    /// Extrae y procesa los datos del archivo MIDI cargado.
    /// Distribuye las notas a las pistas correspondientes y configura el total de notas.
    /// </summary>
    private void GetDataFromMidi()
    {
      var notes = midiFile.GetNotes();
      var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
      notes.CopyTo(array, 0);

      foreach (var lane in lanes) lane.SetTimeStamps(array);

      GlobalScore.totalNotes = notes.Count;
    }

    /// <summary>
    /// Corrutina que monitorea el estado de la reproducción de audio y gestiona el cambio de escena.
    /// Se ejecuta hasta que la canción termine o el juego sea pausado.
    /// </summary>
    /// <returns>Corrutina que maneja la transición de escena</returns>
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
    #endregion
  }
}