using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    private AudioSource audioSource;
    private const int maxHp = 20;
    private int hp = maxHp;

    private double startX = 0;
    private double endX = 0;

    private bool isRight = true;

    public AudioClip deathSound;

    // Start is called before the first frame update

    Animator anim;
    Rigidbody2D rb;

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Player"){
            bool isAttack = collision.gameObject.GetComponent<PlayerController>().isAttack;
            if(isAttack && hp > 0){
                anim.SetTrigger("hurting");
                audioSource.Play();
                rb.AddForce(new Vector2(1.0f, 0.5f) * 0.25f, ForceMode2D.Impulse);
                hp -= 5;
                if(hp <= 0){
                    anim.SetTrigger("death");
                    audioSource.clip = deathSound;
                    audioSource.Play();
                    collision.gameObject.GetComponent<PlayerController>().addScore();
                    Destroy(this.gameObject, 1.5f);
                }
                Debug.Log("hp Slime: " + hp);
            }else if(hp > 0){
                collision.gameObject.GetComponent<PlayerController>().damage(5);
                audioSource.Play();
                rb.AddForce(new Vector2(-1.0f, 0.5f) * 0.25f, ForceMode2D.Impulse);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            if(other.GetComponent<Transform>().position.x < this.transform.position.x){
                anim.SetTrigger("attacking");
                this.transform.localRotation = Quaternion.Euler(0, 180, 0);
                rb.AddForce(new Vector2(-2.0f, 2.0f) * 1.0f, ForceMode2D.Impulse);
            }else{
                anim.SetTrigger("attacking");
                isRight = true;
                this.transform.localRotation = Quaternion.Euler(0, 0, 0);
                rb.AddForce(new Vector2(2.0f, 2.0f) * 1.0f, ForceMode2D.Impulse);
            }
        }
        
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        startX = transform.position.x - 2;
        endX = transform.position.x + 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > endX || transform.position.x < startX){
            isRight = !isRight;
        }
        if(hp > 0){
            this.transform.Translate(new Vector2((isRight ? 0.5f : -0.5f) * Time.deltaTime, 0), 0);
        }
        
    }

}
