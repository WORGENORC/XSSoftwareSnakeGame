using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool canMove;
    public GameObject deathScreen;

    private bool canMoveUp = true;
    private bool canMoveDown = true;
    private bool canMoveRight = true;
    private bool canMoveLeft = true;

    private string operation = "-";
    private int indexToUpdate = 1;
    
    private float timeToMove = 0.15f;

    private GameManager gameManager;

    public float maxSwipeTime;
    public float minSwipeDistance;

    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeTime;

    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;
    private float swipeLength;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        canMoveDown = false;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            SwipeTest();
            if (Input.GetKeyDown(KeyCode.UpArrow) && canMoveUp)
            {
                operation = "-";
                indexToUpdate = 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && canMoveDown)
            {
                operation = "+";
                indexToUpdate = 1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && canMoveRight)
            {
                operation = "+";
                indexToUpdate = 2;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && canMoveLeft)
            {
                operation = "-";
                indexToUpdate = 2;
            }

            if(timeToMove <= 0)
            {
                MovePlayer(operation, indexToUpdate);
                timeToMove = 0.15f;
            }
            else
            {
                timeToMove -= Time.deltaTime;
            }
        }
    }

    void MovePlayer(string operation, int indexToUpdate)
    {
        if(operation == "-")
        {
            if(indexToUpdate == 1)
            {
                gameManager.iPlayerIndex--;
                if (gameManager.iPlayerIndex < 0)
                {
                    if(!GameManager.isClassicMode)
                    {
                        gameManager.iPlayerIndex = gameManager.gridMapSize - 1;
                        canMoveRight = true;
                        canMoveLeft = true;
                        canMoveDown = false;
                        gameManager.DrawPlayer(gameManager.iPlayerIndex, gameManager.jPlayerIndex);
                    }
                    else
                    {
                        Death();
                    }
                    
                }
                else
                {
                    canMoveRight = true;
                    canMoveLeft = true;
                    canMoveDown = false;
                    gameManager.DrawPlayer(gameManager.iPlayerIndex, gameManager.jPlayerIndex);
                } 
            }
            else if(indexToUpdate == 2)
            {
                gameManager.jPlayerIndex--;
                if (gameManager.jPlayerIndex < 0)
                {
                    if(!GameManager.isClassicMode)
                    {
                        gameManager.jPlayerIndex = gameManager.gridMapSize - 1;
                        canMoveUp = true;
                        canMoveDown = true;
                        canMoveRight = false;
                        gameManager.DrawPlayer(gameManager.iPlayerIndex, gameManager.jPlayerIndex);
                    }
                    else
                    {
                        Death();
                    }
                }
                else
                {
                    canMoveUp = true;
                    canMoveDown = true;
                    canMoveRight = false;
                    gameManager.DrawPlayer(gameManager.iPlayerIndex, gameManager.jPlayerIndex);
                }
            }
        }
        else if(operation == "+")
        {
            if(indexToUpdate == 1)
            {
                gameManager.iPlayerIndex++;
                if (gameManager.iPlayerIndex > gameManager.gridMapSize-1)
                {
                    if(!GameManager.isClassicMode)
                    {
                        gameManager.iPlayerIndex = 0;
                        canMoveRight = true;
                        canMoveLeft = true;
                        canMoveUp = false;
                        gameManager.DrawPlayer(gameManager.iPlayerIndex, gameManager.jPlayerIndex);
                    }
                    else
                    {
                        Death();
                    }
                }
                else
                {
                    canMoveRight = true;
                    canMoveLeft = true;
                    canMoveUp = false;
                    gameManager.DrawPlayer(gameManager.iPlayerIndex, gameManager.jPlayerIndex);
                }
            }
            else if(indexToUpdate == 2)
            {
                gameManager.jPlayerIndex++;
                if (gameManager.jPlayerIndex > gameManager.gridMapSize-1)
                {
                    if(!GameManager.isClassicMode)
                    {
                        gameManager.jPlayerIndex = 0;
                        canMoveUp = true;
                        canMoveDown = true;
                        canMoveLeft = false;
                        gameManager.DrawPlayer(gameManager.iPlayerIndex, gameManager.jPlayerIndex);
                    }
                    else
                    {
                        Death();
                    }
                }
                else
                {
                    canMoveUp = true;
                    canMoveDown = true;
                    canMoveLeft = false;
                    gameManager.DrawPlayer(gameManager.iPlayerIndex, gameManager.jPlayerIndex);
                }
            }
        }
    }

    void SwipeTest()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                swipeStartTime = Time.time;
                startSwipePosition = touch.position;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                swipeEndTime = Time.time;
                endSwipePosition = touch.position;
                swipeTime = swipeEndTime - swipeStartTime;
                swipeLength = (endSwipePosition - startSwipePosition).magnitude;
                if(swipeTime < maxSwipeTime && swipeLength > minSwipeDistance)
                {
                    SwipeControl();
                }
            }
        }
    }

    void SwipeControl()
    {
        Vector2 Distance = endSwipePosition - startSwipePosition;
        float xDistance = Mathf.Abs(Distance.x);
        float yDistance = Mathf.Abs(Distance.y);
        if(xDistance > yDistance)
        {
            if(Distance.x > 0 && canMoveRight)
            {
                operation = "+";
                indexToUpdate = 2;
            }
            else if(Distance.x < 0 && canMoveLeft)
            {
                operation = "-";
                indexToUpdate = 2;
            }
        }
        else if(yDistance > xDistance)
        {
            if (Distance.y > 0 && canMoveUp)
            {
                operation = "-";
                indexToUpdate = 1;
            }
            else if (Distance.y < 0 && canMoveDown)
            {
                operation = "+";
                indexToUpdate = 1;
            }
        }
    }

    public void Death()
    {
        canMove = false;
        deathScreen.SetActive(true);

        gameManager.LoadScoreboard();
        gameManager.SortScoreBoard();
        if (GameManager.scoreboard[gameManager.scoreboardSize - 1] <= gameManager.score)
        {
            GameManager.scoreboard[gameManager.scoreboardSize - 1] = gameManager.score;
            gameManager.SortScoreBoard();
        }

        gameManager.SaveScoreboard();
    }
}
