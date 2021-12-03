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
    //private int score;
    //private int hiScore;
    [SerializeField] DinoCanvasTouchMovement Dino;
    [SerializeField] RandomTerrainGenerator Terrain;
    #endregion

    public Action callback;


    private void Awake()
    {
        // singleton
        MakeSingleton();

        //-----------------------------

        //score = 0;
        //hiScore = 0;
        //-----------------------------
    }
    private void Start()
    {
        UpdateGameState(GameState.MAIN);
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

    }
    public void HandlePause()
    {
//        Time.timeScale = (Time.timeScale != 0) ? 0 : 1;
    }
    public void HandleGameOver()
    {

    }









    /*
public bool GetGameIsRunning() { return gameIsRunning; }

public GameState GetGameState()
{
    return gameState;
}
public void SetGameState(GameState newGameState)
{
    gameState = newGameState;
}
*/
}

public enum GameState
{
    MAIN,
    TUTORIAL, // solo la prima volta che si avvia il gioco
    GAME,
    PAUSE,
    GAMEOVER
}
