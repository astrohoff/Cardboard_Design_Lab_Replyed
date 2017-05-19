using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for fading in and out the Splash Scene splash panels.
public class SplashFader : MonoBehaviour {
	// Angle at which the UI panel switches frome fading in to fading out.
	// Angle measured between player head forward and global down.
	private const float DownAngleThresh = 35;
	private const float FadeDurration = 1.0f;
	private const float FadeInPauseDurration = 0.5f;

	public Transform playerHead;
	// UI panel components.
	public Graphic[] uiComponents;

	// Alpha component of UI colors.
	private float alpha = 0;
	private float timeSinceLookup = 0;
	private bool oldLookDownState = true;

	private void Update () {
		// Calculate magnitude of change in transparency for this frame.
		float fadeChange = Time.deltaTime / FadeDurration;
		// Calculate look down angle.
		float lookDownAngle = Vector3.Angle (playerHead.forward, Vector3.down);
		// Make change negitive (fade out) if looking down.
		bool lookDownState = lookDownAngle < DownAngleThresh;
		if (lookDownState) {
			fadeChange *= -1;
		}
		// If looking up...
		else {
			// If alpha is zero (haven't started fading in yet) & player just looked up...
			if (alpha == 0 && oldLookDownState) {
				// Reset fade in wait time.
				timeSinceLookup = 0;
			}
			timeSinceLookup += Time.deltaTime;
		}

		// TODO: Figure out clean way to not fade in until pause period has passed.

		// Update alpha value with fadeChange, and clamp betwen 0 and 1.
		alpha = Mathf.Clamp01 (alpha + fadeChange);
		// Update UI component colors.
		for (int i = 0; i < uiComponents.Length; i++) {
			SetUIAlpha (uiComponents [i], alpha);
		}
		oldLookDownState = lookDownState;
	}

	// Sets the alpha component of a UI component's color.
	private static void SetUIAlpha(Graphic uiComponent, float alpha){
		// A color's components can't be modified individually when it is activly being used by a game component.
		// Instead, get the color, set the alpha, then reassign it to the UI component.
		Color tempColor = uiComponent.color;
		tempColor.a = alpha;
		uiComponent.color = tempColor;
	}
}
