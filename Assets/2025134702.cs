using System.Collections.Generic;

/*
 딕셔너리 기능 리스트업
    - Add(key, value): 새로운 키와 값을 딕셔너리에 추가
    - Remove(key): 특정 키에 해당하는 데이터를 딕셔너리에서 제거
    - GetValue(key, out value): 특정 키에 해당하는 데이터를 딕셔너리에서 찾기(성공시 true 와 값을 반환, 실패시 false 반환)
    - Count: 딕셔너리에 담긴 키 - 값 쌍의 개수를 반환
    - Keys: 모든 키를 반환
    - Values: 모든 값을 반환
    - Clear(): 딕셔너리를 초기화하는 메서드
 */

//해시충돌에 관한 설명 참고 사이트 : https://unity-programming-study.tistory.com/28

/// <summary>
/// [딕셔너리 내부 노드 클래스]
/// 서로 동일한 해시값을 가진 키들이 충돌하지 않게끔, 연결 리스트로 노드를 이어주는 역할을 하는 클래스.
/// </summary>
public class HJYHashNode<TKey, TValue> 
{
    public TKey Key;                    // 해당 노드의 키
    public TValue Value;                // 해당 노드의 키에 해당하는 값
    public HJYHashNode<TKey, TValue> Next; // 다음 노드 참조

    /// <summary>
    /// [노드 생성자]
    /// 키와 값을 받아서 노드를 초기화. 다음 노드는 초기에 null로 하여 끝을 표시.
    /// </summary>
    public HJYHashNode(TKey key, TValue value) 
    {
        Key = key;                      // 키 설정
        Value = value;                  // 값 설정
        Next = null;                    // 초기 다음 노드는 없음
    }
}

/// <summary>
/// [자료구조 중간평가용 딕셔너리 클래스]
/// 해시 테이블을 기반으로 한 간단한 딕셔너리 구현.
/// </summary>
public class DictionaryHJY<TKey, TValue>
{
    private HJYHashNode<TKey, TValue>[] buckets;                // 데이터를 저장할 해시 배열

    public DictionaryHJY(int capacity = 16)                     // 딕셔너리 생성자
    {
        buckets = new HJYHashNode<TKey, TValue>[capacity];      // 해시 배열 초기화
        Count = 0;                                              // 초기 개수 설정
    }

    /// <summary>
    /// [Count]
    /// 딕셔너리에 담긴 키 - 값 쌍의 개수를 반환
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// [Keys]- 시간복잡도 : O(buckets.Length + n)
    /// buckets 배열을 돌며 검색하는 과정이 O(buckets.Length)이고, 각 연결리스트를 따라가며 값을 추가하는 과정이 O(n).
    /// </summary>
    public List<TKey> Keys  //딕셔너리에 담긴 모든 "키(TKey)"를 List로 묶어서 반환하는 기능
    {
        get
        {
            List<TKey> keyList = new List<TKey>();                  // 키를 담을 새로운 리스트 생성.
            for (int i = 0; i < buckets.Length; i++)                // 배열 전체 검색.
            {
                HJYHashNode<TKey, TValue> current = buckets[i];     // 각 인덱스의 연결 리스트 시작점.

                // 해시 충돌로 인해 노드 여러개가 연결되어 있을 수도 있으므로 연결리스트를 따라가며 전부 검색.
                while (current != null)                             // 연결 리스트가 끝날 때까지 계속 키 찾고 다음으로 이동하고 반복.
                {
                    keyList.Add(current.Key);                       // 리스트에 키를 추가.
                    current = current.Next;                         // 다음 노드로 이동.
                }
            }
            return keyList;                                         // 결과 : 키 리스트 반환.
        }
    }

    /// <summary>
    /// [Values] - 시간복잡도 : O(buckets.Length + n)
    /// buckets 배열을 돌며 검색하는 과정이 O(buckets.Length)이고, 각 연결리스트를 따라가며 값을 추가하는 과정이 O(n).
    /// </summary>
    public List<TValue> Values      //딕셔너리에 담긴 모든 "값(TValue)"을 List로 묶어서 반환하는 기능
                                    // Keys와 동일한 방식으로 구현.
    {
        get
        {
            List<TValue> valueList = new List<TValue>();
            for (int i = 0; i < buckets.Length; i++)
            {
                HJYHashNode<TKey, TValue> current = buckets[i];
                while (current != null)
                {
                    valueList.Add(current.Value);
                    current = current.Next;
                }
            }
            return valueList;
        }
    }

    /// <summary>
    /// [GetHash] - 시간복잡도 : O(1)
    /// 키의 해시코드를 가져와서 나머지 연산을 수행하므로, 데이터 양과 상관없이 일정한 시간이 걸림.
    /// </summary>
    private int GetHash(TKey key)   //키를 해시값으로 바꾸고, 배열의 인덱스로 바꿔주는 함수.
    {
        int hash = System.Math.Abs(key.GetHashCode());              // 해시값의 절대값 추출
        return hash % buckets.Length;                               // 배열 크기로 나눈 나머지를 인덱스로 사용
    }

    /// <summary>
    /// [Add] - 시간복잡도 : O(1), 최악의 경우 O(n)
    /// 맨 앞에 추가하는 방식으로 일반적으로는 O(1) 이지만, 해시 충돌이 많아져서 연결 리스트가 길어지는 경우에는 연결 리스트를 따라가며 검색해야 하므로 O(n)까지 늘어날 수 있음.
    /// </summary>
    public void Add(TKey key, TValue value)     //키의 해시값을 계산하여 해당 인덱스에 노드를 추가하는 메서드.
    {
        int index = GetHash(key);                                   // 인덱스 계산
        HJYHashNode<TKey, TValue> current = buckets[index];         // 해당 인덱스의 연결 리스트 시작점

        while (current != null)                                     // 키가 중복하지 않을 때까지 연결 리스트를 따라가며 검색.
        {
            if (current.Key.Equals(key))                            // 제네릭 타입이므로 Equals로 비교.
            {
                return;                                             // 덮어쓰지 않고 무시함.
            }
            current = current.Next;                                 // 다음 노드로 이동.
        }

        HJYHashNode<TKey, TValue> newNode = new HJYHashNode<TKey, TValue>(key, value);  //buckets[index]의 연결리스트 맨 앞에 새 노드를 추가.
        newNode.Next = buckets[index];                              // 새 노드의 다음을 현재 맨 앞 노드로 설정
        buckets[index] = newNode;                                   // 배열의 맨 앞을 새 노드로 교체
        Count++;                                                    // 값들의 개수 증가.
    }

    /// <summary>
    /// [Remove] - 시간복잡도 : O(1), 최악의 경우 O(n)
    /// 인덱스를 찾아가는 과정은 O(1) 이지만, 해시 충돌이 많아져서 연결 리스트가 길어지는 경우에는 연결 리스트를 따라가며 검색해야 하므로 O(n)까지 늘어날 수 있음.
    /// </summary>
    public bool Remove(TKey key)    //키를 해시값으로 변환하여 해당 인덱스에서 노드를 찾아 삭제하는 메서드.
    {
        int index = GetHash(key);                                   // 인덱스 계산
        HJYHashNode<TKey, TValue> current = buckets[index];         // 해당 인덱스의 연결 리스트 시작점
        HJYHashNode<TKey, TValue> previous = null;                  // 이전 노드를 저장하는 변수

        while (current != null)                                     // 연결리스트를 따라가며 계속 키를 찾음.
        {
            if (current.Key.Equals(key))                            // 지울 노드를 찾았다면
            {
                if (previous == null)                           // 만약 지울 노드가 연결 리스트의 맨 첫 번째라면
                {
                    buckets[index] = current.Next;                  // 다음 노드를 (지울노드인 가장 첫번째 노드)로 설정하여 첫번째 노드를 떨어뜨림.
                }
                else                                            // 중간이나 끝에 있는 노드라면
                {
                    previous.Next = current.Next;                   // 이전 노드의 Next와 지울 노드의 Next 를 연결하여 지울 노드를 떨어뜨림.
                }
                Count--;                                            // 값들의 개수 감소
                return true;                                        // 삭제 성공 반환
            }

            previous = current;                                     // 다음 노드로 가기 전에 현재 노드를 previous에 저장.
            current = current.Next;                                 // 다음 노드로 이동.
        }

        return false;                                               // 절대 없었으면 false 반환.
    }

    /// <summary>
    /// [GetValue] - 시간복잡도 : O(1), 최악의 경우 O(n)
    /// 인덱스를 찾아가는 과정은 O(1) 이지만, 해시 충돌이 많아져서 연결 리스트가 길어지는 경우에는 연결 리스트를 따라가며 검색해야 하므로 O(n)까지 늘어날 수 있음.
    /// </summary>
    public bool GetValue(TKey key, out TValue value)    //키를 사용하여 해당하는 값을 찾는 메서드.
    {
        value = default(TValue);                                    // 반환할 값을 기본값으로 초기화

        int index = GetHash(key);                                   // 인덱스 계산
        HJYHashNode<TKey, TValue> current = buckets[index];         // 해당 인덱스의 연결 리스트 시작점

        while (current != null)                                     // 연결리스트를 따라가며 계속 키를 찾음.
        {
            if (current.Key.Equals(key))                            // 해당하는 키를 찾으면
            {
                value = current.Value;                              // 값을 담아주고
                return true;                                        // 탐색 성공 반환
            }
            current = current.Next;                                 // 다음 노드로 이동.
        }

        return false;                                               // 절대 없었으면 false 반환.
    }

    /// <summary>
    /// [Clear]- 시간복잡도 : O(buckets.Length + n)
    /// buckets 배열을 돌며 검색하는 과정이 O(buckets.Length)이고, 각 연결리스트를 따라가며 값을 초기화하는 과정이 O(n).
    /// </summary>
    public void Clear()     //딕셔너리를 초기화하는 메서드.
    {
        for (int i = 0; i < buckets.Length; i++)                    // 배열을 돌면서
        {
            buckets[i] = null;                                      // 모든 연결 리스트의 시작점을 null로 설정하여 초기화.
        }
        Count = 0;                                                  // 개수 초기화
    }
}


