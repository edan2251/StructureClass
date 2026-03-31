using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DictionaryHJY
{
    private class Node
    {
        public object Key { get; set; }
        public object Value { get; set; }
        public Node Next { get; set; }
        public Node(object key, object value)
        {
            this.Key = key;
            this.Value = value;
            this.Next = null;
        }
    }

    //Bucket 배열
    private Node[] buckets;
    private int size;

    public DictionaryHJY(int size = 32)
    {
        this.buckets = new Node[size];
        this.size = size;
    }

    //Key/Value 엔트리 추가
    public void Add(object key, object value)
    {
        //해시함수를 통해 Bucket 인덱스 계산
        int index = HashFunction(key);

        if (buckets[index] == null)
        {
            buckets[index] = new Node(key, value);
        }
        else
        {
            //연결리스트 앞에 추가
            Node node = new Node(key, value);
            node.Next = buckets[index];
            buckets[index] = node;
        }
    }

    public object Get(object key)
    {
        int index = HashFunction(key);

        Node node = buckets[index];
        while (node != null)
        {
            //연결리스트에서 동일한 키 검색
            if (node.Key == key)
            {
                return node.Value;
            }
            node = node.Next;
        }
        throw new System.Exception("키가 존재하지 않습니다.");
    }

    //Key가 해시테이블에 있는지 체크
    public bool Contains(object key)
    {
        int index = HashFunction(key);

        Node node = buckets[index];
        while (node != null)
        {
            if (node.Key == key)
            {
                return true;
            }
            node = node.Next;
        }
        return false;
    }




    private int HashFunction(object key)
    {
        int h = Mathf.Abs(key.GetHashCode());

        int hash = h & 0xff;
        hash += (h >> 8) & 0xff;

        return hash % size; //Bucket 배열 크기로 나눈 나머지로 인덱스 계산
    }

    

}
