using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWaypoints : MonoBehaviour
{

  SpriteRenderer _spriteRenderer;

  // Start is called before the first frame update
  void Start()
  {
    _spriteRenderer = GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    _spriteRenderer.enabled = false;
  }
}
