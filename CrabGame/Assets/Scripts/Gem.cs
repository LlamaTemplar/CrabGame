using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int healingNum = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var contact = collision.gameObject.tag;

        if(contact == "Player")
        {
			ScoreManager gemManager = FindObjectOfType<ScoreManager>();
			gemManager.RemoveAlgae(1);
            var player = collision.gameObject.GetComponentInParent<Player>();
            player.PlayEatSound();
            player.Heal(healingNum);
			Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        transform.right = Camera.main.transform.right;
    }
}
