using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RotaryHeart.Lib.SerializableDictionary;

public class UIManager : MonoBehaviour
{

  [SerializeField]
  TextMeshProUGUI healthText = null;

  [SerializeField]
  TextMeshProUGUI currencyText = null;

  [SerializeField]
  TextMeshProUGUI towerNameText = null;

  [SerializeField]
  TextMeshProUGUI towerRangeText = null;

  [SerializeField]
  TextMeshProUGUI towerCostText = null;

  [SerializeField]
  TextMeshProUGUI towerRefundText = null;

  public GameObject selectedObject = null;

  [SerializeField]
  Transform infoPanel = null;

  [SerializeField]
  Transform pausePanel = null;

  Dictionary<string, Tower> towerInfoList = null;

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
    towerInfoList = new Dictionary<string, Tower>();
    LoadTowerButtonList();
  }

  public void LoadUIManager() { }

  void LoadTowerButtonList()
  {

    foreach (string towerName in Enum.GetNames(typeof(TowerButtons)))
    {
      var scriptObj = Resources.Load<TowerButton>("ScriptableObjects/Buttons/" + towerName);
      towerInfoList.Add(towerName, scriptObj.towerInfo);
    }
    foreach (KeyValuePair<string, Tower> towerButton in towerInfoList)
    {
      Debug.Log(towerButton);
    }
  }

  GameObject GetObjectWithRaycast()
  {
    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

    if (hit.collider != null)
    {
      return hit.collider.gameObject;
    }
    else
    {
      return null;
    }
  }

  void GetObjectOnClick()
  {
    var newHit = GetObjectWithRaycast();

    if (newHit != null)
    {
      if (newHit.gameObject.tag == "Tower")
      {
        if (selectedObject != null)
        {
          selectedObject.GetComponentInChildren<BuildingController>().HideUI();
        }
        selectedObject = newHit.gameObject;
        selectedObject.GetComponentInChildren<BuildingController>().ShowUI();
      }
    }
    else
    {
      if (selectedObject != null)
      {
        selectedObject.GetComponentInChildren<BuildingController>().HideUI();
      }
      selectedObject = newHit;
    }
  }

  void Update()
  {
    LoadPlayerInfo();

    if (Input.GetMouseButtonDown(0))
    {
      GetObjectOnClick();
    }

    var hoverHit = GetObjectWithRaycast();
    if (hoverHit != null)
    {
      if (hoverHit.tag == "Tower Button")
      {
        Debug.Log(hoverHit.name);
        LoadPanelInfo(towerInfoList[hoverHit.name]);
        ShowInfoPanel();
      }
    }
    else
    {
      HideInfoPanel();
    }
  }

  void LoadPlayerInfo()
  {
    if (healthText != null && currencyText != null)
    {
      healthText.text = GameManager.Instance.currentPlayerHealth.ToString();
      currencyText.text = GameManager.Instance.currentPlayerCurrency.ToString();
    }
  }

  void LoadPanelInfo(Tower tower)
  {
    towerNameText.text = tower.name;
    towerRangeText.text = tower.range.ToString();
    towerCostText.text = tower.price.ToString();
    towerRefundText.text = tower.refundValue.ToString();
  }

  public void ShowInfoPanel()
  {
    if (infoPanel != null)
    {
      infoPanel.gameObject.SetActive(true);
    }
  }

  public void HideInfoPanel()
  {
    if (infoPanel != null)
    {
      infoPanel.gameObject.SetActive(false);
    }
  }

  public void Resume()
  {
    Time.timeScale = 1f;
    if (pausePanel != null)
    {
      pausePanel.gameObject.SetActive(false);
    }
    GameManager.Instance.isGamePaused = false;
  }

  public void Pause()
  {
    Time.timeScale = 0f;
    if (pausePanel != null)
    {
      pausePanel.gameObject.SetActive(true);
    }
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
