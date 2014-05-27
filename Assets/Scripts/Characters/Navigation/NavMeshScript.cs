﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavMeshScript : MonoBehaviour {

	private Transform currentTarget;
	private NavMeshAgent agent;
	
	private List<Transform> listTarget = new List<Transform>();
	private bool executeActions;
	private bool hasReachedTheLastPoint;
	
	// Use this for initialization
	void Start () {
		
		agent = this.GetComponent<NavMeshAgent>();
		executeActions = false;
		hasReachedTheLastPoint = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(executeActions)
		{
			// if there are targets in the list but none is selected yet
			if(listTarget.Count > 0 && currentTarget == null)
				setTarget();
			
			// if a target has been defined
			if(currentTarget != null)
			{
				// move the object
				agent.SetDestination(currentTarget.position);
				
				// get the position of the object and the target
				Vector3 objectPos = this.transform.position;
				Vector3 targetPos = currentTarget.position;
				
				objectPos.y = 0.0f;
				targetPos.y = 0.0f;
				
				// if the object has reached the target, the directionPoint is destroyed, the target is removed  from the list
				if(Vector3.Distance(objectPos, targetPos) <=1)
				{
					DestroyImmediate(listTarget[0].gameObject);
					listTarget.RemoveAt(0);
					currentTarget = null;
				}
				
				if(listTarget.Count == 0)
					hasReachedTheLastPoint = true;
				else
					hasReachedTheLastPoint = false;
				
				this.transform.GetChild(1).GetComponent<PathTracerScript>().deleteLine(objectPos);
			}
		}
	}
	
	public void addTarget(GameObject pos)
	{
		listTarget.Add(pos.transform);
		
		this.transform.GetChild(1).GetComponent<PathTracerScript>().addCheckpoint(pos);
	}
	
	private void setTarget()
	{
		currentTarget = listTarget[0];
	}
	
	public void setExecuteActions(bool execute)
	{
		executeActions = execute;
		
		if(listTarget.Count == 0)
			hasReachedTheLastPoint = true;
		else
			hasReachedTheLastPoint = false;
	}
	
	public bool hasFinishedItsPath()
	{
		return hasReachedTheLastPoint;
	}
}
