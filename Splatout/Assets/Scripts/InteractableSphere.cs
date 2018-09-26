using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSphere : MonoBehaviour {

    [System.Serializable]
    public class LineConnection {
        public LineRenderer line;
        public InteractableSphere connectedSphere;
    }

    public Gradient lineRendererGradientProfile;
    public LineRenderer linePrefab;
    public float minSpeed = 1;
    public float maxSpeed = 5;
    public float angularSpeed = 3;
    public List<LineConnection> lines = new List<LineConnection>();

    Rigidbody rb;
    float speed;

    private void Awake() {

        transform.Rotate(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359));
        speed = Random.Range(minSpeed, maxSpeed) * .5f; 
        rb = GetComponent<Rigidbody>();

        StartCoroutine(GenerateLines());
        StartCoroutine(UpdateLines());

    }

    private void FixedUpdate() {

        //Sphere Velocity
        rb.velocity += transform.forward * speed * Time.deltaTime * .25f;
        var horizontalMagnitude = new Vector3(transform.position.x, 0, transform.position.z * 1.5f).magnitude;
        if (horizontalMagnitude > 10f) rb.velocity += -transform.position * Time.deltaTime * .025f;
        var verticalMagnitude = new Vector3(0, transform.position.y, transform.position.z * 1.5f).magnitude;
        if (verticalMagnitude > 6f) rb.velocity += -transform.position * Time.deltaTime * .04f;
        if (rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;
        rb.angularVelocity += new Vector3(Random.Range(-angularSpeed, angularSpeed), 
            Random.Range(-angularSpeed, angularSpeed), Random.Range(-angularSpeed, angularSpeed));

        //Update Line Positions
        foreach(LineConnection l in lines) {
            l.line.SetPosition(0, transform.position);
            l.line.SetPosition(1, l.connectedSphere.transform.position);
        }

    }

    IEnumerator UpdateLines() {

        while (true) {

            for (int i = lines.Count - 1; i >= 0; i--) {
                if (Vector3.Distance(transform.position, lines[i].connectedSphere.transform.position) > 4.5f) {
                    Destroy(lines[i].line.gameObject);
                    GeneratorController.runtimeInst.lines.Remove(lines[i]);
                    lines.RemoveAt(i);
                } else {
                    lines[i].line.startColor = lineRendererGradientProfile.Evaluate((lines[i].line.GetPosition(0).x + 15) / 30f);
                    lines[i].line.endColor = lineRendererGradientProfile.Evaluate((lines[i].line.GetPosition(1).x + 15) / 30f);
                }
            }

            yield return new WaitForSeconds(.3f + Random.Range(0, .3f));

        }

    }

    IEnumerator GenerateLines() {

        yield return null;

        while (true) {

            if (lines.Count < 3 && GeneratorController.runtimeInst.lines.Count < GeneratorController.runtimeInst.linesLimit) {
                
                var spheres = Physics.SphereCastAll(transform.position, 4f, Vector3.up, .01f);

                foreach (RaycastHit hit in spheres) {
                    var sphere = hit.transform.GetComponent<InteractableSphere>();
                    if (sphere) {
                        if (sphere == this || sphere.lines.Count > 2) continue;
                        bool foundAttached = false;
                        for (int i = 0; i < sphere.lines.Count; i++) { if (sphere.lines[i].connectedSphere == this) { foundAttached = true; break; } }
                        if (foundAttached) continue;
                        if (Random.value < .25f) {
                            LineConnection newLine = new LineConnection();
                            newLine.connectedSphere = sphere;
                            var lineRen = Instantiate(linePrefab);
                            newLine.line = lineRen;
                            lineRen.useWorldSpace = true;
                            lineRen.SetPosition(0, transform.position);
                            lineRen.SetPosition(1, sphere.transform.position);
                            lineRen.startColor = lineRendererGradientProfile.Evaluate((lineRen.GetPosition(0).x + 15) / 30f);
                            lineRen.endColor = lineRendererGradientProfile.Evaluate((lineRen.GetPosition(1).x + 15) / 30f);
                            lines.Add(newLine);
                            GeneratorController.runtimeInst.lines.Add(newLine);
                            if (lines.Count >= 3) break;
                        }
                    }
                }
                
            }

            yield return new WaitForSeconds(Random.Range(1f,3f));

        }

    }

    //Code used to generate a special gradient (one-time use) - was used in Awake function
    /*int colorSamples = 7;
    GradientColorKey[] colors = new GradientColorKey[colorSamples];
    for(int i = 0; i < colorSamples; i++) {
        colors[i].color = Color.HSVToRGB((1f / colorSamples) * i + .4f, 1, 1);
        colors[i].time = (1f / colorSamples) * i + .05f;
        Debug.Log(colors[i].time);
    }
    lineRendererGradientProfile.colorKeys = colors;*/




}
