using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBook : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static PlayOutput ZagaToVol(Team team)
    {
        PlayOutput output = new PlayOutput();

        Player actor = new Player();
        Player receiver = new Player();

        Player[] actorsAvailable = Formation.GetActorsAvailable(team, 0);
        Player[] receiversAvailable = Formation.GetReceiversAvailable(team, 1);

        int higher = 0;
        for (int i = 0; i < actorsAvailable.Length; i++)
        {
            int currentHit = RollDice(6);
            if (currentHit > higher)
            {
                higher = currentHit;
                actor = actorsAvailable[i];
            }
        }

        if (DontHit(actor.stats.pass + 10))
        {
            //Get the receiver
            higher = 0;
            for (int i = 0; i < receiversAvailable.Length; i++)
            {
                int currentHit = RollDice(6);
                // O Recebedor não pode receber dele mesmo
                if (currentHit > higher && receiversAvailable[i].number != actor.number)
                {
                    higher = currentHit;
                    receiver = receiversAvailable[i];
                }
            }

            output.actor = actor;
            output.receiver = receiver;
            output.success = true;
            output.hasNext = true;
        }
        else
        {
            output.actor = actor;
            output.success = false;
            output.hasNext = false;
        }
        return output;
    }

    public static int RollDice(int sides)
    {
        return Random.Range(1, sides);
    }

    public static bool DontHit(int value)
    {
        return !(RollDice(value) == 1);
    }
}
