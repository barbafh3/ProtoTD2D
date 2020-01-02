using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class BuildingController : MonoBehaviour
{


  [SerializeField]
  GameObject towerContent;

  [SerializeField]
  GameObject upgradeContent;

  [SerializeField]
  GameObject arrowTowerButton;

  [SerializeField]
  TextMeshProUGUI arrowTowerCost;

  [SerializeField]
  GameObject cannonTowerButton;

  [SerializeField]
  TextMeshProUGUI cannonTowerCost;

  [SerializeField]
  GameObject prefab;

  GameObject currentTower;

  public bool isAvailable = true;

  void Awake()
  {
    // TowerManager.Instance.OnEnable();
    SetAllButtons();
  }

  void Update()
  {
    CheckTowerPrice();
  }

  void SetAllButtons()
  {
    SetButtonInfo(arrowTowerCost, arrowTowerButton, TowerList.ArrowTower);
    SetButtonInfo(cannonTowerCost, cannonTowerButton, TowerList.CannonTower);
  }

  void SetButtonInfo(TextMeshProUGUI costText, GameObject button, TowerList tower)
  {
    var newSpriteState = new SpriteState();
    costText.text = TowerManager.GetTowerInfo(tower).price.ToString();
    button.GetComponent<Image>().sprite = TowerManager.GetTowerInfo(tower).buttonBaseSprite;
    newSpriteState.disabledSprite = TowerManager.GetTowerInfo(tower).buttonDisabledSprite;
    newSpriteState.highlightedSprite = TowerManager.GetTowerInfo(tower).buttonHighlightSprite;
    newSpriteState.pressedSprite = TowerManager.GetTowerInfo(tower).buttonPressedSprite;
    button.GetComponent<Button>().spriteState = newSpriteState;
  }

  private void OnMouseDown()
  {
    if (isAvailable)
      towerContent.SetActive(!towerContent.activeSelf);
    else
      upgradeContent.SetActive(!upgradeContent.activeSelf);
  }

  public void BuyTower(Tower tower)
  {
    if (GameManager.Instance.currentPlayerCurrency >= TowerManager.towerList[tower.name].price)
    {
      currentTower = Instantiate(prefab, transform.position, Quaternion.identity);
      currentTower.transform.parent = transform;
      TowerManager.Instance.RegisterTower(currentTower);
      currentTower.GetComponentInChildren<TowerController>().towerInfo = TowerManager.towerList[tower.name];
      GameManager.Instance.SpendCurrency(TowerManager.towerList[tower.name].price);
      towerContent.SetActive(false);
      isAvailable = false;
    }
  }

  public void SellTower()
  {
    upgradeContent.SetActive(false);
    GameManager.Instance.ReceiveCurrency(null, currentTower.GetComponentInChildren<TowerController>().refundValue);
    TowerManager.Instance.UnregisterTower(currentTower);
    Destroy(currentTower);
    isAvailable = true;
  }

  void CheckTowerPrice()
  {
    if (GameManager.Instance.currentPlayerCurrency < 75)
    {
      arrowTowerButton.GetComponent<Button>().interactable = false;
    }
    else
    {
      arrowTowerButton.GetComponent<Button>().interactable = true;
    }
    if (GameManager.Instance.currentPlayerCurrency < 100)
    {
      cannonTowerButton.GetComponent<Button>().interactable = false;
    }
    else
    {
      cannonTowerButton.GetComponent<Button>().interactable = true;
    }
  }

}