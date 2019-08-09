using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelData
{
    public LevelData(int levelNum)
    {
        string data = PlayerPrefs.GetString(levelNum.ToString());
        if (data.Equals(""))
        {
            return;
        }
        string[] allData = data.Split('&');
        StarsNum = int.Parse(allData[0]);
        BestScore = int.Parse(allData[1]);
    }

    public int BestScore{ set; get; }

    public int StarsNum{ set; get; }

}

public class UI_Manager : MonoBehaviour
{

    public Transform uiContainer;

    public GameObject levelBtnContainer, levelWindow;

    private bool nextLevelLocked = false;

    public Button[] girdBtns;
    public Image[] audioBtns;
    public Sprite[] audioBtnSprites;
    

    private int gridIndex = -1;

    void Awake()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            audioBtns[0].sprite = (PlayerPrefs.GetInt("Music") == 1) ? audioBtnSprites[0] : audioBtnSprites[1];
        }

        if (PlayerPrefs.HasKey("Sound"))
        {
            audioBtns[1].sprite = (PlayerPrefs.GetInt("Sound") == 1) ? audioBtnSprites[2] : audioBtnSprites[3];
        }
    }

    void Start()
    {
        //x num of levels, y num of gridcontainers, z num of gridcontainers that i suppost start with 
        int x = 1, y = -1, z = -1;
        GameObject gridcontainer = new GameObject();
        GameObject[] levels = Resources.LoadAll<GameObject>("Levels");

        /*SortedDictionary<string,GameObject> sortedDictionary = new SortedDictionary<string, GameObject>();
        for (int i = 0; i < levels.Length; i++)
        {
            sortedDictionary.Add(levels[i].name, levels[i]);
        }
        sortedDictionary.Values.CopyTo(levels, 0);
        for (int i = 0; i < levels.Length; i++)
        {
            Debug.Log(levels[i].name);
        }*/

        foreach (GameObject i in levels)
        {
            
            if (x == 1)
            {
                gridcontainer = Instantiate(levelBtnContainer)as GameObject;
                gridcontainer.transform.SetParent(levelWindow.transform, false);
            }
            x++;
            string[] ss = i.name.Split('_');
            int sceneNum = int.Parse(ss[1].ToString());
            GameObject s = Instantiate(i)as GameObject;
            s.transform.SetParent(gridcontainer.transform, false);


            LevelData level = new LevelData(sceneNum);
            if (!nextLevelLocked)
            {
                s.transform.GetChild(0).GetComponent<Text>().text = sceneNum.ToString();
            }
            else
            {
                s.transform.GetChild(0).GetComponent<Text>().text = "";
            }
            s.GetComponent<Button>().interactable = !nextLevelLocked;
            s.transform.GetChild(1).gameObject.SetActive(!nextLevelLocked);

            if (level.StarsNum == 0)
            {
                //not played yet
                nextLevelLocked = true;
                if (z == -1)
                {
                    z = y;
                }
            }
            else if (level.StarsNum == 3)
            {
                //three stars
                for (int j = 0; j < level.StarsNum; j++)
                {
                    s.transform.GetChild(1).GetChild(j).gameObject.SetActive(true);
                }
            }
            else if (level.StarsNum == 2)
            {
                //two stars
                for (int j = 0; j < level.StarsNum; j++)
                {
                    s.transform.GetChild(1).GetChild(j).gameObject.SetActive(true);
                }
            }
            else if (level.StarsNum == 1)
            {
                //one star
                for (int j = 0; j < level.StarsNum; j++)
                {
                    s.transform.GetChild(1).GetChild(j).gameObject.SetActive(true);
                }
            }


            s.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneNum));
            if (x == 9)
            {
                x = 1;
                gridcontainer = null;
                y++;
            }
        }
        if (z == -1)
        {
            z = y;
        }
        gridIndex = z;
        showLevelGrid(gridIndex);
    }

    private void LoadLevel(int sceneNum)
    {
        uiContainer.GetChild(3).gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(sceneNum));
    }

    IEnumerator LoadAsynchronously(int sceneNum)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNum);
        while (!operation.isDone)
        {
            uiContainer.GetChild(3).GetChild(0).GetComponent<Slider>().value = operation.progress;
            yield return null;
        }
    }

    public void mainMenuUI(int index)
    {
        switch (index)
        {
            case 0:
                uiContainer.GetChild(2).gameObject.SetActive(true);
                uiContainer.GetChild(1).gameObject.SetActive(false);
                break;
            case 1:
                Application.Quit();
                break;
            case 2:
                audioBtns[0].sprite = (audioBtns[0].sprite == audioBtnSprites[1]) ? audioBtnSprites[0] : audioBtnSprites[1];
                int musicValue = (audioBtns[0].sprite == audioBtnSprites[0]) ? 1 : 0;
                PlayerPrefs.SetInt("Music", musicValue);
                break;
            case 3:
                audioBtns[1].sprite = (audioBtns[1].sprite == audioBtnSprites[3]) ? audioBtnSprites[2] : audioBtnSprites[3];
                int soundValue = (audioBtns[1].sprite == audioBtnSprites[2]) ? 1 : 0;
                PlayerPrefs.SetInt("Sound", soundValue);
                break;
        }
    }

    public void levelSelectionUI(int index)
    {
        switch (index)
        {
            case 0:
                uiContainer.GetChild(1).gameObject.SetActive(true);
                uiContainer.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                showLevelGrid(++gridIndex);
                break;
            case -1:
                showLevelGrid(--gridIndex);
                break;
        }
    }

    void showLevelGrid(int index)
    {
        for (int i = 0; i < levelWindow.transform.childCount; i++)
        {
            if (i == index)
            {
                levelWindow.transform.GetChild(i).gameObject.SetActive(true);
                continue;
            }
            levelWindow.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (index == 0)
        {
            girdBtns[0].interactable = true; //nextBtn
            girdBtns[1].interactable = false;//previousBtn
        }
        else if (index > 0 && index < levelWindow.transform.childCount - 1)
        {
            girdBtns[0].interactable = true; //nextBtn
            girdBtns[1].interactable = true;//previousBtn
        }
        else if (index == levelWindow.transform.childCount - 1)
        {
            girdBtns[0].interactable = false; //nextBtn
            girdBtns[1].interactable = true;//previousBtn
        }
    }
}
