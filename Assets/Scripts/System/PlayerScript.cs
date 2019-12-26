using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

  [SerializeField] float speed = 2f;
  Rigidbody2D _playerBody;
  float _moveH, _moveV;

  // Start is called before the first frame update
  void Start()
  {
    _playerBody = GetComponent<Rigidbody2D>();

  }

  // Update is called once per frame
  void Update()
  {
    _moveH = Input.GetAxisRaw("Horizontal") * speed;
    _moveV = Input.GetAxisRaw("Vertical") * speed;
  }

  void FixedUpdate()
  {
    _playerBody.velocity = new Vector2(_moveH, _moveV);
  }
}
