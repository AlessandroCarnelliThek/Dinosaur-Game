using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //private int score;
    //private int hiScore;

    public bool groundIsRunning = false;
    public bool dinoIsRunning = false;
    public bool isFirstPlay = true;

    public Action callback;

    public void StartGround()
    {
        groundIsRunning = true;
    }
    public void StartDino()
    {
        dinoIsRunning = true;
    }
    private void StartGame()
    {
        UpdateGameState(!isFirstPlay ? GameState.GAME : GameState.TUTORIAL);
    }

    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform endPoint;

    [SerializeField] DinoCanvasTouchMovement Dino;
    [SerializeField] RandomGroundGenerator Ground;
    public Vector3 GetSpawnPoint() { return spawnPoint.position; }
    public Vector3 GetEndPoint() { return endPoint.position; }

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

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
        if (groundIsRunning) { Ground.UpdateGround(); }
    }



    public void HandleMain()
    {

    }
    public void MainTransition()
    {
        Debug.Log("GAME START");
        Dino.StartDinoIntroAnimation();
        Ground.StartGroundAnimation(() => StartGame());
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
        Time.timeScale = (Time.timeScale != 0) ? 0 : 1;
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
