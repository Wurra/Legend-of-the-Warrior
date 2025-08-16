using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsCheck : MonoBehaviour
{
    public float checkRadius;
    public PlayerController playerController; // 引用 PlayerController 脚本
    public Rigidbody2D rb;
    public Vector2 buttomoffest;
    public Vector2 leftWallOffset;
    public Vector2 rightWallOffset;
    public LayerMask groundLayer;

    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    public bool onWall;

    private void FixedUpdate()
    {
        Check();
    }
    private void Check()
    {
        // 检测地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(buttomoffest.x * transform.localScale.x, buttomoffest.y * transform.localScale.y), checkRadius, groundLayer);
        
        // 根据角色朝向确定实际的左右墙检测
        if (transform.localScale.x > 0) // 角色面向右侧
        {
            // 正常使用左右墙偏移
            touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftWallOffset.x, leftWallOffset.y), checkRadius, groundLayer);
            touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightWallOffset.x, rightWallOffset.y), checkRadius, groundLayer);
        }
        else // 角色面向左侧
        {
            // 交换左右墙偏移
            touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(-rightWallOffset.x, rightWallOffset.y), checkRadius, groundLayer);
            touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(-leftWallOffset.x, leftWallOffset.y), checkRadius, groundLayer);
        }
        
        if (playerController != null)
        {
            onWall = (touchLeftWall || touchRightWall) && playerController.isWallsliding && rb.velocity.y <= 0;
        }
    }
    // 同样修改Gizmos绘制方法
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(buttomoffest.x * transform.localScale.x, buttomoffest.y * transform.localScale.y), checkRadius);
        
        Gizmos.color = Color.blue;
        if (transform.localScale.x > 0)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftWallOffset.x, leftWallOffset.y), checkRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightWallOffset.x, rightWallOffset.y), checkRadius);
        }
        else
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(-rightWallOffset.x, rightWallOffset.y), checkRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(-leftWallOffset.x, leftWallOffset.y), checkRadius);
        }
    }
}
