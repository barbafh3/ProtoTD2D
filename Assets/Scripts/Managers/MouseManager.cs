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
    if (Input.GetMouseButtonUp(0))
    {
      // Ray traces to mouse position on click
      //   RaycastHit2D hit = new ddRaycastHit2D();
      //   Ray2D ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

      // Set selected to null if no object is hit
      if (hit.collider != null)
      {
        Debug.Log(hit.collider.gameObject);
        selectedObject = hit.collider.gameObject;
      }
      else
      {
        Debug.Log("No object");
        selectedObject = null;
      }
    }
  }

  void OnDisable()
  {
    _instance = null;
  }

}
