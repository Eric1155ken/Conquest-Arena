//Attribution Declaration: The script is adapted from this tutorial: 
//    https://www.youtube.com/watch?v=Dj_pSEDxs8o&list=PLuKMRhgr5rGkgABx8Sezws-ekSWIWRf4Q&index=7

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthUI : MonoBehaviourPunCallbacks, IPunObservable
{
    public Slider healthSlider3D;
    Slider healthSlider2D;

    private void Awake()
    {
        healthSlider2D = GameObject.Find("HealthBar2D").GetComponent<Slider>();
    }
    public void StartSlider(float maxValue)
    {
        healthSlider3D.maxValue = maxValue;
        healthSlider3D.value = maxValue;

        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        if (healthSlider2D == null)
        {
            healthSlider2D = GameObject.Find("HealthBar2D").GetComponent<Slider>();
        }
        healthSlider2D.maxValue = maxValue;
        healthSlider2D.value = maxValue;
    }

    // Update is called once per frame
    public void Update3DSlider(float value)
    {
        healthSlider3D.value = value;
    }

    public void Update2Dslider(float value)
    {
        healthSlider2D.value = value;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(healthSlider3D.value);
        }
        else
        {
            float receivedValue = (float)stream.ReceiveNext();
            Update3DSlider(receivedValue);
        }
    }
}
