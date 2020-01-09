using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

  private static SceneLoader instance;

  public static SceneLoader Instance
  {
    get
    {
      if (instance == null)
      {
        instance = FindObjectOfType<SceneLoader>();
        if (instance == null)
        {
          GameObject obj = new GameObject();
          obj.name = typeof(GameManager).Name;
          instance = obj.AddComponent<SceneLoader>();
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
  }

  void OnDisable()
  {
    instance = null;
  }

  public static void LoadScene(GameScenes scene)
  {
    Debug.Log("Load Scene");
    SceneManager.LoadScene(scene.ToString());
  }

  public static void LoadScene(string name)
  {
    // GameManager.Instance.RestartResources();
    SceneManager.LoadScene(name);
  }

}
