using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections; // ���� ����

public class PZ_Sliding_Tile : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI _tileNumberText; // �׽�Ʈ�� ���� Ÿ�� ��ȣ ����� ���� text
    private int _tileNumber; // ���� Ÿ�� ��ȣ

    private PZ_Sliding_Board _sliding_Board;
    private Vector3 _correctPosition;
    public bool _isCorrect = false;

    // ���� ����
    public int TileNumber
    {
        get { return _tileNumber; }

        set
        {
            _tileNumber = value;
            _tileNumberText.text = _tileNumber.ToString();
        }
    }
    private void Awake()
    {
        _sliding_Board = GetComponentInParent<PZ_Sliding_Board>();
    }

    // tileEmptyIndex ������ �� Ÿ���� index
    public void TileSetup(int tileNumber, int tileEmptyIndex)
    {
        _tileNumberText = GetComponentInChildren<TextMeshProUGUI>();
        TileNumber = tileNumber;

        if (TileNumber == tileEmptyIndex)
        {
            GetComponent<Image>().enabled = false;
            _tileNumberText.enabled = false;
        }
    }

    // ���� ��ġ ����
    public void SetCorrectPosition()
    {
        _correctPosition = GetComponent<RectTransform>().localPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click" + _tileNumber);

        _sliding_Board.MoveTile(this);
    }

    public void TileMoveto(Vector3 endPosition)
    {
        StartCoroutine("TileMove", endPosition);
    }

    // Ÿ�� �ڿ������� �����̱�
    private IEnumerator TileMove(Vector3 endPosition)
    {
        float currentTime = 0f;
        float currentPecentage = 0f;
        float moveDuration = 0.1f;
        Vector3 startPosition = GetComponent<RectTransform>().localPosition;

        while (currentPecentage < 1)
        {
            currentTime += Time.deltaTime;
            currentPecentage = currentTime / moveDuration;
            GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPosition, endPosition, currentPecentage);

            yield return null;
        }

        _isCorrect = CheckCorrectPosition();

        _sliding_Board.IsPuzzleClear();
    }

    private bool CheckCorrectPosition()
    {
        if (GetComponent<RectTransform>().localPosition == _correctPosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}