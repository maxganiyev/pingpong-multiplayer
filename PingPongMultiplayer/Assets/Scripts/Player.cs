using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject LocalPlayerInstance;
    public bool isBottom;

    private Camera camera;

    private Vector3 deltaPosition;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;

        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastPosition = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y, transform.position.z);
            }

            if (Input.GetMouseButton(0))
            {
                deltaPosition = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y, transform.position.z) - lastPosition;

                transform.position = new Vector3(transform.position.x + deltaPosition.x, transform.position.y, transform.position.z);

                lastPosition = new Vector3(camera.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y, transform.position.z);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
