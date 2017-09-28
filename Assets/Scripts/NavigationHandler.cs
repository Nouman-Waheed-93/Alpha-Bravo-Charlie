using UnityEngine;
using System.Collections;

public class NavigationHandler : MonoBehaviour {
	public float patrolSpeed = 2f;							
	public float chaseSpeed = 5f;							
	public float chaseWaitTime = 5f;						
	public float patrolWaitTime = 1f;						
	public Transform[] patrolWayPoints;

	private EnemySight enemySight;						
	private UnityEngine.AI.NavMeshAgent nav;	

	private float chaseTimer;								
	private float patrolTimer;								
	private int wayPointIndex;						

	void Patrolling ()
	{

		nav.speed = patrolSpeed;
		

		if(nav.remainingDistance < nav.stoppingDistance)
		{
		
			patrolTimer += Time.deltaTime;
			

			if(patrolTimer >= patrolWaitTime)
			{

				if(wayPointIndex == patrolWayPoints.Length - 1)
					wayPointIndex = 0;
				else
					wayPointIndex++;
				

				patrolTimer = 0;
			}
		}
		else

			patrolTimer = 0;
		

		nav.destination = patrolWayPoints[wayPointIndex].position;
	}

	void Chasing ()
	{
		// Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
		
		// If the the last personal sighting of the player is not close...
		if(sightingDeltaPos.sqrMagnitude > 4f)
			// ... set the destination for the NavMeshAgent to the last personal sighting of the player.
			nav.destination = enemySight.personalLastSighting;
		
		// Set the appropriate speed for the NavMeshAgent.
		nav.speed = chaseSpeed;
		
		// If near the last personal sighting...
		if(nav.remainingDistance < nav.stoppingDistance)
		{
			// ... increment the timer.
			chaseTimer += Time.deltaTime;
			
			// If the timer exceeds the wait time...
			if(chaseTimer >= chaseWaitTime)
			{
				// ... reset last global sighting, the last personal sighting and the timer.
				chaseTimer = 0f;
			}
		}
		else
			// If not near the last sighting personal sighting of the player, reset the timer.
			chaseTimer = 0f;
	}

}
