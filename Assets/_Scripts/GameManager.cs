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

    #region GAME BOOLEANS
    public bool isFirstPlay = true; // if hi score == 0;
    public bool terrainIsRunning = false;
    public bool dinoIsRunning = false;

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
        //UpdateGameState(!isFirstPlay ? GameState.GAME : GameState.TUTORIAL);
        UpdateGameState(GameState.GAME);
    }
    #endregion

    #region ENVIRONMENT

    [SerializeField] DinoCanvasTouchMovement Dino;
    [SerializeField] RandomTerrainGenerator Terrain;
    #endregion

    public Action callback;

    #region SCORE
    private int score;
    private float scoreTimer = 0;

    IEnumerator UpdateScore()
    {
        scoreTimer = 0;
        WaitForSeconds ws = new WaitForSeconds(0.1f);
        if (PlayerPrefs.GetInt("hiScore", 0) == 0)
        {
            while (State != GameState.GAMEOVER)
            {
                yield return ws;
                score += 1;
                UI_Manager.instance.UpdateScorePanel(score);
            }
        }
        else
        {
            while (score < PlayerPrefs.GetInt("hiScore", 0) && State != GameState.GAMEOVER)
            {
                yield return ws;
                score += 1;
                UI_Manager.instance.UpdateScorePanel(score);
            }
        }


        UI_Manager.instance.StartScorePanelAnimation();

        while (State != GameState.GAMEOVER)
        {

            score += 1;
            UI_Manager.instance.UpdateHiScorePanel(score);
            yield return ws;
        }

        if (score > PlayerPrefs.GetInt("hiScore", 0))
        {
            PlayerPrefs.SetInt("hiScore", score);
        }

    }


    public int GetScore() { return score; }
    #endregion

    private void Awake()
    {
        // singleton
        MakeSingleton();

        //-----------------------------

        score = 0;
        Time.timeScale = 1;
        PlayerPrefs.SetInt("hiScore", 100);
        //-----------------------------
    }
    private void Start()
    {
        UpdateGameState(GameState.MAIN);
        UI_Manager.instance.UpdateScorePanel(score);
        UI_Manager.instance.InitializeHiScorePanel();
    }
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



    public void Update()
    {
        Dino.UpdateDino();
        if (terrainIsRunning) { Terrain.UpdateTerrain(); }

    }

    public void HandleMain()
    {

    }
    public void MainTransition()
    {
        Debug.Log("GAME START");
        Dino.StartDinoIntroAnimation();
        Terrain.StartTerrainAnimation(() => StartGame());
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

    }
}

public enum GameState
{
    MAIN,
    TUTORIAL, // solo la prima volta che si avvia il gioco
    GAME,
    PAUSE,
    GAMEOVER
}

