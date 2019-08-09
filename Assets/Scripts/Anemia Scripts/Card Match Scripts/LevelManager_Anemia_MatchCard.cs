using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager_Anemia_MatchCard : MonoBehaviour
{
    
    private static LevelManager_Anemia_MatchCard instance;

    public static LevelManager_Anemia_MatchCard Instance{ get { return instance; } }

    public Sprite[] cardFaces;
    public Sprite cardBack;
    public GameObject[] cards;

    private int matches;

    public int matchCount;

    public Text scoreText;

    private bool isShow;

    public Transform winMenu;
    public Button[] mainUIBtn;

    void Awake()
    {
        instance = this;
        isShow = true;
        matches = cards.Length / 2;
        matchCount = 0;
    }

    void Start()
    {
        InitializeCards();
    }

    void InitializeCards()
    {
        
        int[] spriteIndex = new int[cards.Length];
        ArrayList cardIndex = new ArrayList();
        for (int i = 0; i < cards.Length; i++)
        {
            cardIndex.Add(i);
        }

        //create random sprites
        for (int i = 0; i < cards.Length; i++)
        {
            int x = Random.Range(0, cards.Length - i);
            int xx = (int)cardIndex[x];
            spriteIndex[xx] = (i % (cards.Length / 2));
            cardIndex.RemoveAt(x);
        }

        //set the card values
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<Cards_Match>().CardValue = spriteIndex[i];
            cards[i].GetComponent<Cards_Match>().SetupGraphics();
        }
    }

    void Update()
    {
        if (Timer.Instance.timerSecond <= 0 && isShow)
        {
            Timer.Instance.gameObject.SetActive(false);
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].GetComponent<Cards_Match>().State = 1;
                cards[i].GetComponent<Cards_Match>().FalseCheck();
            }
            isShow = false;
        }
        if (matchCount == 2)
        {
            Cards_Match.do_Not = true;
            CheckCards();
            matchCount = 0;
        }
    }



    public Sprite GetCardBack()
    {
        return cardBack;
    }

    public Sprite GetCardFace(int cardValue)
    {
        return cardFaces[cardValue];
    }

    void CheckCards()
    {
        List<int> c = new List<int>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<Cards_Match>().State == 1)
            {
                c.Add(i);
            }
            if (c.Count == 2)
            {
                break;
            }
        }

        if (c.Count == 2)
        {
            CardComparison(c);
        }
        else
        {
            Debug.Log("something wrong");
        }
    }

    void CardComparison(List<int> c)
    {
        if (cards[c[0]].GetComponent<Cards_Match>().CardValue == cards[c[1]].GetComponent<Cards_Match>().CardValue)
        {
            cards[c[0]].GetComponent<Cards_Match>().State = 2;
            cards[c[1]].GetComponent<Cards_Match>().State = 2;
            matches--; 
            scoreText.text = (int.Parse(scoreText.text) + 10).ToString();
            Cards_Match.do_Not = false;
            StartCoroutine(DestroyEffect(c));
            if (matches == 0)
            {
                //end of level
                FinshTheGame();
            }
            return;
        }
        StartCoroutine(Pause(c));

    }

    IEnumerator DestroyEffect(List<int> c)
    {
        yield return new WaitForSeconds(.1f);
        cards[c[0]].SetActive(false);
        cards[c[1]].SetActive(false);
    }

    IEnumerator Pause(List<int> c)
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<Cards_Match>().FalseCheck();
        }

        scoreText.text = (int.Parse(scoreText.text) - 5).ToString();

        Cards_Match.do_Not = false;
    }

    void FinshTheGame()
    {
        int starNum = 0;
        float percent = (float.Parse(scoreText.text)) / ((float)((cards.Length / 2) * 10));
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
        //Time.timeScale = 0;

        foreach (Button i in mainUIBtn)
        {
            i.gameObject.SetActive(false);
        }

        winMenu.gameObject.SetActive(!winMenu.gameObject.activeSelf);
    }
}
