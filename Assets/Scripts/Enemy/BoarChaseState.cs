using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{ 
    public override void onEnter(Enemy enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("�����enemy����Ϊ��");
            return;
        }
        
        currentEnemy = enemy;
        Debug.Log("����׷��״̬");
        // ���õ��˵�״̬Ϊ׷��
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed; // ����׷���ٶ�
        
        if (currentEnemy.anim != null)
            currentEnemy.anim.SetBool("run", true); // ��ʼ׷�𶯻�
        
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

        currentEnemy.anim.SetBool("run", false); // ֹͣ׷�𶯻�

    }

}