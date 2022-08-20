using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// ��Ϸ����ű�����·������ͬʱ����ֻ��ѡ��
/// </summary>
public class GameManager : MonoBehaviour
{
    //������
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

    //���ã�
    #region
    //��Ϸ�����еĶ�������
    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;
    private List<GameObject> pacdotGos = new List<GameObject>();
    //UI�������
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

    //���ԣ�
    #region
    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> { 0, 1, 2, 3 };
    public bool isSuperPacman = false;
    //�����������Ե��Ķ�����������ҵ÷�
    private int pacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;
    #endregion

    //ʵ��������
    #region
    private void Awake()
    {
        _instance = this;
        int tempCount = rawIndex.Count;
        //����ԭʼ����rawIndex���������ȡ��һ�������浽usingIndex�����ڣ����ҽ�������ȡ��������ԭʼ�������޳�
        for(int i = 0; i < tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
        }

        //��ȡ�����ж��Ӷ���
        foreach (Transform t in GameObject.Find("Maze").transform)
        {
            pacdotGos.Add(t.gameObject);
        }

        pacdotNum = GameObject.Find("Maze").transform.childCount;//��¼��ͼ��������
    }

    private void Start()
    {
        SetGameState(false);
    }

    #endregion

    //update����
    #region
    private void Update()
    {
        if(nowEat == pacdotNum && pacman.GetComponent<Pacman>().enabled != false)
        {
            //ʤ��
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
        //ʵʱ����UI
        if (gamePanel.activeInHierarchy)
        {
            remainText.text = "Remain:\n\n" + (pacdotNum - nowEat);
            nowText.text = "Eaten:\n\n" + nowEat;
            scoreText.text = "Score:\n\n" + score;
        }
    }
    #endregion

    //���ɳ���������ҳԵ����ӷ���
    #region
    /// <summary>
    /// ������ɳ�����
    /// </summary>
    private void CreateSuperPacdot()
    {
        //�������ɶ���Э�̵���ʱ����ʱǡ����ҳԵ����һ�����ӣ���ʱ�ᱨ��
        if (pacdotGos.Count < 5)
        {
            return;
        }
        int tempIndex = Random.Range(0, pacdotGos.Count);
        pacdotGos[tempIndex].transform.localScale = new Vector3(3, 3, 3);
        pacdotGos[tempIndex].GetComponent<Pacdot>().isSuperPacdot = true;
    }

    /// <summary>
    /// �����ӱ�����,�����ӴӴ�ų������Ӷ���ļ������Ƴ�
    /// </summary>
    /// <param name="go"></param>
    public void OnEatPacdot(GameObject go)
    {
        nowEat++;
        score += 100;
        pacdotGos.Remove(go);
    }

    /// <summary>
    /// ���������ӱ����ˣ�������ط���
    /// </summary>
    public void OnEatSuperPacdot()
    {
        score += 200;
        //���Ե�һ��������֮��ʮ���������һ��������
        isSuperPacman = true;
        FreezeEnemy();
        StartCoroutine(RecoveryEnemy());
        Invoke("CreateSuperPacdot", 10f);
    }
    #endregion

    //�ָ����ˣ����������ⶳ����
    #region
    /// <summary>
    /// ʹ���˻ָ�Э�̷���
    /// </summary>
    /// <returns></returns>
    IEnumerator RecoveryEnemy()
    {
        //������
        yield return new WaitForSeconds(3f);
        DisFreezeEnemy();
        isSuperPacman = false;
    }

    /// <summary>
    /// �������
    /// </summary>
    private void FreezeEnemy()
    {
        //����.enable����������Ϊfalse֮�󣬸Ľű���Update��FixedUpdate�����ᱻ���ã���������
        blinky.GetComponent<GhostMove>().enabled = false;
        clyde.GetComponent<GhostMove>().enabled = false;
        inky.GetComponent<GhostMove>().enabled = false;
        pinky.GetComponent<GhostMove>().enabled = false;

        //�õ��˱���
        blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f,0.7f,0.7f,0.7f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f,0.7f,0.7f,0.7f);
        inky.GetComponent<SpriteRenderer>().color = new Color(0.7f,0.7f,0.7f,0.7f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f,0.7f,0.7f,0.7f);

    }

    /// <summary>
    /// �ⶳ����
    /// </summary>
    private void DisFreezeEnemy()
    {
        //����.enable����������Ϊfalse֮�󣬸Ľű���Update��FixedUpdate�����ᱻ���ã���������
        blinky.GetComponent<GhostMove>().enabled = true;
        clyde.GetComponent<GhostMove>().enabled = true;
        inky.GetComponent<GhostMove>().enabled = true;
        pinky.GetComponent<GhostMove>().enabled = true;

        //�õ��˱���
        blinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

    }
    #endregion

    //UI��ط�����
    #region
    /// <summary>
    /// ��Ϸ��ʼ���棬���ж��󶼲��ɶ�
    /// </summary>
    /// <param name="state">���ݹ�����״̬</param>
    private void SetGameState(bool state)
    {
        pacman.GetComponent<Pacman>().enabled = state;
        blinky.GetComponent<GhostMove>().enabled = state;
        clyde.GetComponent<GhostMove>().enabled = state;
        inky.GetComponent<GhostMove>().enabled = state;
        pinky.GetComponent<GhostMove>().enabled = state;
    }

    /// <summary>
    /// ����Ұ��¿�ʼ��ť�����ŵ���ʱ������������Ϸ����
    /// </summary>
    public void OnStartButton()
    {
        StartCoroutine(PlayStartCountDown());
        AudioSource.PlayClipAtPoint(startClip, new Vector3(0,0,-5));//����λ���������Խ������Խ��
        startPanel.SetActive(false);//������Ϸ��ʼ�������
    }

    /// <summary>
    /// ����Ұ����˳���ť
    /// </summary>
    public void OnExitButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// ��������ʱЭ��
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayStartCountDown()
    {
        GameObject go = Instantiate(startCountDownPrefab);
        yield return new WaitForSeconds(4f);//�ȴ���������4s
        Destroy(go);
        SetGameState(true);
        //�ӳٵ���������ɳ�������
        Invoke("CreateSuperPacdot", 10f);
        gamePanel.SetActive(true);//������Ϸ�����
        GetComponent<AudioSource>().Play();
    }
    #endregion
}
