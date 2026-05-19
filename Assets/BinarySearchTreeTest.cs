using System;
using UnityEngine;

public class BinarySearchTreeTest : MonoBehaviour
{
    /// <summary>
    /// 이진트리
    /// 왼쪽 < 부모 < 오른쪽
    /// </summary>

    public class TreeNode
    {
        public int Value;
        public TreeNode Left;
        public TreeNode Right;

        public TreeNode(int value)
        {
            Value = value;
        }
    }

    private TreeNode rootNode = null;

    public void Add(int data)
    {
        if(rootNode == null)
        {
            rootNode = new TreeNode(data);
            return;
        }

        TreeNode node = rootNode;

        while(node != null)
        {
            //data가 현재 노드랑 같은지.
            if(node.Value == data)
            {
                throw new
                    ApplicationException("중복입니다");
            }
            //다르면 data가 현재 노드보다 작은지.
            else
            {
                //작으면 왼쪽에 넣음
                if (data < node.Value)
                {
                    if (node.Left == null)
                    {
                        node.Left = new TreeNode(data);
                        return;
                    }
                    else
                    {
                        node = node.Left;
                    }
                }
                //크면 오른쪽에 넣음
                else
                {
                    if (node.Right == null)
                    {
                        node.Right = new TreeNode(data);
                        return;
                    }
                    else
                    {
                        node = node.Right;
                    }
                }
            }
        }
    }

    public bool Search(int data)
    {
        TreeNode node = rootNode;

        while (node != null)
        {
            if(node.Value == data)
            {
                return true;
            }
            else
            {
                if(node.Value > data)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }
        }
        return false;
    }
    
    public void Remove(int data)
    {
        TreeNode node = rootNode;
        TreeNode prevNode = null;

        while (node != null)
        {
            if(node.Value == data)
            {
                break;
            }
            else
            {
                if (node.Value > data)
                {
                    prevNode = node;
                    node = node.Left;
                }
                else
                {
                    prevNode = node;
                    node = node.Right;
                }
            }
        }

        //case1 : 삭제 노드의 자식이 없음.
        if (node.Left == null && node.Right == null)
        {
            if (prevNode.Left == node)
            {
                prevNode.Left = null;
            }
            else
            {
                prevNode.Right = null;
            }

            node = null;
        }

        //case2 : 삭제 노드의 자식이 1개 있음.
        else if (node.Left == null || node.Right == null)
        {
            TreeNode childNode = node.Left != null ? node.Left : node.Right;

            if (prevNode.Left == node)
            {
                prevNode.Left = childNode;
            }
            else
            {
                prevNode.Right = childNode;
            }

            node = null;
        }

        //case3 : 삭제 노드의 자식이 2개 이상 있음.
        else
        {
            TreeNode preNode = node;
            TreeNode minNode = node.Right;

            //오른쪽 서브트리에서 가장 작은 노드 찾기.
            while (preNode.Left != null)
            {
                preNode = minNode;
                minNode = minNode.Left;
            }

            //삭제 노드의 값을 가장 작은 노드의 값으로 교체.
            node = minNode;

            if (preNode.Left == minNode)
            {
                preNode.Left = minNode.Right;
            }
            else
            {
                preNode.Right = minNode.Right;
            }
        }
    }
}

