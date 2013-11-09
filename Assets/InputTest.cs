using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour {
	public GameObject portcullis;
	public float speed;
	
	// Use this for initialization
	void Start () {
		print(portcullis.transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPosition = portcullis.transform.position;
		
		if (Input.GetKey(KeyCode.E))
		{	
			newPosition.y = newPosition.y + speed;
		}
		else if (newPosition.y > 0)
		{
			newPosition.y = newPosition.y - speed;
		}
		
		portcullis.transform.position = newPosition;
	}
}
