using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextController : MonoBehaviour
{
    public GameObject player;
    public GameObject hpContainer;
    public GameObject scoreContainer;
    
    private RectTransform rectHp;

    private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        rectHp = hpContainer.GetComponent<RectTransform>();
        scoreText = scoreContainer.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        int hp = player.GetComponent<PlayerController>().getHp();
        int score = player.GetComponent<PlayerController>().getScore();

        float dim = ((float) hp / 50) * 900;

        rectHp.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dim);

        scoreText.text = "SCORE: " + score.ToString();
    }
}
