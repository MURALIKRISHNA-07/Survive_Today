using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject Light;

    public Camera UiCamera;
    public Camera WeaponCam;
    public GameObject Player;
    public GameObject CrossHair;
    public GameObject EnemySpawanner;
    public GameObject Room;

    public enum UIState { menu, ingame, victory, death, empty ,Dialogues }

    public GameObject Menu;
    public GameObject Resume;
    public GameObject InGame;
    public GameObject Victory;
    public GameObject Death;
    public GameObject Dialogue_Panel;
    public GameObject Help_Panel;
    public GameObject Completed;


    public Text Missionname;
    public Text Mission_Count;

    //POPUP FOR SHOWING MIssion
    public GameObject PopUP;
    public Text P_Missionname;
    public Text P_Mission_Count;

    public bool Once = false;
    public bool gamestarted;
    private bool pause;
    public bool iswave = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EnterUIState(UIState.menu);

    }

    // Update is called once per frame
    void Update()
    {
        if(gamestarted)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pause = !pause;
                if (!pause)
                {
                    resume();

                }
                else if (pause)
                {
                    Pause();

                }

            }  
        }

    }
    public void StartGame()
    {
        Room.SetActive(false);
        EnterUIState(UIState.ingame);
        Player.SetActive(true);
        EnemySpawanner.SetActive(true);
        UiCamera.enabled = false;
        WeaponCam.enabled = true;
        Light.SetActive(true);
        StartCoroutine( ShowPop());
    }

    public void playGame()
    {
        UiCamera.enabled = false;
        Light.SetActive(false);
        Room.SetActive(true);
        Time.timeScale = 1f;
        Lock();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        Resume.SetActive(true);
        CrossHair.SetActive(false);
        Unlock();

    }
    public void ForWaves()
    {
        UiCamera.enabled = false;
        Light.SetActive(true);
        Room.SetActive(false);
        EnterUIState(UIState.ingame);
        Player.SetActive(true);
        EnemySpawanner.SetActive(true);
        UiCamera.enabled = false;
        WeaponCam.enabled = true;
        iswave = true;
       
    }
    public void resume()
    {
        Time.timeScale = 1f;
        Lock();
        Resume.SetActive(false);
        if (gamestarted)
        {
            CrossHair.SetActive(true);

        }
    }


    public void EnterUIState(UIState uIState)
    {
        switch (uIState)
        {
            case UIState.menu:
                Clear();
                Menu.SetActive(true);
                CrossHair.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                break;

            case UIState.ingame:
                Clear();
                InGame.SetActive(true);
                CrossHair.SetActive(true);
                break;

            case UIState.victory:
                Clear();
                Victory.SetActive(true);
                CrossHair.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                break;
            case UIState.death:
                Clear();
                Death.SetActive(true);
                CrossHair.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                break;
            case UIState.Dialogues:
                Clear();
                Dialogue_Panel.SetActive(true);
                CrossHair.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                break;
            case UIState.empty:
                Clear();
                break;

        }
    }
    public void Clear()
    {
        Menu.SetActive(false);
        InGame.SetActive(false);
        Victory.SetActive(false);
        Death.SetActive(false);
        Dialogue_Panel.SetActive(false);
        Help_Panel.SetActive(false);
    }
    public void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public IEnumerator ShowPop()
    {
        PopUP.SetActive(true);
        yield return new WaitForSeconds(2.2f);
        PopUP.SetActive(false);
    }

    public void BacktoMenu()
    {
        WeaponCam.enabled=false;
        PopUP.SetActive(false);
        Player.SetActive(false);
        Room.SetActive(false);
        EnemySpawanner.SetActive(false);
        UiCamera.enabled = true;
        gamestarted = false;
        Time.timeScale = 1;
        //EnterUIState(UIManager.UIState.menu);
        SceneManager.LoadScene(0);

    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void showHelp()
    {
        Help_Panel.SetActive(true);
    }

    public void showMenu()
    {
        EnterUIState(UIState.menu);
    }

    public void Nextlevel()
    {
        if (PlayerPrefs.GetInt("Level") == 3)
        {
            BacktoMenu();
            Completed.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        }
    }
    
    
}
