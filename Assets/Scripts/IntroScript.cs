using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    public float waitTime = 4f;
    private Animator animator;

    //private bool startFade;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        gameObject.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitTime <= 0)
        {
            gameObject.GetComponent<Animator>().enabled = true;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }
}
