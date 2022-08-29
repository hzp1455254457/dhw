using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStarManager
{
    public static AstarPath pathfinding;
     static AStarManager() {

        pathfinding = GameObject.FindObjectOfType<AstarPath>();
}

    public static void CreactWay()
    {
        if(pathfinding!=null)
        pathfinding.Scan();
    }
}
