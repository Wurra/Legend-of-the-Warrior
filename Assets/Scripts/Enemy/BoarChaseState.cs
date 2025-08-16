using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{ 
    public override void onEnter(Enemy enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("传入的enemy对象为空");
            return;
        }
        
        currentEnemy = enemy;
        Debug.Log("进入追逐状态");
        // 设置敌人的状态为追逐
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed; // 设置追逐速度
        
        if (currentEnemy.anim != null)
            currentEnemy.anim.SetBool("run", true); // 开始追逐动画
        
    }
    
    public override void LogicUpdate()
    {
        //if ((!currentEnemy.physicsCheck.isGround) || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        //{
        //    currentEnemy.transform.localScale=new Vector3(currentEnemy.faceDir.x, 1, 1);
        //}
        if(currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.switchState(NPCState.Patrol); 
        }
        if ((!currentEnemy.physicsCheck.isGround) || currentEnemy.physicsCheck.isSpike || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }

    }
  
    public override void PhysicsUpdate()
    {
       
    }
    
    public override void onExit()
    {

        currentEnemy.anim.SetBool("run", false); // 停止追逐动画

    }

}