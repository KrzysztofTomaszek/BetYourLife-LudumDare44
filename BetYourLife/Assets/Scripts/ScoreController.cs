using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{    
    static bool buttonTrigger1;
    static bool buttonTrigger2;
    static bool buttonTrigger3;
    static bool buttonTrigger4;
    public AudioClip buttonSound;
    public AudioSource audioSource;
    static Sprite skull;
    static Sprite crown;
    static Sprite button;
    static Sprite none;
    static Sprite curtine;

    private void Awake()
    {
        none = Resources.Load<Sprite>("none");
        curtine = Resources.Load<Sprite>("black");
        buttonTrigger1 = false;
        buttonTrigger2 = false;
        buttonTrigger3 = false;
        buttonTrigger4 = false;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        skull = Resources.Load<Sprite>("skull");
        crown = Resources.Load<Sprite>("crown");
        button = Resources.Load<Sprite>("done");

        DrawScoreBoard(WhoPlayerWin());//Może na Awake()
        LoadSprite("Courtine", none);
    }
    
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
                        case "GoToStartButton1":
                            if(buttonTrigger1)
                            {
                                audioSource.clip = buttonSound;
                                audioSource.Play();
                                LoadSprite("Courtine", curtine);
                                SceneManager.UnloadSceneAsync("ScoreScene");
                                SceneManager.LoadScene("StartScene");
                            }
                            break;
                        case "GoToStartButton2":
                            if(buttonTrigger2)
                            {
                                audioSource.clip = buttonSound;
                                audioSource.Play();
                                LoadSprite("Courtine", curtine);
                                SceneManager.UnloadSceneAsync("ScoreScene");
                                SceneManager.LoadScene("StartScene");
                            }
                            break;
                        case "GoToStartButton3":
                            if(buttonTrigger3)
                            {
                                audioSource.clip = buttonSound;
                                audioSource.Play();
                                LoadSprite("Courtine", curtine);
                                SceneManager.UnloadSceneAsync("ScoreScene");
                                SceneManager.LoadScene("StartScene");
                            }
                            break;
                        case "GoToStartButton4":
                            if(buttonTrigger4)
                            {
                                audioSource.clip = buttonSound;
                                audioSource.Play();
                                LoadSprite("Courtine", curtine);
                                SceneManager.UnloadSceneAsync("ScoreScene");
                                SceneManager.LoadScene("StartScene");
                            }
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

    void DrawScoreBoard(int winner)
    {
        DrawPlayers(Globals.playersNumOnStart);
        DrawRounds(Globals.playersRound, Globals.playersNumOnStart);
        DrawMoney(Globals.playersCash, Globals.playersNumOnStart);
        DrawIcons(Globals.playersAlive,winner, Globals.playersNumOnStart);
        DrawButton(Globals.playersNumOnStart);
    }

    int WhoPlayerWin()
    {
        if(Globals.onePlayerWin == true)
        {
            return FindLastLivePlayer(Globals.playersAlive);
        }
        else
        {
            return WhoHaveMostCash(Globals.playersAlive,Globals.playersCash);
        }
    }

    void DrawPlayers(int playersCount)
    {
        for(int i = 0;i < playersCount;i++)
        {
            GameObject.Find("Player" + (i + 1)).GetComponent<TextMeshProUGUI>().enabled=true;
        }
    }

    void DrawRounds(int[] playersRounds, int playersCount)
    {
        for(int i = 0;i < playersCount;i++)
        {
            GameObject.Find("Round" + (i + 1)).GetComponent<TextMeshPro>().SetText(playersRounds[i].ToString());
        }
    }

    void DrawMoney(int[] playersCash, int playersCount)
    {
        for(int i = 0;i < playersCount;i++)
        {
            GameObject.Find("Money" + (i + 1)).GetComponent<TextMeshPro>().SetText(playersCash[i].ToString()+"$");
        }
    }

    void DrawIcons(bool[] playersAlive, int winner, int playersCount)
    {
        for(int i = 0;i < playersCount;i++)
        {
            if(playersAlive[i] == false)
            {
                LoadSprite("Icon" + (i + 1), skull);
            }
            else if(i == winner - 1)
            {
                LoadSprite("Icon" + (i + 1), crown);
            }
                
        }
    }

    void DrawButton(int playersCount)
    {
        switch(playersCount)
        {
            case 1:
                LoadSprite("GoToStartButton2", button);
                buttonTrigger2 = true;
                break;            
            case 2:
                LoadSprite("GoToStartButton2", button);
                buttonTrigger2 = true;
                break;
            case 3:
                LoadSprite("GoToStartButton3", button);
                buttonTrigger3 = true;
                break;
            case 4:
                LoadSprite("GoToStartButton3", button);
                buttonTrigger3 = true;
                break;
            case 5:
                LoadSprite("GoToStartButton4", button);
                buttonTrigger4 = true;
                break;
            case 6:
                LoadSprite("GoToStartButton4", button);
                buttonTrigger4 = true;
                break;
            case 7:
                LoadSprite("GoToStartButton1", button);
                buttonTrigger1 = true;
                break;
            case 8:
                LoadSprite("GoToStartButton1", button);
                buttonTrigger1 = true;
                break;
        }
    }

    void LoadSprite(string placeName, Sprite sprite)
    {
        GameObject place;
        place = GameObject.Find(placeName);
        place.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    int FindLastLivePlayer(bool[] playersAlive)
    {
        for(int i = 0;i < Globals.playersAlive.Length;i++)
        {
            if(Globals.playersAlive[i])
                return i+1;
        }
        return -1;
    }

    int WhoHaveMostCash(bool[] playersAlive, int[] playersCash)
    {
        int maxCash=0;
        int richestPlayer = 0;
        for(int i = 0;i < Globals.playersAlive.Length;i++)
        {
            if(Globals.playersAlive[i])
            {
                if(playersCash[i] > maxCash)
                {
                    maxCash = playersCash[i];
                    richestPlayer = i;
                }
            }
        }
        return richestPlayer;
    }
}
