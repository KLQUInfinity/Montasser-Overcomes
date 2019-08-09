using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager_Levels : MonoBehaviour
{

    public Transform pauseMenu, loadingContainer;

    // leftBtn,rightBtn and pauseBtn
    public Button[] mainUIBtn;

    public Image[] audioBtns;
    public Sprite[] audioBtnSprites;

    public Slider healthSlider;

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

    public void Toggel_PauseMenu()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;

        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
        foreach (Button i in mainUIBtn)
        {
            i.interactable = !i.interactable;
        }
    }

    public void DieMenu()
    {
        if (healthSlider.value <= 0)
        {
            Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
            pauseMenu.GetChild(0).GetChild(3).gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
            foreach (Button i in mainUIBtn)
            {
                i.interactable = !i.interactable;
            }
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        loadingContainer.gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        loadingContainer.gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(0));
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        loadingContainer.gameObject.SetActive(true);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
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

    public void PauseMenuUI(int index)
    {
        switch (index)
        {
            case 0:
                audioBtns[0].sprite = (audioBtns[0].sprite == audioBtnSprites[1]) ? audioBtnSprites[0] : audioBtnSprites[1];
                int musicValue = (audioBtns[0].sprite == audioBtnSprites[0]) ? 1 : 0;
                PlayerPrefs.SetInt("Music", musicValue);
                break;
            case 1:
                audioBtns[1].sprite = (audioBtns[1].sprite == audioBtnSprites[3]) ? audioBtnSprites[2] : audioBtnSprites[3];
                int soundValue = (audioBtns[1].sprite == audioBtnSprites[2]) ? 1 : 0;
                PlayerPrefs.SetInt("Sound", soundValue);
                break;
        }
    }
}
