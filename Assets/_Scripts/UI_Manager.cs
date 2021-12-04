using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    private void MakeSingleton()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    [SerializeField] GameObject UI_MainPanel, UI_PausePanel, UI_GameOverPanel, UI_ScorePanel, UI_HiScorePanel, UI_JoystickPanel;
    [SerializeField] StartButton startButton;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject imgPause, imgStart;

    [SerializeField] Text scoreText, hiScoreText, labelText;


    private void Awake()
    {
        MakeSingleton();
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
        UI_ScorePanel.SetActive(state != GameState.MAIN && state != GameState.TUTORIAL);// && GameManager.instance.GetScore() <= PlayerPrefs.GetInt("hiScore", 0));
        UI_HiScorePanel.SetActive(state != GameState.MAIN && state != GameState.TUTORIAL && PlayerPrefs.GetInt("hiScore", 0) > 0);
        UI_JoystickPanel.SetActive(state != GameState.MAIN && state != GameState.PAUSE && state != GameState.GAMEOVER);

        pauseButton.SetActive(state == GameState.GAME || state == GameState.PAUSE);
    }

    void Start()
    {
        startButton.onStartButtonDown.AddListener(HandleStartButton);
        pauseButton.GetComponent<TogglePauseButton>().onTogglePauseButtonDown.AddListener(TogglePause);
        UpdateLabel("HI ");
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
            Time.timeScale = 0;
            imgPause.SetActive(false);
            imgStart.SetActive(true);
            Debug.Log("PAUSE ON");
            GameManager.instance.UpdateGameState(GameState.PAUSE);
        }
        else
        {
            Time.timeScale = 1;
            imgStart.SetActive(false);
            imgPause.SetActive(true);
            Debug.Log("PAUSE OFF");
            GameManager.instance.UpdateGameState(GameState.GAME);
        }

    }

    public void UpdateScorePanel(int score)
    {

        if (score < 10) { scoreText.text = $"000{score}"; }
        else if (score < 100) { scoreText.text = $"00{score}"; }
        else if (score < 1000) { scoreText.text = $"0{score}"; }
        else { scoreText.text = $"{score}"; }

    }
    public void EnableScorePanel(bool var)
    {
        UI_ScorePanel.SetActive(var);
    }

    public void UpdateHiScorePanel(int hiScore)
    {
        if (hiScore < 10) { hiScoreText.text = $"000{hiScore}"; }
        else if (hiScore < 100) { hiScoreText.text = $"00{hiScore}"; }
        else if (hiScore < 1000) { hiScoreText.text = $"0{hiScore}"; }
        else { hiScoreText.text = $"{hiScore}"; }
    }
    public void UpdateLabel(string label)
    {
        labelText.text = label;
    }
    public void InitializeHiScorePanel()
    {
        int hiScore = PlayerPrefs.GetInt("hiScore", 0);

        if (hiScore < 10) { hiScoreText.text = $"000{hiScore}"; }
        else if (hiScore < 100) { hiScoreText.text = $"00{hiScore}"; }
        else if (hiScore < 1000) { hiScoreText.text = $"0{hiScore}"; }
        else { hiScoreText.text = $"{hiScore}"; }
    }
    public void StartScorePanelAnimation()
    {
        StartCoroutine(ScorePanelTransition());
    }
    IEnumerator ScorePanelTransition()
    {
        float timer = 0;
        float duration = 0.4f;
        Vector3 startPoint = UI_HiScorePanel.transform.position;
        Vector3 endPoint = new Vector3(4, UI_HiScorePanel.transform.position.y, 0);
        Vector2 boxSizeAfter = new Vector2(hiScoreText.rectTransform.sizeDelta.x, 65);
        Vector2 boxSizeBefore = new Vector2(120, 65);
        Vector2 panelSizeAfter = new Vector2(UI_HiScorePanel.GetComponent<RectTransform>().sizeDelta.x, 45);
        Vector2 panelSizeBefore = new Vector2(UI_HiScorePanel.GetComponent<RectTransform>().sizeDelta.x, 65);
        CanvasGroup scorePanelCanvasGroup = UI_ScorePanel.GetComponent<CanvasGroup>();

        UpdateLabel("NEW HI ");

        while (timer < duration)
        {
            timer += Time.deltaTime;
            scorePanelCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / duration);
            UI_HiScorePanel.transform.position = Vector3.Lerp(startPoint, endPoint, timer / duration);
            hiScoreText.rectTransform.sizeDelta = Vector2.Lerp(boxSizeAfter, boxSizeBefore, timer / duration);
            UI_HiScorePanel.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(panelSizeAfter, panelSizeBefore, timer / duration);
            //transform.position = Vector3.Lerp(startPoint, endPoint, timer / duration);
            yield return null;
        }
        scorePanelCanvasGroup.alpha = 0;
        hiScoreText.rectTransform.sizeDelta = boxSizeBefore;
        UI_HiScorePanel.GetComponent<RectTransform>().sizeDelta = panelSizeBefore;

        UI_HiScorePanel.transform.position = endPoint;
        EnableScorePanel(false);
    }
}
