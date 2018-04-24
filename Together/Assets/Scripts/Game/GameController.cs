using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the game, but most notably the ending sequence. 
/// </summary>
public class GameController : MonoBehaviour {

	/// <summary>
	/// The player heart.
	/// </summary>
	public Beat heart;

	/// <summary>
	/// The stress UI visual.
	/// </summary>
	public Text stress;

	/// <summary>
	/// The compassion UI visual.
	/// </summary>
	public Text compassion;

	/// <summary>
	/// The love UI visual.
	/// </summary>
	public Text love;

	/// <summary>
	/// Instructional control scheme text. 
	/// </summary>
	public Text arrows;

	public GameObject gameover;
	private bool over;
	public Text dialogue; 
	// set the response to the location you want it to go to after
	private int location;

	/// <summary>
	/// The amount of time before another letter is typed. 
	/// </summary>
	private float letterPause;

	/// <summary>
	/// Whether the current dialogue is being typed.
	/// </summary>
	private bool typing;

	/// <summary>
	/// True if the player wants to skip the text delay.
	/// </summary>
	private bool skip;

	/// <summary>
	/// The dialogue texts.
	/// </summary>
    private List<string> dialogueTexts = new List<string>();

    private bool cracking;

	/// <summary>
	/// Whether or not something is currently fading in. 
	/// </summary>
	private bool fadeIn;

	/// <summary>
	/// Whether or not something is currently fading out. 
	/// </summary>
	private bool fadeOut;

	/// <summary>
	/// Both the speed and frequency of the falling text. 
	/// </summary>
    public static float speed; 

	/// <summary>
	/// Text indicating that the player should press the spacebar. 
	/// </summary>
	public Text space;

	/// <summary>
	/// Whether or not the spacebar text should be active. 
	/// </summary>
	private bool pressSpace;

	/// <summary>
	/// The list of falling texts. 
	/// </summary>
	public List<GameObject> texts; 

	/// <summary>
	/// Timed counter indicating the next falling text. 
	/// </summary>
	private float counter;

	/// <summary>
	/// The current falling text index. 
	/// </summary>
	[SerializeField]
	private int index;
    public static bool ending;

	public void Start() {
		over = false;
		location = 0;
		letterPause = 0.005f;
		typing = false;
		skip = false;
        gameover.SetActive(false);
        ending = false;
		pressSpace = false;
        speed = 1;
        cracking = false;
		StartCoroutine (Arrows ());


		index = 0;
		counter = 150;
		dialogueTexts.Add("They love you");
		dialogueTexts.Add("But over time feelings change");
		dialogueTexts.Add("Love is fleeting and the past, only memories");
		dialogueTexts.Add("Even though you still feel the same love towards them");
		dialogueTexts.Add("They will never feel the same way about you");
		dialogueTexts.Add("They break up with you"); // crack love - 25
		dialogueTexts.Add(". . .");
		dialogueTexts.Add("You feel lost");
		dialogueTexts.Add("They were your whole world");
		dialogueTexts.Add("What made you get up in the morning"); // crack love - 25
		dialogueTexts.Add("What brought you joy during hard times"); // crack love - 25
		dialogueTexts.Add("What gave your life purpose");
		dialogueTexts.Add(". . .");
		dialogueTexts.Add("You can't live like this");
		dialogueTexts.Add("Without something to live for");
		dialogueTexts.Add("You consider ending it all");
		dialogueTexts.Add(". . ."); // final crack
		dialogueTexts.Add("But you don't");
		dialogueTexts.Add("Because even if you'll never get back together");
		dialogueTexts.Add("Maybe someday you'll be able to feel the same love again");
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
                    heart.end = true;
                    dialogue.gameObject.SetActive(true);
                    Talk();
                }
                else
                {
                    counter--;
                }
            }
            stress.text = "Stress: " + (100 - heart.Frequency);
			compassion.text = "Compassion: " + (100 - heart.Frozen);
			love.text = "Love: " + (heart.Love);
		}
        
	}

	/// <summary>
	/// Restarts the game from the menu. 
	/// </summary>
	public void Restart()
	{
		SceneManager.LoadScene("Menu");
	}


	/// <summary>
	/// Types the given message on screen. 
	/// </summary>
	/// <returns>The text.</returns>
    IEnumerator TypeText(string message)
    {
        if (!cracking)
        {
            typing = true;
            string current = "";
            dialogue.text = "";
            char[] m = message.ToCharArray();
            for (int i = 0; i < message.Length; i++)
            {
                if (skip)
                {
                    dialogue.text = message;
                    skip = false;
                    break;
                }
                current += m[i];
                dialogue.text = current;
                dialogue.text += "<color=#00000000>";
                for (int j = i + 1; j < message.Length; j++)
                {
                    dialogue.text += m[j];
                }
                dialogue.text += "</color>";
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
        else if ((location == 6 || location == 10 || location == 11 || location == 17) && heart.Love > 0 && !cracking)
        {
			if (!(location == 17) || heart.Love > 10) {
				cracking = true;
				StartCoroutine (Crack ());
			} else {
				heart.Love = 0;
			}
        }
		else if (!cracking && location < dialogueTexts.Count)
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
            heart.Frequency = 80;
        }
    }

    public IEnumerator Crack()
    {
        dialogue.text = "";
        yield return new WaitForSeconds(0.5f);
        heart.Love -= 25;
        heart.Frequency -= 10;
        if (heart.Frequency < 10)
        {
            heart.Frequency = 10;
        }
        heart.Crack();
        if(heart.cracks == 4)
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

