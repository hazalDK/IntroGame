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
    public TextMeshProUGUI playerVelocity;
    public TextMeshProUGUI playerPosition;

    private Vector3 lastPosition;  // To store the player's last position
    public Vector3 velocity;      // To store the calculated velocit

    void Start (){
        count = 0;
        winText.text = "";
        SetCountText();
        lastPosition = transform.position;
    }

    void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate() {
        Vector3 movement = new(moveValue.x, 0.0f, moveValue.y);

        GetComponent<Rigidbody>().AddForce(speed * Time.fixedDeltaTime * movement);

        velocity = (transform.position - lastPosition)/Time.deltaTime;

        lastPosition = transform.position;
        UpdatePlayerInfo();

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
        }
    }

    void UpdatePlayerInfo() {
        Vector3 playerPos = transform.position;
        // playerVelocity.text = "Player Velocity: " + ((playerPos - lastPosition).magnitude/Time.deltaTime).ToString("F5") + " m/s";
        playerVelocity.text = "Player Velocity: " + velocity.magnitude.ToString("0.00") + " m/s";
        playerPosition.text = "Player Position: " + transform.position.ToString("0.00");
    }
}
