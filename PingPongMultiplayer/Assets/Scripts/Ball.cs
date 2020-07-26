using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    public PhotonView photonView;

    private bool isMoving = true;
    private RoomManager roomManager;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        roomManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<RoomManager>();
        
        //Set random direction
        direction = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-1f, 1f), 0).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isMoving)
                transform.position += (direction * speed) * Time.deltaTime;

            //Side collision
            if (transform.position.x < -5f || transform.position.x > 5f)
            {
                Bounce(Vector3.right);
            }

            //Bottom collision
            else if (transform.position.y < -5)
            {
                isMoving = false;
                transform.position = Vector3.zero;
                Invoke("ResetBall", 0.7f);
                photonView.RPC("UpScore", RpcTarget.All);
            }

            //Top collision
            else if (transform.position.y > 5)
            {
                isMoving = false;
                transform.position = Vector3.zero;
                Invoke("ResetBall", 0.7f);
                photonView.RPC("DownScore", RpcTarget.All);
            }
        }
    }

    void ResetBall()
    {
        isMoving = true;
        direction = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-1f, 1f), 0).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Bounce(collision.GetContact(0).normal);
        }
    }
    
    void Bounce(Vector3 surfaceNormal)
    {
        direction = Vector3.Reflect(direction, surfaceNormal);
    }

    [PunRPC]
    void UpScore()
    {
        roomManager.upScore++;
    }

    [PunRPC]
    void DownScore()
    {
        roomManager.downScore++;
    }
}
