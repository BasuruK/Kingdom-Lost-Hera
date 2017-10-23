using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.name == "King Dutugamunu")
		{
			//Debug.Log("Bullet hit on the Enemy");
			//Destroy(gameObject);
			Destroy (gameObject);

			Debug.Log ("LIfe");
		}
	}
}
