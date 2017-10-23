using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPanelController : UIAdapter
{
    // holds pause menu ref
    public GameObject pause_menu;
    // holds level progress bar
    public GameObject level_progress;
    // ground
    public GameObject ground;
    // backdrop
    private GameObject backdrop;
    // main enemy level 1
    public GameObject levelMainEnemy;
    // game over panel
    public GameObject gameOver;
    // game over panel main message
    public Text gameOverMainText;
    // game over panel message
    public Text gameOverText;
    // game over panel next btn text
    public Text gameOverNextBtn;
    // menu button
    public GameObject menuButton;
    // next button
    public GameObject nextButton;

    // global move x of player
    [HideInInspector] public static float m_move_x = 0;
    // keep running mode
    [HideInInspector] public static float m_speed = 0;

    // local
    private bool display_gameover = false;
    private int level = 0;
    private bool paused = false;
    // level one enemy created
    private bool display_enemy = false;
    // renderers
    private Renderer m_backdrop_renderer;
    private Image m_progress_renderer;
    // holds ground titles
    private List<GameObject> groundSegments = new List<GameObject>();
    // ground tiles counter
    private float tile_count = 0, pre_max_move_x = 0;

    // level section distances
    private float p_1, p_2, p_3, p_4, level_max;

    private static UIPanelController instance = null;

    public static UIPanelController getInstance()
    {
        return instance;
    }

    override
    public void viewGameOver(int level, bool state)
    {
        this.level = level; // update 
        if (!this.display_gameover)
        {
            this.pre_max_move_x = Constant.level_1_max;
            this.display_gameover = true;
            this.m_progress_renderer.material.mainTextureOffset = new Vector2(1.45f, 0.033f);
            this.gameOver.SetActive(true);
            if (state)
            {
                // state = true (complete level)
                this.gameOverMainText.text = "Level Completed";
                this.gameOverText.text = "Congratulation !.. \nYou have completed the level";
                if (this.level == 1)
                {
                    this.gameOverNextBtn.text = "Next";
                }
                else
                {
                    this.nextButton.SetActive(false);
                }
            }
            else
            {
                // state = false
                this.gameOverMainText.text = "Game Over";
                this.gameOverText.text = "Try Again !..";
                this.gameOverNextBtn.text = "Try Again";
            }
            this.gameOver.GetComponent<Animator>().SetTrigger("show");
        }
    }

    override
    public void changePoints(int value)
    {

    }

    public void GameOverButtonOne()
    {
        // update current level
        Constant.current_level = 1;
        // move to home screen
        SceneManager.LoadScene("Home Menu", LoadSceneMode.Single);
    }

    public void GameOverButtonTwo()
    {
        if (this.gameOverNextBtn.text == "Try Again")
        {
            if (this.level == 1)
            {
                SceneManager.LoadScene("Level-1", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("Level-2", LoadSceneMode.Single);
            }
        }
        else
        {
            if (this.level == 1)
            {
                // update current level
                Constant.current_level = 2;
                // move to Level-2
                SceneManager.LoadScene("Level-2", LoadSceneMode.Single);
            }
            else
            {
                // update current level
                Constant.current_level = 1;
                // reset current level
                SceneManager.LoadScene("Level-1", LoadSceneMode.Single);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        Time.timeScale = 1;
        pause_menu.SetActive(false); // hide pause menu (default)
        paused = false;
        if (Constant.current_level == 1)
        {
            this.p_1 = Constant.level_1_p_1;
            this.p_2 = Constant.level_1_p_2;
            this.p_3 = Constant.level_1_p_3;
            this.p_4 = Constant.level_1_p_4;
            this.level_max = Constant.level_1_max;

            this.backdrop = GameObject.Find("BackgroundScene");
            m_backdrop_renderer = backdrop.GetComponent<Renderer>();
            groundSegments.Add(Instantiate(ground, new Vector3(-8.29f, -5.4f, 9.94f), Quaternion.Euler(0, 0, 180)));// add initial tile
        }
        else
        {
            this.p_1 = Constant.level_2_p_1;
            this.p_2 = Constant.level_2_p_2;
            this.p_3 = Constant.level_2_p_3;
            this.p_4 = Constant.level_2_p_4;
            this.level_max = Constant.level_2_max;
        }
        m_progress_renderer = level_progress.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!this.display_gameover)
        {
            float moveHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime; // Move the Main Character Left and Right
            updateDistance(moveHorizontal); // distance changer
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // esc key pressed
            if (paused)
            {
                OnResumeClick();
            }
            else
            {
                OnPauseClick();
            }
        }
    }

    // do actions when distance changes
    private void updateDistance(float moveHorizontal)
    {
        //Debug.Log("move : " + m_move_x);
        // update previous max distance
        if (pre_max_move_x < m_move_x)
        {
            pre_max_move_x = m_move_x;
        }
        if (Constant.current_level == 1)
        {
            if (m_move_x < this.p_4)
            {
                backdrop.transform.position = new Vector3(m_move_x + 50, 20.8f, 22.77f); // move backdrop
                m_backdrop_renderer.material.SetTextureOffset("_MainTex", new Vector2(m_move_x * Constant.offset_speed, 0)); // update background offset value
                if (m_move_x > 10 && moveHorizontal > 0)
                {
                    if (m_move_x > 0 && ((int)(m_move_x) / 150) > tile_count)
                    {
                        // add new tile
                        groundSegments.Add(Instantiate(ground, new Vector3(
                            groundSegments[groundSegments.Count - 1].transform.position.x + 195, -5.4f, 9.94f), Quaternion.Euler(0, 0, 180)));
                        tile_count++; // update tile counter
                    }
                }

            }
        }
        // check progress levels (com - 8.024, 90% - 0.12, 75% - 0.314, 50% - 0.507, 25% - 0.701)_
        if (pre_max_move_x >= this.p_4)
        {
            // level 4
            m_progress_renderer.material.mainTextureOffset = new Vector2(1.45f, 0.12f);
            if (!display_enemy)
            {
                // insitiantiate enemy
                Instantiate(
                    levelMainEnemy,
                    new Vector3((this.level_max + Constant.view_margin_right + Constant.enemy_margin_right), 0f, 10f),
                    Quaternion.Euler(0, -90, 0));
                display_enemy = true;
            }
        }
        else if (pre_max_move_x >= this.p_3)
        {
            // level 3
            m_progress_renderer.material.mainTextureOffset = new Vector2(1.45f, 0.314f);
        }
        else if (pre_max_move_x >= this.p_2)
        {
            // level 2
            m_progress_renderer.material.mainTextureOffset = new Vector2(1.45f, 0.507f);
        }
        else if (pre_max_move_x >= this.p_1)
        {
            // level 1
            m_progress_renderer.material.mainTextureOffset = new Vector2(1.45f, 0.701f);
        }
        else
        {
            // initial level
            m_progress_renderer.material.mainTextureOffset = new Vector2(1.45f, 0.895f);
        }
    }

    // handle pause event
    public void OnPauseClick()
    {
        // pause the game play
        Time.timeScale = 0;
        // show pause menu
        pause_menu.SetActive(true);
        paused = true;
    }

    // handle resume event
    public void OnResumeClick()
    {
        // resume the game play
        Time.timeScale = 1;
        // hide pause menu
        pause_menu.SetActive(false);
        paused = false;
    }

    // handle restart event
    public void OnRestartClick()
    {
        if (Constant.current_level == 1)
        {
            SceneManager.LoadScene("Level-1", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Level-2", LoadSceneMode.Single);
        }
    }

    // handle settings event
    public void OnSettingsClick()
    {

    }

    // handle exit event
    public void OnExitClick()
    {
        // hide pause menu (default)
        pause_menu.SetActive(false);
        paused = false;
        // move to home screen
        SceneManager.LoadScene("Home Menu", LoadSceneMode.Single);
    }
}
