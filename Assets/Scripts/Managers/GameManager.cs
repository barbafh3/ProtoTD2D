using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class GameManager : MonoBehaviour
{

  public Dictionary<string, Map> mapsList = null;

  public bool isGamePaused = false;

  public int currentPlayerHealth { get; set; }

  [SerializeField]
  int _maxPlayerHealth = 100;

  public int currentPlayerCurrency { get; set; }

  [SerializeField]
  int _startingPlayerCurrency = 200;

  public bool gameEnded = false;

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

  void OnDisable()
  {
    instance = null;
  }

  void Awake()
  {
    if (instance != this && instance != null)
    {
      Destroy(gameObject);
    }
    Time.timeScale = 1f;
    instance = this;
    // DontDestroyOnLoad(gameObject);
    currentPlayerHealth = _maxPlayerHealth;
    currentPlayerCurrency = _startingPlayerCurrency;
    mapsList = new Dictionary<string, Map>();
    LoadMapsList();
  }

  public void LoadManager() { }

  void LoadMapsList()
  {
    foreach (string mapName in Enum.GetNames(typeof(Maps)))
    {
      Debug.Log("Loading map info: " + mapName);
      var scriptObj = Resources.Load<Map>("ScriptableObjects/Maps/" + mapName);
      mapsList.Add(mapName, scriptObj);
    }
  }

  public Map GetMapInfo(string mapName)
  {
    Debug.Log("Get map info: " + mapName);
    return mapsList[mapName];
  }

  public void RestartResources()
  {
    currentPlayerHealth = _maxPlayerHealth;
    currentPlayerCurrency = _startingPlayerCurrency;
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