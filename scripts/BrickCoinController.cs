using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class BrickCoinController : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private GameObject coinPrefab;
    private bool isCrashed = false;
    //public System.Action<ItemGenerator.eItemType> onChanged;
    // Start is called before the first frame update
    void Start()
    {
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
                Instantiate(coinPrefab, new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z), transform.rotation);
                isCrashed = true;
            }
        }
    }

}
