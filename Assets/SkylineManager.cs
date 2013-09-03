using UnityEngine;
using System.Collections.Generic;

public class SkylineManager : MonoBehaviour {
    public Transform prefab;
    public int numberOfObjects;
    public float recycleOffset;
    public Runner targetRunner;

    private Vector3 nextPosition;
    private    Queue<Transform> skylineQueue;
    
    // Use this for initialization
    void Start () {
        skylineQueue = new Queue<Transform>(numberOfObjects);
        nextPosition = transform.localPosition;
        
        for(int i = 0; i < numberOfObjects; i++){
            Transform o = (Transform)Instantiate(prefab);
            o.localPosition = nextPosition;
            nextPosition.x += o.localScale.x;
            skylineQueue.Enqueue(o);
        }
    }
    
    // Update is called once per frame
    void Update () {
        float xOne, xTwo;
        
        xOne = skylineQueue.Peek().localPosition.x;
        xTwo = targetRunner.distanceTraveled;
        if( xOne + recycleOffset < xTwo){
            Transform o = skylineQueue.Dequeue();
            o.localPosition = nextPosition;
            nextPosition.x += o.localScale.x;
            skylineQueue.Enqueue(o);
        }
    }    
}
