using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour {
	public Text PointsTextInGUI;
	private static int TotalPoints;
	public int WalkerKillPoints;
	public int MainEnemyKillPoints;

	// Use this for initialization
	void Start () {
		PointsTextInGUI = gameObject.GetComponentInChildren<Text> ();
		TotalPoints = 0;
	}
		
	void Update()
	{
		
	}
	// Run When Walker is dead
	void WalkerKill()
	{
		TotalPoints += WalkerKillPoints;
		PointsTextInGUI.text = TotalPoints.ToString ();

	}

	//Run When Main Enemy is Dead
	void MainEnemyKill()
	{
		TotalPoints += MainEnemyKillPoints;
		PointsTextInGUI.text = TotalPoints.ToString ();
	}
}
