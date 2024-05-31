using System;
using System.Collections;
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
    
    private String _sceneCredits = "Credits";
    private String _sceneGame = "Game";
    
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();
    
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
    
    public void CreditsScene()
    {
      Debug.Log(_sceneCredits);
      SceneManager.LoadScene(_sceneCredits);
    }
    
    public void QuitGame()
    {
      Debug.Log("Quit Game");
      Application.Quit();
    }

    public void ChangeSprite(GameObject button)
    {
      StartCoroutine(ChangeSpriteWithDelay(button, 0.1f));
    }

    private IEnumerator ChangeSpriteWithDelay(GameObject button, float waitTime)
    {
      // Guardar la escala original del botón si no se ha guardado antes
      if (!originalScales.ContainsKey(button))
      {
        originalScales[button] = button.transform.localScale;
      }

      // Cambiar el tamaño del sprite cuando se aprieta, para quedar con un efecto de profundidad
      button.transform.localScale = originalScales[button] * 0.95f;

      // Esperar un cierto número de segundos
      yield return new WaitForSeconds(waitTime);

      // Restaurar el tamaño original del sprite
      button.transform.localScale = originalScales[button];
    }
  }
}
