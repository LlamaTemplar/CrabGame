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

    public void SetLeftPunchFalse()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerActions.SetAnimations("left",false);
            playerActions.DestroyHitBox();
        }
        else
        {
            enemyActions.SetAnimations("left", false);
            enemyActions.DestroyHitBox();
        }
    }

    public void SetRightPunchFalse()
    {
        if (gameObject.CompareTag("Player"))
        {
            playerActions.SetAnimations("right", false);
            playerActions.DestroyHitBox();
        }
        else
        {
            enemyActions.SetAnimations("right", false);
            enemyActions.DestroyHitBox();
        }
    }

    public void PauseBlock()
    {
        playerActions.SetAnimatorSpeed(0f);
    }

    public void SetBlockFalse()
    {
        playerActions.SetAnimations("block", false);
    }
}
