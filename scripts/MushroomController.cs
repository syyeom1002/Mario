using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    public float moveSpeed = 0.05f;
    private Animator anim;
    //private MarioController marioController;

    void Start()
    {
        this.anim = this.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(this.moveSpeed, 0, 0);
        if (this.transform.position.y < -3.5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "portal")
        {
            moveSpeed = moveSpeed * -1;
        }
        if (collision.gameObject.tag == "wall")
        {
            moveSpeed = moveSpeed * -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "mario")
        {
            //애니메이션 실행
            this.anim.SetBool("isAttacked", true);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.3f);//-2.69-(-3.16)
            this.moveSpeed = 0f;
            //몇초후 사라지기
            Invoke("Die", 0.15f); 
        }
    }
    private void Die()
    {
        Destroy(this.gameObject);
    }
}
