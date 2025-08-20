using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("事件监听")]
    public VoidEventSO newGameEvent;
    public UnityEvent<Character> onHealthChange;
    [Header("基本属性")]
    public float currentHealth;
    public float maxHealth;

    [Header("受伤无敌")]
    public float inVulnerableDuration;
    //无敌时间
    //通过调整inVulnerableDuration的值，可以控制角色受伤后的无敌时间长短

    private float inVulnerableCounter;
    [Header("计时器")]
    public bool inVulnerable;

    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void NewGame()
    {

        currentHealth = maxHealth;
        onHealthChange?.Invoke(this);
    }
    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised-= NewGame;
    }
    private void Update()
    {
        if(inVulnerable)
        {
            inVulnerableCounter -= Time.deltaTime;
            if(inVulnerableCounter <= 0 )
            {
                inVulnerable = false;
            }

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Water"))
        {
            currentHealth = 0;
            onHealthChange?.Invoke(this);
            onDie?.Invoke();
        }
    }
    public UnityEvent<Transform> OnTakeDamage;
    //使用UnityEvent来处理受伤事件，可以在Inspector中绑定其他脚本的响应函数
    //例如可以在受伤时播放音效、触发特效等
    //传入了Transform参数，可以在响应函数中获取受伤的角色对象
    //需要启动一下才能让UnityEvent正常工作
    public UnityEvent onDie;
  
    public void takeDamage(Attack attacker)//此处传参函数好处：调用时能让挂载对应脚本的对应对象扣血
    {
        if (inVulnerable)
            return;

        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform);
            //触发受伤事件
            //?.操作符用于判断OnTakeDamage是否为null，如果不为null则调用Invoke方法
        }
        else
        {
            currentHealth = 0;
            //如果扣血后小于0,死亡
            onDie?.Invoke();
        }
        onHealthChange?.Invoke(this);
    }
    public void TriggerInvulnerable()
    {
        if(!inVulnerable)
        {
            inVulnerable = true;
            inVulnerableCounter = inVulnerableDuration;
            //增加空检查，避免重复触发无敌状态，对于动画的调用很重要
            if (playerAnimation != null)
                playerAnimation.PlayerInvulnerable();
        }
    }
}
