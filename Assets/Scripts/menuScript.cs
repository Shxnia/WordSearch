using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GemMine.EasyEasing;

namespace GemMine.WordSearch {

	//
	// This script is attache to th emenuManager gameObject
	// it handles all menu interaction
	//

	public class menuScript : MonoBehaviour {

		// refernce to the main panel sweeping in from the left
		public GameObject menuPanel;
		// and the whole panel which can be moved
		public GameObject mainPanel;


		// Reference to the lit on category screen
		// to be filled via script 
		public GameObject listEntry;
		public GameObject container;

		// Toggle button on the settings screen
		public Toggle toggleClassic;
		public Toggle toggleBlitz;
		public Toggle toggleEasy;
		public Toggle toggleMedium;
		public Toggle toggleHard;
		public Toggle toggleHint;

		// we need access to the database
		DataService ds;


		// Use this for initialization
		void Start () {

			// move main menu in 
			easyEasing.Vector2To (menuPanel,
				easyEasing.Params ("easeType", "easeOutQuad",
					"from", (menuPanel.GetComponent<RectTransform> ().anchoredPosition),
					"to", (menuPanel.GetComponent<RectTransform> ().anchoredPosition + new Vector2(1080,0)),
					"duration", 0.5f,
					"onUpdateTarget", this.gameObject,
					"onUpdate", "moveMenuPanel"));

			// initialize and read all the playerprefs
			initializePlayerPrefs ();
			// initialize the categories
			initializeCategories();
		}
		


		//
		// public void initializeCategories()
		//
		// set the puzzle categories
		//

		public void initializeCategories() {
			// establish connection to wordsearch database
			ds = new DataService ("wordsearch.db");

			// get the list of existing categories (aka table categories)
			var categories = ds.getCategories ();

			// now calculate the number of different categories
			int numcat = ds.getCategoriesCount ();

			// set the scroll container's size
			container.GetComponent<RectTransform> ().sizeDelta = new Vector2 (880, numcat * 160);
			// set the scroll container's position
			container.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, (1000-numcat*160)/2);

			// calculate the starting position of first entry
			int startpos = (numcat * 160 - 160) / 2;

			int counter = 0;

			// iterate the categories
			foreach (category cat in categories) {
				// create GO
				GameObject entry = Instantiate (listEntry, Vector3.zero, Quaternion.identity) as GameObject;
				// parent it to the scroll view
				entry.transform.SetParent (container.transform);
				// set scale and position
				entry.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1); 
				entry.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0,startpos-counter*160);
				// check if the number is dividable by 2
				// used for background color selection
				if (counter % 2 == 0)
					entry.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Textures/categories/blue");
				else
					entry.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Textures/categories/gray");
				// set the button's icon and text
				Sprite sprite = Resources.Load<Sprite> ("Textures/categories/"+cat.Name.ToLower());
				if (sprite == null) {
					Debug.Log("failed to load " + cat.Name.ToLower());
					sprite = Resources.Load<Sprite> ("Textures/categories/animals");
				}
				entry.transform.Find ("Icon").GetComponent<Image>().sprite = sprite;
				entry.transform.Find ("Text").GetComponent<Text>().text = cat.Name.ToUpper();
				// tried to set cat.id in the listener - did not work
				// so I had to try this fallback solution
				int id = (cat.Id);
				entry.GetComponent<Button>().onClick.AddListener(() => { catClicked(id);}); 
				counter++;
			}
		}


		//
		// public void initializePlayerPrefs()
		//
		// This method sets the standard settings
		//

		public void initializePlayerPrefs() {
			// set standard mode to "Classic"
			// Classic = 1
			// Blitz = 2
			if (!PlayerPrefs.HasKey("Mode")) {
				PlayerPrefs.SetInt ("Mode", 1);
			} 
			else {
				if (PlayerPrefs.GetInt("Mode") == 1)
					toggleClassic.isOn = true;
				else
					toggleBlitz.isOn = true;
			}
			// set standard mode to "Easy"
			// Easy = 1
			// Medium = 2
			// Hard = 3
			if (!PlayerPrefs.HasKey("Level")) {
				PlayerPrefs.SetInt ("Level", 1);
			} 
			else {
				if (PlayerPrefs.GetInt("Level") == 1)
					toggleEasy.isOn = true;
				if (PlayerPrefs.GetInt("Level") == 2)
					toggleMedium.isOn = true;
				if (PlayerPrefs.GetInt("Level") == 3)
					toggleHard.isOn = true;
			}
			// set standard mode to "Hints on"
			// (Show list of words to guess)
			// On = 1
			// Off = 0
			if (!PlayerPrefs.HasKey("Hints")) {
				PlayerPrefs.SetInt ("Hints", 1);
				toggleHint.isOn = true;
			} 
			else {
				if (PlayerPrefs.GetInt("Hints") == 1)
					toggleHint.isOn = true;
				else
					toggleHint.isOn = false;
			}
		}



		// Tween-moves
		// for the main panel

		public void moveMenuPanel(Vector2 position) {
			menuPanel.GetComponent<RectTransform> ().anchoredPosition = position;
		}


		public void moveMainPanel(Vector2 position) {
			mainPanel.GetComponent<RectTransform> ().anchoredPosition = position;
		}



		// main menu screen

		public void OnPlayClicked() {
			easyEasing.Vector2To (mainPanel,
				easyEasing.Params ("easeType", "easeOutQuad",
					"from", (mainPanel.GetComponent<RectTransform> ().anchoredPosition),
					"to", (mainPanel.GetComponent<RectTransform> ().anchoredPosition - new Vector2(1080,0)),
					"duration", 0.5f,
					"onUpdateTarget", this.gameObject,
					"onUpdate", "moveMainPanel"));
		}


		// settings screen (1)

		public void OnMenuClicked() {
			easyEasing.Vector2To (mainPanel,
				easyEasing.Params ("easeType", "easeOutQuad",
					"from", (mainPanel.GetComponent<RectTransform> ().anchoredPosition),
					"to", (mainPanel.GetComponent<RectTransform> ().anchoredPosition + new Vector2(1080,0)),
					"duration", 0.5f,
					"onUpdateTarget", this.gameObject,
					"onUpdate", "moveMainPanel"));
		}

		public void OnNextClicked() {
			easyEasing.Vector2To (mainPanel,
				easyEasing.Params ("easeType", "easeOutQuad",
					"from", (mainPanel.GetComponent<RectTransform> ().anchoredPosition),
					"to", (mainPanel.GetComponent<RectTransform> ().anchoredPosition - new Vector2(1080,0)),
					"duration", 0.5f,
					"onUpdateTarget", this.gameObject,
					"onUpdate", "moveMainPanel"));
		}

		public void modeClicked() {
			if (toggleClassic.isOn == true)
				PlayerPrefs.SetInt ("Mode", 1);
			if (toggleBlitz.isOn == true)
				PlayerPrefs.SetInt ("Mode", 2);
		}

		public void levelClicked() {
			if (toggleEasy.isOn == true)
				PlayerPrefs.SetInt ("Level", 1);
			if (toggleMedium.isOn == true)
				PlayerPrefs.SetInt ("Level", 2);
			if (toggleHard.isOn == true)
				PlayerPrefs.SetInt ("Level", 3);
		}

		public void hintsClicked() {
			if (toggleHint.isOn == true)
				PlayerPrefs.SetInt ("Hints", 1);
			else
				PlayerPrefs.SetInt ("Hints", 0);
		}

		// categories screen (2)

		public void OnBackClicked() {
			easyEasing.Vector2To (mainPanel,
				easyEasing.Params ("easeType", "easeOutQuad",
					"from", (mainPanel.GetComponent<RectTransform> ().anchoredPosition),
					"to", (mainPanel.GetComponent<RectTransform> ().anchoredPosition + new Vector2(1080,0)),
					"duration", 0.5f,
					"onUpdateTarget", this.gameObject,
					"onUpdate", "moveMainPanel"));
		}

		public void OnFinishClicked() {
			PlayerPrefs.SetInt("Category", 1);
			SceneManager.LoadScene("wordPuzzle");}

		public void catClicked(int cat) {
			//category catname = ds.getCategory (cat);
			PlayerPrefs.SetInt("Category", cat);
			SceneManager.LoadScene("wordPuzzle");
		}
	}
}
