using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Զ��˽ű�
/// </summary>
public class Pacman : MonoBehaviour
{
    public float speed = 0.35f;//�ٶ�
    private Vector2 dest = Vector2.zero;//Ŀ�ĵ�

    private void Start()
    {
        //��Ϸ��ʼʱĿ�ĵ�Ϊ��������Ϸ��ʼʱ����
        dest = transform.position;
    }

    /// <summary>
    /// ����������ƶ�����fixedUpdate���棬�̶�֡����
    /// </summary>
    private void FixedUpdate()
    {
        //ʵ��������ƶ���
        //transform.position��˲���ƶ�������ʹ�ò�ֵ�����Ե���ʵ����˲�ƵĹ��̷�Ϊ���ɸ�˲�ƽ׶Σ�
        //Rigidbody��������ʩ�������������ƶ�
        //��һ����������ʼλ�ã��ڶ���������Ŀ�ĵأ��������������ƶ�����ٶȣ����ٶȻỺ�����ӣ����ᳬ������ٶȣ�
        Vector2 temp = Vector2.MoveTowards(transform.position,dest,speed);
        GetComponent<Rigidbody2D>().MovePosition(temp);

        //��ΪfixedUpdate�˶������ں�۽ǶȻ��Ƿǳ���ģ����Ի����һ�����⣺������������ƶ���Vector2.up�ƶ�һ�׵Ĺ�������Ҫ���úü���fixedUpdate������ɣ�����һ�û�ƶ�����һ�׵�ʱ����Ұ�����һ����������������ֻ�ƶ�����㼸��ʱ������һ�������ƶ�����ʹ����ڹսǴ���ס����Ϸ���鲻��
        //������Ҫ��if�������жϣ�����һ���˶���ɵ�ʱ�򣬲ſ��Խ�����һ���˶�
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

            //���ƶ���״̬��
            Vector2 dir = dest - (Vector2)transform.position;//����ҵ�Ŀ�ĵص�����
            GetComponent<Animator>().SetFloat("DirX",dir.x);//���ö���״̬����Float���ͱ���ֵ����������ΪDirX������ֵ��dir������x����ֵ
            GetComponent<Animator>().SetFloat("DirY",dir.y);//���ö���״̬����Float���ͱ���ֵ����������ΪDirY������ֵ��dir������y����ֵ
        }
    }

    /// <summary>
    /// ��FixedUpdate�ƶ������У���if�������жϣ�����һ���˶���ɵ�ʱ�򣬲ſ��Խ�����һ���˶������ǵ���������ϰ�ǽʱ����ҵ��˶�Ŀ�����ϰ�ǽ���ڵ�λ�ã�������Ϊ��ײ����ǵִﲻ���ϰ�ǽ�ģ�Ҳ����˵����һ���˶����ǲ�������ɵģ����ԡ���һ���˶���Ҳ��������ɣ���Ҿͻ��������ϰ�ǽ��ǰ
    /// ������Ҫһ������Valid���ж����Ҫ�ִ��λ���Ƿ��ǿ��Եִ��
    /// </summary>
    /// <param name="dir">��ҵ�Ŀ�ĵ�</param>
    /// <returns></returns>
    private bool Valid(Vector2 dir)
    {
        Vector2 pos = transform.position;//���λ��
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);//�����Ҫ����λ�÷���һ�����ߵ����λ�ã��������������ײ�����壬�Ὣ��ײ��Ϣ���浽RaycastHit2D������
        return (hit.collider == GetComponent<Collider2D>());//����������ײ���ǲ����������������˵��Ҫ����λ�õ����֮��û���ϰ���������ϰ���
    }
}
