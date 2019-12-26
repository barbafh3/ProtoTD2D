using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{

  [SerializeField]
  List<GameObject> towerList;

  Canvas _sellCanvas;
  Canvas _optionsCanvas;

  GameObject _tower;

  bool _isSlotAvalable = true;

  public void PlaceTower()
  {
    Debug.Log("Place tower clicked");
    if (GameManager.Instance.currentPlayerCurrency >= 75)
    {
      Debug.Log("Placing");
      GameManager.Instance.SpendCurrency(75);
      _tower = Instantiate(towerList[0], transform.position, Quaternion.identity);
      _tower.GetComponent<SpriteRenderer>().sortingOrder = 5;
      _isSlotAvalable = false;
      _optionsCanvas.enabled = false;
      MouseManager.Instance.selectedObject = null;
    }
    else
    {
      Debug.Log("Not enough gold.");
    }
  }

  public void SellTower()
  {
    _sellCanvas.enabled = false;
    _isSlotAvalable = true;
    Destroy(_tower);
    MouseManager.Instance.selectedObject = null;
  }

  void SetCanvas()
  {
    Canvas[] canvasList = gameObject.GetComponentsInChildren<Canvas>();
    foreach (Canvas canvas in canvasList)
    {
      switch (canvas.name)
      {
        case "SellUI":
          _sellCanvas = canvas;
          _sellCanvas.enabled = false;
          break;
        case "OptionsUI":
          _optionsCanvas = canvas;
          _optionsCanvas.enabled = false;
          break;
      }
    }
  }

  void ToggleButton(bool state)
  {
    var button = _optionsCanvas.GetComponentInChildren<Button>();
    button.interactable = state;
  }

  void Start()
  {
    SetCanvas();
  }

  void Update()
  {
    // Checks if this object is the current selected object.
    // If true, checks if the current slot is available
    // for construction.
    if (MouseManager.Instance.selectedObject == gameObject)
    {
      // If current slot is available, opens store
      // and removes current object from selection.
      if (_isSlotAvalable)
      {
        _optionsCanvas.enabled = true;
      }
      // If false, shows sell button instead.
      else
      {
        _sellCanvas.enabled = true;
      }
    }
    // If false, disables sellCanvas.
    else
    {
      _sellCanvas.enabled = false;
      _optionsCanvas.enabled = false;
    }
    if (GameManager.Instance.currentPlayerCurrency <= 75)
    {
      ToggleButton(false);
    }
    else
    {
      ToggleButton(true);
    }
  }
}
