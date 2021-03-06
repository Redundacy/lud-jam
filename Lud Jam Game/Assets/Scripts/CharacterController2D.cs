using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour {
    public float MoveSpeed = 1f;
    public float BlockCooldown = 5f;
    public float AttackSpeed = 1f;
    public GameObject HealthBar;
    public GameObject Slash;
    public List<Sprite> SpriteList = new List<Sprite>();
    public Animator Animator;
    public GameObject GameOver;

    private Rigidbody2D _rb;
    private float inputHorizontal;
    private float inputVertical;
    private float actualMoveSpeed;
    private int attackTime = 0;
    private bool attacking;
    private bool blocking;
    private bool dead = false;
    private HealthBar healthBar;

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        healthBar = HealthBar.GetComponent<HealthBar>();
        healthBar.SetMaxValue(BlockCooldown);
        GetComponent<SpriteRenderer>().sprite = SpriteList[0];
        Animator.speed = (1f / AttackSpeed);
        actualMoveSpeed = MoveSpeed;
    }

    void FixedUpdate() {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        if (dead) {
            if (Input.anyKeyDown) {
                SceneManager.LoadScene("Menu");
            }
            Slash.SetActive(false);
            transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            return;
        }

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

        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && !attacking) {
            // Debug.Log("Attack Swing!");
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
        if ((Input.GetMouseButton(1) || Input.GetKey(KeyCode.J)) && !attacking && healthBar.slider.value >= BlockCooldown/2) {
            // Debug.Log("Block!");
            GetComponent<BoxCollider2D>().enabled = true;
            blocking = true;
            GetComponent<SpriteRenderer>().sprite = SpriteList[1];
            // hold out the shield in front of character
            // or instead just do radius shield
            actualMoveSpeed = 1f;
        }
        else {
            GetComponent<BoxCollider2D>().enabled = false;
            blocking = false;
            GetComponent<SpriteRenderer>().sprite = SpriteList[0];
            actualMoveSpeed = MoveSpeed;
        }

        _rb.velocity = new Vector2(inputHorizontal * actualMoveSpeed, inputVertical * actualMoveSpeed);

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
            GameOver.SetActive(true);
            dead = true;
        } else if (collision.tag == "Bad" && blocking) {
            Destroy(collision.gameObject);
            blocking = false;
            healthBar.slider.value -= BlockCooldown/2;
        }
    }
}
