using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

  [SerializeField]
  TextMeshProUGUI healthText = null;

  [SerializeField]
  TextMeshProUGUI currencyText = null;

  public GameObject selectedObject = null;

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
  }

  void Update()
  {
    if (healthText != null && currencyText != null)
    {
      healthText.text = GameManager.Instance.currentPlayerHealth.ToString();
      currencyText.text = GameManager.Instance.currentPlayerCurrency.ToString();
    }
    //  If left mouse button was clicked do the following
    if (Input.GetMouseButtonDown(0))
    {
      //  Ray traces from screen to mouse position on click
      Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      // Return hit if any object was hit by the ray trace
      RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
      //  If the hit exists, set the selected object as the hit collider
      if (hit.collider != null)
      {
        if (hit.collider.gameObject.tag == "Tower")
        {
          if (selectedObject != null)
          {
            selectedObject.GetComponentInChildren<BuildingController>().HideUI();
          }
          selectedObject = hit.collider.gameObject;
          selectedObject.GetComponentInChildren<BuildingController>().ShowUI();
        }
      }
      //  Set selected to null if no object is hit
      else
      {
        if (selectedObject != null)
        {
          selectedObject.GetComponentInChildren<BuildingController>().HideUI();
        }
        selectedObject = null;
      }
    }
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
