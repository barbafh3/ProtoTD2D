using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

  public int currentPlayerHealth { get; set; }
  private int maxPlayerHealth = 100;

  public int currentPlayerCurrency { get; set; }
  private int startingPlayerCurrency = 200;

  private static GameManager instance;

  public static GameManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = FindObjectOfType<GameManager>();
        if (instance == null)
        {
          GameObject obj = new GameObject();
          obj.name = typeof(GameManager).Name;
          instance = obj.AddComponent<GameManager>();
        }
      }
      return instance;
    }
  }

  void Awake()
  {
    if (instance != this && instance != null)
    {
      Destroy(gameObject);
    }
    instance = this;
    mapLoadList = new
    {
      mainMenu = new Action(() => { SceneManager.LoadScene("MainMenu"); }),
      map1 = new Action(() => { SceneManager.LoadScene("Map1"); }),
      map2 = new Action(() => { SceneManager.LoadScene("Map2"); }),
      gameOver = new Action(() => { SceneManager.LoadScene("GameOver"); }),
    };
    currentPlayerHealth = maxPlayerHealth;
    currentPlayerCurrency = startingPlayerCurrency;
  }

  void OnDisable()
  {
    instance = null;
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