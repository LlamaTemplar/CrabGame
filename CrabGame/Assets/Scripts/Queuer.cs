using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queuer : MonoBehaviour
{
    private GameObject playerObject;
    private SoundPlayer soundPlayer;

    public bool isWalking = false;
    private Vector3 oldPos;
    public bool once = false;

    public bool isPunching = false;

    // Start is called before the first frame update
    void Start()
    {
        // Check if Player Object Exsists
        if (GameObject.Find("Player"))
        {
            // Too many objects with Tag "Player", so I changed it to the Name of the Object that has the move component.
            //GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerObject = GameObject.Find("Player");

            // Store Current Position of Player
            oldPos = playerObject.transform.position;
        }
        else
        {
            playerObject = null;
            print("Player Object Does not exsist in scene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject != null)
        {
            if (playerObject.GetComponent<SoundPlayer>() != null)
                soundPlayer = playerObject.GetComponent<SoundPlayer>();

            if (playerObject.GetComponent<MovePrototypeOne>() != null)
            {
                CheckIfWalking();
                UpdateOldPosition();
            }
        }


        PlayWalkingSound(isWalking);
    }

    private void CheckIfWalking()
    {
        if (oldPos != playerObject.transform.position)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateOldPosition()
    {
        oldPos = playerObject.transform.position;
    }

    private void PlayWalkingSound(bool canPlay)
    {
        if (canPlay)
        {
            if (once == false)
            {
                soundPlayer.PlaySound("Walking");
                once = true;
            }
        }
        else
        {
            if (once == true)
            {
                soundPlayer.StopSound("Walking");
                once = false;
            }
        }
    }

    public void PlayPunchingSound()
    {
        if (isPunching)
        {
            soundPlayer.StopSound("Punching");
            isPunching = false;
        }

        soundPlayer.PlaySound("Punching");
        isPunching = true;
    }
}
