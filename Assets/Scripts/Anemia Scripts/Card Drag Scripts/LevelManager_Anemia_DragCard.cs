using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelManager_Anemia_DragCard : MonoBehaviour
{

    private static LevelManager_Anemia_DragCard instance;

    public static LevelManager_Anemia_DragCard Instance{ get { return instance; } }

    public GameObject card, cardContainer;
    public Sprite[] sprites;
    public ArrayList SpritesArrList;
    public ArrayList states;

    private bool created, finished;

    public Text scoreText;
    public Transform winMenu;
    public Button[] mainUIBtn;

    void Awake()
    {
        instance = this;
        created = false;
        finished = false;
        SpritesArrList = new ArrayList();
        states = new ArrayList();
        for (int i = 0; i < sprites.Length; i++)
        {
            SpritesArrList.Add(i);
        }
        InitializeState();
        CreateCard();
    }

    void InitializeState()
    {
        for (int i = 0; i < SpritesArrList.Count; i++)
        {
            if (sprites[i].name.Contains("Good"))
            {
                states.Add("Good");
            }
            else if (sprites[i].name.Contains("Bad"))
            {
                states.Add("Bad");
            }
        }
    }

    void CreateCard()
    {
        created = true;
        GameObject c = Instantiate(card)as GameObject;
        c.transform.SetParent(cardContainer.transform, false);
        int index = Random.Range(0, SpritesArrList.Count);
        int spriteIndex = (int)SpritesArrList[index];
        c.GetComponent<Image>().sprite = sprites[spriteIndex];
        if (sprites[spriteIndex].name.Contains("Good"))
        {
            Drag_Handeler.state = "Good";
        }
        else if (sprites[spriteIndex].name.Contains("Bad"))
        {
            Drag_Handeler.state = "Bad";
        }
        SpritesArrList.RemoveAt(index);
        states.RemoveAt(index);
    }

    void FixedUpdate()
    {
        if (!created && !finished)
        {
            CreateCard();
        }
        if (finished)
        {
            finished = false;
            FinshTheGame();
        }
    }

    public void FinshTheGame()
    {
        int starNum = 0;
        float percent = (float.Parse(scoreText.text)) / ((float)(sprites.Length * 10));
        //Debug.Log(percent);
        if (percent <= 1.0f && percent >= .75f)
        {// 3 stars
            for (int i = 4; i < 7; i++)
            {
                starNum++;
                winMenu.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
        }
        else if (percent < .75f && percent >= .5f)
        {// 2 stars
            for (int i = 4; i < 6; i++)
            {
                starNum++;
                winMenu.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
        }
        else if (percent < .5f && percent > 0.0f)
        {// 1 star
            for (int i = 4; i < 5; i++)
            {
                starNum++;
                winMenu.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
        }
        else if (percent <= 0)
        {// no stars level not complete
            winMenu.GetChild(0).GetChild(7).gameObject.SetActive(false);
        }

        winMenu.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = scoreText.text;

        if (starNum != 0)
        {
            SavalevelData(starNum);
        }
        OpenWinWindow();
    }

    void SavalevelData(int starNum)
    {
        string saveString = "";
        LevelData level = new LevelData(SceneManager.GetActiveScene().buildIndex);
        saveString += (starNum > level.StarsNum) ? starNum.ToString() : level.StarsNum.ToString();
        saveString += "&";
        saveString += (int.Parse(scoreText.text) > level.BestScore) ? scoreText.text : level.BestScore.ToString();
        PlayerPrefs.SetString(SceneManager.GetActiveScene().buildIndex.ToString(), saveString);
    }

    void OpenWinWindow()
    {
        foreach (Button i in mainUIBtn)
        {
            i.gameObject.SetActive(false);
        }

        winMenu.gameObject.SetActive(!winMenu.gameObject.activeSelf);
    }

    public bool Created
    {
        get{ return created; }
        set{ created = value; }
    }

    public bool Finished
    {
        get{ return finished; }
        set{ finished = value; }
    }
}
 