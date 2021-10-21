using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour {
    public float MoveSpeed = 1f;
    public float BlockCooldown = 5f;

    private Rigidbody2D _rb;
    private float inputHorizontal;
    private float inputVertical;
    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");

        _rb.velocity = new Vector2(inputHorizontal * MoveSpeed, inputVertical * MoveSpeed);

        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg - 90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bad") {
            // todo reset the game
        }
    }
}
