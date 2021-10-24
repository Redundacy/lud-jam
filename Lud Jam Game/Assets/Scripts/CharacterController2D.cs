using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour {
    public float MoveSpeed = 1f;
    public float BlockCooldown = 5f;
    public float AttackSpeed = 1f;
    public GameObject HealthBar;
    public GameObject Slash;
    public List<Sprite> SpriteList = new List<Sprite>();
    public Animator Animator;

    private Rigidbody2D _rb;
    private float inputHorizontal;
    private float inputVertical;
    private int attackTime = 0;
    private bool attacking;
    private bool blocking;
    private HealthBar healthBar;

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        healthBar = HealthBar.GetComponent<HealthBar>();
        healthBar.SetMaxValue(BlockCooldown);
        GetComponent<SpriteRenderer>().sprite = SpriteList[0];
        Animator.speed = (1f / AttackSpeed);
    }

    void FixedUpdate() {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");

        if (healthBar.slider.value < BlockCooldown) {
            healthBar.slider.value += (1f / 60);
        }

        if (attackTime < AttackSpeed * 60) {
            attackTime++;
        } else if (attackTime < AttackSpeed * 90) {
            attackTime++;
        }
        else {
            Slash.SetActive(false);
            attacking = false;
        }

        if (Input.GetMouseButton(0) && !attacking) {
            Debug.Log("Attack Swing!");
            // if blocking, stop blocking
            if (blocking) {
                GetComponent<BoxCollider2D>().enabled = false;
                blocking = false;
                GetComponent<SpriteRenderer>().sprite = SpriteList[0];
            }
            Slash.SetActive(true);
            attacking = true;
            attackTime = 0;
        }
        if (Input.GetMouseButton(1) && !attacking && healthBar.slider.value == BlockCooldown) {
            Debug.Log("Block!");
            GetComponent<BoxCollider2D>().enabled = true;
            blocking = true;
            GetComponent<SpriteRenderer>().sprite = SpriteList[1];
            // hold out the shield in front of character
            // or instead just do radius shield
        }
        else {
            GetComponent<BoxCollider2D>().enabled = false;
            blocking = false;
            GetComponent<SpriteRenderer>().sprite = SpriteList[0];
        }

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
        if (collision.tag == "Bad" && !blocking) {
            // todo reset the game
        } else if (collision.tag == "Bad" && blocking) {
            Destroy(collision.gameObject);
            blocking = false;
            healthBar.slider.value = 0;
        }
    }
}
