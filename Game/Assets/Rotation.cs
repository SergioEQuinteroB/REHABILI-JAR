using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class Rotation : MonoBehaviour
{
    float [] q = new float [4];

    public float [] Constquat = new float[] {0.5f,-0.5f,0.5f,0.5f};

    public SerialPort serialPort = new SerialPort("COM7",115200);
    
    void Start(){
        serialPort.Open();
    }

    /*float decodeFloat(string inString) {
        byte [] inData = new byte[4];

        if(inString.Length == 8) {
            inData = System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary.Parse(inString).Value;
        }
        int intbits = (inData[3] << 24) | ((inData[2] & 0xff) << 16) | ((inData[1] & 0xff) << 8) | (inData[0] & 0xff);
        return BitConverter.ToSingle(BitConverter.GetBytes(intbits),0);
    }*/

    float[] mul(float[] q1,float[] q2) {

        q = new float [4];

        q[0] = -q1[1] * q2[1] - q1[2] * q2[2] - q1[3] * q2[3] + q1[0] * q2[0];
        q[1] =  q1[1] * q2[0] + q1[2] * q2[3] - q1[3] * q2[2] + q1[0] * q2[1];
        q[2] = -q1[1] * q2[3] + q1[2] * q2[0] + q1[3] * q2[1] + q1[0] * q2[2];
        q[3] =  q1[1] * q2[2] - q1[2] * q2[1] + q1[3] * q2[0] + q1[0] * q2[3];
                

        return q;
    }

    void Update(){
        try{
            if (serialPort.IsOpen){
                string rawData = serialPort.ReadLine();
                string[] data = rawData.Split(',');
                if(data.Length >= 5) { // q1,q2,q3,q4,\r\n so we have 5 elements
                    q[0] = Single.Parse(data[0]);
                    q[1] = Single.Parse(data[1]);
                    q[2] = Single.Parse(data[2]);
                    q[3] = Single.Parse(data[3]);
                }
                Rot(q);
            }
        }
        catch (System.Exception){            
            throw;
        }
    }

    void Rot(float[] q){
        //q = mul(Constquat,q);
        transform.rotation = new Quaternion(q[1], q[2], -q[3], -q[0]);
    }

}
