using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsCheck : MonoBehaviour
{
    public float checkRadius;
    public PlayerController playerController; // ���� PlayerController �ű�
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
        // ������
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(buttomoffest.x * transform.localScale.x, buttomoffest.y * transform.localScale.y), checkRadius, groundLayer);
        
        // ���ݽ�ɫ����ȷ��ʵ�ʵ�����ǽ���
        if (transform.localScale.x > 0) // ��ɫ�����Ҳ�
        {
            // ����ʹ������ǽƫ��
            touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftWallOffset.x, leftWallOffset.y), checkRadius, groundLayer);
            touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightWallOffset.x, rightWallOffset.y), checkRadius, groundLayer);
        }
        else // ��ɫ�������
        {
            // ��������ǽƫ��
            touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(-rightWallOffset.x, rightWallOffset.y), checkRadius, groundLayer);
            touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(-leftWallOffset.x, leftWallOffset.y), checkRadius, groundLayer);
        }
        
        if (playerController != null)
        {
            onWall = (touchLeftWall || touchRightWall) && playerController.isWallsliding && rb.velocity.y <= 0;
        }
    }
    // ͬ���޸�Gizmos���Ʒ���
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
