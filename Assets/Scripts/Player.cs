using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] int Hp = 10;
    [SerializeField] GameObject HpBar;
    [SerializeField] Text ScoreText;
    [SerializeField] Button ReplayButton;
    GameObject currentFloor;
    int score = 0;
    float scoreTime = 0f;
    Animator anim;
    SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        Hp = 10;
        score = 0;
        scoreTime = 0f;
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player when the arrow keys are pressed
        if (Input.GetKey(KeyCode.RightArrow))
        {
            render.flipX = false;
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            anim.SetBool("run", true);
        } 
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            render.flipX = true;
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }
        // Update the player's score
        UpdateScore();
    }

    // Called when the player collides with another object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player collides with a normal floor
        if (collision.gameObject.CompareTag("Normal"))
        {
            // Only do this if the player is above the floor
            if (collision.relativeVelocity.y > 0)
            {
                currentFloor = collision.gameObject;
                ModifyHp(1);
                collision.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        // If the player collides with a nail floor
        else if (collision.gameObject.CompareTag("Nail"))
        {
            // Only do this if the player is above the floor
            if (collision.relativeVelocity.y > 0)
            {
                currentFloor = collision.gameObject;
                ModifyHp(-3);
                anim.SetTrigger("hurt");
                collision.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        // If the player collides with the ceiling
        else if (collision.gameObject.CompareTag("Ceiling"))
        {
            // Disable the collider on the current floor
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHp(-3);
            anim.SetTrigger("hurt");
            collision.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    // Called when the player collides with a trigger
    void OnTriggerEnter2D(Collider2D collider)
    {
        // If the player collides with the death line
        if (collider.gameObject.CompareTag("Death"))
        {
            ModifyHp(-10);
        }
    }

    // Modify the player's health
    public void ModifyHp(int amount)
    {
        Hp += amount;
        if (Hp > 10)
        {
            Hp = 10;
        }
        else if (Hp <= 0)
        {
            Hp = 0;
            Die();
        }
        UpdateHpBar();
    }

    // Update the HpBar
    void UpdateHpBar()
    {
        // Loop through all the HpBar's children
        for(int i = 0; i < HpBar.transform.childCount; i++)
        {
            // If the child's index is less than the player's Hp, enable it
            if (i < Hp)
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            // Otherwise, disable it
            else
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // Update the player's score
    void UpdateScore()
    {
        scoreTime += Time.deltaTime;
        if (scoreTime >= 2f)
        {
            scoreTime = 0f;
            score++;
            ScoreText.text = "地下" + score + "層";
        }
    }

    // Player died
    void Die()
    {
        gameObject.GetComponent<AudioSource>().Play();
        Time.timeScale = 0f;
        ReplayButton.gameObject.SetActive(true);
    }

    // Replay the game
    public void Replay()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
