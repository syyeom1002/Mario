using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class BrickMushroomController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip mushroomProduceSfx;
    private Animator anim;
    [SerializeField]
    private GameObject mushroomPrefab;
    private bool isCrashed = false;
    //public System.Action<ItemGenerator.eItemType> onChanged;
    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
        this.anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "mario")
        {
            this.anim.SetBool("isCrashed", true);
            if (isCrashed == false)
            {
                Instantiate(mushroomPrefab, new Vector3(transform.position.x, transform.position.y + 0.9f, transform.position.z), transform.rotation);
                this.audioSource.PlayOneShot(this.mushroomProduceSfx);
                isCrashed = true;
            }
        }
    }

}
