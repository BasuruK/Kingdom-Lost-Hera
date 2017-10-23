using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerL2 : MonoBehaviour
{

    public GameObject walker_l2_prefab;
    private GameObject mainPlayer;


    private float timeFrequncy = 3.0f;
    private float repeatRate = 5f;
    private float _preX = 0;
    private float playerPosition = 0;
    private int minRange = 0;
    private int maxRange = 0;
    public int spawnDistance = 40;
    int count = 0;
    int countw = 0;

    public int LivesPerLevel;
    public float LivesLocation;
    public float LivesLocationHieght;

    // Use this for initialization
    void Start()
    {




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
            if (maxRange <= (Constant.level_2_max - 100f))
            {
                count++;

                GameObject _tempEnemyObj = (GameObject)Instantiate(walker_l2_prefab);

                // Find a random index between zero and one less than the number of spawn points.
                //int spawnPointIndex = Random.Range (0, spawnPoints.Length);
                _tempEnemyObj.name = "EnemyWalker";
                _preX = playerPosition;
            }
        }

        //	Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
