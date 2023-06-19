using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUI : MonoBehaviour
{

    [SerializeField] Image immuneImage;
    [SerializeField] TextMeshProUGUI immuneAmmoText;
    [SerializeField] Image dashImage;
    [SerializeField] TextMeshProUGUI dashAmmoText;


    public void UpdateAmmo(int powerChoice, int ammo)
    {
        if (powerChoice == 0) immuneAmmoText.text = ammo.ToString();
        if (powerChoice == 1) dashAmmoText.text = ammo.ToString();
    }
    public void UpdateCooldown(int powerChoice, float current, float total)
    {
        if (powerChoice == 0) immuneImage.fillAmount = current / total;

        if (powerChoice == 1) dashImage.fillAmount = current / total;

    }

}
