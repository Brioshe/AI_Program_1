using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Dijkstra : MonoBehaviour
{
    PathFinder pathFinder;
    Node Start;
    Node Goal;
    GraphClass Graph;
    GraphView GraphView;

    List<Node> FrontierNodes;  // Acts as a priority queue
    List<Node> ExploredNodes;
    List<Node> PathNodes;

    Dictionary<Node, int> distances;

    public bool isComplete;
    public int iterations = 0;
    public int maxStored = 0;

    public void Init(PathFinder pathFinder, GraphClass graph, GraphView graphView, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            Debug.LogWarning("Dijkstra Init error: missing components.");
            return;
        }

        if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("Dijkstra Init error: Start or goal node cannot be a blocked node!");
            return;
        }

        this.pathFinder = pathFinder;
        Graph = graph;
        GraphView = graphView;
        Start = start;
        Goal = goal;

        FrontierNodes = new List<Node>();
        ExploredNodes = new List<Node>();
        PathNodes = new List<Node>();
        distances = new Dictionary<Node, int>();

        // Reset all nodes and initialize distances
        for (int y = 0; y < Graph.m_height; y++)
        {
            for (int x = 0; x < Graph.m_width; x++)
            {
                Node node = Graph.nodes[x, y];
                node.Reset();
                distances[node] = int.MaxValue;
            }
        }

        distances[Start] = 0;
        FrontierNodes.Add(Start);
    }

    public IEnumerator DijkstraAlgorithm(float timeStep)
    {
        while (!isComplete)
        {
            if (FrontierNodes.Count > 0)
            {
                // Select the node with the lowest distance
                Node currentNode = FrontierNodes.OrderBy(n => distances[n]).First();
                FrontierNodes.Remove(currentNode);
                iterations++;

                if (!ExploredNodes.Contains(currentNode))
                {
                    ExploredNodes.Add(currentNode);
                }

                if (currentNode == Goal)
                {
                    PathNodes = pathFinder.GetPathNodes(Goal);
                    pathFinder.showColors(GraphView, Start, Goal, FrontierNodes, ExploredNodes, PathNodes);
                    isComplete = true;
                    break;
                }

                ExpandFrontier(currentNode);

                maxStored = Mathf.Max(maxStored, FrontierNodes.Count);
                pathFinder.showColors(GraphView, Start, Goal, FrontierNodes, ExploredNodes, PathNodes);
                yield return new WaitForSeconds(timeStep);
            }
            else
            {
                isComplete = true;
            }
        }

        Debug.Log("Iterations: " + iterations);
        Debug.Log("Explored Nodes: " + ExploredNodes.Count);
        Debug.Log("Max Frontier: " + maxStored);
    }

    public void ExpandFrontier(Node current)
    {
        foreach (Node neighbor in current.neighbors)
        {
            if (neighbor.nodeType == NodeType.Blocked)
                continue;

            int newDist = distances[current] + 1;

            if (newDist < distances[neighbor])
            {
                distances[neighbor] = newDist;
                neighbor.previous = current;

                if (!FrontierNodes.Contains(neighbor))
                {
                    FrontierNodes.Add(neighbor);
                }
            }
        }
    }
}
