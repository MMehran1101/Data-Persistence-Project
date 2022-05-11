using UnityEngine;
using TMPro;
using System.IO;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [SerializeField] 
    private TMP_InputField playerName_TMP;
    public string playerNameInput;
    public TextMeshProUGUI bestScore;

    
    private void Awake()
    {
        if(Instance != null) Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        LoadBestScore();
        playerName_TMP.Select();
        // Submit player name
        var submitName= new TMP_InputField.SubmitEvent();
        submitName.AddListener(SubmitName);
        playerName_TMP.onEndEdit = submitName;
    }

    private void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/save.json";

        if (!File.Exists(path)) return;
        string json = File.ReadAllText(path);

        SaveData data = JsonUtility.FromJson<SaveData>(json);
        bestScore.text = $"Best Score: {data.name} : {data.highScore}";
    }

    private void SubmitName(string arg0)
    {
        Instance.playerNameInput = arg0;
    }
}
