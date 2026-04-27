using UnityEngine;

public class Test_2025137042 : MonoBehaviour
{
    void Start()
    {
        // 5개의 아이템을 저장할 수 있는 인벤토리 생성
        DictionaryHJY<string, string> inventory = new DictionaryHJY<string, string>(5);

        Debug.Log("--------- [Add] 테스트 ---------");
        inventory.Add("Sword", "공격력 : +10");
        inventory.Add("Shield", "방어력 : +5");
        inventory.Add("Potion", "체력 회복 : +20");
        inventory.Add("Sword", "아브라카다브라으라차차차");                   // 중복 키 추가 안되는지 확인용
        Debug.Log($"현재 아이템 개수(Count): {inventory.Count}");            // 3 나와야 정상

        Debug.Log("--------- [GetValue] 테스트 ---------");
        if (inventory.GetValue("Shield", out string shieldDescription))     // 존재하는 키로 시도
        {
            Debug.Log($"Shield 찾기 성공: {shieldDescription}");
        }
        if (!inventory.GetValue("Bow", out string bowDescription))          // 존재하지 않는 키로 시도
        {
            Debug.Log("Bow 찾기 실패: 도감에 없는 아이템입니다.");
        }

        Debug.Log("--------- [Keys & Values] 테스트 ---------");
        foreach (var key in inventory.Keys)                                 // 모든 키 출력
        {
            Debug.Log($"보유 아이템 이름(Key): {key}");                      //Sword, Shield, Potion 나와야 정상
        }
        foreach (var val in inventory.Values)                               // 모든 값 출력
        {
            Debug.Log($"아이템 설명(Value): {val}");                         // 공격력 : +10, 방어력 : +5, 체력 회복 : +20 나와야 정상
        }

        Debug.Log("--------- [Remove] 테스트 ---------");
        bool isRemoved = inventory.Remove("Shield");                        // 존재하는 키로 제거 시도
        Debug.Log($"Shield 삭제 성공 여부: {isRemoved}");                    // Shield 삭제 성공 여부: True 나와야 정상
        Debug.Log($"삭제 후 아이템 개수: {inventory.Count}");                // 2 나와야 정상

        Debug.Log("--------- [Clear] 테스트 ---------");
        inventory.Clear();
        Debug.Log($"Clear 후 아이템 개수: {inventory.Count}");               // 0 나와야 정상
    }
}