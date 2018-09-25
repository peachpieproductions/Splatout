using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSphere : MonoBehaviour {

    Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        //rb.vel
    }




}
