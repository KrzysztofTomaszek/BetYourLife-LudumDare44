using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShootController : MonoBehaviour
{
    static int playerWhoPlay;
    static int multiply;
    static int cashToWin;
    static bool drumActive;
    static bool hammerActive;
    static bool triggerActive;
    static bool buttonActive;
    static Sprite arrow;
    static Sprite noneArrow;
    static Sprite DeadText;
    static Sprite WinText;
    static Sprite nextPlayerText;
    static Sprite doneButton;
    static Sprite none;
    static Sprite curtine;
    public AudioClip emptySound;
    public AudioClip shootSound;
    public AudioClip hammerSound;
    public AudioClip drumSound;
    public AudioClip buttonSound;
    public AudioSource musicSource;
    public Animator gunAnimator;
    static bool killed;




    private void Awake()
    {
        none = Resources.Load<Sprite>("none");
        curtine = Resources.Load<Sprite>("black");
        playerWhoPlay = Globals.playerWhoPlay;
        multiply = Globals.multiply;
        cashToWin = Globals.cashToWin;
        drumActive=true;
        hammerActive=false;
        triggerActive=false;
        buttonActive = false;
        Screen.orientation = ScreenOrientation.Landscape;
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;        
        arrow = Resources.Load<Sprite>("thatArrow");
        noneArrow = Resources.Load<Sprite>("none");
        DeadText = Resources.Load<Sprite>("Dead");
        WinText = Resources.Load<Sprite>("Win");
        nextPlayerText = Resources.Load<Sprite>("next");
        doneButton = Resources.Load<Sprite>("done");
        Globals.playersRound[playerWhoPlay - 1] = Globals.roundsNumCount;
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
                        case "DrumCollider":
                            //animacja kręcenia bębenka znikanie strzałki z bębenak i posjawienie nad kurkiem
                            if(drumActive)
                            {
                                drumActive = false;
                                gunAnimator.SetTrigger("DrumTrigger");
                                musicSource.clip = drumSound;
                                musicSource.Play();
                                LoadSprite("DrumArrow", noneArrow);
                                hammerActive = true;
                                LoadSprite("HammerArrow", arrow);
                            }
                            break;
                        case "HammerCollider":
                            //animacja zciągania kurak znikanie strzałki z kurka i posjawienie nad spustem
                            if(hammerActive)
                            {
                                hammerActive = false;
                                gunAnimator.SetTrigger("HammerTrigger");
                                musicSource.clip = hammerSound;
                                musicSource.Play();
                                LoadSprite("HammerArrow", noneArrow);
                                triggerActive = true;
                                LoadSprite("TriggerArrow", arrow);
                            }
                            break;
                        case "TriggerCollider":
                            //animacja zciągnięcia spustu znikanie strzałki z kurka,policzenie czy strzał się udał jeśli nie dodanie pieniędzy lub zabicie gracza i oczywiście dzwięk, pojawić odpowiedni napis oraz przycisk done
                            if(triggerActive)
                            {
                                gunAnimator.SetTrigger("TriggerTrigger");
                                triggerActive = false;
                                buttonActive = true;
                                if(killed = Shoot(multiply, Globals.roundsNum)) //dodaj rozpoznanie czy lucky shot żeby było jasne że to to i dzwięk do tego
                                {
                                    Dead();
                                }
                                else
                                {
                                    Win();
                                }
                                LoadSprite("TriggerArrow", noneArrow);
                                LoadSprite("ShootText", noneArrow);
                                LoadSprite("NextPlayerText", nextPlayerText);
                                LoadSprite("NextPlayerButton", doneButton);
                            }
                            break;
                        case "NextPlayerButton":
                            if(buttonActive)
                            {
                                musicSource.clip = buttonSound;
                                musicSource.Play();
                                EndScene();
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

int HowManyPlayersExist()
    {
        int playersCount = 0;
        foreach(bool exist in Globals.playersAlive)
        {
            if(exist)
                playersCount++;
        }
        return playersCount;
    }

    int EndScene()
    {
        int playersInGame = HowManyPlayersExist();
        

        if(OnlyOnePlayer())
        {
            Globals.onePlayerWin = true;
            LoadSprite("Courtine", curtine);
            SceneManager.UnloadSceneAsync("ShootScene");
            SceneManager.LoadScene("ScoreScene");
            return 0;
        }
        else
        {

            if(Globals.playersToEndRound <= 1)
            {
                Globals.roundsNum--;
                Globals.roundsNumCount++;
                if(Globals.roundsNum <= 0)
                {
                    LoadSprite("Courtine", curtine);
                    SceneManager.LoadScene("ScoreScene");
                    SceneManager.UnloadSceneAsync("ShootScene");
                    return 0;
                }
                Globals.playersToEndRound = playersInGame;
                Globals.playerWhoPlay = WhatsPlayerIsFirst();
            }
            else
            {
                Globals.playersToEndRound--;
                Globals.playerWhoPlay = WhatsPlayerNext(playerWhoPlay);
            }
            LoadSprite("Courtine", curtine);
            SceneManager.UnloadSceneAsync("ShootScene");
            SceneManager.LoadScene("ChoiceScene");
            return 0;
        }
    }

    void LoadSprite(string placeName, Sprite sprite)
    {
        GameObject place;
        place = GameObject.Find(placeName);
        place.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    bool Shoot(int multiply, int roundsNum)
    {
        bool[] chanceToLive = new bool[10];
        int LuckyRange = 0;
        switch(multiply)
        {            
            case 2:
                chanceToLive = new bool[10] { false, false, true, false, false, true, false, false, false, false };
                LuckyRange = 20;
                break;
            case 4:
                chanceToLive = new bool[10] { false, false, true, false, false, true, false, false, true, false };
                LuckyRange =21;
                break;
            case 6:
                chanceToLive = new bool[10] { false, true, true, true, false, true, false, false, true, false };
                LuckyRange =22;
                break;
            case 8:
                chanceToLive = new bool[10] { true, true, true, false, true, true, true, false, true, false };
                LuckyRange =23;
                break;
            case 10:
                chanceToLive = new bool[10] { true, true, true, false, true, true, true, true, true, false };
                LuckyRange =25;
                break;
            case 12:
                chanceToLive = new bool[10] { true, true, true, true, true, true, true, true, true, true };
                LuckyRange = 3;
                break;
        }
        int shoot =Random.Range(0, 9);

        if(Globals.roundsNumCount==1)
        {
            int LuckyOne = Random.Range(1, 2);
            if(multiply != 12)
            {
                if(LuckyOne == 2)
                    return false;
            }

        }else if(Globals.roundsNumCount == 2)
        {
            int LuckyTwo = Random.Range(1, 3);
            if(multiply != 12)
            {
                if(LuckyTwo == 2)
                    return false;
            }
        }

        int LuckyShoot = Random.Range(1, LuckyRange);
        if(multiply == 12 && shoot == 9)//to daje 97% przy 6/6
        {
            if(LuckyShoot == LuckyRange)
                return false;
        }
        else if(multiply != 12)
        {
            if(LuckyShoot == LuckyRange)
                return false;
        }
        return chanceToLive[shoot];
    }

    void Dead()
    {
        musicSource.clip = shootSound;
        musicSource.Play();
        Globals.playersAlive[playerWhoPlay-1] = false;
        LoadSprite("Dead",DeadText);
        Globals.playersNum--;
    }

    void Win()
    {
        musicSource.clip = emptySound;
        musicSource.Play();
        Globals.playersCash[playerWhoPlay-1] += cashToWin;
        LoadSprite("Win", WinText);
    }

    int WhatsPlayerNext(int playerBefore)
    {
        for(int i = playerBefore;i < Globals.playersAlive.Length;i++)
        {
            if(Globals.playersAlive[i])
                return i + 1;
        }
        return 0;
    }
    int WhatsPlayerIsFirst()
    {
        for(int i = 0;i < Globals.playersAlive.Length;i++)
        {
            if(Globals.playersAlive[i])
                return i+1;
        }
        return 0;
    }

    bool OnlyOnePlayer()
    {
        int count = 0;
        for(int i = 0;i < Globals.playersAlive.Length;i++)
        {
            if(Globals.playersAlive[i])
                count++;
        }
        if(count >= 2)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }
}
