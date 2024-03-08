using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Map2main : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip backgoundSfx;
    private UIDirector uiDirector;
    [SerializeField]
    private GameObject uiPrefab;
    private Transform parent;
    [SerializeField]
    private GameObject marioPrefab;
    [SerializeField]
    private Transform marioTrans;
    private MarioController marioController;
    
    void Start()
    {

        this.audioSource = this.GetComponent<AudioSource>();
        this.audioSource.PlayOneShot(this.backgoundSfx);
        uiDirector.UpdateUI();
        //this.LoadData();
        //this.txtMapName.text = string.Format("WORLD\n1-2");
        //this.time = PlayerPrefs.GetFloat("Time");
        ////uiGo.transform.SetParent(parent, false);
        ///GameObject uiGo = Instantiate(this.uiPrefab);
        this.marioController.onChangeScene = (sceneName) => {
            this.ChangeScene(sceneName);//레이 검사해서 포탈 있으면 onChangeScene 호출하고 있음
        };
        uiDirector.txtMapName.text = string.Format("WORLD\n1-2");
        this.marioController.onCoinEat = () =>
        {
            this.EatCoin();
            Debug.Log("코인획득");
        };
    }
    public void ChangeScene(string sceneName)
    {
        bool isBig = this.marioController.IsBig;   //현재 씬에서 커진 상태 다음 씬담당 메인에게 전달
        Debug.LogFormat("isBig: {0}", isBig);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        //씬로드가 완료되면 호출 되는 이벤트에 대리자 연결 
        asyncOperation.completed += (obj) =>
        {

            switch (sceneName)
            {

                case "Map3Scene":
                    Map3main map3Main = GameObject.FindObjectOfType<Map3main>();
                    map3Main.Init(isBig);
                    break;
            }

        };

    }
    public void EatCoin()
    {
        Debug.Log("eatCoin 호출");
        uiDirector.IncreaseScore(50);
        uiDirector.IncreaseCoin();
        uiDirector.UpdateUI();
    }
    //public void InitUI()
    //{
    //   this.CreateUI();
    //}

    public void InitBig(bool isBig)
    {
        //마리오 만들기 
        this.CreateMarioBig(isBig);

        //UI생성 
        this.CreateUI(); ;
    }
    public void InitSmall(bool isSmall)
    {
        //마리오 만들기 
        this.CreateMarioSmall(isSmall);
        this.CreateUI();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    this.time -= Time.deltaTime;
    //    this.txtTime.text = time.ToString("F0");
    //    this.txtTime.text = string.Format("TIME\n{0}", this.txtTime.text);
    //    this.SaveData();
    //}
    //public void LoadData()
    //{
    //    int score = PlayerPrefs.GetInt("Score");
    //    this.txtScore.text = score.ToString();
    //    this.txtScore.text = string.Format("MARIO\n{0:D6}", score);

    //    this.txtCoin.text= PlayerPrefs.GetInt("Coin").ToString();
    //    this.txtCoin.text = string.Format("X {0}", this.txtCoin.text);
    //}

    //public void SaveData()
    //{
    //    PlayerPrefs.SetInt("Score2", score);
    //    PlayerPrefs.SetFloat("Time2",time);
    //    PlayerPrefs.SetInt("Coin2", coin);
    //}

    private void CreateMarioBig(bool isBig)
    {
      
        var go = Instantiate(this.marioPrefab);
        go.transform.position = this.marioTrans.position;
        go.name = "Mario";
        this.marioController = go.GetComponent<MarioController>();

        //이전씬에서 커진 상태 전달 받아 크게 만들기 
        if (isBig)//이미 커진 상태일때 -> 생성할 때 크게 만들기
            this.marioController.Bigger(0.6f);
            
    }
    private void CreateMarioSmall(bool isSmall)
    {

        var go = Instantiate(this.marioPrefab);
        Debug.Log(go);
        go.transform.position = this.marioTrans.position;
        go.name = "Mario";
        this.marioController = go.GetComponent<MarioController>();

        //이전씬에서 작은 상태 전달 받아 작게 만들기 
        if (isSmall)//이미 작아진 상태일때 -> 생성할 때 작게 만들기
            this.marioController.Smaller(0.4f);

    }
    private void CreateUI()
    {
        GameObject uiGo = Instantiate(this.uiPrefab);
        uiGo.transform.SetParent(parent, false);
        this.uiDirector = uiGo.GetComponent<UIDirector>();
        this.uiDirector.LoadData();
    }
}
