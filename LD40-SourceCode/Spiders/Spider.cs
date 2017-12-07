using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public vp_FPPlayerDamageHandler damageHandler;
    public AudioSource sound;
    public List<AudioClip> hitClips;
    public List<AudioClip> attackClips;
    public List<AudioClip> angryClips;
    public List<AudioClip> deathClips;
    public float health = 5f;
    public float hitRange = 3f;
    public bool Alive
    {
        get
        {
            return alive;
        }
    }
    bool alive, attacking;
    Animator anim;
    float velocity, currentTime, timeTilNextSound;
    Vector3 previous;

    void OnEnable()
    {
        damageHandler = GameController.Instance.Player.gameObject.GetComponent<vp_FPPlayerDamageHandler>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        alive = true;
        attacking = false;

        sound.clip = angryClips[Random.Range(0, angryClips.Count)];
        sound.Play();

        timeTilNextSound = Random.Range(15f, 30f);
    }

    void Update()
    {
        if (!alive) return;

        currentTime += Time.deltaTime;
        if (currentTime > timeTilNextSound)
        {
            timeTilNextSound = Random.Range(15f, 30f);
            currentTime = 0f;
            if (!sound.isPlaying)
            {
                sound.clip = angryClips[Random.Range(0, angryClips.Count)];
                sound.Play();
            }
        }

        velocity = ((transform.position - previous).normalized.magnitude) / Time.deltaTime;
        previous = transform.position;

        anim.SetFloat("Speed", velocity);

        float distance = Vector3.Distance(GameController.Instance.Player.position, transform.position);
        if (distance < hitRange && !attacking)
        {
            StartCoroutine(Attack());
        }
    }

    public void Hit()
    {
        health -= 1f;
        sound.clip = hitClips[Random.Range(0, hitClips.Count)];
        sound.Play();

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        yield return new WaitForSeconds(1.25f);

        if (alive)
        {
            sound.clip = attackClips[Random.Range(0, attackClips.Count)];
            sound.Play();

            float distance = Vector3.Distance(GameController.Instance.Player.position, transform.position);
            if (distance < (hitRange * 2))
            { 
                damageHandler.Damage(1f);
            }
        }

        attacking = false;
        anim.SetBool("Attacking", false);
    }

    void Die()
    {
        alive = false;
        anim.SetBool("Dead", true);
        sound.clip = deathClips[Random.Range(0, deathClips.Count)];
        sound.Play();
        GameController.Instance.RemoveSpider(this);
        StartCoroutine(DestroyAfterTime(30f));
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
