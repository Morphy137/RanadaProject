using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Interface
{
  public class MenuManager : MonoBehaviour
  {
    [SerializeField] private RawImage fondo;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;

    private String _sceneGame = "Game";
    private String _sceneMainMenu = "MenuPrincipal";
    
    private Dictionary<GameObject, Vector3> originalScales = new();
    
    private static MenuManager instance;
    
    private void Awake()
    {
      if (instance == null)
      {
        instance = this;
      }
    }

    private void Update()
    {
      fondo.uvRect = new Rect(fondo.uvRect.position + new Vector2(horizontal, vertical), fondo.uvRect.size);
    }
    
    public void ContinueGame()
    {
      Time.timeScale = 1;
      
      Debug.Log("Continue Game");
      SceneManager.LoadScene(_sceneGame);
    }
    
    public void NewGame()
    {
      Time.timeScale = 1;
      
      Debug.Log(_sceneGame);
      SoundManager.Instance.ResetAudioSource();
      SceneManager.LoadScene(_sceneGame);
      
    }

    public void GoToMainMenu()
    {
      Debug.Log("Ir al Menu Principal");
      SceneManager.LoadScene(_sceneMainMenu);
    }
    
    public void QuitGame()
    {
      Debug.Log("Quit Game");
      Application.Quit();
    }
  }
}
