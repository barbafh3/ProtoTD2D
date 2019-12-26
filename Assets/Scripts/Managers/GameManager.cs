using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


  public int currentPlayerHealth { get; set; }
  private int _maxPlayerHealth = 100;

  public int currentPlayerCurrency { get; set; }
  private int _startingPlayerCurrency = 200;

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

  void Awake()
  {
    if (_instance != this && _instance != null)
    {
      Destroy(gameObject);
    }
    _instance = this;
    mapLoadList = new
    {
      mainMenu = new Action(() => { SceneManager.LoadScene("MainMenu"); }),
      map1 = new Action(() => { SceneManager.LoadScene("Map1"); }),
      map2 = new Action(() => { SceneManager.LoadScene("Map2"); }),
      gameOver = new Action(() => { SceneManager.LoadScene("GameOver"); }),
    };
    currentPlayerHealth = _maxPlayerHealth;
    currentPlayerCurrency = _startingPlayerCurrency;
  }

  void OnDisable()
  {
    _instance = null;
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
}