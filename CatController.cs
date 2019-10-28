using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatController : MonoBehaviour
{

    private Rigidbody2D rd2d;
    private int score;
    private int lives;
    private int level;
    private int count;
    private bool facingRight = true;

    public float speed;
    public Text scoreText;
    public Text livesText;
    public Text countText;
    public Text winText;
    public Text loseText;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;

    Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score = 0;
        lives = 3;
        level = 1;
        count = 0;
        winText.text = "";
        loseText.text = "";
        SetScore();
        SetLives();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (verMovement > 0)
        {
            anim.SetInteger("State", 2);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            if (anim.GetInteger("State") != 2)
            {
                anim.SetInteger("State", 1);
            }
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            if (anim.GetInteger("State") != 2)
            {
                anim.SetInteger("State", 0);
            }
        }
    }
       
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            score = score + 1;
            SetScore();
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lives = lives - 1;
            SetLives();
        }

        if (other.gameObject.CompareTag("Boop"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCount();
        }


        if (score == 4 && level == 1)
        {
            level = level + 1;
            transform.position = new Vector2(-100f, 0f);
            lives = 3;
            SetLives();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.tag == "Ground")
        {
            if (anim.GetInteger("State") == 2)
            {
                anim.SetInteger("State", 0);
            }

            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);            
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                anim.SetInteger("State", 2);
            }
            
            //if(Input.GetKeyUp(KeyCode.W))
            //{
              //  //anim.SetInteger("State", 0);
            //}
        }
    }

        private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void SetScore()
    {
        scoreText.text = "Score: " + score.ToString();
        if (score >= 8)
        {
            transform.position = new Vector2(-100f, 100f);
            winText.text = "You Win!! Game created by David Kingsley!!";
            musicSource.Stop();
        }
    }

    void SetLives()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives <= 0)
        {
            transform.position = new Vector2(100f, 0f);
            loseText.text = "You Lose!!";
            musicSource.Stop();
        }
    }

    void SetCount()
    {
        countText.text = "Count: " + count.ToString();
        if (count == 1)
        {
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}