using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class Map3main : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip backgoundSfx;
    [SerializeField]
    private GameObject uiPrefab;
    private Transform parent;
    private UIDirector uiDirector;
    [SerializeField]
    private GameObject marioPrefab;
    [SerializeField]
    private Transform marioTrans;
    private MarioController marioController;
    [SerializeField]
    private CameraController cameraController;
    // Start is called before the first frame update

    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
        this.audioSource.PlayOneShot(this.backgoundSfx);
        this.marioController.onCoinEat = () =>
        {
            this.EatCoin();
            Debug.Log("����ȹ��");
        };
        uiDirector.txtMapName.text = string.Format("WORLD\n2-1");
    }
    public void EatCoin()
    {
        Debug.Log("eatCoin ȣ��");
        uiDirector.IncreaseScore(50);
        uiDirector.IncreaseCoin();
        uiDirector.UpdateUI();
    }
    public void Init(bool isBig)
    {
        //������ ����� 
        this.CreateMario(isBig);
        cameraController.Init();
        this.CreateUI();
    }


    private void CreateMario(bool isBig)
    {
        var go = Instantiate(marioPrefab);
        go.transform.position = this.marioTrans.position;
        go.name = "Mario";
        this.marioController = go.GetComponent<MarioController>();

        //���������� Ŀ�� ���� ���� �޾� ũ�� ����� 
        if (isBig)//�̹� Ŀ�� �����϶� -> ������ �� ũ�� �����
            this.marioController.Bigger(0.6f);
 

    }
    private void CreateUI()
    {
        GameObject uiGo = Instantiate(this.uiPrefab);
        uiGo.transform.SetParent(parent, false);
        this.uiDirector = uiGo.GetComponent<UIDirector>();
        this.uiDirector.LoadData();
    }
}
