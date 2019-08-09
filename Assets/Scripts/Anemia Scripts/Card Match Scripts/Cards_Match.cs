using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cards_Match : MonoBehaviour
{

    public static bool do_Not;

    private int state, cardValue;

    private Sprite cardBack, cardFace;

    void Awake()
    {
        //state -1 for show cards only , 0 the card show back , 1 the card show face , 2 the card is matched 
        state = -1;
        do_Not = false;
    }

    public void SetupGraphics()
    {
        cardBack = LevelManager_Anemia_MatchCard.Instance.GetCardBack();
        cardFace = LevelManager_Anemia_MatchCard.Instance.GetCardFace(cardValue);

        FlipCard();
    }

    public void FlipCard()
    {
        if (state == -1)
        {
            GetComponent<Image>().sprite = cardFace;
        }
        else if (state == 0 && !do_Not) //the card is back and will be face
        {
            state = 1;
            GetComponent<Image>().sprite = cardFace;
            LevelManager_Anemia_MatchCard.Instance.matchCount++;
        }
        else if (state == 1 && !do_Not) //the card is face and will be back
        {
            state = 0;
            GetComponent<Image>().sprite = cardBack;
            LevelManager_Anemia_MatchCard.Instance.matchCount--;
            LevelManager_Anemia_MatchCard.Instance.scoreText.text = (int.Parse(LevelManager_Anemia_MatchCard.Instance.scoreText.text) - 2).ToString();
        }
    }

    public int CardValue
    {
        get{ return cardValue; }
        set{ cardValue = value; }
    }

    public int State
    {
        get{ return state; }
        set{ state = value; }
    }

    public void FalseCheck()
    {
        if (state == 0) //the card is back and will be face
        {
            state = 1;
            GetComponent<Image>().sprite = cardFace;
        }
        else if (state == 1) //the card is face and will be back
        {
            state = 0;
            GetComponent<Image>().sprite = cardBack;
        }
    }
}
