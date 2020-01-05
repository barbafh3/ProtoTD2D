using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Map")]
public class Map : ScriptableObject
{

  public Sprite buildingSlot;
  public Transform[] mapNodes;
  public List<Wave> mapWaves;

}
