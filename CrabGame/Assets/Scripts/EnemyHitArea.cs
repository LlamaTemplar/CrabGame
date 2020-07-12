using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitArea : MonoBehaviour
{
    private EnemyActions enemyActions;

    // Start is called before the first frame update
    void Start()
    {
        enemyActions = gameObject.GetComponentInParent<EnemyActions>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        //print("here");
        if (col.gameObject.CompareTag("Player"))
        {
            enemyActions.canAct = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        enemyActions.canAct = false;
    }
}
