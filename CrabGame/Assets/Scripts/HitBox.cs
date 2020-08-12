using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{

    private float hitRadius = 1f;
    private LayerMask targetMask;
    private bool isInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(isInitialized && GetHits().Length > 0)
        {
            print("HIT");
        }
    
    }

    public void InitializeHitBox(LayerMask mask)
    {
        isInitialized = true;
        targetMask = mask;
    }

    public Collider2D[] GetHits()
    {
        return Physics2D.OverlapCircleAll(transform.position, hitRadius, targetMask);
    }

}
