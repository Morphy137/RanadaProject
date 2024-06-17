using UnityEngine;
using UnityEngine.UI;
public class GlobalScore : MonoBehaviour
{
    public static int score;        // El puntaje
    public static int totalCombo;   // El total de combos
    public static int currentCombo; // El combo actual
    public static int highestCombo; // El combo más alto
    public static int missesHit;    // Pucha
    public static int perfectHits;  // Uyuuuy
    public static int greatHits;    // Bakan
    public static int gooodHits;    // Wena
    public static int totalNotes;   // Notas totales

        // Método para guardar el puntaje
    public static void SaveValues()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("PerfectHits", perfectHits);
        PlayerPrefs.SetInt("GreatHits", greatHits);
        PlayerPrefs.SetInt("GooodHits", gooodHits);
        PlayerPrefs.SetInt("MissesHit", missesHit);
    }
}
