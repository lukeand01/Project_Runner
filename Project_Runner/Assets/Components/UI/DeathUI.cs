using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    GameObject holder;
    [SerializeField] GameObject congratulation;
    [SerializeField] TextMeshProUGUI currentScoreText;
    [SerializeField] TextMeshProUGUI highestScoreText;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void StartDeathUI()
    {
        //we show the player and give it th option to choose what to do now.

        holder.SetActive(true);

        float highestScore = GameHandler.instance.highestScore;
        float currentScore = GameHandler.instance.meters;

        congratulation.SetActive(highestScore == currentScore);
        currentScoreText.text = "you ran " + currentScore.ToString() + " meters";
        highestScoreText.text = "your highest score is: " + highestScore.ToString();
    }

    public void CloseDeathUI()
    {
        holder.SetActive(false);
    }


    public void StartGame()
    {
        CloseDeathUI();
        GameHandler.instance.RestartGame();
        
    }
    public void BackToMenu()
    {
        GameHandler.instance.loader.ChangeScene((int)SceneIndex.MainMenu);
    }
    public void BackToDesktop()
    {
        Application.Quit();
    }
}
