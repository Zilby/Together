using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject heart;
	private Beat h;
	public Text stress;
	public Text compassion;
	public Text love;
	public GameObject gameover;
	public bool over;
	public List<Button> options; 
	public Button dialogue; 
	private bool narrating;
	private bool slightRedirect; // for if on a tangent but related path
	private int location;
	private int curr;
	private List<string[]> optionsTexts = new List<string[]>();
	private List<string[]> dialogueTexts = new List<string[]>();

	public void Start() {
		over = false;
		slightRedirect = false;
		location = 0;
		h = heart.GetComponent<Beat>();
        gameover.SetActive(false);
		InitializeText();
		TransitionToTalk ();
	}

	public void Update() {
		print (location);
        if(h.cracks == 4)
        {
            foreach (Button b in options)
            {
                b.GetComponent<Text>().text = "";
            }
            dialogue.GetComponent<Text>().text = "";
        }
		if(over) {

        } else if (heart == null) {
			stress.text = "";
			compassion.text = "";
			love.text = "";
			string[] messages = { "Again?", "Maybe Next Time", "Broken", "The End", 
				"It Was Never Real", "Get Over It", "Love Is A Myth" };
            gameover.SetActive(true);
			gameover.GetComponent<Button>().GetComponent<Text>().text = messages [Random.Range (0, 7)];
			over = true;
		} else {
			stress.text = "Stress: " + (100 - h.frequency);
			compassion.text = "Compassion: " + (100 - h.frozen);
			love.text = "Love: " + (h.love);
		}
	}

	public void Talk() {
		if(narrating && !over) {
			if (curr == dialogueTexts [location].Length) {
				narrating = false; 
				curr = 0;
				dialogue.GetComponent<Text> ().text = "";
				if (slightRedirect) {
                    print("here");
					location--;
					slightRedirect = false;
				}
				for (int i = 0; i < optionsTexts [location].Length; i++) {
					options [i].GetComponent<Text> ().text = optionsTexts [location] [i];
				}
			} else {
				dialogue.GetComponent<Text>().text = dialogueTexts [location] [curr]; 
				foreach (Button b in options) {
					b.GetComponent<Text>().text = "";
				}
				curr += 1; 
			}
		}
	}

	public void OptionOne() {
		if(!narrating && !over) {
			if (location == 0) {
				location = 1; 
			}
			else if (location == 1) {
				location = 5; 
			} else {
				return;
			}
			TransitionToTalk();
		}
	}

	public void OptionTwo() {
		if(!narrating && !over) {
			if (location == 0) {
				location = 2; 
			}
			else if (location == 1) {
				location = 6; 
				slightRedirect = true; 
			} else {
				return;
			}
			TransitionToTalk ();
		}
	}

	public void OptionThree() {
		if(!narrating && !over) {
			if (location == 0) {
				location = 3; 
				h.love += 10f;
			} else if (location == 1) {
				h.love += 10;
				location = 7; 
			} else {
				return;
			}
			TransitionToTalk ();
		}
	}

	public void OptionFour() {
		if(!narrating && !over) {
			if (location == 0) {
				location = 4; 
				h.frozen += 30f; 
			} else {
				return;
			}
			TransitionToTalk ();
		}
	}

	public void TransitionToTalk() {
		curr = 0;
		narrating = true;
		foreach (Button b in options) {
			b.GetComponent<Text> ().text = "";
		}
		dialogue.GetComponent<Text> ().text = dialogueTexts [location] [curr];
		curr++;
 	}

	public void Restart()
	{
		Application.LoadLevel("Menu");
	}

	public void InitializeText() {
		string[] template = { "Template" }; 
		for (int i = 0; i < 2000; i++) {
			dialogueTexts.Add (template); 
			optionsTexts.Add (template);
		}

		// Scene 1: 0-100

		string[] dialogue0 = { "Oh hey didnt see you there", "My name is Q" };
		dialogueTexts[0] = dialogue0;
		string[] dialogue1 = {"You go to Brooklyn Tech right?", 
			"Seems like we'll be sharing this commute by train together quite often",
			"Assuming you regularly wake up on time that is"
		};
		dialogueTexts[1] = dialogue1;
		string[] dialogue2 = {"Yeah my parents are pretty strange", 
			"Better than being stuck with my brother &'s name though"
		};
		dialogueTexts[2] = dialogue2;
		string[] dialogue3 = {"Well thank you", 
			"Kinda creepy that you'd say that to a complete stranger", 
			"But it does wonders to my self esteem"
		};
		dialogueTexts[3] = dialogue3;
		string[] dialogue4 = {"Ooooookaaay.. I'll catch you around I guess", "* Several weeks pass *",
			"* Each day you ride the train with Q *", 
			"* Each day you shoot down their attempts to socialize *", 
			"* You attend classes but remain distant with your peers *",
            "* Still, for some reason Q seems to have taken an interest in you *"
		};
		dialogueTexts[4] = dialogue4;
		string[] dialogue5 = {"Studious then huh?", "Well I guess that makes sense", 
			"You are going to Brooklyn Tech", "Honestly I'd wake up later but my mom would have my ass"
		};
		dialogueTexts[5] = dialogue5;
		string[] dialogue6 = {"Agreeeeed", 
			"If I could sleep until 11 each morning..", "Man that'd be the life", 
			"My mom would have my ass though"
		};
		dialogueTexts[6] = dialogue6;// goes to 5
		string[] dialogue7 = {"Awww that's sweet of you", 
			"Glad you're the first person I met going to this school", 
			"I hear most of the people are creeps"};
		dialogueTexts[7] = dialogue7;
		string[] options0 = {"Hey nice to meet you", "What an odd name", 
			"Wow you're stunning", "Fuck off"
		};
		optionsTexts[0] = options0;
		string[] options1 = {"I tend to be on time", "Every day I sleep in is a success", 
			"I'll wake up to make sure you have some company"
		};
		optionsTexts[1] = options1;
		string[] options2 = { "Wow.. yeah that's pretty odd", "I WISH my name was &, what a badass moniker" };
		optionsTexts[2] = options2;
		string[] options3 = {"I don't suppose a fine specimen like you happens to be on her way to my high school as well?", 
			"I say it like it is sweet stuff", "Yeah sorry about that you kinda caught me off guard"
		};
		optionsTexts[3] = options3;
		string[] options5 = { "Problems at home?", "Yeah moms can be annoying like that" };
		optionsTexts[5] = options5;
		string[] options7 = { "Who says I'm not?", "So what kind of things are you into?", "God I'm tired right now..." };
		optionsTexts[7] = options7;
	}
}

