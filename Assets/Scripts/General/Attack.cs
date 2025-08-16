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
        //加了个问号，避免空指针异常，相当于不为空的判断，只有当collision有Character组件时才执行后面的代码
        collision.GetComponent<Character>()?.takeDamage(this);
        //如果碰撞体有Character组件，则调用takeDamage方法，传入当前攻击对象
        

    }
}
