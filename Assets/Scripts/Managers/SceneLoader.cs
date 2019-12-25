using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

  private static SceneLoader _instance;

  public static SceneLoader Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType<SceneLoader>();
        if (_instance == null)
        {
          GameObject obj = new GameObject();
          obj.name = typeof(GameManager).Name;
          _instance = obj.AddComponent<SceneLoader>();
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
  }

  void OnDisable()
  {
    _instance = null;
  }

  public static void LoadScene(GameScenes scene)
  {
    SceneManager.LoadScene(scene.ToString());
  }
}
