using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardController : MonoBehaviour
{
    public GameObject scoreContainer;
    public GameObject positionsContainer;

    public GameObject scoreTextCell;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.SortScoreBoard();

        for (int i = 0; i < gameManager.scoreboardSize; i++)
        {
            GameObject newChild = Instantiate(scoreTextCell, transform);
            newChild.transform.SetParent(scoreContainer.transform, false);
            newChild.GetComponent<Text>().fontSize = 32;

            if(GameManager.scoreboard[i] <= 0)
            {
                newChild.GetComponent<Text>().text = "0";
            }
            else
            {
                newChild.GetComponent<Text>().text = GameManager.scoreboard[i].ToString();
            }
        }

        for (int i = 0; i < gameManager.scoreboardSize; i++)
        {
            GameObject newChild = Instantiate(scoreTextCell, transform);
            newChild.transform.SetParent(positionsContainer.transform, false);
            newChild.GetComponent<Text>().fontSize = 32;

            newChild.GetComponent<Text>().text = (i + 1).ToString();
        }

        resizeContainer(scoreContainer);
        resizeContainer(positionsContainer);

        scoreTextCell.gameObject.SetActive(false);
    }

    void resizeContainer(GameObject container)
    {
        Vector2 newSize;
        float width = container.GetComponent<RectTransform>().rect.width;
        float height = container.GetComponent<RectTransform>().rect.height;

        newSize = new Vector2(width, height / gameManager.scoreboardSize);
        container.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }
}
