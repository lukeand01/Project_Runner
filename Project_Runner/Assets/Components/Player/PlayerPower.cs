using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerPower : MonoBehaviour
{

    //things that it can receive.
    //

    //spells are immune and dash.
    //powers are speed.

    PlayerHandler handler;

    [SerializeField]int powerImmuneAmmo;
    [SerializeField]int powerDashAmmo;

    float immuneCurrent;
    [SerializeField]float immuneTotal;

    float dashCurrent;
    [SerializeField] float dashTotal;

    [Separator("SOUND")]
    [SerializeField] AudioClip immuneClip;
    [SerializeField] AudioClip dashClip;

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
    }

    private void Start()
    {
        UIHolder.instance.power.UpdateAmmo(0, powerImmuneAmmo);
        UIHolder.instance.power.UpdateCooldown(0, immuneCurrent, immuneTotal);

        UIHolder.instance.power.UpdateAmmo(1, powerDashAmmo);
        UIHolder.instance.power.UpdateCooldown(1, dashCurrent, dashTotal);
    }

    private void Update()
    {
        if(immuneCurrent > 0)
        {
            immuneCurrent -= Time.deltaTime;
            UIHolder.instance.power.UpdateCooldown(0, immuneCurrent, immuneTotal);
        }

        if(dashCurrent > 0)
        {
            dashCurrent -= Time.deltaTime;
            UIHolder.instance.power.UpdateCooldown(1, dashCurrent, dashTotal);
        }
    }

    public void RecovereAmmo(int choice)
    {
        if(choice == 0)
        {
            powerImmuneAmmo++;
        }
        if(choice == 1)
        {
            powerDashAmmo++;
        }
    }

    public bool HasPowerAmmo(int power)
    {
        if (power == 0) return powerImmuneAmmo > 0;
        if (power == 1) return powerDashAmmo > 0;

        return false;
    }

    public bool CanUsePower(int power)
    {
        if (!HasPowerAmmo(power)) return false;

        if (power == 0) return immuneCurrent <= 0;
        if (power == 1) return dashCurrent <= 0;

        return false;
    }

    public void UseImmunePower()
    {
        UsePower(0);
        gameObject.layer = 6;
        handler.rend.color = Color.black;

        StartCoroutine(ImmuneProcess());
    }

    IEnumerator ImmuneProcess()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = 0;
        handler.rend.color = Color.white;
    }

    public void UsePower(int powerChoice)
    {
        if(powerChoice == 0)
        {
            powerImmuneAmmo -= 1;
            UIHolder.instance.power.UpdateAmmo(0, powerImmuneAmmo);
            GameHandler.instance.CreateSFX(immuneClip);
            immuneCurrent = immuneTotal;
        }

        if(powerChoice == 1)
        {
            powerDashAmmo -= 1;
            UIHolder.instance.power.UpdateAmmo(1, powerDashAmmo);
            GameHandler.instance.CreateSFX(dashClip);
            dashCurrent = dashTotal;
        }
    }
}
