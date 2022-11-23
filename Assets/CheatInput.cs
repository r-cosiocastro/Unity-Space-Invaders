using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheatInput : MonoBehaviour
{
    [TextArea(10, 10)]
    public string description = "";
    public KeyCode[] CheatCode;
    public UnityEvent CheatEvent;
    public float AllowedDelay = 1f;
    private Done_GameController gameController;

    private float _delayTimer;
    private int _index = 0;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Done_GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void Update()
    {
        _delayTimer += Time.deltaTime;
        if (_delayTimer > AllowedDelay)
        {
            ResetCheatInput();
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(CheatCode[_index]))
            {
                _index++;
                _delayTimer = 0f;
            }
            else
            {
                ResetCheatInput();
            }
        }

        if (_index == CheatCode.Length)
        {
            Cheat();
            ResetCheatInput();
            CheatEvent.Invoke();
            StartCoroutine(gameController.ShowCheatActivated());
        }
    }

    void ResetCheatInput()
    {
        _index = 0;
        _delayTimer = 0f;
    }

    public void Cheat()
    {
        Debug.Log("CHEAT ACTIVATED");
    }
}
