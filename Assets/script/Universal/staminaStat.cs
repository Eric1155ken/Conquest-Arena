using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staminaStat : MonoBehaviourPun
{
    public int maxStamina = 60;
    int staminaRecoveryAmount = 5;
    float staminaRecoveryInterval = 3f;
    public int currentStamina;

    public StaminaUI staminaUI;

    private void Awake()
    {
        currentStamina = maxStamina;
    }
    void Start()
    {
        staminaUI.StartSlider(currentStamina);

        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }
        StartCoroutine(RecoverStamina());
    }

    private IEnumerator RecoverStamina()
    {
        while (true)
        {
            yield return new WaitForSeconds(staminaRecoveryInterval);

            currentStamina += staminaRecoveryAmount;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); //Debug.Log(currentStamina);

            staminaUI.Update3DSlider(currentStamina);
            staminaUI.Update2Dslider(currentStamina);
        }
    }

    public void deductStamina(int deductAmount)
    {
        currentStamina -= deductAmount;
        staminaUI.Update3DSlider(currentStamina);
        staminaUI.Update2Dslider(currentStamina);
    }
}
