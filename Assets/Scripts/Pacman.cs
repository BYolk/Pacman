using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 吃豆人脚本
/// </summary>
public class Pacman : MonoBehaviour
{
    public float speed = 0.35f;//速度
    private Vector2 dest = Vector2.zero;//目的地

    private void Start()
    {
        //游戏开始时目的地为自身，即游戏开始时不动
        dest = transform.position;
    }

    /// <summary>
    /// 关于物理的移动放在fixedUpdate里面，固定帧调用
    /// </summary>
    private void FixedUpdate()
    {
        //实现物体的移动：
        //transform.position：瞬移移动，可以使用插值让其显得真实（将瞬移的过程分为若干个瞬移阶段）
        //Rigidbody：给物体施加力，让物体移动
        //第一个参数：起始位置；第二个参数：目的地；第三个参数：移动最大速度，该速度会缓慢增加（不会超过这个速度）
        Vector2 temp = Vector2.MoveTowards(transform.position,dest,speed);
        GetComponent<Rigidbody2D>().MovePosition(temp);

        //因为fixedUpdate运动次数在宏观角度还是非常快的，所以会造成一种问题：假如玩家向上移动，Vector2.up移动一米的过程中需要调用好几次fixedUpdate才能完成，在玩家还没移动完这一米的时候，玩家按下下一个按键，导致线上只移动了零点几米时就往另一个方向移动，会使玩家在拐角处卡住，游戏体验不好
        //所以需要用if条件来判断，当上一次运动完成的时候，才可以进行下一次运动
        if ((Vector2)transform.position == dest)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Valid(Vector2.up))
            {
                dest = (Vector2)transform.position + Vector2.up;
            }

            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Valid(Vector2.down))
            {
                dest = (Vector2)transform.position + Vector2.down;
            }

            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Valid(Vector2.left))
            {
                dest = (Vector2)transform.position + Vector2.left;
            }

            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Valid(Vector2.right))
            {
                dest = (Vector2)transform.position + Vector2.right;
            }

            //控制动画状态机
            Vector2 dir = dest - (Vector2)transform.position;//从玩家到目的地的向量
            GetComponent<Animator>().SetFloat("DirX",dir.x);//设置动画状态机的Float类型变量值，变量名称为DirX，变量值是dir向量的x坐标值
            GetComponent<Animator>().SetFloat("DirY",dir.y);//设置动画状态机的Float类型变量值，变量名称为DirY，变量值是dir向量的y坐标值
        }
    }

    /// <summary>
    /// 在FixedUpdate移动代码中，用if条件来判断，当上一次运动完成的时候，才可以进行下一次运动，但是当玩家遇到障碍墙时，玩家的运动目标是障碍墙所在的位置，但是因为碰撞玩家是抵达不了障碍墙的，也就是说“这一次运动”是不可能完成的，所以“下一次运动”也不可能完成，玩家就会困死在障碍墙面前
    /// 所以需要一个方法Valid来判断玩家要抵达的位置是否是可以抵达的
    /// </summary>
    /// <param name="dir">玩家的目的地</param>
    /// <returns></returns>
    private bool Valid(Vector2 dir)
    {
        Vector2 pos = transform.position;//玩家位置
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);//从玩家要到的位置发射一条射线到玩家位置，如果这条射线碰撞到物体，会将碰撞信息保存到RaycastHit2D对象内
        return (hit.collider == GetComponent<Collider2D>());//返回射线碰撞的是不是自身，如果是自身，说明要到的位置到玩家之间没有障碍物，否则有障碍物
    }
}
