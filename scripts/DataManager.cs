using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static readonly DataManager instance = new DataManager();
    private Dictionary<int, MarioData> dicMarioDatas = new Dictionary<int, MarioData>();
    public void LoadMarioData()
    {
        TextAsset asset= Resources.Load<TextAsset>("mario_data");
        string json = asset.text;
        Debug.Log(json);
        var marioDatas = JsonConvert.DeserializeObject<MarioData[]>(json);
        Debug.LogFormat("arrMarioDatas.length:{0}", marioDatas.Length);

        foreach(var marioData in marioDatas)
        {
            this.dicMarioDatas.Add(marioData.id, marioData);
        }
        Debug.LogFormat("mario_data 에셋을 로드 했습니다. {0}", this.dicMarioDatas.Count);
    }

    public List<MarioData> GetMarioDatas()
    {
        return this.dicMarioDatas.Values.ToList();

    }
    
}
