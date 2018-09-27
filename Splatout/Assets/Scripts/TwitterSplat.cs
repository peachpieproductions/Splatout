using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitterSplat : MonoBehaviour {

    public float life = 2f;

    CanvasGroup cg;
    Vector3 targetScale;

    private void Awake() {
        targetScale = transform.localScale;
        transform.localScale *= .8f;
        cg = GetComponent<CanvasGroup>();
        transform.Rotate(0, 0, Random.Range(-15, 15));
    }

    private void Update() {

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime);

        if (life > 0) life -= Time.deltaTime;
        else {
            cg.alpha -= Time.deltaTime * .5f;
            if (cg.alpha <= 0) Destroy(gameObject);
        }
    }


}
