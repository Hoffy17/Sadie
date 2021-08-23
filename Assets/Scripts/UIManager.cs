using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private float uIMessageDelay;
    [Header("Menu - Get Coffee")]
    [SerializeField] private GameObject getCoffeeMessage;
    [SerializeField] private Animator getCoffeeAnimator;
    [Header("Menu - Get Cat Food")]
    [SerializeField] private GameObject getCatFoodMessage;
    [SerializeField] private Animator getCatFoodAnimator;
    [Header("Menu - Chapter End")]
    [SerializeField] private GameObject chapterEndMenu;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameManager.gameState == GameState.game)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            gameManager.gameState = GameState.pause;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && gameManager.gameState == GameState.pause)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            gameManager.gameState = GameState.game;
        }

        if (gameManager.gameState == GameState.pause && getCoffeeMessage.activeSelf == true)
        {
            StartCoroutine(GetCoffee_Off());
        }

        if (gameManager.gameState == GameState.pause && getCatFoodMessage.activeSelf == true)
        {
            StartCoroutine(GetCatFood_Off());
        }
    }

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

    private IEnumerator GetCoffee_On()
    {
        if (gameManager.gameState == GameState.game)
        {
            gameManager.gameState = GameState.pause;
            yield return new WaitForSeconds(uIMessageDelay);
            getCoffeeMessage.SetActive(true);
        }
    }

    private IEnumerator GetCoffee_Off()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            getCoffeeAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(uIMessageDelay);
            gameManager.gameState = GameState.game;
            getCoffeeMessage.SetActive(false);
        }
    }

    private IEnumerator GetCatFood_On()
    {
        if (gameManager.gameState == GameState.game)
        {
            gameManager.gameState = GameState.pause;
            yield return new WaitForSeconds(uIMessageDelay);
            getCatFoodMessage.SetActive(true);
        }
    }

    private IEnumerator GetCatFood_Off()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            getCatFoodAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(uIMessageDelay);
            gameManager.gameState = GameState.game;
            getCatFoodMessage.SetActive(false);
        }
    }

    private IEnumerator ChapterEnd()
    {
        if (gameManager.gameState == GameState.game)
        {
            gameManager.gameState = GameState.pause;
            yield return new WaitForSeconds(uIMessageDelay);
            chapterEndMenu.SetActive(true);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
