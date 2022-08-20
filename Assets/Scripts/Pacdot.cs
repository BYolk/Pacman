using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 豆豆脚本
/// </summary>
public class Pacdot : MonoBehaviour
{
    //属性
    public bool isSuperPacdot = false;

    /// <summary>
    /// 当物体进入触发范围时触发方法
    /// </summary>
    /// <param name="collision">进入触发范围的物体对象</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果进入触发范围的是Pacman销毁自身表示被吃豆人吃了
        if(collision.gameObject.name == "Pacman")
        {
            if (isSuperPacdot)
            {
                GameManager.Instance.OnEatPacdot(gameObject);
                GameManager.Instance.OnEatSuperPacdot();
                Destroy(gameObject);
            }
            else
            {
                GameManager.Instance.OnEatPacdot(gameObject);
                Destroy(gameObject);
            }
            
        }
    }
}
