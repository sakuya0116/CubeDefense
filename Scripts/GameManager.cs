using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject tipsPanel;

    public static GameManager Instance;

    private EnemySpawner enemySpawner;

    void Awake()
    {
        Instance = this;
        enemySpawner = GetComponent<EnemySpawner>();
    }

   public void Victory()
    {
        victoryPanel.SetActive(true);
    }
	
   public void Defeat()
    {
        enemySpawner.Stop();
        gameOverPanel.SetActive(true);
    }

    public void OnButtonAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnButtonMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void OnButtonOK()
    {
        tipsPanel.SetActive(false);
    }
}
