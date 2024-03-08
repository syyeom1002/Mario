using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIDirector : MonoBehaviour
{
    
    [SerializeField]
    private Text txtScore;
    [SerializeField]
    private Text txtCoin;
    [SerializeField]
    private Text txtTime;
    public Text txtMapName;

    public float time =400f;
    private int score=0;
    private int coin=0;
    // Start is called before the first frame update
    void Start()
    {
        //this.LoadData();
        //this.txtMapName.text = string.Format("WORLD\n1-1");
        //this.UpdateUI();
        //PlayerPrefs.SetInt
    }

    // Update is called once per frame
    void Update()
    {
        this.time -= Time.deltaTime;
        this.txtTime.text = this.time.ToString("F0");//정수로 표현
        this.txtTime.text = string.Format("TIME\n{0}", this.txtTime.text);
    }

    public void UpdateUI()
    {
        List<MarioData> marioDatas = DataManager.instance.GetMarioDatas();
        foreach (MarioData data in marioDatas) {
            Debug.LogFormat("<color=green>id:{0},score{1}</color>",data.id,data.score);
        }
        this.txtScore.text = string.Format("MARIO\n{0:D6}", score);//총 6자리로 앞에 0으로 채우기
        this.txtCoin.text = string.Format("X {0}", this.coin);
        this.SaveData();
    }
    public void IncreaseScore(int score)
    {
        Debug.Log("50점 획득");
        this.score += score;
    }
    public void IncreaseCoin()
    {
        this.coin += 1;
    }

    public void SaveData()
    {
        
        PlayerPrefs.SetInt("Score",score);
        PlayerPrefs.SetFloat("Time", time);
        PlayerPrefs.SetInt("Coin", coin);
    }
    public void LoadData()
    {
        int score = PlayerPrefs.GetInt("Score");
        this.txtScore.text = score.ToString();
        this.txtScore.text = string.Format("MARIO\n{0:D6}", score);

        this.txtCoin.text = PlayerPrefs.GetInt("Coin").ToString();
        this.txtCoin.text = string.Format("X {0}", this.txtCoin.text);
    }
}
