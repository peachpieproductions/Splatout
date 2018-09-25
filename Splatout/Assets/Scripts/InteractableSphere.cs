using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSphere : MonoBehaviour {

    [System.Serializable]
    public class LineConnection {
        public LineRenderer line;
        public InteractableSphere connectedSphere;
    }

    public LineRenderer linePrefab;
    public float minSpeed = 1;
    public float maxSpeed = 5;
    public float angularSpeed = 3;
    public List<LineConnection> lines = new List<LineConnection>();

    Rigidbody rb;
    float speed;
    Material mat;

    private void Awake() {
        mat = GetComponent<MeshRenderer>().material;
        transform.Rotate(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359));
        speed = Random.Range(minSpeed, maxSpeed) * .5f; 
        rb = GetComponent<Rigidbody>();
        //rb.velocity = transform.forward * speed;

        StartCoroutine(ColorShift());
        StartCoroutine(GenerateLines());
        StartCoroutine(UpdateLines());
    }

    private void FixedUpdate() {
        
        //Sphere Velocity
        rb.velocity += transform.forward * speed * Time.deltaTime * .25f;
        if (transform.position.magnitude > 10f) rb.velocity += -transform.position * Time.deltaTime * .025f;
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
                if (Vector3.Distance(transform.position, lines[i].connectedSphere.transform.position) > 3f) {
                    Destroy(lines[i].line.gameObject);
                    GeneratorController.runtimeInst.lines.Remove(lines[i]);
                    lines.RemoveAt(i);
                } else {
                    lines[i].line.material.color = mat.color;
                }
            }

            yield return new WaitForSeconds(.3f + Random.Range(0, .3f));

        }

    }

    IEnumerator GenerateLines() {

        yield return null;

        while (true) {

            if (lines.Count < 1 && GeneratorController.runtimeInst.lines.Count < GeneratorController.runtimeInst.linesLimit) {

                var spheres = Physics.SphereCastAll(transform.position, 2f, Vector3.up, .01f);

                foreach (RaycastHit hit in spheres) {
                    var sphere = hit.transform.GetComponent<InteractableSphere>();
                    if (sphere) {
                        if (sphere == this) continue;
                        if (sphere.lines.Count > 0 && sphere.lines[0].connectedSphere == this) continue;
                        if (Random.value < .5f) {
                            LineConnection newLine = new LineConnection();
                            newLine.connectedSphere = sphere;
                            var lineRen = Instantiate(linePrefab);
                            newLine.line = lineRen;
                            lineRen.useWorldSpace = true;
                            lineRen.material.color = mat.color;
                            lineRen.SetPosition(0, transform.position);
                            lineRen.SetPosition(1, sphere.transform.position);
                            lines.Add(newLine);
                            GeneratorController.runtimeInst.lines.Add(newLine);
                        }
                    }
                }

            }

            yield return new WaitForSeconds(Random.Range(1f,3f));

        }

    }

    IEnumerator ColorShift() {

        while (true) {

            var col = mat.color;
            Vector3 hsv;
            Color.RGBToHSV(col, out hsv.x, out hsv.y, out hsv.z);
            hsv.x = (transform.position.x + 20) / 30;
            mat.color = Color.HSVToRGB(Mathf.Clamp01(hsv.x), .6f, 1);

            var xVel = Mathf.Abs(rb.velocity.x);
            yield return new WaitForSeconds((6f - xVel) * .05f);

        }

    }




}
