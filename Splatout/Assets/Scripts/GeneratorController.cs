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



}
