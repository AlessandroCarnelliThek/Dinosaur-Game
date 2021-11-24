using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject UI_MainPanel, UI_PausePanel, UI_GameOverPanel, UI_ScorePanel, UI_JoystickPanel;
    [SerializeField] StartButton startButton;


    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStartChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStartChanged;
    }
    private void GameManagerOnOnGameStartChanged(GameState state)
    {
        UI_MainPanel.SetActive(state == GameState.MAIN);
        UI_PausePanel.SetActive(state == GameState.PAUSE);
        UI_GameOverPanel.SetActive(state == GameState.GAMEOVER);
        UI_ScorePanel.SetActive(state != GameState.MAIN && state != GameState.TUTORIAL);
        UI_JoystickPanel.SetActive(state != GameState.MAIN && state != GameState.PAUSE && state != GameState.GAMEOVER);
    }

    void Start()
    {
        startButton.onStartButtonDown.AddListener(HandleStartButton);
    }

    public void HandleStartButton()
    {
        //GameManager.instance.StartGame();
        GameManager.instance.UpdateGameState(GameState.GAME);
    }
}
