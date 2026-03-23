using UnityEngine;
using System;
using System.Collections.Generic;

public class StackHJY<T>
{
    private List<T> testStack = new List<T>();

    // 데이터 삽입
    public void Push(T item)
    {
        testStack.Add(item);
    }

    // 데이터 삭제 및 반환
    public T Pop()
    {
        if (testStack.Count == 0)
        {
            Debug.LogError("스택이 비어있음");
        }
        int lastIndex = testStack.Count - 1;
        T item = testStack[lastIndex];
        testStack.RemoveAt(lastIndex);
        return item;

    }

    // 상단 데이터 확인 (삭제 X)
    public T Peek()
    {
        if (testStack.Count == 0)
        {
            Debug.LogError("스택이 비어있음");
        }
        return testStack[testStack.Count - 1];
    }

    public int Count => testStack.Count;
}
