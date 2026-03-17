using UnityEngine;

public class QueueHJY<T>
{
    int[] items = new int[10];

    private int front = -1;

    private int rear = -1;

    private void Algorithm()
    {
        items[0] = 5;

        front = 0;
        rear = 0;


        //가득찬 상태인지 확인
        if ((rear + 1) % items.Length == front)
        {
            //배열 확장시켜주고 값 추가?
        }
        else
        {
            //가득 찬 상태가 아니면
            int nextIndex = rear + 1 % items.Length;
            items[nextIndex] = 10;      //값 추가
        }

            //비어있는지 체크

            front = front + 1 % items.Length;

        if(front == rear)
        {
            front = -1;
            rear = -1;
        }
    }

    public void Enqueue()
    {
        //가득찬 상태인지 확인
        if ((rear + 1) % items.Length == front)
        {
            //배열 확장시켜주고 값 추가?
        }
        else
        {
            //가득 찬 상태가 아니면
            int nextIndex = rear + 1 % items.Length;
            items[nextIndex] = 10;      //값 추가
        }
    }

    //public T Dequeue()
    //{

    //}
}
