using System.Collections;
using System.Collections.Generic;
using System.Media;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator enemyAnimator;//һ����Ҫ��Animator�����Ƶ��˵Ķ���������Animation!!!
    protected Character character;
   public Rigidbody2D rb;
    public Transform attacker;
    [HideInInspector]public Animator anim;
    [Header("��������")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector] public float currentSpeed;
    public Vector2 faceDir;
    public float hurtForce;

   
    [Header("��ʱ��")]
    public float waitTime;
    public float waitTimeCounter;
    public bool isWaiting;
    public float lostTime;
    public float lostTimeCounter;
  
    [Header("״̬")]
    public bool isHurt;

    protected BaseState patrolState;
    public BaseState currentState;
    public BaseState chaseState;

    [Header("���")]
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
        // �ȼ��㳯���ټ��ǽ��
        //������˵��scale����0��ʾ���ң�С��0��ʾ����
        //�����������scale�������С��0����ȴ�ǳ��ҵģ�����Ҫȡ��
        faceDir = new Vector2(-transform.localScale.x, 0);

        // ���ǽ�ڲ�ת��
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
        //������Vector2�ı�����x����ˮƽ��y����ֱ
        rb.velocity = new Vector2(currentSpeed * faceDir.x, rb.velocity.y);
    }
    // ��ʱ������
    public void TimeCounter()
    {
        if (isWaiting)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                isWaiting = false;
                waitTimeCounter = waitTime;
                // ��ת��ǰ��X����ֵ
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
        if (!FoundPlayer() && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;

        }
        else if (FoundPlayer()) // ������������жϣ��ڷ�����ҵ�ʱ�����ö�ʧʱ��
        {
            lostTimeCounter = lostTime;
        }

        //�ֿ�����fuck��������
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
    #region �¼�ִ�з���
    public void onTakeDamage(Transform attackTrans)
    {
        attacker= attackTrans;
        if(attackTrans.position.x-transform.position.x>0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // ������������ұߣ�����
        }
        if(attackTrans.position.x - transform.position.x < 0)
        {  
            transform.localScale = new Vector3(1, 1, 1); // �������������ߣ�����
        }
        isHurt = true; // ��������״̬Ϊtrue
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity=new Vector2(0,rb.velocity.y); // ����ˮƽ�ٶȣ�������ֱ�ٶ�
        anim.SetTrigger("hurt"); // �������˶���
        StartCoroutine(onHurt(dir)); // ��������Э��
    }

    private IEnumerator onHurt(Vector2 dir)
    {
        rb.AddForce(new Vector2(dir.x * hurtForce, 0), ForceMode2D.Impulse); // ��ӳ����
        yield return new WaitForSeconds(0.5f); // �ȴ�0.5��
        isHurt = false; // ��������״̬
    }
    public void Die()
    {
        if(character.currentHealth==0)
        {
            anim.SetTrigger("dead"); // ������������
            rb.velocity = Vector2.zero; // ֹͣ�����˶�
            rb.isKinematic = true; // ���ø���Ϊ�˶�ѧģʽ��ֹͣ������
            Destroy(gameObject, 1.5f); // 2������ٵ��˶���
        }
    }
    #endregion
    private void onDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffest + new Vector3(checkDistance * (-transform.localScale.x), 0, 0), checkSize.y);
    }
}
