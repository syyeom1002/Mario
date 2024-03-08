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

            //�������� ���̻� ��ǲ�� ���� ���� 
            mario.IgnoreInput(true);
        

            Rigidbody2D mRigid = mario.GetComponent<Rigidbody2D>();

            //���� �ӵ��� 0���� ���� 
            mRigid.velocity = Vector3.zero;
            mRigid.angularVelocity = 0;

            //�θ� �ڵ�� ���� 
            mario.transform.SetParent(this.handle);

            //���� ������ �ȹް� ���� 
            mRigid.isKinematic = true;

            //������ ��ġ
            handle.transform.position = new Vector3(handle.transform.position.x, mario.transform.position.y, handle.transform.position.z);
            //ó�� ��ġ ���� 
            mario.transform.position = handle.transform.position;

            //�ڵ� �̵��ϱ� 
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
        Debug.LogFormat("����� �̵��� �Ϸ� �߽��ϴ�.");
 

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
        Debug.LogFormat("handle�� �̵��� �Ϸ� �߽��ϴ�.");
    }

}
