using UnityEngine;

public class HJYNode<T> // 노드 클래스
{
    public T Value;     // 노드가 담고 있는 데이터
    public HJYNode<T> Next; // 다음 노드를 가리키는 참조
    public HJYNode<T> Prev; // 이전 노드를 가리키는 참조

    public HJYNode(T value)     // 생성자에서 노드의 값을 설정
    {
        Value = value;          // 노드의 값 설정
    }
}

public class LinkedListHJY<T>       // 연결 리스트 클래스
{
    public HJYNode<T> Head { get; private set; }        // 리스트의 첫 번째 노드를 가리키는 참조
    public HJYNode<T> Tail { get; private set; }        // 리스트의 마지막 노드를 가리키는 참조
    public int Count { get; private set; }              // 리스트에 담긴 노드의 개수

    public void AddLast(T value)                // 리스트의 끝에 새로운 노드를 추가하는 메서드
    {
        HJYNode<T> newNode = new HJYNode<T>(value); // 새로운 노드 생성

        if (Head == null)       // 리스트가 비어있는 경우, 새 노드가 헤드이자 테일이 됨
        {
            Head = newNode;     // 새 노드가 리스트의 첫 번째 노드가 됨
            Tail = newNode;     // 새 노드가 리스트의 마지막 노드가 됨
        }
        else
        {
            Tail.Next = newNode;    // 현재 테일의 다음 노드로 새 노드를 설정
            newNode.Prev = Tail;    // 새 노드의 이전 노드로 현재 테일을 설정
            Tail = newNode;         // 새 노드가 리스트의 마지막 노드가 됨
        }
        Count++;                    // 리스트에 노드가 하나 추가되었으므로 개수 증가
    }

    public void Remove(HJYNode<T> node)     // 리스트에서 특정 노드를 제거하는 메서드
    {
        if (node.Prev != null)              // 제거할 노드가 리스트의 첫 번째 노드가 아닌 경우, 이전 노드의 다음 노드를 제거할 노드의 다음 노드로 설정
        {
            node.Prev.Next = node.Next;     // 제거할 노드의 이전 노드가 제거할 노드의 다음 노드를 가리키도록 설정
        }
        else
        {
            Head = node.Next;               // 제거할 노드가 리스트의 첫 번째 노드인 경우, 헤드를 제거할 노드의 다음 노드로 설정
        }

        if(node.Next != null)               // 제거할 노드가 리스트의 마지막 노드가 아닌 경우, 다음 노드의 이전 노드를 제거할 노드의 이전 노드로 설정
        {
            node.Next.Prev = node.Prev;     // 제거할 노드의 다음 노드가 제거할 노드의 이전 노드를 가리키도록 설정
        }
        else
        {
            Tail = node.Prev;               // 제거할 노드가 리스트의 마지막 노드인 경우, 테일을 제거할 노드의 이전 노드로 설정
        }

        Count--;                            // 리스트에서 노드가 하나 제거되었으므로 개수 감소
    }

    public HJYNode<T> Find(T value)       // 리스트에서 특정 값을 가진 노드를 찾는 메서드
        {
            HJYNode<T> current = Head;          // 리스트의 첫 번째 노드부터 탐색 시작
    
            while (current != null)             // 리스트의 끝까지 탐색
            {
                if (current.Value.Equals(value)) // 현재 노드의 값이 찾고자 하는 값과 일치하는 경우
                {
                    return current;              // 해당 노드를 반환
                }
                current = current.Next;          // 다음 노드로 이동
            }
            return null;                        // 리스트에 해당 값이 없는 경우 null 반환
    }

    public void Clear()                    // 리스트를 초기화하는 메서드
    {
        Head = null;       // 헤드를 null로 설정하여 리스트를 비움
        Tail = null;       // 테일을 null로 설정하여 리스트를 비움
        Count = 0;         // 노드 개수를 0으로 초기화
    }
}
