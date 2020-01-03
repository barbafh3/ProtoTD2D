using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideColliderMap : MonoBehaviour
{

  TilemapRenderer _tilemapRenderer = null;

  void Awake()
  {
    _tilemapRenderer = GetComponent<TilemapRenderer>();
  }

  // Start is called before the first frame update
  void Start()
  {
    _tilemapRenderer.enabled = false;
  }

}
