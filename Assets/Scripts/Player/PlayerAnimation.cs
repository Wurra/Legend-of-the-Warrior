using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
   private Animator anim;
   private Rigidbody2D rb;
   physicsCheck physicscheck;
    private PlayerController playerController;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicscheck= GetComponent<physicsCheck>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        setAnimation();
    }
    public void setAnimation()
    {
        // 设置动画参数
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", physicscheck.isGround);
        anim.SetBool("isDead",playerController.isDead);
        anim.SetBool("isAttack", playerController.isAttack);
    }
    public void Playerhurt()
    {
        anim.SetBool("hurt", true);
    }
    public void PlayerInvulnerable()
    {
        anim.SetBool("Invulnerable", true);
        // 取消受伤状态
        // 这里可以添加其他逻辑，比如播放受伤特效等
    }
    public void PlayerAttack()
    {
        anim.SetTrigger("attack");
    }
}
 