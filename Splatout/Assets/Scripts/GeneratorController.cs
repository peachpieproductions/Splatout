using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour {

    public static GeneratorController runtimeInst;
    public GameObject spherePrefab;
    public GameObject sphereContainer;
    public int spawnAmount;
    public float spawnRadius;
    public List<InteractableSphere.LineConnection> lines = new List<InteractableSphere.LineConnection>();
    public int linesLimit;

    Transform sphereParent;

    private void Awake() {
        runtimeInst = this;
    }

    private void Start() {

        Application.targetFrameRate = 300;

        sphereParent = new GameObject("Sphere Parent").transform;

        for (int i = 0; i < spawnAmount; i++) {
            var newSphereInst = Instantiate(spherePrefab, sphereParent);
            newSphereInst.transform.position = Random.insideUnitSphere * spawnRadius;
        }

    }

    private void Update() {
        
        if (Input.GetMouseButton(0)) {

            var touchWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);

            var spheres = Physics.SphereCastAll(touchWorldPos, 4, Vector3.forward, 50f);

            foreach(RaycastHit hit in spheres) {
                var rb = hit.transform.GetComponent<Rigidbody>();
                if (rb) rb.velocity += (touchWorldPos - hit.transform.position) * Time.deltaTime * 50;
            }

        }

    }



}
