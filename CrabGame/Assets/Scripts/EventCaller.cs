using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCaller : MonoBehaviour
{
    private PlayerActions playerActions;
    private EnemyActions2 enemyActions;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerActions = GetComponentInParent<PlayerActions>();
        }
        else
        {
            enemyActions = GetComponentInParent<EnemyActions2>();
        }
        
    }

    public void DestroyHitBox()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerActions.DestroyHitBox();
        }
        else
        {
            enemyActions.DestroyHitBox();
        }
    }

    public void SetLeftPunchFalse()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerActions.SetPunchAnim("IsAttack_LeftClaw", false);
        }
        else
        {
            enemyActions.SetPunchAnim("IsAttack_LeftClaw", false);
        }
        DestroyHitBox();
    }

    public void SetRightPunchFalse()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerActions.SetPunchAnim("IsAttack_RightClaw", false);
        }
        else
        {
            enemyActions.SetPunchAnim("IsAttack_RightClaw", false);
        }
        DestroyHitBox();
    }

    public void PauseBlock()
    {
        if (gameObject.CompareTag("Player"))
            playerActions.SetAnimatorSpeed(0f);
        else
            enemyActions.SetAnimatorSpeed(0f);
    }

    public void SetBlockFalse()
    {
        if (gameObject.CompareTag("Player"))
            playerActions.SetAnimations("block", false);
        else
            enemyActions.SetAnimations("block", false);
    }
}
