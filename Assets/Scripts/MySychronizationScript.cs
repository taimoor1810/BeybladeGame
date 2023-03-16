using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySychronizationScript : MonoBehaviour, IPunObservable
{
    Rigidbody rb;
    PhotonView photonView;
    Vector3 networkedPosition;
    Quaternion networkedRotaion;
    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;
    private float distance;
    private float angle;


    private void Awake() {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        networkedPosition = new Vector3();
        networkedRotaion = new Quaternion();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if(!photonView.IsMine)
        {
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, distance*(1.0f/PhotonNetwork.SerializationRate));
        rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotaion, angle*(1.0f/PhotonNetwork.SerializationRate));
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //This Photoview is mine and I control this player
            //Should send position, velocity etc data to the other players
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);

            if(synchronizeVelocity)
            {
                stream.SendNext(rb.velocity);
            }

            if(synchronizeAngularVelocity)
            {
                stream.SendNext(rb.angularVelocity);
            }
        }

        else
        {
            //Called on my player gameobject that exists in remote players game
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotaion = (Quaternion)stream.ReceiveNext();

            if(isTeleportEnabled)
            {
                if(Vector3.Distance(rb.position, networkedPosition) > teleportIfDistanceGreaterThan)
                {
                    rb.position = networkedPosition;
                }
            }

            if(synchronizeVelocity || synchronizeAngularVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if(synchronizeVelocity)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();
                    networkedPosition += rb.velocity * lag;
                    distance = Vector3.Distance(rb.position, networkedPosition);
                }

                if(synchronizeAngularVelocity)
                {
                     rb.angularVelocity = (Vector3)stream.ReceiveNext();
                    networkedRotaion = Quaternion.Euler(rb.angularVelocity*lag)*networkedRotaion;
                    angle = Quaternion.Angle(rb.rotation, networkedRotaion);
                }
            }
        }
    }
}
