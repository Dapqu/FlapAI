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
    [SerializeField] private Spawn spawner;

    // public gameObject rays;
    // private float[] hits = new float[5];

    private float top = 4.5f;
    private float bottom = -2.6f;
    private float normalizedpos = 1.2f;
    private float normalize(float pos, float top, float bottom) {
        return (pos - bottom) / (top - bottom);
    }

    private float closestPipe;

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
        spawner.Reset();
        GameManager.instance.ResetScore();
        transform.position = new Vector3(-1.2f, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        birdRigidbody.velocity = new Vector2(0f, 0f);
    }


    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(normalizedpos);
        sensor.AddObservation(birdRigidbody.velocity.y);
        sensor.AddObservation(closestPipe);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        if (actions.DiscreteActions[0] == 1) {
            player.Jump();
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("scoring")) {
            AddReward(+1f);
        }

        if (other.CompareTag("ground_training") || other.CompareTag("obstacle_training")) {
            AddReward(-0.5f);
            EndEpisode();
        }

    }

    public override void Heuristic(in ActionBuffers actions) {
        ActionSegment<int> discreteActions = actions.DiscreteActions;
        discreteActions[0] = Input.GetMouseButtonDown(0) ? 1 : 0;
    }

    private void Update() {
        AddReward(+0.5f * Time.deltaTime);
        closestPipe = spawner.GetClosestPipePos();
        normalizedpos = normalize(transform.position.y, top, bottom);
        if (Input.GetKeyDown(KeyCode.L)) {
            EndEpisode();
        }
    }

}
