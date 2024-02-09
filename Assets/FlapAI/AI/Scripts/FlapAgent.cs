using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EasyAgent : Agent
{
    private Player player;

    private void Awake() {
        // Get references to components in Awake
        player = GetComponent<Player>();
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(player.birdRigidbody.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        Debug.Log(actions.DiscreteActions[0]);
    }
}
