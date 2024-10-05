using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem ;
using UnityEngine . UI ;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    public float speed;
    private int count;
    public int numPickups = 7;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;

    void Start (){
        count = 0;
        winText.text = "";
        SetCountText();
    }

    void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate() {
        Vector3 movement = new(moveValue.x, 0.0f, moveValue.y);

        GetComponent<Rigidbody>().AddForce(speed * Time.fixedDeltaTime * movement);
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("PickUp")) {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void SetCountText() {
        scoreText.text = "Score: " + count.ToString();
        if(count >= numPickups) {
            winText.text = "You win!!";
            scoreText.text = "";
        }
    }
}
