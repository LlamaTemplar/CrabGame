using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var contact = collision.collider.tag;

        if(contact == "Player")
        {
			ScoreManager gemManager = FindObjectOfType<ScoreManager>();
			gemManager.AddGems(1);

			Destroy(gameObject);
        }
    }
}
