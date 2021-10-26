using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossHandler : MonoBehaviour {
    public GameObject Fireball;
    public GameObject HealthBar;
    public GameObject WinScreen;
    public int Health = 20;
    public enum Pattern {
        Fan,
        Line,
        Diagonal,
        Spike,
        Pillar,
    }
    private float randomLocation = 3;
    private float attackSpread = 0.25f;
    private int attackBuffer = 6;
    private bool canAttack = true;
    private int moveBuffer;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.GetComponent<HealthBar>().SetMaxValue(Health);
        HealthBar.transform.Find("Healthtext").GetComponent<Text>().text =
            HealthBar.GetComponent<HealthBar>().slider.value + "/" + Health;
    }

    // Update is called once per frame
    void Update() {
        if (attackBuffer > 5) {
            DoMove(true);
            attackBuffer = 0;
            moveBuffer++;
        }

        if (moveBuffer > 5) {
            transform.position = new Vector3(0, 2.5f);
            moveBuffer = 0;
        }

        Pattern pattern;
        if (attackBuffer <=4 && canAttack) {
            pattern = (Pattern)attackBuffer;
            canAttack = false;
            IEnumerator coroutine = DoAttack(pattern, 7 + (Health - (int)HealthBar.GetComponent<HealthBar>().slider.value));
            StartCoroutine(coroutine);
        }
        else if (canAttack)
        {
            pattern = (Pattern)Random.Range(0, 4);
            canAttack = false;
            IEnumerator coroutine = DoAttack(pattern, 7 + (Health - (int)HealthBar.GetComponent<HealthBar>().slider.value));
            StartCoroutine(coroutine);
        }
        
    }

    IEnumerator DoAttack(Pattern pattern, int amount) {
        float angleSplit = 60f / (amount - 1);
        float centerer = 30f;
        float length = 8f;
        if (transform.position.x > 0) {
            centerer += 180f;
        } else if (transform.position.x == 0) {
            centerer += 90f;
            length = 16f;
        }


        switch (pattern) {
            case Pattern.Fan:
                for (int i = 0; i < amount; i++) {
                    Instantiate(Fireball, transform.position,
                        transform.rotation * Quaternion.Euler(0, 0, i * angleSplit - centerer));
                    yield return new WaitForSeconds(attackSpread);
                }
                break;
            case Pattern.Line:
                for (int i = 0; i < amount; i++) {
                    Instantiate(Fireball, transform.position,
                        transform.rotation * Quaternion.Euler(0, 0, 30f - centerer));
                    yield return new WaitForSeconds(attackSpread);
                }
                break;
            case Pattern.Diagonal:
                if (transform.position.x == 0) {
                    for (int i = 0; i < amount; i++) {
                        Instantiate(Fireball,
                            new Vector3(transform.position.x + length / 2 - i * (length / amount) - 1f,
                                transform.position.y),
                            transform.rotation * Quaternion.Euler(0, 0, -centerer + 30f));
                        yield return new WaitForSeconds(attackSpread);
                    }
                } else {
                    for (int i = 0; i < amount; i++) {
                        Instantiate(Fireball,
                            new Vector3(transform.position.x,
                                transform.position.y + length / 2 - i * (length / amount) - 0.5f),
                            transform.rotation * Quaternion.Euler(0, 0, centerer - 30f));
                        yield return new WaitForSeconds(0.8f);
                    }
                }
                break;
            case Pattern.Spike:
                if (transform.position.x == 0) {
                    if (amount % 2 == 1) {
                        Instantiate(Fireball,
                            new Vector3(transform.position.x, transform.position.y),
                            transform.rotation * Quaternion.Euler(0, 0, -centerer + 30f));
                        yield return new WaitForSeconds(attackSpread);
                    }
                    for (int i = (amount / 2) - 1; i >= 0; i--) {
                        Instantiate(Fireball,
                            new Vector3(transform.position.x + length / 2 - i * (length / amount),
                                transform.position.y),
                            transform.rotation * Quaternion.Euler(0, 0, -centerer + 30f));
                        Instantiate(Fireball,
                            new Vector3(transform.position.x - length / 2 + i * (length / amount),
                                transform.position.y),
                            transform.rotation * Quaternion.Euler(0, 0, -centerer + 30f));
                        yield return new WaitForSeconds(attackSpread);
                    }
                } else {
                    if (amount % 2 == 1) {
                        Instantiate(Fireball,
                            new Vector3(transform.position.x, transform.position.y),
                            transform.rotation * Quaternion.Euler(0, 0, centerer - 30f));
                        yield return new WaitForSeconds(0.8f);
                    }
                    for (int i = (amount / 2) - 1; i >= 0; i--) {
                        Instantiate(Fireball,
                            new Vector3(transform.position.x,
                                transform.position.y + length / 2 - i * (length / amount)),
                            transform.rotation * Quaternion.Euler(0, 0, centerer - 30f));
                        Instantiate(Fireball,
                            new Vector3(transform.position.x,
                                transform.position.y - length / 2 + i * (length / amount)),
                            transform.rotation * Quaternion.Euler(0, 0, centerer - 30f));
                        yield return new WaitForSeconds(0.8f);
                    }
                }
                break;
            case Pattern.Pillar:
                if (transform.position.x == 0) {
                    for (int i = 0; i < amount; i++) {
                        Instantiate(Fireball,
                            new Vector3(transform.position.x + length / 2 - i * (length / amount) - 1f,
                                transform.position.y),
                            transform.rotation * Quaternion.Euler(0, 0, -centerer + 30f));
                    }
                }
                else {
                    for (int i = 0; i < amount; i++) {
                        Instantiate(Fireball,
                            new Vector3(transform.position.x,
                                transform.position.y + length / 2 - i * (length / amount) - 0.5f),
                            transform.rotation * Quaternion.Euler(0, 0, centerer - 30f));
                    }
                }
                yield return new WaitForSeconds(attackSpread);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null);
        }

        yield return new WaitForSeconds(2f);
        attackBuffer++;
        canAttack = true;
    }

    void DoMove(bool toHittableLocation) {
        
        if (toHittableLocation) {
            randomLocation = Random.Range(1, 5);
        }
        else {
            randomLocation = Random.Range(1, 4);
        }

        switch (randomLocation) {
            case 1:
                transform.position = new Vector3(-11, 0);
                break;
            case 2:
                transform.position = new Vector3(11, 0);
                break;
            case 3:
                transform.position = new Vector3(0, 6.5f);
                break;
            case 4:
                transform.position = new Vector3(0, 2.5f);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Slash") {
            HealthBar.GetComponent<HealthBar>().SetValue(HealthBar.GetComponent<HealthBar>().slider.value - 1);
            HealthBar.transform.Find("Healthtext").GetComponent<Text>().text =
                HealthBar.GetComponent<HealthBar>().slider.value + "/" + Health;
            DoMove(false);
            moveBuffer = 0;
            attackSpread -= .05f;

            if (HealthBar.GetComponent<HealthBar>().slider.value <= 0) {
                WinScreen.SetActive(true);
            }
        }
    }
}
