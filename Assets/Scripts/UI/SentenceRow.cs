using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SentenceRow : MonoBehaviour
{
    public WordText wordPrefab;
	public WordText wordPrefabDef;

    internal RectTransform rt;

    private Stack<WordText> textStack = new Stack<WordText>();

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        wordPrefab.gameObject.SetActive(false);
    }

    public WordText AddText(string word)
    {
		WordText wordClone;
		if (word.Substring (0, 1) == "#") {
			wordClone = Instantiate(wordPrefabDef, wordPrefab.transform.parent);	
		} else {
		wordClone = Instantiate(wordPrefab, wordPrefab.transform.parent);	
		}
        


        wordClone.gameObject.SetActive(true);
        wordClone.text.text = word;

		if (wordClone.text.text.Substring(0, 1) == "#") {
			//Debug.Log (wordClone.text.text);
			//wordClone.GetComponent<Button> ().colors
			//Changes the button's Normal color to the new color.
			/*ColorBlock cb = wordClone.GetComponent<Button> ().colors;
			cb.normalColor = Color.green;
			wordClone.GetComponent<Button> ().colors = cb;*/

			ColorBlock buttonColors;




			var strs = wordClone.text.text.Split("#"[0]);

			//Debug.Log (strs[1]);
			wordClone.text.text = strs[1];
			//wordClone.GetComponent<Button> ().colors = Color.cyan;
		}

        textStack.Push(wordClone);
        return wordClone;

		wordPrefabDef.gameObject.SetActive(false);
    }

    public void PopText()
    {
        Destroy(textStack.Pop().gameObject);
    }
}
