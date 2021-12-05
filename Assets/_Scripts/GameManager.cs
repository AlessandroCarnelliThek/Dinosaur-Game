using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void MakeSingleton()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    #region POINT OF INTEREST
    [SerializeField] Transform terrainSpawnPoint;
    //[SerializeField] Transform cactusSpawnPoint;
    //[SerializeField] Transform enemySpawnPoint;
    //[SerializeField] Transform cloudsSpawnPoint;
    [SerializeField] Transform endPoint;
    public Vector3 GetTerrainSpawnPoint() { return terrainSpawnPoint.position; }
    public Vector3 GetEndPoint() { return endPoint.position; }
    #endregion

    public bool isFirstPlay = true; // if hi score == 0;
    public bool terrainIsRunning = false;
    public bool dinoIsRunning = false;

    [SerializeField] DinoCanvasTouchMovement Dino;
    [SerializeField] RandomTerrainGenerator Terrain;

    public Action callback;

    private int score;

    // AWAKE
    private void Awake() { MakeSingleton(); }

    // START
    private void Start() { UpdateGameState(GameState.MAIN); }

    // UPDATE
    public void Update()
    {
        if (dinoIsRunning) { Dino.UpdateDino(); }
        if (terrainIsRunning) { Terrain.UpdateTerrain(); }

    }

    // UPDATE GAME STATE
    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case (GameState.MAIN):
                HandleMain();
                break;
            case (GameState.TUTORIAL):
                HandleTutorial();
                break;
            case (GameState.GAME):
                HandleGame();
                break;
            case (GameState.PAUSE):
                HandlePause();
                break;
            case (GameState.GAMEOVER):
                HandleGameOver();
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }
    public void HandleMain()
    {
        PlayerPrefs.SetInt("hiScore", 100);
        score = 0;
        Time.timeScale = 1;
        UI_Manager.instance.UpdateScorePanel(score);
        UI_Manager.instance.InitializeHiScorePanel();
    }
    public void HandleTutorial()
    {
        //joystick animation:

        //#_JUMP PULSE
        //viene spawnato un cactus per il salto

        //#_CROUCH PULSE
        //viene spawnato uno ptero per l'accovacciamento

        //start game
        //UpdateGameState(GameState.GAME);
    }
    public void HandleGame()
    {
        if (score == 0)
        {
            StartCoroutine(UpdateScore());
        }
    }
    public void HandlePause()
    {
    }
    public void HandleGameOver()
    {
        AudioManager.instance.Play("gameOver");

    }

    // METHOD
    public void StartGround()
    {
        terrainIsRunning = true;
    }
    public void StartDino()
    {
        dinoIsRunning = true;
    }
    private void StartGame()
    {
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!! da implementare con tutorial
        //UpdateGameState((PlayerPrefs.GetInt("hiScore", 0) > 0) ? GameState.GAME : GameState.TUTORIAL);
        UpdateGameState(GameState.GAME);
    }
    public void MainTransition()
    {
        Debug.Log("GAME START");
        Dino.StartDinoIntroAnimation(() => Terrain.StartTerrainAnimation(() => StartGame()));
        ;
    }

    IEnumerator UpdateScore()
    {
        WaitForSeconds ws = new WaitForSeconds(0.1f);
        
        if (PlayerPrefs.GetInt("hiScore", 0) == 0)
        {
            while (State != GameState.GAMEOVER)
            {
                yield return ws;
                score += 1;
                UI_Manager.instance.UpdateScorePanel(score);
                if (score % 100 == 0)
                {
                    AudioManager.instance.Play("score");
                }
            }
        }
        else
        {
            while (score < PlayerPrefs.GetInt("hiScore", 0) && State != GameState.GAMEOVER)
            {
                yield return ws;
                score += 1;
                UI_Manager.instance.UpdateScorePanel(score);
                if (score % 100 == 0)
                {
                    AudioManager.instance.Play("score");
                }
            }
        }
        
        UI_Manager.instance.StartScorePanelAnimation();
        AudioManager.instance.Play("hiScore");

        while (State != GameState.GAMEOVER)
        {
            score += 1;
            UI_Manager.instance.UpdateHiScorePanel(score);
            if (score % 100 == 0)
            {
                AudioManager.instance.Play("score");
            }
            yield return ws;
        }

        if (score > PlayerPrefs.GetInt("hiScore", 0))
        {
            PlayerPrefs.SetInt("hiScore", score);
        }
    }
    public int GetScore() { return score; }
}

public enum GameState
{
    MAIN,
    TUTORIAL, // solo la prima volta che si avvia il gioco
    GAME,
    PAUSE,
    GAMEOVER
}

