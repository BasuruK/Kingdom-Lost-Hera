using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class homeMenuController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onPlayButtonClick()
    {
        SceneManager.LoadScene("Level-1", LoadSceneMode.Single);
    }

    // handle exit event
    public void OnExitClick()
    {
        Application.Quit();
    }
}
