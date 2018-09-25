using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour {

    public GameObject spherePrefab;
    public GameObject invertedSphere;
    public int spawnAmount;

    float invertedSphereRadius;
    Transform sphereParent;

    private void Start() {

        invertedSphereRadius = invertedSphere.GetComponent<SphereCollider>().radius * invertedSphere.transform.localScale.x;

        sphereParent = new GameObject("Sphere Parent").transform;

        for (int i = 0; i < spawnAmount; i++) {
            var newSphereInst = Instantiate(spherePrefab, sphereParent);
            newSphereInst.transform.position = Random.insideUnitSphere * invertedSphereRadius * .5f;
        }
    }



}
