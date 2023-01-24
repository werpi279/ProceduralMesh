using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Linq;
using System.Threading;

public class ReceiveDataClimbing : MonoBehaviour
{
    SerialPort stream;

    float val;
    

    private Queue queue;

    
    void Start()
    {
        stream = new SerialPort("COM9", 9600);
        stream.DtrEnable = false;   //Prevent the Arduino from rebooting once we connect to it. 
                                    //A 10 uF cap across RST and GND will prevent this. Remove cap when programming.
        stream.ReadTimeout = 1;     //Shortest possible read time out.
        stream.WriteTimeout = 1;    //Shortest possible write time out.
        if (!stream.IsOpen)
            stream.Open();

        queue = Queue.Synchronized(new Queue());

        val = float.Parse(stream.ReadLine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && stream.IsOpen)
            stream.Close();

        CheckForRecievedData();

        string[] value = ReceivedData();

        float newVal = float.Parse(stream.ReadLine());
        Debug.Log(newVal);
        if (newVal<-1)
        {
            this.transform.position = this.transform.position + new Vector3(0, 0, 1);
            //val = newVal;
        }
        /*else if (newVal<-1)
        {
            this.transform.position = this.transform.position + new Vector3(0, 0, -1);
            val = newVal;
        }
        else if (newVal >=-1 || newVal<=1)
        {
            return;
        }*/
    }

    public void CheckForRecievedData()
    {
        try //Sometimes malformed serial commands come through. We can ignore these with a try/catch.
        {
            string inData = stream.ReadTo(";");

            if (inData != null)
                queue.Enqueue(inData);


            //int inSize = inData.Count();
            //if (inSize > 0)
            //{
            //    Debug.Log("ARDUINO->|| " + inData + " ||MSG SIZE:" + inSize.ToString());
            //}
            ////Got the data. Flush the in-buffer to speed reads up.
            //inSize = 0;
            //stream.BaseStream.Flush();
            //stream.DiscardInBuffer();


        }
        catch {; }
    }

    public string[] ReceivedData()
    {
        if (queue.Count == 0)
            return null;

        //else ovvio
        char splitChar = ',';
        string[] dataRaw = ((string)queue.Dequeue()).Split(splitChar);
        return dataRaw;
    }
}