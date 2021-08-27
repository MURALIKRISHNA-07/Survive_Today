using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int LevelId;

    // Start is called before the first frame update
    List<GameObject> EnemyList;
    List<GameObject> DeadList;
    public GameObject[] EnemyPrefabs;

    public Transform[] SpawnPoints;
    public  int wavenumber;
    public int EnemyCount;


    public float WaittimeToRespawn;

    private int Spawnid = 0;
    private float Timer = 0f;

    public bool KillEnemies { get; private set; }
    public float Counter { get; private set; }

    public bool iswave;

    public float missionTime;

    public float currentMissionTime;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
      
        KillEnemies = false;
        Counter = 0;
        EnemyList = new List<GameObject>();
        DeadList = new List<GameObject>();
        if (UIManager.instance.iswave)
        {
            iswave = true;
            EnemyCount = 20;
            SpawnEnemies();
            UIManager.instance.P_Missionname.text = "Wave " + 1;
            StartCoroutine(Waves());
            UIManager.instance.P_Mission_Count.text = "" + missionTime.ToString() + " Seconds";
          StartCoroutine( UIManager.instance.ShowPop());
        }
        else
        {
            LevelId = PlayerPrefs.GetInt("Level");
            DisplayLevel();

            UIManager.instance.Missionname.text = "Kill The Enemies "+ Counter.ToString();
           // UIManager.instance.Mission_Count.text = "" + Counter.ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Storing Dead PEnemies.....And RESPAWNING.......
        if (DeadList.Count != 0)
        {
            Timer += Time.deltaTime;
            if (Timer > WaittimeToRespawn)
            {
                if (!DeadList[0].activeInHierarchy)
                {
                    if (Spawnid >= SpawnPoints.Length)
                    {
                        Spawnid = 0;
                    }

                    DeadList[0].transform.localPosition = SpawnPoints[Spawnid].position;
                    DeadList[0].transform.rotation = Quaternion.identity;

                    Spawnid++;
                    GameObject thisone = DeadList[0];
                    DeadList.Remove(DeadList[0]);
                    Timer = 0;

                    thisone.SetActive(true);
                   // this.GetComponent<EnemyController>().setMax_Health();
                }

            }
        }
        currentMissionTime += Time.deltaTime;


        UIManager.instance.Mission_Count.text = ""+Mathf.RoundToInt(missionTime-currentMissionTime).ToString()+" Seconds";
       

        if (iswave)
        {
            if (currentMissionTime >= missionTime)
            {
                currentMissionTime = 0;
                StartCoroutine(Waves());
                if(wavenumber>=11)
                {
                    missionTime = 0;
                    Time.timeScale = 0f;
                    UIManager.instance.gamestarted = false;
                    CamShake.Camshake = false;
                    UIManager.instance.EnterUIState(UIManager.UIState.victory);
                }
            }

        }
        else
        {
            if (!KillEnemies)
            { 
                if (currentMissionTime >= missionTime)
               {
                  UIManager.instance.EnterUIState(UIManager.UIState.victory);
                }
  

            }
            else
            if (currentMissionTime >= missionTime)
            {
                
           
                Time.timeScale = 0f;
                UIManager.instance.gamestarted = false;
                CamShake.Camshake = false;
                UIManager.instance.EnterUIState(UIManager.UIState.death);
            }
        }

    }


    public void SpawnEnemies()
    {
        for (int i = 0; i < EnemyCount; i++)
        {

            int index = Random.Range(0, EnemyPrefabs.Length);
            GameObject obj = (GameObject)Instantiate(EnemyPrefabs[index]);
            obj.SetActive(false);
            EnemyList.Add(obj);
        }
        GetEnemies();

    }

    public void GetEnemies()
    {
        //checking the availibility of objects that are not active
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (!EnemyList[i].activeInHierarchy)
            {
                if (Spawnid >= SpawnPoints.Length)
                {
                    Spawnid = 0;
                }

                EnemyList[i].transform.localPosition = SpawnPoints[Spawnid].position;
                EnemyList[i].transform.rotation = Quaternion.identity;

                Spawnid++;
                EnemyList[i].SetActive(true);

            }
        }

    }

    public void DeadEnemy(GameObject dead)
    {

        dead.SetActive(false);
        DeadList.Add(dead);
        if (KillEnemies)
        {
            Counter--;
            UIManager.instance.Missionname.text = "Kill The Enemies "+Counter.ToString();
           // UIManager.instance.Mission_Count.text = "" + Counter.ToString();

            if (Counter <= 0)
            {
                //Won Panel
                Time.timeScale = 0f;
                UIManager.instance.EnterUIState(UIManager.UIState.victory);
                
            }
        }
        
    }


    void DisplayLevel()
    {
        switch (LevelId)
        {
            case 0:
                KillEnemies = true;
                Counter = 7f;
                GameManager.instance.EnemyCount = 12;
                GameManager.instance.SpawnEnemies();
                missionTime = 120f;
                UIManager.instance.P_Missionname.text="Kill The Enemies "+ Counter.ToString();
                UIManager.instance.P_Mission_Count.text = ""+missionTime.ToString() + " Seconds";
               // missionTime = 120f;

                break;

            case 1:
                KillEnemies = true;
                Counter = 10f;
                GameManager.instance.EnemyCount = 15;
                GameManager.instance.SpawnEnemies();
                missionTime = 120f;
                UIManager.instance.P_Missionname.text = "Kill The Enemies "+ Counter.ToString();
                UIManager.instance.P_Mission_Count.text = "" + missionTime.ToString() + " Seconds";
                //missionTime = 120f;

                break;

            case 2:
                KillEnemies = false;
                Counter = 7f;
                GameManager.instance.EnemyCount = 17;
                GameManager.instance.SpawnEnemies();
                missionTime = 150f;
                UIManager.instance.P_Missionname.text = "Survive Till the End";
               UIManager.instance.P_Mission_Count.text = "" + missionTime.ToString() + " Seconds";
                //missionTime = 150f;

                break;

            case 3:
                KillEnemies = true;
                Counter = 20f;
                GameManager.instance.EnemyCount = 20;
                GameManager.instance.SpawnEnemies();
                missionTime = 300f;
                UIManager.instance.P_Missionname.text = "Kill The Enemies"+ Counter.ToString();
                UIManager.instance.P_Mission_Count.text = "" + missionTime.ToString()+" Seconds";
                //missionTime = 300f;

                break;

        }

    }
  

   public IEnumerator Waves()
    {
        wavenumber++;
        spawnner(20);
        UIManager.instance.Missionname.text = "Wave "+wavenumber;   
        yield return null;
    
    }

    void spawnner(int i)
    {
        missionTime = 100+i;
    }
   
}

