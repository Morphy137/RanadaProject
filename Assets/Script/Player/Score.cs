using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI perfectHitsText;
    public TextMeshProUGUI greatHitsText;
    public TextMeshProUGUI gooodHitsText;
    public TextMeshProUGUI missesHitText;

    void Start()
    {
        // Recupera los valores de PlayerPrefs
        int score = PlayerPrefs.GetInt("Score", 0);
        int perfectHits = PlayerPrefs.GetInt("PerfectHits", 0);
        int greatHits = PlayerPrefs.GetInt("GreatHits", 0);
        int gooodHits = PlayerPrefs.GetInt("GooodHits", 0);
        int missesHit = PlayerPrefs.GetInt("MissesHit", 0);

        // Muestra los valores en los textos correspondientes
        scoreText.text = "Score: " + score.ToString();
        perfectHitsText.text = "Perfect Hits: " + perfectHits.ToString();
        greatHitsText.text = "Great Hits: " + greatHits.ToString();
        gooodHitsText.text = "Goood Hits: " + gooodHits.ToString();
        missesHitText.text = "Misses: " + missesHit.ToString();
    }
}