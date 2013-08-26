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
	public class Enemy : BodyNode
	{
		b2World theWorld;
		string baseImageName;
		string spriteImageName;
		CCPoint initialLocation;

		
		int breaksAfterHowMuchContact; //0 will break the enemy after first contact
		int damageLevel; //tracks how much damage has been done
		public bool BreaksOnNextDamage { get; set; } 
		
		bool isRotationFixed; //enemy wont rotate if set to YES
		
		float theDensity;
		CreationMethod shapeCreationMethod; //same as all stack objects, check shape definitions in Constants.h
		
		public bool DamagesFromGroundContact { get; set; }
		public bool DamagesFromDamageEnabledStackObjects { get; set; }  //stack objects must be enabled to damageEnemy 
		
		bool differentSpritesForDamage; //whether or not you've included different images for damage progression (recommended)
		
		public int PointValue { get; set; }
		public BreakEffect SimpleScoreVisualFX { get; set; }  //defined in constants, which visual effect occurs when the enemy breaks
		
		int currentFrame;
		int framesToAnimateOnBreak; //if 0 won't show any break frames
		
		bool enemyCantBeDamagedForShortInterval; // after damage occurs the enemy gets a moment of un-damage-abilty, which should play better ( I think)
		

		public Enemy (b2World world,
		              CCPoint location,
		              string spriteFileName,
		              bool isTheRotationFixed,
		              bool getsDamageFromGround,
		              bool doesGetDamageFromDamageEnabledStackObjects,
		              int breaksFromHowMuchContact,
		              bool hasDifferentSpritesForDamage,
		              int numberOfFramesToAnimateOnBreak,
		              float density,
		              CreationMethod createHow,
		              int points,
		              BreakEffect simpleScoreVisualFXType )
		{
			InitWithWorld( world,
			               location,
			               spriteFileName,
			               isTheRotationFixed,
			               getsDamageFromGround,
			               doesGetDamageFromDamageEnabledStackObjects,
			               breaksFromHowMuchContact,
			               hasDifferentSpritesForDamage,
			               numberOfFramesToAnimateOnBreak,
			               density,
			               createHow,
			               points,
			               simpleScoreVisualFXType );
		}

		void InitWithWorld(b2World world,
		                    CCPoint location,
		                    string spriteFileName,
		                    bool isTheRotationFixed,
		                    bool getsDamageFromGround,
		                    bool doesGetDamageFromDamageEnabledStackObjects,
		                    int breaksFromHowMuchContact,
		                    bool hasDifferentSpritesForDamage,
		                    int numberOfFramesToAnimateOnBreak,
		                    float density,
		                    CreationMethod createHow,
		                    int points,
		                    BreakEffect simpleScoreVisualFXType )
		{
			this.theWorld = world;
			this.initialLocation = location;
			this.baseImageName = spriteFileName;
			this.spriteImageName =  String.Format("{0}.png", baseImageName);
			
			this.DamagesFromGroundContact = getsDamageFromGround; // does the ground break / damage the enemy
			
			this.damageLevel = 0; //starts at 0, if breaksAfterHowMuchContact also equals 0 then the enemy will break on first/next contact
			this.breaksAfterHowMuchContact = breaksFromHowMuchContact; //contact must be made this many times before breaking, or if set to 0, the enemy will break on first/next contact 
			this.differentSpritesForDamage = hasDifferentSpritesForDamage; //will progress through damage frames if this is YES, for example,  enemy_damage1.png, enemy_damage2.png
			
			this.currentFrame = 0;
			this.framesToAnimateOnBreak = numberOfFramesToAnimateOnBreak;  //will animate through breaks frames if this is more than 0, for example,  enemy_break0001.png, enemy_break0002.png
			
			
			this.theDensity = density;
			this.shapeCreationMethod = createHow;
			
			this.isRotationFixed = isTheRotationFixed;
			
			this.PointValue = points ;
			this.SimpleScoreVisualFX = simpleScoreVisualFXType;
			
			this.DamagesFromDamageEnabledStackObjects = doesGetDamageFromDamageEnabledStackObjects; 
			
			
			if ( damageLevel == breaksAfterHowMuchContact) {  
				BreaksOnNextDamage = true;
			} else {
				BreaksOnNextDamage = false; //duh
				
			}
			
			
			CreateEnemy();
			

		}

		void CreateEnemy ()
		{
			
			
			// Define the dynamic body.
			var bodyDef = new b2BodyDef();
			bodyDef.type = b2BodyType.b2_dynamicBody; //or you could use b2_staticBody
			
			bodyDef.fixedRotation = isRotationFixed;
			
			bodyDef.position.Set(initialLocation.X/Constants.PTM_RATIO, initialLocation.Y/Constants.PTM_RATIO);
			
			b2PolygonShape shape = new b2PolygonShape();
			b2CircleShape shapeCircle = new b2CircleShape();
			
			if (shapeCreationMethod == CreationMethod.DiameterOfImageForCircle) {
				
				var tempSprite = new CCSprite(spriteImageName);
				float radiusInMeters = (tempSprite.ContentSize.Width / Constants.PTM_RATIO) * 0.5f;
				
				shapeCircle.Radius = radiusInMeters;
				
			}
			
			
			else if ( shapeCreationMethod == CreationMethod.ShapeOfSourceImage) {
				
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 4;
				b2Vec2[] vertices = {
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //top left corner
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom left corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 )/ Constants.PTM_RATIO), //bottom right corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO) //top right corner
				};
				shape.Set(vertices, num);
			}
			else if ( shapeCreationMethod == CreationMethod.ShapeOfSourceImageButSlightlySmaller ) {
				
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 4;
				b2Vec2[] vertices = {
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) *.8f / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 )*.8f / Constants.PTM_RATIO), //top left corner
					new b2Vec2( (tempSprite.ContentSize.Width / -2 )*.8f / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 )*.8f / Constants.PTM_RATIO), //bottom left corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 )*.8f / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 )*.8f / Constants.PTM_RATIO), //bottom right corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 )*.8f / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 )*.8f / Constants.PTM_RATIO) //top right corner
				};
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == CreationMethod.Triangle) {
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 3;
				b2Vec2[] vertices = {
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom left corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom right corner
					new b2Vec2( 0.0f / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 )/ Constants.PTM_RATIO) // top center of image
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == CreationMethod.TriangleRightAngle) {
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 3;
				b2Vec2[] vertices = {
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO),  //top right corner
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //top left corner
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 )/ Constants.PTM_RATIO) //bottom left corner
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == CreationMethod.Trapezoid) {
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 4;
				b2Vec2[] vertices = {
					new b2Vec2( (tempSprite.ContentSize.Width / 4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO),  //top of image, 3/4's across
					new b2Vec2( (tempSprite.ContentSize.Width / -4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO),  //top of image, 1/4's across
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom left corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom right corner
				};
				
				shape.Set(vertices, num);
			}
			
			
			else if ( shapeCreationMethod == CreationMethod.Hexagon) {
				
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 6;
				b2Vec2[] vertices = {
					new b2Vec2( (tempSprite.ContentSize.Width / -4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //top of image, 1/4 across
					new b2Vec2( (tempSprite.ContentSize.Width / -2 )  / Constants.PTM_RATIO, 0.0f / Constants.PTM_RATIO), // left, center
					new b2Vec2( (tempSprite.ContentSize.Width / -4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom of image, 1/4 across
					new b2Vec2( (tempSprite.ContentSize.Width / 4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom of image, 3/4's across
					new b2Vec2( (tempSprite.ContentSize.Width /  2 ) / Constants.PTM_RATIO, 0.0f / Constants.PTM_RATIO), // right, center
					new b2Vec2( (tempSprite.ContentSize.Width / 4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO) //top of image, 3/4's across
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == CreationMethod.Pentagon) {
				
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 5;
				b2Vec2[] vertices = {
					new b2Vec2( 0 / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //top of image, center 
					new b2Vec2( (tempSprite.ContentSize.Width / -2 )  / Constants.PTM_RATIO, 0.0f / Constants.PTM_RATIO), // left, center
					new b2Vec2( (tempSprite.ContentSize.Width / -4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom of image, 1/4 across
					new b2Vec2( (tempSprite.ContentSize.Width / 4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom of image, 3/4's across
					new b2Vec2( (tempSprite.ContentSize.Width /  2 ) / Constants.PTM_RATIO, 0.0f / Constants.PTM_RATIO), // right, center
					
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == CreationMethod.Octagon) {
				
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 8;
				b2Vec2[] vertices = {
					new b2Vec2( (tempSprite.ContentSize.Width / -6 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //use the source image octogonShape.png for reference
					new b2Vec2( (tempSprite.ContentSize.Width / -2 )  / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 6 ) / Constants.PTM_RATIO), 
					new b2Vec2( (tempSprite.ContentSize.Width / -2 )  / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -6 ) / Constants.PTM_RATIO), 
					new b2Vec2( (tempSprite.ContentSize.Width / -6 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), 
					new b2Vec2( (tempSprite.ContentSize.Width / 6 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), 
					new b2Vec2( (tempSprite.ContentSize.Width /  2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -6 ) / Constants.PTM_RATIO), 
					new b2Vec2( (tempSprite.ContentSize.Width /  2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 6 ) / Constants.PTM_RATIO), 
					new b2Vec2( (tempSprite.ContentSize.Width / 6 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO) 
				};
				
				shape.Set(vertices, num);
			}
			else if ( shapeCreationMethod == CreationMethod.Parallelogram) {
				
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 4;
				b2Vec2[] vertices = {
					new b2Vec2( (tempSprite.ContentSize.Width / -4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //top of image, 1/4 across
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom left corner
					new b2Vec2( (tempSprite.ContentSize.Width / 4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom of image, 3/4's across
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO) //top right corner
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == CreationMethod.CustomCoordinates1) {  //use your own custom coordinates from a program like Vertex Helper Pro
				
				var num = 4;
				b2Vec2[] vertices = {
					new b2Vec2(-64.0f / Constants.PTM_RATIO, 16.0f / Constants.PTM_RATIO),
					new b2Vec2(-64.0f / Constants.PTM_RATIO, -16.0f / Constants.PTM_RATIO),
					new b2Vec2(64.0f / Constants.PTM_RATIO, -16.0f / Constants.PTM_RATIO),
					new b2Vec2(64.0f / Constants.PTM_RATIO, 16.0f / Constants.PTM_RATIO)
				};
				shape.Set(vertices, num);
			}
			
			// Define the dynamic body fixture.
			var fixtureDef = new b2FixtureDef();
			
			if ( shapeCreationMethod == CreationMethod.DiameterOfImageForCircle) {
				
				fixtureDef.shape = shapeCircle;	
				
			} else {
				fixtureDef.shape = shape;	
				
			}
			
			fixtureDef.density = theDensity;
			fixtureDef.friction = 0.3f;
			fixtureDef.restitution =  0.1f; //how bouncy basically
			
			CreateBodyWithSpriteAndFixture(theWorld, bodyDef, fixtureDef, spriteImageName);
			
			
			var blinkInterval = Cocos2D.CCRandom.Next(3,8); // range 3 to 8
			
			Schedule(Blink, blinkInterval); //comment this out if you never want to show the blink
			
		}

		public void DamageEnemy() {
			
			Unschedule(Blink);
			Unschedule(OpenEyes);
			
			if ( !enemyCantBeDamagedForShortInterval ) {
				
				damageLevel ++;
				enemyCantBeDamagedForShortInterval = true;

				ScheduleOnce(EnemyCanBeDamagedAgain, 1.0f);

				if ( differentSpritesForDamage ) {

					//GameSounds.SharedGameSounds.PlayVoiceSoundFX("enemyGrunt.mp3");  //that sound file doesn't exist

					sprite.Texture =  new CCSprite(String.Format("{0}_damage{1}.png", baseImageName, damageLevel)).Texture;
				}
				
				
				if ( damageLevel == breaksAfterHowMuchContact ) {
					
					BreaksOnNextDamage = true;
				}
				
			}
			
			
		}

		void EnemyCanBeDamagedAgain(float delta) 
		{
			
			enemyCantBeDamagedForShortInterval = false;
		}
		
		
		public void BreakEnemy ()
		{
			
			
			Unschedule(Blink);
			Unschedule(OpenEyes);
			
			Schedule(StartBreakAnimation, 1.0f/30.0f);
			
			
		}

		void StartBreakAnimation(float delta)
		{ 
			
			if ( currentFrame == 0) {
				
				RemoveBody();
			}
			
			currentFrame ++; //adds 1 every frame
			
			if (currentFrame <= framesToAnimateOnBreak ) {  //if we included frames to show for breaking and the current frame is less than the max number of frames to play
				
				if (currentFrame < 10) {
					
					sprite.Texture =  new CCSprite(String.Format("{0}_break000{1}.png", baseImageName, currentFrame)).Texture;

				} else if (currentFrame < 100) { 
					
					sprite.Texture =  new CCSprite(String.Format("{0}_break00{1}.png", baseImageName, currentFrame)).Texture;
				}
				
			}
			
			if (currentFrame > framesToAnimateOnBreak ) { 
				
				//if the currentFrame equals the number of frames to animate, we remove the sprite OR if
				// the stackObject didn't include animated images for breaking
				
				RemoveSprite();
				Unschedule(StartBreakAnimation);
				
			}
			
			
		}

		void Blink(float delta)
		{
			
			sprite.Texture = new CCSprite(String.Format("{0}_blink.png", baseImageName)).Texture;
			
			Unschedule(Blink);
			Schedule(OpenEyes, 0.5f);
		}
		
		void OpenEyes(float delta)
		{
			
			sprite.Texture = new CCSprite(String.Format("{0}.png", baseImageName)).Texture;
			
			Unschedule(OpenEyes);
			
			var blinkInterval = Cocos2D.CCRandom.Next(3,8);//   random.Next(3,8); // range 3 to 8
			Schedule(Blink,blinkInterval);
		}

		
		public void MakeUnScoreable() {
			
			PointValue = 0;
			CCLog.Log("points have been accumulated for this object");
		}


	}
}

