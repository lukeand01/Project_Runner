using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{


    SpriteRenderer powerRend;
    [SerializeField] TextMeshProUGUI powerText;
    [Separator("Graphics")]
    [SerializeField] Sprite healthSprite;
    [SerializeField] Sprite ammoSprite;
    [SerializeField] Sprite meterSprite;
    [Separator("SPEED")]
    [SerializeField]float speed;
    int choice;

    bool canAct;
    private void Awake()
    {
        //its random unless you are in need in one of the two.
        //the meter onee appears every 10000 * modifier
        Debug.Log("got here");

        powerRend = GetComponent<SpriteRenderer>();

      

        float random = Random.Range(0, 100);

        if(random >= 0 && random <= 40)
        {
            choice = 0;
            powerRend.sprite = healthSprite;
            powerText.gameObject.SetActive(false);
        }

        if(random > 30 && random <=50)
        {
            choice = 1;
            powerRend.sprite = ammoSprite;
            powerText.gameObject.SetActive(true);
            powerText.text = "Immune";
        }
        if(random >50 && random <= 70)
        {
            choice = 2;
            powerRend.sprite = ammoSprite;
            powerText.gameObject.SetActive(true);
            powerText.text = "Dash";
        }
        if(random > 70)
        {
            choice = 3;
            powerRend.sprite = meterSprite;
            powerText.gameObject.SetActive(false);
        }

        canAct = true;
        GameHandler.instance.EventControlEverything += Control;

    }

     void Control(bool choice)
    {
        
        canAct = choice;
    }

    private void Update()
    {
        if (!canAct) return;

        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }


    //we will choose what buff this is here.
    //its randomidezed, and in certain priority: 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Player") return;

        if(choice == 0)
        {
            PlayerHandler.instance.resource.HealHealth(1);
        }
        if(choice == 1)
        {
            PlayerHandler.instance.power.RecovereAmmo(0);
        }
        if(choice == 2)
        {
            PlayerHandler.instance.power.RecovereAmmo(1);
        }
        if(choice == 3)
        {
            //increase timere.
            GameHandler.instance.AddMeterModifier(0.05f);
        }

        Destroy(gameObject);

    }

}
