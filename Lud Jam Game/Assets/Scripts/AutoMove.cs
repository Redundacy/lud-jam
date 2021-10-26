using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour {
    public float Speed = 4f;
    

    private Vector3 startingPosition;
    void Awake() {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.right * Speed * Time.deltaTime;
        transform.localScale -= new Vector3(0.001f, 0.001f, 0);
        if (Mathf.Abs(startingPosition.x - transform.position.x) > 20 || Mathf.Abs(startingPosition.y - transform.position.y) > 20) {
            Destroy(this.gameObject);
        }
    }
}
