using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Interface
{
  public class SceneLoader : MonoBehaviour
  {
    public void LoadSceneInBackground(string sceneName)
    {
      // Muestra la pantalla de carga
      LoadScreenManager.Instance.ShowLoadingScreen();

      // Comienza a cargar la escena en segundo plano
      StartCoroutine(LoadYourAsyncScene(sceneName));
    }

    private IEnumerator LoadYourAsyncScene(string sceneName)
    {
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

      // Espera hasta que la escena esté completamente cargada
      while (!asyncLoad.isDone)
      {
        float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
        
        LoadScreenManager.Instance.slider.value = progress;
        LoadScreenManager.Instance.progressText.text = progress * 100f + "%";
        
        yield return null;
      }

      // Una vez que la escena está cargada, la activa
      SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

      // Oculta la pantalla de carga
      LoadScreenManager.Instance.HideLoadingScreen();
    }
  }
}