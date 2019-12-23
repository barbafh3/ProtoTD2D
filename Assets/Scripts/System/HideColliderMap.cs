using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideColliderMap : MonoBehaviour
{

  TilemapRenderer tilemapRenderer;

  void Awake()
  {
    tilemapRenderer = GetComponent<TilemapRenderer>();
  }

  // Start is called before the first frame update
  void Start()
  {
    tilemapRenderer.enabled = false;
  }

}
