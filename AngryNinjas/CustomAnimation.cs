using System;

using Box2D;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Dynamics.Contacts;
using Box2D.Dynamics.Joints;

using Cocos2D;

namespace AngryNinjas
{
	public class CustomAnimation : CCNode
	{
		string fileNameToAnimate;
		int currentFrame;
		int framesToAnimate;
		int frameToStartWith;
		
		CCSprite someSprite;
		
		bool animationFlippedX;
		bool animationFlippedY;
		bool doesTheAnimationLoop;
		bool useRandomFrameToLoop;

		public CustomAnimation (string theFileNameToAnimate,
		                        int theFrameToStartWith,
		                        int theNumberOfFramesToAnimate,
		                        int theX,
		                        int theY,
		                        bool flipOnX,
		                        bool flipOnY,
		                        bool doesItLoop,
		                        bool doesItUseRandomFrameToLoop)
		{
			InitWithOurOwnProperties( theFileNameToAnimate,
			                          theFrameToStartWith,
			                          theNumberOfFramesToAnimate,
			                          theX,
			                          theY,
			                          flipOnX,
			                          flipOnY,
			                          doesItLoop,
			                          doesItUseRandomFrameToLoop);
		}

		void InitWithOurOwnProperties(string theFileNameToAnimate,
		                              int theFrameToStartWith,
		                              int theNumberOfFramesToAnimate,
		                              int theX,
		                              int theY,
		                              bool flipOnX,
		                              bool flipOnY,
		                              bool doesItLoop,
		                              bool doesItUseRandomFrameToLoop)
		{
			
			this.fileNameToAnimate = theFileNameToAnimate;
			
			this.frameToStartWith = theFrameToStartWith;
			this.currentFrame = frameToStartWith;
			
			this.framesToAnimate = theNumberOfFramesToAnimate;
			
			this.animationFlippedX = flipOnX;
			this.animationFlippedY = flipOnY;
			
			this.doesTheAnimationLoop = doesItLoop;
			this.useRandomFrameToLoop = doesItUseRandomFrameToLoop;
			
			this.someSprite = new CCSprite(String.Format("{0}_000{1}.png", fileNameToAnimate, currentFrame));
			AddChild(someSprite);
			someSprite.PositionX = theX;
			someSprite.PositionY = theY;
			
			someSprite.FlipX = animationFlippedX;
			someSprite.FlipY = animationFlippedY;
			
			
			
			Schedule(RunMyAnimation, 1.0f / 60.0f);
				
		}

		void RunMyAnimation(float delta) 
		{
			
			currentFrame ++; //adds 1 to currentFrame
			
			if (currentFrame <= framesToAnimate) {
				
				//if you don't want leading zeros
				
				if (currentFrame < 10) {
					
					someSprite.Texture = new CCSprite(String.Format("{0}_000{1}.png", fileNameToAnimate, currentFrame)).Texture;
					
				} else if (currentFrame < 100) {
					
					someSprite.Texture = new CCSprite(String.Format("{0}_00{1}.png", fileNameToAnimate, currentFrame)).Texture;
				} else {
					
					someSprite.Texture = new CCSprite(String.Format("{0}_0{1}.png", fileNameToAnimate, currentFrame)).Texture;
					
				}
				
				
			} else {
				
				if ( doesTheAnimationLoop && !useRandomFrameToLoop) {
					
					currentFrame = frameToStartWith;
					
				} else if ( doesTheAnimationLoop && useRandomFrameToLoop ) {
					
					currentFrame = Cocos2D.CCRandom.Next() % framesToAnimate; // you'd get a range of 0 to whatever framesToAnimate is
					
				} else {
					RemoveChild(someSprite, false);
					RemoveFromParentAndCleanup(false);
					Unschedule(RunMyAnimation);
				}
				
			}
			
			
		}
		
		void ChangeOpacityTo(byte theNewOpacity)
		{
			
			someSprite.Opacity = theNewOpacity;  //range of 0 to 255
			
		}

		void TintMe(CCColor3B theColor)
		{
			
			someSprite.Color = theColor;
			
		}
		


	}
}

