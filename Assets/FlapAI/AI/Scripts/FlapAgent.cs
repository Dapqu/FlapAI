using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EasyAgent : Agent
{
    private Player player;
    private Rigidbody2D birdRigidbody;

    private float top = 4.5f;
    private float bottom = -2.6f;
    private float normalizedpos = 1.2f;
    private float normalize(float pos, float top, float bottom) {
        return (pos - bottom) / (top - bottom);
    }

    private void Awake() {
        // Get references to components in Awake
        birdRigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    private void Start() {
        if (GameManager.instance.state == GameManager.States.EnterGame) {
            GameManager.instance.EnterGame();
        }
    }

    public override void OnEpisodeBegin() {
        transform.position = new Vector3(-1.2f, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        birdRigidbody.velocity = new Vector2(0f, 0f);
    }


    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(normalizedpos);
        sensor.AddObservation(birdRigidbody.velocity.y);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        if (actions.DiscreteActions[0] == 1) {
            player.Jump();
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("scoring")) {
            //AddReward(1f);
        }

        if (other.CompareTag("ground_training") || other.CompareTag("obstacle_training")) {
            AddReward(-0.5f);
            EndEpisode();
        }

    }

    private void Update() {
        AddReward(1.0f * Time.deltaTime);
        normalizedpos = normalize(transform.position.y, top, bottom);
        if (Input.GetKeyDown(KeyCode.L)) {
            EndEpisode();
        }
    }

}
