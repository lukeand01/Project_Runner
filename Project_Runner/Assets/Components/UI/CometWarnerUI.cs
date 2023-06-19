using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CometWarnerUI : MonoBehaviour
{
    [SerializeField] GameObject leftWarn;
    [SerializeField] GameObject rightWarn;
    [SerializeField] GameObject centerWarn;


    public void CometWarn(int choice)
    {
        //it just shines for a second.
        if(choice == -2)
        {
            leftWarn.SetActive(true);
            StartCoroutine(Process(leftWarn));
        }

        if (choice == 0)
        {
            centerWarn.SetActive(true);
            StartCoroutine(Process(centerWarn));
        }

        if(choice == 2)
        {
            rightWarn.SetActive(true);
            StartCoroutine(Process(rightWarn));
        }
    }

    IEnumerator Process(GameObject target)
    {
        yield return new WaitForSeconds(0.15f);
        target.SetActive(false);
    }

}
