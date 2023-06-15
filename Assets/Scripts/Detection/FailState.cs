using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailState : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;

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
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
