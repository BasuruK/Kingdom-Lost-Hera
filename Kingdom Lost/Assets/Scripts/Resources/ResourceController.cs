using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {

	public GameObject HealthPotionPrefab;
	public int NumberOfHealthPotions;

    public float DistanceStart; // Start Location
    public float DistanceEnd; // End Location
	// Use this for initialization
	void Start () {
        DistanceStart = 50f;
        if (Constant.current_level == 1)
        {
            DistanceEnd = Constant.level_1_max;
        }
        else
        {
            DistanceEnd = Constant.level_2_max;
        }
        
		DistributeHealthPotions (DistanceStart, DistanceEnd);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //Instantiate Healthpotions to the distances given
	void DistributeHealthPotions(float DistanceStart, float DistanceEnd){

		for (int i = 0; i < NumberOfHealthPotions; i++) {
			Vector3 position = new Vector3 (Random.Range (DistanceStart, DistanceEnd), 3.46f, 9.15f);
			Instantiate (HealthPotionPrefab, position , Quaternion.identity);
			//Debug.Log ("Vector " + position);
		}
	}
}
