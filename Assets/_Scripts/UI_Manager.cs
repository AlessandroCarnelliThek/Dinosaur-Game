using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject UI_MainPanel, UI_PausePanel, UI_GameOverPanel, UI_ScorePanel, UI_JoystickPanel;
    [SerializeField] StartButton startButton;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject imgPause, imgStart;


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

        pauseButton.SetActive(state == GameState.GAME || state == GameState.PAUSE);
    }

    void Start()
    {
        startButton.onStartButtonDown.AddListener(HandleStartButton);
        pauseButton.GetComponent<TogglePauseButton>().onTogglePauseButtonDown.AddListener(TogglePause);
    }

    public void HandleStartButton()
    {
        //GameManager.instance.StartGame();
        Debug.Log("GAME START");
        GameManager.instance.MainTransition();
        UI_MainPanel.SetActive(false);
    }
    public void TogglePause()
    {
            //Debug.Log("funzionaaaaaaaaaaaaa");
        
        if (Time.timeScale > 0)
        {
            GameManager.instance.UpdateGameState(GameState.PAUSE);
            Time.timeScale = 0;
            imgPause.SetActive(false);
            imgStart.SetActive(true);
            Debug.Log("PAUSE ON");
        }
        else
        {
            GameManager.instance.UpdateGameState(GameState.GAME);
            Time.timeScale = 1;
            imgStart.SetActive(false);
            imgPause.SetActive(true);
            Debug.Log("PAUSE OFF");
        }
        
    }
}
