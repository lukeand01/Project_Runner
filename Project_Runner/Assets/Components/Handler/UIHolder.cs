using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHolder : MonoBehaviour
{
    public static UIHolder instance;

    public DeathUI death;
    public PlayerGUI player;
    public CometWarnerUI cometWarner;
    public PowerUI power;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
}
