using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private int startingHP = 20;
    public float currentHP;
    public bool loseArm = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = startingHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            loseArm = true;
        }
        else if (currentHP >= startingHP)
        {
            currentHP = startingHP;
            loseArm = false;
        }

        if (loseArm)
        {
            currentHP += Time.deltaTime;
        }
    }

    public void TakeDamamge(int dmg)
    {
        if (loseArm == false)
        {
            currentHP -= dmg;
        }
    }
}
