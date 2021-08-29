using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //The GameManager is called to control game states
    private GameManager gameManager;
    private AudioManager audioManager;
    [SerializeField] private AudioSource sFXGetItem;
    [SerializeField] private GameObject pauseMenu;
    //Delay between when dialogue ends and UI event occurs
    [SerializeField] private float uIEventDelay;
    //UI event that plays when the player gets a coffee
    [Header("Menu - Get Coffee")]
    [SerializeField] private GameObject getCoffeeMessage;
    [SerializeField] private Animator getCoffeeAnimator;
    //UI event that plays when the player gets some cat food
    [Header("Menu - Get Cat Food")]
    [SerializeField] private GameObject getCatFoodMessage;
    [SerializeField] private Animator getCatFoodAnimator;
    //UI event that plays at the end of the level
    [Header("Menu - Chapter End")]
    [SerializeField] private GameObject chapterEndMenu;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Update()
    {
        //During gameplay, allow the player to pause the game
        if (Input.GetKeyDown(KeyCode.Escape) && gameManager.gameState == GameState.game)
        {
            pauseMenu.SetActive(true);
            audioManager.PauseRain();
            Time.timeScale = 0;
            gameManager.gameState = GameState.pause;
        }
        //And allow them to unpause too
        else if (Input.GetKeyDown(KeyCode.Escape) && gameManager.gameState == GameState.pause)
        {
            pauseMenu.SetActive(false);
            audioManager.UnpauseRain();
            Time.timeScale = 1;
            gameManager.gameState = GameState.game;
        }

        //If the Get Coffee UI event is active, allow the player to turn it off
        if (gameManager.gameState == GameState.pause && getCoffeeMessage.activeSelf == true)
        {
            StartCoroutine(GetCoffee_Off());
        }

        //If the Get Cat Food UI event is active, allow the player to turn it off
        if (gameManager.gameState == GameState.pause && getCatFoodMessage.activeSelf == true)
        {
            StartCoroutine(GetCatFood_Off());
        }
    }

    //Functions for each UI event, called from other scripts
    public void TriggerGetCoffee()
    {
        StartCoroutine(GetCoffee_On());
    }

    public void TriggerGetCatFood()
    {
        StartCoroutine(GetCatFood_On());
    }

    public void TriggerChapterEnd()
    {
        StartCoroutine(ChapterEnd());
    }

    //Pause the game and display the Get Coffee UI event
    private IEnumerator GetCoffee_On()
    {
        if (gameManager.gameState == GameState.game)
        {
            gameManager.gameState = GameState.pause;
            yield return new WaitForSeconds(uIEventDelay);
            getCoffeeMessage.SetActive(true);
            sFXGetItem.Play();
        }
    }

    //Unpause the game and turn off the Get Coffee UI event
    private IEnumerator GetCoffee_Off()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            getCoffeeAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(uIEventDelay);
            gameManager.gameState = GameState.game;
            getCoffeeMessage.SetActive(false);
        }
    }

    //Pause the game and display the Get Cat Food UI event
    private IEnumerator GetCatFood_On()
    {
        if (gameManager.gameState == GameState.game)
        {
            gameManager.gameState = GameState.pause;
            yield return new WaitForSeconds(uIEventDelay);
            getCatFoodMessage.SetActive(true);
            sFXGetItem.Play();
        }
    }

    //Unpause the game and turn off the Get Cat Food UI event
    private IEnumerator GetCatFood_Off()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            getCatFoodAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(uIEventDelay);
            gameManager.gameState = GameState.game;
            getCatFoodMessage.SetActive(false);
        }
    }

    //Pause the game and display the Chapter End UI event
    private IEnumerator ChapterEnd()
    {
        if (gameManager.gameState == GameState.game)
        {
            gameManager.gameState = GameState.pause;
            yield return new WaitForSeconds(uIEventDelay);
            chapterEndMenu.SetActive(true);
        }
    }

    //Controls the New Game button on the Menu screen
    public void PlayGameTrigger()
    {
        Time.timeScale = 1;
        StartCoroutine(PlayGame());
    }

    private IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(uIEventDelay);
        SceneManager.LoadScene("Main");
    }

    //Controls the Quit button on the Pause screen
    public void QuitToMenuTrigger()
    {
        Time.timeScale = 1;
        StartCoroutine(QuitToMenu());
    }

    private IEnumerator QuitToMenu()
    {
        yield return new WaitForSeconds(uIEventDelay);
        SceneManager.LoadScene("Menu");
    }

    //Closes the application from the Menu screen
    public void QuitGame()
    {
        Application.Quit();
    }
}
