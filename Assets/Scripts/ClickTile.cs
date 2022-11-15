using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickTile : MonoBehaviour 
{
    public int tileX;
    public int tileY;
    public Pathfinding map;

    void OnMouseUp() 
    {
        Debug.Log ("Click");

        if(EventSystem.current.IsPointerOverGameObject())
            return;

        map.GeneratePathTo(tileX, tileY);
    }

}