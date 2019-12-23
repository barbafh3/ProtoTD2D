using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

  [SerializeField] float speed = 2f;
  Rigidbody2D playerBody;
  float moveH, moveV;

  // Start is called before the first frame update
  void Start()
  {
    playerBody = GetComponent<Rigidbody2D>();

  }

  // Update is called once per frame
  void Update()
  {
    moveH = Input.GetAxisRaw("Horizontal") * speed;
    moveV = Input.GetAxisRaw("Vertical") * speed;
  }

  void FixedUpdate()
  {
    playerBody.velocity = new Vector2(moveH, moveV);
  }
}
