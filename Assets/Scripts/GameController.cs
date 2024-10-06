using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI distanceText; // Reference to the UI text element for distance
    public TextMeshProUGUI playerVelocity;
    public TextMeshProUGUI playerPosition;
    public GameObject[] pickups; // Array to hold all pickup objects
    public GameObject player; // the player object
    private int currentDebugMode; // 0: Normal, 1: Distance, 2: Vision
    private LineRenderer lineRenderer; // LineRenderer to visualize the distance and velocity
    public Vector3 startPosition;
    public Vector3 endPosition;
    public PlayerController playerController;

    void Start()
    {
        // Initialize LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        startPosition = player.transform.position;
        // Width of 0.1 f both at origin and end of the line
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        currentDebugMode = 0; // Start in Normal mode
    }

    void Update()
    {
        // Handle Space key input to switch debug modes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentDebugMode = (currentDebugMode + 1) % 3; // Cycle through modes
        }
        startPosition = player.transform.position;

        // Update based on the current debug mode
        switch (currentDebugMode)
        {
            case 0: // Normal mode
                distanceText.text = ""; // No debug info
                playerPosition.text = "";
                playerVelocity.text = "";
                lineRenderer.enabled = false; // Disable the line renderer
                ResetAllPickupsColor();
                break;

            case 1: // Distance mode
                UpdateClosetPickup();
                break;
            
            case 2: // Vision mode
                lineRenderer.enabled = true;
                UpdateClosetPickupVisionMode();
                break;
        }
    }

    // Utility function to reset all active pickups' colors to white
    void ResetAllPickupsColor(){
        foreach (GameObject pickup in pickups){
            if (pickup.activeSelf){ // Only reset color for active pickups
                pickup.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    void UpdateClosetPickup() {
        GameObject closestPickup = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject pickup in pickups){
            if (pickup.activeSelf){
                float distance = Vector3.Distance(player.transform.position, pickup.transform.position);

                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestPickup = pickup;
                }
                ResetAllPickupsColor();
            }
            if (closestPickup != null){
                closestPickup.GetComponent<Renderer>().material.color = Color.blue;
                distanceText.text = "Distance to next pickup: " + closestDistance.ToString("0.00");
                endPosition = closestPickup.transform.position;
            }
        }
    }

        void UpdateClosetPickupVisionMode() {
        GameObject closestPickup = null;
        float closestAngle = Mathf.Infinity;

        foreach (GameObject pickup in pickups){
            if (pickup.activeSelf){
                // Calculate vector from player to pickup
                Vector3 toPickup = pickup.transform.position - startPosition;
                // Calculate the angle between the player's velocity and the direction towards the pickup
                float angle = Vector3.Angle(toPickup, playerController.velocity);

                // Check if this pickup is the one the player is most directly moving towards
                if (angle < closestAngle) {
                    closestAngle = angle;
                    closestPickup = pickup;
                }

                // Ensure all pickups rotate normally
                pickup.transform.Rotate(Vector3.up, 50 * Time.deltaTime);
                ResetAllPickupsColor();
            }
            if (closestPickup != null){
                closestPickup.GetComponent<Renderer>().material.color = Color.green;
                closestPickup.transform.LookAt(player.transform);
                float closestDistance = Vector3.Distance(startPosition, closestPickup.transform.position);
                distanceText.text = "Distance to next pickup: " + closestDistance.ToString("0.00");
                endPosition = closestPickup.transform.position;
                DrawLineToPickup();
            }
        }
    }

    void DrawLineToPickup(){
        // 0 for the start point , position vector ’ startPosition ’
        lineRenderer.SetPosition(0, startPosition);
        // 1 for the end point , position vector ’endPosition ’
        lineRenderer.SetPosition(1, endPosition);
    }
}
