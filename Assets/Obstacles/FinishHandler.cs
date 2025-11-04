using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishHandler : MonoBehaviour {
    [SerializeField] private Vector2 moveTo;
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<GameObject> obstaclesToHide;
    [SerializeField] private GameObject player;
    [SerializeField] private ObstacleMove lastObstacle;

    private bool moved = false;
    private bool inMotion = false;
    private Vector3 initialPosition;

    private void Start() {

        initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision != null && !moved) {

            StartCoroutine(MoveFinish());
            player.GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezePositionX;
        }
    }

    private IEnumerator MoveFinish() {

        inMotion = true;

        while (Vector2.Distance(transform.position, moveTo) > 0.05f) {
            transform.position = Vector2.MoveTowards(transform.position, moveTo, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = moveTo;

        foreach (GameObject obstacle in obstaclesToHide) {

            Vector2 targetPos = new Vector2(obstacle.transform.position.x, obstacle.transform.position.y - 10f);
            StartCoroutine(MoveObstacleAway(obstacle, targetPos));
        }

        lastObstacle.ManualMove();

        inMotion = false;

        player.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;

        moved = true;
    }

    private IEnumerator MoveObstacleAway(GameObject obstacle, Vector2 targetPos) {

        var handlerA = obstacle.GetComponent<ObstacleMove>();
        var handlerB = obstacle.GetComponent<ObstacleMove>();

        if (handlerA != null) {

            handlerA.SetInMotion(true);
        }

        if (handlerB != null) {

            handlerB.SetInMotion(true);
        }


        while (Vector2.Distance(obstacle.transform.position, targetPos) > 0.05f) {
            obstacle.transform.position = Vector2.MoveTowards(obstacle.transform.position, targetPos, moveSpeed / 2 * Time.deltaTime);
            yield return null;
        }

        obstacle.transform.position = targetPos;

        if (handlerA != null) {

            handlerA.SetInMotion(false);
        }

        if (handlerB != null) {

            handlerB.SetInMotion(false);
        }
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
}
