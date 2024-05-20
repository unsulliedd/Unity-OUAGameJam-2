using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class EnemyAgent : Agent
{
    public Transform player;
    public Transform[] obstacles;
    public float moveSpeed = 2f;
    public float turnSpeed = 300f;
    private Rigidbody rb;
    private int observationSize; // To store the dynamic observation size

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        observationSize = 6 + obstacles.Length * 3; // 6 for enemy position, player position, and direction to player
    }

    public override void OnEpisodeBegin()
    {
        // Reset the enemy and player positions, and any other necessary state
        transform.localPosition = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f));
        player.localPosition = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Add the enemy's position (3 observations)
        sensor.AddObservation(transform.localPosition);
        // Add the player's position (3 observations)
        sensor.AddObservation(player.localPosition);
        // Add the direction to the player (3 observations)
        sensor.AddObservation((player.localPosition - transform.localPosition).normalized);
        // Add directions to each obstacle (3 observations per obstacle)
        foreach (var obstacle in obstacles)
        {
            sensor.AddObservation((obstacle.localPosition - transform.localPosition).normalized);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousActions = actions.ContinuousActions;
        float moveX = continuousActions[0];
        float moveZ = continuousActions[1];

        // Move towards the player
        Vector3 moveDirection = (player.position - transform.position).normalized;
        Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + move);

        // Rotate towards the player
        Quaternion toRotation = Quaternion.LookRotation(moveDirection);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetReward(1.0f); // Reward for reaching the player
            EndEpisode();
            Debug.Log("Enemy reached the player!");
        }
        else if (other.CompareTag("Obstacle"))
        {
            SetReward(-1.0f); // Penalty for hitting an obstacle
            EndEpisode();
            Debug.Log("Enemy hit an obstacle!");
        }
    }
}
