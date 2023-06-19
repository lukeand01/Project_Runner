using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    [Separator("Meter")]
    [SerializeField] TextMeshProUGUI meterText;
    public void UpdateMeter(float value)
    {
        meterText.text = value.ToString("F" + 2) + "m";
    }

    [Separator("Pause")]
    [SerializeField] GameObject pauseHolder;
    public void PauseGame()
    {
        if (pauseHolder.activeInHierarchy)
        {
            Time.timeScale = 1;
            pauseHolder.SetActive(false);
            PlayerHandler.instance.block.RemoveBlock("Pause");
        }
        else
        {
            Time.timeScale = 0;
            pauseHolder.SetActive(true);
            PlayerHandler.instance.block.AddBlock("Pause", BlockClass.BlockType.Partial);
        }
    }


    [Separator("Health")]
    [SerializeField] Transform healthContainer;
    [SerializeField] GameObject fullHeartTemplate;
    [SerializeField] GameObject emptyHeartTemplate;

    public void UpdateHealth(int current, int total)
    {
        int lostHealth = current - total;

        ClearUI();

        for (int i = 0; i < current; i++)
        {
            CreateUI(fullHeartTemplate);
        }
        for (int i = 0; i < lostHealth; i++)
        {
            CreateUI(emptyHeartTemplate);
        }
    }

    void ClearUI()
    {
        for (int i = 0; i < healthContainer.childCount; i++)
        {
            Destroy(healthContainer.GetChild(i).gameObject);
        }

        
    }
    void CreateUI(GameObject template)
    {
        GameObject newObject = Instantiate(template, healthContainer.position, Quaternion.identity);
        newObject.transform.parent = healthContainer;
    }

    public void ReturnToMenu()
    {
        GameHandler.instance.loader.ChangeScene((int)SceneIndex.MainMenu);
    }
}
