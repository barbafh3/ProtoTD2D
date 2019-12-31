using UnityEngine;
using UnityEngine.UI;

namespace ProtoTD2D {
    public class TowerController : MonoBehaviour {
        public Tower tower;

        public Image spriteButtonBuy;
        public Text txtPrice;

        private Building building;

        private void Start () {
            building = transform.parent.parent.parent.gameObject.GetComponent<Building>();

            spriteButtonBuy.sprite = tower.buttonSprite;
            txtPrice.text = "$ " + tower.price;
        }


        public void BuyTower () {
            building.BuyTower(tower.towerSprite);
        }
    }
}