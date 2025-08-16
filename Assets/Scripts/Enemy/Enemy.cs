using System.Collections;
using System.Collections.Generic;
using System.Media;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator enemyAnimator;//一定是要用Animator来控制敌人的动画而不是Animation!!!
    protected Character character;
   public Rigidbody2D rb;
    public Transform attacker;
    [HideInInspector]public Animator anim;
    [Header("基本参数")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;
    public Vector2 faceDir;
    public float hurtForce;

   
    [Header("计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public bool isWaiting;
    public float lostTime;
    public float lostTimeCounter;
  
    [Header("状态")]
    public bool isHurt;

    protected BaseState patrolState;
    public BaseState currentState;
    public BaseState chaseState;

    [Header("检测")]
    public LayerMask attackLayer;
    public Vector2 centerOffest;
    public Vector2 checkSize;
    public float checkDistance;

    [HideInInspector] public enemyPhysicsCheck physicsCheck;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = normalSpeed;
        enemyAnimator= GetComponent<Animator>();
        physicsCheck = GetComponent<enemyPhysicsCheck>();
        //waitTimeCounter = waitTime;
        character = GetComponent<Character>();
        
    }
    private void OnEnable()
    {
        currentState=patrolState;
        currentState.onEnter(this);
    }
    private void Update()
    {
        // 先计算朝向，再检测墙壁
        //正常来说是scale大于0表示朝右，小于0表示朝左
        //但是这里猪的scale本身就是小于0但是却是朝右的，所以要取反
        faceDir = new Vector2(-transform.localScale.x, 0);

        // 检测墙壁并转向
        currentState.LogicUpdate();
        TimeCounter();
    }
    public void FixedUpdate()
    {
        if (!isHurt && !isWaiting)
        { 
            Move();
        } 
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.onExit();
    }
    public virtual void Move()
    {
        //但凡是Vector2的变量，x代表水平，y代表垂直
        rb.velocity = new Vector2(currentSpeed * faceDir.x, rb.velocity.y);
    }
    // 计时器方法
    public void TimeCounter()
    {
        if (isWaiting)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                isWaiting = false;
                waitTimeCounter = waitTime;
                // 翻转当前的X缩放值
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
        if (!FoundPlayer() && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;

        }
        else if (FoundPlayer()) // 添加这个额外的判断，在发现玩家的时候重置丢失时间
        {
            lostTimeCounter = lostTime;
        }

        //又卡关了fuck！！！！
        //if (!FoundPlayer() && lostTimeCounter > 0)
        //{
        //    lostTimeCounter -= Time.deltaTime;
        //}
        //else if (FoundPlayer())
        //{
        //    lostTimeCounter = lostTime;
        //   isWaiting = false;
        //    waitTimeCounter = waitTime;
        //}
    }

    public bool FoundPlayer()
    {
        
        return Physics2D.BoxCast(transform.position+(Vector3)centerOffest, checkSize , 0f, faceDir, checkDistance , attackLayer);
    }
    public void switchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null
        };
        currentState.onExit();
        currentState = newState;
        currentState.onEnter(this);
    }
    #region 事件执行方法
    public void onTakeDamage(Transform attackTrans)
    {
        attacker= attackTrans;
        if(attackTrans.position.x-transform.position.x>0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // 如果攻击者在右边，朝左
        }
        if(attackTrans.position.x - transform.position.x < 0)
        {  
            transform.localScale = new Vector3(1, 1, 1); // 如果攻击者在左边，朝右
        }
        isHurt = true; // 设置受伤状态为true
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity=new Vector2(0,rb.velocity.y); // 重置水平速度，保留垂直速度
        anim.SetTrigger("hurt"); // 播放受伤动画
        StartCoroutine(onHurt(dir)); // 启动受伤协程
    }

    private IEnumerator onHurt(Vector2 dir)
    {
        rb.AddForce(new Vector2(dir.x * hurtForce, 0), ForceMode2D.Impulse); // 添加冲击力
        yield return new WaitForSeconds(0.5f); // 等待0.5秒
        isHurt = false; // 重置受伤状态
    }
    public void Die()
    {
        if(character.currentHealth==0)
        {
            anim.SetTrigger("dead"); // 播放死亡动画
            rb.velocity = Vector2.zero; // 停止所有运动
            rb.isKinematic = true; // 设置刚体为运动学模式，停止物理交互
            Destroy(gameObject, 1.5f); // 2秒后销毁敌人对象
        }
    }
    #endregion
    private void onDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffest + new Vector3(checkDistance * (-transform.localScale.x), 0, 0), checkSize.y);
    }
}
