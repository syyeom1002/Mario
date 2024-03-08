using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    
    private Transform playerTransform;
    [SerializeField]
    Vector3 cameraPosition;

    [SerializeField]
    Vector2 center;
    [SerializeField]
    Vector2 mapSize;

    [SerializeField]
    float cameraMoveSpeed;
    float height;
    float width;


    public void Init()//시작하자마자 말고, 호출될때 (마리오 생성되고) 호출되어야함
    {
        playerTransform = GameObject.Find("Mario").GetComponent<Transform>();//생성된 마리오 이름의 오브젝트의 위치를 찾아서 저장

        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }


    void FixedUpdate()
    {
        LimitCameraArea();
    }

    void LimitCameraArea()
    {
        //초기화 되기전에는 호출되면 안됨 
        if (playerTransform == null) return;

        transform.position = Vector3.Lerp(transform.position,
                                          playerTransform.position + cameraPosition,
                                          Time.deltaTime * cameraMoveSpeed);
        float lx = mapSize.x - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}
