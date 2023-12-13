using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : MonoBehaviour
{
    public GameObject Apple;
    public Vector3 ApplePosition;
    public List<Vector3> ResultPath;
    public List<float> ResultF;
    private float q;
    public int LoopCount = 0;

    public class Node
    {
        public Node Parent;
        public Vector3 NodePos;
        public float f;

        public Node(Node Par, Vector3 CurrentPos, Vector3 ApplePos)
        {
            Parent = Par;
            NodePos = CurrentPos;
            f = Vector3.Distance(ApplePos, NodePos);
        }
    }

    private bool SuccessorCheck(Node Successor, List<Node> OL, List<Node> CL)
    {
        if (Successor.Parent != null && Successor.Parent.Parent != null && Successor.NodePos == Successor.Parent.Parent.NodePos){
            return false;
        }
        for (int i = 0; i < OL.Count; i++)
        {
            if (OL[i].NodePos == Successor.NodePos && OL[i].f < Successor.f){
                return false;
            }
        }
        List<Transform> SnakeSegments = this.GetComponent<PlayerScript>()._segments;
        for (int j = 0; j < CL.Count; j++)
        {
            if(CL[j].NodePos == Successor.NodePos && CL[j].f < Successor.f){
                return false;
            }
        }
        for (int k = 0; k < SnakeSegments.Count; k++)
        {
            if (SnakeSegments[k].position == Successor.NodePos){
                return false;
            }
        }
        return true;
    }

    public void AStar()
    {
        LoopCount = 0;
        ApplePosition = Apple.transform.position;
        ResultPath.Clear();

        //Initializing Open and Closed Lists
        List<Node> OpenList = new List<Node>();
        List<Node> ClosedList = new List<Node>();

        //Putting Current Position Into Open List
        Node TempNode = new Node(null, this.transform.position, Apple.transform.position);
        TempNode.f = 0;

        OpenList.Add(TempNode);

        //While Open List is Not Empty
        while(OpenList.Count != 0){
            LoopCount++;
            if(LoopCount > 10000){
                for (int i = 0; i < ClosedList.Count; i++){
                    ResultPath.Add(ClosedList[i].NodePos);
                    ResultF.Add(ClosedList[i].f);
                }
                print("Fucked it");
                return;
            }
            //Find Node with Lowest F on Open List
            TempNode = OpenList[0];
            q = TempNode.f;
            for (int i = 1; i < OpenList.Count; i++){
                if (OpenList[i].f < q){
                    TempNode = OpenList[i];
                    q = TempNode.f;
                }
            }

            //Pop q off Open List
            OpenList.Remove(TempNode);

            //Add q to Closed List
            ClosedList.Add(TempNode);

            //Generate Successors and Set Parent to q
            //If Node in OpenList same Position but <f, skip Successor
            //If Node in ClosedList same Position but <f, skip Successor
            //If Successor same position as snake, skip Successor
            //If Successor position same as Apple, Stop Search
            //Otherwise add to openlist            
            Node UpNode = new Node(TempNode, TempNode.NodePos + new Vector3(0, 1, 0), Apple.transform.position);
            if (UpNode.NodePos == Apple.transform.position){
                ClosedList.Add(UpNode);
                break;
            }
            if (SuccessorCheck(UpNode, OpenList, ClosedList)){
                OpenList.Add(UpNode);
            }
            Node DownNode = new Node(TempNode, TempNode.NodePos - new Vector3(0, 1, 0), Apple.transform.position);
            if (DownNode.NodePos == Apple.transform.position){
                ClosedList.Add(DownNode);
                break;
            }
            if (SuccessorCheck(DownNode, OpenList, ClosedList)){
                OpenList.Add(DownNode);
            }
            Node LeftNode = new Node(TempNode, TempNode.NodePos - new Vector3(1, 0, 0), Apple.transform.position);
            if (LeftNode.NodePos == Apple.transform.position){
                ClosedList.Add(LeftNode);
                break;
            }
            if (SuccessorCheck(LeftNode, OpenList, ClosedList)){
                OpenList.Add(LeftNode);
            }
            Node RightNode = new Node(TempNode, TempNode.NodePos + new Vector3(1, 0, 0), Apple.transform.position);
            if (RightNode.NodePos == Apple.transform.position){
                ClosedList.Add(RightNode);
                break;
            }
            if (SuccessorCheck(RightNode, OpenList, ClosedList)){
                OpenList.Add(RightNode);
            }
        }
        
        //transform closedlist to vector3 list
        for (int i = 0; i < ClosedList.Count; i++){
            ResultPath.Add(ClosedList[i].NodePos);
        }
    }
}
