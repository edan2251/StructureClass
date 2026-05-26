using System.Collections.Generic;
using System;

public class MaxHeapTest
{
    //arr[0] : 루트 노드
    //배열 생성
    private List<int> arr = new List<int>();

    public void Add(int data)
    {
        //배열의 끝에 데이터 추가.
        arr.Add(data);

        int i = arr.Count - 1;

        while (i > 0)
        {
            //부모 노드의 인덱스 계산.
            int parentIndex = (i - 1) / 2;

            //현재 노드의 값이 부모 노드의 값보다 크면.
            if (arr[i] > arr[parentIndex])
            {
                //값 교환.
                int tmp = arr[i];
                arr[i] = arr[parentIndex];
                arr[parentIndex] = tmp;

                //인덱스 이동 후 다시 비교.
                i = parentIndex;
            }
            else
            {
                //현재 노드의 값이 부모 노드의 값보다 작거나 같으면 종료.
                break;
            }
        }
    }

    public int Remove()
    {
        //배열이 비어있으면 예외처리.
        if (arr.Count == 0)
        {
            throw new ApplicationException("배열에 값이 존재하지 않습니다.");
        }

        //루트 노드의 값 저장.
        int data = arr[0];

        //새로 추가한 노드를 루트 노드로 이동 후 배열의 끝에서 제거.
        arr[0] = arr[arr.Count - 1];
        arr.RemoveAt(arr.Count - 1);

        int i = 0;
        //마지막 노드의 인덱스 저장.
        int last = arr.Count - 1;

        //루트를 타고 내려가기.
        while (i < last)
        {
            //왼쪽 자식 노드 인덱스 계산.
            int child = 2 * i + 1;

            if (child < last && arr[child] < arr[child + 1])
            {
                child++;
            }

            if ( child > last || arr[i] >= arr[child])
            {
                break;
            }

            int tmp = arr[i];
            arr[i] = arr[child];
            arr[child] = tmp;

            i = child;
        }

        return data;
    }
}
