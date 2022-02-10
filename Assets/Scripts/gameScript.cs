using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using GemMine.EasyEasing;

namespace GemMine.WordSearch {

	public class gameScript : MonoBehaviour {


		// References to GUI obects
		public GameObject menuPanel;
		public GameObject wonPanel;
		public GameObject alertPanel;
		public GameObject countdownPanel;
		public Text hoveringText;
		public Image backgroundImage;
		public Text zeichen;
		public Text wordList;
		public Text gameTime;
		public Image gridImage;
		public GameObject charMatrix;
		public GameObject gridMatrix;

		public Text textWon;
		public Text textCountdownInfo;
		public Text textCountdownWon;

		// menu entries
		public Toggle pauseToggle;
		public Toggle gridToggle;
		public Toggle nightToggle;

		public RectTransform mainMenuPanel;
		public RectTransform newGamePanel;

		// night and day mode
		public Sprite blackGrid, whiteGrid;

		public GameObject imgLine;
		public Canvas canvas;

		Vector2 startPosition = Vector2.zero;
		Vector2 endPosition = Vector2.zero;
		
		bool isDrawing;
		GameObject actualLine;

		bool menuVisible;

		bool wordFound;

		word actualWord;

		// the matrix to store the words in
		string[,] matrix;
		List<word> entries = new List<word>();
		// matrix size - depending on the smartphone's size
		int matrixSizeY = 16;
		int matrixSizeX = 12;

		// is the game over
		bool gameOver;

		int openWords, wordsToFind;

		// the clock increments every second
		float timerInterval = 1f;
		float timeElapsed;
		
		// user friendly time display in the xx:yy format
		float secondsElapsed;
		float minutesElapsed;
		float countdown;

		DataService ds;

		Vector2 menuPosition;

		float scaleFactor;
		float leftOffset;

		public Vector2 DeviceResolution;
		public int DeviceWidth = 1080;

		bool isDown = false;
		bool isDrawing1 = false;

		Vector2 matrixStart;
		Vector2 matrixEnd;

		// did the plaer start at least the menu once?
		// Playerprefs must be initialized
		void Awake() {
			if (!PlayerPrefs.HasKey("Hints"))
				SceneManager.LoadScene("menu");
		}

		// Use this for initialization
		void Start () {
			// Canvas scaler uses height of device for scaling
			scaleFactor = Screen.height / 1920f;
			leftOffset = (Screen.width - DeviceWidth * scaleFactor) / 2f;
			initGame ();
		}


		public void initGame() {
			float posX = (Screen.width - menuPanel.GetComponent<RectTransform> ().sizeDelta.x * scaleFactor) / 2 / scaleFactor;
			menuPosition = new Vector2 (-posX- menuPanel.GetComponent<RectTransform> ().sizeDelta.x, menuPanel.GetComponent<RectTransform> ().anchoredPosition.y);
			menuPanel.GetComponent<RectTransform> ().anchoredPosition = menuPosition;
			// we have a new game here
			gameOver = false;
			// Blitz Mode: 60 Seconds of fun
			countdown = 60;
			//
			wordsToFind = 0;
			// generate the puzzle matrix
			matrix = new string[matrixSizeY,matrixSizeX];
			// generate the wordlist
			fillList ();
			// fill the matrix with ""
			fillGrid();
			
			// how often shall we try to position a single word
			int maxtries = 55;
			int tries;
			// clear the hint list
			wordList.text = "";
			// iterate the word list
			for (int ab = entries.Count - 1; ab >= 0; ab--) {
				for (tries = 1; tries <= maxtries; tries++) {
					if (entries[ab].Name != "") {
						// generate a direction for the word
						int[] direction = generateDirection();
						// generate a position to place the word
						int[] position = generatePosition(entries[ab].Name, direction);
						// check, if position and direction are ok
						if (checkWord(entries[ab].Name, position, direction)) {
							// place the word in the matrix
							placeWord(entries[ab].Name, position, direction);
							// koordinates in tiles where the word starts 
							entries[ab].StartPosition = new Vector2(position[0],position[1]);
							// koordinates in tiles where the word ends 
							entries[ab].EndPosition = new Vector2(position[0]+direction[0]*(entries[ab].Length-1),position[1]+direction[1]*(entries[ab].Length-1));
							// has the word already been found?
							entries[ab].Found = false;
							// this word is used in the puzzle
							entries[ab].Used = true;
							// we need to store how many word the puzzle has
							wordsToFind++;
							break;
						}
					}
				}
			}
			
			// fill the rest of the matrix with random chars
			fillRandomGrid ();
			
			// build the hint list
			buildHintList ();
			
			// set menu start position
			menuVisible = false;
			
			// set the toggles to false
			pauseToggle.isOn = false;
			gridToggle.isOn = false;
			nightToggle.isOn = false;
			
			// calculate start position
			float startX = -(DeviceWidth - (DeviceWidth/12)) / 2;
			float startY = (1920 - (DeviceWidth/12)) / 2;

			// destroy the puzzle matrix
			// (if existent)
			GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("puzzleChar");
			foreach (GameObject target in gameObjects) {
				GameObject.Destroy(target);
			}

			gameObjects = GameObject.FindGameObjectsWithTag("imgLine");
			foreach (GameObject target in gameObjects) {
				GameObject.Destroy(target);
			}

			// now use the grid to draw the puzzle on the screen
			float actY = startY;
			for (int y = 0; y < matrixSizeY; y++) {
				float actX = startX;
				for (int x = 0;x < matrixSizeX; x++) {
					Text text = Instantiate (zeichen, Vector3.zero, Quaternion.identity) as Text;
					text.transform.SetParent (charMatrix.transform);
					text.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (actX + x*90, actY - y*90);
					text.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
					text.GetComponent<RectTransform> ().sizeDelta = new Vector2 (90, 90);
					text.GetComponent<Text> ().text = "A";
					//if (matrix [y, x] != "")
						text.GetComponent<Text> ().text = matrix [y, x];
					//else {
					//	text.GetComponent<Text> ().color = Color.red;
					//	text.GetComponent<Text> ().text = System.Convert.ToChar(Random.Range(65,91)).ToString();
					//}
					text.GetComponent<Text> ().fontSize = 64;
					text.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
					text.name = "puzzleChar";
					text.tag = "puzzleChar";
					text.GetComponent<puzzlecharScript> ().setMatrixPosition (y, x);
					text.GetComponent<puzzlecharScript> ().setScreenPosition (actX + x * 90, actY - y * 90);
					Image image = Instantiate(gridImage,Vector3.zero,Quaternion.identity) as Image;
					image.transform.SetParent(gridMatrix.transform);
					image.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (actX + x*90, actY - y*90);
					image.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
					image.GetComponent<RectTransform> ().sizeDelta = new Vector2 (90, 90);
					image.tag = "puzzleGrid";
				}
			}
		}


		// Update is called once per frame
		void Update() {
			// this part realizes the clock
			if (!gameOver) {
				// increment the clock
				timeElapsed += Time.deltaTime;
				if (timeElapsed > timerInterval) {
					timeElapsed -= timerInterval;
					if (PlayerPrefs.GetInt ("Mode") == 1) {
						secondsElapsed++;
						// convert the seconds to user friendly format
						if (secondsElapsed > 59) {
							secondsElapsed -= 60;
							minutesElapsed++;
						}
					} else {
						countdown--;
						if (countdown < 10 && !gameOver)
							alertPanel.GetComponent<alertPanel> ().flash ();
						if (countdown == 0) {
							gameOver = true;
							countdownPanel.gameObject.SetActive (true);
							if (openWords == 0) {
								textCountdownInfo.text = "Congratulations! You completed the puzzle!";
								textCountdownWon.text = "You found all words.";
							} 
							else {
								textCountdownInfo.text = "Sorry... You lost the game!";
								textCountdownWon.text = "You found " + (wordsToFind-openWords) +" out of " + wordsToFind + " words.";
							}
						}

					}
				}
			}

			// write text to the screen clock
			if (PlayerPrefs.GetInt ("Mode") == 1)
				gameTime.text = "TIME: " + minutesElapsed.ToString ("00") + ":" + secondsElapsed.ToString ("00");
			else {
				gameTime.text = "TIME: " + countdown.ToString ("00");
			}

			// Player pressed the left mouse button
			// the search for the word begins
			if (Input.GetButtonDown("Fire1")) {
				// check, if an used word starts at the clicked position
				actualWord = checkWordBegin(matrixStart);

			}

			// Player released the left mouse button
			if (Input.GetButtonUp ("Fire1")) {
				// the player released the button above the last char
				if (checkWordEnd(actualWord, matrixEnd)) {
					actualWord.Found = true;
					// remove the found word and rebuild the hint list
					buildHintList();

					endPosition.x = ((Input.mousePosition.x-Screen.width/2f) / Screen.width) * DeviceWidth ;
					endPosition.y = ((Input.mousePosition.y-Screen.height/2f) / Screen.height) * 1920f;
					Text textSlide = Instantiate (hoveringText, Vector3.zero, Quaternion.identity) as Text;
					textSlide.text = actualWord.Name.ToUpper();
					textSlide.transform.SetParent (backgroundImage.transform);
					textSlide.GetComponent<RectTransform> ().anchoredPosition = endPosition;

					if (gameOver) {
						wonPanel.SetActive (true);
						textWon.text = "Time needed: " + minutesElapsed.ToString ("00") + ":" + secondsElapsed.ToString ("00");
					}
				}
				else {
					// Player did not find a word match
					// so destroy the blue line
					Destroy (actualLine);
				}
			}

			// after all, 
			// draw the blue line
			drawLine ();
		}


		//
		// public void buildHintList()
		//
		// This method is called to build the hint list
		// (the list with the words the player has to find)
		//

		public void buildHintList() {
			openWords = 0;
			// clear the text label
			wordList.text = "";
			// iterate the whole list
			foreach (word w in entries) {
				// is the word in the list (to find)
				// and not found yet...
				if (w.Used && !w.Found) {
					wordList.text += w.Name+ ", ";
					openWords += 1;
				}
			}

			// the word list contains the words, separated by ","
			// here, we remove the last ","

			//wordList.text = wordList.text.Remove (wordList.text.Length - 2, 2);

			if (openWords == 0)
				gameOver = true;
		}



		//
		// public bool checkWordEnd(word actualWord, Vector3 mousePosition)
		//
		// This method is called when the player releases the mouse
		// It checks if the position the player releases belongs to a word
		// It checks if the char belongs to the same word the started with
		//

		public bool checkWordEnd(word actualWord, Vector2 matrixPosition) {
			// did the player find a word when he clicked the mouse button down?
			// if not, leave
			if (actualWord == null)
				return false;

			// check, if tile belongs to a word
			// and if tile belongs to the same word the player started with
			foreach (word w in entries) {
				if (!w.Found) {
					if (w.EndPosition == matrixPosition && w == actualWord) {
						return true;
					}
				}
			}
			return false;

		}

	

		//
		// public word checkWordBegin(Vector3 mousePosition)
		//
		// This method is called when the player presses the mouse
		// It checks if the position the player clicks belongs to a word
		// and returns the word
		//

		public word checkWordBegin(Vector2 matrixPosition)  {
			// check, if tile belongs to a word
			foreach (word w in entries) {
				if (!w.Found && w.Used) {
					if (w.StartPosition == matrixPosition) {
						return w;
					}
				}
			}
			return null;
		}
			

		//
		// public void fillList()
		//
		// This method fills the list with the puzzle words
		//

		public void fillList() {
			entries.Clear();

			// establish connection to wordsearch database
			ds = new DataService ("wordsearch.db");
			
			// get the list of existing categories (aka table categories)
			var puzzleItems = ds.getItems (PlayerPrefs.GetInt("Level"), PlayerPrefs.GetInt("Category"));

			foreach (puzzleItem pi in puzzleItems) {
				entries.Add (new word(pi.ItemName));
			}

		}


		public int[] generateDirection() {
			int[] direction = new int[2];
			
			while (direction[0] == 0 && direction[1]== 0) {
				direction[0] = Random.Range(-1,2);
				direction[1] = Random.Range(-1,2);
			}
			return direction;
		}
		
		
		
		public int[] generatePosition(string entry, int[] direction) {
			int[] position = new int[2];
			
			// now, depending on the vector, determine the
			// word's start position
			if (direction[0] == 1)
				position[0] = Random.Range(0, matrixSizeX-entry.Length);
			if (direction[1] == 1)
				position[1] = Random.Range(0, matrixSizeY-entry.Length);
			if (direction[0] == -1)
				position[0] = Random.Range(entry.Length-1, matrixSizeX);
			if (direction[1] == -1)
				position[1] = Random.Range(entry.Length-1, matrixSizeY);
			return position;
		}
		
		
		
		public bool checkWord(string entry, int[] position, int[] direction) {
			
			int xPos = position[0];
			int yPos = position[1];
			for (int chars = 0; chars < entry.ToString().Length; chars ++) {
				if (xPos < 0 || xPos > matrixSizeX-1 || yPos < 0 || yPos > matrixSizeY-1)
					return false;
				if (matrix[yPos, xPos] != "" && !string.Equals(matrix[yPos, xPos].ToString().ToUpper(),entry[chars].ToString().ToUpper())) {
					return false;
				}
				xPos += direction[0];
				yPos += direction[1];
			}
			return true;
		}
		
		
		
		public void placeWord(string entry, int[] position, int[] direction) {
			
			int xPos = position[0];
			int yPos = position[1];
			for (int chars = 0; chars < entry.Length; chars ++) {
				matrix[yPos, xPos] = entry[chars].ToString().ToUpper();
				xPos += direction[0];
				yPos += direction[1];
			}
		}

		public void fillRandomGrid() {
			for (int y = 0; y < matrixSizeY; y++) {
				for (int x = 0; x < matrixSizeX; x++) {
					if (matrix[y,x] == "")
						matrix[y,x] = System.Convert.ToChar(Random.Range(65,91)).ToString();
				}
			}
		}

		public void fillGrid() {
			for (int y = 0; y < matrixSizeY; y++) {
				for (int x = 0; x < matrixSizeX; x++) {
					matrix[y,x] = "";
				}
			}
		}

		public void drawLine() {
			if (isDown) {
				if (!isDrawing1) {
					actualLine = Instantiate (imgLine, new Vector3 (startPosition.x, startPosition.y, 0), Quaternion.identity) as GameObject;
					actualLine.transform.SetParent (GameObject.Find ("lines").transform);
					actualLine.transform.localPosition = new Vector3 (startPosition.x, startPosition.y, 0);
					actualLine.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1.5f, 1);
					actualLine.tag = "imgLine";
					isDrawing1 = true;
				}
			
				Vector2 length = (endPosition-startPosition);
				float angle = Mathf.Atan2(length.y,length.x) * Mathf.Rad2Deg;
				angle = (angle+360)%360;

				bool drawit = false;


				if (angle > 355 && angle < 5) {
					angle = 0;
				}
				if (angle > 40 && angle < 50) {
					angle = 45;
				}
				if (angle > 85 && angle < 95) {
					angle = 90;
				}
				if (angle > 130 && angle < 140) {
					angle = 135;
				}
				if (angle > 175 && angle < 185) {
					angle = 180;
				}
				if (angle > 220 && angle < 230) {
					angle = 225;
				}
				if (angle > 365 && angle < 275) {
					angle = 270;
				}
				if (angle > 310 && angle < 320) {
					angle = 315;
				}

				if (angle % 45 == 0) {
					drawit = true;
				}

				if (drawit) {
					actualLine.GetComponent<RectTransform>().sizeDelta = new Vector2(length.magnitude,32);
					actualLine.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0,0,angle));
				}

			}

			if (!isDown) {
				isDrawing1 = false;
			}
		}


		// Event Handling

		// 
		// public void menuClicked()
		//
		// This method is clled if player clicks the menu bar
		//

		public void moveMenuPanel(Vector2 position) {
			menuPanel.GetComponent<RectTransform> ().anchoredPosition = position;
		}


		public void OnMenuClicked() {
			if (menuVisible == false) {
				menuVisible = true;
				easyEasing.Vector2To (menuPanel,
					easyEasing.Params ("easeType", "easeOutQuad",
						"from", (menuPosition),
						"to", (menuPosition + new Vector2(menuPanel.GetComponent<RectTransform>().sizeDelta.x,0)),
						"duration", 0.5f,
						"onUpdateTarget", this.gameObject,
						"onUpdate", "moveMenuPanel"));
			} 
			else {
				menuVisible = false;
				easyEasing.Vector2To (menuPanel,
					easyEasing.Params ("easeType", "easeOutQuad",
						"from", (menuPosition + new Vector2(menuPanel.GetComponent<RectTransform>().sizeDelta.x,0)),
						"to", (menuPosition),
						"duration", 0.5f,
						"onUpdateTarget", this.gameObject,
						"onUpdate", "moveMenuPanel"));		
			}
		}

		// the menu pops up

		//
		// public void mainMenuClicked()
		//
		// Player clicks the entry "main menu"
		// Hide the popup menu again
		// and show dialog window
		//

		public void mainMenuClicked() {
			mainMenuPanel.gameObject.SetActive (true);
			menuVisible = false;
			easyEasing.Vector2To (menuPanel,
				easyEasing.Params ("easeType", "easeOutQuad",
					"from", (menuPosition + new Vector2(menuPanel.GetComponent<RectTransform>().sizeDelta.x,0)),
					"to", (menuPosition),
					"duration", 0.5f,
					"onUpdateTarget", this.gameObject,
					"onUpdate", "moveMenuPanel"));		
		}

		//
		// public void mainMenuCancelClicked()
		//
		// Player clicked the cancel button in the dialog window
		// so simply hide the dialog again
		//

		public void mainMenuCancelClicked() {
			mainMenuPanel.gameObject.SetActive (false);
		}

		//
		// public void mainMenuOKClicked()
		//
		// Player clicked the OK button in the dialog window
		// Hide the 

		public void mainMenuOKClicked() {
			mainMenuPanel.gameObject.SetActive (false);
			wonPanel.SetActive (false);
			countdownPanel.SetActive (false);
			SceneManager.LoadScene("menu");
		}
			

		//
		// public void restartGameClicked()
		//
		// The player wants to restart the game
		//

		public void restartGameClicked() {
			newGamePanel.gameObject.SetActive (true);
			menuVisible = false;
			easyEasing.Vector2To (menuPanel,
				easyEasing.Params ("easeType", "easeOutQuad",
					"from", (menuPanel.GetComponent<RectTransform> ().anchoredPosition),
					"to", (menuPanel.GetComponent<RectTransform> ().anchoredPosition - new Vector2(menuPanel.GetComponent<RectTransform>().sizeDelta.x,0)),
					"duration", 0.5f,
					"onUpdateTarget", this.gameObject,
					"onUpdate", "moveMenuPanel"));		
		}

		//
		// public void newGameCancelClicked()
		//
		// Player clicked the cancel button in the dialog window
		// so simply hide the dialog again
		//
		
		public void newGameCancelClicked() {
			newGamePanel.gameObject.SetActive (false);
		}
		
		//
		// public void newGameOKClicked()
		//
		// Player clicked the OK button in the dialog window
		// Hide the 
		
		public void newGameOKClicked() {
			newGamePanel.gameObject.SetActive (false);
			initGame ();
		}

		public void OnRestartClicked() {
			wonPanel.SetActive (false);
			countdownPanel.SetActive (false);
			initGame ();
		}


		//
		// public void pauseClicked()
		//
		// The player enabled/disabled the pause button
		
		public void pauseClicked() {
			
		}

		//
		// public void gridClicked()
		//
		// The player enabled/disabled the grid button
		
		public void gridClicked() {
			//GameObject[] grid = GameObject.FindGameObjectsWithTag("puzzleGrid");
			gridMatrix.SetActive(gridToggle.isOn);
		}

		//
		// public void nightClicked()
		//
		// The player enabled/disabled the night mode button
		
		public void nightClicked() {
			if (nightToggle.isOn == true) {
				backgroundImage.GetComponent<Image> ().color = Color.black;
				GameObject[] chars = GameObject.FindGameObjectsWithTag("puzzleChar");
				foreach (GameObject c in chars) {
					c.GetComponent<Text>().color = Color.white;
				}
				gridMatrix.SetActive(true);
				GameObject[] grid = GameObject.FindGameObjectsWithTag("puzzleGrid");
				foreach (GameObject g in grid) {
					g.GetComponent<Image>().sprite = whiteGrid;
				}
				gridMatrix.SetActive(gridToggle.isOn);
				wordList.color = Color.white;
			}
			else {
				backgroundImage.GetComponent<Image> ().color = Color.white;
				GameObject[] chars = GameObject.FindGameObjectsWithTag("puzzleChar");
				foreach (GameObject c in chars) {
					c.GetComponent<Text>().color = Color.black;
				}
				gridMatrix.SetActive(true);
				GameObject[] grid = GameObject.FindGameObjectsWithTag("puzzleGrid");
				foreach (GameObject g in grid) {
					g.GetComponent<Image>().sprite = blackGrid;
				}
				gridMatrix.SetActive(gridToggle.isOn);
				wordList.color = Color.black;
			}
		}

		public void PointerDown(Vector2 screenPosition, Vector2 matrix) {
			//Debug.Log ("gameScript: PointerDown " + screenPosition);
			startPosition = screenPosition;
			endPosition = screenPosition;
			matrixStart = matrix;
			matrixEnd = matrix;
			isDown = true;
		}

		public void PointerUp() {
			//Debug.Log ("gameScript: PointerUp");
			isDown = false;
		}

		public void PointerEnter(Vector2 screenPosition, Vector2 matrix) {
			if (isDown) {
				endPosition = screenPosition;
				matrixEnd = matrix;
			}
			//Debug.Log ("gameScript: PointerEnter"  + screenPosition);
		}

		public void PointerExit() {
			//Debug.Log ("gameScript: PointerExit");
		}

	}
}
