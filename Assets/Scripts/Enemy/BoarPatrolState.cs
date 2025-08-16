using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    public override void onEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed; // ����Ѳ���ٶ�
    }
   
    public override void LogicUpdate()
    {
        //if (!currentEnemy.physicsCheck.isGround||(currentEnemy.physicsCheck.touchLeftWall) || (currentEnemy.physicsCheck.touchRightWall))
        //{
        //    currentEnemy.isWaiting = true;
        //    currentEnemy.anim.SetBool("walk", false); // ֹͣ���߶���
        //    currentEnemy.TimeCounter();
        //}
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.switchState(NPCState.Chase);
        }
        if ((!currentEnemy.physicsCheck.isGround)  || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            //Debug.Log(currentEnemy.physcisCheck.isGround);
            currentEnemy.rb.velocity = Vector2.zero;
            currentEnemy.isWaiting = true;
            currentEnemy.anim.SetBool("walk", false);
            
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true); // ��ʼ���߶���
            
        }
    }

   
 public override void PhysicsUpdate()
    {
        //throw new System.NotImplementedException();
    }

    public override void onExit()
    {
        currentEnemy.anim.SetBool("walk", false);
        Debug.Log("exit patrol state");
    }

   
}
