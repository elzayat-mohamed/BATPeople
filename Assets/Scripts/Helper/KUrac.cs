using UnityEngine;
using System.Collections;

public class KUrac : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

	float step = 2;
	void Update () 
	{
		this.transform.position = new Vector3 (this.transform.position.x + step, this.transform.position.y + step, this.transform.position.z + step);
		step *= -1; 
	}
}
