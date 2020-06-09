using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject container;
    public GameObject cellChild;
    public Text scoreCounter;

    public int iPlayerIndex = 7;
    public int jPlayerIndex = 7;

    public int gridMapSize = 12; // This is how much squares each row and column has in the map! This value also needs to be the same as the value at Canvas/Panel under Grid Layout Group "Constraint Count" for "Fixed Column Count"!
    public GameObject[,] cellsArray;

    public int score = 0;
    public int scoreboardSize = 10;
    public static int[] scoreboard;

    public static bool isClassicMode = true;

    private int snakeLength = 1;
    private List<GameObject> snakePositions;
    
    private AudioManager audioManager;

    private PlayerController playerController;

    void Start()
    {
        scoreboard = new int[scoreboardSize];

        if(SceneManager.GetActiveScene().name == "ScoreboardScene")
        {
            LoadScoreboard();
        }
        
        if (SceneManager.GetActiveScene().name == "MainLevelScene")
        {
            playerController = FindObjectOfType<PlayerController>();
            audioManager = FindObjectOfType<AudioManager>();
            snakePositions = new List<GameObject>();

            createMapCells();
            resizeMap();
            SpawnApple();
            DrawPlayer(iPlayerIndex, jPlayerIndex);
        }
    }

    void createMapCells()
    {
        cellsArray = new GameObject[gridMapSize, gridMapSize];

        for (int i = 0; i < gridMapSize; i++)
        {
            for (int j = 0; j < gridMapSize; j++)
            {
                GameObject newChild = Instantiate(cellChild, transform);
                newChild.transform.SetParent(container.transform, false);

                cellsArray[i, j] = newChild;
                cellsArray[i, j].gameObject.tag = "Ground";
            }
        }

        cellChild.gameObject.SetActive(false);
    }

    void resizeMap()
    {
        Vector2 newSize;
        float width = container.GetComponent<RectTransform>().rect.width;
        float height = container.GetComponent<RectTransform>().rect.height;

        if (width > height)
        {
            newSize = new Vector2(height / gridMapSize, height / gridMapSize);
            container.GetComponent<GridLayoutGroup>().cellSize = newSize;
        }
        else
        {
            newSize = new Vector2(width / gridMapSize, width / gridMapSize);
            container.GetComponent<GridLayoutGroup>().cellSize = newSize;
        }
    }

    public void SortScoreBoard()
    {
        for (int i = 0; i < scoreboard.Length; i++)
        {
            for (int j = i + 1; j < scoreboard.Length; j++)
            {
                if (scoreboard[i] < scoreboard[j])
                {
                    int swaper = scoreboard[i];
                    scoreboard[i] = scoreboard[j];
                    scoreboard[j] = swaper;
                }
            }
        }
    }

    public void DrawPlayer(int iPlIndex, int jPlIndex)
    {
        if(cellsArray[iPlIndex, jPlIndex].gameObject.tag == "Apple")
        {
            score++;
            scoreCounter.text = score.ToString();
            snakeLength++;
            SpawnApple();
            audioManager.Play("AppleCollect");

        }
        else if(cellsArray[iPlIndex, jPlIndex].gameObject.tag == "Player")
        {
            playerController.Death();
        }
        
        snakePositions.Add(cellsArray[iPlIndex, jPlIndex]);

        if(snakePositions.Count > snakeLength)
        {
            snakePositions[0].GetComponent<Image>().color = new Color32(255, 200, 0, 255);
            snakePositions[0].gameObject.tag = "Ground";
            snakePositions.RemoveAt(0);
        }

        for (int i = 0; i < snakePositions.Count; i++)
        {
            snakePositions[i].GetComponent<Image>().color = Color.green;
            snakePositions[i].gameObject.tag = "Player";
        }

    }

    public void SpawnApple()
    {
        int iAppleIndex = Random.Range(0, 12);
        int jAppleIndex = Random.Range(0, 12);

        if (cellsArray[iAppleIndex, jAppleIndex].gameObject.tag == "Ground")
        {
            cellsArray[iAppleIndex, jAppleIndex].GetComponent<Image>().color = Color.red;
            cellsArray[iAppleIndex, jAppleIndex].gameObject.tag = "Apple";
        }
        else
        {
            SpawnApple();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainLevelScene", LoadSceneMode.Single);
        if(playerController != null)
        {
            playerController.canMove = true;
        }

        score = 0;
    }

    public void PlayClassicMode()
    {
        isClassicMode = true;
        StartGame();
    }

    public void PlayEasyMode()
    {
        isClassicMode = false;
        StartGame();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }

    public void GoToScoreboard()
    {
        SceneManager.LoadScene("ScoreboardScene", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveScoreboard()
    {
        SaveSystem.SaveScoreboard(this);
    }

    public void LoadScoreboard()
    {
        ScoreboardData data = SaveSystem.LoadScoreboard();

        if(data != null)
        {
            for (int i = 0; i < scoreboardSize; i++)
            {
                scoreboard[i] = data.scoreboard[i];
            }
        }
    }

}
