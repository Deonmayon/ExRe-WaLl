using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthHeartManager : MonoBehaviour
{
    //[SerializeField] int max_health = 3;
    public static int health;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    [Space]
    [SerializeField] GameObject summary;
    public static int score;
    [SerializeField] GameObject ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        health = hearts.Length;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Image image in hearts)
        {
            image.sprite = emptyHeart;
        }
        for (int i = 0; i < health; i++){
            hearts[i].sprite = fullHeart;
        }
        ScoreText.GetComponent<Text>().text = " Score: " + score;
        summary.GetComponent<TextMeshProUGUI>().text = score + " Walls Passed";
    }
}
