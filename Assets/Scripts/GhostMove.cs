using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ����Ѳ��·��
/// </summary>
public class GhostMove : MonoBehaviour
{
    //����
    public float speed = 0.2f;//�ٶ�
    private int wayPointIndex = 0;//��ǰǰ���ĸ�·�����;��
    private Vector3 startPos;//����ʼλ��

    //����
    public GameObject[] wayPointGOs;//��ȡѲ��·��Ԥ�����������
    private List<Vector3> wayPoints = new List<Vector3>();//�������ϣ����ڴ��Ѳ��·��

    private void Start()
    {
        startPos = transform.position + new Vector3(0,3,0);
        //���ѡ��һ��Ѳ��·��Ԥ����
        //GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder -2]:��ȡGameManager�����usingIndex���ϣ���ȡ������ϵĲ㼶˳������-2���㼶˳���2��5��-2�Ļ����Ǵ�0��3����Ҳ����˵����ͬ�ĵ��˴�usingIndex���������ȡ��ֵ���±��ǲ�ͬ�ҹ̶��ģ�����±��Ԫ��ֵ�������
        LoadAPath(wayPointGOs[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder -2]],wayPoints);
    }

    /// <summary>
    /// �ƶ�����ж��fixedUpdate
    /// </summary>
    private void FixedUpdate()
    {
        //�ж��Ƿ񵽴�·���㣬����·�������ܼ�������һ��·����
        if(transform.position != wayPoints[wayPointIndex])
        {
            //��ǰ�ƶ�����
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[wayPointIndex], speed);
            GetComponent<Rigidbody2D>().MovePosition(temp);//���ƶ������˶�
        }
        else
        {
            wayPointIndex++;
            //����±곬��·�������·��������˵�������յ㣬������һ��·��
            if(wayPointIndex >= wayPoints.Count)
            {
                wayPointIndex = 0;
                //�������һ��·��
                LoadAPath(wayPointGOs[Random.Range(0, wayPointGOs.Length)], wayPoints);
            }
        }
        //��ȡ��ǰ�ƶ��������õ���ǰ�ƶ�������x��y��ֵ��ͨ���ж������Ǹ������Ŷ���
        Vector2 dir = wayPoints[wayPointIndex] - transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //������봥����Χ����Pacman������ң���ʾ��ұ���ɱ��
        if (collision.gameObject.name == "Pacman")
        {
            if (GameManager.Instance.isSuperPacman)
            {
                transform.position = startPos - new Vector3(0, 3, 0);
                wayPointIndex = 0;
                GameManager.Instance.score += 500;
            }
            else
            {
                //���������������Ϸ����壬ʵ������Ϸ���������ӳټ�������ó���
                collision.gameObject.SetActive(false);
                GameManager.Instance.gamePanel.SetActive(false);
                Instantiate(GameManager.Instance.gameoverPrefab);
                Invoke("ReStart", 3f);
            }
            
        }
    }

    /// <summary>
    /// ����·������
    /// </summary>
    /// <param name="go">Ҫ���ص�·��Ԥ����</param>
    /// <param name="wayPoints">���ڴ��·��Ԥ�����ڵ�·����ļ���</param>
    void LoadAPath(GameObject go,List<Vector3> wayPoints)
    {
        //���ԭ�ȼ����ڵ�·����
        wayPoints.Clear();
        //��Ѳ��·��Ԥ��������ڵ�·���㱣�浽WayPoints��������
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t.position);
        }
        wayPoints.Insert(0, startPos);//����·����һ��·����
        wayPoints.Add(startPos);//����·�������һ��·����
    }

    /// <summary>
    /// ��Ϸ���������ó���
    /// </summary>
    private void ReStart()
    {
        SceneManager.LoadScene(0);
    }
}
