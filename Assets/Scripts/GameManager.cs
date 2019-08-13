using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus
{
    next,
    play,
    gameover,
    win
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private int totalEnemies = 3; //enemy u talasu
    [SerializeField] private int enemiesPerSpawn;

    //UI
    [SerializeField] private int totalWaves = 1;
    [SerializeField] private Text totalMoneyLbl;
    [SerializeField] private Text currentWaveLvl;
    [SerializeField] private Text playBtnLbl;
    [SerializeField] private Button playBtn;
    [SerializeField] private Text totalEscapeLbl;


    private int waveNumber = 0;
    private int totalMoney = 6;
    private int whichEnemySpawn = 0;
    private int enemySpawn = 0;
    private GameStatus currentState = GameStatus.play;

    //private int enemies = 0;
    private const float spawnDelay = 0.5f;

    public List<Enemy> EnemyList = new List<Enemy>();

    public List<Projectile> ProjectileList = new List<Projectile>(); 

    public int TotalEscape { get; set; } = 0;

    public int RoundEscape { get; set; } = 0;

    public int TotalKilled { get; set; } = 0;

    public AudioSource AudioSource { get; private set; }

    public int TotalMoney
    {
        get { return totalMoney; }
        set
        {
            totalMoney = value;
            totalMoneyLbl.text = totalMoney.ToString();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        playBtn.gameObject.SetActive(false);
        AudioSource = GetComponent<AudioSource>();
        ShowManu();
    }

    // Update is called once per frame
    void Update()
    {
        Esc();
    }


    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0, enemySpawn)]) as Enemy;
                    newEnemy.transform.position = spawnPoint.transform.position;
                    
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)  
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyAll()
    {
        foreach (Enemy enemy in EnemyList)
            Destroy(enemy.gameObject);

        EnemyList.Clear();
    }



    public void AddMOney(int amount)
    {
        TotalMoney += amount;
    }

    public void SubtracyMoney(int amount)
    {
        if(TotalMoney > 0)
          TotalMoney -= amount;
    }

    public void WaveOVer()
    {
        totalEscapeLbl.text = "Escape " + TotalEscape + "/10";
        if((RoundEscape + TotalKilled) == totalEnemies)
        {
            if(waveNumber <= enemies.Length)
            {
                enemySpawn = waveNumber;
            }
            SetCurrentState();
            ShowManu();
        }
    }


    public void SetCurrentState()
    {
        if(TotalEscape >= 10)
        {
            currentState = GameStatus.gameover;
        }
        else if(waveNumber == 0 && (TotalKilled + RoundEscape) == 0)
        {
            currentState = GameStatus.play;
        }
        else if(waveNumber >= totalWaves)
        {
            currentState = GameStatus.win;
        }
        else
        {
            currentState = GameStatus.next;
        }
    }

    public void ShowManu()
    {
        switch (currentState)
        {
            case GameStatus.gameover:
                playBtnLbl.text = "Play Again";
                AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
                break;

            case GameStatus.next:
                playBtnLbl.text = "Next wave";

                break;

            case GameStatus.play:
                playBtnLbl.text = "Play";

                break;

            case GameStatus.win:
                playBtnLbl.text = "Win";

                break;
        }

        playBtn.gameObject.SetActive(true);
    }

    public void PlatBtnPress()
    {
        Debug.Log("Play dugme...");

        switch (currentState)
        {
            case GameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;

            

            default:
                totalEnemies = 3;
                TotalEscape = 0;
                TotalMoney = 6;
                enemySpawn = 0;
                waveNumber = 0;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagsBuildSite();
                TowerManager.Instance.DestroyAllProjectiles();
                totalMoneyLbl.text = TotalMoney.ToString();
                totalEscapeLbl.text = "Escaped " + TotalEscape + "/10";
                AudioSource.PlayOneShot(SoundManager.Instance.NewGame);
                break;
        }
        DestroyAll();
        TotalKilled = 0;
        RoundEscape = 0;
        currentWaveLvl.text = "Wave " + (waveNumber + 1);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }

    private void Esc() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDragSprite();
            TowerManager.Instance.towerBtnPress = null;
        }
    }
}
