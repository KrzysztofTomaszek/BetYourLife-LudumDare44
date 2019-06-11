using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals: MonoBehaviour
{
    public static int playersNum;
    public static int playersNumOnStart;
    public static int roundsNum;
    public static int roundsNumCount;
    public static int playersToEndRound;
    public static int playerWhoPlay;
    public static int multiply;
    public static int cashToWin;
    public static bool onePlayerWin;
    public static int[] playersCash = new int[8] {0, 0, 0, 0, 0, 0, 0, 0};
    public static int[] playersRound = new int[8] { 1, 1, 1, 1, 1, 1, 1, 1};
    public static bool[] playersAlive = new bool[8] {false, false, false, false, false, false, false, false};
}
