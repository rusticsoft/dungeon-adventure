using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MOVEMENT_TYPE
{
    LEGGED,
    SLITHER,
    ROLL,
    FLAP,
    GLIDE,
    ABSOLUTE, // Should things like the camera that aren't even technically in the world have some other movement type
    TRACKING  // than ABSOLUTE or TRACKING? Perhaps there are multiple concepts (movement, tangibility, etc) that we need to separate out
}

[RequireComponent (typeof (Messageable))]
public class Movable : MonoBehaviour {
    public Vector3 movementDirection;
    public Vector3 facingDirection;

    public float walkingSpeed = 5.0f;
    public float walkingSnappyness = 5f;
    public float runScale = 1.5f;

    public bool running = false;

    public Transform hitObject;
    public float objectDistance;

    public bool solidBeneath;
    private RaycastHit lastGroundHit;
    public float jumpPower = 50.0f;

    public Messageable messageSource;

    public delegate Vector3 MovementFunction (Vector3 v);
    public Dictionary<Ability, MovementFunction>  abilityToMovementMap;

    void Start () {
        abilityToMovementMap = new Dictionary<Ability, MovementFunction>();
        abilityToMovementMap.Add(Ability.MOVE_RIGHT, delegate(Vector3 v) {v.x += 1.0f; return v;});
        abilityToMovementMap.Add(Ability.MOVE_LEFT, delegate(Vector3 v) {v.x -= 1.0f; return v;});
        abilityToMovementMap.Add(Ability.MOVE_UP, delegate(Vector3 v) {v.z += 1.0f; return v;});
        abilityToMovementMap.Add(Ability.MOVE_DOWN, delegate(Vector3 v) {v.z -= 1.0f; return v;});
        abilityToMovementMap.Add(Ability.JUMP, delegate(Vector3 v) { v.y += 1.0f; return v; });
        abilityToMovementMap.Add(Ability.FAST_FALL, delegate(Vector3 v) {v.y -= 1.0f; return v; });
    }

    void Update () {
        movementDirection.Set(0f,0f,0f);
        solidBeneath = (Physics.Raycast (transform.position, -transform.up, out lastGroundHit, 1.1f));

        /*if(tryGetTargetInFront) {
            RaycastHit hitInfo;
            Physics.Raycast(transform.position, transform.forward, out hitInfo);
            hitObject = hitInfo.transform;
            objectDistance = hitInfo.distance;
        }*/

        foreach(Ability a in abilityToMovementMap.Keys) {
            if(messageSource == null) {
                Debug.Log("Null source for movable {" + this.gameObject.ToString() + "}");
                return;
            }

            if(messageSource.lastAbility == null) {
                Debug.Log("Null abilityUseList for movable {" + this.gameObject.ToString() + "}");
                return;
            }

            if(messageSource.lastAbility.Contains(a)) {
                movementDirection = abilityToMovementMap[a](movementDirection);
            }
        }


        Vector3 desiredVelocity = new Vector3(movementDirection.x, 0, movementDirection.z);
        
        desiredVelocity.Normalize();
        desiredVelocity *=  walkingSpeed * (running ? runScale : 1);
        desiredVelocity.y = (solidBeneath) ? movementDirection.y * jumpPower : rigidbody.velocity.y;

        Vector3 deltaVelocity = desiredVelocity - rigidbody.velocity;

    if(deltaVelocity.y > 0.01f || deltaVelocity.y < -0.01f) {
        Debug.Log(deltaVelocity);
    }
        deltaVelocity.x *= walkingSnappyness;
        deltaVelocity.z *= walkingSnappyness;
        rigidbody.AddForce (deltaVelocity, ForceMode.Acceleration);

	}
}
