using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Messageable : MonoBehaviour {
    public HashSet<Ability> usingAbility;
    public HashSet<Ability> lastAbility;

    void Start () {
        usingAbility = new HashSet<Ability>();
        lastAbility = new HashSet<Ability>();
    }

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
        lastAbility = new HashSet<Ability>(usingAbility);
        usingAbility.Clear();
    }
}
