using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    [SerializeField] SaveData save;

    [HideInInspector]public Spawner spawner;
    [HideInInspector] public SceneLoader loader;

    [SerializeField] bool startGame;

    AudioSource source;

    [Separator("BACKGROUND SOUND")]
    [SerializeField] AudioClip mainMenuBM;
    [SerializeField] AudioClip gameBM;
    [SerializeField] AudioClip deathBM;

    #region EVENTS
    public event Action<bool> EventControlEverything;
    public void OnControlEverything(bool choice) => EventControlEverything?.Invoke(choice);

    public event Action EventDestroyEverything;
    public void OnDestroyEverything() => EventDestroyEverything?.Invoke();
    #endregion
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        spawner = GetComponent<Spawner>();
        loader = GetComponent<SceneLoader>();
        

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        source.volume = 0.8f;

        if (startGame) StartGame();
    }


    public float meters;
    float meterModifier = 1;

    public float highestScore;
    public bool GameStarted;

    private void Update()
    {
        if (!GameStarted) return;

        meters += Time.deltaTime * 10 * meterModifier;
        UIHolder.instance.player.UpdateMeter(meters);

        if (meters > 100) spawner.ChangePhase(1);

        if (meters > highestScore) highestScore = meters;
    }

    



    public void AddMeterModifier(float add)
    {
        meterModifier += add;
    }

    public void StartGame()
    {
        if (PlayerHandler.instance == null)
        {
            StartBackgroundMusic(0);
            return;
        }
        StartBackgroundMusic(1);
        meterModifier = 1;
        PlayerHandler.instance.StartGame();
        meters = 0;
        spawner.StartSpawn();
        PlayerHandler.instance.block.ClearBlock();

        GameStarted = true;
    }

    public void RestartGame()
    {
        //load and unload the same thing.
        spawner.KillAll();
        StartGame();
    }

    public void ContinueGame()
    {

    }
    public void StopGame()
    {
        
        if(PlayerHandler.instance != null)PlayerHandler.instance.block.AddBlock("Stop", BlockClass.BlockType.Complete);
                   
        
        spawner.StopSpawn();
        GameStarted = false;
        OnControlEverything(false);
        save.SetHighScore(highestScore);
    }
    
    public void QuitGame()
    {
        
    }



    #region SOUND

    public void CreateSFX(AudioClip clip)
    {
        SFXUnit newObject = new GameObject().AddComponent<SFXUnit>();
        newObject.SetUp(clip);
    }

    public void StartBackgroundMusic(int choice)
    {
        //this starts again whenever it ends.
        source.Stop();

        if(choice == 0)
        {
            source.clip = mainMenuBM;
        }
        if(choice == 1)
        {
            source.clip = gameBM;
        }
        if (choice == 2)
        {
            source.clip = deathBM;
        }

        source.Play();
    }

    

    #endregion
}
