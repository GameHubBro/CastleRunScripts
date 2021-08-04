using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UIGameManager : MonoBehaviour
{
    [SerializeField] private Animator resumeButtonAnim;
    [SerializeField] private Animator newGameButtonAnim;
    [SerializeField] private Animator highscoresButtonAnim;
    [SerializeField] private Animator settingsButtonAnim;
    [SerializeField] private Animator backToMenuButtonAnim;

    [SerializeField] private Animator highscoresPanelAnim;
    [SerializeField] private Animator settingsPanelAnim;

    [SerializeField] private Animator backgroundAnim;
    [SerializeField] private Animator pauseButtonAnim;

    [SerializeField] private Animator blackVeilAnim;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject blackVeil;

    [SerializeField] private Toggle toggleSound;
    [SerializeField] private Text toggleTextSound;

    [SerializeField] private Animator losePanelAnim;

    [SerializeField] private AudioSource music;

    [SerializeField] private Text inputFieldText;

    [SerializeField] private Text askName;

    public Text scoreText;

    public Text score;

    public static UIGameManager instance;

    private int delay = 1;

    private float timer = 1f;

    private void Awake()
    {
        if (UIGameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        UIGameManager.instance = this;
        StartCoroutine(WaitToWhiteVeil(delay * 1.5f));
    }

    IEnumerator WaitToWhiteVeil(float delay)
    {
        blackVeil.SetActive(true);
        yield return new WaitForSeconds(delay);
        blackVeil.SetActive(false);
    }

    private void Update()
    {
        Time.timeScale = timer;
        isMute();
    }

    private void isMute()
    {
        toggleTextSound.text = toggleSound.isOn ? "Sound Off" : "Sound On";
    }

    public void ButtonPauseGame()
    {
        ShowGameMenu();
        Debug.Log("You've pressed Pause Button");
    }

    public void ButtonResumeGame()
    {
        HideGameMenu();
        Debug.Log("You've pressed Resume Game Button");
    }

    public void ButtonNewGame()
    {
        StartCoroutine(WaitToChangeScene(delay, SceneManager.GetActiveScene().buildIndex));
        Debug.Log("You've pressed New Game Button");
    }
    public void ButtonBackToMenu()
    {
        StartCoroutine(WaitToChangeScene(delay, SceneManager.GetActiveScene().buildIndex - 1));
        Debug.Log("You've pressed Back To MenuButton");
    }

    public void ButtonSettings()
    {
        if (!highscoresPanelAnim.GetBool("isHidden"))
        {
            highscoresPanelAnim.SetBool("isHidden", true);
        }
        settingsPanelAnim.SetBool("isHidden", false);
        Debug.Log("You've pressed Settings Button");
    }

    public void ButtonHisgscoresGame()
    {
        if (!settingsPanelAnim.GetBool("isHidden"))
        {
            settingsPanelAnim.SetBool("isHidden", true);
        }
        if (highscoresPanelAnim.GetBool("isHidden"))
        {
            highscoresPanelAnim.SetBool("isHidden", false);
            Debug.Log("You've pressed Highscores Button");

            HighscoreTable.instance.CreateHighscoreTable();
        }
    }

    public void SetScoreToTable()
    {
        int score = Convert.ToInt32(this.score.text);
        string inputName = inputFieldText.text;

        HighscoreTable.instance.AddHighscoreEntry(score, inputName);
        if (inputName.Length > 0)
        {
            losePanelAnim.SetBool("isHidden", true);
        }
        else
        {
            askName.color = Color.red;
        }
    }

    private void ShowGameMenu()
    {
        menuPanel.SetActive(true);
        resumeButtonAnim.SetBool("isHidden", false);
        newGameButtonAnim.SetBool("isHidden", false);
        highscoresButtonAnim.SetBool("isHidden", false);
        settingsButtonAnim.SetBool("isHidden", false);
        backToMenuButtonAnim.SetBool("isHidden", false);
        backgroundAnim.SetBool("isHidden", false);
        pauseButtonAnim.SetBool("isHidden", true);
        timer = 0;
    }

    private void HideGameMenu()
    {
        resumeButtonAnim.SetBool("isHidden", true);
        newGameButtonAnim.SetBool("isHidden", true);
        highscoresButtonAnim.SetBool("isHidden", true);
        settingsButtonAnim.SetBool("isHidden", true);
        backToMenuButtonAnim.SetBool("isHidden", true);
        highscoresPanelAnim.SetBool("isHidden", true);
        settingsPanelAnim.SetBool("isHidden", true);
        backgroundAnim.SetBool("isHidden", true);
        pauseButtonAnim.SetBool("isHidden", false);
        StartCoroutine(WaitToCloseMenu(delay));
    }

    private void ShowGameMenuAfterDeath()
    {
        menuPanel.SetActive(true);
        //resumeButtonAnim.SetBool("isHidden", false);
        newGameButtonAnim.SetBool("isHidden", false);
        highscoresButtonAnim.SetBool("isHidden", false);
        settingsButtonAnim.SetBool("isHidden", false);
        backToMenuButtonAnim.SetBool("isHidden", false);
        backgroundAnim.SetBool("isHidden", false);
        pauseButtonAnim.SetBool("isHidden", true);
        timer = 0;
    }

    public IEnumerator WaitToShowLosePanel(int delay)
    {
        yield return new WaitForSeconds(delay);
        ShowGameMenuAfterDeath();
        losePanelAnim.SetBool("isHidden", false);
        music.volume /= 2;
    }

    IEnumerator WaitToCloseMenu(int delay)
    {
        timer = 0.7f;
        yield return new WaitForSeconds(delay);
        menuPanel.SetActive(false);
        timer = 1f;
    }
    IEnumerator WaitToChangeScene(int delay, int index)
    {
        blackVeil.SetActive(true);
        blackVeilAnim.SetBool("isBlack", true);
        timer = 1f;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(index);
    }
}
