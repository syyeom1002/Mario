using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MarioController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip eatCoinSfx;
    [SerializeField]
    private AudioClip powerUpSfx;
    [SerializeField]
    private AudioClip portalSfx;
    [SerializeField]
    private AudioClip jumpSfx;
    private Rigidbody2D rBody2D;
    public float jumpForce = 500f;
    public float moveForce = 60f;
    public Animator anim;
    private bool isJump = false;
    private bool isLongJump = false;
    //[SerializeField]
    //private UIDirector uiDirector;
    [SerializeField]
    private Transform rayOrigin;    //체크 아래쪽 하기 위한 레이 발사 시작 위치 
    //private float scaleChange = 0;

    public System.Action<float> onGetMushroom; //대리자 변수 정의 
    public System.Action<float> onAttackedEnemy;
    public System.Action<string> onChangeScene;
    public System.Action onCoinEat;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
        this.rBody2D = this.GetComponent<Rigidbody2D>();
        this.anim = this.GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {

        //인풋이 막인 상태에서는 아무것도 못하게 막음 
        if (this.ignoreInput) return;
        
        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 이중점프 막기
            if (this.rBody2D.velocity.y == 0)
            {
                this.rBody2D.AddForce(Vector2.up * this.jumpForce);
                if (this.isJump == false)
                {
                    this.isJump = true;
                }
                this.audioSource.PlayOneShot(this.jumpSfx);
            }
        }
        if (this.isJump)
        {
            if (this.rBody2D.velocity.y > 0)
            {
                // Jumping
                this.anim.SetBool("isJump", true);
            }
            else if (this.rBody2D.velocity.y <= 0)
            {
                // Falling
                this.anim.SetBool("isJump", false);
            }
        }
        //if (Input.GetButtonUp("Horizontal"))
        //{
        //    this.rBody2D.velocity = new Vector2(this.rBody2D.velocity.normalized.x * 0.8f, this.rBody2D.velocity.y);
        //}
        //if (this.isJump)// space를 눌렀는데 
        //{
        //if (this.rBody2D.velocity.y > 0)// 점프상태
        //{
        // this.anim.SetInteger("State", 2);
        //}
        //}

        int dirX = 0;


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dirX = -1;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            dirX = 1;
        }
        float speedX = Mathf.Abs(this.rBody2D.velocity.x);//절대값(속도는 음수 나오면 안됨), 가속도

        if (speedX < 5f)//가속도 제한 
        {
            this.rBody2D.AddForce(new Vector2(dirX, 0) * this.moveForce);
        }

        //움직이는 방향으로 쳐다보기 
        if (dirX != 0)//달리고 있다면?
        {
            //if (!this.isJump)//좌우를 쳐다보는데 점프 상태가 아니라면 => 걷는중
            //{
                this.anim.SetInteger("State", 1);
                this.anim.SetBool("isJump", false);
            //}
            this.transform.localScale = new Vector3(dirX *Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, 1);
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8.1f, 25.9f),this.transform.position.y, 0);
        
        }
        else//좌우를 쳐다보지도 않고 점프 상태도 아니라면 =>idle
        {
            if (this.isJump==false)
            {
                this.anim.SetInteger("State", 0);
                this.anim.SetBool("isJump", false);
            }
        }
        this.anim.speed = speedX / 2.0f;

        //길게 누르면 높은 점프 짧게 누르면 낮은 점프
        if (Input.GetKey(KeyCode.Space))
        {
            isLongJump = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isLongJump = false;
        }



        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("아래 들어갈때가 있는지 확인한다");
            this.CheckPortalBelow();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.CheckPortalRight();
        }

    }

    private Coroutine routineCheckPortalBelow;
    private Coroutine routineCheckPortalRight;
    private void CheckPortalBelow()
    {
        if (this.routineCheckPortalBelow != null) StopCoroutine(this.routineCheckPortalBelow);
        this.routineCheckPortalBelow = this.StartCoroutine(this.CoCheckPortalBelow());
    }
    private void CheckPortalRight()
    {
        if (this.routineCheckPortalRight != null) StopCoroutine(this.routineCheckPortalRight);
        this.routineCheckPortalRight = this.StartCoroutine(this.CoCheckPortalRight());
    }

    private IEnumerator CoCheckPortalBelow()
    {
        yield return null;


        //레이를 만듬 
        Ray ray = new Ray(this.rayOrigin.transform.position, this.transform.up * -1);//this.transform.up*-1 = 아래로 레이만들기
        
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);
        

        LayerMask mask = 1 << LayerMask.NameToLayer("Mario");//마리오Layer만(6)
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1, ~mask);//~mask= 마리오 빼고 다
        
        if (hit)
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.name == "portal2")
            {
                Debug.Log("씬2로 전환");
                this.onChangeScene("Map2Scene");
                this.audioSource.PlayOneShot(this.portalSfx);
            }
        }
        

    }
    private IEnumerator CoCheckPortalRight()
    {
        yield return null;

        Ray ray2 = new Ray(this.rayOrigin.transform.position, this.transform.right * -1);
        Debug.DrawRay(ray2.origin, ray2.direction, Color.green, 12f);
        LayerMask mask = 1 << LayerMask.NameToLayer("Mario");//마리오Layer만(6)
        RaycastHit2D hit2 = Physics2D.Raycast(ray2.origin, ray2.direction, 1, ~mask);

        if (hit2)
        {
            Debug.Log(hit2.collider.gameObject.name);
            if (hit2.collider.gameObject.name == "portal3")
            {
                Debug.Log("씬3으로전환");
                this.onChangeScene("Map3Scene");
                this.audioSource.PlayOneShot(this.portalSfx);
            }
            
        }

    }
    private void FixedUpdate()
    {
        //인풋이 막인 상태에서는 아무것도 안함 
        if (this.ignoreInput) return;

        if (isLongJump && this.rBody2D.velocity.y > 0)
        {
            this.rBody2D.gravityScale = 1.2f;
        }
        else
        {
            this.rBody2D.gravityScale = 2.5f;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "flag")
        { 
            this.anim.SetInteger("State", 3);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "coin")
        {
            Destroy(other.gameObject);
            this.onCoinEat();
            this.audioSource.PlayOneShot(this.eatCoinSfx);
        }
            //this.uiDirector.IncreaseScore(400);

        
        //this.uiDirector.UpdateUI();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "mushroom")
        {
            Destroy(collision.gameObject);  //버섯 제거 
            this.audioSource.PlayOneShot(this.powerUpSfx);
            this.onGetMushroom(0.6f);    //대리자 호출 

        }

        //if (isSmall == false)
        //{
        if (collision.gameObject.tag == "enemy")
        {
            //if (this.transform.localScale.x == 0.4f)
            //{
            //    SceneManager.LoadScene("GameOverScene");
            //}
            //this.onAttackedEnemy(0.4f);
            //Destroy(collision.gameObject);
        }
        //}
    }


    public void Bigger(float scale)
    {
        if (this.IsBig)
        {
            Debug.Log("이미 커진 상태 입니다.");
        }
        else //작은 상태면 크기증가
        {
            this.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            this.IsBig = true;
        }
        
    }
    public void Smaller(float scale)
    {
        if (this.IsSmall)
        {
            Debug.Log("이미 작아진 상태 입니다.");
        }
        else
        {
            this.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            this.IsSmall = true;
            this.IsBig = false;
        }
        
    }

    private bool ignoreInput = false;
    public void IgnoreInput(bool ignore)
    {
        ignoreInput = ignore;
        Debug.LogFormat("IgnoreInput: <color=lime>{0}</color>", this.ignoreInput);
    }

    public bool IsBig//true,false
    {
        get; set;
    }
    public bool IsSmall
    {
        get;set;
    }
}
