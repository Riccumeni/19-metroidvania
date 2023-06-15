using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    private AudioSource audioSource;
    private const int maxHp = 20;
    private int hp = maxHp;

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
                    Destroy(this.gameObject, 1.5f);
                }
                Debug.Log("hp Slime: " + hp);
            }else if(hp > 0){
                collision.gameObject.GetComponent<PlayerController>().damage(5);
                audioSource.Play();
                rb.AddForce(new Vector2(-1.0f, 0.5f) * 0.25f, ForceMode2D.Impulse);
            }
        }

        if(collision.gameObject.tag == "Wall"){
            isRight = !isRight; 
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
    }

    // Update is called once per frame
    void Update()
    {
        if(hp > 0){
            if(isRight){
                this.transform.Translate(new Vector2(0.5f * Time.deltaTime, 0), 0);
            }else{
                this.transform.Translate(new Vector2(-0.5f * Time.deltaTime, 0), 0);
            }
        }
        
    }

}
