using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PathFinder : MonoBehaviour
{
    Node m_startNode;
    Node m_goalNode;
    GraphClass m_graph;
    GraphView m_graphView;
    Queue<Node> m_frontierNodes;
    List<Node> m_exploredNodes;
    List<Node> m_pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;
    public bool isComplete;
    public int iterations = 0;
    public void Init(GraphClass graph, GraphView graphView, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            Debug.LogWarning("BFS Init error: missing components.");
            return;
        }
        if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("BFS Init error: Start or goal node cannot be a blocked node!");
            return;
        }

        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_goalNode = goal;
        m_frontierNodes = new Queue<Node>();
        m_frontierNodes.Enqueue(start);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();

        for (int y = 0; y < m_graph.m_height; y++)
        {
            for (int x = 0; x < m_graph.m_width; x++)
            {
                m_graph.nodes[x, y].Reset();
            }
        }

        ShowColors();

        isComplete = false;
        iterations = 0;
    }

    public void showColors(GraphView graphView, Node start, Node goal)
    {
        if (graphView == null || start == null || goal == null)
        {
            return;
        } 

        NodeView startNodeView = m_graphView.nodeViews[start.xIndex, start.yIndex];
        NodeView goalNodeView = m_graphView.nodeViews[goal.xIndex, goal.yIndex];

        if (m_frontierNodes != null)
        {
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }
        if (m_exploredNodes != null)
        {
            graphView.ColorNodes(m_exploredNodes, exploredColor);
        }
        if (m_pathNodes != null)
        {
            graphView.ColorNodes(m_pathNodes, pathColor);
        }
        if (startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }
        if (goalNodeView != null)
        {
            goalNodeView.ColorNode(goalColor);
        }
    }

    public void ShowColors()
    {
        showColors(m_graphView, m_startNode, m_goalNode);
    }

    // Breadth First Search
    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {
        yield return null;
        while (!isComplete)
        {
            if (m_frontierNodes.Count > 0)
            {
                Node currentNode = m_frontierNodes.Dequeue();
                iterations++;
                if (!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                ExpandFrontier(currentNode);
                if(m_frontierNodes.Contains(m_goalNode))
                {
                    m_pathNodes = GetPathNodes(m_goalNode);
                    isComplete = true;
                }
                yield return new WaitForSeconds(timeStep);
            }
            else
            {
                isComplete = true;
            }
            ShowColors();
        }
    }
    private void ExpandFrontier(Node node)
    {
        for (int i = 0; i < node.neighbors.Count; i++)
        {
            if (!m_exploredNodes.Contains(node.neighbors[i]) && !m_frontierNodes.Contains(node.neighbors[i]))
            {
                node.neighbors[i].previous = node;
                m_frontierNodes.Enqueue(node.neighbors[i]);
            }
        }
    }

    public List<Node> GetPathNodes(Node goalNode)
    {
        List<Node> path = new List<Node>();
        if (goalNode == null)
        {
            return path;
        }
        path.Add(goalNode);
        Node currentNode = goalNode.previous;
        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }
        return path;
    }
}

