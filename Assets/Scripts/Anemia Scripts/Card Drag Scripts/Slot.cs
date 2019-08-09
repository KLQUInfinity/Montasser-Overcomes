using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IDropHandler
{
    
    public Text scoreText;

    public GameObject item
    {
        get
        {
            if (transform.childCount > 1)
            {
                return transform.GetChild(1).gameObject;
            }
            return null;
        }
    }

    #region IDropHandler implementation

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            Drag_Handeler.itemBeingDragged.transform.SetParent(transform);

            if (gameObject.name == Drag_Handeler.state)
            {
                scoreText.text = (int.Parse(scoreText.text) + 10).ToString();
            }
            else if (gameObject.name != Drag_Handeler.state)
            {
                scoreText.text = (int.Parse(scoreText.text) - 5).ToString();
            }

            Destroy(transform.GetChild(1).gameObject);
            if (!LevelManager_Anemia_DragCard.Instance.Finished && LevelManager_Anemia_DragCard.Instance.SpritesArrList.Count > 0)
            {
                LevelManager_Anemia_DragCard.Instance.Created = false;
            }
            else if (LevelManager_Anemia_DragCard.Instance.SpritesArrList.Count == 0)
            {
                LevelManager_Anemia_DragCard.Instance.Finished = true;
                //LevelManager_Anemia_DragCard.Instance.FinshTheGame();
            }
        }
    }

    #endregion
}
