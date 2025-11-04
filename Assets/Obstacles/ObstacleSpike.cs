using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpike : MonoBehaviour {

    [SerializeField] private Vector2 spikesMoveTo;
    [SerializeField] private float spikesSpeed;
    [SerializeField] private GameObject spikes;

    private bool spikesElevated = false;
    private bool inMotion = false;
    private Vector3 spikesInitialPosition;

    private void Start() {

        spikesInitialPosition = spikes.transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision != null && !spikesElevated) {

            spikesElevated = true;
            StartCoroutine(ElevateSpikes());
        }
    }

    private IEnumerator ElevateSpikes() {

        inMotion = true;

        while (Vector2.Distance(spikes.transform.localPosition, spikesMoveTo) > 0.02f) {
            spikes.transform.localPosition = Vector2.MoveTowards(spikes.transform.localPosition, spikesMoveTo, spikesSpeed * Time.deltaTime);
            yield return null;
        }

        spikes.transform.localPosition = spikesMoveTo;
        inMotion = false;
    }

    private IEnumerator RestartWhenStopped() {

        while (inMotion) {
            yield return null;
        }

        spikes.transform.localPosition = spikesInitialPosition;

    }

    public void Restart() {

        spikesElevated = false;
        StartCoroutine(RestartWhenStopped());

    }

    public void SetInMotion(bool value) {

        inMotion = value;
    }
}
