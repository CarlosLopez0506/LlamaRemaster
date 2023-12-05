using UnityEngine;

[System.Serializable]
public class TwelveButton
{
    private int _buttonNumber;
    private GameObject _buttonPrefab;
    private string _trapType;
    private Color _originalColor;

    public TwelveButton(int buttonNumber, GameObject buttonPrefab, string trapType, Color originalColor)
    {
        _buttonNumber = buttonNumber;
        _buttonPrefab = buttonPrefab;
        _trapType = trapType;
        _originalColor = originalColor;
    }

    public int ButtonNumber
    {
        get { return _buttonNumber; }
        set { _buttonNumber = value; }
    }

    public GameObject ButtonPrefab
    {
        get { return _buttonPrefab; }
        set { _buttonPrefab = value; }
    }

    public string TrapType
    {
        get { return _trapType; }
        set { _trapType = value; }
    }

    public Color OriginalColor
    {
        get { return _originalColor; }
        set { _originalColor = value; }
    }

    void Start()
    {
    }
}