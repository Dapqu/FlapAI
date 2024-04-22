using UnityEngine;
using Unity.MLAgents;

public class TrainingPipes : MonoBehaviour
{
    // Speed at which the pipes move to the left
    private float speed = 2.4f;

    private void Update() {
        // Check if the game state is not GameOver before moving the pipes
        if (GameManager.instance.state != GameManager.States.GameOver) {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    private void Start() {
        Transform bottom = transform.Find("SPR_BottomPipe");
        Transform top = transform.Find("SPR_TopPipe");
        float width = Academy.Instance.EnvironmentParameters.GetWithDefault("pipe_width", 1.6f);
        float added_pos = (width - 1.6f) / 2f;
        bottom.transform.position += Vector3.down * added_pos;
        top.transform.position += Vector3.up * added_pos;
    }
}