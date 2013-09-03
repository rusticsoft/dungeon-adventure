using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Runner : MonoBehaviour {
    public float distanceTraveled;

    // Use this for initialization
    void Start () {
        distanceTraveled = transform.localPosition.x;    
    }
    
    public Vector3 movementDirection;
    public Vector3 facingDirection;
        
    public float walkingSpeed = 5.0f;
    public float walkingSnappyness = 5f;
    public float turningSmoothing = 0.3f;
    public float runScale = 1.5f;
    
    public bool running = false;
    
    public Transform hitObject;
    public float objectDistance;
    public bool hitting = false;
    private HashSet<Ability> usingAbility = new HashSet<Ability>();
    
    // The angle between dirA and dirB around axis
    static float AngleAroundAxis (Vector3 dirA, Vector3 dirB, Vector3 axis) {
        // Project A and B onto the plane orthogonal target axis
        dirA = dirA - Vector3.Project (dirA, axis);
        dirB = dirB - Vector3.Project (dirB, axis);
       
        // Find (positive) angle between A and B
        float angle = Vector3.Angle (dirA, dirB);
       
        // Return angle multiplied with 1 or -1
        return angle * (Vector3.Dot (axis, Vector3.Cross (dirA, dirB)) < 0 ? -1 : 1);
    }
    
    
    //Probably make a WorldMessage class at some point
    public void receiveMessage(WorldMessage message) {
        switch (message.type) {
        case WorldMessage.MessageType.USE_ABILITY:
            usingAbility.Add((Ability) message.content);
            break;
        default:
            break;
        }
    }
    
    // Update is called once per frame    
    void Update () {
        float x = 0f;
        float y = 0f;
        float z = 0f;
        RaycastHit hitInfo;
        HashSet<Ability> lastAbility = new HashSet<Ability>(usingAbility);
        usingAbility.Clear();
        
        //foreach(Ability a in Ability) {
        //    if(lastAbility.Contains(a]) {
        if(lastAbility.Contains(Ability.MOVE_RIGHT)) {
            x += 1.0f;
        }
        if(lastAbility.Contains(Ability.MOVE_LEFT)) {        
            x -= 1.0f;
        }
        if(lastAbility.Contains(Ability.MOVE_UP)) {
            z += 1.0f;
        }
        if(lastAbility.Contains(Ability.MOVE_DOWN)) {
            z -= 1.0f;
        }
        if(lastAbility.Contains(Ability.JUMP)) {
            //Old Way
            //Physics.Raycast (transform.position, -transform.up, out hitInfo, 1.1f);
            //hitObject = hitInfo.transform;

            //New Way
            if(Physics.Raycast (transform.position, -transform.up, out hitInfo, 1.1f)) {
                hitObject = hitInfo.transform;
                y += 2.0f;
            }
        }
        if(lastAbility.Contains(Ability.FAST_FALL)) {
            y -= 1.0f;
        }
        if(lastAbility.Contains(Ability.FAST_FALL)) {
            Physics.Raycast(transform.position, transform.forward, out hitInfo);
            hitObject = hitInfo.transform;
            objectDistance = hitInfo.distance;
        }
        if(lastAbility.Contains(Ability.FAST_FALL)) {
        }
        
        movementDirection.Set(x,y,z);
        
        // Handle the movement of the character
        Vector3 targetVelocity = movementDirection * walkingSpeed;
        if(running) {
            targetVelocity *= runScale;
        }
        Vector3 deltaVelocity = targetVelocity - rigidbody.velocity;
        deltaVelocity.x *= walkingSnappyness;
        deltaVelocity.z *= walkingSnappyness;
        rigidbody.AddForce (deltaVelocity, ForceMode.Acceleration);
        //rigidbody.AddForce (deltaVelocity , ForceMode.Acceleration);
    }
}
