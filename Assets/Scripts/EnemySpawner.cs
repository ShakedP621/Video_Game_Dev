using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private DamageReceiver player;

    [SerializeField] private float timeInterval = 3f;

    [SerializeField] private int enemiesPerWave = 5;

    [SerializeField] private Transform[] spawnPoints;
    private int enemiesToEliminate;
    private float nextSpawnTime = 0;

    private int waveNum = 1;

    private bool WaitingForWave = true;

    private float newWaveTimer = 0;

    private int enemiesEliminated = 0;

    private int enemiesSpawned = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        newWaveTimer = 10;
        WaitingForWave = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (WaitingForWave)
        {
            if (newWaveTimer >= 0)
                newWaveTimer -= Time.deltaTime;
            else
            {
                enemiesToEliminate = waveNum * enemiesPerWave;
                enemiesEliminated = 0;
                enemiesSpawned = 0;
                WaitingForWave = false;
            }
        }
        else
        {
            
            if (Time.time > nextSpawnTime)
            {
                nextSpawnTime = Time.time + timeInterval;
                if (enemiesSpawned < enemiesToEliminate)
                {
                    List<Room> spawnPoints = LevelBuilder.getListRoom();
                    Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
                    randomPoint.position += Vector3.up * 0.000001f;
                    if (randomPoint.parent != null)
                    {
                        GameObject enemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);
                        Enemy n_enemy = enemy.GetComponent<Enemy>();
                        Debug.Log("Spawner");
                        n_enemy.target = player.transform;
                        n_enemy.es = this;
                        enemiesSpawned++;
                    }
                }
            }
        }

        if (player.playerHP <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, Screen.height - 35, 100, 25), ((int)player.playerHP).ToString() + "HP");
       
        if(player.playerHP <= 0)
            GUI.Box(new Rect(Screen.width / 2 - 35, Screen.height / 2 - 20, 150, 40), "Game Over\n Press Space to resetart");
        
        
    }

    public void EnemiesEliminated(Enemy enemy)
    {
        enemiesEliminated++;
        if (enemiesToEliminate - enemiesEliminated <= 0)
        {
            newWaveTimer = 7;
            WaitingForWave = true;
            waveNum++;
        }
    }
}
