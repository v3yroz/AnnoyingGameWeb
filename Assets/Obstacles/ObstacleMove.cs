using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour {

    [SerializeField] private Vector2 moveTo;
    [SerializeField] private float moveSpeed;

    private bool moved = false;
    private bool inMotion = false;
    private Vector3 initialPosition;

    private void Start() {

        initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision != null && !moved) {

            moved = true;
            StartCoroutine(MoveObstacle());
        }
    }

    private IEnumerator MoveObstacle() {

        inMotion = true;

        while (Vector2.Distance(transform.position, moveTo) > 0.05f) {
            transform.position = Vector2.MoveTowards(transform.position, moveTo, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = moveTo;
        inMotion = false;
    }

    public void ManualMove() {

        moved = true;
        StartCoroutine(MoveObstacle());
    }

    private IEnumerator RestartWhenStopped() {

        while (inMotion) {
            yield return null;
        }

        transform.position = initialPosition;

    }

    public void Restart() {

        moved = false;
        StartCoroutine(RestartWhenStopped());

    }

    public void SetInMotion(bool value) {

        inMotion = value;
    }
}
