using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileEffect2 : MonoBehaviourPun, IPunObservable
{
    public int damageAmount;
    public float impactForce;
    public float lifeTime = 5f;
    public float maxDistance;
    private Vector3 initialPosition;

    public int shooterPhotonViewID;

    void Start()
    {
        Invoke(nameof(selfDestroy), lifeTime);

        initialPosition = transform.position;
    }

    [PunRPC]
    public void SynchronizeVelocity(int photonViewID)
    {
        shooterPhotonViewID = photonViewID;
    }

    void selfDestroy()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }
        PhotonNetwork.Destroy(gameObject);
        //Destroy(gameObject);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, initialPosition);

        if (distance > maxDistance)
        {
            selfDestroy();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (collision.gameObject.GetPhotonView().ViewID == shooterPhotonViewID)
            {
                return;
            }

            Vector3 impactDirection = collision.contacts[0].point - transform.position;

            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            enemyRigidbody.AddForce(impactDirection * impactForce, ForceMode.Impulse);


            Stats enemyStats = collision.gameObject.GetComponent<Stats>();

            if (enemyStats != null)
            {
                enemyStats.TakeDamage(collision.gameObject, damageAmount);
            }


            //selfDestroy();
        }


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ;
    }
}
