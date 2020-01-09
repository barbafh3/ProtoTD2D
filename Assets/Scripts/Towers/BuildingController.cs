using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class BuildingController : MonoBehaviour
{


  [SerializeField]
  GameObject towerContent = null;

  [SerializeField]
  GameObject upgradeContent = null;

  [SerializeField]
  GameObject arrowTowerButton = null;

  [SerializeField]
  TextMeshProUGUI arrowTowerCost = null;

  [SerializeField]
  GameObject cannonTowerButton = null;

  [SerializeField]
  TextMeshProUGUI cannonTowerCost = null;

  [SerializeField]
  GameObject slowTowerButton = null;

  [SerializeField]
  TextMeshProUGUI slowTowerCost = null;

  [SerializeField]
  GameObject prefab = null;

  GameObject _currentTower = null;

  public bool isAvailable = true;

  void Awake()
  {
    SetAllButtons();
  }

  void Start()
  {
    TowerManager.Instance.LoadTowerManager();
    if (SceneManager.GetActiveScene().name == GameScenes.Map1.ToString())
    {
      var slowButton = slowTowerButton.GetComponent<Button>();
      var sprState = new SpriteState();
      slowButton.interactable = false;
      sprState.disabledSprite = TowerManager.Instance.GetTowerInfo(Towers.SlowTower).buttonUnavailableSprite;
      slowButton.spriteState = sprState;
    }
  }

  void Update()
  {
    SetButtonsInteractable();
  }

  public void HideUI()
  {
    if (_currentTower != null)
    {
      _currentTower.GetComponentInChildren<TowerController>().SetRangeVisibility(false);
    }
    upgradeContent.SetActive(false);
    towerContent.SetActive(false);
  }

  public void ShowUI()
  {
    if (isAvailable)
    {
      towerContent.SetActive(true);
    }
    else
    {
      upgradeContent.SetActive(true);
      _currentTower.GetComponentInChildren<TowerController>().SetRangeVisibility(true);
    }
  }

  void SetAllButtons()
  {
    SetButtonInfo(arrowTowerCost, arrowTowerButton, Towers.ArrowTower);
    SetButtonInfo(cannonTowerCost, cannonTowerButton, Towers.CannonTower);
    SetButtonInfo(slowTowerCost, slowTowerButton, Towers.SlowTower);
  }

  void SetButtonInfo(TextMeshProUGUI costText, GameObject button, Towers tower)
  {
    var newSpriteState = new SpriteState();
    costText.text = "$" + TowerManager.Instance.GetTowerInfo(tower).price.ToString();
    button.GetComponent<Image>().sprite = TowerManager.Instance.GetTowerInfo(tower).buttonBaseSprite;
    newSpriteState.disabledSprite = TowerManager.Instance.GetTowerInfo(tower).buttonDisabledSprite;
    newSpriteState.highlightedSprite = TowerManager.Instance.GetTowerInfo(tower).buttonHighlightSprite;
    newSpriteState.pressedSprite = TowerManager.Instance.GetTowerInfo(tower).buttonPressedSprite;
    button.GetComponent<Button>().spriteState = newSpriteState;
  }

  private void OnMouseDown()
  {
  }

  public void BuyTower(Tower tower)
  {
    Debug.Log("Buy tower " + tower);
    if (GameManager.Instance.currentPlayerCurrency >= TowerManager.Instance.towerList[tower.name].price)
    {
      _currentTower = Instantiate(prefab, transform.position, Quaternion.identity);
      _currentTower.transform.parent = transform;
      TowerManager.Instance.RegisterTower(_currentTower);
      _currentTower.GetComponentInChildren<TowerController>().towerInfo = TowerManager.Instance.towerList[tower.name];
      GameManager.Instance.SpendCurrency(TowerManager.Instance.towerList[tower.name].price);
      towerContent.SetActive(false);
      isAvailable = false;
    }
  }

  public void SellTower()
  {
    upgradeContent.SetActive(false);
    GameManager.Instance.ReceiveCurrency(null, _currentTower.GetComponentInChildren<TowerController>().refundValue);
    TowerManager.Instance.UnregisterTower(_currentTower);
    var sellAnimation = transform.GetChild(1).Find("SellAnimation");
    var animator = sellAnimation.GetComponent<Animator>();
    var text = sellAnimation.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    text.text = "+" + _currentTower.GetComponentInChildren<TowerController>().towerInfo.refundValue.ToString();
    sellAnimation.GetChild(0).gameObject.SetActive(true);
    animator.Play("SellTower");
    Destroy(_currentTower);
    isAvailable = true;
  }

  void SetButtonsInteractable()
  {

    arrowTowerButton.GetComponent<Button>().interactable = CheckTowerPrice(75);
    cannonTowerButton.GetComponent<Button>().interactable = CheckTowerPrice(100);
    if (SceneManager.GetActiveScene().name != GameScenes.Map1.ToString())
    {
      slowTowerButton.GetComponent<Button>().interactable = CheckTowerPrice(125);
    }
  }

  bool CheckTowerPrice(int price)
  {
    return (GameManager.Instance.currentPlayerCurrency >= price);
  }

}