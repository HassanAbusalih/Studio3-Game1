using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    GuardManager[] enemies;
    public Text enemyCountText;
    
    void Start()
    {
        enemies = FindObjectsOfType<GuardManager>();
        enemyCountText.text = enemies.Length.ToString();
    }
}
