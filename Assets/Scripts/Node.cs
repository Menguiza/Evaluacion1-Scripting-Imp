using UnityEngine;
using System.Collections.Generic;

public class Node
{
	public List<Node> neighbours;
	public int x;
	public int y;
	
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
	public Node parent;

	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
		neighbours = new List<Node>();
	}

	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}

	public Node()
	{
		neighbours = new List<Node>();
	}

	public float DistanceTo(Node n)
	{
		if (n == null)
		{
			Debug.LogError("WTF?");
		}

		return Vector2.Distance(
			new Vector2(x, y),
			new Vector2(n.x, n.y)
		);
	}
}
