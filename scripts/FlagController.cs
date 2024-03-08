using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip gameOverSfx;
    [SerializeField]
    private GameObject flagGo;
    [SerializeField]
    private float moveSpeed = 0.5f;
    [SerializeField]
    private Transform handle;
    private MarioController marioController;
    private bool isInFlag = false;
    private bool canKey = true;
    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
        isInFlag = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);

        if (collision.CompareTag("mario"))
        {
            MarioController mario = GameObject.Find("Mario").GetComponent<MarioController>();

            //마리오는 더이상 인풋을 받지 못함 
            mario.IgnoreInput(true);
        

            Rigidbody2D mRigid = mario.GetComponent<Rigidbody2D>();

            //현재 속도를 0으로 만듬 
            mRigid.velocity = Vector3.zero;
            mRigid.angularVelocity = 0;

            //부모를 핸들로 설정 
            mario.transform.SetParent(this.handle);

            //물리 영향을 안받게 설정 
            mRigid.isKinematic = true;

            //마리오 위치
            handle.transform.position = new Vector3(handle.transform.position.x, mario.transform.position.y, handle.transform.position.z);
            //처음 위치 설정 
            mario.transform.position = handle.transform.position;

            //핸들 이동하기 
            this.StartCoroutine(this.CoMoveHandle());
            this.StartCoroutine(this.CoMoveFlag());
            this.StartCoroutine(this.CoLoadGameOver());
            // mario.IgnoreInput(false);
        }


    }
    private IEnumerator CoMoveFlag()
    {
        while (true)
        {
            flagGo.transform.Translate(this.moveSpeed * Vector3.down * Time.deltaTime);
            if (flagGo.transform.localPosition.y <= 0.3f)
            {
                this.moveSpeed = 0f;
                this.audioSource.PlayOneShot(this.gameOverSfx);
                break;
            }
            yield return null;
        }
        Debug.LogFormat("깃발이 이동을 완료 했습니다.");
 

    }
    private IEnumerator CoLoadGameOver()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameOverScene");
        
    }
    private IEnumerator CoMoveHandle()
    {
        while (true)
        {
            handle.transform.Translate(this.moveSpeed * Vector3.down * Time.deltaTime);
            if (handle.transform.localPosition.y <= 0.63f)
            {
                this.moveSpeed = 0f;
                break;
            }
            yield return null;
        }
        Debug.LogFormat("handle이 이동을 완료 했습니다.");
    }

}
