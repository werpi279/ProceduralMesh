using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
            this.transform.position = this.transform.position + new Vector3(0,  0, -1);
        if (Input.GetKey(KeyCode.D))
            this.transform.position = this.transform.position + new Vector3(0,  0,  1);
        if (Input.GetKey(KeyCode.W))
            this.transform.position = this.transform.position + new Vector3(0,  1,  0);
        if (Input.GetKey(KeyCode.S))
            this.transform.position = this.transform.position + new Vector3(0, -1,  0);
    }
}
