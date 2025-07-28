using Script.Interface;
using Script.Notes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
  /// <summary>
  /// Gestor principal de entradas del jugador para el juego de ritmo.
  /// Maneja la detección de teclas, cambios visuales de sprites y comunicación
  /// con las pistas (lanes) para procesar los golpes de notas.
  /// Integra el nuevo sistema de Input System de Unity.
  /// </summary>
  public class InputManager : MonoBehaviour
  {
    #region Serialized Fields
    [Header("Configuración de Pistas")]
    [Tooltip("Primera pista que recibirá las entradas del jugador")]
    [SerializeField] private Lane lane1;

    [Tooltip("Segunda pista que recibirá las entradas del jugador")]
    [SerializeField] private Lane lane2;

    [Header("Componentes de Interfaz")]
    [Tooltip("Componente de menú de pausa para gestionar la pausa del juego")]
    [SerializeField] private MenuPause menuPause;

    [Tooltip("Array de objetos hijos que cambiarán de sprite al presionar teclas")]
    [SerializeField] private GameObject[] objects;

    [Header("Sprites de Retroalimentación Visual")]
    [Tooltip("Sprite mostrado cuando la tecla no está presionada")]
    [SerializeField] private Sprite defaultSprite;

    [Tooltip("Sprite mostrado cuando la tecla está presionada")]
    [SerializeField] private Sprite pressedSprite;
    #endregion

    #region Private Fields
    /// <summary>Referencia al sistema de entrada del jugador</summary>
    private PlayerInput playerInput;

    /// <summary>Estado de presión de la tecla de la primera pista</summary>
    private bool lane1KeyPressed;

    /// <summary>Estado de presión de la tecla de la segunda pista</summary>
    private bool lane2KeyPressed;

    /// <summary>Array de renderizadores de sprites para los objetos visuales</summary>
    private SpriteRenderer[] spriteRenderers;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// Inicializa el sistema de entrada y configura los eventos de input.
    /// Establece las conexiones entre teclas y acciones del juego.
    /// </summary>
    private void Awake()
    {
      playerInput = new PlayerInput();

      // Configurar eventos de entrada para las pistas
      playerInput.Player.Lane1Key.performed += ctx => lane1.OnInputPressed();
      playerInput.Player.Lane2Key.performed += ctx => lane2.OnInputPressed();

      // Configurar evento de pausa
      playerInput.Player.Pause.canceled += ctx => menuPause.TogglePause();

      // Configurar sonidos de click
      playerInput.Player.Click.performed += ctx => SoundManager.Instance.PlayClickSound();
    }

    /// <summary>
    /// Inicializa los componentes de renderizado de sprites para feedback visual.
    /// </summary>
    private void Start()
    {
      spriteRenderers = new SpriteRenderer[objects.Length];
      for (int i = 0; i < objects.Length; i++)
      {
        spriteRenderers[i] = objects[i].GetComponent<SpriteRenderer>();
      }
    }

    /// <summary>
    /// Actualiza el estado visual de las pistas basándose en las entradas del jugador.
    /// </summary>
    private void Update()
    {
      HandleLaneInput(lane1, lane1KeyPressed, 0);
      HandleLaneInput(lane2, lane2KeyPressed, 1);
    }

    /// <summary>
    /// Habilita el sistema de entrada cuando el objeto se activa.
    /// </summary>
    private void OnEnable()
    {
      playerInput.Player.Enable();
    }

    /// <summary>
    /// Deshabilita el sistema de entrada cuando el objeto se desactiva.
    /// </summary>
    private void OnDisable()
    {
      playerInput.Player.Disable();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Maneja la entrada de una pista específica y actualiza su representación visual.
    /// </summary>
    /// <param name="lane">Pista objetivo para procesar la entrada</param>
    /// <param name="keyPressed">Estado actual de la tecla (presionada/liberada)</param>
    /// <param name="spriteIndex">Índice del sprite a actualizar en el array</param>
    private void HandleLaneInput(Lane lane, bool keyPressed, int spriteIndex)
    {
      if (lane != null && spriteRenderers[spriteIndex] != null)
      {
        if (keyPressed)
        {
          spriteRenderers[spriteIndex].sprite = pressedSprite;
          lane.OnInputPressed(); // Método para manejar la entrada presionada en el script de Lane
        }
        else
        {
          spriteRenderers[spriteIndex].sprite = defaultSprite;
          lane.OnInputReleased(); // Método para manejar la entrada liberada en el script de Lane
        }
      }
    }
    #endregion
  }
}