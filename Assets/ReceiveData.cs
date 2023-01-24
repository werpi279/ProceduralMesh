using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class ReceiveData : MonoBehaviour
{
    float val;
    //public static SerialPort sp = new SerialPort("COM14", 9600, Parity.None, 8, StopBits.One);
    public static SerialPort sp = new SerialPort("COM9", 9600);
    public string message2;
    float timePassed = 0.0f;
    // Use this for initialization
    void Start()
    {
        OpenConnection();
        val = float.Parse(sp.ReadLine());
    }

    public void OpenConnection()
    {
        if (sp != null)
        {
            if (sp.IsOpen)
            {
                sp.Close();
                print("Closing port, because it was already open!");
            }
            else
            {
                sp.Open();  // opens the connection
                sp.ReadTimeout = 16;  // sets the timeout value before reporting error
                print("Port Opened!");
                //		message = "Port Opened!";
            }
        }
        else
        {
            if (sp.IsOpen)
            {
                print("Port is already open");
            }
            else
            {
                print("Port == null");
            }
        }
    }

    void OnApplicationQuit()
    {
        sp.Close();
    }

    private void Update()
    {
        try
        {
            //print(sp.ReadLine());
            float newVal = float.Parse(sp.ReadLine());
            Debug.Log(newVal);
            if (newVal >= val + 1)
            {
                this.transform.position = this.transform.position + new Vector3(0, 0, 1);
                val = newVal;
            }
            else if (newVal <= val - 1)
            {
                this.transform.position = this.transform.position + new Vector3(0, 0, -1);
                val = newVal;
            }
        }
        catch (System.Exception)
        {
        }

        
    }
}
