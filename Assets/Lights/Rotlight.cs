using UnityEngine;
using System.Collections;

public class Rotlight : MonoBehaviour {
    public float rotateRateScale = 3.0f;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        float deltaTime = Time.deltaTime;
        float rotateRate = (deltaTime % 60) * rotateRateScale;

        transform.Rotate(Vector3.up, rotateRate, Space.World);
    }
}
