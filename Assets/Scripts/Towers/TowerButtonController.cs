using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonController : MonoBehaviour
{

  [SerializeField]
  TowerButton buttonInfo;

  [SerializeField]
  GameObject towerPrefab;

  Transform _rootObj;
  TowerPlacement _rootScript;
  Canvas _optionsCanvas;
  Canvas _sellCanvas;

  Button _button;

  int _towerCost;

  void LoadButtonInfo()
  {
    _towerCost = buttonInfo.towerCost;
    _optionsCanvas = GetComponentInParent<Canvas>();
    _button = GetComponentInChildren<Button>();
    _rootObj = transform.parent.parent;
    _rootScript = _rootObj.GetComponent<TowerPlacement>();
  }

  void SetCanvas()
  {
    Canvas[] canvasList = _rootObj.GetComponentsInChildren<Canvas>();
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
  public void SellTower()
  {
    _sellCanvas.enabled = false;
    _rootScript.isSlotAvalable = true;
    var towerController = _rootScript.tower.GetComponentInChildren<TowerController>();
    GameManager.Instance.ReceiveCurrency(null, towerController.refundValue);
    Destroy(_rootScript.tower);
    MouseManager.Instance.selectedObject = null;
  }

  public void PlaceTower()
  {
    if (_rootScript.isSlotAvalable == true)
    {
      if (GameManager.Instance.currentPlayerCurrency >= _towerCost)
      {
        GameManager.Instance.SpendCurrency(_towerCost);
        _rootScript.tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        _rootScript.tower.GetComponentInChildren<SpriteRenderer>().sortingOrder = 5;
        _rootScript.isSlotAvalable = false;
        _optionsCanvas.enabled = false;
        MouseManager.Instance.selectedObject = null;
      }
      else
      {
        Debug.Log("Not enough gold.");
      }
    }
    else
    {

    }
  }

  void Start()
  {
    LoadButtonInfo();
    SetCanvas();
  }

  // Update is called once per frame
  void Update()
  {
    if (GameManager.Instance.currentPlayerCurrency < _towerCost)
    {
      _button.interactable = false;
    }
    else
    {
      _button.interactable = true;
    }

  }
}
