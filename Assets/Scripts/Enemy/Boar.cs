using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
   //public override void Move()
   // {
   //     base.Move();
   //     // ���д����ʾ�ȿ���ʹ�û����Move������Ҳ�������±����Boar���е��ƶ��߼�

   //     // �������������Boar���еĶ�������Ϊ
   //     enemyAnimator.SetBool("walk", true);
   // }
   protected override void Awake()
   {
       base.Awake();
      patrolState = new BoarPatrolState();
      chaseState= new BoarChaseState();
    }
}
