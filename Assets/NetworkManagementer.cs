using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void GuiDelegate();
public class NetworkManagementer : MonoBehaviour {
    private GuiDelegate currentGui;
    public GUIStyle style = GUIStyle.none;
    private Rect drawRect = new Rect(Screen.width - 150, 20, 130, 20);
    public string serverIp = "127.0.0.1";

    private void checkForStart () {
        if (GUI.Button(drawRect, "Start Server")) {
            Network.InitializeServer(4, 9090, false);
            currentGui = waitForStart;
        }
    }

    private void waitForStart () {
        string ellipsish = "";
        switch (Time.frameCount / 30 % 4) {
        case 0:
            ellipsish = ".";
            break;
        case 1:
            ellipsish = "..";
            break;
        case 2:
            ellipsish = "...";
            break;
        case 3:
            ellipsish = "";
            break;
        }

        GUI.Box(drawRect, "Starting Server" + ellipsish, style);
    }


    // Use this for initialization
    void Start () {
        currentGui = checkForStart;
    }

    // Update is called once per frame
    void Update () {
    }

    void OnGUI() {
        currentGui();

        drawRect.y += 30;
        GUI.Box(drawRect, "isClient" + Network.isClient);
        drawRect.y += 30;
        GUI.Box(drawRect, "isServer" + Network.isServer);
        drawRect.y += 30;
        GUI.Box(drawRect, "isQRunning" + Network.isMessageQueueRunning);
        drawRect.y += 40;
        serverIp = GUI.TextField(drawRect, serverIp);
        drawRect.y += 30;
        if (GUI.Button(drawRect, "Connect to Server")) {
            Network.Connect(serverIp, 9090);
        }



        drawRect.y = 20;
    }
}
