using UnityEngine;
using System.IO.Ports;
using System;

public class HardwareBridge : MonoBehaviour
{
    public string portName = "COM3"; // You will change this in Unity's Inspector
    public int baudRate = 115200;

    SerialPort serialPort;

    // Variables that the HandController will read
    public float pitch, roll, yaw;
    public int thumbFlex, indexFlex, middleFlex, ringFlex, pinkyFlex;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 50; // Prevent Unity from freezing while waiting for data
        
        try
        {
            serialPort.Open();
            Debug.Log("Serial Port Opened Successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }
    }

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                if (serialPort.BytesToRead > 0)
                {
                    string data = serialPort.ReadLine();
                    ParseData(data);
                }
            }
            catch (TimeoutException) { }
        }
    }

    void ParseData(string dataString)
    {
        // We expect the Arduino to send: <AcX,AcY,AcZ,Thumb,Index,Middle,Ring,Pinky>
        if (dataString.StartsWith("<") && dataString.EndsWith(">"))
        {
            // Remove the < and > brackets
            string cleanData = dataString.Substring(1, dataString.Length - 2);
            string[] values = cleanData.Split(',');

            if (values.Length == 8)
            {
                float.TryParse(values[0], out pitch);
                float.TryParse(values[1], out roll);
                float.TryParse(values[2], out yaw);
                
                int.TryParse(values[3], out thumbFlex);
                int.TryParse(values[4], out indexFlex);
                int.TryParse(values[5], out middleFlex);
                int.TryParse(values[6], out ringFlex);
                int.TryParse(values[7], out pinkyFlex);
            }
        }
    }

    void OnApplicationQuit()
    {
        // Safely disconnect the Arduino when you close the game
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}