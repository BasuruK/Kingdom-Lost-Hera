using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBodies : MonoBehaviour {

	private Renderer[] m_WaterBodyList;
	private float m_move_x;
	// Use this for initialization
	void Start () {
		m_WaterBodyList = gameObject.GetComponentsInChildren<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Renderer rend in m_WaterBodyList) {
			m_move_x += 0.1f * Time.deltaTime;
			rend.material.SetTextureOffset ("_MainTex", new Vector2(m_move_x, 0));
		}
	}
}
