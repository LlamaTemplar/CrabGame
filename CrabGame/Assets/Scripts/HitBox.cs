using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{

    private float hitRadius = 1f;
    private LayerMask targetMask;
    private bool isInitialized = false;
    private int damage;

    private List<Unit> hitList = new List<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var hits = GetHits();
        if (isInitialized && hits.Length > 0)
        {
            DealDamage(hits);
        }
    
    }

    public void InitializeHitBox(int damage, LayerMask mask)
    {
        isInitialized = true;
        this.damage = damage;
        targetMask = mask;
    }

    public Collider2D[] GetHits()
    {
        return Physics2D.OverlapBoxAll(transform.position, new Vector2(1,3),transform.rotation.eulerAngles.z, targetMask);
    }

    public void DealDamage(Collider2D[] hits)
    {
        foreach(var hit in hits)
        {
            Unit unit = hit.GetComponentInParent<Unit>();

            // check if unit has already been hit
            if (hitList.Contains(unit)) 
                continue;
            else
                hitList.Add(unit);

            unit.TakeDamage(damage);
        }
    }


}
