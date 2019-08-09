using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{

    public Transform loadingContainer;
    // Use this for initialization
    void Start()
    {
        string saveString = "";
        LevelData level = new LevelData(SceneManager.GetActiveScene().buildIndex);
        saveString += "2";
        saveString += "&";
        saveString += "50";
        PlayerPrefs.SetString(SceneManager.GetActiveScene().buildIndex.ToString(), saveString);
        loadingContainer.gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(0));
    }

    IEnumerator LoadAsynchronously(int sceneNum)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNum);
        while (!operation.isDone)
        {
            loadingContainer.GetChild(0).GetComponent<Slider>().value = operation.progress;
            yield return null;
        }
    }
	

}
