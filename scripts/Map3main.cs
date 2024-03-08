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
            Debug.Log("코인획득");
        };
        uiDirector.txtMapName.text = string.Format("WORLD\n2-1");
    }
    public void EatCoin()
    {
        Debug.Log("eatCoin 호출");
        uiDirector.IncreaseScore(50);
        uiDirector.IncreaseCoin();
        uiDirector.UpdateUI();
    }
    public void Init(bool isBig)
    {
        //마리오 만들기 
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

        //이전씬에서 커진 상태 전달 받아 크게 만들기 
        if (isBig)//이미 커진 상태일때 -> 생성할 때 크게 만들기
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
