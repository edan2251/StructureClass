using System;
using UnityEngine;

public class QueueHJY<T>
{
    private T[] items;
    private int front = 0; // 첫 번째 데이터의 위치
    private int rear = 0;  // 데이터가 삽입될 위치
    private int count = 0; // 현재 데이터 개수 (가득 참/비어있음 구분용)

    public QueueHJY(int capacity = 4)
    {
        items = new T[capacity];
    }

    public void Enqueue(T item)
    {
        // 가득 찼을 경우 확장
        if (count == items.Length)
        {
            Resize(items.Length * 2);
        }

        // rear 위치에 저장 후 순환 이동
        items[rear] = item;
        rear = (rear + 1) % items.Length;
        count++;
    }

    public T Dequeue()
    {
        if (count == 0) Debug.Log("큐가 비어있습니다.");

        // front 위치에서 추출 및 순환 이동
        T item = items[front];
        items[front] = default;
        front = (front + 1) % items.Length;
        count--;
        return item;
    }

    private void Resize(int newSize)
    {
        T[] newArray = new T[newSize];

        // front부터 차례대로 새 배열의 0번부터 복사
        for (int i = 0; i < count; i++)
        {
            newArray[i] = items[(front + i) % items.Length];
        }

        items = newArray;
        front = 0;
        rear = count;
        Debug.Log($"큐 확장됨: {newSize}");
    }

    public int Count => count;
}
