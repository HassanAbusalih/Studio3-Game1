using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    PlayerMovement playerMovement;
    public static event Action ResetGame;

    private void Start()
    {
        Time.timeScale = 1f;
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnEnable()
    {
        LineOfSight.CaughtPlayer += PlayerCaught;
    }

    private void OnDisable()
    {
        LineOfSight.CaughtPlayer -= PlayerCaught;
    }

    void PlayerCaught()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void ResetLevel()
    {
        ResetGame?.Invoke();
        playerMovement.transform.position = Checkpoint.currentRespawnPosition;
        playerMovement.transform.rotation = Checkpoint.currentRespawnRotation;
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);
    }

    public void NextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex < SceneManager.sceneCount - 1)
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
