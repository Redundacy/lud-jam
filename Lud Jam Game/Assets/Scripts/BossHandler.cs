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
    public int Health = 20;
    public enum Pattern {
        Fan,
        Line,
        Wide,
        Diagonal,
        Spike,
        Pillar,
    }

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.GetComponent<HealthBar>().SetMaxValue(Health);
        HealthBar.transform.Find("Healthtext").GetComponent<Text>().text =
            HealthBar.GetComponent<HealthBar>().slider.value + "/" + Health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoAttack(Pattern pattern, int amount) {
        switch (pattern) {
            case Pattern.Fan:
                // todo instantiate in a loop to do the pattern in an amount
                break;
            case Pattern.Line:
                break;
            case Pattern.Wide:
                break;
            case Pattern.Diagonal:
                break;
            case Pattern.Spike:
                break;
            case Pattern.Pillar:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null);
        }
    }

    void DoMove(bool toHittableLocation) {
        float randomLocation;
        if (toHittableLocation) {
            randomLocation = Random.Range(1, 5);
        }
        else {
            randomLocation = Random.Range(1, 4);
        }
        // todo move to location 1-4
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Slash") {
            HealthBar.GetComponent<HealthBar>().SetValue(HealthBar.GetComponent<HealthBar>().slider.value - 1);
            HealthBar.transform.Find("Healthtext").GetComponent<Text>().text =
                HealthBar.GetComponent<HealthBar>().slider.value + "/" + Health;
        }
    }
}
