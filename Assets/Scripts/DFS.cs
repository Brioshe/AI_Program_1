using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class DFS : MonoBehaviour
{
    PathFinder pathFinder;
    Node Start;
    Node Goal;
    GraphClass Graph;
    GraphView GraphView;
    Stack<Node> FrontierNodes;
    List<Node> ExploredNodes;
    List<Node> PathNodes;

    public bool isComplete;
    public int iterations = 0;

    public void Init(PathFinder pathFinder, GraphClass graph, GraphView graphView, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            Debug.LogWarning("DFS Init error: missing components.");
            return;
        }
        if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("BFS Init error: Start or goal node cannot be a blocked node!");
            return;
        }

        this.pathFinder = pathFinder;
        Graph = graph;
        GraphView = graphView;
        Start = start;
        Goal = goal;
        FrontierNodes = new Stack<Node>();
        FrontierNodes.Push(start);
        ExploredNodes = new List<Node>();
        PathNodes = new List<Node>();

        for (int y = 0; y < Graph.m_height; y++)
        {
            for (int x = 0; x < Graph.m_width; x++)
            {
                Graph.nodes[x, y].Reset();
            }
        }
    }

    public IEnumerator DFSAlgorithm(float timeStep)
    {
        yield return null;
        while (!isComplete)
        {
            if (FrontierNodes.Count > 0)
            {
                Node currentNode = FrontierNodes.Pop();
                ExploredNodes.Add(currentNode);
                iterations++;

                if(FrontierNodes.Contains(Goal))
                {
                    PathNodes = pathFinder.GetPathNodes(Goal);
                    pathFinder.showColors(GraphView, Start, Goal);
                    isComplete = true;
                }

                for (int i = 0; i < currentNode.neighbors.Count; i++)
                {
                    if (!ExploredNodes.Contains(currentNode.neighbors[i]) && !FrontierNodes.Contains(currentNode.neighbors[i]))
                    {
                        FrontierNodes.Push(currentNode.neighbors[i]);
                        ExploredNodes.Add(currentNode.neighbors[i]);
                    }
                }
                yield return new WaitForSeconds(timeStep);
            }
            else
            {
                isComplete = true;
            }
            pathFinder.showColors(GraphView, Start, Goal);
        }
    }
}