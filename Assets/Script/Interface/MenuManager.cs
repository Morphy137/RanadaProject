using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Interface
{
  /// <summary>
  /// Gestiona la funcionalidad principal del menú del juego.
  /// Controla la navegación entre escenas, efectos visuales de fondo y operaciones del menú principal.
  /// Implementa el patrón singleton para acceso global.
  /// </summary>
  public class MenuManager : MonoBehaviour
  {
    #region Serialized Fields
    [Header("Configuración de Fondo")]
    [Tooltip("Imagen de fondo que se animará con movimiento UV")]
    [SerializeField] private RawImage fondo;

    [Tooltip("Velocidad de desplazamiento horizontal del fondo")]
    [SerializeField] private float horizontal;

    [Tooltip("Velocidad de desplazamiento vertical del fondo")]
    [SerializeField] private float vertical;
    #endregion

    #region Private Fields
    /// <summary>Nombre de la escena principal del juego</summary>
    private String _sceneGame = "Game";

    /// <summary>Nombre de la escena del menú principal</summary>
    private String _sceneMainMenu = "MenuPrincipal";

    /// <summary>Diccionario para almacenar las escalas originales de GameObjects</summary>
    private Dictionary<GameObject, Vector3> originalScales = new();

    /// <summary>Instancia singleton del MenuManager</summary>
    private static MenuManager instance;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Inicializa el patrón singleton asegurando una única instancia.
    /// </summary>
    private void Awake()
    {
      if (instance == null)
      {
        instance = this;
      }
    }

    /// <summary>
    /// Actualiza la animación del fondo desplazando las coordenadas UV continuamente.
    /// </summary>
    private void Update()
    {
      fondo.uvRect = new Rect(fondo.uvRect.position + new Vector2(horizontal, vertical), fondo.uvRect.size);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Continúa el juego cargando la escena principal del juego.
    /// Restablece el timeScale a valor normal antes de la transición.
    /// </summary>
    public void ContinueGame()
    {
      Time.timeScale = 1;

      Debug.Log("Continue Game");
      SceneManager.LoadScene(_sceneGame);
    }

    /// <summary>
    /// Inicia un nuevo juego cargando la escena principal desde el inicio.
    /// Reinicia el sistema de audio y restablece el timeScale.
    /// </summary>
    public void NewGame()
    {
      Time.timeScale = 1;

      Debug.Log(_sceneGame);
      SoundManager.Instance.ResetAudioSource();
      SceneManager.LoadScene(_sceneGame);
    }

    /// <summary>
    /// Navega de vuelta al menú principal del juego.
    /// </summary>
    public void GoToMainMenu()
    {
      Debug.Log("Ir al Menu Principal");
      SceneManager.LoadScene(_sceneMainMenu);
    }

    /// <summary>
    /// Cierra completamente la aplicación del juego.
    /// En el editor, esta función no tendrá efecto.
    /// </summary>
    public void QuitGame()
    {
      Debug.Log("Quit Game");
      Application.Quit();
    }
    #endregion
  }
}
