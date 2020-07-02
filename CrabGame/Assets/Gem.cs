﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var contact = collision.collider.tag;

        print(contact);

        if(contact == "Player")
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
