// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Script.Interface
// {
//   public class LoadScreenManager : MonoBehaviour
//   {
//     public GameObject loadingScreen; // Referencia a tu pantalla de carga
//     public Slider slider; // Referencia a la barra de progreso
//     public TextMeshProUGUI progressText; // Referencia al texto de progreso
//
//     public static LoadScreenManager Instance { get; private set; }
//
//     private void Awake()
//     {
//       // Asegura que el objeto de la pantalla de carga no se destruya al cargar nuevas escenas
//       DontDestroyOnLoad(gameObject);
//
//       if (this != Instance)
//       {
//         Instance = this;
//       }
//     }
//
//
//     // NO CARGA LA PANTALLA DE CARGA (QUIZAS), REVISAR ERROR
//     public void ShowLoadingScreen()
//     {
//       // Muestra la pantalla de carga
//       loadingScreen.SetActive(true);
//       SoundManager.Instance.PlayPauseMusic();
//     }
//
//     public void HideLoadingScreen()
//     {
//       // Oculta la pantalla de carga
//       loadingScreen.SetActive(false);
//     }
//   }
// }