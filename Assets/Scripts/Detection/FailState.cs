using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class FailState : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    PlayerMovement playerMovement;
    public static event Action ResetGame;

    private void Awake()
    {
        Time.timeScale = 1.0f;
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
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.transform.position = Checkpoint.currentRespawnPosition;
        playerMovement.transform.rotation = Checkpoint.currentRespawnRotation;
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
