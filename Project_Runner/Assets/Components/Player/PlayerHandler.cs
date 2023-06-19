using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;

    [HideInInspector] public PlayerController controller;
    [HideInInspector] public PlayerResource resource;
    [HideInInspector] public PlayerCamera cam;
    [HideInInspector] public PlayerPower power;

    [HideInInspector]public BlockClass block;

    [HideInInspector] public SpriteRenderer rend;


    private void Awake()
    {
        block = new();

        controller = GetComponent<PlayerController>();
        resource = GetComponent<PlayerResource>();
        cam = GetComponent<PlayerCamera>();
        power = GetComponent<PlayerPower>();

        rend = transform.GetChild(0).GetComponent<SpriteRenderer>();

        instance = this;
    }

    public void StartGame()
    {
        resource.ResetResource();
        controller.ResetControl();
        cam.ResetCamera();
    }


}
