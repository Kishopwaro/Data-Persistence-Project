using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button backToStartMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Text pauseText;
    [SerializeField] private Text highscoreText;

    private bool isPaused = false;


    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        SetHighScoreOnStart();
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (!m_Started)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Started = true;
                    float randomDirection = Random.Range(-1.0f, 1.0f);
                    Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                    forceDir.Normalize();

                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                }

            }
            else if (m_GameOver)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RestartGame();
                }
            }
            else if (m_Started && !m_GameOver)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGame();
                    isPaused = true;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ResumeGame();
            }
        }

    }



    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        backToStartMenuButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        if (m_Points > DataBetweenScenes.Instance.highScore)
        {
            UpdateHighScore(m_Points);
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseText.gameObject.SetActive(true);
        backToStartMenuButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        pauseText.gameObject.SetActive(false);
        backToStartMenuButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateHighScore(int highScore)
    {
        DataBetweenScenes.Instance.highScoreNameText = DataBetweenScenes.Instance.nameText;
        highscoreText.text = "Best Score: " + DataBetweenScenes.Instance.highScoreNameText + ": " + highScore;
        DataBetweenScenes.Instance.highScore = highScore;
    }

    private void SetHighScoreOnStart()
    {
        if (DataBetweenScenes.Instance.highScore > 0)
        {
            highscoreText.text = "Best Score: " + DataBetweenScenes.Instance.highScoreNameText + ": " + DataBetweenScenes.Instance.highScore;
        }
    }

}
