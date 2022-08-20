using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ű�
/// </summary>
public class Pacdot : MonoBehaviour
{
    //����
    public bool isSuperPacdot = false;

    /// <summary>
    /// ��������봥����Χʱ��������
    /// </summary>
    /// <param name="collision">���봥����Χ���������</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //������봥����Χ����Pacman���������ʾ���Զ��˳���
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
