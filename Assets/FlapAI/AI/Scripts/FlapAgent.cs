using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EasyAgent : Agent
{
    [SerializeField] public Player player;
    private Rigidbody2D birdRigidbody;

    private void Awake() {
        // Get references to components in Awake
        birdRigidbody = player.GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin() {
        player.transform.position = new Vector3(-1.2f, 0f, 0f);
        player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        birdRigidbody.velocity = new Vector2(0f, 0f);
    }


    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(player.transform.position);
        sensor.AddObservation(birdRigidbody.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        if (actions.DiscreteActions[0] == 1) {
            player.Jump();
        }
    }

    public void Entered(Collider2D other) {
        if(other.CompareTag("scoring")) {
            AddReward(1f);
        }

        if (other.CompareTag("ground_training") || other.CompareTag("obstacle_training")) {
            EndEpisode();
        }

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            EndEpisode();
        }
    }

}
