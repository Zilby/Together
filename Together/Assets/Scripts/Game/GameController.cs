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
	public Text arrows;
	public GameObject gameover;
	public bool over;
	public bool selected;
	public Button dialogue; 
	private bool narrating;
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
	private bool fadeIn;
	private bool fadeOut;
    public static float speed; // speed/frequency of the falling text 
	public Text space;
	private bool pressSpace;

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
		pressSpace = false;
        speed = 1;
        cracking = false;
		StartCoroutine (Arrows ());


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

	void Update() {
		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}
		if (fadeIn && !pressSpace) {
			arrows.color = Color.Lerp (arrows.color, new Color (1, 1, 1, 1), 1.2f * Time.deltaTime);
			if (arrows.color.a > 0.95) {
				fadeIn = false;
				fadeOut = true;
			}
		} else if (fadeOut && !pressSpace) {
			arrows.color = Color.Lerp (arrows.color, new Color (1, 1, 1, 0), 1.5f * Time.deltaTime);
			if (arrows.color.a < 0.01) {
				fadeOut = false;
                arrows.enabled = false;
			}
		}
		if (pressSpace) {
			if (fadeIn) {
				space.color = Color.Lerp (space.color, new Color (1, 1, 1, 1), 1.2f * Time.deltaTime);
				if (space.color.a > 0.95) {
					fadeIn = false;
					fadeOut = true;
				}
			} else if (fadeOut) {
				space.color = Color.Lerp (space.color, new Color (1, 1, 1, 0), 1.5f * Time.deltaTime);
				if (space.color.a < 0.01) {
					fadeOut = false;
					fadeIn = true;
				}
			}
		}
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
                        speed += 0.03f;
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
            stress.text = "Stress: " + (100 - h.Frequency);
			compassion.text = "Compassion: " + (100 - h.Frozen);
			love.text = "Love: " + (h.Love);
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
        else if ((location == 6 || location == 10 || location == 11 || location == 17) && h.Love > 0 && !cracking)
        {
			if (!(location == 17) || h.Love > 10) {
				cracking = true;
				StartCoroutine (Crack ());
			} else {
				h.Love = 0;
			}
        }
        else if (!cracking)
        {
            StartCoroutine(TypeText(dialogueTexts[location]));
			if (location == 0 && !space.enabled) {
				space.enabled = true;
				StartCoroutine (PressSpace ());
			} else if (location == 1 && space.enabled) {
				space.enabled = false;
				pressSpace = false;
				fadeIn = false;
				fadeOut = false;
			}
            location++;
        }
        if (location == 18)
        {
            h.Frequency = 80;
        }
    }

    public IEnumerator Crack()
    {
        string temp = dialogue.GetComponent<Text>().text;
        dialogue.GetComponent<Text>().text = "";
        yield return new WaitForSeconds(0.5f);
        h.Love -= 25;
        h.Frequency -= 10;
        if (h.Frequency < 10)
        {
            h.Frequency = 10;
        }
        h.Crack();
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

	public IEnumerator Arrows() {
		yield return new WaitForSeconds (0.5f);
		fadeIn = true;
	}

	private IEnumerator PressSpace() {
		yield return new WaitForSeconds (4.0f);
		pressSpace = true;
		fadeIn = true;
	}
}

