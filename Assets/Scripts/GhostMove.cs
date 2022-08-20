using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 敌人巡逻路径
/// </summary>
public class GhostMove : MonoBehaviour
{
    //属性
    public float speed = 0.2f;//速度
    private int wayPointIndex = 0;//当前前往哪个路径点的途中
    private Vector3 startPos;//鬼起始位置

    //引用
    public GameObject[] wayPointGOs;//获取巡逻路径预制体对象数组
    private List<Vector3> wayPoints = new List<Vector3>();//创建集合，用于存放巡逻路径

    private void Start()
    {
        startPos = transform.position + new Vector3(0,3,0);
        //随机选择一个巡逻路径预制体
        //GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder -2]:获取GameManager对象的usingIndex集合，获取这个集合的层级顺序，让其-2（层级顺序从2到5，-2的话就是从0到3），也就是说，不同的敌人从usingIndex这个集合中取的值的下标是不同且固定的，这个下标的元素值是随机的
        LoadAPath(wayPointGOs[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder -2]],wayPoints);
    }

    /// <summary>
    /// 移动方法卸载fixedUpdate
    /// </summary>
    private void FixedUpdate()
    {
        //判断是否到达路径点，到达路径点后才能继续往下一个路径点
        if(transform.position != wayPoints[wayPointIndex])
        {
            //当前移动向量
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[wayPointIndex], speed);
            GetComponent<Rigidbody2D>().MovePosition(temp);//沿移动向量运动
        }
        else
        {
            wayPointIndex++;
            //如果下标超过路径对象的路径点数，说明到达终点，加载吓一跳路径
            if(wayPointIndex >= wayPoints.Count)
            {
                wayPointIndex = 0;
                //随机加载一条路径
                LoadAPath(wayPointGOs[Random.Range(0, wayPointGOs.Length)], wayPoints);
            }
        }
        //获取当前移动向量，得到当前移动向量的x，y的值，通过判断是正是负来播放动画
        Vector2 dir = wayPoints[wayPointIndex] - transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果进入触发范围的是Pacman销毁玩家，表示玩家被鬼杀了
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
                //玩家死亡，隐藏游戏中面板，实例化游戏结束对象，延迟几秒后重置场景
                collision.gameObject.SetActive(false);
                GameManager.Instance.gamePanel.SetActive(false);
                Instantiate(GameManager.Instance.gameoverPrefab);
                Invoke("ReStart", 3f);
            }
            
        }
    }

    /// <summary>
    /// 加载路径方法
    /// </summary>
    /// <param name="go">要加载的路径预制体</param>
    /// <param name="wayPoints">用于存放路径预制体内的路径点的集合</param>
    void LoadAPath(GameObject go,List<Vector3> wayPoints)
    {
        //清空原先集合内的路径点
        wayPoints.Clear();
        //将巡逻路径预制体对象内的路径点保存到WayPoints集合里面
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t.position);
        }
        wayPoints.Insert(0, startPos);//插入路径第一个路径点
        wayPoints.Add(startPos);//插入路径的最后一个路径点
    }

    /// <summary>
    /// 游戏结束，重置场景
    /// </summary>
    private void ReStart()
    {
        SceneManager.LoadScene(0);
    }
}
