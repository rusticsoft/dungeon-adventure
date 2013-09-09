using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InputTranslator : MonoBehaviour {
    public Messageable target;
    public Messageable follower;

    public Material playerMaterial;
    public Material followerMaterial;
    public Material neutralMaterial;

    public GameObject createe;
    public float moveThreshold = 1.0f;

    void Start() {
        applyMaterial(target, playerMaterial);
        applyMaterial(follower, followerMaterial);

        SandboxThing.Start();
    }

    void OnGUI() {
        SandboxThing.OnGUI();
    }

    private static void applyMaterial(Messageable o, Material m) {
        if(o == null) return;

        MeshRenderer mrMesh = o.GetComponent<MeshRenderer>();
        mrMesh.material = m;
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

    void moveCharacterTowards (Messageable target, Vector3 destination)
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
                moveCharacterTowards (target, hitData.point);
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

                applyMaterial(follower, neutralMaterial);
                applyMaterial(newF, followerMaterial);
                applyMaterial(target, playerMaterial);

                follower = newF;
            }
        }
        if(follower != null) {
            updateFollower(follower, target);
        }
        if(Input.anyKey) {
            if(target != null) {
                dispatchMessages(target, messageMap);
            }
        }

        SandboxThing.Update();
    }

    public void dispatchMessages (Messageable target, Dictionary<KeyCode, WorldMessage> messageMap)
    {
        foreach (var key in messageMap.Keys) {
            if (Input.GetKey(key)) {
                target.receiveMessage(messageMap[key]);
            }
        }
    }

    void updateFollower(Messageable follower, Messageable target) {
        if (follower == target) {
            return;
        }
        moveCharacterTowards(follower, target.transform.position);
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

public class SandboxThing {

    private delegate void guiFunction ();

    private static guiFunction newGuiFunc = delegate() {
                   Rect r = new Rect(Screen.height / 2 - 100, Screen.width / 2 - 100, Screen.height / 2 + 100, Screen.width / 2 + 100);
                   GUI.Box (r, "treat me better");
                };

    private static List<guiFunction> allGuis = new List<guiFunction>();

    public static void Start ()
    {
        allGuis.Add(
            delegate() {
                if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button")) {
                    //print("You clicked the button!");
                }
            }
        );
    }

    public static void Update ()
    {
        if(Input.GetKey(KeyCode.Escape)) {
            if(allGuis.Contains(newGuiFunc)) {
                allGuis.Remove(newGuiFunc);
            } else {
                allGuis.Add(newGuiFunc);
            }

        }
    }

    public static void OnGUI ()
    {
        foreach (guiFunction gf in allGuis) {
            gf();
        }
    }
}
