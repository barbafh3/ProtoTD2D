using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class GameManager : MonoBehaviour
{

  [System.Serializable]
  public class MapsDict : SerializableDictionaryBase<string, Map> { }

  [SerializeField]
  public MapsDict mapsList = null;

  public bool isGamePaused = false;

  public int currentPlayerHealth { get; set; }

  [SerializeField]
  int _maxPlayerHealth = 100;

  public int currentPlayerCurrency { get; set; }

  [SerializeField]
  int _startingPlayerCurrency = 200;

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
    instance = this;
    DontDestroyOnLoad(gameObject);
    currentPlayerHealth = _maxPlayerHealth;
    currentPlayerCurrency = _startingPlayerCurrency;
  }

  public Map GetMapInfo(string mapName)
  {
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