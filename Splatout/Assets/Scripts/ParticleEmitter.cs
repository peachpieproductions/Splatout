using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitter : MonoBehaviour {


    public Gradient particleColorGradient;

    ParticleSystem ps;

    private void Awake() {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start() {
        StartCoroutine(SpawnParticles());
    }

    IEnumerator SpawnParticles() {

        ParticleSystem.MainModule psMain = ps.main;

        while (true) {
            psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f, 1f));
            ps.Emit(3);
            yield return new WaitForSeconds(.05f);
        }

    }

}
