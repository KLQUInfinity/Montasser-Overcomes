using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager_Anemia : MonoBehaviour
{

    public GameObject player;

    public GameObject[] fallingObjects;
    private float maxWidth;

    public GameObject[] border;

    public static int goodFoodNum;

    private bool checkEnd;
    public Transform winMenu;
    public Button[] mainUIBtn;
    public Text scoreText;

    public GameObject mobileUI;

    void Start()
    {
        checkEnd = false;
        goodFoodNum = 0;
        Vector3 targetPos = new Vector3(Screen.width, Screen.height, 0f);
        targetPos = Camera.main.ScreenToWorldPoint(targetPos);
        maxWidth = targetPos.x;

        GetComponent<Collider2D>().offset = new Vector2(0.0f, -(targetPos.y + 10.0f + transform.position.y));

        Instantiate(border[0], new Vector3((-maxWidth), player.transform.position.y, 0.0f), Quaternion.identity);
        Instantiate(border[1], new Vector3((maxWidth), player.transform.position.y, 0.0f), Quaternion.identity);

        StartCoroutine(spwan());
    }

    IEnumerator spwan()
    {
        yield return new WaitForSeconds(2.0f);
        while (Timer.Instance.timerSecond > 0)
        {
            int index = Random.Range(0, fallingObjects.Length);
            float foodWidth = fallingObjects[index].GetComponent<Renderer>().bounds.extents.x;
            maxWidth -= foodWidth;
            Vector3 spwanPosition = new Vector3(
                                        Random.Range(-maxWidth, maxWidth),
                                        transform.position.y,
                                        0.0f
                                    );

            maxWidth += foodWidth;
            GameObject ob = Instantiate(fallingObjects[index], spwanPosition, Quaternion.identity)as GameObject;
            ob.GetComponent<Rigidbody2D>().isKinematic = false;
            if (ob.tag.Equals("Good"))
            {
                goodFoodNum++;
            }
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
    }

    void FixedUpdate()
    {
        if (Timer.Instance.timerSecond <= 0 && !checkEnd)
        {
            int starNum = 0;
            checkEnd = true;
            float percent = ((float)PlayerController_Anemia.goodFoodNum) / ((float)goodFoodNum);

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
            else if (percent == 0)
            {// no stars level not complete
                winMenu.GetChild(0).GetChild(7).gameObject.SetActive(false);
            }

            winMenu.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = scoreText.text;

            if (PlayerController_Anemia.goodFoodNum != 0)
            {
                SavalevelData(starNum);
            }
            OpenWinWindow();
        }
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
        Time.timeScale = 0;
        Timer.Instance.gameObject.SetActive(false);
        mobileUI.SetActive(false);
        foreach (Button i in mainUIBtn)
        {
            i.gameObject.SetActive(false);
        }
        winMenu.gameObject.SetActive(!winMenu.gameObject.activeSelf);
        goodFoodNum = 0;
        PlayerController_Anemia.goodFoodNum = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Good") || other.tag.Equals("Bad"))
        {
            Destroy(other.gameObject);
        }
    }
}
