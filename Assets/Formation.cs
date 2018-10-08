using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation {

    public string formation;

    // Formations
    public const int FORMATION_442 = 0;

    // Formation TYPES

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static Player[] GetActorsAvailable(Team team, int zoneIndex)
    {
        Player[] output = new Player[0];
        switch (zoneIndex)
        {
            case 0:
                if (team.formation == FORMATION_442)
                {
                    output = new Player[4];
                    output[0] = team.players[1];
                    output[1] = team.players[2];
                    output[2] = team.players[3];
                    output[3] = team.players[4];
                }
                break;
            default:
                break;
        }

        return output;
    }

    public static Player[] GetReceiversAvailable(Team team, int zoneIndex)
    {
        Player[] output = new Player[0];
        switch (zoneIndex)
        {
            case 0:
                if (team.formation == FORMATION_442)
                {
                    output = new Player[4];
                    output[0] = team.players[1];
                    output[1] = team.players[2];
                    output[2] = team.players[3];
                    output[3] = team.players[4];
                }
                break;
            case 1:
                if (team.formation == FORMATION_442)
                {
                    output = new Player[4];
                    output[0] = team.players[1];
                    output[1] = team.players[4];
                    output[2] = team.players[5];
                    output[3] = team.players[6];
                }
                break;
            case 3:
                if (team.formation == FORMATION_442)
                {
                    output = new Player[6];
                    output[0] = team.players[10];
                    output[1] = team.players[9];
                    output[2] = team.players[8];
                    output[3] = team.players[7];
                    output[4] = team.players[1];
                    output[5] = team.players[4];
                }
                break;
            case 4:
                if (team.formation == FORMATION_442)
                {
                    output = new Player[2];
                    output[0] = team.players[10];
                    output[1] = team.players[9];
                }
                break;
            default:
                break;
        }

        return output;
    }
}
