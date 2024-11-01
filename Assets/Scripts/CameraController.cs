using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    public float offset;
    public float offsetSmoothing;
    private Vector3 playerPosition;



    private void Update()
    {
        playerPosition = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);

        if(Player.transform.localScale.x > 0f )
        {
            playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y, playerPosition.z);
        }
        else
        {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, playerPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }




}
