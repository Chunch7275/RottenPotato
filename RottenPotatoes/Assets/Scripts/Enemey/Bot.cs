using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject target;
    private Rigidbody rbTarget;
    private Vector3 wanderTarget = Vector3.zero;

    float currentSpeed => agent.velocity.magnitude;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        if (target != null)
            rbTarget = target.GetComponent<Rigidbody>();
        else
            rbTarget = null;
    }

    public void MoveToTarget()
    {
        if (target == null) return;

        if (rbTarget == null)
            rbTarget = target.GetComponent<Rigidbody>();

        Vector3 targetDir = target.transform.position - transform.position;

        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));

        if ((toTarget > 90 && relativeHeading < 20) || (rbTarget != null && rbTarget.velocity.magnitude < 0.01f))
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + (rbTarget != null ? rbTarget.velocity.magnitude : 0f));
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    public void Wander()
    {
        float wanderRadius = 10f;
        float wanderDistance = Random.Range(-10.0f, 10.0f);
        float wanderJitter = 1f;

        wanderTarget += new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0, Random.Range(-1f, 1f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Seek(transform.position + targetLocal);
    }

    private void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }
}
