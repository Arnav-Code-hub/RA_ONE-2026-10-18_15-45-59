# RA_ONE-2026-10-18_15-45-59

Real-Time 3-DOF Wrist and 5-Finger Tracking Bridge
This repository contains the firmware and software required to build a real-time, low-latency bridge between custom physical hand-tracking hardware and a Unity 3D simulation. It captures 3 Degrees of Freedom (Pitch, Roll, Yaw) for wrist orientation using an IMU, alongside independent flex data for five fingers, and maps them to a rigged 3D hand model.

⚙️ System Architecture
The architecture is divided into a Hardware/Firmware Layer (Arduino Nano C++) and a Kinematic/Simulation Layer (Unity C#).

The Arduino Nano was explicitly chosen over the Uno to utilize the hardware-level A6 and A7 analog pins, allowing all five flex sensors to be read simultaneously without requiring an external multiplexer, while leaving A4 and A5 free for the I2C bus.

✨ Features
Real-Time Data Streaming: Serial communication running at 115200 baud to ensure zero-lag synchronization between physical movement and the 3D simulation.

3-DOF Wrist Tracking: Utilizes the MPU6050 gyroscope and accelerometer to calculate wrist pitch, roll, and yaw.

Independent Finger Tracking: 5 independent voltage dividers process analog resistance from flex sensors to track individual finger curl (Thumb, Index, Middle, Ring, Pinky).

Lightweight Unity Bridge: A C# serial parser that runs without blocking the main Unity thread, converting raw analog/I2C telemetry into normalized Quaternion joint rotations.

🛠️ Bill of Materials (BOM)
1x Arduino Nano V3.0 (ATmega328P) - Do not substitute with Uno

1x MPU6050 6-Axis IMU (Soldered headers required)

5x 2.2" Flex Sensors

5x 10kΩ Resistors (For pull-down voltage dividers)

Jumper Wires & Breadboard

🔌 Hardware Wiring Guide
MPU6050 (I2C): VCC -> 5V | GND -> GND | SDA -> A4 | SCL -> A5

Flex Sensors (Voltage Dividers):

Flex Pin 1 -> 5V

Flex Pin 2 -> A0, A1, A2, A3, A6

10kΩ Resistor -> Bridges Flex Pin 2 and GND.

📡 Data Protocol
The Arduino firmware transmits a continuous stream of comma-separated values formatted inside angle brackets to ensure packet integrity.
Format: <AcX,AcY,AcZ,Thumb,Index,Middle,Ring,Pinky>
Example Payload: <14500,-2300,16000,227,100,311,227,323>

🚀 Getting Started
Hardware Assembly: Wire the components according to the BOM. Ensure the MPU6050 pins are physically soldered to the board to maintain I2C signal integrity.

Flash Firmware: Open HardwareBridge.ino in the Arduino IDE. Select Arduino Nano and ATmega328P (Old Bootloader). Upload to the board.

Verify Data: Open the Serial Monitor at 115200 baud. Verify the <...> packet stream is actively running. Close the Serial Monitor.

Unity Setup:

Open your Unity Project.

Go to Edit > Project Settings > Player > Other Settings and set Api Compatibility Level to .NET Framework.

Attach HardwareBridge.cs to an empty SerialController GameObject. Update the COM port.

Attach HandController.cs to a HandController GameObject. Map your 3D Hand model's wrist and finger root joints in the Inspector.

Run: Press Play in Unity. Calibrate the flatHandAnalog and bentHandAnalog variables in the Inspector for your specific resistors.
