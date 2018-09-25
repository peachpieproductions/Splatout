using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSphere : MonoBehaviour {

    public float minSpeed = 1;
    public float maxSpeed = 5;
    public float angularSpeed = 3;

    Rigidbody rb;
    float speed;
    Material mat;

    private void Awake() {
        mat = GetComponent<MeshRenderer>().material;
        transform.Rotate(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359));
        speed = Random.Range(minSpeed, maxSpeed) * .5f; 
        rb = GetComponent<Rigidbody>();
        StartCoroutine(ColorShift());
    }

    private void FixedUpdate() {
        
        rb.velocity = transform.forward * speed;
        if (transform.position.magnitude > 5) rb.velocity += -transform.position * .1f;
        rb.angularVelocity += new Vector3(Random.Range(-angularSpeed, angularSpeed), 
            Random.Range(-angularSpeed, angularSpeed), Random.Range(-angularSpeed, angularSpeed));
    }

    IEnumerator ColorShift() {

        while (true) {

            var col = mat.color;
            Vector3 hsv;
            Color.RGBToHSV(col, out hsv.x, out hsv.y, out hsv.z);
            hsv.x = (transform.position.x + 20) / 30;
            mat.color = Color.HSVToRGB(Mathf.Clamp01(hsv.x), .6f, 1);

            yield return new WaitForSeconds(Random.Range(.4f, .5f));

        }

    }




}
