using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

  public bool isGamePaused = false;

  public int currentPlayerHealth { get; set; }
  private int _maxPlayerHealth = 100;

  public int currentPlayerCurrency { get; set; }
  private int _startingPlayerCurrency = 200;

  List<GameObject> deployedTowers;

  GameObject pauseMenuCanvas;

  public delegate void OnEnemyDeathEventHandler(GameObject obj);
  public event OnEnemyDeathEventHandler OnEnemyDeath;

  private static GameManager _instance;

  public static GameManager Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType<GameManager>();
        if (_instance == null)
        {
          GameObject obj = new GameObject();
          obj.name = typeof(GameManager).Name;
          _instance = obj.AddComponent<GameManager>();
        }
      }
      return _instance;
    }
  }

  void OnDisable()
  {
    _instance = null;
  }

  void Awake()
  {
    if (_instance != this && _instance != null)
    {
      Destroy(gameObject);
    }
    _instance = this;
    DontDestroyOnLoad(gameObject);
    mapLoadList = new
    {
      mainMenu = new Action(() => { SceneManager.LoadScene("MainMenu"); }),
      map1 = new Action(() => { SceneManager.LoadScene("Map1"); }),
      map2 = new Action(() => { SceneManager.LoadScene("Map2"); }),
      gameOver = new Action(() => { SceneManager.LoadScene("GameOver"); }),
    };
    currentPlayerHealth = _maxPlayerHealth;
    currentPlayerCurrency = _startingPlayerCurrency;
    deployedTowers = new List<GameObject>();
    pauseMenuCanvas = GameObject.Find("PauseUI");
    pauseMenuCanvas.SetActive(false);
  }

  public void EnemyDied(GameObject enemy, int? value)
  {
    OnEnemyDeath(enemy);
  }

  public void RegisterTower(GameObject tower)
  {
    var towerScript = tower.GetComponentInChildren<TowerController>();
    OnEnemyDeath += new OnEnemyDeathEventHandler(towerScript.EnemyDied);
    deployedTowers.Add(tower);
  }

  public void UnregisterTower(GameObject tower)
  {
    Debug.Log(tower);
    var towerScript = tower.GetComponentInChildren<TowerController>();
    Debug.Log(towerScript);
    OnEnemyDeath -= new OnEnemyDeathEventHandler(towerScript.EnemyDied);
    deployedTowers.Remove(tower);
  }

  private object mapLoadList;

  public void LoadNextMap(string name)
  {
    Action loadScene = (Action)mapLoadList.GetType().GetProperty(name).GetValue(mapLoadList);
    loadScene();
  }

  public void PlayerTakeDamage()
  {
    currentPlayerHealth--;
  }

  public void SpendCurrency(int value)
  {
    currentPlayerCurrency -= value;
  }

  public void ReceiveCurrency(GameObject obj, int? value)
  {
    if (value != null)
    {
      currentPlayerCurrency += (int)value;
    }
  }

  public void Resume()
  {
    Time.timeScale = 1f;
    pauseMenuCanvas.SetActive(false);
    // pauseMenuCanvas.GetComponentInChildren<BoxCollider2D>().enabled = false;
    isGamePaused = false;
  }

  public void Pause()
  {
    Time.timeScale = 0f;
    pauseMenuCanvas.SetActive(true);
    // pauseMenuCanvas.enabled = true;
    // pauseMenuCanvas.GetComponentInChildren<BoxCollider2D>().enabled = true;
    isGamePaused = true;
  }

}