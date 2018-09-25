using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour {

    TextMeshProUGUI fpsText;

    private void Awake() {
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {

        fpsText.text = (1f / Time.deltaTime).ToString("F0");

    }

}
