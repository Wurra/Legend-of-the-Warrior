using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("�����¼�")]
    public SceneLoadEventSO loadEvent;
    public VoidEventSO afterSceneLoadEvent;
    public PlayerInputControl inputControl;
    //������proje���Ҽ����Assets�ļ��У�ѡ��Create->Input Actions���������������
    //���ֿ�������ΪPlayerInputControl
    //���ȥ���и�create c# script�İ�ť������������һ��PlayerInputControl.cs�ű�
    // ����ű����Զ�����������ƵĴ���
    private Rigidbody2D rb;
    private PlayerAnimation playerAnimation;
    public CapsuleCollider2D capsuleCollider2D;
    private physicsCheck playerCheck;
    //physicsCheck��һ���Զ���Ľű������ڼ������Ƿ��ڵ�����
    // ����ű�������ҽű���ʹ�ã������ж�����Ƿ������Ծ
    public Vector2 inputDirection;
    // ���ڴ洢��ҵ����뷽��
    public float moveSpeed = 5f;
    // ���Ը�����Ҫ�����ƶ��ٶ�
    private int jumpCounts=2;// ���ڼ�¼��Ծ����
    private Vector2 inputDir;
    private float wallDir;
    [Header("������ƻ�������")]
    public float dashcoolTime;
    public float dashTime;
    public float speed;
    public float jumpForce;
    public float wallJumpForce;
    public float dashForce;
    public float hurtFoorce;
    [Header("�������״̬")]
    public bool isJump;// �����ж�����Ƿ�����Ծ״̬
    public bool canDash;// �����ж�����Ƿ���Գ��
    public bool isDashing;// �����ж�����Ƿ��ڳ��״̬
    public bool isWallsliding;// �����ж�����Ƿ�������״̬
    public bool isWallJumping;// �����ж�����Ƿ���ǽ����Ծ״̬
    public bool isHurt;// �����ж�����Ƿ�������״̬
    public bool isDead;// �����ж�����Ƿ�����״̬
    public bool isAttack;// �����ж�����Ƿ��ڹ���״̬
    [Header("�������")]
    public PhysicsMaterial2D normal; // �����������
    public PhysicsMaterial2D wall; // �����������
    private void Awake()
    {
        inputControl = new PlayerInputControl();//��ȡ PlayerInputControl ���
        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.Dash.started += Dash;
        inputControl.Gameplay.Attack.started += PlayerAttack;
        rb = GetComponent<Rigidbody2D>();//��ȡ Rigidbody2D ���
        inputControl.Gameplay.Climb.performed += Wallsliding;
        inputControl.Gameplay.Climb.canceled += CancelWallsliding;
    //��Unity��������ϵͳ�У����붯����������Ҫ���¼��׶Σ�
    //performed��ִ����ɣ�
    //performed �¼������붯�����ɹ�����ʱ���á���ͨ����ζ�ţ�
    //	��ť����ȫ���´ﵽ��ֵ
    //	��ס������ﵽԤ������
    //	���ƻ������ʶ��Ϊ����
    //����δ����У�����Ұ�ס������ť����������ﵽϵͳ�϶���"��ִ��"��ֵʱ�������Wallsliding������ʹ��ҽ�������״̬��
   
    //canceled��ȡ����
    //canceled �¼������붯���������ж�ʱ���á���ͨ�������ڣ�
    //	��ť���ͷ�
    //	���Ʊ��ж�
    //	����������û���ֹ
    capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        playerCheck = GetComponent<physicsCheck>();
        isJump = true; // ��ʼ����Ծ״̬Ϊ true
        canDash = true; // ��ʼ�����Գ��״̬Ϊ true
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    



    //����onEnable��OnDisable������Ϊ��������ڹ�������ʱ�����������
    private void OnEnable()
    {
        //���� PlayerInputControl
        inputControl.Enable();
        loadEvent.LoadRequestEvent +=OnloadEvent;
        afterSceneLoadEvent.OnEventRaised += OnafterSceneLoadEvent;
    }
    private void OnDisable()
    {
        //���� PlayerInputControl
        inputControl.Disable();
        loadEvent.LoadRequestEvent -= OnloadEvent;
        afterSceneLoadEvent.OnEventRaised -= OnafterSceneLoadEvent;
    }

   

    private void Update()
    {
        if (isDashing || isHurt) // ���isHurt���
            return;
        //Debug.Log(rb.velocity.y);
        if (isDashing)
            return; // ������ڳ�̣��򲻴�����������
        if (playerCheck.isGround&&isJump)
            jumpCounts = 2; // ����ڵ����ϣ�������Ծ����Ϊ2
        if(isWallJumping||playerCheck.onWall)
        {
            return; // �������ǽ����Ծ����ǽ�ϣ��򲻴�����������
        }

        if (playerCheck.onWall)
            jumpCounts = 2;

        //��ȡ�ƶ�����
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        InputCorrection(); // ����������������
        //CheckState(); // ������״̬�������������
    }
    private void FixedUpdate()
    {
        if (isDashing || isHurt||isAttack) // ���isHurt��� // ������ڳ�̣��򲻴�����������
            return;
       
        MovePlayer();
        Wallslide();
    }

    private void OnloadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable(); // �����������
    }

    private void OnafterSceneLoadEvent()
    {
        inputControl.Gameplay.Enable(); // �����������
    }

    private void MovePlayer()
    {
        if (!isWallJumping)
        //ʹ�����������ƶ����
        //ԭ����ˣ�һ��Vector2��ֵֻҪȥ��ȡ����Ӧ��x����y�������Ӧ��floatֵ
        { rb.velocity = new Vector2(inputDir.x * moveSpeed, rb.velocity.y); }
        //��ת��ҵĳ���
        if (inputDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // �����Ҳ�
        }
        else if (inputDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // �������
        }
    }
    //����Ĳ����� InputAction.CallbackContext
    //������������������¼�����������Ϣ�����Բ���Ҫ��update�����л�ȡ����
    private void Jump(InputAction.CallbackContext obj)
    {
        // �������Ƿ��ڵ�����
        if (playerCheck.isGround && jumpCounts == 2)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(playerJump()); // ������ԾЭ��
            jumpCounts--; // ������Ծ����
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
        else if (!playerCheck.isGround && jumpCounts == 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // ���ô�ֱ�ٶ�
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCounts--; // ������Ծ����
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
            rb.velocity = new Vector2(rb.velocity.x, 0); // ���ô�ֱ�ٶ�
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCounts--; // ������Ծ����
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }
    private IEnumerator playerJump()
    {
        isJump = false; // ������Ծ״̬Ϊflase
        yield return new WaitForSeconds(0.03f); // �ȴ�0.03��
        isJump = true; // ������Ծ״̬Ϊtrue
    }
    private void Dash(InputAction.CallbackContext context)
    {
        if(canDash)
        {
            canDash = false;
            isDashing = true;
            StartCoroutine(PlayerDash(dashTime, dashcoolTime)); // �������Э��
        }
    }
    private IEnumerator PlayerDash(float dashTime,float dashCoolTime)
    {
        float originalGravity=rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity=new Vector2(transform.localScale.x*dashForce, 0);

        yield return new WaitForSeconds(dashTime); // �ȴ����ʱ��

        rb.velocity= Vector2.zero; // ֹͣ���
        rb.gravityScale = originalGravity; // �ָ�����
        isDashing = false; // ���ó��״̬Ϊfalse

        yield return new WaitForSeconds(dashCoolTime); // �ȴ������ȴʱ��

        canDash = true; // ���ÿ��Գ��
    }
  

    private void Wallsliding(InputAction.CallbackContext context)
    {
        isWallsliding = true;
    }
    private void CancelWallsliding(InputAction.CallbackContext context)
    {
        isWallsliding = false; // ��������״̬Ϊfalse
        rb.velocity=new Vector2(rb.velocity.x,rb.velocity.y); 
    }
    private void Wallslide()
    {
        if(playerCheck.onWall &&  !isWallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y /10 ); // ʹ�����ǽ�ϻ���
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
    // ������������������뷽��ĽǶ�
    private void InputCorrection()
    {
        // �������뷽��ĽǶ�
        // ʹ�� Mathf.Atan2 ����Ƕȣ�������ת��Ϊ����
        float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
        if(angle < 0)
        {
            // ����Ƕ�С��0������ת��Ϊ���Ƕ�
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
            inputDir = Vector2.zero; // ���û��ƥ�䵽�κη���������Ϊ������
        }
    }
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log(collision.name);
    //}
    public void GetHurt(Transform attacker)
    {
        isHurt = true; // ��������״̬Ϊtrue
        //rb.velocity=new Vector2.zero; �����벻����new�ؼ��֣���Ϊ Vector2.zero ��һ����̬����
        //����������� Vector2.zero �Ѿ���һ����̬���ԣ�Ԥ����ĳ�������������һ������,
        //���Բ����� new ����������ʵ����
        //�� C# �У�new �ؼ������ڴ���һ�����͵���ʵ�����������һ���������ơ�
        //Vector2.zero �� Unity �ṩ��һ����ݷ�ʽ����ͬ�� new Vector2(0, 0)
        rb.velocity = Vector2.zero; // ������ҵ��ٶ�
        Vector2 dir=new Vector2(transform.position.x - attacker.position.x,0 ).normalized;
        // �������˷���
        rb.AddForce(dir * hurtFoorce, ForceMode2D.Impulse); // ���������
    }
    public void Die()
    {
        isDead = true; // ��������״̬Ϊtrue
        inputControl.Gameplay.Disable(); // �����������

    }
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayerAttack(); // ������Ҷ����Ĺ�������
        isAttack=true; // ���ù���״̬Ϊtrue
    }
    private void CheckState()
    {
        capsuleCollider2D.sharedMaterial = playerCheck.isGround ? normal : wall;
    }
         
}