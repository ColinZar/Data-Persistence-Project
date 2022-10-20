using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

//delay ui elements, as other things may need to init first
[DefaultExecutionOrder(1000)]
public class Menu : MonoBehaviour
{
    private Menu Instance;

    public string username;

    public TMP_InputField InputField;

    

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log(Application.persistentDataPath);
        LoadUsername();
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Debug.Log(username);
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }


    [System.Serializable]
    public class SaveData
    {
        public string username;
    }

    public void UpdateUsername()
    {
        username = InputField.text;
    }

    public void SaveUsername()
    {
        SaveData data = new SaveData();
        data.username = username;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/datasavefile.json", json);
    }

    public void LoadUsername()
    {
        string path = (Application.persistentDataPath + "/datasavefile.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            username = data.username;
            InputField.text = username;
        }
    }
}
