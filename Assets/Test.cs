using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ListTest();

        //Debug.Log("=====================================");

        //LinkedListTest();

        //Debug.Log("=====================================");

        DictionaryTest();
    }

    void ListTest()
    {
        //int형을 담는 리스트 생성 (초기 용량 2로 설정)
        ListHJY<int> myNumbers = new ListHJY<int>(2);

        //데이터 추가
        myNumbers.Add(10);
        myNumbers.Add(20);

        Debug.Log($"현재 개수: {myNumbers.Count}, 전체 용량: {myNumbers.Capacity}");
        Debug.Log($"0번 값: {myNumbers[0]}, 1번 값: {myNumbers[1]}");

        //데이터 추가 (용량 초과 발생 -> 내부적으로 배열 크기가 4로 확장됨)
        myNumbers.Add(30);

        Debug.Log($"추가 후 개수: {myNumbers.Count}, 확장된 용량: {myNumbers.Capacity}");

        //인덱서를 이용한 접근 및 수정
        myNumbers[0] = 100; // 첫 번째 값을 100으로 변경

        for (int i = 0; i < myNumbers.Count; i++)
        {
            Debug.Log($"{i}번 위치의 값: {myNumbers[i]}");
        }
    }

    void LinkedListTest()
    {
        //리스트 생성 (문자열 아이템 로그)
        LinkedListHJY<string> cardLog = new LinkedListHJY<string>();

        //AddLast: 획득 로그 추가
        cardLog.AddLast("마법카드 획득");
        cardLog.AddLast("아이템카드 획득");
        cardLog.AddLast("SSSR 초미소녀 카드 획득");
        Debug.Log($"로그 개수: {cardLog.Count}"); // 3나옴

        // Find: 특정 아이템 찾기
        HJYNode<string> targetNode = cardLog.Find("아이템카드 획득");

        if (targetNode != null)
        {
            Debug.Log($"찾은 노드의 값: {targetNode.Value}"); //1이려나

            //Remove: 찾은 노드 삭제 (아이템카드 로그 지우기)
            cardLog.Remove(targetNode);
            Debug.Log("아이템카드를 삭제했습니다.");
        }

        // 전체 한번씩 확인 (Head부터 Next를 따라가며 출력)
        Debug.Log("--- 현재 남은 로그 목록 ---");
        HJYNode<string> current = cardLog.Head;
        while (current != null)
        {
            Debug.Log($"- {current.Value}");
            current = current.Next;
        }

        //Clear 모든 로그 초기화
        cardLog.Clear();
        Debug.Log($"초기화 후 개수: {cardLog.Count}"); // 출력: 0
    }

    void DictionaryTest()
    {
            DictionaryHJY myDictionary = new DictionaryHJY();
    
            //데이터 추가
            myDictionary.Add("Player1", 5125334124124214111);
            myDictionary.Add("Player2", "이게몇점일까요");
            myDictionary.Add("Player3", 300 + "이거도출력돼?");
    
            //데이터 조회
            Debug.Log($"Player1의 점수: {myDictionary.Get("Player1")}");
            Debug.Log($"Player2의 점수: {myDictionary.Get("Player2")}");
            Debug.Log($"Player3의 점수: {myDictionary.Get("Player3")}");
    }
}
