using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

  Transform pauseMenuCanvas;

  public GameObject selectedObject { get; set; }

  [SerializeField]
  TextMeshProUGUI healthText;

  [SerializeField]
  TextMeshProUGUI currencyText;

  private static UIManager instance;

  public static UIManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = FindObjectOfType<UIManager>();
        if (instance == null)
        {
          GameObject obj = new GameObject();
          obj.name = typeof(UIManager).Name;
          instance = obj.AddComponent<UIManager>();
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

  void Update()
  {
    if (healthText != null && currencyText != null)
    {
      healthText.text = GameManager.Instance.currentPlayerHealth.ToString();
      currencyText.text = GameManager.Instance.currentPlayerCurrency.ToString();
    }
  }

  void OnDisable()
  {
    instance = null;
  }

  public void Resume()
  {
    Time.timeScale = 1f;
    transform.GetChild(0).gameObject.SetActive(false);
    GameManager.Instance.isGamePaused = false;
  }

  public void Pause()
  {
    Time.timeScale = 0f;
    transform.GetChild(0).gameObject.SetActive(true);
    GameManager.Instance.isGamePaused = true;
  }

  public void GoToMap1()
  {
    if (GameManager.Instance.isGamePaused)
    {
      UIManager.Instance.Resume();
    }
    SceneLoader.LoadScene(GameScenes.Map1);
  }

  public void ReplayMap1()
  {
    SceneLoader.LoadScene(GameScenes.Map1);
  }

  public void GoToMainMenu()
  {
    if (GameManager.Instance.isGamePaused)
    {
      UIManager.Instance.Resume();
    }
    SceneLoader.LoadScene(GameScenes.MainMenu);
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
