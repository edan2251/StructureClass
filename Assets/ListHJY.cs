using UnityEngine;

public class ListHJY<T>
{
    private T[] boxes;      //데이터를 담는 배열
    public int Count { get; private set; }      //데이터의 개수
    public int Capacity => boxes.Length;        //배열의 크기

    public ListHJY(int capacity = 4)            //생성자에서 배열의 크기를 설정
    {
        boxes = new T[capacity];                //초기 크기는 4로 설정, 필요에 따라 변경 가능
    }
        
    public void Add(T item)                     //데이터를 추가하는 메서드
    {
        if (Count == boxes.Length)              // 배열이 가득 찼을 때
        {
            T[] newArray = new T[Count * 2];    // 2배 크기의 새로운 배열을 생성
            System.Array.Copy(boxes, newArray, Count);      // 기존 배열의 데이터를 새로운 배열로 복사
            boxes = newArray;                   // 새로운 배열을 기존 배열로 대체
        }

        boxes[Count++] = item;                  // 새로운 데이터를 배열에 추가하고 개수를 증가
    }

    public T this[int index]                    // 인덱서를 통해 배열에 접근할 수 있도록 구현
    {
        get => boxes[index];                    // 인덱스에 해당하는 데이터를 반환
        set => boxes[index] = value;            // 인덱스에 해당하는 위치에 데이터를 설정
    }
}
