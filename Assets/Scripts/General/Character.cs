using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("�¼�����")]
    public VoidEventSO newGameEvent;
    public UnityEvent<Character> onHealthChange;
    [Header("��������")]
    public float currentHealth;
    public float maxHealth;

    [Header("�����޵�")]
    public float inVulnerableDuration;
    //�޵�ʱ��
    //ͨ������inVulnerableDuration��ֵ�����Կ��ƽ�ɫ���˺���޵�ʱ�䳤��

    private float inVulnerableCounter;
    [Header("��ʱ��")]
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
    //ʹ��UnityEvent�����������¼���������Inspector�а������ű�����Ӧ����
    //�������������ʱ������Ч��������Ч��
    //������Transform��������������Ӧ�����л�ȡ���˵Ľ�ɫ����
    //��Ҫ����һ�²�����UnityEvent��������
    public UnityEvent onDie;
  
    public void takeDamage(Attack attacker)//�˴����κ����ô�������ʱ���ù��ض�Ӧ�ű��Ķ�Ӧ�����Ѫ
    {
        if (inVulnerable)
            return;

        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            OnTakeDamage?.Invoke(attacker.transform);
            //���������¼�
            //?.�����������ж�OnTakeDamage�Ƿ�Ϊnull�������Ϊnull�����Invoke����
        }
        else
        {
            currentHealth = 0;
            //�����Ѫ��С��0,����
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
            //���ӿռ�飬�����ظ������޵�״̬�����ڶ����ĵ��ú���Ҫ
            if (playerAnimation != null)
                playerAnimation.PlayerInvulnerable();
        }
    }
}
