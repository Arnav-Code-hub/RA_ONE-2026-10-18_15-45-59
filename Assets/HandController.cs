using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Hardware Connection")]
    public HardwareBridge dataBridge; 

    [Header("Wrist Transform")]
    public Transform wrist;

    [Header("Finger Root Transforms")]
    public Transform thumbRoot;
    public Transform indexRoot;
    public Transform middleRoot;
    public Transform ringRoot;
    public Transform pinkyRoot;

    [Header("Flex Sensor Calibration")]
    public float flatHandAnalog = 227f; // Your baseline number when fingers are straight
    public float bentHandAnalog = 600f; // The raw number when fully bent
    
    // Unity rotation limits
    private float fingerStraightAngle = 0f;
    private float fingerBentAngle = 90f;

    void Update()
    {
        if (dataBridge == null) return;

        // 1. Process MPU6050 (Wrist)
        // Convert the raw X/Y/Z accelerometer vectors into Euler angles
        float calculatedPitch = Mathf.Atan2(dataBridge.pitch, dataBridge.yaw) * Mathf.Rad2Deg;
        float calculatedRoll = Mathf.Atan2(dataBridge.roll, dataBridge.yaw) * Mathf.Rad2Deg;
        
        if (wrist != null)
        {
            wrist.localRotation = Quaternion.Euler(calculatedPitch, 0, calculatedRoll); 
        }

        // 2. Process Flex Sensors (Fingers)
        ApplyFingerBend(thumbRoot, dataBridge.thumbFlex);
        ApplyFingerBend(indexRoot, dataBridge.indexFlex);
        ApplyFingerBend(middleRoot, dataBridge.middleFlex);
        ApplyFingerBend(ringRoot, dataBridge.ringFlex);
        ApplyFingerBend(pinkyRoot, dataBridge.pinkyFlex);
    }

    void ApplyFingerBend(Transform joint, int rawAnalogValue)
    {
        if (joint == null) return;

        // Convert the raw Arduino number (e.g., 227-600) into a percentage (0.0 to 1.0)
        float normalizedFlex = Mathf.InverseLerp(flatHandAnalog, bentHandAnalog, rawAnalogValue);
        
        // Convert that percentage into a Unity angle (0 to 90 degrees)
        float targetAngle = Mathf.Lerp(fingerStraightAngle, fingerBentAngle, normalizedFlex);

        // Apply the rotation. Depending on the 3D model, it might curl on the X, Y, or Z axis.
        joint.localRotation = Quaternion.Euler(targetAngle, 0, 0); 
    }
}