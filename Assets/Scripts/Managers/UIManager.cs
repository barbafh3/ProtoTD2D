using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

  GameObject pauseMenuCanvas;

  public GameObject selectedObject { get; set; }

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
    SetPauseCanvas();
  }

  void OnDisable()
  {
    instance = null;
  }

  void Update()
  {
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
          selectedObject = hit.collider.gameObject;
        }
      }
      //  Set selected to null if no object is hit
      else
      {
        selectedObject = null;
      }
    }
  }

  public void SetPauseCanvas()
  {
    pauseMenuCanvas = GameObject.Find("PauseUI");
    pauseMenuCanvas.SetActive(false);
  }

  public void Resume()
  {
    Time.timeScale = 1f;
    pauseMenuCanvas.SetActive(false);
    GameManager.Instance.isGamePaused = false;
  }

  public void Pause()
  {
    Time.timeScale = 0f;
    pauseMenuCanvas.SetActive(true);
    GameManager.Instance.isGamePaused = true;
  }
}
