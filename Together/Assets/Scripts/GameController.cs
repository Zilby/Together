using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject heart;
	private Beat h;
	public Text stress;
	public Text compassion;
	public Text love;
	public GameObject gameover;
	public bool over;
	public bool selected;
	//public List<Button> options; 
	public Button dialogue; 
	//public AudioSource song1;
	private bool narrating;
	//private bool slightRedirect; // for if on a tangent but related path
	//private int response; // for responding to a specific choice but going to main path
						  // set the response to the location you want it to go to after
	//private int response2; // for responding to a specific choice but going to main path
	// set the response to the location you want it to go to after
	private int location;
	//private int curr;
	private float letterPause; // amount of time before another letter is typed
	private bool typing; // whether the current dialogue is being typed
	private bool skip; // true if you want to skip the text delay
	//private bool option1; // for exhausting an option from a selection
	//private bool option2; 
	//private bool option3; 
	//private List<string[]> optionsTexts = new List<string[]>();
	//private List<string[]> dialogueTexts = new List<string[]>();
    private List<string> dialogueTexts = new List<string>();

    private bool cracking;
    public static float speed; // speed/frequency of the falling text 

	public List<GameObject> texts; 
	private float counter;
	private int index;
    public static bool ending;

	public void Start() {
		over = false;
		//slightRedirect = false;
		//response = -1;
		//response2 = -1; 
		location = 0;
		letterPause = 0.005f;
		typing = false;
		skip = false;
		//option1 = true;
		//option2 = true;
		//option3 = true;
		h = heart.GetComponent<Beat>();
        gameover.SetActive(false);
        ending = false;
        //InitializeText();
        //TransitionToTalk ();

        speed = 1;
        cracking = false;

		index = 0; 
		counter = 150;
        string template = "";  // "template"
        for (int i = 0; i < 2000; i++)
        {
            dialogueTexts.Add(template);
        }
        string dialogue0 = "They love you";
        dialogueTexts[0] = dialogue0;
        string dialogue1 = "But over time feelings change";
        dialogueTexts[1] = dialogue1;
        string dialogue2 = "Love is fleeting and the past, only memories";
        dialogueTexts[2] = dialogue2;
        string dialogue3 = "Even though you still feel the same love towards them";
        dialogueTexts[3] = dialogue3;
        string dialogue4 = "They will never feel the same way about you"; 
        dialogueTexts[4] = dialogue4;
        string dialogue5 = "They break up with you";  // crack love - 25
        dialogueTexts[5] = dialogue5;
        string dialogue6 = ". . .";
        dialogueTexts[6] = dialogue6;
        string dialogue7 = "You feel lost";
        dialogueTexts[7] = dialogue7;
        string dialogue8 = "They were your whole world";
        dialogueTexts[8] = dialogue8;
        string dialogue9 = "What made you get up in the morning"; // crack love - 25
        dialogueTexts[9] = dialogue9;
        string dialogue10 = "What brought you joy during hard times"; // crack love - 25
        dialogueTexts[10] = dialogue10;
        string dialogue11 = "What gave your life purpose"; 
        dialogueTexts[11] = dialogue11;
        string dialogue12 = ". . .";
        dialogueTexts[12] = dialogue12;
        string dialogue13 = "You can't live like this";
        dialogueTexts[13] = dialogue13;
        string dialogue14 = "Without something to live for";
        dialogueTexts[14] = dialogue14;
        string dialogue15 = "You consider ending it all"; 
        dialogueTexts[15] = dialogue15;
        string dialogue16 = ". . ."; // final crack
        dialogueTexts[16] = dialogue16;
        string dialogue17 = "But you don't";
        dialogueTexts[17] = dialogue17;
        string dialogue18 = "Because even if you'll never get back together";
        dialogueTexts[18] = dialogue18;
        string dialogue19 = "Maybe someday you'll be able to feel the same love again";
        dialogueTexts[19] = dialogue19;
    }

	public void FixedUpdate() {
		if(over) {

        } else if (heart == null) {
            foreach(GameObject obj in texts)
            {
                if(obj)
                {
                    obj.SetActive(false);
                }
            }
			stress.text = "";
			compassion.text = "";
			love.text = "";
			string[] messages = { "Again?", "Maybe Next Time", "Broken", "The End", 
				"It Was Never Real", "Get Over It", "Love Is A Myth" };
            gameover.SetActive(true);
			gameover.GetComponent<Button>().GetComponent<Text>().text = messages [Random.Range (0, 7)];
			over = true;
		} else {
            if(ending)
            {
                if(location == 20)
                {
                    StartCoroutine(End());
                }
            }
            else
            {
                if (counter <= 0 && index < texts.Count)
                {
                    texts[index].SetActive(true);
                    counter = 200 / speed;
                    index++;
                    if (index % 5 == 0)
                    {
                        speed += 0.05f;
                    }
                }
                else if (index == texts.Count && !texts[texts.Count - 1])
                {
                    ending = true;
                    h.end = true;
                    dialogue.gameObject.SetActive(true);
                    Talk();
                }
                else
                {
                    counter--;
                }
            }
            stress.text = "Stress: " + (100 - h.frequency);
			compassion.text = "Compassion: " + (100 - h.frozen);
			love.text = "Love: " + (h.love);
		}
        
	}

	public void Restart()
	{
		SceneManager.LoadScene("Menu");
	}

    IEnumerator TypeText(string message)
    {
        if (!cracking)
        {
            typing = true;
            string current = "";
            dialogue.GetComponent<Text>().text = "";
            char[] m = message.ToCharArray();
            for (int i = 0; i < message.Length; i++)
            {
                if (skip)
                {
                    dialogue.GetComponent<Text>().text = message;
                    skip = false;
                    break;
                }
                current += m[i];
                dialogue.GetComponent<Text>().text = current;
                dialogue.GetComponent<Text>().text += "<color=#00000000>";
                for (int j = i + 1; j < message.Length; j++)
                {
                    dialogue.GetComponent<Text>().text += m[j];
                }
                dialogue.GetComponent<Text>().text += "</color>";
                yield return new WaitForSeconds(letterPause);
            }
            typing = false;
        }
    }

    public void Talk()
    {
        if (typing)
        {
            skip = true;
        }
        else if ((location == 6 || location == 10 || location == 11 || location == 17) && h.love > 0 && !cracking)
        {
            cracking = true;
            StartCoroutine(Crack());
        }
        else if (!cracking)
        {
            StartCoroutine(TypeText(dialogueTexts[location]));
            location++;
        }
        if (location == 18)
        {
            h.frequency = 80;
        }
    }

    public IEnumerator Crack()
    {
        string temp = dialogue.GetComponent<Text>().text;
        dialogue.GetComponent<Text>().text = "";
        yield return new WaitForSeconds(0.5f);
        h.love -= 25;
        h.frequency -= 10;
        if (h.frequency < 10)
        {
            h.frequency = 10;
        }
        h.Crack((int)h.cracks + 1);
        if(h.cracks == 4)
        {
            dialogue.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(1f);
        cracking = false;
        StartCoroutine(TypeText(dialogueTexts[location]));
        location++;
    }

    public IEnumerator End()
    {
        location++;
        yield return new WaitForSeconds(6.0f);
        SceneManager.LoadScene("Menu");
    }
    /*
	public void Talk() {
		if (typing) {
			skip = true;
		} else if (narrating && !over) {
			if (curr == dialogueTexts [location].Length) {
				if (response != -1) {
					location = response;
					response = -1;
					curr = 0;
					StartCoroutine (TypeText (dialogueTexts [location] [curr]));
					curr++;
				} else if (response2 != -1) {
					location = response2;
					response2 = -1;
					curr = 0;
					StartCoroutine (TypeText (dialogueTexts [location] [curr]));
					curr++;
				} else {
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
				}
			} else {
				if(location == 26 && curr == 2) {
					song1.Play ();
				}
				if(location == 26 && curr == 3) {
					song1.Stop ();
				}
				StartCoroutine (TypeText (dialogueTexts [location] [curr]));
				foreach (Button b in options) {
					b.GetComponent<Text> ().text = "";
				}
				curr += 1; 
			}
		} else {
			selected = true;
			ExecuteOption ();
		}
	}


	public void OptionOne() {
		DeselectOptions (0);
		if (!selected) {
			ShakeText (options [0].gameObject);
		} else if(!narrating && !over && selected) {
			if (location == 0) {
				h.love += 1;
				location = 1; 
			}
			else if (location == 1) {
				location = 5; 
			} else if (location == 2) {
				location = 11;
				response = 1;
			} else if (location == 3) {
				h.love += 5f;
				location = 1; 
			} else if (location == 5) {
				h.love += 2f;
				location = 14; 
				response = 18;
			} else if (location == 7) {
				h.love += 2f;
				location = 16; 
				response = 18;
			} else if (location == 8) {
				h.love += 1f;
				location = 1; 
			} else if (location == 18 || location == 19 || location == 22) {
				if (option1) {
					optionsTexts [22].SetValue ("", 0);
					optionsTexts [19].SetValue ("", 0);
					dialogueTexts [22].SetValue ("So what else do you like?", 0);
					option1 = false;
					location = 23; 
					response = 22;
					h.love -= 1f;
				} else {
					return;
				}
			} else if (location == 24) {
				h.love += 3f;
				location = 25; 
			} else if (location == 25) {
				h.love += 8f;
				location = 26; 
				response = 22; 
			} else if (location == 29) {
				location = 30; 
			} else if (location == 30) {
				h.love += 4f;
				location = 31; 
				response = 22;
			} else if (location == 33) {
				h.love -= 3f;
				location = 34; 
				response = 22;
			} else if (location == 36) {
				location = 41;
				response = 20;
				response2 = 100;
				h.love -= 3f;
			} else if (location == 38) {
				location = 39;
				response = 22;
				h.love += 3f;
			} else {
				return;
			} 
			TransitionToTalk();
		}
	}

	public void OptionTwo() {
		DeselectOptions (1);
		if (!selected) {
			ShakeText (options [1].gameObject);
		} else if(!narrating && !over && selected) {
			if (location == 0) {
				h.love += 1f;
				location = 2; 
			}
			else if (location == 1) {
				h.love += 2f;
				location = 6; 
				slightRedirect = true; 
			} else if (location == 2) {
				h.love += 3f;
				location = 12;
				response = 1;
			} else if (location == 3) {
				h.love -= 5f;
				location = 13; 
				response = 1;
			} else if (location == 5) {
				h.love -= 2f;
				location = 15; 
				response = 18;
			} else if (location == 7) {
				location = 17; 
				response = 18;
			} else if (location == 8) {
				h.love -= 5f; 
				location = 9;
				response = 1;
			} else if (location == 18 || location == 19 || location == 22) {
				if (option2) {
					optionsTexts [22].SetValue ("", 1);
					optionsTexts [19].SetValue ("", 1);
					dialogueTexts [22].SetValue ("Anything else you're into?", 0);
					option2 = false;
					location = 24; 
					h.love += 3f;
				} else {
					return;
				}
			} else if (location == 24) {
				h.love -= 2f;
				location = 29; 
			} else if (location == 25) {
				h.love += 2f;
				location = 27; 
				response = 22;
			} else if (location == 29) {
				h.love -= 2f;
				location = 22; 
			} else if (location == 30) {
				h.love -= 2f;
				location = 32; 
				response = 22;
			} else if (location == 33) {
				h.love += 2f;
				location = 35; 
				response = 22;
			} else if (location == 36) {
				location = 37;
				response = 22;
				h.love += 2f;
			} else if (location == 38) {
				location = 40;
				response = 22;
				h.love -= 1f;
			} else {
				return;
			}
			TransitionToTalk ();
		}
	}

	public void OptionThree() {
		DeselectOptions (2);
		if (!selected) {
			ShakeText (options [2].gameObject);
		} else if(!narrating && !over && selected) {
			if (location == 0) {
				location = 3; 
				h.love += 5f;
			} else if (location == 1) {
				h.love += 5;
				location = 7; 
			} else if (location == 3) {
				location = 8; 
			} else if (location == 5 || location == 7) {
				h.love -= 10f; 
				h.frozen += 5f; 
				location = 10;
				response = 100;
			} else if (location == 25) {
				h.love -= 3f;
				location = 28;
				response = 22;
			} else if (location == 24) {
				h.love -= 1f;
				location = 33; 
			} else if (location == 18 || location == 19 || location == 22) {
				if (option3) {
					optionsTexts [22].SetValue ("", 2);
					optionsTexts [19].SetValue ("", 2);
					dialogueTexts [22].SetValue ("So what else are you into?", 0);
					option3 = false;
					location = 36; 
					h.love += 3f;
				} else {
					return;
				}
			} else if (location == 36) {
				location = 38;
			} else {
				return;
			}
			TransitionToTalk ();
		}
	}

	public void OptionFour() {
		DeselectOptions (3);
		if (!selected) {
			ShakeText (options [3].gameObject);
		} else if(!narrating && !over && selected) {
			if (location == 0) {
				location = 4; 
				response = 100;
				h.frozen += 30f; 
			} else if (location == 18 || location == 22) {
				location = 19; 
				if (!option1 && !option2 && !option3) {
					location = 42; 
					response = 100; 
					h.love += 3;
				}
			} else if (location == 19) {
				location = 20;
				response = 100;
				h.love -= 3f;
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
		StartCoroutine(TypeText(dialogueTexts [location] [curr]));
		//dialogue.GetComponent<Text> ().text = dialogueTexts [location] [curr];
		curr++;
 	}

	

	public void DeselectOptions(int op) {
		int i = 0;
		while(i < 4) {
			if (i != op) {
				KeyButton k = options [i].GetComponent<KeyButton> ();
				k.StartColorTween (options [i].colors.normalColor, false);
			}
			i++;
		}
	}

	public void ExecuteOption() {
		int i = 1;
		foreach (Button b in options) {
			if (b.GetComponent<Graphic> ().canvasRenderer.GetColor() == 
				b.colors.pressedColor) {
				if (i == 1) {
					OptionOne ();
				} else if (i == 2) {
					OptionTwo ();
				} else if (i == 3) {
					OptionThree ();
				} else {
					OptionFour ();
				}
				KeyButton k = b.GetComponent<KeyButton> ();
				k.StartColorTween (b.colors.normalColor, false);
				break;
			}
			i++;
		}
	}

	public void ShakeText(GameObject g) {
		iTween.ShakePosition(g, new Vector3(5, 0f, 0f), 0.2f);	
	}

	public void InitializeText() {
		string[] template = { "" };  // "template"
		for (int i = 0; i < 2000; i++) {
			dialogueTexts.Add (template); 
			optionsTexts.Add (template);
		}
		// ------------------------------------------------------------------
		// Scene 1: 0-99
		// ------------------------------------------------------------------

		string[] dialogue0 = { "Oh hey didnt see you there", "My name is Q" };
		dialogueTexts[0] = dialogue0;
		string[] dialogue1 = {"You go to Brooklyn Tech right?", 
			"I wonder sometimes if all the studying for its entrance exam was worth it",
			"It seems like we'll be sharing this commute by train together quite often",
			"Assuming you regularly wake up on time that is"
		};
		dialogueTexts[1] = dialogue1;
		string[] dialogue2 = {"Yeah my parents are pretty strange", 
			"They have quite the thing for unique names", 
			"Kind of like Alaska's parents if you've ever read John Green", 
			"Better than being stuck with my sister &'s name though"
		};
		dialogueTexts[2] = dialogue2;
		string[] dialogue3 = {"Well thank you", 
			"Kinda creepy that you'd say that to a complete stranger", 
			"Makes me question your intentions a little bit", 
			"But comments like that do wonders to my self esteem"
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
			"You *are* going to Brooklyn Tech", "If you weren't studious you wouldn't have gotten in", 
			"Honestly I'd wake up later but my mom would have my ass"
		};
		dialogueTexts[5] = dialogue5;
		string[] dialogue6 = {"Agreeeeed", 
			"If I could sleep until 11 each morning..", "Man that'd be the life", 
			"Be so well rested..", "Lounge around my room for an hour..", "Watch some youtube on my bed...",
			"My mom would have my ass though"
		};
		dialogueTexts[6] = dialogue6;// goes to 5
		string[] dialogue7 = {"Awww that's sweet of you", "I'll hold you to that promise", 
			"I'm glad you're the first person I met going to this school though", 
			"I hear most of the people are creeps"};
		dialogueTexts[7] = dialogue7;
		string[] dialogue8 = {"Oh *laughs* my bad"};
		dialogueTexts[8] = dialogue8;
		string[] dialogue9 = {"..yeaaahh.."};
		dialogueTexts[9] = dialogue9;
		string[] dialogue10 = {"Oh.. okay", "see you around"};
		dialogueTexts[10] = dialogue10;
		string[] dialogue11 = {"I like to think of it as quirky", 
			"Not everyone gets to have a unique name", 
			"And at least it's not hard to spell or remember"};
		dialogueTexts[11] = dialogue11;
		string[] dialogue12 = {"*laughs* I'll have to let her know lucky she is"};
		dialogueTexts[12] = dialogue12;
		string[] dialogue13 = {"Okay, please stop it with that now", "You're coming off as a bit of a douche"};
		dialogueTexts[13] = dialogue13;
		string[] dialogue14 = {"You have no idea", "she's the most stressful part of my life", 
			"Think Russian tiger-mom with a dash of crazy", 
			"Still love her lots but sometimes I just can't even"};
		dialogueTexts[14] = dialogue14;
		string[] dialogue15 = {"Yeah.."};
		dialogueTexts[15] = dialogue15;
		string[] dialogue16 = {"Don't seem creepy to me *smiles*"};
		dialogueTexts[16] = dialogue16;
		string[] dialogue17 = {"I guess not but who knows?", "Most of my middle school was full of creeps", 
			"So my expectations are pretty low"};
		dialogueTexts[17] = dialogue17;
		string[] dialogue18 = {"So what kinds of things are you into?"};
		dialogueTexts[18] = dialogue18;
		string[] dialogue19 = {"Anything specific?"};
		dialogueTexts[19] = dialogue19;
		string[] dialogue20 = {"*You stand with each other quietly for awhile*", "*The train comes to a stop*", 
			"Oh, I guess we're here", "I'll see you around"};
		dialogueTexts[20] = dialogue20;
		string[] dialogue21 = {"*The train comes to a stop*", "Oh, I guess we're here", "I'll see you around"};
		dialogueTexts[21] = dialogue21;
		string[] dialogue22 = {"So what else are you into?"};
		dialogueTexts[22] = dialogue22;
		string[] dialogue23 = {"I'm not really into gaming unless you count board games", 
			"I do play solitaire on my ipod", "But I don't know if that really counts for much"};
		dialogueTexts[23] = dialogue23;
		string[] dialogue24 = {"Oh what kinds of music do you listen to?", 
			"I mostly listen to classic rock and alternative"};
		dialogueTexts[24] = dialogue24;
		string[] dialogue25 = {"Awesome! I've really been wanting to get into more electronic", 
			"Can you show me one of your favorites?"};
		dialogueTexts[25] = dialogue25;
		string[] dialogue26 = {"*You pull out your ipod and hand them an earbud*", 
			"*You look through your music before selecting For You by Madeon*", 
			"*Q listens and bobs their head to the beat*", 
			"That was great!", "We'll have to exchange music sometime *smiles*"};
		dialogueTexts[26] = dialogue26;
		string[] dialogue27 = {"Definitely! *smiles*"};
		dialogueTexts[27] = dialogue27;
		string[] dialogue28 = {"Oh... yeah sure"};
		dialogueTexts[28] = dialogue28;
		string[] dialogue29 = {"Oh.. that's too bad", "I really like metal actually"};
		dialogueTexts[29] = dialogue29;
		string[] dialogue30 = {"Oh well if all you ever hear is thrash then maybe you'd like some other genres of metal", 
			"I don't even like thrash all that much"};
		dialogueTexts[30] = dialogue30;
		string[] dialogue31 = {"Nice! *smiles*", "We'll have to exchange music sometime"};
		dialogueTexts[31] = dialogue31;
		string[] dialogue32 = {"Oh...", "That's too bad then..."};
		dialogueTexts[32] = dialogue32;
		string[] dialogue33 = {"Not really a huge fan of rap", "It's kinda bigoted", "Like... 95% of it at least"};
		dialogueTexts[33] = dialogue33;
		string[] dialogue34 = {"Yeah..."};
		dialogueTexts[34] = dialogue34;
		string[] dialogue35 = {"Oh? Huh", "I'll have to listen to some of your music sometime then", 
			"maybe I could get into it"};
		dialogueTexts[35] = dialogue35;
		string[] dialogue36 = {"Same! I've been mostly watching Dr. Who, Sherlock and HIMYM", 
			"You watch any of those shows?"};
		dialogueTexts[36] = dialogue36;
		string[] dialogue37 = {"I think I've got enough shows as it is *laughs*", 
			"I think you'd really like those first two if you've never seen them before though"};
		dialogueTexts[37] = dialogue37;
		string[] dialogue38 = {"Oh it's How I Met Your Mother", 
			"It's basically a cheesy sitcom", 
			"But I've rewatched it about thirty times"};
		dialogueTexts[38] = dialogue38;
		string[] dialogue39 = {"You should!", "I'd love it if you did"};
		dialogueTexts[39] = dialogue39;
		string[] dialogue40 = {"Yeah a bit", "Watching it relaxes me though", 
			"A lot of the time I just periodically look over at it while doing work"};
		dialogueTexts[40] = dialogue40;
		string[] dialogue41 = {"Oh... that's too bad"};
		dialogueTexts[41] = dialogue41;
		string[] dialogue42 = {"*The train comes to a stop*", 
			"Oh, I guess we're here.",
			"I'll see you around *smiles*"};
		dialogueTexts[42] = dialogue42;
		string[] options0 = {"Hey nice to meet you", "What an odd name", 
			"Wow you're stunning", "Fuck off" };
		optionsTexts[0] = options0;   
		string[] options1 = {"I tend to be on time", "Every day I sleep in is a success", 
			"I'll wake up to make sure you have some company" };
		optionsTexts[1] = options1;
		string[] options2 = { "Wow.. yeah that's pretty odd", "I WISH my name was &, what a badass moniker" };
		optionsTexts[2] = options2;
		string[] options3 = {"I don't suppose a fine specimen like you happens to be on her way to my high school as well?", 
			"I say it like it is sweet stuff", "Yeah sorry about that you kinda caught me off guard"
		};
		optionsTexts[3] = options3;
		string[] options5 = { "Strict parents?", "Yeah moms can be annoying like that", 
			"God I'm tired... sorry I'm just going to sit down for now" };
		optionsTexts[5] = options5;
		string[] options7 = { "Who says I'm not?", 
			"Not sure if that's the best attitude to have on your first day *laughs*", 
			"God I'm tired... sorry I'm just going to sit down for now" };
		optionsTexts[7] = options7;
		string[] options8 = { "*laughs* it's okay", "...yeah..." };
		optionsTexts[8] = options8;
		string[] options18 = {"Mostly video games, but some other stuff as well", 
			"Recently I've been really into music", 
			"I've been watching a fair number of shows lately", "Lots of different stuff" };
		optionsTexts[18] = options18;  
		string[] options19 = {"Mostly video games, but some other stuff as well", 
			"Recently I've been really into music", 
			"I've been watching a fair number of shows lately", "No" };
		optionsTexts[19] = options19;  
		string[] options22 = {"Mostly video games, but some other stuff as well", 
			"Recently I've been really into music", 
			"I've been watching a fair number of shows lately", "Lots of different stuff" };
		optionsTexts[22] = options22;  
		string[] options24 = {"I love those genres, I mostly listen to alternative and electronic overall", 
			"A little of everything, I hate metal and country though", 
			"I've been listening to a lot of rap lately" };
		optionsTexts[24] = options24; 
		string[] options25 = {"Yeah sure!", 
			"Sure, we can exchange music sometime", 
			"Maybe some other time" };
		optionsTexts[25] = options25; 
		string[] options29 = {"Yeah.. it's mostly just because my brother blasts thrash metal all day", 
			"Too bad indeed"};
		optionsTexts[29] = options29; 
		string[] options30 = {"I'd give it a shot", 
			"I still don't think I'd like it"};
		optionsTexts[30] = options30; 
		string[] options33 = {"Yeah, well, that's the genre I suppose", 
			"I do listen to some of that, but most of my favorite artists are clean"};
		optionsTexts[33] = options33; 
		string[] options36 = {"No", 
			"Not yet but I'm always looking for new shows", 
			"What's a HIMYM?" };
		optionsTexts[36] = options36; 
		string[] options38 = {"Must be entertaining, I'll have to watch it sometime", 
			"Wow that is... obsessive"};
		optionsTexts[38] = options38; 

		// ------------------------------------------------------------------
		// Scene 2: 100-199
		// ------------------------------------------------------------------

		string[] dialogue100 = { "Scene 2" };
		dialogueTexts[100] = dialogue100;
	}
	*/
}

