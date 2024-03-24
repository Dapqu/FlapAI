using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class HardAgent : Agent
{
    private Player player;

    public float oldGravity { get; private set; }
    public float oldstrength { get; private set; }

    [SerializeField] private BulletSpawner bulletSpawner;
    private Rigidbody2D birdRigidbody;
    [SerializeField] private Spawn spawner;

    // top and bottom of game area, for normalizing player & pipe y positions
    private float top = 4.5f;
    private float bottom = -2.6f;
    private float normalize(float pos, float top, float bottom) {
        return (pos - bottom) / (top - bottom);
    }

    // Pipe and coin closest to player
    Vector3 closestPipe;
    Vector3 closestCoin;

    private void Awake() {
        birdRigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        oldGravity = birdRigidbody.gravityScale;
        oldstrength = player.strength;
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
        birdRigidbody.gravityScale = oldGravity;
        player.strength = oldstrength;
        player.transform.localScale = player.ogScale;
        player.hasBubble = false;
    }


    public override void CollectObservations(VectorSensor sensor) {
        closestPipe = spawner.GetClosestPipePos();
        closestCoin = spawner.GetClosestCoinPos();
        sensor.AddObservation(normalize(transform.position.y, top, bottom));
        sensor.AddObservation(birdRigidbody.velocity.y);
        sensor.AddObservation(normalize(closestPipe.x, 3.2f, -1.2f));
        sensor.AddObservation(normalize(closestPipe.y, top, bottom));
        sensor.AddObservation(normalize(closestCoin.x, 3.2f, -1.2f));
        sensor.AddObservation(normalize(closestCoin.y, top, bottom));
    }

    public override void OnActionReceived(ActionBuffers actions) {
        if (actions.DiscreteActions[0] == 1) {
            player.Jump();
        }
        if (actions.DiscreteActions[1] == 1) {
            bulletSpawner.Shoot();
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("scoring")) {
            AddReward(1f);
        }
        if (other.CompareTag("collectable_training")) {
            AddReward(1f);
        }

        if (other.CompareTag("ground_training") || other.CompareTag("obstacle_training")) {
            // AddReward(-0.25f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actions) {
        ActionSegment<int> discreteActions = actions.DiscreteActions;
        discreteActions[0] = Input.GetMouseButtonDown(0) ? 1 : 0;
        discreteActions[1] = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;
    }

    private void Update() {
        //Reward based on time survived
        // AddReward(+0.5f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.L)) {
            EndEpisode();
        }
    }

}
