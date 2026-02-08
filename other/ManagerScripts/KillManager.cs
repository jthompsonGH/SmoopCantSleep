using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillManager
{
    public static int dorpsKilled = 0;
    public static int swarmersKilled = 0;
    public static int knightsKilled = 0;

    public static void ResetNumbers()
    {
        dorpsKilled = 0;
        swarmersKilled = 0;
        knightsKilled = 0;
    }
}
