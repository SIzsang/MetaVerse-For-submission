using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TheStack : MonoBehaviour
{
    private const float BoundSize = 3.5f; // �� ������
    private const float MovingBoundsSize = 3f; // �̵���
    private const float StackMovingSpeed = 5.0f; // �̵� �ӵ�
    private const float BlockMovingSpeed = 3.5f;
    private const float ErrorMargin = 0.1f; // �������� ����� ���� ����

    public GameObject originBlock = null;

    private Vector3 prevBlockPosition;
    private Vector3 desiredPosition;
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize); // ���Ӱ� �����Ǵ� ���� ������

    Transform lastBlock = null; // ���ο� ���� �����ϱ� ���� ������
    float blocktransition = 0f;
    float secondaryPositon = 0f;

    int stackCount = -1; // �����ϸ鼭 +1 �ϸ鼭 ���
    
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

        prevColor = GetRandomColor(); // ���ʿ� �ѹ� ������ ����
        nextColor = GetRandomColor();

        prevBlockPosition = Vector3.down; // down = y���� -1�� ������ ����ִ�.

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
        if (Input.GetMouseButtonDown(0)) // ���� ���콺 Ŭ��, ����Ʈ�� ��ġ
        {
            if (PlaceBlock())
            {
                Spawn_Block();
            }
            else
            {
                // ���� ����
                Debug.Log("Game Over");
                Updatescore();
                isGameOver = true;
                GameOverEffect();
                TheStackUIManager.Instance.SetScoreUI();
            }

        }
        MoveBlock();
        transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);
        // ���� ��ġ, �������� ��ġ, ������ �ۼ������� �� �����Ӹ��� ���������� desiredPosition���� �̵� �� �� �ְ� ������ش�.
    }

    bool Spawn_Block()
    {
        if (lastBlock != null)
            prevBlockPosition = lastBlock.localPosition;

        GameObject newBlock = null;
        Transform newTrans = null;

        newBlock = Instantiate(originBlock); // Instantiate oringinBlock�� ������ �Ѵ�. ������ ������Ʈ�� ���� ��ȯ�Ѵ�.

        if (newBlock == null)
        {
            Debug.Log("NewBlklock Instantiate Failed");
            return false;
        }

        ColorChange(newBlock);

        newTrans = newBlock.transform;
        newTrans.parent = this.transform; // ���Ӱ� ������ ������Ʈ�� �θ� ���� Ʈ���������� �������ش�.
        newTrans.localPosition = prevBlockPosition + Vector3.up;
        newTrans.localRotation = Quaternion.identity; // Quaternion�� �ʱⰪ ȸ���� ���� ���� 
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        stackCount++;

        desiredPosition = Vector3.down * stackCount; // ���� ī��Ʈ�� �����ϴ� ��ŭ The stack�̶�� �ָ� �ٴ����� ��ĭ�� ������.
        blocktransition = 0f;

        lastBlock = newTrans;

        isMovingX = !isMovingX;

        TheStackUIManager.Instance.UpdateScore();

        return true;


    }

    Color GetRandomColor()
    {
        float r = Random.Range(100f, 250f) / 255f; // 100���� �����ϴ� ������ 100���� ������ �ʹ� ��ο� ��ġ���� ���� ����
        float g = Random.Range(100f, 250f) / 255f;
        float b = Random.Range(100f, 250f) / 255f;

        return new Color(r, g, b);
    }

    void ColorChange(GameObject go)
    {
        
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % step + 1) / step);

        Renderer rn = go.GetComponent<Renderer>(); // renderer ���𰡸� �׷����� �۾�

        if (rn == null)
        {
            Debug.Log("Renderer is NULL");
            return;
        }
        rn.material.color = applyColor;
        Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f);

        if (applyColor.Equals(nextColor) == true) // ������ �÷��� ������ �÷��� �������ٸ�
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }

        // Color�� ������ ���¿��� ���� ������ ���� nextColor�� ���� ��������.
        // �������� �Ǹ� prevColor�� �����ϰ� nextColor�� ���Ӱ� �����Ѵ�.
    }

    void MoveBlock()
    {
        blocktransition += Time.deltaTime * BlockMovingSpeed;

        float movePosition = Mathf.PingPong(blocktransition, BoundSize) - BoundSize / 2; // PingPong �̶� ? 0���� �츮�� �������� ��������� ��ȯ�ϴ� ��

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
            float deltaX = prevBlockPosition.x - lastPosition.x; // deltax = �߷��������ϴ� �� ��Ż�� ����
                                                                 // �������� ���� ���� ���� ������
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
            Debug.Log("�ְ� ���� ����");
            bestScore = stackCount;
            bestCombo = comboCount;

            PlayerPrefs.SetInt(BestComboKey, bestCombo);
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
        }
    }

    void GameOverEffect()
    {
        int childCount = this.transform.childCount; // transform���� ������ �ִ� ������Ʈ�� ����
                                                    // �̰����� TheStack �ȿ� �ִ� ����, �����
        for(int i = 1; i < 20; i++)
        {
            if (childCount < i) break;

            GameObject go = transform.GetChild(childCount - i).gameObject; // transform ������ ������Ʈ���� �ε����� ã�ƿ��� ���
            
            if (go.name.Equals("Rubble")) continue;

            Rigidbody rigid = go.AddComponent<Rigidbody>();

            rigid.AddForce(
                (Vector3.up * Random.Range(0, 10f) + Vector3.right * (Random.Range(0, 10f) - 5f)) * 100f
                ); 
            //  rigidbody�� ���� �⺻���� �߷��� ���� �� �߷����� ���� �������� ������Ʈ���� �¹��� ƨ���������� ���� �۵�
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
