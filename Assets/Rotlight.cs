using UnityEngine;
using System.Collections;

public enum ROTATION_VECTOR {
    OBJECT_UP,
    WORLD_UP
}

public enum ROTATION_TYPE {
    DEFAULT,
    AROUND,
    AROUND_LOCAL
}
public class Rotlight : MonoBehaviour {
    public float rotateRateScale = 3.0f;
    public ROTATION_TYPE rotType = ROTATION_TYPE.DEFAULT;
    public ROTATION_VECTOR rotVector = ROTATION_VECTOR.WORLD_UP;
    
    // Use this for initialization
    void Start () {
    
    }
    
    delegate void rotate(Vector3 v, float f);
    
    // Update is called once per frame
    void Update () {
        //DateTime time = DateTime.Now;
        float deltaTime = Time.deltaTime;
        float rotateRate = deltaTime * rotateRateScale;
        Vector3 rotateVector = transform.up;
        rotate rotateFunction = transform.Rotate;
        
        switch (rotVector) {
        case ROTATION_VECTOR.WORLD_UP:
            rotateVector = UnityEngine.Vector3.up;
            break;
        case ROTATION_VECTOR.OBJECT_UP:
            rotateVector = transform.up;
            break;
        default:
        break;
        }
        
        switch(rotType) {
            case ROTATION_TYPE.AROUND:
            rotateFunction = transform.RotateAround;
            break;
            case ROTATION_TYPE.AROUND_LOCAL:
            rotateFunction = transform.RotateAroundLocal;
            break;
            case ROTATION_TYPE.DEFAULT:
            rotateFunction = transform.Rotate;
            break;
        }
        //transform.Rotate(-transform.up, rotateRate);
        rotateFunction(rotateVector, rotateRate);
        //transform.RotateAroundLocal(transform.up, rotateRate);
        //UnityEngine.Debug.developerConsoleVisible = true;
        //UnityEngine.Debug.Log(transform.);
        //transform.RotateAround
        //transform.localRotation.y += rotateRate;
        //seconds.localRotation = Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees);
    }
}
