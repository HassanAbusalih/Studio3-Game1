using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelExit : MonoBehaviour
{
    Collider2D col2D;
    int sceneIndex;
    int scenes;
    public static Action Exited;

    private void Start()
    {
        col2D = GetComponent<Collider2D>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        scenes = SceneManager.sceneCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sceneIndex < scenes - 1)
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    void ActivateExit()
    {
        col2D.isTrigger = true;
    }

    private void OnEnable()
    {
        ItemsUI.LevelComplete += ActivateExit;
    }

    private void OnDisable()
    {
        ItemsUI.LevelComplete -= ActivateExit;
    }
}
