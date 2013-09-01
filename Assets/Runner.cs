using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {	
	public float distanceTraveled;

	// Use this for initialization
	void Start () {
		distanceTraveled = transform.localPosition.x;	
	}
	
	public Vector3 movementDirection;
	public Vector3 facingDirection;
		
	public float walkingSpeed = 5.0f;
	public float walkingSnappyness = 50f;
	public float turningSmoothing = 0.3f;
	public float runScale = 1.5f;
	
	public bool running = false;
	
	public Transform hitObject;
	public float objectDistance;
	public bool hitting = false;
	
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
		
	// Update is called once per frame	
	void Update () {
		float x = 0f;
		float y = 0f;
		float z = 0f;
		RaycastHit hitInfo;
		
		if(Input.anyKey) {
			if(Input.GetKey(KeyCode.D)) {
				x += 1.0f;
			}
			if(Input.GetKey(KeyCode.A)) {				
				x -= 1.0f;
			}
			if(Input.GetKey(KeyCode.W)) {
				y += 1.0f;
			}
			if(Input.GetKey(KeyCode.S)) {
				y -= 1.0f;
			}
			if(Input.GetKey(KeyCode.Space)) {
				Physics.Raycast (transform.position, -transform.up, out hitInfo, 1.1f);
				hitObject = hitInfo.transform;
				if(hitObject != null) {
					z += 1.0f;
				}
			}
			if(Input.GetKey(KeyCode.LeftShift)) {				
				z -= 1.0f;
			}
			if(Input.GetKey(KeyCode.Q)) {
				Physics.Raycast(transform.position, transform.forward, out hitInfo);
				hitObject = hitInfo.transform;
				objectDistance = hitInfo.distance;
			}
			if(Input.GetKey(KeyCode.E)) {
			}
		}
		movementDirection.Set(x,z,y);
		
		// Handle the movement of the character
		Vector3 targetVelocity = movementDirection * walkingSpeed;
		if(running) {
			targetVelocity *= runScale;
		}
		Vector3 deltaVelocity = targetVelocity - rigidbody.velocity;
		if (rigidbody.useGravity) {
			deltaVelocity.y = z / 4;
			
		}
		rigidbody.AddForce (deltaVelocity * walkingSnappyness, ForceMode.Acceleration);
		
		// Setup player to face facingDirection, or if that is zero, then the movementDirection
		/*Vector3 faceDir = facingDirection;
		if (faceDir == Vector3.zero)
			faceDir = movementDirection;
		
		// Make the character rotate towards the target rotation
		if (faceDir == Vector3.zero) {
			rigidbody.angularVelocity = Vector3.zero;
		}
		else {
			float rotationAngle = AngleAroundAxis (transform.forward, faceDir, Vector3.up);
			rigidbody.angularVelocity = (Vector3.up * rotationAngle * turningSmoothing);
		}*/
	}/* */
}
