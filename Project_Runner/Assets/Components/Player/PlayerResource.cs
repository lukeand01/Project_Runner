using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerResource : MonoBehaviour, IDamageable
{
    PlayerHandler handler;

    [SerializeField] GameObject effectHitTemplate;
    [SerializeField] GameObject effectDeathTemplate;

    [SerializeField] float initialHealth;
    float currentHealth;
    float maxHealth;
    bool isDead;


    Rigidbody2D rb;

    Quaternion originalRotation;
    Vector3 originalPos;

    [Separator("Audio")]
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip deathClip;

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        maxHealth = initialHealth;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        originalPos = transform.position;
        originalRotation = transform.rotation;
    }

    public void ResetResource()
    {

        isDead = false;
        currentHealth = maxHealth;
        UIHolder.instance.player.UpdateHealth((int)currentHealth, (int)maxHealth);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.position = originalPos;
        transform.rotation = originalRotation;

    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UIHolder.instance.player.UpdateHealth((int)currentHealth, (int)maxHealth);
        SpawnEffect(effectHitTemplate);



        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        GameHandler.instance.CreateSFX(hitClip);
        handler.cam.CameraHitEffect();
    }

    public void HealHealth(int heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UIHolder.instance.player.UpdateHealth((int)currentHealth, (int)maxHealth);
    }

    void Die()
    {
        //stop everything.
        //call the ui.
        GameHandler.instance.StartBackgroundMusic(2);
        GameHandler.instance.StopGame();
        SpawnEffect(effectDeathTemplate);
        GameHandler.instance.CreateSFX(deathClip);
        handler.cam.CameraDeathEffect();
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        

        while (handler.cam.isPlayingEffect)
        {
            yield return null;
        }
        rb.constraints = RigidbodyConstraints2D.None;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(new Vector2(-0.5f, 0.8f) , ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);

        UIHolder.instance.death.StartDeathUI();

    }

    //drop 



    void SpawnEffect(GameObject effect)
    {
       GameObject newObject = Instantiate(effect, transform.position, Quaternion.identity);
        newObject.AddComponent<KillSelf>().SetUp(5);

    }
   
}
