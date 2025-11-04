using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttemptCounter : MonoBehaviour {

    private int attemptNumber = 1;
    private TextMeshPro text;

    private void Start() {

        text = GetComponent<TextMeshPro>();
    }

    public void Add() {

        attemptNumber++;
        text.text = "Attempt: " + attemptNumber;
    }
}
