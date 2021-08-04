using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    [SerializeField] private Animator startButtonAnimator;
    [SerializeField] private Animator settingsButtonAnimator;
    [SerializeField] private Animator leaderboardButtonAnimator;
    [SerializeField] private Animator creditsButtonAnimator;
    [SerializeField] private Animator exitButtonAnimator;
    [SerializeField] private Animator backButtonAnimator;
    [SerializeField] private Animator settingsPanelAnimator;
    [SerializeField] private Animator leaderBoardPanelAnimator;
    [SerializeField] private Animator creditsPanelAnimator;

    [SerializeField] private Animator goodByeTextAnimator;
    [SerializeField] private Animator startNewGameText1;
    [SerializeField] private Animator startNewGameText2;
    [SerializeField] private Animator startNewGameText3;

    [SerializeField] private Animator[] startNewGameTexts;

    [SerializeField] private Toggle toggle;
    [SerializeField] private Text toggleText;

    [SerializeField] private AudioSource music;

    [SerializeField] private GameObject blackVeil;
    [SerializeField] private Animator blackVeilAnim;

    private int count = 0;

    private bool isSettingsHidden = true;
    private bool isLeaderBoardHidden = true;
    private bool isCreditsHidden = true;

    private bool isQuiting = false;
    private bool isStartingNewGame = false;

    private int delayToProceed = 2;

    private void Awake()
    {
        StartCoroutine(WaitToWhiteVeil(delayToProceed));
    }

    private void StartingNewGame()
    {
        HideMainMenu();
        isStartingNewGame = true;       
    }

    private void Update()
    {
        if (isQuiting)
        {
            Quiting();
        }
        else if (isStartingNewGame)
        {
            Starting();
        }

        if (!isSettingsHidden)
        {
            isMute();
        }
    }

    private void Quiting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            goodByeTextAnimator.SetTrigger("Clicked");
            StartCoroutine(WaitToExit(delayToProceed));
        }
    }

    private void Starting()
    {
        if (count == 0)
        {
            startNewGameTexts[0].SetBool("isHidden", false);
            count++;
            Debug.Log($"count = {count}");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && count < startNewGameTexts.Length)
        {
            startNewGameTexts[count].SetBool("isHidden", false);
            count++;
            Debug.Log($"count = {count}");
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && count == startNewGameTexts.Length)
        {
            StartCoroutine(LetsGo(delayToProceed));
        }
    }

    public void ButtonResumeGame()
    {
        Debug.Log($"You've clicked on ResumeGame button!");
    }

    public void ButtonNewGame()
    {
        Debug.Log($"You've clicked on NewGame button!");
        StartingNewGame();
    }   

    public void ButtonSettings()
    {
        Debug.Log($"You've clicked on Settings button!");
        ShowSettings();
    }

    public void ButtonLeaderBoard()
    {
        Debug.Log($"You've clicked on LeaderBoard button!");
        ShowLeaders();
        HighscoreTable.instance.CreateHighscoreTable();
    }

    public void ButtonCredits()
    {
        Debug.Log($"You've clicked on Credits button!");
        ShowCredits();
    }

    public void ButtonExitGame()
    {
        Debug.Log($"You've clicked on ExitGame button!");
        GoodBye();
    }

    public void ButtonBack()
    {
        Debug.Log($"You've clicked on Back button!");
        if (!isSettingsHidden)
        {
            HideSettings();
        }
        else if (!isLeaderBoardHidden)
        {
            HideLeaders();
        }
        else if (!isCreditsHidden)
        {
            HideCredits();
        }
    }

    private void HideMainMenu()
    {
        startButtonAnimator.SetBool("isHidden", true);
        settingsButtonAnimator.SetBool("isHidden", true);
        leaderboardButtonAnimator.SetBool("isHidden", true);
        creditsButtonAnimator.SetBool("isHidden", true);
        exitButtonAnimator.SetBool("isHidden", true);
    }

    private void ShowMainMenu()
    {
        startButtonAnimator.SetBool("isHidden", false);
        settingsButtonAnimator.SetBool("isHidden", false);
        leaderboardButtonAnimator.SetBool("isHidden", false);
        creditsButtonAnimator.SetBool("isHidden", false);
        exitButtonAnimator.SetBool("isHidden", false);
    }

    private void ShowSettings()
    {
        isSettingsHidden = false;
        HideMainMenu();
        settingsPanelAnimator.SetBool("isHidden", false);
        backButtonAnimator.SetBool("isHidden", false);
    }

    private void HideSettings()
    {
        isSettingsHidden = true;
        ShowMainMenu();
        settingsPanelAnimator.SetBool("isHidden", true);
        backButtonAnimator.SetBool("isHidden", true);
    }

    private void ShowLeaders()
    {
        isLeaderBoardHidden = false;
        HideMainMenu();
        leaderBoardPanelAnimator.SetBool("isHidden", false);
        backButtonAnimator.SetBool("isHidden", false);
    }

    private void HideLeaders()
    {
        isLeaderBoardHidden = true;
        ShowMainMenu();
        leaderBoardPanelAnimator.SetBool("isHidden", true);
        backButtonAnimator.SetBool("isHidden", true);
    }
    private void ShowCredits()
    {
        isCreditsHidden = false;
        HideMainMenu();
        creditsPanelAnimator.SetBool("isHidden", false);
        backButtonAnimator.SetBool("isHidden", false);
    }

    private void HideCredits()
    {
        isCreditsHidden = true;
        ShowMainMenu();
        creditsPanelAnimator.SetBool("isHidden", true);
        backButtonAnimator.SetBool("isHidden", true);
    }
    private void GoodBye()
    {
        HideMainMenu();
        StartCoroutine(WaitBeforExitText(delayToProceed));
    }

    private void isMute()
    {
        toggleText.text = toggle.isOn ? "Sound Off" : "Sound On";
    }

    IEnumerator WaitBeforExitText(int delay)
    {
        yield return new WaitForSeconds(delay);
        goodByeTextAnimator.SetBool("isHidden", false);
        isQuiting = true;
    }

    IEnumerator WaitToExit(int delay)
    {
        yield return new WaitForSeconds(delay);
        
        Debug.Log("App has been closed!");
        //if (UnityEditor.EditorApplication.isPlaying)
        //{
        //    UnityEditor.EditorApplication.isPlaying = false;
        //}
        //else
        //{
            Application.Quit();
        //}
    }

    IEnumerator LetsGo(int delay)
    {
        Debug.Log("Starting LetsGo");
        for (int i = 0; i < startNewGameTexts.Length; i++)
        {
            startNewGameTexts[i].SetTrigger("Clicked");
            Debug.Log($"{startNewGameTexts[i]} triggered Clicked");
        }
        Debug.Log($"Starting to wait {delay} sec.");
        yield return new WaitForSeconds(delay);
        Debug.Log($"Finished to wait {delay} sec.");
        Debug.Log("Moveing to the nex scene");
        StartCoroutine(WaitToChangeScene(delayToProceed, SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator WaitToWhiteVeil(float delay)
    {
        blackVeil.SetActive(true);
        yield return new WaitForSeconds(delay);
        blackVeil.SetActive(false);
    }

    IEnumerator WaitToChangeScene(int delay, int index)
    {
        blackVeil.SetActive(true);
        blackVeilAnim.SetBool("isBlack", true);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(index);
    }
}
