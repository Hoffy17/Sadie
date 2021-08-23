using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject getCoffeeMessage;
    [SerializeField] private GameObject getCatFoodMessage;
    [SerializeField] private GameObject chapterEndMenu;
    [SerializeField] private float uIMessageDelay;

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
    }

    public void TriggerGetCoffee()
    {
        StartCoroutine(GetCoffee());
    }

    public void TriggerGetCatFood()
    {
        StartCoroutine(GetCatFood());
    }

    public void TriggerChapterEnd()
    {
        StartCoroutine(ChapterEnd());
    }

    private IEnumerator GetCoffee()
    {
        if (gameManager.gameState == GameState.game)
        {
            gameManager.gameState = GameState.pause;
            yield return new WaitForSeconds(uIMessageDelay);
            chapterEndMenu.SetActive(true);
        }
    }

    private IEnumerator GetCatFood()
    {
        if (gameManager.gameState == GameState.game)
        {
            gameManager.gameState = GameState.pause;
            yield return new WaitForSeconds(uIMessageDelay);
            chapterEndMenu.SetActive(true);
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
