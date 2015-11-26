using System;
using UnityEngine;
using System.Collections.Generic;
using DigitalRubyShared;

//public enum InputType
//{
//	Null,
//	Up,
//	Down, 
//	Right,
//	Left,
//	Tap
//}

public class InputAction : MonoBehaviour
{
	
	public FingersScript FingerScript;
	private TapGestureRecognizer tapGesture;
	private SwipeGestureRecognizer swipeGesture;

	public event Action OnTap;
	public event Action OnSwipe;

	void Awake()
	{
		CreateSwipeGesture();
		CreateTapGesture();
	}

	private GestureTouch FirstTouch(ICollection<GestureTouch> touches)
	{
		foreach (GestureTouch t in touches)
		{
			return t;
		}
		return new GestureTouch();
	}

	private void CreateTapGesture()
	{
		tapGesture = new TapGestureRecognizer();
		tapGesture.Updated += TapGestureCallback;
		FingerScript.AddGesture(tapGesture);
	}

	private void TapGestureCallback(GestureRecognizer gesture, ICollection<GestureTouch> touches)
	{
		if (gesture.State == GestureRecognizerState.Ended)
		{
			GestureTouch t = FirstTouch (touches);
			if (t.IsValid())
			{
				OnTap();
			}
		}
	}

	private void CreateSwipeGesture()
	{
		swipeGesture = new SwipeGestureRecognizer();
		swipeGesture.Direction = SwipeGestureRecognizerDirection.Any;
		swipeGesture.Updated += SwipeGestureCallback;
		FingerScript.AddGesture(swipeGesture);
	}

	private void SwipeGestureCallback(GestureRecognizer gesture, ICollection<GestureTouch> touches)
	{
		if (gesture.State == GestureRecognizerState.Ended)
		{
			GestureTouch t = FirstTouch (touches);
			if (t.IsValid())
			{
				OnSwipe();
			}
		}
	}


//	private static float fingerStartTime  = 0.0f;
//	private static Vector2 fingerStartPos = Vector2.zero;
//	
//	private static bool isSwipe = false;
//	private static float minSwipeDist  = 50.0f;
//	private static float maxSwipeTime = 0.5f;
//	
//	
//	
//	// Update is called once per frame
//	public static InputType GetInput () 
//	{
//		
//		if (Input.touchCount > 0){
//			
//			var touch = Input.touches[0];
//			{
//				if (touch.phase == TouchPhase.Ended && fingerStartTime < 0.2f) 
//				{
//					return InputType.Tap;
//				}
//
//				switch (touch.phase)
//				{
//				case TouchPhase.Began :
//					/* this is a new touch */
//					isSwipe = true;
//					fingerStartTime = Time.time;
//					fingerStartPos = touch.position;
//					break;
//					
//				case TouchPhase.Canceled :
//					/* The touch is being canceled */
//					isSwipe = false;
//					break;
//					
//					
//				case TouchPhase.Moved :
//					
//					float gestureTime = Time.time - fingerStartTime;
//					float gestureDist = (touch.position - fingerStartPos).magnitude;
//					
//					if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
//					{
//						Vector2 direction = touch.position - fingerStartPos;
//						Vector2 swipeType = Vector2.zero;
//						
//						if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
//							// the swipe is horizontal:
//							swipeType = Vector2.right * Mathf.Sign(direction.x);
//						}else{
//							// the swipe is vertical:
//							swipeType = Vector2.up * Mathf.Sign(direction.y);
//						}
//						
//						if(swipeType.x != 0.0f){
//							if(swipeType.x > 0.0f){
//								// MOVE RIGHT
//								
//								return InputType.Right;
//							}else{
//								// MOVE LEFT
//								return InputType.Left;
//							}
//						}
//						
//						if(swipeType.y != 0.0f ){
//							if(swipeType.y > 0.0f){
//								// MOVE UP
//								return InputType.Up;
//							}else{
//								// MOVE DOWN
//								return InputType.Down;
//							}
//						}
//						
//					}
//					
//					break;
//					
//				case TouchPhase.Ended :
//					
//					float gestureTimeEnd = Time.time - fingerStartTime;
//					float gestureDistEnd = (touch.position - fingerStartPos).magnitude;
//					
//					if (isSwipe && gestureTimeEnd < maxSwipeTime && gestureDistEnd > minSwipeDist)
//					{
//						Vector2 direction = touch.position - fingerStartPos;
//						Vector2 swipeType = Vector2.zero;
//						
//						if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
//							// the swipe is horizontal:
//							swipeType = Vector2.right * Mathf.Sign(direction.x);
//						}else{
//							// the swipe is vertical:
//							swipeType = Vector2.up * Mathf.Sign(direction.y);
//						}
//						
//						if(swipeType.x != 0.0f){
//							if(swipeType.x > 0.0f){
//								// MOVE RIGHT
//								
//								return InputType.Right;
//							}else{
//								// MOVE LEFT
//								return InputType.Left;
//							}
//						}
//						
//						if(swipeType.y != 0.0f ){
//							if(swipeType.y > 0.0f){
//								// MOVE UP
//								return InputType.Up;
//							}else{
//								// MOVE DOWN
//								return InputType.Down;
//							}
//						}
//						
//					}
//					break;
//					
//				}
//			}
//		}
//		return InputType.Null;
//	}
}

