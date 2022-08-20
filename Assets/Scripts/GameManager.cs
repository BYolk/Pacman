using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// 游戏管理脚本：让路径不能同时被多只鬼选择
/// </summary>
public class GameManager : MonoBehaviour
{
    //单例：
    #region
    public static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    //引用：
    #region
    //游戏场景中的对象引用
    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;
    private List<GameObject> pacdotGos = new List<GameObject>();
    //UI相关引用
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject startCountDownPrefab;
    public GameObject gameoverPrefab;
    public GameObject winPrefab;
    public AudioClip startClip;
    public Text remainText;
    public Text nowText;
    public Text scoreText;

    #endregion

    //属性：
    #region
    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> { 0, 1, 2, 3 };
    public bool isSuperPacman = false;
    //豆子数量，吃掉的豆子数量，玩家得分
    private int pacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;
    #endregion

    //实例化方法
    #region
    private void Awake()
    {
        _instance = this;
        int tempCount = rawIndex.Count;
        //遍历原始集合rawIndex，从中随机取出一个数保存到usingIndex集合内，并且将这个随机取出的数从原始集合中剔除
        for(int i = 0; i < tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
        }

        //获取场景中豆子对象
        foreach (Transform t in GameObject.Find("Maze").transform)
        {
            pacdotGos.Add(t.gameObject);
        }

        pacdotNum = GameObject.Find("Maze").transform.childCount;//记录地图豆子数量
    }

    private void Start()
    {
        SetGameState(false);
    }

    #endregion

    //update方法
    #region
    private void Update()
    {
        if(nowEat == pacdotNum && pacman.GetComponent<Pacman>().enabled != false)
        {
            //胜利
            gamePanel.SetActive(false);
            Instantiate(winPrefab);
            StopAllCoroutines();
            SetGameState(false);
        }
        if(nowEat == pacdotNum)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
        //实时更新UI
        if (gamePanel.activeInHierarchy)
        {
            remainText.text = "Remain:\n\n" + (pacdotNum - nowEat);
            nowText.text = "Eaten:\n\n" + nowEat;
            scoreText.text = "Score:\n\n" + score;
        }
    }
    #endregion

    //生成超级豆与玩家吃到豆子方法
    #region
    /// <summary>
    /// 随机生成超级豆
    /// </summary>
    private void CreateSuperPacdot()
    {
        //避免生成豆子协程倒计时结束时恰好玩家吃掉最后一个豆子，此时会报错
        if (pacdotGos.Count < 5)
        {
            return;
        }
        int tempIndex = Random.Range(0, pacdotGos.Count);
        pacdotGos[tempIndex].transform.localScale = new Vector3(3, 3, 3);
        pacdotGos[tempIndex].GetComponent<Pacdot>().isSuperPacdot = true;
    }

    /// <summary>
    /// 当豆子被吃了,将豆子从存放场景豆子对象的集合中移除
    /// </summary>
    /// <param name="go"></param>
    public void OnEatPacdot(GameObject go)
    {
        nowEat++;
        score += 100;
        pacdotGos.Remove(go);
    }

    /// <summary>
    /// 当超级豆子被吃了，调用相关方法
    /// </summary>
    public void OnEatSuperPacdot()
    {
        score += 200;
        //当吃掉一个超级豆之后，十秒后再生成一个超级豆
        isSuperPacman = true;
        FreezeEnemy();
        StartCoroutine(RecoveryEnemy());
        Invoke("CreateSuperPacdot", 10f);
    }
    #endregion

    //恢复敌人，冻结敌人与解冻敌人
    #region
    /// <summary>
    /// 使敌人恢复协程方法
    /// </summary>
    /// <returns></returns>
    IEnumerator RecoveryEnemy()
    {
        //等三秒
        yield return new WaitForSeconds(3f);
        DisFreezeEnemy();
        isSuperPacman = false;
    }

    /// <summary>
    /// 冻结敌人
    /// </summary>
    private void FreezeEnemy()
    {
        //调用.enable，将其设置为false之后，改脚本的Update，FixedUpdate方法会被禁用，其他不会
        blinky.GetComponent<GhostMove>().enabled = false;
        clyde.GetComponent<GhostMove>().enabled = false;
        inky.GetComponent<GhostMove>().enabled = false;
        pinky.GetComponent<GhostMove>().enabled = false;

        //让敌人变虚
        blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f,0.7f,0.7f,0.7f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f,0.7f,0.7f,0.7f);
        inky.GetComponent<SpriteRenderer>().color = new Color(0.7f,0.7f,0.7f,0.7f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f,0.7f,0.7f,0.7f);

    }

    /// <summary>
    /// 解冻敌人
    /// </summary>
    private void DisFreezeEnemy()
    {
        //调用.enable，将其设置为false之后，改脚本的Update，FixedUpdate方法会被禁用，其他不会
        blinky.GetComponent<GhostMove>().enabled = true;
        clyde.GetComponent<GhostMove>().enabled = true;
        inky.GetComponent<GhostMove>().enabled = true;
        pinky.GetComponent<GhostMove>().enabled = true;

        //让敌人变虚
        blinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

    }
    #endregion

    //UI相关方法：
    #region
    /// <summary>
    /// 游戏开始界面，所有对象都不可动
    /// </summary>
    /// <param name="state">传递过来的状态</param>
    private void SetGameState(bool state)
    {
        pacman.GetComponent<Pacman>().enabled = state;
        blinky.GetComponent<GhostMove>().enabled = state;
        clyde.GetComponent<GhostMove>().enabled = state;
        inky.GetComponent<GhostMove>().enabled = state;
        pinky.GetComponent<GhostMove>().enabled = state;
    }

    /// <summary>
    /// 当玩家按下开始按钮，播放倒计时，激活所有游戏对象
    /// </summary>
    public void OnStartButton()
    {
        StartCoroutine(PlayStartCountDown());
        AudioSource.PlayClipAtPoint(startClip, new Vector3(0,0,-5));//播放位置离摄像机越近声音越大
        startPanel.SetActive(false);//隐藏游戏开始界面面板
    }

    /// <summary>
    /// 当玩家按下退出按钮
    /// </summary>
    public void OnExitButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// 开启倒计时协程
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayStartCountDown()
    {
        GameObject go = Instantiate(startCountDownPrefab);
        yield return new WaitForSeconds(4f);//等待动画播放4s
        Destroy(go);
        SetGameState(true);
        //延迟调用随机生成超级豆子
        Invoke("CreateSuperPacdot", 10f);
        gamePanel.SetActive(true);//激活游戏中面板
        GetComponent<AudioSource>().Play();
    }
    #endregion
}
