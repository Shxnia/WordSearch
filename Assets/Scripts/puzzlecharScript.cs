using UnityEngine;
using System.Collections;

namespace GemMine.WordSearch {

	public class puzzlecharScript : MonoBehaviour {

		Vector2 matrix;
		Vector2 screenPosition;

		public void setMatrixPosition(int y, int x) {
			matrix = new Vector2 (x, y);
		}

		public void setScreenPosition(float x, float y) {
			screenPosition = new Vector3 (x, y);
		}

		public Vector2 getMatrixPosition() {
			return matrix;
		}

		public Vector3 getScreenPosition() {
			return screenPosition;
		}


		public void PointerEnter() {
			GameObject.Find ("GameManager").GetComponent<gameScript> ().PointerEnter (screenPosition,matrix);	
		}

		public void PointerExit() {
			GameObject.Find ("GameManager").GetComponent<gameScript> ().PointerExit ();	
		}

		public void PointerDown() {
			GameObject.Find ("GameManager").GetComponent<gameScript> ().PointerDown (screenPosition, matrix);	
		}

		public void PointerUp() {
			GameObject.Find ("GameManager").GetComponent<gameScript> ().PointerUp ();	
		}
	}
}