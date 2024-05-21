using System;
using System.Collections;
using Script.Interface;
using Script.Notes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
  public class InputManager : MonoBehaviour
  {
    [Header("Objetos")] 
    [SerializeField] private Lane lane1;
    [SerializeField] private Lane lane2;
    [SerializeField] private MenuPause menuPause;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject[] objects; // Arreglo de hijos
    
    [Header("Sprites")] 
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pressedSprite;

    private bool lane1KeyPressed;
    private bool lane2KeyPressed;

    private SpriteRenderer[] spriteRenderers;
    
    private void Awake()
    {
      playerInput = new PlayerInput();
      soundManager = SoundManager.Instance;
      
      playerInput.Player.Lane1Key.performed += ctx => lane1.OnInputPressed();
      
      playerInput.Player.Lane2Key.performed += ctx => lane2.OnInputPressed();
      
      playerInput.Player.Pause.canceled += ctx => menuPause.TogglePause();
      
      playerInput.Player.Click.performed += ctx => SoundManager.Instance.PlayClickSound();
    }

    private void Start()
    {
      spriteRenderers = new SpriteRenderer[objects.Length];
      for (int i = 0; i < objects.Length; i++)
      {
        spriteRenderers[i] = objects[i].GetComponent<SpriteRenderer>();
      }
    }

    private void Update()
    {
      HandleLaneInput(lane1, lane1KeyPressed, 0);
      HandleLaneInput(lane2, lane2KeyPressed, 1);
    }
    
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

    private void OnEnable()
    {
      playerInput.Player.Enable();
    }

    private void OnDisable()
    {
      playerInput.Player.Disable();
    }
  }
}