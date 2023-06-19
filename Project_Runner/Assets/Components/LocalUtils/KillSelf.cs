using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelf : MonoBehaviour
{
    [SerializeField] float total;
    float current;

    public void SetUp(float total)
    {
        this.total = total;
    }
    private void Update()
    {
        if(current > total)
        {
            Destroy(gameObject);
        }
        else
        {
            current += Time.deltaTime;
        }
    }
}
