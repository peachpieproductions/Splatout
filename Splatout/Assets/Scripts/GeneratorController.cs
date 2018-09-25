using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour {

    public GameObject spherePrefab;
    public GameObject sphereContainer;
    public int spawnAmount;
    public float spawnRadius;

    Transform sphereParent;

    private void Start() {

        sphereParent = new GameObject("Sphere Parent").transform;

        for (int i = 0; i < spawnAmount; i++) {
            var newSphereInst = Instantiate(spherePrefab, sphereParent);
            newSphereInst.transform.position = Random.insideUnitSphere * spawnRadius;
        }
    }

    private void Update() {
        
        if (Input.GetMouseButton(0)) {

            var touchWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 15);

            var spheres = Physics.SphereCastAll(touchWorldPos + Vector3.down * 2.5f, 5, Vector3.up);

            foreach(RaycastHit hit in spheres) {
                var rb = hit.transform.GetComponent<Rigidbody>();
                if (rb) rb.velocity += (touchWorldPos - hit.transform.position) * Time.deltaTime * 100;
            }

        }

    }



}
