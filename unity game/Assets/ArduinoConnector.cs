/* ArduinoConnector - Moves stepper motor based on angular velocity
 *                    rather than change in direction. 
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System; 

public class ArduinoConnector : MonoBehaviour
{
    //public string port = "COM4";
    public string port = "/dev/tty.usbmodem1411";
    public int baudrate = 9600;
    public float motorSpeed = 700; 

    private SerialPort stream;
    private Rigidbody rb; 
    public int deltaTime = 50;
    private int currTime = 0;
    private int currVelocity = 0;
    private int threshVelocity = 75; 


    public void Open()
    {
        stream = new SerialPort(port, baudrate);
        stream.ReadTimeout = 50;

        if (!stream.IsOpen) stream.Open();

        print("Stream opened");
    }


    public void SendData(int n)
    {
        string data = n.ToString();
        data = "MOVE " + data;

        stream.WriteLine(data);
        print("Sending Message: " + data);
        stream.BaseStream.Flush();
    }

    public float map(float x, float fromLow, float fromHigh, float toHigh, float toLow)
    {
        return (x - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow; 
       
    }

    public float GetVelocity(float velocity)
    {
        // Fix bounds
        if(Math.Abs(velocity) > motorSpeed)
        {
            if (velocity < 0) return -motorSpeed;
            else return motorSpeed; 
        }

        return map(velocity, (float)-0.75, (float)0.75, -motorSpeed, motorSpeed);
    }

    public void Close()
    {
        stream.Close();
    }

    void Start()
    {
        Open();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            stream.WriteLine("STOP");
            stream.BaseStream.Flush();
        }
        if (currTime > deltaTime)
        {
            currVelocity = (int)GetVelocity(rb.angularVelocity.y);
            if (Math.Abs(currVelocity) < threshVelocity) currVelocity = 0; 
            print("Sending Velocity: " + currVelocity); 
            SendData(currVelocity);
            currTime = 0; 
        }

        currTime++;

    }
}