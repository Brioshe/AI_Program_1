using System.Collections.Generic;
using UnityEngine;

public class GraphView : MonoBehaviour
{
    public GameObject nodeviewPrefab;
    public Color openColor = Color.white;
    public Color blockedColor = Color.black;
    public NodeView[,] nodeViews;
    public void Init(GraphClass graph)
    {
        if (graph == null)
        {
            Debug.LogWarning("Graphview error! No graph to init!");
            return;
        }

        nodeViews = new NodeView[graph.m_width, graph.m_height];
        foreach(Node n in graph.nodes)
        {
            GameObject instance = Instantiate(nodeviewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeview = instance.GetComponent<NodeView>();
            Debug.Log("Position: " + n.position.x + ", " + n.position.z);
            if (nodeview != null)
            {
                nodeview.Init(n);
                nodeViews[n.xIndex, n.yIndex] = nodeview;
                if (n.nodeType == NodeType.Blocked)
                {
                    nodeview.ColorNode(blockedColor);
                }
                else
                {
                    nodeview.ColorNode(openColor);
                }
            }
        }
    }

    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];
                if (nodeView != null)
                {
                    nodeView.ColorNode(color);
                }
            }
        }
    }
}


