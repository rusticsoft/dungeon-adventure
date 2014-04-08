using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class GuiTestThing : MonoBehaviour {
    private delegate void guiFunction ();

    private static guiFunction newGuiFunc = delegate() {
                   Rect r = new Rect(Screen.height / 2 - 100, Screen.width / 2 - 100, Screen.height / 2 + 100, Screen.width / 2 + 100);
                   GUI.Box (r, "treat me better");
                };

    private static List<guiFunction> allGuis = new List<guiFunction>();

    void Start ()
    {
        allGuis.Add(
            delegate() {
                if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button")) {
                    print("You clicked the button!");
                }
            }
        );
    }

    void Update ()
    {
        if(Input.GetKey(KeyCode.Escape)) {
            if(allGuis.Contains(newGuiFunc)) {
                allGuis.Remove(newGuiFunc);
            } else {
                allGuis.Add(newGuiFunc);
            }

        }
    }

    void OnGUI ()
    {
        foreach (guiFunction gf in allGuis) {
            gf();
        }
    }
}
