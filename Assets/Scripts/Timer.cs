using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    private static Timer instance;

    public static Timer Instance{ get { return instance; } }

    public float timerSecond;

    void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (timerSecond > 0)
        {
            timerSecond -= Time.deltaTime;
            GetComponent<Text>().text = ((int)timerSecond).ToString();
        }
        else
        {
            GetComponent<Text>().text = "00";

        }
    }
}
