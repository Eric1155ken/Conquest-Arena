using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class leaveButtonEvent : MonoBehaviour
{
    public void Leave()
    {
        SceneManager.LoadScene("Connect");
    }
}
