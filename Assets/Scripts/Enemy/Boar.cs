using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
   //public override void Move()
   // {
   //     base.Move();
   //     // 这行代码表示既可以使用基类的Move方法，也可以在下边添加Boar特有的移动逻辑

   //     // 可以在这里添加Boar特有的动画或行为
   //     enemyAnimator.SetBool("walk", true);
   // }
   protected override void Awake()
   {
       base.Awake();
      patrolState = new BoarPatrolState();
      chaseState= new BoarChaseState();
    }
}
