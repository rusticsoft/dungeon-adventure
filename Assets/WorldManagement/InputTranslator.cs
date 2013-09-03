using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InputTranslator : MonoBehaviour {
    public Runner target;
    public GameObject createe;
    
    void Start() {
    
    }
    
    public void ReceiveMessage(WorldMessage message) {
        
    }
    
    public void TranslateMessage(WorldMessage message) {
    
    }
    
    public static readonly Dictionary<KeyCode, WorldMessage> messageMap = new Dictionary<KeyCode, WorldMessage>();
        
    static InputTranslator() {
        WorldMessage.MessageType messType = WorldMessage.MessageType.USE_ABILITY;
        messageMap.Add(KeyCode.D, new WorldMessage(messType, Ability.MOVE_RIGHT));
        messageMap.Add(KeyCode.A, new WorldMessage(messType, Ability.MOVE_LEFT));
        messageMap.Add(KeyCode.W, new WorldMessage(messType, Ability.MOVE_UP));
        messageMap.Add(KeyCode.S, new WorldMessage(messType, Ability.MOVE_DOWN));
        messageMap.Add(KeyCode.Space, new WorldMessage(messType, Ability.JUMP));
        messageMap.Add(KeyCode.LeftShift, new WorldMessage(messType, Ability.FAST_FALL));
        messageMap.Add(KeyCode.Q, new WorldMessage(messType, null));
        messageMap.Add(KeyCode.E, new WorldMessage(messType, null));
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData)) {
                //Instantiate(particle, transform.position, transform.rotation) as GameObject;
                Runner newR = hitData.transform.GetComponent<Runner>();
                target = newR == null ? target : newR;
            }
        }
        if(Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData)) {
                if(target.transform.position.x - hitData.point.x > 1) {
                    target.receiveMessage(new WorldMessage(WorldMessage.MessageType.USE_ABILITY, Ability.MOVE_RIGHT));
                }
                if(target.transform.position.x - hitData.point.x < -1) {
                    target.receiveMessage(new WorldMessage(WorldMessage.MessageType.USE_ABILITY, Ability.MOVE_LEFT));
                }
                //Instantiate(createe, hitData.point, Quaternion.identity);
                //Runner newR = hitData.transform.GetComponent<Runner>();
                //target = newR == null ? target : newR;
            }
        }
        if(Input.GetMouseButtonDown(2)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData)) {
                Instantiate(createe, hitData.point + new Vector3(0, createe.transform.lossyScale.y, 0), hitData.transform.rotation);
                //Runner newR = hitData.transform.GetComponent<Runner>();
                //target = newR == null ? target : newR;
            }
        }
        if(Input.anyKey) {
            foreach (var key in messageMap.Keys) {
                if (Input.GetKey(key)) {
                    //target.SendMessage("whaaaaaa?!?!?!?!");
                    target.receiveMessage(messageMap[key]);
                }
            }
        }
    }
}

public class WorldMessage {
    public enum MessageType {
        USE_ABILITY,
        HANG_OUT_WITH_DUDE
    }
    public WorldMessage (MessageType type, object content)
    {
        this.type = type;
        this.content = content;
    }
    
    public MessageType type;
    public object content;
}