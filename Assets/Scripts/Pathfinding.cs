using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Pathfinding : MonoBehaviour 
{

	public GameObject selectedPlayer;

	public TypeTile[] typeTiles;

	int[,] _tiles;
	Node[,] _graph;


	int mapSizeX = 20;
	int mapSizeY = 20;

	void Start() 
	{
		selectedPlayer.GetComponent<Player>().tileX = (int)selectedPlayer.transform.position.x;
		selectedPlayer.GetComponent<Player>().tileY = (int)selectedPlayer.transform.position.y;
		selectedPlayer.GetComponent<Player>().map = this;

		GenerateMapData();
		GeneratePathfindingGraph();
		GenerateMapVisual();
	}

	void GenerateMapData() 
	{
		_tiles = new int[mapSizeX,mapSizeY];
		
		int x,y;
		
		for(x=0; x < mapSizeX; x++) 
		{
			for(y=0; y < mapSizeX; y++) 
			{
				_tiles[x,y] = 0;
			}
		}

		for(x=3; x <= 5; x++) 
		{
			for(y=0; y < 4; y++) 
			{
				_tiles[x,y] = 1;
			}
		}
		
		_tiles[4, 4] = 2;
		_tiles[5, 4] = 2;
		_tiles[6, 4] = 2;
		_tiles[7, 4] = 2;
		_tiles[8, 4] = 2;

		_tiles[4, 5] = 2;
		_tiles[4, 6] = 2;
		_tiles[8, 5] = 2;
		_tiles[8, 6] = 2;

	}

	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY) 
	{
		TypeTile tt = typeTiles[_tiles[targetX,targetY]];

		if(UnitCanEnterTile(targetX, targetY) == false)
			return Mathf.Infinity;

		float cost = tt.movementCost;

		if( sourceX!=targetX && sourceY!=targetY) 
		{
			cost += 0.001f;
		}

		return cost;

	}

	void GeneratePathfindingGraph() 
	{
		_graph = new Node[mapSizeX,mapSizeY];

		for(int x=0; x < mapSizeX; x++) 
		{
			for(int y=0; y < mapSizeX; y++) 
			{
				_graph[x,y] = new Node();
				_graph[x,y].x = x;
				_graph[x,y].y = y;
			}
		}

		for(int x=0; x < mapSizeX; x++) 
		{
			for(int y=0; y < mapSizeX; y++) 
			{
				if(x > 0)
					_graph[x,y].neighbours.Add( _graph[x-1, y] );
				if(x < mapSizeX-1)
					_graph[x,y].neighbours.Add( _graph[x+1, y] );
				if(y > 0)
					_graph[x,y].neighbours.Add( _graph[x, y-1] );
				if(y < mapSizeY-1)
					_graph[x,y].neighbours.Add( _graph[x, y+1] );

			}
		}
	}

	void GenerateMapVisual() 
	{
		for(int x=0; x < mapSizeX; x++) 
		{
			for(int y=0; y < mapSizeX; y++) 
			{
				TypeTile tt = typeTiles[ _tiles[x,y] ];
				GameObject go = (GameObject)Instantiate( tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity );

				ClickTile ct = go.GetComponent<ClickTile>();
				ct.tileX = x;
				ct.tileY = y;
				ct.map = this;
			}
		}
	}

	public Vector3 TileCoordToWorldCoord(int x, int y) 
	{
		return new Vector3(x, y, 0);
	}

	public bool UnitCanEnterTile(int x, int y) 
	{
		return typeTiles[ _tiles[x,y] ].isWalkable;
	}

	public void GeneratePathTo(int x, int y) 
	{
		selectedPlayer.GetComponent<Player>().currentPath = null;

		if( UnitCanEnterTile(x,y) == false ) 
		{
			return;
		}

		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

		List<Node> unvisited = new List<Node>();
		
		Node source = _graph
		[
			selectedPlayer.GetComponent<Player>().tileX, 
			selectedPlayer.GetComponent<Player>().tileY
		];
		
		Node target = _graph
		[
			x, 
			y
		];
		
		dist[source] = 0;
		prev[source] = null;

		foreach(Node v in _graph) 
		{
			if(v != source) 
			{
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			unvisited.Add(v);
		}

		while(unvisited.Count > 0) 
		{
			Node u = null;

			foreach(Node possibleU in unvisited) 
			{
				if(u == null || dist[possibleU] < dist[u]) 
				{
					u = possibleU;
				}
			}

			if(u == target) 
			{
				break;
			}

			unvisited.Remove(u);

			foreach(Node v in u.neighbours) 
			{
				float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
				if( alt < dist[v] ) 
				{
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

		if(prev[target] == null) 
		{
			return;
		}

		List<Node> currentPath = new List<Node>();

		Node curr = target;

		while(curr != null) 
		{
			currentPath.Add(curr);
			curr = prev[curr];
		}

		currentPath.Reverse();

		selectedPlayer.GetComponent<Player>().currentPath = currentPath;
	}

}