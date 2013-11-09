using UnityEngine;
using System.Collections;

public class PhysicalProperties : MonoBehaviour {
	
	public float weight;
	public Vector3 forceOn;
	private float speed = 0.05f;
	public bool grounded;

	void Start () {
	
	}
	
	void Update () {
		grounded = transform.position.y <= 0.0f;
		
		Vector3 newPosition = transform.position;
		
		if (forceOn.y > weight) {
	    	newPosition.y = newPosition.y + speed;
		} else if (!grounded) {
			newPosition.y = newPosition.y - speed;
		}
				
		transform.position = newPosition;
	}
}
