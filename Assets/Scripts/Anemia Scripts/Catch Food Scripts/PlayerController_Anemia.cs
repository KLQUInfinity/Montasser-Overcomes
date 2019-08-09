using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class PlayerController_Anemia : MonoBehaviour
{

    //movement variables
    public float maxSpeed;

    private Rigidbody2D myRB;
    private Animator myAnim;
    private bool facingRight;

    public Slider healthSlider;
    public Text scoreText;

    public static int goodFoodNum;

    private float move;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        facingRight = true;

        goodFoodNum = 0;
    }

    void FixedUpdate()
    {
        move = CrossPlatformInputManager.GetAxis("Horizontal");
        myAnim.SetFloat("speed", Mathf.Abs(move));


        myRB.velocity = new Vector2(move * maxSpeed, myRB.velocity.y);


    }

    void LateUpdate()
    {
        if (move > 0 && !facingRight)
        {
            flip();
        }
        else if (move < 0 && facingRight)
        {
            flip();
        }
    }

    private void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Good"))
        {
            Destroy(other.gameObject);
            string[] s = other.gameObject.name.Split('_');
            s[1] = s[1].Remove(s[1].IndexOf('('));

            goodFoodNum++;

            scoreText.text = (int.Parse(scoreText.text) + int.Parse(s[1])).ToString();
        }
        else if (other.tag.Equals("Bad"))
        {
            Destroy(other.gameObject);
            string[] s = other.gameObject.name.Split('_');
            s[1] = s[1].Remove(s[1].IndexOf('('));

            healthSlider.value -= int.Parse(s[1]);
        }
    }
}
