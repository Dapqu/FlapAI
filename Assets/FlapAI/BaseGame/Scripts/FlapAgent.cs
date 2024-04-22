using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EasyAgent : Agent
{
    // References to components
    private BirdControl player;
    private Rigidbody2D birdRigidbody;
    [SerializeField] private Spawn spawner;

    // Variables for normalizing positions
    private float top = 4.5f;
    private float bottom = -2.6f;

    // Normalize a position between top and bottom
    private float Normalize(float pos, float top, float bottom) {
        return (pos - bottom) / (top - bottom);
    }

    Vector3 closestPipe;

    private new void Awake() {
        birdRigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<BirdControl>();
    }

    private void Start() {
        if (GameManager.instance.state == GameManager.States.EnterGame) {
            GameManager.instance.EnterGame();
        }
    }

    // Reset the environment when an episode begins
    public override void OnEpisodeBegin() {
        spawner.Reset();
        GameManager.instance.ResetScore();
        transform.position = new Vector3(-1.2f, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        birdRigidbody.velocity = new Vector2(0f, 0f);
    }

    // Collect observations from the environment
    public override void CollectObservations(VectorSensor sensor) {
        closestPipe = spawner.GetClosestPipePos();
        sensor.AddObservation(Normalize(transform.position.y, top, bottom));
        sensor.AddObservation(birdRigidbody.velocity.y);
        sensor.AddObservation(Normalize(closestPipe.x, 3.2f, -1.2f));
        sensor.AddObservation(Normalize(closestPipe.y, top, bottom));
    }

    // Receive actions and take corresponding actions
    public override void OnActionReceived(ActionBuffers actions) {
        if (actions.DiscreteActions[0] == 1) {
            player.Jump();
        }
    }

    // Handle collisions with scoring, ground, and obstacles
    public void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("scoring")) {
            AddReward(+1f);
        }

        if (other.CompareTag("ground_training") || other.CompareTag("obstacle_training")) {
            AddReward(-0.25f);
            EndEpisode();
        }
    }

    // Provide a heuristic for human control
    public override void Heuristic(in ActionBuffers actions) {
        ActionSegment<int> discreteActions = actions.DiscreteActions;
        discreteActions[0] = Input.GetMouseButtonDown(0) ? 1 : 0;
    }

    private void Update() {
        // Reward based on time survived
        AddReward(+0.5f * Time.deltaTime);

        // End episode if 'L' key is pressed
        if (Input.GetKeyDown(KeyCode.L)) {
            EndEpisode();
        }
    }
}