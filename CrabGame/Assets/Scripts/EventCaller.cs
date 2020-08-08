using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCaller : MonoBehaviour
{
    private PlayerActions playerActions;

    // Start is called before the first frame update
    void Start()
    {
        playerActions = GetComponentInParent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLeftPunchFalse()
    {
        playerActions.SetAnimations("left",false);
    }

    public void SetRightPunchFalse()
    {
        playerActions.SetAnimations("right", false);
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
