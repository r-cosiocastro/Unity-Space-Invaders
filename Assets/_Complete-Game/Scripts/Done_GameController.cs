using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Done_GameController : MonoBehaviour
{
    /*
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    */

    public int lifes = 3;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text winnerText;
    //public Text lifesText;
    public GameObject energyBar;
    public Text readyShadowText;
    public Text readyText;
    public GameObject cheatGameObject;
    public GameObject playerPrefab;
    public GameObject enemiesPrefab;
    public float initialZEnemiesPosition = 130;

    public bool gameOver;
    private bool restart;
    private int score;
    private bool gameStarted;
    private EnergyBar bar;

    private bool increaseEnemyRateFire = false;
    private bool slowTime = false;
    private bool infiniteLifes = false;

    void Start()
    {
        bar = energyBar.GetComponent<EnergyBar>();
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        winnerText.text = "";
        score = 0;
        UpdateScore();
        UpdateLifes();
        //StartCoroutine(SpawnWaves());
        StartCoroutine(ShowReady());
    }

    IEnumerator ShowReady()
    {
        readyShadowText.GetComponent<DelayAudio>().Play();
        StartCoroutine(FadeTextToFullAlpha(2f, readyShadowText));
        StartCoroutine(FadeTextToFullAlpha(2f, readyText));
        Swipe(2f, readyText, -(Screen.width / 2), 2);
        Swipe(2f, readyShadowText, (Screen.width / 2), -4);
        yield return new WaitForSeconds(4);
        StartCoroutine(FadeTextToZeroAlpha(2f, readyShadowText));
        StartCoroutine(FadeTextToZeroAlpha(2f, readyText));
        Swipe(2f, readyText, 2, (Screen.width / 2));
        Swipe(2f, readyShadowText, -4, -(Screen.width / 2));
        gameStarted = true;
    }

    void Swipe(float t, Text i, float from, float to)
    {
        i.rectTransform.localPosition = new Vector3(from, i.rectTransform.localPosition.y, i.rectTransform.localPosition.z);
        LeanTween.move(i.rectTransform, new Vector3(to, i.rectTransform.localPosition.y, i.rectTransform.localPosition.z), t);
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    void Update()
    {
        if (gameOver)
        {
            restartText.text = "Presiona R para reiniciar";
            if (Input.GetButtonDown("Restart"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    /*
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }
    }
    */

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Puntuación: " + score;
    }

    void UpdateLifes()
    {
        //lifesText.text = "Vidas: " + lifes;
        bar.SetValueCurrent(lifes);
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOverText.text = "Game Over";
            gameOver = true;
        }
    }

    IEnumerator AnotherWave()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(ShowReady());
        initialZEnemiesPosition -= 4;
        Instantiate(enemiesPrefab, new Vector3(0, 0, 240), Quaternion.Euler(0,0,0));
        winnerText.text = "";
    }

    public void Win()
    {
        winnerText.text = "¡Ganaste!";
        StartCoroutine(AnotherWave());
        //gameOver = true;
    }

    public void DiscountLife()
    {
        if (!gameOver) 
        {
            gameStarted = false;
            if (!infiniteLifes)
            {
                lifes--;
                UpdateLifes();
            }
            if (lifes <= 0)
            {
                GameOver();
            }
            else
            {
                StartCoroutine(RestartGame());
                Instantiate(playerPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0));
            }
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(ShowReady());
    }

    public void SwitchTime()
    {
        if (slowTime)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0.5f;
        }
        slowTime = !slowTime;
    }

    public void SwitchEnemyRateFire()
    {
        increaseEnemyRateFire = !increaseEnemyRateFire;
    }

    public bool ShouldIncreaseRateFire()
    {
        return increaseEnemyRateFire;
    }

    public IEnumerator ShowCheatActivated()
    {
        cheatGameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        cheatGameObject.SetActive(false);
        yield return null;
    }

    public bool HasGameStarted()
    {
        return gameStarted;
    }

    public void InfiniteLifes()
    {
        infiniteLifes = true;
        lifes = 3;
        UpdateLifes();
    }

    public void GodMode()
    {
        InfiniteLifes();
        energyBar.SetActive(false);
    }
}