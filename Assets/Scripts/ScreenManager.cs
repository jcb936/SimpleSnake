using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private int[] rHeights = {600, 768, 960};
    private int[] rWidths = {800, 1024, 1280};
    private int resolutionSwitch;
    
    private void Awake() {
        Screen.SetResolution(1024, 768, false);
        resolutionSwitch = 1;
        DontDestroyOnLoad(this);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            resolutionSwitch++;
            resolutionSwitch %= 3;
            Screen.SetResolution(rWidths[resolutionSwitch], rHeights[resolutionSwitch], false);
        }
    }
}
