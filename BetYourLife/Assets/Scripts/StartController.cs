using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class StartController : MonoBehaviour
{
    public TextMeshProUGUI players;
    public TextMeshProUGUI rounds;
    public int cashOnStart;
    public AudioClip arrowSound;
    public AudioClip buttonSound;
    public AudioSource audioSource;
    static Sprite none;
    static Sprite curtine;
    int roundsN;
    int playerN;

    // Ustawianie tablic życia i pieniedzy oraz ich wielkośći
    private void Awake()
    {
        none = Resources.Load<Sprite>("none");
        curtine = Resources.Load<Sprite>("black");
        Screen.orientation = ScreenOrientation.Portrait;
    }
    void Start()
    {
        roundsN = 3;
        playerN = 2;
        Screen.orientation = ScreenOrientation.Portrait;
        LoadSprite("Courtine", none);       
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            HandleTouch(touch.fingerId, Camera.main.ScreenToWorldPoint(touch.position), touch.phase);
        }

        // Simulate touch events from mouse events
        if(Input.touchCount == 0)
        {
            if(Input.GetMouseButtonDown(0))
            {
                HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Began);
            }
            if(Input.GetMouseButton(0))
            {
                HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Moved);
            }
            if(Input.GetMouseButtonUp(0))
            {
                HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Ended);
            }
        }
    }

private void HandleTouch(int touchFingerId, Vector3 touchPosition, TouchPhase touchPhase)
{
    switch(touchPhase)
    {
        case TouchPhase.Began:

                Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if(Physics.Raycast(raycast, out raycastHit))
                {
                    switch(raycastHit.collider.name)
                    {
                        case "RightArrowPlayers":
                            if(playerN < 8)
                            {
                                playerN++;
                                players.SetText(playerN.ToString());
                                audioSource.clip = arrowSound;
                                audioSource.Play();
                            }
                            break;
                        case "LeftArrowPlayers":
                            if(playerN > 2)
                            {
                                playerN--;
                                players.SetText(playerN.ToString());
                                audioSource.clip = arrowSound;
                                audioSource.Play();
                            }
                            break;
                        case "RightArrowRounds":
                            if(roundsN < 10)
                            {
                                roundsN++;
                                rounds.SetText(roundsN.ToString());
                                audioSource.clip = arrowSound;
                                audioSource.Play();
                            }
                            break;
                        case "LeftArrowRounds":
                            if(roundsN > 3)
                            {
                                roundsN--;
                                rounds.SetText(roundsN.ToString());
                                audioSource.clip = arrowSound;
                                audioSource.Play();
                            }
                            break;
                        case "ButtonPlay":
                            audioSource.clip = buttonSound;
                            audioSource.Play();
                            Globals.playersNum = playerN;
                            Globals.playersNumOnStart = playerN;
                            Globals.roundsNum = roundsN;
                            Globals.roundsNumCount = 1;
                            Globals.onePlayerWin = false;
                            SetPlayers(playerN);
                            LoadSprite("Courtine", curtine);
                            SceneManager.UnloadSceneAsync("StartScene");
                            SceneManager.LoadScene("ChoiceScene");
                            break;
                            case "Exit":
                            Application.Quit();
                            break;
                    }
                }
        
        break;
        case TouchPhase.Moved:
            // TODO
            break;
        case TouchPhase.Ended:
            // TODO
            break;
    }
}
void SetPlayers(int playerN)
    {
        Globals.playerWhoPlay = 1;
        for(int i = 0;i < playerN;i++)
        {
            Globals.playersCash[i] = cashOnStart;
            Globals.playersAlive[i] = true;
        }
        Globals.playersToEndRound = playerN;
    }
    void LoadSprite(string placeName, Sprite sprite)
    {
        GameObject place;
        place = GameObject.Find(placeName);
        place.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
