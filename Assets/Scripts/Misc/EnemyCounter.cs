using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    GameObject[] enemies;
    public Text enemyCountText;
    void Start()
    {
        
    }

    
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Guard");
        enemyCountText.text = " " + enemies.Length.ToString();
    }
}
