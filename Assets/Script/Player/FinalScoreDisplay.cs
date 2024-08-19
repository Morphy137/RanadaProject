using UnityEngine;
using TMPro;

public class FinalScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI perfectHitsText;
    public TextMeshProUGUI greatHitsText;
    public TextMeshProUGUI gooodHitsText;
    public TextMeshProUGUI missesHitText;

    void Start()
    {
        // Asigna los valores de las variables a los componentes de texto
        perfectHitsText.text = GlobalScore.perfectHits.ToString();
        greatHitsText.text = GlobalScore.greatHits.ToString();
        gooodHitsText.text = GlobalScore.gooodHits.ToString();
        missesHitText.text = GlobalScore.missesHit.ToString();
    }
}