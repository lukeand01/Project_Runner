using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highestScoreText;
    public void UpdateUI()
    {
        float highesScore = GameHandler.instance.highestScore;
        highestScoreText.gameObject.SetActive(highesScore > 0);
        highestScoreText.text = highesScore.ToString();
    }

    public void PlayGame()
    {
        GameHandler.instance.loader.ChangeScene((int)SceneIndex.MainScene);
    }
    public void Quit()
    {
        Application.Quit();
    }


}
