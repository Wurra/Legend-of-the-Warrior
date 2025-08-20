using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoadEventSO loadEvent;
    public VoidEventSO afterSceneLoadEvent;
    public PlayerInputControl inputControl;
    //这是在proje中右键点击Assets文件夹，选择Create->Input Actions创建的输入控制器
    //名字可重命名为PlayerInputControl
    //点进去后有个create c# script的按钮，点击后会生成一个PlayerInputControl.cs脚本
    // 这个脚本会自动生成输入控制的代码
    private Rigidbody2D rb;
    private PlayerAnimation playerAnimation;
    public CapsuleCollider2D capsuleCollider2D;
    private physicsCheck playerCheck;
    //physicsCheck是一个自定义的脚本，用于检测玩家是否在地面上
    // 这个脚本会在玩家脚本中使用，用于判断玩家是否可以跳跃
    public Vector2 inputDirection;
    // 用于存储玩家的输入方向
    public float moveSpeed = 5f;
    // 可以根据需要调整移动速度
    private int jumpCounts=2;// 用于记录跳跃次数
    private Vector2 inputDir;
    private float wallDir;
    [Header("人物控制基础参数")]
    public float dashcoolTime;
    public float dashTime;
    public float speed;
    public float jumpForce;
    public float wallJumpForce;
    public float dashForce;
    public float hurtFoorce;
    [Header("人物控制状态")]
    public bool isJump;// 用于判断玩家是否在跳跃状态
    public bool canDash;// 用于判断玩家是否可以冲刺
    public bool isDashing;// 用于判断玩家是否在冲刺状态
    public bool isWallsliding;// 用于判断玩家是否在攀爬状态
    public bool isWallJumping;// 用于判断玩家是否在墙壁跳跃状态
    public bool isHurt;// 用于判断玩家是否在受伤状态
    public bool isDead;// 用于判断玩家是否死亡状态
    public bool isAttack;// 用于判断玩家是否在攻击状态
    [Header("物理材质")]
    public PhysicsMaterial2D normal; // 正常物理材质
    public PhysicsMaterial2D wall; // 滑动物理材质
    private void Awake()
    {
        inputControl = new PlayerInputControl();//获取 PlayerInputControl 组件
        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.Dash.started += Dash;
        inputControl.Gameplay.Attack.started += PlayerAttack;
        rb = GetComponent<Rigidbody2D>();//获取 Rigidbody2D 组件
        inputControl.Gameplay.Climb.performed += Wallsliding;
        inputControl.Gameplay.Climb.canceled += CancelWallsliding;
    //在Unity的新输入系统中，输入动作有三个主要的事件阶段：
    //performed（执行完成）
    //performed 事件在输入动作被成功触发时调用。这通常意味着：
    //	按钮被完全按下达到阈值
    //	按住类操作达到预设条件
    //	手势或操作被识别为完整
    //在这段代码中，当玩家按住攀爬按钮，并且输入达到系统认定的"已执行"阈值时，会调用Wallsliding方法，使玩家进入攀爬状态。
   
    //canceled（取消）
    //canceled 事件在输入动作结束或被中断时调用。这通常发生在：
    //	按钮被释放
    //	手势被中断
    //	输入操作被用户终止
    capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        playerCheck = GetComponent<physicsCheck>();
        isJump = true; // 初始化跳跃状态为 true
        canDash = true; // 初始化可以冲刺状态为 true
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    



    //下面onEnable和OnDisable方法是为了让玩家在过场动画时禁用输入控制
    private void OnEnable()
    {
        //启用 PlayerInputControl
        inputControl.Enable();
        loadEvent.LoadRequestEvent +=OnloadEvent;
        afterSceneLoadEvent.OnEventRaised += OnafterSceneLoadEvent;
    }
    private void OnDisable()
    {
        //禁用 PlayerInputControl
        inputControl.Disable();
        loadEvent.LoadRequestEvent -= OnloadEvent;
        afterSceneLoadEvent.OnEventRaised -= OnafterSceneLoadEvent;
    }

   

    private void Update()
    {
        if (isDashing || isHurt) // 添加isHurt检查
            return;
        //Debug.Log(rb.velocity.y);
        if (isDashing)
            return; // 如果正在冲刺，则不处理其他输入
        if (playerCheck.isGround&&isJump)
            jumpCounts = 2; // 如果在地面上，重置跳跃次数为2
        if(isWallJumping||playerCheck.onWall)
        {
            return; // 如果正在墙壁跳跃或在墙上，则不处理其他输入
        }

        if (playerCheck.onWall)
            jumpCounts = 2;

        //获取移动输入
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        InputCorrection(); // 调用输入修正方法
        //CheckState(); // 检查玩家状态并更新物理材质
    }
    private void FixedUpdate()
    {
        if (isDashing || isHurt||isAttack) // 添加isHurt检查 // 如果正在冲刺，则不处理其他输入
            return;
       
        MovePlayer();
        Wallslide();
    }

    private void OnloadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable(); // 禁用输入控制
    }

    private void OnafterSceneLoadEvent()
    {
        inputControl.Gameplay.Enable(); // 启用输入控制
    }

    private void MovePlayer()
    {
        if (!isWallJumping)
        //使用物理引擎移动玩家
        //原来如此，一个Vector2的值只要去获取它对应的x或者y就是相对应的float值
        { rb.velocity = new Vector2(inputDir.x * moveSpeed, rb.velocity.y); }
        //翻转玩家的朝向
        if (inputDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // 面向右侧
        }
        else if (inputDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // 面向左侧
        }
    }
    //传入的参数是 InputAction.CallbackContext
    //这个参数包含了输入事件的上下文信息，所以不需要在update方法中获取输入
    private void Jump(InputAction.CallbackContext obj)
    {
        // 检查玩家是否在地面上
        if (playerCheck.isGround && jumpCounts == 2)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(playerJump()); // 启动跳跃协程
            jumpCounts--; // 减少跳跃次数
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
        else if (!playerCheck.isGround && jumpCounts == 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // 重置垂直速度
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCounts--; // 减少跳跃次数
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
        else if(playerCheck.onWall)
        {
            rb.AddForce(new Vector2(wallDir, 2f)*wallJumpForce,ForceMode2D.Impulse);
            isWallJumping = true;
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
        if (!playerCheck.isGround && jumpCounts == 2&&!playerCheck.onWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // 重置垂直速度
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCounts--; // 减少跳跃次数
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }
    private IEnumerator playerJump()
    {
        isJump = false; // 设置跳跃状态为flase
        yield return new WaitForSeconds(0.03f); // 等待0.03秒
        isJump = true; // 设置跳跃状态为true
    }
    private void Dash(InputAction.CallbackContext context)
    {
        if(canDash)
        {
            canDash = false;
            isDashing = true;
            StartCoroutine(PlayerDash(dashTime, dashcoolTime)); // 启动冲刺协程
        }
    }
    private IEnumerator PlayerDash(float dashTime,float dashCoolTime)
    {
        float originalGravity=rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity=new Vector2(transform.localScale.x*dashForce, 0);

        yield return new WaitForSeconds(dashTime); // 等待冲刺时间

        rb.velocity= Vector2.zero; // 停止冲刺
        rb.gravityScale = originalGravity; // 恢复重力
        isDashing = false; // 设置冲刺状态为false

        yield return new WaitForSeconds(dashCoolTime); // 等待冲刺冷却时间

        canDash = true; // 设置可以冲刺
    }
  

    private void Wallsliding(InputAction.CallbackContext context)
    {
        isWallsliding = true;
    }
    private void CancelWallsliding(InputAction.CallbackContext context)
    {
        isWallsliding = false; // 设置攀爬状态为false
        rb.velocity=new Vector2(rb.velocity.x,rb.velocity.y); 
    }
    private void Wallslide()
    {
        if(playerCheck.onWall &&  !isWallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y /10 ); // 使玩家在墙上滑动
        }
        if(isWallJumping && !playerCheck.onWall) 
            isWallJumping = false;
        if (playerCheck.onWall && playerCheck.touchLeftWall)
            wallDir = 1;
        else if (playerCheck.onWall && playerCheck.touchRightWall)
            wallDir = -1;
        else
            wallDir = 0;

       
    }
    // 这个方法用于修正输入方向的角度
    private void InputCorrection()
    {
        // 计算输入方向的角度
        // 使用 Mathf.Atan2 计算角度，并将其转换为度数
        float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
        if(angle < 0)
        {
            // 如果角度小于0，则将其转换为正角度
            angle += 360;
        }
        if(inputDirection==Vector2.right)
        {
            inputDir = new Vector2(1, 0);
            return;

        }
        if ((angle >= 337.5 && angle < 360) || (angle > 0 && angle < 22.5))
            inputDir = new Vector2(1, 0);
        else if((angle >=22.5 && angle < 67.5))
            inputDir = new Vector2(1,1);
        else if((angle >= 67.5 && angle < 112.5))
            inputDir = new Vector2(0, 1);
        else if((angle >= 112.5 && angle < 157.5))
            inputDir = new Vector2(-1, 1);
        else if((angle >= 157.5 && angle < 202.5))
            inputDir = new Vector2(-1, 0);
        else if((angle >= 202.5 && angle < 247.5))
            inputDir = new Vector2(-1,-1);
        else if((angle >= 247.5 && angle < 292.5))
            inputDir = new Vector2(0, -1);
        else if((angle >= 292.5 && angle < 337.5))
            inputDir = new Vector2(1,-1);
        else
        {
            inputDir = Vector2.zero; // 如果没有匹配到任何方向，则设置为零向量
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log(collision.name);
    //}
    public void GetHurt(Transform attacker)
    {
        isHurt = true; // 设置受伤状态为true
        //rb.velocity=new Vector2.zero; 这句代码不能用new关键字，因为 Vector2.zero 是一个静态属性
        //这里的问题是 Vector2.zero 已经是一个静态属性（预定义的常量），它不是一个类型,
        //所以不能用 new 来创建它的实例。
        //在 C# 中，new 关键字用于创建一个类型的新实例，必须跟随一个类型名称。
        //Vector2.zero 是 Unity 提供的一个便捷方式，等同于 new Vector2(0, 0)
        rb.velocity = Vector2.zero; // 重置玩家的速度
        Vector2 dir=new Vector2(transform.position.x - attacker.position.x,0 ).normalized;
        // 计算受伤方向
        rb.AddForce(dir * hurtFoorce, ForceMode2D.Impulse); // 添加受伤力
    }
    public void Die()
    {
        isDead = true; // 设置死亡状态为true
        inputControl.Gameplay.Disable(); // 禁用输入控制

    }
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayerAttack(); // 调用玩家动画的攻击方法
        isAttack=true; // 设置攻击状态为true
    }
    private void CheckState()
    {
        capsuleCollider2D.sharedMaterial = playerCheck.isGround ? normal : wall;
    }
         
}