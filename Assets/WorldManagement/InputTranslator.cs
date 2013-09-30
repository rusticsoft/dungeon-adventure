using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TEST_MODE_OPTIONS {
    CAMERA_FPS_LOOK,
    CAMERA_FPS_MOVE,
        CAMERA_FOLLOW_LOOK,
    CAMERA_FOLLOW_MOVE
}

public class InputTranslator : MonoBehaviour {
    public Messageable target;
    public Messageable follower;
    
    public Camera primaryCamera;

    public Material playerMaterial;
    public Material followerMaterial;
    public Material neutralMaterial;
    
    public List<TEST_MODE_OPTIONS> visibleOptions;
    
    private delegate void OptionToggleFunction (Dictionary<TEST_MODE_OPTIONS, bool> optionStates);
    
    private Dictionary<TEST_MODE_OPTIONS, bool> optionToState;
    private Dictionary<TEST_MODE_OPTIONS, OptionToggleFunction> optionToToggleFunction;
    
    public GameObject createe;
    public float moveThreshold = 1.0f;
    
    void Start() {
        if(target != null) {
           applyMaterial(target, playerMaterial);
        }
        if(follower != null) {
           applyMaterial(follower, followerMaterial);
        }
        
        optionToState = new Dictionary<TEST_MODE_OPTIONS, bool>();
        optionToToggleFunction = new Dictionary<TEST_MODE_OPTIONS, OptionToggleFunction>();
        
        for (int i = 0; i < visibleOptions.Count; ++i) {
            var item = visibleOptions[i];
            optionToState[item] = false;
            optionToToggleFunction[item] = delegate (Dictionary<TEST_MODE_OPTIONS, bool> optionStates){
                optionStates[item] = !optionStates[item];
                if(optionStates[item]) {
                    switch(item) {
                    case TEST_MODE_OPTIONS.CAMERA_FPS_LOOK:
                        optionStates[TEST_MODE_OPTIONS.CAMERA_FOLLOW_LOOK] = false;
                        primaryCamera.gameObject.AddComponent<MouseLook>();
                        break;
                    case TEST_MODE_OPTIONS.CAMERA_FPS_MOVE:
                        optionStates[TEST_MODE_OPTIONS.CAMERA_FOLLOW_MOVE] = false;
                        primaryCamera.gameObject.AddComponent<FPSInputController>();
                        break;
                    case TEST_MODE_OPTIONS.CAMERA_FOLLOW_LOOK:
                        optionStates[TEST_MODE_OPTIONS.CAMERA_FPS_LOOK] = false;
                        primaryCamera.gameObject.AddComponent<SmoothLookAt>();
                        primaryCamera.GetComponent<SmoothLookAt>().target = target.transform;
                        break;
                    case TEST_MODE_OPTIONS.CAMERA_FOLLOW_MOVE:
                        optionStates[TEST_MODE_OPTIONS.CAMERA_FPS_MOVE] = false;
                        primaryCamera.gameObject.AddComponent<SmoothFollow>();
                        primaryCamera.GetComponent<SmoothFollow>().target = target.transform;
                        break;
                    }
                }
                
                if(!optionStates[TEST_MODE_OPTIONS.CAMERA_FPS_LOOK]) {
                    Object.Destroy(primaryCamera.GetComponent<MouseLook>());
                }
                if(!optionStates[TEST_MODE_OPTIONS.CAMERA_FPS_MOVE]) {
                    Object.Destroy(primaryCamera.GetComponent<FPSInputController>());
                }
                if(!optionStates[TEST_MODE_OPTIONS.CAMERA_FOLLOW_LOOK]) {
                    Object.Destroy(primaryCamera.GetComponent<SmoothLookAt>());
                }
                if(!optionStates[TEST_MODE_OPTIONS.CAMERA_FOLLOW_MOVE]) {
                    Object.Destroy(primaryCamera.GetComponent<SmoothFollow>());
                }
            };
        }
    }

    void OnGUI() {
        
        for (int i = 0; i < visibleOptions.Count; ++i) {
            var item = visibleOptions[i];
            if (GUI.Button(new Rect(10, 10 + 35 * (i + 1), 180, 30), item.ToString() + ": " + ((optionToState[item]) ? "ON" : "OFF"))) {
               optionToToggleFunction[item](optionToState);
            }
        }
    }

    private static void applyMaterial(Messageable o, Material m) {
        MeshRenderer mrMesh = o.GetComponent<MeshRenderer>();
        mrMesh.material = m;
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

    void moveMessageableTowards (Messageable target, Vector3 destination)
    {
        if(target.transform.position.x - destination.x > moveThreshold) {
           target.receiveMessage(new WorldMessage(WorldMessage.MessageType.USE_ABILITY, Ability.MOVE_LEFT));
        }
        if(target.transform.position.x - destination.x < -moveThreshold) {
           target.receiveMessage(new WorldMessage(WorldMessage.MessageType.USE_ABILITY, Ability.MOVE_RIGHT));
        }
        if(target.transform.position.z - destination.z > moveThreshold) {
           target.receiveMessage(new WorldMessage(WorldMessage.MessageType.USE_ABILITY, Ability.MOVE_DOWN));
        }
        if(target.transform.position.z - destination.z < -moveThreshold) {
         target.receiveMessage(new WorldMessage(WorldMessage.MessageType.USE_ABILITY, Ability.MOVE_UP));
        }
    }

    void Update() {
        if(Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift)) {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hitData;
           if (Physics.Raycast(ray, out hitData)) {
               Messageable newR = hitData.transform.GetComponent<Messageable>();
               if (newR != null && newR != target) {
                  applyMaterial(target, neutralMaterial);
                  if(follower != null) applyMaterial(follower, followerMaterial);
                  applyMaterial(newR, playerMaterial);
                  target = newR;
               }
           }
        }
        if(Input.GetMouseButton(1)) {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hitData;
           if (Physics.Raycast(ray, out hitData)) {
               moveMessageableTowards (target, hitData.point);
           }
        }
        if(Input.GetMouseButtonDown(2)) {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hitData;
           if (Physics.Raycast(ray, out hitData)) {
               Instantiate(createe, hitData.point + new Vector3(0, createe.transform.lossyScale.y, 0), hitData.transform.rotation);
           }
        }
        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift)) {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hitData;
           if (Physics.Raycast(ray, out hitData)) {
               Messageable newF = hitData.transform.GetComponent<Messageable>();
               if(follower != null) applyMaterial(follower, neutralMaterial);
               if (newF != null) {
                  applyMaterial(newF, followerMaterial);
                  applyMaterial(target, playerMaterial);
               }
               follower = newF;
           }
        }
        if(follower != null) {
           updateFollower(follower, target);
        }
        if(Input.anyKey) {
           if(Input.GetKey(KeyCode.Escape)) {
               Rect r = new Rect(Screen.height / 2 - 100, Screen.width / 2 - 100, Screen.height / 2 + 100, Screen.width / 2 + 100);
               //GUI.Box (r, "treat me better");
           }
           foreach (var key in messageMap.Keys) {
               if (Input.GetKey(key)) {
                  target.receiveMessage(messageMap[key]);
               }
           }
        }
    }

    void updateFollower(Messageable follower, Messageable target) {
        if (follower == target) {
           return;
        }
        moveMessageableTowards(follower, target.transform.position);
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