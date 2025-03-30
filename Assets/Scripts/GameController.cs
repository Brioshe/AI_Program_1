using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MapData mapData;
    public GraphClass graph;
    public PathFinder pathFinder;
    public DFS DFSAlgorithm;
    public int startX = 0;
    public int startY = 0;
    public int goalX = 3;
    public int goalY = 1;
    public float timeStep = 0.1f;

    void Start()
    {
        if (mapData != null && graph != null)
        {
            int [,] mapInstance = mapData.MakeMap(); // 2D array of 1's and 0's
            graph.Init(mapInstance); // Convert the above to array of nodes
            GraphView graphView = graph.gameObject.GetComponent<GraphView>();
            if(graphView != null)
            {
                graphView.Init(graph);
            }
            else
            {
                Debug.LogWarning("No graph is found.");
            }
            if (graph.IsWithinBounds(startX, startY) && graph.IsWithinBounds(goalX, goalY) && pathFinder != null)
            {
                Debug.LogWarning("pathfinder init successfully runs");
                Node startNode = graph.nodes[startX, startY];
                Node goalNode = graph.nodes[goalX, goalY];

                // BFS
                //pathFinder.Init(graph, graphView, startNode, goalNode);
                //StartCoroutine(pathFinder.SearchRoutine(timeStep));

                // DFS
                DFSAlgorithm.Init(pathFinder, graph, graphView, startNode, goalNode);
                StartCoroutine(DFSAlgorithm.DFSAlgorithm(timeStep));
            }
        }
    }
}
