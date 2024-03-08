using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Map1Main : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip backgoundSfx;
    [SerializeField]
    private UIDirector uiDirector;
    [SerializeField]
    private GameObject uiPrefab;
    private Transform parent;
    [SerializeField]
    private GameObject marioPrefab;
    [SerializeField]
    private Transform marioTrans;
    [SerializeField]
    private CameraController cameraController;

    private MarioController marioController;
    // Start is called before the first frame update

    void Start()
    {
        //오디오
        this.audioSource = this.GetComponent<AudioSource>();
        this.audioSource.PlayOneShot(this.backgoundSfx);

        DataManager.instance.LoadMarioData();
        //ui 생성--------------------------------------------
        GameObject uiGo = Instantiate(this.uiPrefab);
        uiGo.transform.SetParent(parent, false);
        this.uiDirector = uiGo.GetComponent<UIDirector>();
        this.uiDirector.UpdateUI();
        uiDirector.txtMapName.text = string.Format("WORLD\n1-1");
        //마리오 생성-----------------------------------------
        GameObject marioGo = Instantiate(marioPrefab);
        marioGo.transform.position = this.marioTrans.position;
        marioGo.name = "Mario";//Mario 이름으로 생성됨
        cameraController.Init();
        this.marioController = marioGo.GetComponent<MarioController>();

        this.marioController.onGetMushroom = (scale) => {
            this.marioController.Bigger(scale);//크기 증가하는 함수
            Debug.LogFormat("버섯획득 : isBig => {0}", this.marioController.IsBig);//true or false 출력
        };

        this.marioController.onAttackedEnemy = (scale) =>
        {
            this.marioController.Smaller(scale);
            Debug.Log("적과 부딪혔습니다.");
        };

        this.marioController.onChangeScene = (sceneName) => {
            this.ChangeScene(sceneName);//레이 검사해서 포탈 있으면 onChangeScene 호출하고 있음
        };

        this.marioController.onCoinEat = () =>
        {
            this.EatCoin();
            Debug.Log("코인획득");
        };
    }

    public void ChangeScene(string sceneName)
    {
        bool isBig = this.marioController.IsBig;//현재 씬에서 커진 상태 다음 씬담당 메인에게 전달
        bool isSmall = this.marioController.IsSmall;
        Debug.LogFormat("isBig: {0}", isBig);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        //씬로드가 완료되면 호출 되는 이벤트에 대리자 연결 
        asyncOperation.completed += (obj) => {

            switch (sceneName)
            {
                //굴둑 동전 먹으러 
                case "Map2Scene":
                    //다음 씬 메인 가져오기 
                    Map2main map2Main = GameObject.FindObjectOfType<Map2main>();
                    if (isBig)
                    {
                        map2Main.InitBig(isBig);//초기화 //마리오 생성
                        //map2Main.InitUI();
                    }
                    else if (isSmall)
                    {
                        map2Main.InitSmall(isSmall);

                    }
                    else
                        map2Main.InitSmall(isSmall);
                    break;
            }

        };

    }
    private void Save()
    {

    }
    public void EatCoin()
    {
        Debug.Log("eatCoin 호출");
        uiDirector.IncreaseScore(50);
        uiDirector.IncreaseCoin();
        uiDirector.UpdateUI();
    }
   
}
