using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team {
	public string name;
	public Player[] players = new Player[10];
}
public class Player
{
	public string name;
	public string pos;
	public PlayerStats stats = new PlayerStats();
}
public class PlayerStats {
	public int pass;
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

	private float steps = 2;

	public Text actionsText;

	private int currentZone = 0;

	private Team[] teams = new Team[2];

	Player playMaker;
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
		};

        ArrayList namesTeamA = new ArrayList(10);
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

        ArrayList positionsTeamA = new ArrayList(10);
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

        for (int i = 0; i < 10; i++)
        {
            Player player = new Player();
            player.name = (string)namesTeamA[i];
            player.pos = (string)positionsTeamA[i];
            Debug.Log("player " + player.stats);
            if (player.pos == "lat")
            {
                player.stats.pass = Random.Range(2, 4);
                player.stats.shot = Random.Range(2, 3);
                player.stats.finishing = Random.Range(1, 2);
            } else if (player.pos == "zag")
            {
                player.stats.pass = Random.Range(2, 3);
                player.stats.shot = Random.Range(1, 5);
                player.stats.finishing = Random.Range(2, 2);
            }
            else if (player.pos == "vol")
            {
                player.stats.pass = Random.Range(3, 5);
                player.stats.shot = Random.Range(3, 6);
                player.stats.finishing = Random.Range(2, 3);
            }
            else if (player.pos == "meia")
            {
                player.stats.pass = Random.Range(4, 5);
                player.stats.shot = Random.Range(4, 6);
                player.stats.finishing = Random.Range(3, 4);
            } else if (player.pos == "atc")
            {
                player.stats.pass = Random.Range(3, 4);
                player.stats.shot = Random.Range(4, 6);
                player.stats.finishing = Random.Range(4, 6);
            }

            teamA.players[i] = player;
        }

        Debug.Log(teamA);
        teams[0] = teamA;

        string teamAContent = "";

        foreach (Player playerTeamA in teams[0].players)
        {
            teamAContent += playerTeamA.name + " (" + playerTeamA.pos + ")\n";
            teamAContent += "Pass: "+ playerTeamA.stats.pass + "\n";
            teamAContent += "Shot: " + playerTeamA.stats.shot + "\n";
            teamAContent += "Finishing: " + playerTeamA.stats.finishing + "\n\n";
        }
        textTeamA.text = teamAContent;
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime * spedUpTime;

		if (timer >= steps && currentZone < 10)
		{
			string actionText = "";
			ArrayList play;

			play = Play0();
			actionText += "\n" + play[0];

			if ((bool)play[1])
			{
				play = Play1();
				actionText += "\n" + play[0];
			}
            if ((bool)play[1])
            {
                play = Play2();
                actionText += "\n" + play[0];
            }
            if ((bool)play[1])
            {
                play = Play3();
                actionText += "\n" + play[0];
            }
            if ((bool)play[1])
            {
                play = Play4();
                actionText += "\n" + play[0];
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

	ArrayList Play0()
	{
		ArrayList output  = new ArrayList(2);

		playersInPlay = new Player[4];
		playersInPlay[0] = teams[0].players[0];
		playersInPlay[1] = teams[0].players[1];
		playersInPlay[2] = teams[0].players[2];
		playersInPlay[3] = teams[0].players[3];

		playMaker = playersInPlay[Random.Range(1, playersInPlay.Length)];

		int result = RollDice(playMaker.stats.pass + 10);

		if (result == 1)
		{
			output.Add("Jogador " + playMaker.name + "(" + playMaker.pos + ") errou o passe.");
			output.Add(false);
		}
		else
		{
			output.Add("Jogador " + playMaker.name + "(" + playMaker.pos + ") passou o fino!!!.");
			output.Add(true);
		}

		return output;
	}
	public ArrayList Play1()
	{
		ArrayList output = new ArrayList(2);

		playersInPlay = new Player[4];
		// Laterais sobem
		playersInPlay[0] = teams[0].players[0]; // Lateral que subiu
		playersInPlay[1] = teams[0].players[3]; // Lateral que subiu
		playersInPlay[2] = teams[0].players[4]; // Volantaço
		playersInPlay[3] = teams[0].players[5]; // Volantaço

		int higher = 0;

		foreach (Player player in playersInPlay)
		{
			int pass = RollDice(player.stats.pass);
			// Volante tem mais chances de passar do que laterais
			if (player.pos == "vol")
			{
				pass += 1;
			}
			if (pass > higher)
			{
				higher = pass;
				playMaker = player;
			};
		}

		int result1 = RollDice(playMaker.stats.pass + 5);
        Debug.Log("Result do roll Dice: " + result1);
		if (result1 == 1)
		{
			output.Add(playMaker.name + "(" + playMaker.pos + ") errou o passe na volancia.");
			output.Add(false);
		}
		else
		{
			output.Add(playMaker.name + "(" + playMaker.pos + ") passou fino na volacia!!!.");
			output.Add(true);
		}

		return output;
	}

    public ArrayList Play2()
    {
        ArrayList output = new ArrayList(2);

        playersInPlay = new Player[6];
        // Laterais sobem

        playersInPlay[0] = teams[0].players[0]; // Lateral que subiu
        playersInPlay[1] = teams[0].players[3]; // Lateral que subiu
        playersInPlay[2] = teams[0].players[4]; // Volantaço
        playersInPlay[3] = teams[0].players[5]; // Volantaço
        playersInPlay[4] = teams[0].players[6]; // Meia Afensivo
        playersInPlay[5] = teams[0].players[7]; // Meia Afensivo

        int higher = 0;

        foreach (Player player in playersInPlay)
        {
            int pass = RollDice(player.stats.pass);
            // Meia tem mais chances de participar que os laterais e os volantes
            if (player.pos == "meia")
            {
                pass += 1;
            }
            if (pass > higher)
            {
                higher = pass;
                playMaker = player;
            };
        }

        int result1 = RollDice(playMaker.stats.pass);
        if (result1 == 1)
        {
            output.Add(playMaker.name + "(" + playMaker.pos + ") errou o passe na meiuca.");
            output.Add(false);
        }
        else
        {
            output.Add(playMaker.name + "(" + playMaker.pos + ") passou fino meiuca!.");
            output.Add(true);
        }

        return output;
    }

    public ArrayList Play3()
    {
        Player shotPlayer = new Player();
        Player passPlayer = new Player();

        ArrayList output = new ArrayList(2);

        playersInPlay = new Player[4];
        // Volantes subiram
        playersInPlay[0] = teams[0].players[4]; // Volantaço
        playersInPlay[1] = teams[0].players[5]; // Volantaço
        playersInPlay[2] = teams[0].players[6]; // Meia Ofensivo
        playersInPlay[3] = teams[0].players[7]; // Meia Ofensivo

        int higherShot = 0;
        foreach (Player player in playersInPlay)
        {
            int shot = RollDice(player.stats.shot);
            if (shot > higherShot)
            {
                higherShot = shot;
                shotPlayer = player;
            };
        }

        int higherPass = 0;
        foreach (Player player in playersInPlay)
        {
            int pass = RollDice(player.stats.pass);
            if (pass > higherPass)
            {
                higherPass = pass;
                passPlayer = player;
            };
        }

        if (higherShot > higherPass)
        {
            playMaker = shotPlayer;
            // Do Shot
            int result1 = RollDice(playMaker.stats.shot);
            output.Add(playMaker.name + "(" + playMaker.pos + ") deu um chute " + playMaker.stats.shot);
            output.Add(true);
        } else
        {
            playMaker = passPlayer;
            int result1 = RollDice(playMaker.stats.pass);
            if (result1 == 1)
            {
                output.Add(playMaker.name + "(" + playMaker.pos + ") errou o passe no meio para dentro da area.");
                output.Add(false);
            }
            else
            {
                output.Add(playMaker.name + "(" + playMaker.pos + ") passou fino para dentro da AREA!.");
                output.Add(true);
            }
        }



        return output;
    }

    public ArrayList Play4()
    {
        ArrayList output = new ArrayList(2);

        playersInPlay = new Player[2];
        // Laterais sobem

        playersInPlay[0] = teams[0].players[8];
        playersInPlay[1] = teams[0].players[9];

        int higher = 0;

        foreach (Player player in playersInPlay)
        {
            int finishing = RollDice(player.stats.finishing);
            if (finishing > higher)
            {
                higher = finishing;
                playMaker = player;
            };
        }

        int resultDice = RollDice(playMaker.stats.finishing);
        output.Add(playMaker.name + "(" + playMaker.pos + ") Fnializou " + resultDice);
        output.Add(true);

        return output;
    }

}
