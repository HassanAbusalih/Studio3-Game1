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
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            Exited?.Invoke();
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
