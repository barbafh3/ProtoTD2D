using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerControllerMarcial2 : MonoBehaviour
{

  public Tower tower;

  public Image spriteButtonBuy;
  public Text txtPrice;

  private Building building;

  private void Start()
  {
    building = transform.parent.parent.parent.gameObject.GetComponent<Building>();
    spriteButtonBuy = transform.Find("Button Buy").GetComponent<Image>();
    txtPrice = gameObject.GetComponentInChildren<Text>();
    Debug.Log(tower);
    spriteButtonBuy.sprite = tower.buttonBaseSprite;
    txtPrice.text = "$ " + tower.price;
  }


  public void BuyTower()
  {
    building.BuyTower(tower.towerSprite);
  }
  // Start is called before the first frame update
}
