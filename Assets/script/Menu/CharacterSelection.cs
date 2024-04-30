using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characterList;
    public int characterSelectionIndex;

    public TMP_Text SelectAndReadyButtonText;

    // Start is called before the first frame update
    void Start()
    {
        characterSelectionIndex = 0;
        activateCharacter(characterSelectionIndex);

    }

    private void activateCharacter(int x)
    {

        foreach (GameObject characterSelection in characterList)
        {
            characterSelection.SetActive(false);
        }

        characterList[x].SetActive(true);
    }

    public void OnNextButtonClicked()
    {
        characterSelectionIndex += 1;
        if (characterSelectionIndex >= characterList.Length)
        {
            characterSelectionIndex = 0;
        }
        activateCharacter(characterSelectionIndex);
    }

    public void OnBackButtonClicked()
    {
        characterSelectionIndex -= 1;
        if (characterSelectionIndex < 0)
        {
            characterSelectionIndex = characterList.Length - 1;
        }
        activateCharacter(characterSelectionIndex);
    }

    public void OnSelectAndReadyButtonClicked()
    {
        SelectAndReadyButtonText.text = "Waiting for others...";

        Hashtable characterSelectionProp = new() { { HashTableKey.CHARACTER_SELECTION, characterSelectionIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(characterSelectionProp);

        Hashtable readyStateProp = new() { { HashTableKey.READY_STATE, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(readyStateProp);
    }
}