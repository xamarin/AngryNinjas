using System;
using Microsoft.Xna.Framework;
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
	public class TheLevel : CCLayer
	{
		internal bool IS_IPAD;
		internal bool IS_IPHONE;
        internal bool IS_RETINA;

		static TheLevel layerInstance;

		int screenWidth;
		int screenHeight;
		
		int currentLevel;
		int pointTotalThisRound;
		int pointsToPassLevel;
		int bonusThisRound;
		int bonusPerExtraNinja;
		
		float fontSizeForScore;
		
		float panAmount;
		int initialPanAmount;
		int extraAmountOnPanBack;
		int maxStretchOfSlingShot;
		
		bool openWithMenuInsteadOfGame;
		
		bool topRightTouchEnablesDebugMode;
		
		bool stackIsNowDynamic;
		
		bool areWeOnTheIPad;
		
		bool useImagesForPointScoreLabels;
		
		bool somethingJustScored;
		bool dottingOn;
		bool areWeInTheStartingPosition;
		bool slingShotNinjaInHand;  //if the ninja is in the sling
		bool throwInProgress;  //if the ninja is currently being thrown (in midair / midturn basically)
		bool autoPanningInProgress;
		bool reverseHowFingerPansScreen; //reverses which direction the screen moves based on the finger moving left or right
		bool panningTowardSling;
		bool continuePanningScreenOnFingerRelease;
		bool autoReverseOn;
		
		float multipyThrowPower; 
		
		float yAxisGravity; 
		bool gravityOn;

		
		b2World world;
		ContactListener contactListener;

		//the entire stack
		
		TheStack stack;


		//background art
		
		CCSprite backgroundLayerClouds;
		CCSprite backgroundLayerHills;
		CCParticleSystem system;
		
		
		CCPoint cloudLayerStartPosition;
		CCPoint hillsLayerStartPosition;
		CCPoint particleSystemStartPosition;
		CCPoint labelStartingPoint;



		//sling art
		
		CCSprite slingShotFront;
		
		CCSprite strapFront;
		CCSprite strapBack;
		CCSprite strapEmpty;

		//BOX2D object start positions  (will probably vary between iPAd and iPhone
		
		CCPoint groundPlaneStartPosition;
		CCPoint platformStartPosition;
		CCPoint ninjaStartPosition1;
		CCPoint ninjaStartPosition2;
		CCPoint ninjaStartPosition3;
		CCPoint ninjaStartPosition4;
		CCPoint ninjaStartPosition5;

		// used for moving the world to see the targets..
		float worldMaxHorizontalShift;
		float previousTouchLocationX;
		
		float adjustY;
		float maxScaleDownValue;
		float scaleAmount;
		float speed;
		
		CCLabelTTF highScoreLabel;
		CCLabelTTF currentScoreLabel;
		CCPoint currentScoreLabelStartPosition;
		CCPoint highScoreLabelStartPosition;
		CCPoint menuStartPosition;
		
		CCPoint slingShotCenterPosition;
		CCPoint positionInSling;


		//white dots
		
		int dotCount;
		int throwCount;
		int dotTotalOnEvenNumberedTurn;
		int dotTotalOnOddNumberedTurn;

		//menu button
		
		CCMenu MenuButton;

		//ground and start platform
		GroundPlane theGroundPlane;
		StartPlatform thePlatform;

		// current character being thrown
		
		Ninja currentBodyNode;
		Ninja ninja1;
		// ninjas in line to be thrown
		
		Ninja ninja2;
		Ninja ninja3;
		Ninja ninja4;
		Ninja ninja5;
		
		int ninjasToTossThisLevel;
		int ninjaBeingThrown;

		public TheLevel ()
		{
            layerInstance = this;

			//INITIAL VARIABLES.... (D O N T    E D I T )
			
			// enable touches
#if XBOX || OUYA
            TouchEnabled = false;
            GamePadEnabled = true;
#else
			TouchEnabled = true;
#endif
			
			
			// enable accelerometer
			AccelerometerEnabled = false;

#if iPHONE || iOS
			IS_IPAD = MonoTouch.UIKit.UIDevice.CurrentDevice.UserInterfaceIdiom 
				== MonoTouch.UIKit.UIUserInterfaceIdiom.Pad;

			IS_RETINA = MonoTouch.UIKit.UIScreen.MainScreen.Bounds.Height 
				* MonoTouch.UIKit.UIScreen.MainScreen.Scale >= 1136;
#endif
#if WINDOWS || MACOS || MONOMAC || LINUX || OUYA || XBOX
            IS_IPAD = true;
            IS_RETINA = true;
#endif

			IS_IPHONE = !IS_IPAD;

			// ask director for the window size
			var screenSize = CCDirector.SharedDirector.WinSize;
			screenWidth = (int)screenSize.Width;
			screenHeight = (int)screenSize.Height;

			throwInProgress = false; //is a throw currently in progress, as in, is a ninja in midair (mostly used to prevent tossing two ninjas, one right after another)
			areWeInTheStartingPosition = true;  //is the world back at 0 on the X axis (if yes, then we can put a ninja in the sling)
			
			throwCount = 0;
			dotTotalOnOddNumberedTurn = 0;
			dotTotalOnEvenNumberedTurn = 0;

			currentLevel = GameData.SharedData.Level;  // use currentLevel =  0 for testing new shapes, will call [self buildLevelWithAllShapes];
			
			pointTotalThisRound = 0;
			pointsToPassLevel = GameData.SharedData.PointsToPassLevel;
			bonusPerExtraNinja = GameData.SharedData.BonusPerExtraNinja;
			
			CCLog.Log(string.Format("The level is {0}, you need {1} to move up a level", currentLevel, pointsToPassLevel ));

			//PREFERENCE VARIABLES....
			
			openWithMenuInsteadOfGame = false; // start with the menu opening the game
			
			continuePanningScreenOnFingerRelease = true; // if the screen panning is midway between either the sling or targets, when you release your finger the screen will continue panning the last direction it moved (jumpy on iPhone if set to NO)
			reverseHowFingerPansScreen = false; //switch to yes to reverse. 
			topRightTouchEnablesDebugMode = true;  //SET TO NO IN FINAL BUILD
			useImagesForPointScoreLabels = true; //IF NO, means you use Marker Felt text for scores

			//set up background art
			
			backgroundLayerClouds = new CCSprite (GameData.SharedData.BackgroundCloudsFileName);  // will return the background clouds file for a particular level
			AddChild (backgroundLayerClouds, Constants.DepthClouds);

			backgroundLayerHills = new CCSprite (GameData.SharedData.BackgroundHillsFileName);  // will return the background hills file for a particular level
			AddChild (backgroundLayerHills, Constants.DepthHills);
			backgroundLayerHills.ScaleX = 1.05f;

			slingShotFront = new CCSprite ("slingshot_front.png"); 
			AddChild (slingShotFront, Constants.DepthSlingShotFront);

			strapFront = new CCSprite ("strap.png"); 
			AddChild (strapFront, Constants.DepthStrapFront);


			strapBack = new CCSprite ("strapBack.png");
			AddChild (strapBack, Constants.DepthStrapBack);

			strapEmpty = new CCSprite ("strapEmpty.png");
			AddChild (strapEmpty, Constants.DepthStrapBack);


			strapBack.Visible = false;  //visible only when stretching
			strapFront.Visible = false; //visible only when stretching



			//setup positions and variables for iPad devices or iPhones
			
			if (IS_IPAD) {
				areWeOnTheIPad = true;
				
				//vars 
				
				maxStretchOfSlingShot = 75; //best to leave as is, since this value ties in closely to the image size of strap.png. (should be 1/4 the size of the source image)
				multipyThrowPower = 1; // fine tune how powerful the sling shot is. Range is probably best between .5 to 1.5, currently for the iPad 1.0 is good
				
				worldMaxHorizontalShift = -(screenWidth);  // This determines how far the user can slide left or right to see the entire board. Always a negative number. 
				maxScaleDownValue = 1.0f; //dont change
				scaleAmount = 0; // increment to change the scale of the entire world when panning  
				initialPanAmount = 30; //how fast the screen pan starts
				extraAmountOnPanBack = 10; // I like a faster pan back. Adding a bit more
				adjustY = 0; // best to leave at 0 for iPad (moves the world down when panning)
				
				//background stuff
				
				backgroundLayerClouds.Position = new CCPoint(screenWidth, screenHeight / 2 );
				backgroundLayerHills.Position = new CCPoint(screenWidth, screenHeight / 2 );
				
				if( !IS_RETINA) {


					//non retina adjustment


				} else {

					//retina adjustment
					
					backgroundLayerClouds.Scale = 2.0f;
					backgroundLayerHills.Scale = 2.0f;
				}
				
				
				menuStartPosition = new CCPoint( 130 , screenSize.Height-24);
				currentScoreLabelStartPosition = new CCPoint( 200 , screenSize.Height-60);
				highScoreLabelStartPosition = new CCPoint( 200 , screenSize.Height-80);
				
				fontSizeForScore = 22;
				
				//ground plane and platform
				
				groundPlaneStartPosition = new CCPoint(screenWidth, 50 );
				platformStartPosition = new CCPoint(340,  190 );
				
				//sling shot
				slingShotCenterPosition = new CCPoint(370,  255 );
				
				slingShotFront.Position = new CCPoint(374, 240 );
				strapFront.Position = new CCPoint(slingShotCenterPosition.X, slingShotCenterPosition.Y  );
				strapBack.Position = new CCPoint(slingShotCenterPosition.X + 33 , slingShotCenterPosition.Y - 10 );
				strapEmpty.Position = new CCPoint(378, 235 );
				
				//ninja
				
				ninjaStartPosition1 = new CCPoint(380,  250 );  
				ninjaStartPosition2 = new CCPoint(300,  155 );
				ninjaStartPosition3 = new CCPoint(260,  155 );
				ninjaStartPosition4 = new CCPoint(200,  120 );
				ninjaStartPosition5 = new CCPoint(160,  120 );
				
			}
			
			else if (IS_IPHONE) {
				//CCLOG (@"this is an iphone");
				
				areWeOnTheIPad = false ;
				
				//vars 
				maxStretchOfSlingShot = 75; //best to leave as is, since this value ties in closely to the image size of strap.png. (should be 1/4 the size of the source image)
				multipyThrowPower = 1.0f; // fine tune how powerful the sling shot is. Range is probably best between .5 to 1.5, and a little goes a long way
				
				worldMaxHorizontalShift = -(screenWidth); // This determines how far the user can slide left or right to see the entire board. Always a negative number
				maxScaleDownValue = 0.65f; //range should probably be between 0.75 and 1.0;
				scaleAmount = .01f; // increment to change the scale of the entire world when panning
				adjustY = -34;
				
				initialPanAmount = 20; //how fast the screen pan starts
				extraAmountOnPanBack = 0; // best to leave at 0 on iPhone
				
				//background stuff
				
				
				
				if( !IS_RETINA ) {

					//non retina adjustment
					
					backgroundLayerClouds.Position = new CCPoint(screenWidth, 192 );
					backgroundLayerClouds.Scale = .7f;
					backgroundLayerHills.Position = new CCPoint(screenWidth, 245  );
					backgroundLayerHills.Scale = .7f;
					
				} else {
					
					//retina adjustment
					
					backgroundLayerClouds.Position = new CCPoint(screenWidth, 192 );
					backgroundLayerClouds.Scale = 1.7f;
					backgroundLayerHills.Position = new CCPoint(screenWidth, 265  );
					backgroundLayerHills.Scale = 1.7f;
				}
				
				
				
				menuStartPosition = new CCPoint( 70 , screenSize.Height-17); 
				currentScoreLabelStartPosition = new CCPoint( 140 , screenSize.Height-50); //score label
				highScoreLabelStartPosition = new CCPoint( 140 , screenSize.Height - 70 );
				fontSizeForScore =  18;
				
				//ground plane and platform
				
				groundPlaneStartPosition = new CCPoint(screenWidth, -25 );
				platformStartPosition = new CCPoint(130,  120 );
				
				//sling shot
				
				slingShotCenterPosition = new CCPoint(160,  185 );
				slingShotFront.Position = new CCPoint(164, 170 );
				strapFront.Position = new CCPoint(slingShotCenterPosition.X, slingShotCenterPosition.Y  );
				strapBack.Position = new CCPoint(slingShotCenterPosition.X + 33 , slingShotCenterPosition.Y - 10 );
				strapEmpty.Position = new CCPoint(168, 163 );
				
				//ninja
				
				ninjaStartPosition1 = new CCPoint(170,  175 );
				ninjaStartPosition2 = new CCPoint(110,  82 );
				ninjaStartPosition3 = new CCPoint(65,  82 );
				ninjaStartPosition4 = new CCPoint(90,  65 );
				ninjaStartPosition5 = new CCPoint(43,  65 );
				
				
				
			}

			SetUpParticleSystemSun();

			CCMenuItemImage button1 = new CCMenuItemImage ("gameMenu.png", "gameMenu.png", ShowMenu);

			MenuButton = new CCMenu (button1);
			MenuButton.Position = menuStartPosition;
			AddChild (MenuButton, Constants.DepthScore);
			
			
			
			// assign CCPoints to keep track of the starting positions of objects that move relative to the entire layer.
			
			hillsLayerStartPosition = backgroundLayerHills.Position;
			cloudLayerStartPosition = backgroundLayerClouds.Position; 
			
			// Define the gravity vector.

            yAxisGravity = -9.81f;

			b2Vec2 gravity = b2Vec2.Zero;
			gravity.Set(0.0f, yAxisGravity);

			
			// Construct a world object, which will hold and simulate the rigid bodies.
			world = new b2World(gravity);

			world.AllowSleep = false;

			world.SetContinuousPhysics (true);

			//EnableDebugMode();
			
			
			// Define the ground body.
			var groundBodyDef = new b2BodyDef();  // Make sure we call 
			groundBodyDef.position.Set(0, 0); // bottom-left corner
			
			// Call the body factory which allocates memory for the ground body
			// from a pool and creates the ground box shape (also from a pool).
			// The body is also added to the world.
			b2Body groundBody = world.CreateBody(groundBodyDef);

			// Define the ground box shape.
			b2EdgeShape groundBox = new b2EdgeShape();

			int worldMaxWidth = screenWidth * 4; //If you ever want the BOX2D world width to be more than it is then increase this  (currently, this should be plenty of extra space)
			int worldMaxHeight = screenHeight * 3; //If you ever want the BOX2D world height to be more  than it is then increase this (currently, this should be plenty of extra space)

			// bottom
			groundBox.Set(new b2Vec2(-4,0), new b2Vec2( worldMaxWidth /Constants.PTM_RATIO,0));
			groundBody.CreateFixture(groundBox,0);

			// top
			groundBox.Set(new b2Vec2(-4,worldMaxHeight/Constants.PTM_RATIO), new b2Vec2( worldMaxWidth /Constants.PTM_RATIO, worldMaxHeight /Constants.PTM_RATIO));
			groundBody.CreateFixture(groundBox,0);

			// left
			groundBox.Set(new b2Vec2(-4,worldMaxHeight/Constants.PTM_RATIO), new b2Vec2(-4,0));
			groundBody.CreateFixture(groundBox,0);

			// right
			groundBox.Set(new b2Vec2( worldMaxWidth /Constants.PTM_RATIO,worldMaxHeight/Constants.PTM_RATIO), new b2Vec2(worldMaxWidth /Constants.PTM_RATIO,0));
			groundBody.CreateFixture(groundBox,0);

			//Contact listener 
			contactListener = new ContactListener();
			world.SetContactListener(contactListener);
			
			//Set up the ground plane
			
			theGroundPlane = new GroundPlane (world, groundPlaneStartPosition, GameData.SharedData.GroundPlaneFileName);
			AddChild(theGroundPlane, Constants.DepthFloor);

			
			//Set up the starting platform
			
			thePlatform = new StartPlatform(world, platformStartPosition, "platform.png"); 
			AddChild(thePlatform, Constants.DepthPlatform);

			//Set up ninjas
			
			ninjaBeingThrown = 1; //always starts at 1 (first ninja, then second ninja, and so on) 
			ninjasToTossThisLevel = GameData.SharedData.NumberOfNinjasToTossThisLevel;  //total number of ninjas to toss for this level
			
			
			
			ninja1 = new Ninja( world, ninjaStartPosition1, @"ninja");
			AddChild(ninja1, Constants.DepthNinjas);
			
			currentBodyNode = ninja1;

			currentBodyNode.SpriteInSlingState();

			if ( ninjasToTossThisLevel >= 2) {
				
				ninja2 = new Ninja( world, ninjaStartPosition2, @"ninjaRed");
				AddChild(ninja2, Constants.DepthNinjas);
				ninja2.SpriteInStandingState();

			}
			if ( ninjasToTossThisLevel >= 3) {
				
				ninja3 = new Ninja( world, ninjaStartPosition3, @"ninjaBlue");
				AddChild(ninja3, Constants.DepthNinjas);
				ninja3.SpriteInStandingState();
			}
			if ( ninjasToTossThisLevel >= 4) {
				
				ninja4 = new Ninja( world, ninjaStartPosition4, @"ninjaBrown");
				AddChild(ninja4, Constants.DepthNinjas);
				ninja4.SpriteInStandingState();
			}
			if ( ninjasToTossThisLevel >= 5) {
				
				ninja5 = new Ninja( world, ninjaStartPosition5, @"ninjaGreen");
				AddChild(ninja5, Constants.DepthNinjas);
				ninja5.SpriteInStandingState();
			}

			//Build the Stack. 
			
			stack = new TheStack(world);
			AddChild(stack, Constants.DepthStack);
			
			
			//give the stack a moment to drop, then switches every pieces to static (locks it into position, until the first slingshot)...
			ScheduleOnce(SwitchAllStackObjectsToStatic, 1.0f);

			currentScoreLabel = new CCLabelTTF(String.Format("{0}: Needed", pointsToPassLevel), "MarkerFelt", fontSizeForScore);
			AddChild(currentScoreLabel, Constants.DepthScore);
			currentScoreLabel.Color = new CCColor3B(255,255,255);
			currentScoreLabel.Position = currentScoreLabelStartPosition;
			currentScoreLabel.AnchorPoint = new CCPoint( 1, .5f);
			// HighScoreForLevel
			highScoreLabel = new CCLabelTTF(String.Format("High Score: {0}", GameData.SharedData.HighScoreForLevel), "MarkerFelt", fontSizeForScore);
			AddChild(highScoreLabel, Constants.DepthScore);
			highScoreLabel.Color = new CCColor3B(255,255,255);

            highScoreLabel.Position = currentScoreLabel.Position - new CCPoint(0, highScoreLabel.ContentSize.Height);// highScoreLabelStartPosition;
			highScoreLabel.AnchorPoint = new CCPoint( 1, .5f);
            highScoreLabelStartPosition = highScoreLabel.Position;


			var levelString = string.Format("Level: {0}", currentLevel );
			ShowBoardMessage(levelString);
			
			CCLog.Log(" ");
			CCLog.Log(" ");
			CCLog.Log(" ");
			CCLog.Log("/////////////////////////////////////////////////////");
			CCLog.Log("/////////////////////////////////////////////////////");
			CCLog.Log(" ");
			CCLog.Log(" ");
			CCLog.Log(" ");
			CCLog.Log("The art and animation in this template is copyright CartoonSmart LLC");
			CCLog.Log("You must make significant changes to the art before submitting your game to the App Store");
			CCLog.Log("Please create your own characters, backgrounds, etc and spend the time to make the game look totally unique");
			CCLog.Log(" ");
			CCLog.Log(" ");
			CCLog.Log(" ");
			CCLog.Log("The Video guide for this template is at https://vimeo.com/cartoonsmart/angryninjasguide  ");
			CCLog.Log(" ");
			CCLog.Log(" ");
			CCLog.Log(" ");
			CCLog.Log("/////////////////////////////////////////////////////");
			CCLog.Log("/////////////////////////////////////////////////////");
			CCLog.Log(" ");
			CCLog.Log(" ");
			
			


			GameSounds.SharedGameSounds.IntroTag();
			
			if ( GameData.SharedData.Level == 1 ) {
				
				GameSounds.SharedGameSounds.PlayBackgroundMusic(AmbientFXSounds.Frogs);
				
			} else {
				
				GameSounds.SharedGameSounds.PlayBackgroundMusic(AmbientFXSounds.Insects); 
			}

            if (GameData.SharedData.FirstRunEver && openWithMenuInsteadOfGame)
            {
                CCLog.Log("First run ever");
                Schedule(ShowMenuFromSelector, 2f);
                GameData.SharedData.FirstRunEver = false;
            }

            // Always do this last.
            this.Schedule(Tick);
        }

        #region GAME PAD SUPPORT

        private float _StickPull = 0f;
        private CCPoint _StickDir = CCPoint.Zero;
        private bool _SlingshotIsCocked = false;

        protected override void OnGamePadStickUpdate(CCGameStickStatus leftStick, CCGameStickStatus rightStick, PlayerIndex player)
        {
            if (player == PlayerIndex.One)
            {
                _StickPull = leftStick.Magnitude;
                _StickDir = leftStick.Direction;
                // Do the rubber band
                CCPoint pt = CCPoint.Zero;
                pt.X = slingShotCenterPosition.X + _StickDir.X * _StickPull * maxStretchOfSlingShot;
                pt.Y = slingShotCenterPosition.Y + _StickDir.Y * _StickPull * maxStretchOfSlingShot;
                if (CockTheSlingshot(pt))
                {
                    _SlingshotIsCocked = true;
                }
                else
                {
                    if (rightStick.Magnitude > 0f)
                    {
                        PanTheScreen((int)(rightStick.Magnitude * 25));
                    }
                }
            }
            else
            {
                // base.OnGamePadStickUpdate(leftStick, rightStick, player);
            }
        }

        private bool _ADown = false;
        protected override void OnGamePadButtonUpdate(CCGamePadButtonStatus backButton, CCGamePadButtonStatus startButton, CCGamePadButtonStatus systemButton, CCGamePadButtonStatus aButton, CCGamePadButtonStatus bButton, CCGamePadButtonStatus xButton, CCGamePadButtonStatus yButton, CCGamePadButtonStatus leftShoulder, CCGamePadButtonStatus rightShoulder, PlayerIndex player)
        {
            if (player == PlayerIndex.One)
            {
                if (aButton == CCGamePadButtonStatus.Pressed)
                {
                    _ADown = true;
                }
                else if (aButton == CCGamePadButtonStatus.Released && _ADown)
                {
                    if (_SlingshotIsCocked && slingShotNinjaInHand)
                    {
                        // Inver the stick direction because this is the launch direction of the ninja
                        b2Vec2 bv = b2Vec2.Zero;
                        bv.x = -_StickDir.X;
                        bv.y = -_StickDir.Y;
                        FireSlignshot(bv);
                    }
                    _SlingshotIsCocked = false;
                    _ADown = false;
                }
            }
            else if (bButton == CCGamePadButtonStatus.Pressed)
            {
            }
            else
            {
                base.OnGamePadButtonUpdate(backButton, startButton, systemButton, aButton, bButton, xButton, yButton, leftShoulder, rightShoulder, player);
            }
        }

        #endregion

        public static TheLevel SharedLevel
		{
			get {
				return layerInstance;
			}
		}

        void ShowMenuFromSelector(float dt)
        {
            ShowMenu(null);
        }
		void ShowMenu (object sender)
		{
			CCLog.Log ("Show Menu");
			CCDirector.SharedDirector.PushScene(TheMenu.Scene);
		}

		void SwitchAllStackObjectsToStatic(float delay)
		{
			
			stackIsNowDynamic = false;
			
			//Iterate over the bodies in the physics world
			for (var b = world.BodyList; b != null; b = b.Next)
			{
				if (b.UserData != null) {
					
					StackObject myActor = b.UserData as StackObject;
					
					if ( myActor != null ) 
					{
						
						myActor.Body.SetType(b2BodyType.b2_staticBody);
						
					}
					
				}	
			}
			
		}
		
		
		void SwitchAllStackObjectsToDynamic(float delay)
		{
			
			
			if ( !stackIsNowDynamic ) {
				
				stackIsNowDynamic = true;
				
				//Iterate over the bodies in the physics world
				for (var b = world.BodyList; b != null; b = b.Next)
				{
					if (b.UserData != null) {
						
						StackObject myActor = b.UserData as StackObject;
						if ( myActor != null ) 
						{
							
							if ( !myActor.IsStatic  ) {
								myActor.Body.SetType(b2BodyType.b2_dynamicBody);
								myActor.Body.SetAwake(true);
								//myActor.Body.SetActive(true);

							} 
							
						}
					}	
				}
				
				
			}
			
		}

		void EnableDebugMode()
		{
			CCBox2dDraw debugDraw = new CCBox2dDraw("fonts/arial-12");
			world.SetDebugDraw(debugDraw);
			debugDraw.AppendFlags(b2DrawFlags.e_shapeBit);

		}

		public override void Draw()
		{
			//
			// IMPORTANT:
			// This is only for debug purposes
			// It is recommend to disable it
			//
			base.Draw();
			
			CCDrawingPrimitives.Begin();
			world.DrawDebugData();
			CCDrawingPrimitives.End();
			
		}

		void SetUpParticleSystemSun()
		{
			
			//recommended for iPad only
			
			system = new CCParticleSun ();
			// AddChild(system, Constants.depthParticles);
			system.Scale = 3;
			system.Position = new CCPoint(240, 400 );
			
			particleSystemStartPosition = system.Position;
		}

		void Tick(float dt)
		{
			int velocityIterations = 8;
			int positionIterations = 1;

			// Instruct the world to perform a single step of simulation. It is
			// generally best to keep the time step and iterations fixed.
			world.Step(dt, velocityIterations, positionIterations);

			//Iterate over the bodies in the physics world
			for (var b = world.BodyList; b != null; b = b.Next)
			{
				if (b.UserData != null) {
					//Synchronize the AtlasSprites position and rotation with the corresponding body
					var myActor = ((CCNode)b.UserData);
					myActor.PositionX = b.Position.x * Constants.PTM_RATIO;                  
					myActor.PositionY = b.Position.y * Constants.PTM_RATIO;
					myActor.Rotation = -1 * CCMacros.CCRadiansToDegrees(b.Angle);  
				}	
			}

		}

		public static CCScene Scene
		{

			get {
				var scene = new CCScene ();
				
				var layer = new TheLevel ();
				
				// add layer as a child to scene
				scene.AddChild (layer);
				
				// return the scene
				return scene;			
			}
		}

		internal void ProceedToNextTurn(Ninja theNinja) 
		{ //if ninja hit the ground
			
			if (theNinja == currentBodyNode) {
				
				//CCLog.Log("the current ninja has hit the ground");
				
				Unschedule(TimerAfterThrow); // unschedule this since the ninja has hit the ground we won't need it
				Schedule(MoveNextNinjaIntoSling, 1.0f);
				
			} else {
				
				// CCLog.Log( "disregard: some other ninja not in play has hit the ground somehow");
				
			}
			
		}
		
		void MoveNextNinjaIntoSling(float delta) 
		{
			
			if (!somethingJustScored) { //dont move a ninja unless we are done scoring
				
				Unschedule(MoveNextNinjaIntoSling);
				
				ninjaBeingThrown ++;
				
				if ( ninjaBeingThrown <= ninjasToTossThisLevel && pointTotalThisRound < pointsToPassLevel ) {
					
					switch (ninjaBeingThrown) { 
					case 2:
						currentBodyNode = ninja2;
						break;
					case 3:
						currentBodyNode = ninja3;
						break;
					case 4:
						currentBodyNode = ninja4;
						break;
					case 5:
						currentBodyNode = ninja5;
						break;
						
					}

                    b2Vec2 locationInMeters = b2Vec2.Zero;
                    locationInMeters.Set(ninjaStartPosition1.X / Constants.PTM_RATIO, ninjaStartPosition1.Y / Constants.PTM_RATIO);
					currentBodyNode.Body.SetTransform( locationInMeters , CCMacros.CCDegreesToRadians( 0  )  );
					
					currentBodyNode.SpriteInSlingState();
					
					throwInProgress = false;
					
				}  else if ( ninjaBeingThrown > ninjasToTossThisLevel || pointTotalThisRound >= pointsToPassLevel) { 
					
					
					ScheduleOnce(ResetOrAdvanceLevel, 2.0f);
				}
				
			}
		}
		
		
		internal void StopDotting ()
		{
			
			dottingOn = false;
			
		}

		internal void ShowNinjaImpactingStack(Ninja theNinja)
		{
			
			
			if (theNinja == currentBodyNode) { //make sure the currentBodyNode is the same ninja that hit the stack
				
				currentBodyNode.SpriteInRollStateWithAnimationFirst();  //if you have animated frames you want to include use this
				
				// do anything you want when the ninja hits any stack object
				//maybe you want extra audio
				
			}
			
		}

		internal void ShowNinjaOnGround(Ninja theNinja)
		{
			
			
			if (theNinja == currentBodyNode) { //make sure the currentBodyNode is the same ninja that hit the ground
				
				Schedule(MakeNinjaStaticOnGround, 1.0f/60.0f);
				
				// do anything else you want when the ninja hits the ground
				
				currentBodyNode.ScheduleOnce(currentBodyNode.FadeThenRemove, 2.0f);      //I like to fade out the sprite after a couple seconds
				
			}
		}

		internal void MakeNinjaStaticOnGround(float delta) 
		{
			
			//optionally you can make the ninja a static body when it hits the ground (keeps it from rolling)
			
			currentBodyNode.SpriteInGroundState();
			
			
			
			currentBodyNode.Body.SetType(b2BodyType.b2_staticBody );
			currentBodyNode.Body.SetTransform( currentBodyNode.Body.Position , CCMacros.CCDegreesToRadians( 0  )  );
			
			Unschedule(MakeNinjaStaticOnGround);
		}

		void MoveScreen(int amountToShiftScreen) 
		{
			
			this.Position = new CCPoint( this.PositionX - amountToShiftScreen, this.PositionY  );
			
			if (areWeOnTheIPad) { //the label seems to stay in one place, but really the entire layer is moving (only do this on iPad)
				
				MenuButton.PositionX = MenuButton.PositionX + amountToShiftScreen;
				//MenuButton.PositionY = MenuButton.PositionY;

				currentScoreLabel.PositionX = currentScoreLabel.PositionX + amountToShiftScreen;

				highScoreLabel.PositionX = highScoreLabel.PositionX + amountToShiftScreen;
			}
			
			//the clouds / particles will seem like they wont move as much as the entire layer
			
			backgroundLayerHills.PositionX = backgroundLayerHills.PositionX + (amountToShiftScreen * .5f);
			backgroundLayerClouds.PositionX = backgroundLayerClouds.PositionX + (amountToShiftScreen * .75f ); 
			if (system != null)
				system.PositionX = system.PositionX + (amountToShiftScreen * .75f);
			
			
			// deal with scaling and y positions...
			
			if (amountToShiftScreen > 0) { //scaling down
				
				if ( this.Scale > maxScaleDownValue) {
					
					this.Scale -= scaleAmount;
					
				}
				
				
				
			} else { //or scaling up
				
				if ( this.Scale < 1) {
					
					this.Scale += scaleAmount;
					
				}
				
			}
			
		}

		void PutEverythingInStartingViewOfSlingShot()
		{
			
			this.Position = CCPoint.Zero;
			
			MenuButton.Position = menuStartPosition;
			currentScoreLabel.Position = currentScoreLabelStartPosition;
			highScoreLabel.Position = highScoreLabelStartPosition;
			system.Position = particleSystemStartPosition;
			backgroundLayerClouds.Position = cloudLayerStartPosition;
			backgroundLayerHills.Position = hillsLayerStartPosition;
			
			this.Scale = 1;
			
			areWeInTheStartingPosition = true;
			
		}

		void PutEverythingInViewOfTargets()
		{
			
			this.PositionX = worldMaxHorizontalShift;
			this.PositionY = adjustY;
			
			if (areWeOnTheIPad) 
			{  //I'm only keeping these labels in place on the iPad because it doesn't scale up or down 
				
				MenuButton.PositionX = menuStartPosition.X - worldMaxHorizontalShift;

				currentScoreLabel.PositionX = currentScoreLabelStartPosition.X - worldMaxHorizontalShift;

				highScoreLabel.PositionX = highScoreLabelStartPosition.X - worldMaxHorizontalShift;

				
			}
			
			
			backgroundLayerHills.PositionX = hillsLayerStartPosition.X - (worldMaxHorizontalShift * .5f);

			backgroundLayerClouds.PositionX = cloudLayerStartPosition.X - (worldMaxHorizontalShift * .75f);

			system.PositionX = particleSystemStartPosition.X - (worldMaxHorizontalShift * .75f);

			if ( this.Scale < maxScaleDownValue) {
				
				this.Scale = maxScaleDownValue;
				
			}
			
			areWeInTheStartingPosition = false;
		}

		#region TOUCHES BEGAN  / MOVED
		public override void TouchesBegan (System.Collections.Generic.List<CCTouch> touches)
		{
			//base.TouchesBegan (touches, event_);
			foreach ( var touch in touches ) {
				var location = touch.LocationInView; 
				
				location = CCDirector.SharedDirector.ConvertToGl(location);
				
				previousTouchLocationX = location.X;
				
				if ( !throwInProgress ) {
					
					currentBodyNode.Body.SetType(b2BodyType.b2_staticBody); 
					
				}
				
				if ( topRightTouchEnablesDebugMode && location.X > screenWidth * .9f && location.Y > screenHeight * .9f ) 
				{
					CCLog.Log(@"touching upper right");
					EnableDebugMode();
				} 
			}
		}

        /// <summary>
        /// Cock the slingshot to the strech poing given in the pt parameter.
        /// </summary>
        /// <param name="pt"></param>
        private bool CockTheSlingshot(CCPoint pt)
        {
            if ((this.CheckCircleCollision(pt, 2, slingShotCenterPosition, maxStretchOfSlingShot) || slingShotNinjaInHand)
                && !throwInProgress && areWeInTheStartingPosition)
            {

                if (!slingShotNinjaInHand)
                {

                    positionInSling = slingShotCenterPosition;
                    slingShotNinjaInHand = true;

                    strapBack.Visible = true;
                    strapFront.Visible = true;
                    strapEmpty.Visible = false;
                }


                float currentAngle = (int)currentBodyNode.Body.Angle;


                float radius = maxStretchOfSlingShot; //radius of slingShot

                float angle = CalculateAngle(pt.X, pt.Y, slingShotCenterPosition.X, slingShotCenterPosition.Y);  //angle from slingShot center to the location of the touch

                // if the user is moving the ninja within the max stretch of the slingShot  (the radius)
                if (this.CheckCircleCollision(pt, 2, slingShotCenterPosition, radius))
                {

                    positionInSling.X = pt.X;
                    positionInSling.Y = pt.Y;

                    //tie the strap size into the location of the touch in relation to the distance from the slingshot center

                    float scaleStrap = (Math.Abs(slingShotCenterPosition.X - pt.X)) / radius;

                    scaleStrap = scaleStrap + 0.3f;  //add a little extra

                    if (scaleStrap > 1)
                    {  //make sure it doesn't go over 100% scale
                        scaleStrap = 1;
                    }

                    strapFront.ScaleX = scaleStrap;
                    strapBack.ScaleX = strapFront.ScaleX;  //strap back is the same size as the strap front (until we rework it a tad below)
                }
                else
                {
                    // if the user is moving the ninja outside the max stretch of the slingShot

                    float angleRadians = CCMacros.CCDegreesToRadians(angle - 90);
                    positionInSling.X = slingShotCenterPosition.X - ((float)Math.Cos(angleRadians) * radius);
                    positionInSling.Y = slingShotCenterPosition.Y + ((float)Math.Sin(angleRadians) * radius);

                    strapFront.ScaleX = 1;
                    strapBack.ScaleX = 1;


                }

                strapFront.Rotation = angle - 90;
                AdjustBackStrap(angle);


                //positions the box2D bodyNode of the ninja

                b2Vec2 locationInMeters = new b2Vec2(positionInSling.X / Constants.PTM_RATIO, positionInSling.Y / Constants.PTM_RATIO);

                currentBodyNode.Body.SetTransform(locationInMeters, CCMacros.CCDegreesToRadians(currentAngle));



                currentBodyNode.SpriteInPulledBackSlingState();
                return (true);
            }
            // Did not consume this point, so let it pass to the next consumer
            return (false);
        }

        private void PanTheScreen(int dx)
        {
            int amountToShiftScreen;
            amountToShiftScreen = ReturnAmountToShiftScreen(dx);  //just a method to prevent the screen from moving too wildly.


            // pan the screen back and forth if there isn't a ninja in hand

            if (this.PositionX <= 0 && this.PositionX >= worldMaxHorizontalShift && !slingShotNinjaInHand)
            {

                areWeInTheStartingPosition = false;

                MoveScreen(amountToShiftScreen);


                if (this.PositionX > 0)
                { // if we try to shift too far left, we reset to the starting position

                    PutEverythingInStartingViewOfSlingShot();


                }

                else if (this.PositionX < worldMaxHorizontalShift)
                { // if we try to shift too far the other way, we keep everything at the max position

                    PutEverythingInViewOfTargets();


                }
            }
        }

		public override void TouchesMoved (System.Collections.Generic.List<CCTouch> touches)
		{
			//base.TouchesMoved (touches, event_);
			if ( autoPanningInProgress ) {
				
				this.CancelAutoPan();
			}
			
			
			
			//Move screen
			
			foreach ( var touch in touches ) 
            {
				var location = touch.LocationInView;

				location = CCDirector.SharedDirector.ConvertToGl(location);



				/////////////////////////////////
				// Move the ninja in the slingshot. IF the screen is in the starting position (this.PositionX == 0) and a throw isn't already in progress, AND our finger is touching around the sling shot

                if (!CockTheSlingshot(location))
                {
					int diff = (int)(location.X - previousTouchLocationX); //difference between the starting/previous touch location and current one
                    PanTheScreen(diff);
				}

				
				previousTouchLocationX = location.X;
			}
		}

		bool CheckCircleCollision ( CCPoint center1,  float radius1, CCPoint center2, float radius2)
		{
			float a = center2.X - center1.X;
			float b = center2.Y - center1.Y;
			float c = radius1 + radius2;
			float distanceSqrd = (a * a) + (b * b);
			
			if (distanceSqrd < (c * c) ) {
				
				return true;
			} else {
				return false;
			}
			
		}
		
		
		
		float CalculateAngle (float x1, float y1, float x2, float y2)
		{
			// DX
			float x = x2 - x1;
			
			// DY
			float y = y2 - y1;
			
			
			float angle = 180 + ((float)Math.Atan2(-x, -y) * (180/(float)Math.PI));
			
			return angle;  //degrees
		}

		int ReturnAmountToShiftScreen(int diff)
		{
			
			int amountToShiftScreen;
			
			
			
			if ( diff > 50) {
				
				amountToShiftScreen = 50;
				
			}  else if ( diff < -50) {
				
				amountToShiftScreen = -50;
				
			} else {
				
				amountToShiftScreen = diff ;
			}

			if (!reverseHowFingerPansScreen) {
				
				amountToShiftScreen = amountToShiftScreen * -1;
				
				
			} 
			
			if ( amountToShiftScreen < 0 ) {
				
				panningTowardSling = true;
				
			} else {
				
				panningTowardSling = false;
			}
			
			
			return amountToShiftScreen;  
		}


		#endregion

        private void FireSlignshot(b2Vec2 dir)
        {
            GameSounds.SharedGameSounds.ReleaseSlingSounds();

            SwitchAllStackObjectsToDynamic(0);


            throwCount++;
            dotCount = 0;

            throwInProgress = true;

            currentBodyNode.Body.SetType(b2BodyType.b2_dynamicBody);
            // SetAwake is set to true when changed to dynamicBody
            //currentBodyNode.Body.SetAwake(false);
            //currentBodyNode.Body.SetActive(true);

            strapBack.Visible = false;
            strapFront.Visible = false;
            strapEmpty.Visible = true;


            // This determines the speed variance

            speed = (float)(Math.Abs(slingShotCenterPosition.X - positionInSling.X)) + (float)(Math.Abs(slingShotCenterPosition.Y - positionInSling.Y));

            speed = speed / 5;

            speed = speed * multipyThrowPower;




            // targetPosition is actually touch point
            //b2Vec2 targetInWorld = new b2Vec2(location.X, location.Y);

            // This determines the actual direction of the ninja, from slingshot center position to touch point

            // This moves the body, and the key part is multiplication
            // of 'speed' variable with direction. .
            currentBodyNode.Body.LinearVelocity = speed * dir;

            initialPanAmount = (int)(currentBodyNode.Body.LinearVelocity.x * 1.25f);

            slingShotNinjaInHand = false;

            currentBodyNode.SpriteInAirState();


            // add dots following throw...

            RemovePreviousDots();

            dottingOn = true;  //dotting in progress

            //Schedule(PlaceWhiteDots,1.0f/45.0f);  //increase or decrease frequency of dots with the interval
            Schedule(PlaceWhiteDots, 1.0f / 45.0f);  //increase or decrease frequency of dots with the interval


            //ensures throwInProgress is set to NO after 6 seconds
            Unschedule(TimerAfterThrow);
            Schedule(TimerAfterThrow, 6.0f);

            if (dir.x > 0f) // fire direction is forward
            {

                StartScreenPanToTargetsWithAutoReverseOn();

            }


        }

		#region TOUCHES ENDED

		public override void TouchesEnded (System.Collections.Generic.List<CCTouch> touches)
		{
			//base.TouchesEnded (touches, event_);
			foreach ( var touch in touches ) {
				var location = touch.LocationInView;
				location = CCDirector.SharedDirector.ConvertToGl(location);
				if (  slingShotNinjaInHand ) 
                {
                    b2Vec2 direction = new b2Vec2(slingShotCenterPosition.X - location.X, slingShotCenterPosition.Y - location.Y);
                    direction.Normalize();
                    FireSlignshot(direction);
				}
				
				else if (continuePanningScreenOnFingerRelease) {
					
					if ( panningTowardSling) {
						
						StartScreenPanToSling();
						
					} else {
						
						StartScreenPanToTargets(); 
					}
					
				}
				


			}
		}

		void RemovePreviousDots()
		{
			
			int someNum = 0;


			// 
			if (throwCount % 2 == 0)
			//if (throwCount % 2 != 0)
			{  //odd numbered turn..
				
				
				
				while(someNum <= dotTotalOnOddNumberedTurn ) 
				{
					
					RemoveChildByTag(Constants.TagForWhiteDotsOddNumberedTurn + someNum, false);
					someNum ++;
					
					
				}
				
				dotTotalOnOddNumberedTurn = 0;
				
			} 
			else { //even numbered turn..
				
				
				
				while(someNum <= dotTotalOnEvenNumberedTurn ) {
					
					RemoveChildByTag(Constants.TagForWhiteDotsEvenNumberedTurn + someNum, false);
					someNum ++;
					
					
				}
				
				dotTotalOnEvenNumberedTurn = 0;
			}
			
			
		}

		void RemoveAllDots()
		{
			
			// runs when the board is cleaned up...
			
			int someNum = 0;
			
			while(someNum <= dotTotalOnOddNumberedTurn ) {
				
				RemoveChildByTag(Constants.TagForWhiteDotsOddNumberedTurn + someNum, false);
				someNum ++;
				
			}
			
			dotTotalOnOddNumberedTurn = 0;
			someNum = 0;
			
			while(someNum <= dotTotalOnEvenNumberedTurn ) {
				
				RemoveChildByTag(Constants.TagForWhiteDotsEvenNumberedTurn + someNum, false);
				someNum ++; 
				
			}
			
			dotTotalOnEvenNumberedTurn = 0;
			
		}

		void PlaceWhiteDots(float delta) {
			
			
			
			if (dottingOn) {
				
				dotCount ++;
				
				CCSprite whiteDot = new CCSprite(@"circle.png");
				
				if (throwCount % 2 != 0){  //odd number..
					
					AddChild(whiteDot, Constants.DepthWhiteDots, Constants.TagForWhiteDotsOddNumberedTurn + dotCount);
					dotTotalOnOddNumberedTurn = dotCount;
					
				} else {
					
					AddChild(whiteDot, Constants.DepthWhiteDots, Constants.TagForWhiteDotsEvenNumberedTurn + dotCount);
					dotTotalOnEvenNumberedTurn = dotCount;
				}
				
				
				whiteDot.PositionX = currentBodyNode.PositionX;
				whiteDot.PositionY = currentBodyNode.PositionY;
				
				if (dotCount % 2 != 0){  //odd number..
					
					whiteDot.Scale = .5f;
					
				} 
				
				
			} else {
				
				Unschedule(PlaceWhiteDots);
				

			}
			
		}

		void TimerAfterThrow(float delta)
		{
			
			// this method will get cancelled if the ninja hits the ground. Which in most cases will happen. 
			// BUT if the ninja were to get stuck on an stack object and never hit the ground, this method will get called
			
			ProceedToNextTurn(currentBodyNode);
			
			Unschedule(TimerAfterThrow);
		}


		#endregion

		#region SCREEN PANNING

		void StartScreenPanToTargets() {
			
			panAmount = initialPanAmount;
			
			autoPanningInProgress = true;
			autoReverseOn = false;
			panningTowardSling = false;
			
			Unschedule(AutoScreenPanToSling);
			Schedule(AutoScreenPanToTargets, 1.0f/60.0f);
			
			if ( !areWeOnTheIPad ) {
				
				Unschedule(MoveScreenUp);
				Schedule(MoveScreenDown, 1.0f/60.0f);
			}
			
		}
		
		void StartScreenPanToTargetsWithAutoReverseOn ()
		{
			
			panAmount = initialPanAmount;
			
			autoPanningInProgress = true;
			autoReverseOn = true;
			panningTowardSling = false;
			
			Unschedule(AutoScreenPanToSling);
			Schedule(AutoScreenPanToTargets, 1.0f/60.0f);
			
			if ( !areWeOnTheIPad ) {
				Unschedule(MoveScreenUp);
				Schedule(MoveScreenDown, 1.0f/60.0f);
			}
			
		}
		
		void StartScreenPanToSlingIfScoringIsNotOccuring(float delta) {
			
			if ( !somethingJustScored) {
				
				CCLog.Log(@"scoring done, auto move back");
				
				StartScreenPanToSling();
				Unschedule(StartScreenPanToSlingIfScoringIsNotOccuring);
			} else {
				
				CCLog.Log(@"something just scored, wait a bit longer to move screen back");
				
			}
			
		}

		void StartScreenPanToSling()
		{
			
			panAmount = initialPanAmount + extraAmountOnPanBack;
			
			autoPanningInProgress = true;
			panningTowardSling = true;
			
			Unschedule(AutoScreenPanToTargets); 
			Schedule(AutoScreenPanToSling, 1.0f/60.0f);

			if ( !areWeOnTheIPad ) {
				
				Unschedule(MoveScreenDown);
				Schedule(MoveScreenUp, 1.0f/60.0f);
			}
			
		}

		void AutoScreenPanToTargets(float delta)
		{
			
			if (panAmount > 3 ) {
				
				panAmount = panAmount - .5f;
				
			}
			
			
			if ( this.PositionX > worldMaxHorizontalShift ) {
				
				
				if (  this.PositionX > worldMaxHorizontalShift && this.PositionX < worldMaxHorizontalShift + 50) {  //slows down panning when close to finishing
					
					MoveScreen(3); 
					
				} else {
					
					MoveScreen((int)panAmount); 
				}
				
				
				
			} else {
				
				Unschedule(AutoScreenPanToTargets);
				PutEverythingInViewOfTargets();
				
				if ( autoReverseOn) {
					
					Schedule(StartScreenPanToSlingIfScoringIsNotOccuring, 2.0f);
				}
			}
			
		}
		
		void AutoScreenPanToSling(float delta)
		{
			
			if (panAmount > 3 ) {
				
				panAmount = panAmount - .5f;
				
			}
			
			if ( this.PositionX < 0 ) {
				
				
				if (  this.PositionX < 0 && this.PositionX > -50) {  //slows down panning when close to finishing
					
					MoveScreen(-3); 
					
				} else {
					
					MoveScreen((int)panAmount * -1); 
				}
				
				
				
			} else {
				
				Unschedule(AutoScreenPanToSling);
				PutEverythingInStartingViewOfSlingShot();
				
				
				
				autoPanningInProgress = false;
				this.Scale = 1;
			}
			
		}

		
		void CancelAutoPan ()
		{
			
			autoPanningInProgress = false;
			Unschedule(AutoScreenPanToSling);
			Unschedule(AutoScreenPanToTargets);
			Unschedule(StartScreenPanToSlingIfScoringIsNotOccuring);
		}

		
		void MoveScreenUp(float delta) 
		{
			
			
			
			if ( this.PositionY < 0 ) {
				
				//this.PositionX = this.Position.x;
				this.PositionY += 2;
				
				
			} else {
				
				this.PositionX = this.PositionX;
				this.PositionY = 0;
				
				Unschedule(MoveScreenUp);
			}
			
			
		}
		
		void MoveScreenDown(float delta)
		{

			if ( this.PositionY > adjustY) {
				
				this.PositionX = this.PositionX;
				this.PositionY -= 2;
				
				
			} else {
				
				this.PositionX = this.PositionX;
				this.PositionY = adjustY;
				Unschedule(MoveScreenDown);
			}

		}

		#endregion

		#region SIMPLE BREAK FX
		
		void showSimpleVisualFX(CCPoint positionToShowScore, BreakEffect theSimpleScoreVisualFX)
		{
			
			if ( theSimpleScoreVisualFX == BreakEffect.SmokePuffs ) {
				
				GameSounds.SharedGameSounds.PlayBreakSound();
				
				CCLog.Log("Play Smoke Puffs on Score");
				
				CustomAnimation smokeFX = new CustomAnimation ("puffs",
				                                               1,
				                                               7,
				                                               (int)positionToShowScore.X,
				                                               (int)positionToShowScore.Y,
				                                               false,
				                                               false,
				                                               false,
				                                               false);
				AddChild(smokeFX, Constants.DepthVisualFx);
				
			} else if ( theSimpleScoreVisualFX == BreakEffect.Explosion ) {
				
				GameSounds.SharedGameSounds.PlayBreakSound();
				
				CCLog.Log("Play explosion on Score");
				
				CustomAnimation smokeFX = new CustomAnimation ("explosion",
				                                               1,
				                                               11,
				                                               (int)positionToShowScore.X,
				                                               (int)positionToShowScore.Y,
				                                               false,
				                                               false,
				                                               false,
				                                               false);
				AddChild(smokeFX, Constants.DepthVisualFx);
				
			}    
			
		}
		

		public void ShowPoints(int pointValue, CCPoint positionToShowScore, BreakEffect theSimpleScoreVisualFX)
		{
			
			pointTotalThisRound = pointTotalThisRound + pointValue;
			UpdatePointsLabel();
			
			//CCLog.Log("Point Value %i, total points is now %i", pointValue, pointTotalThisRound);
			
			showSimpleVisualFX(positionToShowScore, theSimpleScoreVisualFX);
			
			SomethingJustScoredVar();
			
			if ( useImagesForPointScoreLabels) {
				
				ShowPointsWithImagesForValue(pointValue, positionToShowScore);
				
			} else {
				
				ShowPointsWithFontLabelForValue(pointValue, positionToShowScore);
			}
			
			
			
		}

		void ShowPointsWithImagesForValue(int pointValue, CCPoint positionToShowScore)
		{
			
			
			CCSprite scoreLabel;
			
			if (pointValue == 100) {
				
				scoreLabel = new CCSprite ("100points.png");
			}
			else if (pointValue == 500) {
				
				scoreLabel = new CCSprite ("500points.png");
			} 
			else if (pointValue == 1000) {
				
				scoreLabel = new CCSprite ("1000points.png");
			}  
			else if (pointValue == 5000) {
				
				scoreLabel = new CCSprite ("5000points.png");
			}  
			else if (pointValue == 10000) {
				
				scoreLabel = new CCSprite ("10000points.png");
			} 
			else { //default
				
				scoreLabel = new CCSprite ("100points.png");
				
			}
			
			
			AddChild(scoreLabel, Constants.DepthPointScore);
			scoreLabel.Position = positionToShowScore;
			
			
			CCMoveTo moveAction = new CCMoveTo(1.0f, new CCPoint ( scoreLabel.Position.X  , scoreLabel.Position.Y + 25 ));
			
			scoreLabel.RunAction(moveAction);
			
			CCSequence seq = new CCSequence(
			                   new CCFadeTo(1.5f, 20),          
			                   new CCCallFuncN(RemoveThisChild));
			
			scoreLabel.RunAction(seq);
			
		}
		
		
		
		
		
		void RemoveThisChild(object sender)
		{
			
			
			CCSprite theSprite = (CCSprite)sender;
			
			RemoveChild(theSprite, true);
			
			
			
		}
		
		void ShowPointsWithFontLabelForValue(int pointValue, CCPoint positionToShowScore)
		{

			CCLabelTTF scoreLabel = new CCLabelTTF(string.Format("{0}", pointValue), "MarkerFelt", 22);
			AddChild(scoreLabel, Constants.DepthPointScore);
			scoreLabel.Color = new CCColor3B(255,255,255);
			scoreLabel.Position = positionToShowScore;
			
			CCMoveTo moveAction = new CCMoveTo(1.0f, new CCPoint ( scoreLabel.Position.X  , scoreLabel.Position.Y + 25 ));
			
			scoreLabel.RunAction(moveAction);
			
			CCSequence seq = new CCSequence(
				new CCFadeTo(1.5f, 20),          
				new CCCallFuncN(RemoveThisLabel));
			
			scoreLabel.RunAction(seq);

		}
		
		void RemoveThisLabel(object sender)
		{
			CCLabelTTF theLabel = (CCLabelTTF)sender;
			
			RemoveChild(theLabel, true);
			
		}
		

		#endregion

		#region POINTS
		
		void UpdatePointsLabel ()
		{
			
			var updateLabel = String.Format("{0}/{1} ", pointTotalThisRound, pointsToPassLevel);
			currentScoreLabel.Text = updateLabel;
			
		}
		
		void SomethingJustScoredVar()
		{
			
			
			somethingJustScored = true;
			
			Unschedule(ResetSomethingJustScoredVar);
			Schedule(ResetSomethingJustScoredVar, 3.0f);
			
		}
	    void ResetSomethingJustScoredVar (float delta)
		{
			
			somethingJustScored = false;
			Unschedule(ResetSomethingJustScoredVar);
		}

		#endregion





		void AdjustBackStrap (float angle)  
		{
			//CCLog.Log(" %f", angle );
			
			if (angle < 30) {
				
				//CCLog.Log(" between 6 and 7 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * 1.0f;
				strapBack.Rotation = strapFront.Rotation * .8f;
				
			} else if (angle < 60) {
				
				//CCLog.Log(" between 7 and 8 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * 1.05f;
				strapBack.Rotation = strapFront.Rotation * .80f;
				
			} else if (angle < 90) {
				
				//CCLog.Log(" between 8 and 9 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * 1.1f;
				strapBack.Rotation = strapFront.Rotation * .85f;
				
			} else if (angle < 120) {
				
				//CCLog.Log(" between 9 and 10 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * 1.2f;
				strapBack.Rotation = strapFront.Rotation * .95f;
				
			} else if (angle < 150) {
				
				//CCLog.Log(" between 10 and 11 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * 1.2f;
				strapBack.Rotation = strapFront.Rotation * .9f;
				
			} 
			
			else if (angle < 180) {
				//CCLog.Log(" between 11 and 12 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * 1.10f;
				strapBack.Rotation = strapFront.Rotation * .85f;
				
			} 
			else if (angle < 210) {
				//CCLog.Log(" between 12 and 1 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * .95f;
				strapBack.Rotation = strapFront.Rotation * .85f;
				
			} 
			else if (angle < 240) {
				//CCLog.Log(" between 1 and 2 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * .7f;
				strapBack.Rotation = strapFront.Rotation * .85f;
				
			}
			
			else if (angle < 270) {
				//CCLog.Log(" between 2 and 3 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * .6f;
				strapBack.Rotation = strapFront.Rotation * .9f;
				
			} 
			
			else if (angle < 300) {
				//CCLog.Log(" between 3 and 4 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * .5f;
				strapBack.Rotation = strapFront.Rotation * 1.0f;
				
			}
			else if (angle < 330) {
				//CCLog.Log(" between 4 and 5 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * .6f;
				strapBack.Rotation = strapFront.Rotation * 1.1f;
				
			}
			
			else if (angle < 360) {
				//CCLog.Log(" between 5 and 6 oclock");
				
				strapBack.ScaleX = strapBack.ScaleX * .6f;
				strapBack.Rotation = strapFront.Rotation * 1.1f;
			}
			
			
		}

		internal void ShowBoardMessage(string theMessage)
		{
			
			CCLabelTTF boardMessage = new CCLabelTTF(theMessage, "MarkerFelt", 22);
			AddChild(boardMessage, Constants.DepthPointScore);
			boardMessage.Color = new CCColor3B(255,255,255);
			boardMessage.PositionX = screenWidth /2;
			boardMessage.PositionY = screenHeight * .7f;
			
			
			CCSequence seq = new CCSequence(
			                   new CCScaleTo(2.0f, 2.0f),
			                   new CCFadeTo(1.0f, 0),          
			                   new CCCallFuncN(RemoveBoardMessage)  //NOTE: CCCallFuncN  works, CCCallFunc does not.
				);
			
			boardMessage.RunAction(seq);
			
			
		}

		internal void RemoveBoardMessage(object sender)
		{
			
			
			CCLabelTTF boardMessage = (CCLabelTTF)sender;
			
			RemoveChild(boardMessage, true);
			
			//CCLog.Log("Removing Board Message");
			
		}

		void DoPointBonusForExtraNinjas()
		{
			
			
			int ninjasLeft = (ninjasToTossThisLevel - ninjaBeingThrown) + 1;
			
			bonusThisRound = ( bonusPerExtraNinja * ninjasLeft);
			pointTotalThisRound = pointTotalThisRound  + bonusThisRound ;
			
			UpdatePointsLabel();
			
			CCLog.Log(@"Ninjas Left to Throw: %i", ninjasLeft );
		}

		internal void ResetOrAdvanceLevel(float delta) {
			
			
			
			if ( pointTotalThisRound >= pointsToPassLevel ) {
				
				//CCLog.Log("board passed");
				
				DoPointBonusForExtraNinjas();
				
				if ( bonusThisRound > 0 ) { //if theres a bonus, show it in the level passed message
					
					var bonusMessage = string.Format("Level Passed: {0} Bonus!", bonusThisRound);
					ShowBoardMessage(bonusMessage);
					
				} else {
					
					ShowBoardMessage("Level Passed"); 
				}
				
				GameData.SharedData.HighScoreForLevel = pointTotalThisRound; //will check to see if there's a high score set
				
				GameData.SharedData.AddToPointTotalForAllLevels(pointTotalThisRound);
				
				GameData.SharedData.levelUp();  //level up
				
			} else {
				
				GameData.SharedData.HighScoreForLevel = pointTotalThisRound; //will check to see if there's a high score set even if you failed the round

				ShowBoardMessage("Level Failed");
				//CCLog.Log("not enough points to go up a level, will reset with the same board");
			}
			
			
			
			ScheduleOnce(TransitionOut, 3.0f);  //if you want to transition after a different amount of time, then change 3 to whatever

		}

		public void TransitionAfterMenuPop() {
			
			//transition upon coming back from the menu
			
			CancelAutoPan();
			StopDotting();
			UnscheduleAllSelectors();
			
			ScheduleOnce(TransitionOut, 0.1f); 
			
		}

		void TransitionOut(float delta)
		{
			
			CleanupTheLevel();

			// Too select a random transition comment the two lines below and uncomment the section below.
			var transition = Transition2;
			CCDirector.SharedDirector.ReplaceScene(transition);

			  
		    // other transition options...
		    
//		    int diceRoll = cocos2d.Random.Next(0,4); //0 to 4
//			CCTransitionScene transition;
//
//		    switch (diceRoll) {
//		        case 0:
//					transition = Transition0;
//		            break;
//		        case 1:
//					transition = Transition1;
//		            break;
//		        case 2:
//					transition = Transition2;
//		            break;
//		        case 3:
//					transition = Transition3;
//		            break;
//		        case 4:
//					transition = Transition4;
//		            break;
//		            
//		        default:
//					transition = Transition0;
//		            break;
//		    }
//     
//			CCDirector.SharedDirector.ReplaceScene(transition);
		}

		CCTransitionScene Transition0
		{
			get { return new CCTransitionFadeDown(1, TheLevel.Scene); }
		}

		CCTransitionScene Transition1
		{
			get { return new CCTransitionFlipX(1, TheLevel.Scene, CCTransitionOrientation.RightOver); }
		}

		CCTransitionScene Transition2 
		{
			get	{ return new CCTransitionFade (1 , TheLevel.Scene); }
		}

		CCTransitionScene Transition3 
		{
			get	{ return new CCTransitionFlipAngular (1 , TheLevel.Scene, CCTransitionOrientation.DownOver); }
		}

		CCTransitionScene Transition4 
		{
			get { return new CCTransitionFadeTR (1 , TheLevel.Scene); }
		}

		void CleanupTheLevel()
		{
			
			CancelAutoPan();

			PauseSchedulerAndActions();
			
			RemoveAllDots();
			
			RemoveChild(backgroundLayerHills, false);
			RemoveChild(backgroundLayerClouds, false);
			
			RemoveChild(slingShotFront, false);
			RemoveChild(strapFront, false);
			RemoveChild(strapBack, false);
			RemoveChild(strapEmpty, false);
			RemoveChild(stack, false);
			
			
			CCLog.Log("deleting body nodes");
			
			//Iterate over the bodies in the physics world
			for (var b = world.BodyList; b != null; b = b.Next)
			{
				
				CCLog.Log("nodes found");
				
				var myNode = b.UserData as BodyNode;
				if ( myNode != null ) {
					
					myNode.RemoveSpriteAndBody();
					
				}
			}
			
		}

	}
}

