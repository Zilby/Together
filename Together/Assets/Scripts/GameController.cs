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
	public Text gameover;
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
		InitializeText();
		TransitionToTalk ();
	}

	public void Update() {
		print (location);
		if(over) {

		} else if (heart == null) {
			stress.text = "";
			compassion.text = "";
			love.text = "";
			string[] messages = { "Again?", "Maybe Next Time", "Broken", "The End", 
				"It Was Never Real", "Get Over It" }; 
			gameover.text = messages [Random.Range (0, 6)];
			over = true;
		} else {
			stress.text = "Stress: " + (100 - h.frequency);
			compassion.text = "Compassion: " + (100 - h.frozen);
			love.text = "Love: " + (h.love);
		}
	}

	public void Talk() {
		if(narrating) {
			if (curr == dialogueTexts [location].Length) {
				narrating = false; 
				curr = 0;
				dialogue.GetComponent<Text> ().text = "";
				if (slightRedirect) {
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
		if(!narrating) {
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
		if(!narrating) {
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
		if(!narrating) {
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
		if(!narrating) {
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

	public void InitializeText() {
		string[] dialogue0 = { "Oh hey didnt see you there", "My name is A" };
		dialogueTexts.Add (dialogue0);
		string[] dialogue1 = {"You go to Brooklyn Tech right?", 
			"Seems like we will be sharing this commute by train together quite often",
			"Assuming you regularly wake up on time that is"
		};
		dialogueTexts.Add (dialogue1);
		string[] dialogue2 = {"Yeah my parents are pretty strange", 
			"Better than being stuck with my brother &s name though"
		};
		dialogueTexts.Add (dialogue2);
		string[] dialogue3 = {"Well thank you", 
			"Kinda creepy that youd say that to a complete stranger", 
			"But it does wonders to my self esteem"
		};
		dialogueTexts.Add (dialogue3);
		string[] dialogue4 = {"Ooooookaaay.. Ill catch you around I guess", "* Several weeks pass *",
			"* You attend classes but remain distant with your peers *", "* Still, for some reason A seems" +
			"to have taken an interest in you *"
		};
		dialogueTexts.Add (dialogue4);
		string[] dialogue5 = {"Studious then huh?", "Well I guess it makes sense", 
			"You are going to Brooklyn Tech", "Honestly I'd wake up later but my mom would have my ass"
		};
		dialogueTexts.Add (dialogue5); // goes to 4
		string[] dialogue6 = {"Agreeeeed", 
			"If I could sleep until 11 each morning..", "Man that would be the life", 
			"My mom would have my ass though"
		};
		dialogueTexts.Add (dialogue6);
		string[] dialogue7 = {"Awww thats sweet of you", 
			"Glad youre the first person I met going to this school", 
			"I hear most of the people are creeps"};
		dialogueTexts.Add (dialogue7);
		string[] options0 = {"Hey nice to meet you", "What an odd name", 
			"Wow youre stunning", "Fuck off"
		};
		optionsTexts.Add (options0);
		string[] options1 = {"I tend to be on time", "Every day I sleep in is a success", 
			"Ill wake up to make sure you have some company"
		};
		optionsTexts.Add (options1);
		string[] options2 = { "Wow.. yeah thats pretty odd", "I WISH my name was &, what a badass moniker" };
		optionsTexts.Add (options2);
		string[] options3 = {"I dont suppose a fine specimen like you happens to be on her way to my high school as well?", 
			"I say it like it is sweet stuff", "Yeah sorry about that you kinda caught me off guard"
		};
		optionsTexts.Add (options3);
		string[] options4 = { "Template" };
		optionsTexts.Add (options4);
		string[] options5 = { "Problems at home?", "Yeah moms can be annoying like that" };
		optionsTexts.Add (options5);
		string[] options6 = { "Template" };
		optionsTexts.Add (options6);
		string[] options7 = { "Template" };
		optionsTexts.Add (options7);
		string[] options8 = { "Template" };
		optionsTexts.Add (options8);
		string[] options9 = { "Template" };
		optionsTexts.Add (options9);
		string[] options10 = { "Template" };
		optionsTexts.Add (options10);
		string[] options11 = { "Template" };
		optionsTexts.Add (options11);
	}
}

