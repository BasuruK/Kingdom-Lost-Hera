using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController_L1 : MonoBehaviour
{

    private GameObject mainPlayer;
    public GameObject PlayeLives;
    public GameObject cyclopPrefab;
    private float timeFrequncy = 3.0f;
    private float repeatRate = 5f;
    private float _preX = 0;
    private float playerPosition = 0;
    private int minRange = 0;
    private int maxRange = 0;
    public int spawnDistance = 50;
    int count = 0;
    int countw = 0;

    public int LivesPerLevel;
    public float LivesLocation;
    public float LivesLocationHieght;

    // Use this for initialization
    void Start()
    {

        LivesPerLevel = Random.Range(0, 4);


        //		Debug.Log ("lifes "+LivesPerLevel);
        //		for (int i=0; i < LivesPerLevel; i++) {
        //			LivesLocation = Random.Range (30,1000);
        //			LivesLocationHieght = Random.Range (10,17);
        //			GameObject _tempLifeObj = (GameObject)Instantiate (PlayeLives);
        //			_tempLifeObj.transform.position = new Vector3 (LivesLocation, LivesLocationHieght, 10f);
        //			_tempLifeObj.name = "PlayeLife";
        //		
        //		}


        //mainPlayer = GameObject.FindWithTag ("king_dutugamunu");
        mainPlayer = GameObject.Find("King Dutugamunu");
        InvokeRepeating("Spawn", timeFrequncy, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {



    }

    void Spawn()
    {
        playerPosition = mainPlayer.transform.position.x;
        minRange = (int)_preX - spawnDistance;
        maxRange = (int)_preX + spawnDistance;
        countw++;




        //	if (!(Enumerable.Range (minRange,maxRange).Contains ((int)playerPosition))) {
        if ((minRange > playerPosition) || (maxRange < playerPosition))
        {
            if (maxRange <= (Constant.level_1_max - 175f))
            {
                count++;

                GameObject _tempEnemyObj = (GameObject)Instantiate(cyclopPrefab);

                // Find a random index between zero and one less than the number of spawn points.
                //int spawnPointIndex = Random.Range (0, spawnPoints.Length);
                _tempEnemyObj.name = "EnemyCyclop";
                _preX = playerPosition;
            }
        }

        //	Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
