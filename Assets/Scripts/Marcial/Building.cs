using UnityEngine;

namespace ProtoTD2D {
    public class Building : MonoBehaviour {

        public GameObject contentTower;
        public GameObject contentSell;
        public SpriteRenderer towerSprite;

        private bool inUse;

        private void OnMouseDown () {
            if (!inUse)
                contentTower.SetActive (!contentTower.activeSelf);
            else
                contentSell.SetActive (!contentSell.activeSelf);
        }

        public void BuyTower (Sprite sprite) {
            inUse = true;
            towerSprite.sprite = sprite;
            contentTower.SetActive (false);
        }

        public void SellTower () {
            inUse = false;
            towerSprite.sprite = null;
            contentSell.SetActive (false);
        }

    }
}