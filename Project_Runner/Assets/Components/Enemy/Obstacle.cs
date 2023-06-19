using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;

    float timer = 5;
    float currentTimer;

    bool canAct;

    private void Awake()
    {
        canAct = true;
        GameHandler.instance.EventControlEverything += Control;
    }

    void Control(bool choice)
    {
        canAct = choice;
    }

    

    private void Update()
    {
        if (!canAct) return;

        if(currentTimer > timer)
        {
            Destroy(gameObject);
        }
        else
        {
            currentTimer += Time.deltaTime;
        }


        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the playere ever touches this then we do something.
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable == null) return;


        damageable.TakeDamage(damage);
        Destroy(gameObject);
    }
}

