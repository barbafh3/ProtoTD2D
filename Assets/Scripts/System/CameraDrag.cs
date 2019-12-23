using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

  public float dragSpeed = 0.1f;
  private Vector3 dragOrigin;


  void LateUpdate()
  {
    if (Input.GetMouseButtonDown(2))
    {
      dragOrigin = Input.mousePosition;
      return;
    }

    if (!Input.GetMouseButton(2)) return;

    Vector2 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
    Vector2 move = new Vector2(pos.x * dragSpeed, pos.y * dragSpeed);

    transform.Translate(move, Space.World);
  }
}