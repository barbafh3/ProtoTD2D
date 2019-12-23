using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{

  [SerializeField]
  Tower tower;

  float baseDamage;

  float fireRate;

  Tower[] upgradeList;

  GameObject projectileSprite;

  private GameObject currentTarget = null;

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (currentTarget == null)
    {
      currentTarget = collider.gameObject;
    }
  }

  void OnTriggerExit2D(Collider2D collider)
  {
    if (collider.gameObject == currentTarget)
    {
      currentTarget = null;
    }
  }

  void OnTriggerStay2D(Collider2D collider)
  {
    if (currentTarget == null)
    {
      currentTarget = collider.gameObject;
    }
  }

  void SpawnProjectile()
  {
    var projectileObject = Instantiate<GameObject>(projectileSprite, transform.position, Quaternion.identity);
    projectileObject.GetComponent<ProjectileBehaviour>().target = currentTarget;
  }

  IEnumerator DoDamage()
  {
    while (true)
    {
      if (currentTarget)
      {
        var monsterScript = currentTarget.GetComponent<MonsterBehaviour>();
        SpawnProjectile();
        if (monsterScript.currentHealth <= 0)
        {
          currentTarget = null;
        }
      }
      yield return new WaitForSeconds(1);
    }
  }

  void LoadTowerInfo()
  {
    fireRate = tower.fireRate;
    upgradeList = tower.upgradeList;
    projectileSprite = tower.projectileSprite;
  }

  void Start()
  {
    LoadTowerInfo();
    StartCoroutine("DoDamage");
  }

  // Update is called once per frame
  void Update()
  {
    // yield return new WaitForSeconds(1);
    // if (currentTarget != null)
    // {
    //   print(currentTarget.tag);
    // }
  }
}
