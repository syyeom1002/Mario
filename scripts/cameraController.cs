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


    public void Init()//�������ڸ��� ����, ȣ��ɶ� (������ �����ǰ�) ȣ��Ǿ����
    {
        playerTransform = GameObject.Find("Mario").GetComponent<Transform>();//������ ������ �̸��� ������Ʈ�� ��ġ�� ã�Ƽ� ����

        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }


    void FixedUpdate()
    {
        LimitCameraArea();
    }

    void LimitCameraArea()
    {
        //�ʱ�ȭ �Ǳ������� ȣ��Ǹ� �ȵ� 
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
