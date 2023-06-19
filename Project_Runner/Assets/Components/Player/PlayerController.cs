using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //create the game for both computer and tablet.
    PlayerHandler handler;
    Vector2 targetPos;
    [SerializeField] float speed;
    [SerializeField] float maxDistance;
    [SerializeField] float increment;

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
    }

    public void ResetControl()
    {
        targetPos = transform.position;
    }

    private void Start()
    {
        SetUpKeys();
    }
    #region KEYCODES
    KeyCode keyMoveUp;
    KeyCode keyMoveUpAlt;
    KeyCode keyMoveDown;
    KeyCode keyMoveDownAlt;
    KeyCode keyMoveRight;
    KeyCode keyMoveRightAlt;
    KeyCode keyMoveLeft;
    KeyCode keyMoveLeftAlt;
    KeyCode keyPause;
    KeyCode keyDash;
    KeyCode keyImmune;

    public KeyCode GetKey(string id)
    {
        switch (id)
        {
            case "MoveUp":
                return keyMoveUp;
            case "MoveUpAlt":
                return keyMoveUpAlt;
            case "MoveDown":
                return keyMoveDown;
            case "MoveDownAlt":
                return keyMoveDownAlt;
            case "MoveRight":
                return keyMoveRight;
            case "MoveRightAlt":
                return keyMoveRightAlt;
            case "MoveLeft":
                return keyMoveLeft;
            case "MoveLeftAlt":
                return keyMoveLeftAlt;
            case "Pause":
                return keyPause;
            case "Dash":
                return keyDash;
            case "Immune":
                return keyImmune;
        }

        return KeyCode.None;
    }

    void ChangeKey(string id, KeyCode key)
    {

    }

    void SetUpKeys()
    {
        keyMoveUp = KeyCode.W;
        keyMoveDownAlt = KeyCode.UpArrow;

        keyMoveDown = KeyCode.S;
        keyMoveDownAlt = KeyCode.DownArrow;

        keyMoveRight = KeyCode.D;
        keyMoveRightAlt = KeyCode.RightArrow;

        keyMoveLeft = KeyCode.A;
        keyMoveLeftAlt = KeyCode.LeftArrow;

        keyPause = KeyCode.Escape;

        keyImmune = KeyCode.Q;
        keyDash = KeyCode.E;
        
    }




    #endregion

    private void Update()
    {
        if (handler.block.HasBlock(BlockClass.BlockType.Complete)) return;

        PauseInput();

        if (handler.block.HasBlock(BlockClass.BlockType.Partial)) return;
        
        MoveInput();
        SkillInput();
    }

    void MoveInput()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);


        if(new Vector2(transform.position.x, transform.position.y) != targetPos)
        {
            return;
        }
       

        if(Input.GetKeyDown(GetKey("MoveUp")) || Input.GetKeyDown(GetKey("MoveUpAlt")))
        {
           if(transform.position.y + increment < maxDistance)
            {
               
                
                if(handler.power.CanUsePower(1) && Input.GetKey(GetKey("Dash")))
                {

                    float difference = Vector3.Distance(transform.position, new Vector3(transform.position.x, 4,0));
                    MoveOrder(new Vector3(0, difference, 0));
                    handler.power.UsePower(1);
                }
                else
                {
                    MoveOrder(new Vector3(0, increment, 0));
                }
            }
        }
        if (Input.GetKeyDown(GetKey("MoveDown")) || Input.GetKeyDown(GetKey("MoveDownAlt")))
        {
           if(transform.position.y - increment > -maxDistance)
            {
                

                if (handler.power.CanUsePower(1) && Input.GetKey(GetKey("Dash")))
                {
                    //then we dash to the limit.
                    float difference = Vector3.Distance(transform.position, new Vector3(transform.position.x, -4, 0));
                    MoveOrder(new Vector3(0, -difference, 0));
                    handler.power.UsePower(1);
                }
                else
                {
                    MoveOrder(new Vector3(0, -increment, 0));
                }
            }
        }

        if (Input.GetKeyDown(GetKey("MoveRight")) || Input.GetKeyDown(GetKey("MoveRightAlt")))
        {
            if (transform.position.x + increment <= 2) MoveOrder(new Vector3(increment, 0, 0));
        }
        if (Input.GetKeyDown(GetKey("MoveLeft")) || Input.GetKeyDown(GetKey("MoveLeftAlt")))
        {
            if (transform.position.x - increment >= -2) MoveOrder(new Vector3(-increment, 0, 0));
        }
    }

    void MoveOrder(Vector3 moveDir)
    {
        targetPos = transform.position + moveDir;
    }

    void PauseInput()
    {
        if (Input.GetKeyDown(GetKey("Pause")))
        {
            UIHolder.instance.player.PauseGame();
        }
    }


    public void SkillInput()
    {
        if (Input.GetKeyDown(GetKey("Immune")))
        {
            if (!handler.power.CanUsePower(0)) return;
            handler.power.UseImmunePower();
        }
    }
}
