using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour {

    public static GeneratorController runtimeInst;
    public GameObject spherePrefab;
    public RectTransform[] splatPrefabs;
    public Canvas mainCanvas;
    public int spawnAmount;
    public float spawnRadius;
    public List<InteractableSphere> spheres = new List<InteractableSphere>();
    public List<InteractableSphere.LineConnection> lines = new List<InteractableSphere.LineConnection>();
    public int linesLimit;
    public Gradient splatGradient;
    public GameObject fullscreenText;
    Vector3 touchWorldPos;
    Vector3 touchPos;

    float clickDownTimer;
    Transform sphereParent;

    private void Awake() {
        runtimeInst = this;
    }

    private void Start() {

        Application.targetFrameRate = 60;

        sphereParent = new GameObject("Sphere Parent").transform;

        for (int i = 0; i < spawnAmount; i++) {
            var newSphereInst = Instantiate(spherePrefab, sphereParent);
            newSphereInst.transform.position = Random.insideUnitSphere * spawnRadius;
            spheres.Add(newSphereInst.GetComponent<InteractableSphere>());
        }

    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape) && Screen.fullScreen) {

            Screen.fullScreen = !Screen.fullScreen;
            fullscreenText.SetActive(!Screen.fullScreen);

        }
        
        if (Input.GetMouseButton(0)) {

            clickDownTimer += Time.deltaTime;

            touchPos = Input.mousePosition;
            touchWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);

            var spheres = Physics.SphereCastAll(touchWorldPos, 4, Vector3.forward, 50f);

            foreach(RaycastHit hit in spheres) {
                var rb = hit.transform.GetComponent<Rigidbody>();
                if (rb) rb.velocity += (touchWorldPos - hit.transform.position) * Time.deltaTime * 50;
            }

        }

        if (Input.GetMouseButtonUp(0)) {

            if (clickDownTimer < .3f) {
                StartCoroutine(Splat());
            }
            clickDownTimer = 0;

        }

    }

    IEnumerator Splat() {

        var splatSphere = Instantiate(spherePrefab, touchWorldPos, Quaternion.identity);
        splatSphere.GetComponent<InteractableSphere>().splat = true;
        splatSphere.GetComponent<InteractableSphere>().splatLocation = touchPos;
        splatSphere.GetComponent<SphereCollider>().enabled = false;

        while (Vector3.Distance(splatSphere.transform.position,Camera.main.transform.position) > 2f) {
            yield return null;
        }

        var splat = Instantiate(splatPrefabs[Random.Range(0,splatPrefabs.Length)], mainCanvas.transform);
        splat.anchoredPosition = splatSphere.GetComponent<InteractableSphere>().splatLocation / mainCanvas.scaleFactor;
        splat.GetComponent<SVGImage>().color = splatGradient.Evaluate(Random.Range(0f, 1f));

        Destroy(splatSphere);

    }



}
