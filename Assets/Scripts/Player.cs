using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	public int tileX;
	public int tileY;

	public Pathfinding map;

	public List<Node> currentPath = null;

	int moveSpeed = 4;
	float _remainingMovement= 4;
	float _movement=4;

	void Update() 
	{
		if(currentPath != null) 
		{
			int currNode = 0;

			while( currNode < currentPath.Count -1) 
			{

				Vector3 start = map.TileCoordToWorldCoord( currentPath[currNode].x, currentPath[currNode].y ) + 
					new Vector3(0, 0, -0.5f) ;
				Vector3 end   = map.TileCoordToWorldCoord( currentPath[currNode+1].x, currentPath[currNode+1].y )  + 
					new Vector3(0, 0, -0.5f) ;

				Debug.DrawLine(start, end, Color.black);
				
				currNode++;
			}
		}
		
		if(Vector3.Distance(transform.position, map.TileCoordToWorldCoord( tileX, tileY )) < 0.1f)
			AdvancePathing();

		transform.position = Vector3.Lerp(transform.position, map.TileCoordToWorldCoord( tileX, tileY ), 5f * Time.deltaTime);
	}

	void AdvancePathing() 
	{
		if(currentPath==null)
			return;

		if(_remainingMovement <= 0)
			return;
		
		transform.position = map.TileCoordToWorldCoord( tileX, tileY );

		_remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y );

		tileX = currentPath[1].x;
		tileY = currentPath[1].y;

		_movement = _movement - map.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y); 
		Debug.Log("movimiento restante:"+ _movement);
		currentPath.RemoveAt(0);
		
		if(currentPath.Count == 1) 
		{
			currentPath = null;
		}
		
	}

	public void NextTurn() 
	{
		_movement = 4;
		while (currentPath!=null && _remainingMovement > 0) 
		{
			AdvancePathing();
		}
		
		_remainingMovement = moveSpeed;
	}
}
