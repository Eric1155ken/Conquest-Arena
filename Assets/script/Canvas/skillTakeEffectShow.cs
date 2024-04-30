using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skillTakeEffectShow : MonoBehaviour
{
    [Header("Skill")]
    public Image Image3;
    public bool isLaunching = false;
    public float lengthOfTime;

    void Start()
    {
        Image3.fillAmount = 1;
    }

    private void Update()
    {
        if (isLaunching)
        {
            SkillLaunchingUI();
        }
    }



    public void StartLaunching(float duration)
    {
        isLaunching = true;
        lengthOfTime = duration;
    }

    public void SkillLaunchingUI()
    {
        Image3.fillAmount -= 1 / lengthOfTime * Time.deltaTime;

        if (Image3.fillAmount <= 0)
        {
            Image3.fillAmount = 1;
            isLaunching = false;
        }
    }
}
