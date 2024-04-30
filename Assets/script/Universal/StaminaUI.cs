using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviourPunCallbacks, IPunObservable
{
    public Slider staminaSlider3D;
    Slider staminaSlider2D;

    private void Awake()
    {
        staminaSlider2D = GameObject.Find("StaminaBar2D").GetComponent<Slider>();
    }
    public void StartSlider(float maxValue)
    {
        staminaSlider3D.maxValue = maxValue;
        staminaSlider3D.value = maxValue;

        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        if (staminaSlider2D == null)
        {
            staminaSlider2D = GameObject.Find("StaminaBar2D").GetComponent<Slider>();
        }
        staminaSlider2D.maxValue = maxValue;
        staminaSlider2D.value = maxValue;
    }

    public void Update3DSlider(float value)
    {
        staminaSlider3D.value = value;
    }

    public void Update2Dslider(float value)
    {
        staminaSlider2D.value = value;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(staminaSlider3D.value);
        }
        else
        {
            float receivedValue = (float)stream.ReceiveNext();
            staminaSlider3D.value = receivedValue;
        }
    }
}
