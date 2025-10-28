using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TheStack : MonoBehaviour
{
    private const float BoundSize = 3.5f; // 블럭 사이즈
    private const float MovingBoundsSize = 3f; // 이동량
    private const float StackMovingSpeed = 5.0f; // 이동 속도
    private const float BlockMovingSpeed = 3.5f;
    private const float ErrorMargin = 0.1f; // 성공으로 취급할 에러 마진

    public GameObject originBlock = null;

    private Vector3 prevBlockPosition;
    private Vector3 desiredPosition;
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize); // 새롭게 생성되는 블럭의 사이즈

    Transform lastBlock = null; // 새로운 블럭을 생성하기 위한 변수들
    float blocktransition = 0f;
    float secondaryPositon = 0f;

    int stackCount = -1; // 시작하면서 +1 하면서 사용
    
    public int Score { get { return stackCount; } }

    int comboCount = 0;
    public int Combo { get { return comboCount; } }

    private int maxCombo = 0;

    public int MaxCombo { get => maxCombo; }

    public Color prevColor;
    public Color nextColor;

    bool isMovingX = true;

    public float step = 8f;

    int bestScore = 0;
    public int BestScore { get => bestScore; }

    int bestCombo = 0;
    public int BestCombo { get => bestCombo; }

    private const string BestScoreKey = "BestScore";
    private const string BestComboKey = "BestCombo";

    private bool isGameOver = true;


    void Start()
    {

        bestScore = PlayerPrefs.GetInt(BestComboKey, 0);
        bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

        prevColor = GetRandomColor(); // 최초에 한번 색깔을 설정
        nextColor = GetRandomColor();

        prevBlockPosition = Vector3.down; // down = y값이 -1인 값으로 들어있다.

        Spawn_Block();
        Spawn_Block();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
            return;
        {
            
        }
        if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 클릭, 스마트폰 터치
        {
            if (PlaceBlock())
            {
                Spawn_Block();
            }
            else
            {
                // 게임 오버
                Debug.Log("Game Over");
                Updatescore();
                isGameOver = true;
                GameOverEffect();
                TheStackUIManager.Instance.SetScoreUI();
            }

        }
        MoveBlock();
        transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);
        // 현재 위치, 목적지의 위치, 일정한 퍼센테이지 매 프레임마다 지속적으로 desiredPosition으로 이동 할 수 있게 만들어준다.
    }

    bool Spawn_Block()
    {
        if (lastBlock != null)
            prevBlockPosition = lastBlock.localPosition;

        GameObject newBlock = null;
        Transform newTrans = null;

        newBlock = Instantiate(originBlock); // Instantiate oringinBlock을 복제를 한다. 복제된 오브젝트를 이후 반환한다.

        if (newBlock == null)
        {
            Debug.Log("NewBlklock Instantiate Failed");
            return false;
        }

        ColorChange(newBlock);

        newTrans = newBlock.transform;
        newTrans.parent = this.transform; // 새롭게 생성된 오브젝트의 부모를 나의 트랜스폼으로 가져다준다.
        newTrans.localPosition = prevBlockPosition + Vector3.up;
        newTrans.localRotation = Quaternion.identity; // Quaternion의 초기값 회전이 없는 상태 
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        stackCount++;

        desiredPosition = Vector3.down * stackCount; // 스택 카운트가 증가하는 만큼 The stack이라는 애를 바닥으로 한칸씩 내린다.
        blocktransition = 0f;

        lastBlock = newTrans;

        isMovingX = !isMovingX;

        TheStackUIManager.Instance.UpdateScore();

        return true;


    }

    Color GetRandomColor()
    {
        float r = Random.Range(100f, 250f) / 255f; // 100부터 시작하는 이유는 100보다 낮으면 너무 어두운 수치들이 들어가기 때문
        float g = Random.Range(100f, 250f) / 255f;
        float b = Random.Range(100f, 250f) / 255f;

        return new Color(r, g, b);
    }

    void ColorChange(GameObject go)
    {
        
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % step + 1) / step);

        Renderer rn = go.GetComponent<Renderer>(); // renderer 무언가를 그려내는 작업

        if (rn == null)
        {
            Debug.Log("Renderer is NULL");
            return;
        }
        rn.material.color = applyColor;
        Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f);

        if (applyColor.Equals(nextColor) == true) // 현재의 컬러가 다음의 컬러와 같아진다면
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }

        // Color을 생성한 상태에서 선형 보간을 통해 nextColor와 점점 같아진다.
        // 같아지게 되면 prevColor을 갱신하고 nextColor을 새롭게 생성한다.
    }

    void MoveBlock()
    {
        blocktransition += Time.deltaTime * BlockMovingSpeed;

        float movePosition = Mathf.PingPong(blocktransition, BoundSize) - BoundSize / 2; // PingPong 이란 ? 0부터 우리가 지정해준 사이즈까지 순환하는 값

        if (isMovingX)
        {
            lastBlock.localPosition = new Vector3(
                movePosition * MovingBoundsSize, stackCount, secondaryPositon);
        }
        else
        {
            lastBlock.localPosition = new Vector3(
                secondaryPositon, stackCount, -movePosition * MovingBoundsSize);
        }
    }

    bool PlaceBlock()
    {
        Vector3 lastPosition = lastBlock.localPosition;

        if (isMovingX)
        {
            float deltaX = prevBlockPosition.x - lastPosition.x; // deltax = 잘려나가야하는 값 이탈한 범위
                                                                 // 이전블럭과 현재 블럭이 같기 때문에
            bool isNegativeNum = (deltaX < 0) ? true : false;

            deltaX = Mathf.Abs(deltaX);
            if (deltaX > ErrorMargin)
            {
                stackBounds.x -= deltaX;
                if (stackBounds.x <= 0)
                {
                    return false;
                }
                float middle = (prevBlockPosition.x + lastPosition.x) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.x = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubblehalfScale = deltaX / 2f;
                CreateRubble(
                    new Vector3(
                        isNegativeNum
                        ? lastPosition.x + stackBounds.x / 2 + rubblehalfScale
                        : lastPosition.x - stackBounds.x / 2 - rubblehalfScale
                        , lastPosition.y
                        , lastPosition.z
                        ),
                    new Vector3(deltaX, 1, stackBounds.y)
                    );

                comboCount = 0;
            }
            else
            {
                ComboCheck();
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }
        else
        {
            float deltaZ = prevBlockPosition.z - lastPosition.z;
            bool isNegativeNum = (deltaZ < 0) ? true : false;

            deltaZ = Mathf.Abs(deltaZ);
            if (deltaZ > ErrorMargin)
            {
                stackBounds.y -= deltaZ;
                if (stackBounds.y <= 0)
                {
                    return false;
                }
                float middle = (prevBlockPosition.z + lastPosition.z) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.z = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubblehalfScale = deltaZ / 2f;
                CreateRubble(
                    new Vector3(
                        lastPosition.x,
                        lastPosition.y,
                        isNegativeNum
                        ? lastPosition.z + stackBounds.y / 2 + rubblehalfScale
                        : lastPosition.z - stackBounds.y / 2 - rubblehalfScale),
                    new Vector3(stackBounds.x, 1, deltaZ)
                    );
                comboCount = 0;

            }
            else
            {
                ComboCheck();
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }

        secondaryPositon = (isMovingX) ? lastBlock.localPosition.x : lastBlock.localPosition.z;

        return true;
    }

    void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = Instantiate(lastBlock.gameObject);
        go.transform.parent = this.transform;

        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.transform.localRotation = Quaternion.identity;

        go.AddComponent<Rigidbody>();
        go.name = "Rubble";
    }

    void ComboCheck()
    {
        comboCount++;

        if(comboCount > maxCombo)
            maxCombo = comboCount;

        if ((comboCount % 5) == 0 && comboCount != 0)
        {
            Debug.Log("5 Combo Success!");
            stackBounds += new Vector3(0.5f, 0.5f);
            stackBounds.x =
                (stackBounds.x > BoundSize) ? BoundSize : stackBounds.x;
            stackBounds.y =
                (stackBounds.y > BoundSize) ? BoundSize : stackBounds.y;
        }
    }

    void Updatescore()
    {
        if(bestScore < stackCount)
        {
            Debug.Log("최고 점수 갱신");
            bestScore = stackCount;
            bestCombo = comboCount;

            PlayerPrefs.SetInt(BestComboKey, bestCombo);
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
        }
    }

    void GameOverEffect()
    {
        int childCount = this.transform.childCount; // transform에서 하위에 있는 오브젝트의 갯수
                                                    // 이곳에선 TheStack 안에 있는 블럭과, 러블들
        for(int i = 1; i < 20; i++)
        {
            if (childCount < i) break;

            GameObject go = transform.GetChild(childCount - i).gameObject; // transform 하위에 오브젝트들을 인덱스로 찾아오는 기능
            
            if (go.name.Equals("Rubble")) continue;

            Rigidbody rigid = go.AddComponent<Rigidbody>();

            rigid.AddForce(
                (Vector3.up * Random.Range(0, 10f) + Vector3.right * (Random.Range(0, 10f) - 5f)) * 100f
                ); 
            //  rigidbody로 인한 기본적인 중력이 존재 그 중력으로 인해 떨어지는 오브젝트들이 맞물려 튕겨져나가는 힘이 작동
        }
    }

    public void Restart()
    {
        int childCount = transform.childCount;

        for(int i = 0; i < childCount; i++)
        {
            Destroy (transform.GetChild(i).gameObject);
        }

        isGameOver = false;

        lastBlock = null;
        desiredPosition = Vector3.zero;
        stackBounds = new Vector3(BoundSize, BoundSize);

        stackCount = -1;
        isMovingX = true;

        blocktransition = 0f;
        secondaryPositon = 0f;

        comboCount = 0;
        maxCombo = 0;

        prevBlockPosition = Vector3.down;

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        Spawn_Block();
        Spawn_Block();

    }
}
