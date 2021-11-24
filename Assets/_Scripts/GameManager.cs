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

    public Action callback;

    public void StartGround()
    {
        groundIsRunning = true;
    }
    public void StartDino()
    {
        dinoIsRunning = true;
    }

    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform endPoint;

    [SerializeField] GameObject Dino, Ground;
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







    public void HandleMain()
    {

    }
    public void HandleTutorial()
    {

    }
    public void HandleGame()
    {
        Dino.GetComponent<DinoCanvasTouchMovement>().StartDinoIntroAnimation();
        Ground.GetComponent<RandomGroundGenerator>().StartGroundAnimation(()=>
        {
            
        });
    }
    public void HandlePause()
    {

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
