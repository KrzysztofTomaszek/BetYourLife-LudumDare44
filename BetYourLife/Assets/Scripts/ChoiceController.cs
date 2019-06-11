using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ChoiceController : MonoBehaviour
{
    public TextMeshProUGUI players;
    public TextMeshProUGUI player;
    public TextMeshProUGUI rounds;
    public TextMeshProUGUI cash;
    public TextMeshProUGUI multiple;
    public TextMeshProUGUI bet;
    public TextMeshProUGUI estWin;
    public AudioClip arrowShellSound;
    public AudioClip arrowBetSound;
    public AudioClip buttonSound;
    public AudioSource audioSource;
    static Sprite none;
    static Sprite curtine;
    int playerWhoPlay;
    int cashNum;
    int multipleNum;
    int betNum;
    int estWinNum;

    private void Awake()
    {
        none = Resources.Load<Sprite>("none");
        curtine = Resources.Load<Sprite>("black");
        players.SetText(Globals.playersNum.ToString());
        player.SetText(Globals.playerWhoPlay.ToString());
        rounds.SetText(Globals.roundsNumCount.ToString());
        playerWhoPlay = Globals.playerWhoPlay;
        cashNum = Globals.playersCash[playerWhoPlay-1];
        Globals.multiply = 2;
        cash.SetText(cashNum.ToString()+"$");
        bet.SetText(cashNum.ToString() + "$");
        multipleNum = 1;
        betNum = cashNum;
        estWinNum = 100;
        Screen.orientation = ScreenOrientation.Portrait;
    }
    
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Estimate(Globals.multiply);
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
                        case "RightArrowSchell":
                            if(multipleNum < 6)
                            {
                                audioSource.clip = arrowShellSound;
                                audioSource.Play();
                                multipleNum++;
                                DrawBullet(true);
                                Multiply();
                            }
                            break;
                        case "LeftArrowSchell":
                            if(multipleNum > 1)
                            {
                                audioSource.clip = arrowShellSound;
                                audioSource.Play();
                                DrawBullet(false);
                                multipleNum--;
                                Multiply();
                            }
                            break;
                        case "RightArrowBet":
                            if(betNum < cashNum)
                            {
                                audioSource.clip = arrowBetSound;
                                audioSource.Play();
                                betNum += 100;
                                bet.SetText(betNum.ToString() + "$");
                                Estimate(Globals.multiply);
                            }
                            break;
                        case "LeftArrowBet":
                            if(betNum > 100)
                            {
                                audioSource.clip = arrowBetSound;
                                audioSource.Play();
                                betNum -= 100;
                                bet.SetText(betNum.ToString() + "$");
                                Estimate(Globals.multiply);
                            }
                            break;
                        case "ButtonBet":
                            audioSource.clip = buttonSound;
                            audioSource.Play();
                            Globals.playersCash[playerWhoPlay - 1] -= betNum;
                            LoadSprite("Courtine", curtine);
                            SceneManager.UnloadSceneAsync("ChoiceScene");
                            SceneManager.LoadScene("ShootScene");
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

void DrawBullet(bool draw)
    {
        GameObject bullet;
        Sprite bulletSprite;

        if(draw) bulletSprite = Resources.Load<Sprite>("schell");
        else bulletSprite = Resources.Load<Sprite>("schellShadow");

        bullet = GameObject.Find("Schell1");

        switch(multipleNum)
        {
            case 2:
                bullet = GameObject.Find("Schell2");
                break;
            case 3:
                bullet = GameObject.Find("Schell3");
                break;
            case 4:
                bullet = GameObject.Find("Schell4");
                break;
            case 5:
                bullet = GameObject.Find("Schell5");
                break;
            case 6:
                bullet = GameObject.Find("Schell6");
                break;
        }
        bullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;
    }
    void Multiply()
    {
        int multiplity = multipleNum * 2;
        multiple.SetText(multiplity.ToString());
        Globals.multiply = multiplity;
        Estimate(multiplity);
    }

    void Estimate(int mult)
    {
        int multiplity = betNum * mult;
        estWin.SetText(multiplity.ToString());
        Globals.cashToWin = multiplity;
    }
    void LoadSprite(string placeName, Sprite sprite)
    {
        GameObject place;
        place = GameObject.Find(placeName);
        place.GetComponent<SpriteRenderer>().sprite = sprite;
    }

}
