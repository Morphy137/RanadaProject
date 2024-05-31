using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Script.Interface
{
  public class MenuFinalScore : MonoBehaviour
  {
    
    private String _sceneGame = "Game";
    private string _sceneMenuPrincipal = "MenuPrincipal";
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();
    
    private static MenuFinalScore instance;
    
    private void Awake()
    {
      if (instance == null)
      {
        instance = this;
      }
    }
    
    public void ContinueGame()
    {
      Debug.Log("Continue Game");
      SceneManager.LoadScene("Game");
    }
    
        public void GoToMainMenu()
    {
        Debug.Log("Ir al Menu Principal");
        SceneManager.LoadScene(_sceneMenuPrincipal);
    }
    
    public void NewGame()
    {
      Debug.Log(_sceneGame);
      SceneManager.LoadScene(_sceneGame);
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
