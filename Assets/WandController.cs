using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Globalization;

public class WandController : MonoBehaviour
{
    public bool triggerButtonDown = false;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private ushort pulsePower = 1500;
    private bool start = false;
    private SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }
    private SteamVR_Controller.Device elbow_controller
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObj.index + 1);
        }
    }

    private SteamVR_TrackedObject trackedObj;

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        WriteLine("hand_x,hand_y,hand_z,hand_rot_x,hand_rot_y,hand_rot_z,elbow_x,elbow_y,elbow_z,elbow_rot_x,elbow_rot_y,elbow_rot_z");
    }

    void Update()
    {
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }
        triggerButtonDown = controller.GetPressDown(triggerButton);

        if (triggerButtonDown)
        {
            start = true;
        }
        if (start) {
            // haptic feedback to know we touched the button
            controller.TriggerHapticPulse(pulsePower);

            Vector3 rot = controller.transform.rot.eulerAngles;
            Vector3 pos = controller.transform.pos;
            Vector3 elbow_rot = elbow_controller.transform.rot.eulerAngles;
            Vector3 elbow_pos = elbow_controller.transform.pos;

            WriteLine(pos.x.ToString() + "," + pos.y.ToString() + "," + pos.z.ToString() + "," + rot.x.ToString() + "," + rot.y.ToString() + "," + rot.z.ToString() + "," + elbow_pos.x.ToString() + "," + elbow_pos.y.ToString() + "," + elbow_pos.z.ToString() + "," + elbow_rot.x.ToString() + "," + elbow_rot.y.ToString() + "," + elbow_rot.z.ToString());
        }
    }
    void WriteLine(string line)
    {
        string time = DateTime.Now.ToString("dd_MM_yyyy_HH_mm");
        string path = @"C:\Temp\" + time + ".csv";
        using (var w = new StreamWriter(path, true))
        {
            // test data format:
            // hand_x, hand_y, hand_z, hand_rot_x, hand_rot_y, hand_rot_z, elbow_x, elbow_y, elbow_z, elbow_rot_x, elbow_rot_y, elbow_rot_z
            w.WriteLine(line);
        }
    }
}