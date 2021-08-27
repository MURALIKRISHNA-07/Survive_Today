using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMenu : MonoBehaviour
{
    public Level[] levels;

    public GameObject Fpscam;
    private int currentlevel = 0;
    public Text Continue;

    public GameObject DialogueBox;
    public Text DiaText;

    private Queue<string> sentences;

    public bool started = false;
    public bool display = false;
    public bool CHANGESCENE;

    private void Start()
    {
        currentlevel = PlayerPrefs.GetInt("Level");
        sentences = new Queue<string>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Fpscam.transform.position, Fpscam.transform.forward , out hit, 2f))
        {
            if (hit.collider.tag == "Letter" && !started)
            {
                UIManager.instance.EnterUIState(UIManager.UIState.Dialogues);
                this.gameObject.GetComponent<PlayerMovement>().speed = 0f;
                this.gameObject.GetComponent<MouseLook>().sensivity = 0f;
                interact();

            }

        }

        if (Input.GetKey(KeyCode.F) && display)
        {
            Displaynext();
        }
        if(Input.GetKeyDown(KeyCode.I)&&CHANGESCENE)
        {
            UIManager.instance.StartGame();
        }
        // Debug.DrawRay(Fpscam.transform.position, Fpscam.transform.forward, Color.red, 2f);
    }
    void interact()
    {
            Continue.enabled = true;
            Continue.text = "" + "PRESS F TO INTERACT";
           
            if (Input.GetKey(KeyCode.F))
            {
                started = true;
                startdialogue(levels[currentlevel]);
               // currentlevel++;
            }
        
    }

    public void startdialogue(Level thislevel)
    {
        //clearing so that it will be empty
        sentences.Clear();

        foreach (string sentence in thislevel.dialogue)
        {
            sentences.Enqueue(sentence);
        }
        DialogueBox.SetActive(true);

        Displaynext();
    }

    //for the next sentence in dialogue
    public void Displaynext()
    {
        display = false;
        Continue.text = "";
        if (sentences.Count == 0)
        {
            enddialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();//stoping previous one's so that sentence would be neat and clean
        StartCoroutine(Type(sentence));//starting auto type the sentence 
    
    }

    public void enddialogue()
    {
        //RESETTING DIALOGUE AND CONTINUE texts
       DiaText.text = "";
       DialogueBox.SetActive(false);

       Continue.text = "PRESS I TO CONTINUE";
       Continue.enabled = true;
        CHANGESCENE = true;
       
    }

    //Passing sentence as parameter
    IEnumerator Type(string sen)
    {
        DiaText.text = "";
        foreach (char letter in sen.ToCharArray())
        {
            DiaText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new  WaitForSeconds(1f);
      //  Continue.enabled = true;
        Continue.text = "" + "PRESS F TO INTERACT";
        display = true;
    }
}

[System.Serializable]
public struct Level
{
    [TextArea(2, 7)]
    public string[] dialogue;
}