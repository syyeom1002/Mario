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
        //�����
        this.audioSource = this.GetComponent<AudioSource>();
        this.audioSource.PlayOneShot(this.backgoundSfx);

        DataManager.instance.LoadMarioData();
        //ui ����--------------------------------------------
        GameObject uiGo = Instantiate(this.uiPrefab);
        uiGo.transform.SetParent(parent, false);
        this.uiDirector = uiGo.GetComponent<UIDirector>();
        this.uiDirector.UpdateUI();
        uiDirector.txtMapName.text = string.Format("WORLD\n1-1");
        //������ ����-----------------------------------------
        GameObject marioGo = Instantiate(marioPrefab);
        marioGo.transform.position = this.marioTrans.position;
        marioGo.name = "Mario";//Mario �̸����� ������
        cameraController.Init();
        this.marioController = marioGo.GetComponent<MarioController>();

        this.marioController.onGetMushroom = (scale) => {
            this.marioController.Bigger(scale);//ũ�� �����ϴ� �Լ�
            Debug.LogFormat("����ȹ�� : isBig => {0}", this.marioController.IsBig);//true or false ���
        };

        this.marioController.onAttackedEnemy = (scale) =>
        {
            this.marioController.Smaller(scale);
            Debug.Log("���� �ε������ϴ�.");
        };

        this.marioController.onChangeScene = (sceneName) => {
            this.ChangeScene(sceneName);//���� �˻��ؼ� ��Ż ������ onChangeScene ȣ���ϰ� ����
        };

        this.marioController.onCoinEat = () =>
        {
            this.EatCoin();
            Debug.Log("����ȹ��");
        };
    }

    public void ChangeScene(string sceneName)
    {
        bool isBig = this.marioController.IsBig;//���� ������ Ŀ�� ���� ���� ����� ���ο��� ����
        bool isSmall = this.marioController.IsSmall;
        Debug.LogFormat("isBig: {0}", isBig);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        //���ε尡 �Ϸ�Ǹ� ȣ�� �Ǵ� �̺�Ʈ�� �븮�� ���� 
        asyncOperation.completed += (obj) => {

            switch (sceneName)
            {
                //���� ���� ������ 
                case "Map2Scene":
                    //���� �� ���� �������� 
                    Map2main map2Main = GameObject.FindObjectOfType<Map2main>();
                    if (isBig)
                    {
                        map2Main.InitBig(isBig);//�ʱ�ȭ //������ ����
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
        Debug.Log("eatCoin ȣ��");
        uiDirector.IncreaseScore(50);
        uiDirector.IncreaseCoin();
        uiDirector.UpdateUI();
    }
   
}
