using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

  public GameObject selectedObject { get; set; }

  private static MouseManager _instance;

  public static MouseManager Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType<MouseManager>();
        if (_instance == null)
        {
          GameObject obj = new GameObject();
          obj.name = typeof(MouseManager).Name;
          _instance = obj.AddComponent<MouseManager>();
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
    DontDestroyOnLoad(gameObject);
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
        Debug.Log(hit.collider.gameObject);
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

  void OnDisable()
  {
    _instance = null;
  }

}
