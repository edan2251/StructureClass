/*
1. 어떤 자료구조를 사용했는지
   - 그래프를 표현하기 위해 딕셔너리 + 배열 방식을 선택.
   - 다익스트라 알고리즘에서 가장 짧은 거리를 먼저 꺼내기 위해 최소 힙 사용.

2. 해당 자료구조를 선택한 이유
   - 지하철 노선도는 역(정점)과 역 사이의 길(간선)로 이루어져 있으므로 '그래프' 형태가 가장 적합.
   - 수많은 역 중에서 특정 역의 정보를 빠르게 찾기 위해 탐색 속도가 빠른 딕셔너리를 사용.
   - 최소 힙을 사용하면 데이터를 넣고 뺄 때 훨씬 빠르고 효율적으로 최단 거리를 찾을 수 있기 때문에 선택.
*/

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SubwayRouter : MonoBehaviour
{
    /// <summary>
    /// 그래프의 간선을 표현하는 클래스.
    /// 출발역에서 도착역까지 가는 데 필요한 정보들을 하나로 묶어둠.
    /// </summary>
    public class Edge
    {
        public string TargetStation; //연결된 다음 역의 이름
        public int Time;             //다음 역까지 가는 데 걸리는 소요 시간 (초 단위)
        public string Line;          //이 길이 몇 호선인지 (환승 체크를 위해 필요)

        // 생성자 생성 시 데이터 초기화.
        public Edge(string target, int time, string line)
        {
            TargetStation = target;
            Time = time;
            Line = line;
        }
    }

    /// <summary>
    /// 최소 힙을 사용한 우선순위 큐.
    /// 시간이 가장 적게 걸리는 역이 항상 트리의 맨 위에 오도록 정렬.
    /// </summary>
    public class MinHeapPriorityQueue
    {
        //시간, 역 이름으로 구성
        private List<KeyValuePair<int, string>> elements = new List<KeyValuePair<int, string>>();

        public int Count => elements.Count;                     //큐에 남아있는 요소의 개수

        public void Enqueue(int time, string station)           //큐에 데이터 넣고 정렬하기
        {
            elements.Add(new KeyValuePair<int, string>(time, station)); //맨 뒤에 요소 추가

            int index = elements.Count - 1;                     //비교를 위한 바로 이전에 추가된 요소의 인덱스

            while (index > 0)                                   //이전 요소가 존재하는 동안
            {
                int parentIndex = (index - 1) / 2;              //부모 요소의 인덱스 계산(공식)
                if (elements[index].Key >= elements[parentIndex].Key) break; //현재 요소가 부모 요소보다 시간이 더 크거나 같으면 정렬 완료

                var temp = elements[index];                     //서로 위치 교환하는 3줄
                elements[index] = elements[parentIndex];        
                elements[parentIndex] = temp;

                index = parentIndex;                            //교환 후에는 부모 인덱스로 이동해서 계속 비교
            }
        }

        public KeyValuePair<int, string> Dequeue()              //큐에서 가장 짧은 시간, 역 이름을 꺼내는 함수
        {
            var result = elements[0];                           //맨 위에 있는 요소가 가장 짧은 시간이니까 결과로 저장

            elements[0] = elements[elements.Count - 1];         //맨 마지막 요소를 맨 위로 올리고
            elements.RemoveAt(elements.Count - 1);              //마지막 요소 제거

            int index = 0;                                      //맨 위부터 탐색할거니까 0번
            while (index < elements.Count)                      //아직 요소가 남아있는 동안
            {
                int leftChild = 2 * index + 1;                  //왼쪽 자식 인덱스 계산(공식)
                int rightChild = 2 * index + 2;                 //오른쪽 자식 인덱스 계산(공식)
                int smallest = index;                           //가장 작은 요소의 인덱스

                if (leftChild < elements.Count && elements[leftChild].Key < elements[smallest].Key) //왼쪽 자식이 존재하고, 왼쪽 자식이 현재 가장 작은 요소보다 시간이 더 짧으면
                    smallest = leftChild;                       //왼쪽 자식이 가장 작은 요소가 됨

                if (rightChild < elements.Count && elements[rightChild].Key < elements[smallest].Key) //오른쪽 자식이 존재하고, 오른쪽 자식이 현재 가장 작은 요소보다 시간이 더 짧으면
                    smallest = rightChild;                      //오른쪽 자식이 가장 작은 요소가 됨

                if (smallest == index) break;                   //현재 요소가 가장 작은 요소라면 정렬 완료

                var temp = elements[index];                     //서로 위치 교환하는 3줄
                elements[index] = elements[smallest];
                elements[smallest] = temp;

                index = smallest;                               //교환 후에는 가장 작은 요소의 인덱스로 이동해서 계속 비교
            }
            return result;                                      //결과 반환
        }
    }

    [Header("지하철 테스트 설정")]
    public string csvFileName = "FinalExam/subway_data.csv";    //csv 파일 경로
    public string 출발역 = "강남";                               //출발역 예시
    public string 도착역 = "신촌";                               //도착역 예시

    /// <summary>
    /// 지하철 노선도 전체를 담아둘 딕셔너리 기반의 인접 리스트.
    /// Key: 문자열로 된 역 이름
    /// Value: 해당 역과 연결된 다른 역들의 정보
    /// </summary>
    private Dictionary<string, List<Edge>> graph = new Dictionary<string, List<Edge>>();

    private void Start()
    {
        string filePath = Path.Combine(Application.dataPath, csvFileName);

        //데이터 불러와서 그래프 구축 함수 실행
        LoadData(filePath);
    }

    /// <summary>
    /// CSV 파일을 읽어와서 지하철 노선도(그래프) 데이터를 구축하는 함수.
    /// 시간은 초 단위로 변환, 역과 역을 연결.
    /// </summary>
    public void LoadData(string filePath)
    { 
        if (!File.Exists(filePath))                             //파일 존재 여부 검사
        {
            Debug.LogError($"CSV 파일을 찾을 수 없습니다: {filePath}");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);           //파일의 모든 줄을 읽고 각 줄을 쉼표로 쪼개서 배열로 저장

        string prevStation = null;                              //방금 전에 읽은 역 이름 (역간 연결을 위해 기억)
        string prevLine = null;                                 //방금 전에 읽은 호선 (환승 체크를 위해 기억)

        for (int i = 1; i < lines.Length; i++)                  //헤더를 건너뛰고 i=1부터 시작.
        { 
            string[] cols = lines[i].Split(',');                //쉼표로 줄을 쪼개서 저장

            if (cols.Length < 4) continue;                      //예외처리 : 데이터가 부족한 줄 무시

            string currentLine = cols[1].Trim();                //호선 이름
            string currentStation = cols[2].Trim();             //역 이름
            string rawTime = cols[3].Replace("\"", "").Trim();  //시간 데이터 

            int timeInSeconds = 0;                              //최종 시간을 초 단위로 저장할 변수

            if (rawTime.Contains(":"))                          //시간 데이터가 "01:30" 같은 형식일 경우
            {
                string[] parts = rawTime.Split(':');            //":" 을 기준으로 분:초 로 나눔
                int.TryParse(parts[0], out int minutes);        //첫번째를 분으로 변환
                int.TryParse(parts[1], out int seconds);        //두번쨰를 초로 변환
                timeInSeconds = (minutes * 60) + seconds;       //전부 초로 변환해서 저장
            }

            if (timeInSeconds < 0) timeInSeconds = 60;          //(예외처리)시간이 음수로 잘못 입력되면 기본값 60초로 설정

            if (!graph.ContainsKey(currentStation))             //현재 역이 그래프에 없으면
            {
                graph[currentStation] = new List<Edge>();       //새로운 역을 그래프에 추가
            }

            if (prevLine == currentLine && prevStation != null) //이전 역과 현재 역이 같은 호선이고, 이전 역이 존재하면
            {
                graph[prevStation].Add(new Edge(currentStation, timeInSeconds, currentLine));   //이전역 -> 현재역 연결
                graph[currentStation].Add(new Edge(prevStation, timeInSeconds, currentLine));   //현재역 -> 이전역 연결
            }

                                                                //다음 검색을 위해서 이전 데이터 업데이트
            prevStation = currentStation;                       //현재 역을 이전역으로
            prevLine = currentLine;                             //현재 호선을 이전호선으로
        }
    }

    /// <summary>
    /// 다익스트라 알고리즘을 사용하여 두 역 사이의 최단 시간 경로를 찾는 함수.
    /// 최소 힙 사용해서 바로바로 꺼내서 검사 후 저장.
    /// </summary>
    public void FindShortestPath(string startStation, string endStation)
    { 
        if (!graph.ContainsKey(startStation) || !graph.ContainsKey(endStation))     //역 이름이 그래프에 없으면 경고
        {
            Debug.LogWarning("입력한 역 이름이 노선도 데이터에 존재하지 않습니다.");
            return;
        }

        Dictionary<string, int> distances = new Dictionary<string, int>();          //출발역에서 각 역까지 가는데 걸리는 최소 시간을 저장하는 딕셔너리

        Dictionary<string, string> previous = new Dictionary<string, string>();     //바로 직전에 거쳐온 역을 저장하는 딕셔너리

        MinHeapPriorityQueue pq = new MinHeapPriorityQueue();                       //가장 짧은 거리를 먼저 꺼낼 수 있는 최소 힙 사용

        foreach (string station in graph.Keys)                                      //그래프에 있는 모든 역을 순회하면서 데이터 초기화
        {
            distances[station] = int.MaxValue;                                      //아직 가는길 모르니 일단 무한대로 설정
        }

        distances[startStation] = 0;                                                //출발역은 출발지니까 걸리는 시간이 무조건 0
        pq.Enqueue(0, startStation);                                                //출발역을 대기열에 넣고 탐색 시작

        while (pq.Count > 0)                                                        //대기열이 남아있는 동안
        {
            var current = pq.Dequeue();                                             //최소 힙이니까 가장 위에있는 녀석을 꺼내기 (시간, 역이름)
            int currentDist = current.Key;                                          //현재까지 걸린 시간
            string currentStation = current.Value;                                  //현재 역 이름

            if (currentStation == endStation) break;                                //최종 목적지에 도달했으면 바로 종료
            
            if (currentDist > distances[currentStation]) continue;                  //이미 더 빠른 길을 알고 있으면 이 경로는 무시

            foreach (Edge edge in graph[currentStation])                            //인접 역을 전부
            {
                int newDist = currentDist + edge.Time;                              //다음 역으로 갈 때 걸리는 총 시간

                if (newDist < distances[edge.TargetStation])                        //그 예상 시간이 기존에 알고 있던 시간보다 빠르다면
                {
                    distances[edge.TargetStation] = newDist;                        //최소 시간 업데이트
                    previous[edge.TargetStation] = currentStation;                  //역 이름 업데이트

                    pq.Enqueue(newDist, edge.TargetStation);                        //더 빠른 길이므로 pq에 추가
                }
            }
        }

        PrintResult(startStation, endStation, distances, previous);                 //시작 역, 종료 역, 걸린 시간, 이전역들 출력
    }

    /// <summary>
    /// 탐색 결과를 출력하는 함수.
    /// </summary>
    private void PrintResult(string start, string end, Dictionary<string, int> distances, Dictionary<string, string> previous)
    {
        if (distances[end] == int.MaxValue)                                         //(예외처리) 목적지까지의 시간이 무한대라면 가지 못하는 역
        {
            Debug.Log($"{start}역에서 {end}역으로 가는 경로를 찾을 수 없습니다.");
            return;
        }

        List<string> path = new List<string>();                                     //실제 이동한 역들을 순서대로 담을 리스트
        string curr = end;                                                          //도착역부터 시작해서 거꾸로 추적하기 위한 변수

        while (curr != null)                                                        //이전 역이 없을 때까지
        {
            path.Add(curr);                                                         //현재 역을 경로 리스트에 추가
            previous.TryGetValue(curr, out curr);                                   //이전역으로 거슬러 올라가기
        }
        path.Reverse();                                                             //도착역부터 역순으로 저장했으니까 출발역부터 나오도록 순서 뒤집기

        int totalStations = path.Count - 1;                                         //거쳐간 총 역의 개수 (출발역 제외)
        int totalTime = distances[end];                                             //도착역까지 걸린 총 소요 시간 (초 단위)

        string resultLog = "\n=========================================\n[조회 결과]\n"; //조회 결과 헤더

        if (totalStations > 0)                                                      //이동한 역이 하나라도 있다면 환승 및 경로 계산 시작
        {
            string currentLine = "";                                                //현재 탑승 중인 지하철 호선
            int lineTimeSum = 0;                                                    //현재 호선에서 이동한 누적 시간 (초 단위)
            List<string> linePath = new List<string>();                             //현재 호선에서 지나간 역들의 이름 저장

            for (int i = 0; i < path.Count - 1; i++)                                //출발역부터 도착역 직전까지 순서대로 확인
            {
                string u = path[i];                                                 //현재 역
                string v = path[i + 1];                                             //다음으로 갈 역

                Edge connectingEdge = null;                                         //두 역을 이어주는 간선 정보

                foreach (Edge edge in graph[u])                                     //현재 역과 연결된 모든 길을 확인하면서
                {
                    if (edge.TargetStation == v)                                    //가야할 다음역을 찾으면
                    {
                        connectingEdge = edge;                                      //그 길의 정보를 저장하고
                        break;                                                      //탐색 종료
                    }
                }

                if (i == 0)                                                         //첫 역일 경우
                {
                    currentLine = connectingEdge.Line;                              //처음 탑승하는 호선을 설정
                    linePath.Add(u);                                                //첫 출발역을 해당 호선 경로에 추가
                }

                if (connectingEdge.Line != currentLine)                             //현재 호선과 다음으로 가는 역의 호선이 다르다면
                {
                    int m = lineTimeSum / 60;                                       //초단위 시간을 나눠서 분 계산 
                    int s = lineTimeSum % 60;                                       //나머지는 초로 저장
                    string timeStr = (s > 0) ? $"{m}분 {s}초" : $"{m}분";            //초가 있으면 m분s초, 없으면 m분 으로 저장

                    resultLog += $"{currentLine}호선 소요시간 : {timeStr}, 경로 : {string.Join(" -> ", linePath)}\n"; //지금까지 탄 호선 정보 추가
                    resultLog += $"{u} 환승\n";                                     //환승역 이름 출력

                    currentLine = connectingEdge.Line;                              //갈아탄 호선으로 업데이트
                    lineTimeSum = 0;                                                //호선별 시간 0으로 초기화
                    linePath.Clear();                                               //경로 리스트 초기화
                    linePath.Add(u);                                                //환승역부터 경로 다시 시작
                }

                lineTimeSum += connectingEdge.Time;                                 //다음 역으로 가는 데 걸리는 시간을 누적
                linePath.Add(v);                                                    //다음 역을 지나온 경로에 추가
            }

            int finalM = lineTimeSum / 60;                                          //마지막으로 타고 온 호선의 분 계산
            int finalS = lineTimeSum % 60;                                          //마지막으로 타고 온 호선에 초 계산
            string finalTimeStr = (finalS > 0) ? $"{finalM}분 {finalS}초" : $"{finalM}분";                         //초가 있으면 m분s초, 없으면 m분 으로 저장
            resultLog += $"{currentLine}호선 소요시간 : {finalTimeStr}, 경로 : {string.Join(" -> ", linePath)}\n";  //마지막 호선 정보 출력
        }

        int totM = totalTime / 60;                                                  //전체 총 소요 시간 분 계산
        int totS = totalTime % 60;                                                  //전체 총 소요 시간 초 계산
        string totTimeStr = (totS > 0) ? $"{totM}분 {totS}초" : $"{totM}분";         //초가 있으면 m분s초, 없으면 m분 으로 저장

        resultLog += $"총 {totalStations}개 역 이동, 총 소요시간 : {totTimeStr}\n";   //총 이동 역 개수와 걸린 시간 추가
        resultLog += "=========================================";                   //마무리 구분선

        Debug.Log(resultLog);                                                       //최종 완성된 결과 출력
    }

    /// <summary>
    /// 유니티 화면에 간단한 입력 창 UI 함수.
    /// 플레이 모드일 때 화면 좌측 상단에 표시.
    /// </summary>
    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 220, 120), "지하철 최단거리 탐색기");

        GUI.Label(new Rect(20, 40, 50, 20), "출발역:");
        출발역 = GUI.TextField(new Rect(70, 40, 140, 20), 출발역);

        GUI.Label(new Rect(20, 70, 50, 20), "도착역:");
        도착역 = GUI.TextField(new Rect(70, 70, 140, 20), 도착역);

        if (GUI.Button(new Rect(20, 100, 190, 25), "경로 찾기 (Console 확인)"))
        {
            FindShortestPath(출발역, 도착역);
        }
    }
}