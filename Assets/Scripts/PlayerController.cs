using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private const int maxHp = 50;
    private int hp = maxHp;
    private int score = 0;

    public AudioClip slash;
    public AudioClip footstep;
    private AudioSource audioSource;
    
    public GameObject obj;
    private Animator animator;
    private BoxCollider2D boxCollider;
    Vector2 jump;
    float jumpForce = 3.0f;
    Rigidbody2D rb = null;

    private bool invincibility = false;

    private bool isMoving = false;
    public bool isAttack = false;

    private int side = 0;

   // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jump = new Vector2(0.0f, 2.0f);
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision){

        if(collision.collider.tag == "Terrain"){
            animator.SetBool("Grounded", true);
            animator.SetBool("Jump", false);
            animator.SetBool("sliding", false);
        }

        if(collision.collider.tag == "Death"){
            SceneManager.LoadScene(2);
        }
    }

    void OnCollisionStay2D(Collision2D other){
        if(other.gameObject.tag == "Wall"){
            if(animator.GetInteger("AirSpeedY") == 0){
                animator.SetBool("sliding", true);
            }
        }

        
    }

    void OnCollisionLeave2D(Collision2D collision){
        if(collision.collider.tag == "Wall"){
            if(animator.GetInteger("AirSpeedY") == 0){
                animator.SetInteger("AirSpeedY", -1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        obj.transform.localRotation = Quaternion.Euler(0, side, 0);

        if(Input.GetKey(KeyCode.LeftArrow)){
            isMoving = true;
            side = 180;
            animator.SetInteger("AnimState", 1);
            obj.transform.Translate(new Vector2(-2 * Time.deltaTime, 0), 0);
        }else if(Input.GetKey(KeyCode.RightArrow)){
            isMoving = true;
            side = 0;
            animator.SetInteger("AnimState", 1);
            obj.transform.Translate(new Vector2(2 * Time.deltaTime, 0), 0);
            
        }else{
            animator.SetInteger("AnimState", 0);
        }

        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)){
            isMoving = false;
        }

        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)){
            audioSource.clip = footstep;
            audioSource.Play();
        }


        if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)){
            audioSource.Stop();
        }

        if(Input.GetKeyDown(KeyCode.Space) && animator.GetBool("Grounded")){
            animator.SetTrigger("Roll");
            StartCoroutine(rollInvincibility());
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && (animator.GetBool("Grounded") || animator.GetBool("sliding"))){
            animator.SetBool("Grounded", false);
            
            StartCoroutine(stopTime());
            
            rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
        }

        if(Input.GetKey(KeyCode.D) && isMoving == false){
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }

        if(Input.GetKeyUp(KeyCode.D) && isMoving == false){
            animator.SetBool("IdleBlock", false);
        }
        
        if(Input.GetKeyDown(KeyCode.A) && animator.GetInteger("AnimState") == 0 && animator.GetBool("Roll") == false && animator.GetInteger("followAttack") == 0 && isAttack == false){
            animator.SetBool("Attack1", true);
            audioSource.clip = slash;
            audioSource.Play();
            isAttack = true;
            StartCoroutine(temp(animator.GetInteger("followAttack")));
        }

        if(Input.GetKeyDown(KeyCode.A) && animator.GetInteger("followAttack") == 1 && isAttack == false){
            animator.SetBool("Attack2", true);
            audioSource.clip = slash;
            audioSource.Play();
            isAttack = true;
            StartCoroutine(temp(animator.GetInteger("followAttack")));
        }

        if(Input.GetKeyDown(KeyCode.A) && animator.GetInteger("followAttack") == 2){
            animator.SetBool("Attack3", true);
            audioSource.clip = slash;
            audioSource.Play();
            isAttack = true;
            StartCoroutine(finish());
        }

        if(isAttack == true){
            boxCollider.size = new Vector2(2.5f, boxCollider.size.y);
        }else{
            boxCollider.size = new Vector2(1, boxCollider.size.y);
        }
    }

    IEnumerator temp(int currentFollow){
        yield return new WaitForSeconds(0.25f);
        animator.SetInteger("followAttack", animator.GetInteger("followAttack") + 1);
        isAttack = false;
        yield return new WaitForSeconds(2f);
        if(animator.GetInteger("followAttack") < currentFollow + 2){
            animator.SetInteger("followAttack", 0);
        }else{
            StopCoroutine("temp");
        }
    }

    IEnumerator finish(){
        yield return new WaitForSeconds(0.25f);
        isAttack = false;
        animator.SetInteger("followAttack", 0);
    }

    IEnumerator rollInvincibility(){
        Debug.Log("Rolling");
        invincibility = true;
        yield return new WaitForSeconds(1f);
        invincibility = false;
    }

    IEnumerator stopTime(){
        animator.SetFloat("AirSpeedY", 0);
        animator.SetBool("Jump", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetFloat("AirSpeedY", -1);
    }

    IEnumerator death(){
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
    }

    public void damage(int enemyDamage){
        if(invincibility == false){
            if(animator.GetBool("IdleBlock")){
                hp -= enemyDamage / 2;
            }
            else{
                hp -= enemyDamage;
            }
            if(hp > 0){
                animator.SetTrigger("Hurt");
            }else{
                animator.SetTrigger("Death");
                StartCoroutine(death());
            }
            rb.AddForce(new Vector2(-2.0f, 1f) * 1f, ForceMode2D.Impulse);
        }else{
            Debug.Log("E' invincibile");
        }
    }

    public int getHp(){
        return hp;
    }

    public void addScore(){
        score+=100;
    }

    public int getScore(){
        return score;
    }
}
