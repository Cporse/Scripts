using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text levelText;

    private int score = 0;
    private int level = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void ScoreCount()
    {
        score++;
        scoreText.text = "Score : " + score.ToString();
    }

    //public void LevelCount()
    //{
    //    level++;
    //    levelText.text = "Level : " + level.ToString();
    //}
}