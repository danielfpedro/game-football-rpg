using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayOutput
{
    public Player actor;
    public Player receiver;
    public string debugText;
    public bool jump = false;
    public int playValue;
    public bool hasNext;
    public bool success;
}
public class Team {
	public string name;
	public Player[] players = new Player[11];
    public int formation;
    public TeamMindset mindset;
}
public class Player
{
	public string name;
	public string pos;
    public int number;
	public PlayerStats stats = new PlayerStats();
}
public class PlayerStats {
	public int pass;
    public int longPass;
    public int shot;
    public int finishing;
}
public class GameController : MonoBehaviour {

    public Text textTeamA;
	public Font defaultFont;
	public GameObject viewport;
	public Text timerText;
	public float spedUpTime = 2;
	private float timer;

    public TeamMindset teamAMindset;

    private float steps = .2f;

	public Text actionsText;

	private int currentZone = 0;

	private Team[] teams = new Team[2];

	// Player playMaker;
	Player[] playersInPlay;

	private bool playEnded = false;

	Transform lastText;

	// Use this for initialization
	void Start () {
		lastText = viewport.transform;
		playEnded = false;

		Team teamA = new Team
		{
			name = "Botafogo",
            formation = Formation.FORMATION_442,
            mindset = teamAMindset
		};
        Team teamB = new Team
        {
            name = "Criciúma",
        };
        teams[0] = teamA;
        teams[1] = teamB;

        ArrayList namesTeamA = new ArrayList(11);
        namesTeamA.Add("Venâncio");
        namesTeamA.Add("Juca");
        namesTeamA.Add("Julio Castro");
        namesTeamA.Add("Antonio Pereira");
        namesTeamA.Add("Lambert Dinacos");
        namesTeamA.Add("Junqueirinha");
        namesTeamA.Add("Bonzado");
        namesTeamA.Add("Candico");
        namesTeamA.Add("Geraldo");
        namesTeamA.Add("Jarbas");
        namesTeamA.Add("Charles");

        Debug.Log("Name team a" + namesTeamA[0]);

        List<ArrayList> names = new List<ArrayList>(2);
        names.Add(namesTeamA);
        names.Add(namesTeamA);

        ArrayList positionsTeamA = new ArrayList(11);
        positionsTeamA.Add("gk");
        positionsTeamA.Add("lat");
        positionsTeamA.Add("zag");
        positionsTeamA.Add("zag");
        positionsTeamA.Add("lat");
        positionsTeamA.Add("vol");
        positionsTeamA.Add("vol");
        positionsTeamA.Add("meia");
        positionsTeamA.Add("meia");
        positionsTeamA.Add("atc");
        positionsTeamA.Add("atc");

        List<ArrayList> positions = new List<ArrayList>(1);
        positions.Add(positionsTeamA);

        for (int y = 0; y < 2; y++)
        {
            for (int i = 0; i < 11; i++)
            {
                Player player = new Player();
                player.number = i + 1;

                ArrayList namesLocal = names[y];
                player.name = (string)namesLocal[i];

                ArrayList positionsLocal = positions[0];
                player.pos = (string)positionsLocal[i];

                if (player.pos == "gk")
                {
                    player.stats.pass = Random.Range(2, 4);
                    player.stats.longPass = Random.Range(2, 4);
                    player.stats.shot = Random.Range(2, 3);
                    player.stats.finishing = Random.Range(1, 2);
                }
                else if (player.pos == "lat")
                {
                    player.stats.pass = Random.Range(6, 6);
                    player.stats.longPass = Random.Range(6, 6);
                    player.stats.shot = Random.Range(2, 3);
                    player.stats.finishing = Random.Range(1, 2);
                }
                else if (player.pos == "zag")
                {
                    player.stats.pass = Random.Range(2, 3);
                    player.stats.longPass = Random.Range(1, 2);
                    player.stats.shot = Random.Range(1, 5);
                    player.stats.finishing = Random.Range(2, 2);
                }
                else if (player.pos == "vol")
                {
                    player.stats.pass = Random.Range(6, 6);
                    player.stats.longPass = Random.Range(6, 6);
                    player.stats.shot = Random.Range(3, 6);
                    player.stats.finishing = Random.Range(2, 3);
                }
                else if (player.pos == "meia")
                {
                    player.stats.pass = Random.Range(6, 6);
                    player.stats.longPass = Random.Range(6, 6);
                    player.stats.shot = Random.Range(4, 6);
                    player.stats.finishing = Random.Range(3, 4);
                }
                else if (player.pos == "atc")
                {
                    player.stats.pass = Random.Range(3, 4);
                    player.stats.longPass = Random.Range(3, 4);
                    player.stats.shot = Random.Range(4, 6);
                    player.stats.finishing = Random.Range(4, 6);
                }

                teams[y].players[i] = player;
            }
        }

        string teamAContent = "";

        foreach (Player playerTeamA in teams[0].players)
        {
            teamAContent += "["+playerTeamA.number+"]" + playerTeamA.name + " (" + playerTeamA.pos + ")\n";
            teamAContent += "Pass: "+ playerTeamA.stats.pass + "\n";
            teamAContent += "Shot: " + playerTeamA.stats.shot + "\n";
            teamAContent += "Finishing: " + playerTeamA.stats.finishing + "\n\n";
        }
        textTeamA.text = teamAContent;
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime * spedUpTime;

		if (timer >= steps && currentZone < 50)
		{
			string actionText = "";

            PlayOutput play = PlayBook.ZagaToVol(teams[0]);
            if (play.hasNext)
            {
                actionText += "\n["+ play.actor.pos + "]" + play.actor.name + " PASSOU da ZAGA para a VOLANCIA";
                
                play = Play1(play.receiver);
                if (play.success)
                {
                    actionText += "\n[" + play.actor.pos + "]" + play.actor.name + " PASSOU da VOLANCIA para o MEIO";
                    
                    play = Play2(teams[0], play.receiver);
                    if (play.success)
                    {
                        actionText += "\n" + play.debugText;

                        if (!play.jump)
                        {
                            play = Play3(play.receiver);
                            if (play.success)
                            {
                                actionText += "\n[" + play.actor.pos + "]" + play.actor.name + " passou da ENTRADA DA AREA para DENTRO DA AREA";

                                play = Play4(play.receiver);
                                actionText += "\n[" + play.actor.pos + "]" + play.actor.name + " finalizou (" + play.playValue + ")";
                            }
                            else
                            {
                                actionText += "\n[" + play.actor.pos + "]" + play.actor.name + " errou passe da ENTRADA DA AREA para DENTRO DA AREA";
                            }
                        }
                        else {
                            play = Play4(play.receiver);
                            actionText += "\n[" + play.actor.pos + "]" + play.actor.name + " finalizou (" + play.playValue + ")";
                        }
                    }
                    else
                    {
                        actionText += "\n[" + play.actor.pos + "]" + play.actor.name + " errou o passe para a ENTRADA DA AREA";
                    }
                } else
                {
                    actionText += "\n[" + play.actor.pos + "]" + play.actor.name + " errou o passe para o MEIO";
                }
            } else
            {
                actionText += "\n[" + play.actor.pos + "]" + play.actor.name + " ERROU o passe na ZAGA";
            }
           
            GameObject newTextObject = new GameObject();
			newTextObject.AddComponent<Text>();
			newTextObject.GetComponent<Text>().text = actionText;
			newTextObject.GetComponent<Text>().font = defaultFont;
			newTextObject.GetComponent<Text>().fontSize = 15;
			newTextObject.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
			newTextObject.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
			newTextObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
			newTextObject.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);

			newTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20f);
			newTextObject.GetComponent<RectTransform>().position = new Vector2(lastText.position.x, lastText.position.y - 100);

			// newTextObject.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
			//newTextObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

			newTextObject.transform.SetParent(viewport.transform);
			lastText = newTextObject.transform;

			timer = 0;
			currentZone++;
		}
	}

	public int RollDice(int sides)
	{
		return Random.Range(1, sides);
	}

	/**PlayOutput Play0()
	{
		PlayOutput output  = new PlayOutput();
    
        Player actor = new Player();
        Player receiver = new Player();

        Player[] playersInPlay = new Player[4];
        Player[] playersToReceive = new Player[4];

        // Laterais e zagueiros
		playersInPlay[0] = teams[0].players[1];
		playersInPlay[1] = teams[0].players[2];
		playersInPlay[2] = teams[0].players[3];
		playersInPlay[3] = teams[0].players[4];

        // Laterais e volantes
        playersToReceive[0] = teams[0].players[1];
        playersToReceive[1] = teams[0].players[4];
        playersToReceive[2] = teams[0].players[5];
        playersToReceive[3] = teams[0].players[6];

        int higher = 0;
        for (int i = 0; i < playersInPlay.Length; i++)
        {
            int currentHit = RollDice(6);
            if (currentHit > higher)
            {
                higher = currentHit;
                actor = playersInPlay[i];
            }
        }

        if (DontHit(actor.stats.pass + 5))
        {
            //Get the receiver
            higher = 0;
            for (int i = 0; i < playersToReceive.Length; i++)
            {
                int currentHit = RollDice(6);
                // O Recebedor não pode receber dele mesmo
                if (currentHit > higher && playersToReceive[i].number != actor.number)
                {
                    higher = currentHit;
                    receiver = playersToReceive[i];
                }
            }

            Debug.Log("Player " + actor.name);

            output.actor = actor;
            output.receiver = receiver;
            output.success = true;
        } else
        {
            output.actor = actor;
            output.success = false;
        }
		return output;
	}**/
    // Volancia para o meio
	public PlayOutput Play1(Player actor)
	{
        PlayOutput output = new PlayOutput();
        output.actor = actor;

        // Player actor = new Player();
        Player receiver = new Player();

        // Player[] playersInPlay = new Player[4];
        Player[] playersToReceive = new Player[6];

        // Meias, laterais e volantes
        playersToReceive[0] = teams[0].players[1];
        playersToReceive[1] = teams[0].players[4];
        playersToReceive[2] = teams[0].players[5];
        playersToReceive[3] = teams[0].players[6];
        playersToReceive[4] = teams[0].players[7];
        playersToReceive[5] = teams[0].players[8];

        // Actor faz a jogada
        if (DontHit(actor.stats.pass))
        {
            // Get The receiver
            int higher = 0;
            foreach (Player player in playersToReceive)
            {
                int hit = RollDice(6);
                // Meias tem mais chance de receber
                if (player.pos == "meia")
                {
                    hit += 1;
                }
                if (hit > higher && player.number != actor.number)
                {
                    higher = hit;
                    receiver = player;
                };
            }

            output.receiver = receiver;
            output.success = true;

        } else
        {
            output.success = false;
        }

		return output;
	}

    // ZONA 02
    // Pode tocar pra frente ou fazer o passe longo e pular uma zona
    public PlayOutput Play2(Team team, Player actor)
    {
        PlayOutput output = new PlayOutput();
        output.actor = actor;

        // Player actor = new Player();
        Player receiver = new Player();

        // Player[] playersInPlay = new Player[4];
        Player[] playersToReceive = new Player[6];

        // Decide se ele vai lançar longa ou tocar pra frente
        int longPassBuff = (team.mindset == TeamMindset.BOLA_LONGA) ? 2 : 0;
        int longPassResult = RollDice(actor.stats.longPass + longPassBuff);

        int passBuff = (team.mindset == TeamMindset.TROCA_DE_PASSE) ? 2 : 0;
        int passResult= RollDice(actor.stats.pass + passBuff);

        // Faz o passe pra frente
        if (passResult >= longPassResult)
        {
            // Actor faz a jogada
            if (DontHit(actor.stats.pass))
            {
                Player[] availableReceivers = Formation.GetReceiversAvailable(team, 3);
                // Get The receiver
                int higher = 0;
                foreach (Player player in availableReceivers)
                {
                    int hit = RollDice(6);
                    // Meias tem duas vezes mais chance de receber
                    if (player.pos == "meia")
                    {
                        hit += 2;
                    }
                    else if (player.pos == "atc") // Atacantes tem +1 chances de receber
                    {
                        hit += 1;
                    }

                    if (hit > higher && player.number != actor.number)
                    {
                        higher = hit;
                        receiver = player;
                    };
                }

                output.debugText = actor.name + " [" + actor.pos + "] tocou a bola para " + receiver.name + " ["+receiver.pos+"] do MEIO para a ENTRADA DA ÁREA";
                output.receiver = receiver;
                output.success = true;
            }
            else
            {
                output.debugText = actor.name + " [" + actor.pos + "] errou o passe no MEIO para DENTRO DA ÁREA";
                output.success = false;
            }
        }
        // Pula uma e faz o passe longo
        else {
            // Actor faz a jogada
            if (DontHit(actor.stats.longPass))
            {
                Player[] availableReceivers = Formation.GetReceiversAvailable(team, 4);
                // Get The receiver
                int higher = 0;
                foreach (Player player in availableReceivers)
                {
                    int hit = RollDice(6);

                    if (hit > higher && player.number != actor.number)
                    {
                        higher = hit;
                        receiver = player;
                    };
                }

                output.debugText = actor.name + " [" + actor.pos + "] tocou fez o pase longo para " + receiver.name + " [" + receiver.pos + "] do MEIO para a DENTRO DA ÁREA";
                output.receiver = receiver;
                output.jump = true;
                output.success = true;
            }
            else
            {
                output.debugText = actor.name + " [" + actor.pos + "] errou o passe no MEIO para DENTRO DA ÁREA";
                output.success = false;
            }
        }

        return output;
    }

    // Entrada da area pra dentro da area ou chute
    public PlayOutput Play3(Player actor)
    {
        PlayOutput output = new PlayOutput();
        output.actor = actor;

        // Player actor = new Player();
        Player receiver = new Player();

        // Player[] playersInPlay = new Player[4];
        Player[] playersToReceive = new Player[8];

        // Laterais, Volantes, Meias e Atacantes
        playersToReceive[0] = teams[0].players[1];
        playersToReceive[1] = teams[0].players[4];
        playersToReceive[2] = teams[0].players[5];
        playersToReceive[3] = teams[0].players[6];
        playersToReceive[4] = teams[0].players[7];
        playersToReceive[5] = teams[0].players[8];
        playersToReceive[6] = teams[0].players[9];
        playersToReceive[7] = teams[0].players[10];

        // Actor faz a jogada
        if (DontHit(actor.stats.pass))
        {
            // Get The receiver
            int higher = 0;
            foreach (Player player in playersToReceive)
            {
                int hit = RollDice(6);
                // Obs.: Unico que não recebe buff é o volante
                if (player.pos == "lat" || player.pos == "meia")
                {
                    hit += 1;
                }
                else if (player.pos == "atc") // Atacantes tem +3 chances de receber
                {
                    hit += 3;
                }

                if (hit > higher && player.number != actor.number)
                {
                    higher = hit;
                    receiver = player;
                };
            }

            output.receiver = receiver;
            output.hasNext = true;
            output.success = true;
        }
        else
        {
            output.success = false;
        }

        return output;
    }
    public PlayOutput Play4(Player actor)
    {
        PlayOutput output = new PlayOutput();
        output.actor = actor;

        output.playValue = actor.stats.finishing;
        output.hasNext = false;
        output.success = true;

        return output;
    }
 
    bool DontHit(int value)
    {
        return !(RollDice(value) == 1);
    }

}
