using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;
    private void OnTriggerStay2D(Collider2D collision)
    {
        //���˸��ʺţ������ָ���쳣���൱�ڲ�Ϊ�յ��жϣ�ֻ�е�collision��Character���ʱ��ִ�к���Ĵ���
        collision.GetComponent<Character>()?.takeDamage(this);
        //�����ײ����Character����������takeDamage���������뵱ǰ��������
        

    }
}
