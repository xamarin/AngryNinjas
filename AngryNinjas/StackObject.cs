using System;

using Box2D;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Dynamics.Contacts;
using Box2D.Dynamics.Joints;

using cocos2d;

namespace AngryNinjas
{
	public class StackObject : BodyNode
	{
		b2World theWorld;
		string spriteImageName;
		string baseImageName;
		CCPoint initialLocation;
		
		bool addedAnimatedBreakFrames;
		public bool breaksOnGroundContact { get; set; }
		public bool breaksOnNinjaContact { get; set; }
		public bool canDamageEnemy { get; set; }
		public bool isStatic { get; set; }  //will always be static
		float theDensity;
		int shapeCreationMethod;
		
		int angle;
		
		int currentFrame;
		int framesToAnimate;
		
		public int  pointValue { get; set; }
		public int  simpleScoreVisualFX { get; set; } //defined in Constants.cs

		public StackObject (b2World world, 
		                    CCPoint location,
		                    string spriteFileName,
		                    bool breaksOnGround,
		                    bool breaksFromNinja,
		                    bool hasAnimatedBreakFrames,
		                    bool damagesEnemy,
		                    float density,
		                    int createHow,
		                    int angleChange,
		                    bool makeImmovable,
		                    int points,
		                    int simpleScoreVisualFXType)
		{
			InitWithWorld(world, 
			              location, 
			              spriteFileName, 
			              breaksOnGround,
			              breaksFromNinja,
			              hasAnimatedBreakFrames,
			              damagesEnemy,
			              density,
			              createHow,
			              angleChange,
			              makeImmovable,
			              points,
			              simpleScoreVisualFXType);
		}

		void InitWithWorld(b2World world, 
		                   CCPoint location,
		                   string spriteFileName,
		                   bool breaksOnGround,
		                   bool breaksFromNinja,
		                   bool hasAnimatedBreakFrames,
		                   bool damagesEnemy,
		                   float density,
		                   int createHow,
		                   int angleChange,
		                   bool makeImmovable,
		                   int points,
		                   int simpleScoreVisualFXType)
		{
			this.theWorld = world;
			this.initialLocation = location;
			this.baseImageName = spriteFileName;
			this.spriteImageName =  String.Format("{0}.png", baseImageName);
			
			this.breaksOnGroundContact = breaksOnGround; 
			this.breaksOnNinjaContact = breaksFromNinja; 
			this.addedAnimatedBreakFrames = hasAnimatedBreakFrames;
			this.canDamageEnemy = damagesEnemy;
			this.theDensity = density;
			this.shapeCreationMethod = createHow;
			this.angle = angleChange;
			this.isStatic = makeImmovable;
			
			currentFrame = 0;
			framesToAnimate = 10;
			
			this.pointValue = points ;
			this.simpleScoreVisualFX = simpleScoreVisualFXType;
			
			CreateObject();
				
		}

		void CreateObject()
		{
			
			
			// Define the dynamic body.
			b2BodyDef bodyDef = b2BodyDef.Create();
			bodyDef.type = b2BodyType.b2_dynamicBody; //or you could use b2_staticBody

			bodyDef.position.Set(initialLocation.X/Constants.PTM_RATIO, initialLocation.Y/Constants.PTM_RATIO);
			
			b2PolygonShape shape = new b2PolygonShape();
			b2CircleShape shapeCircle = new b2CircleShape();
			
			if (shapeCreationMethod == Constants.useDiameterOfImageForCircle) {
				
				CCSprite tempSprite = new CCSprite(spriteImageName);
				float radiusInMeters = (tempSprite.ContentSize.Width / Constants.PTM_RATIO) * 0.5f;
				
				shapeCircle.Radius = radiusInMeters;
				
			}
			
			
			else if ( shapeCreationMethod == Constants.useShapeOfSourceImage) {
				
				CCSprite tempSprite = new CCSprite(spriteImageName);
				
				int num = 4;
				b2Vec2[] vertices = {
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //top left corner
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom left corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 )/ Constants.PTM_RATIO), //bottom right corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO) //top right corner
				};
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == Constants.useTriangle) {
				CCSprite tempSprite = new CCSprite(spriteImageName);
				
				int num = 3;
				b2Vec2[] vertices = {
					new b2Vec2((tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom left corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom right corner
					new b2Vec2( 0.0f / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 )/ Constants.PTM_RATIO) // top center of image
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == Constants.useTriangleRightAngle) {
				CCSprite tempSprite = new CCSprite(spriteImageName);
				
				int num = 3;
				b2Vec2[] vertices = {
					new b2Vec2((tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO),  //top right corner
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //top left corner
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 )/ Constants.PTM_RATIO) //bottom left corner
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == Constants.useTrapezoid) {
				CCSprite tempSprite = new CCSprite(spriteImageName);
				
				int num = 4;
				b2Vec2[] vertices = {
					new b2Vec2((tempSprite.ContentSize.Width / 4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO),  //top of image, 3/4's across
					new b2Vec2( (tempSprite.ContentSize.Width / -4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO),  //top of image, 1/4's across
					new b2Vec2((tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom left corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom right corner
				};
				
				shape.Set(vertices, num);
			}
			
			
			else if ( shapeCreationMethod == Constants.useHexagon) {
				
				CCSprite tempSprite = new CCSprite(spriteImageName);
				
				int num = 6;
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
			
			else if ( shapeCreationMethod == Constants.usePentagon) {
				
				CCSprite tempSprite = new CCSprite(spriteImageName);
				
				int num = 5;
				b2Vec2[] vertices = {
					new b2Vec2( 0 / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //top of image, center 
					new b2Vec2( (tempSprite.ContentSize.Width / -2 )  / Constants.PTM_RATIO, 0.0f / Constants.PTM_RATIO), // left, center
					new b2Vec2( (tempSprite.ContentSize.Width / -4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom of image, 1/4 across
					new b2Vec2( (tempSprite.ContentSize.Width / 4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom of image, 3/4's across
					new b2Vec2( (tempSprite.ContentSize.Width /  2 ) / Constants.PTM_RATIO, 0.0f / Constants.PTM_RATIO), // right, center
					
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == Constants.useOctagon) {
				
				CCSprite tempSprite = new CCSprite(spriteImageName);
				
				int num = 8;
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
			else if ( shapeCreationMethod == Constants.useParallelogram) {
				
				CCSprite tempSprite = new CCSprite(spriteImageName);
				
				int num = 4;
				b2Vec2[] vertices = {
					new b2Vec2( (tempSprite.ContentSize.Width / -4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //top of image, 1/4 across
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom left corner
					new b2Vec2( (tempSprite.ContentSize.Width / 4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom of image, 3/4's across
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO) //top right corner
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == Constants.customCoordinates1) {  //use your own custom coordinates from a program like Vertex Helper Pro
				
				int num = 4;
				b2Vec2[] vertices = {
					new b2Vec2(-64.0f / Constants.PTM_RATIO, 16.0f / Constants.PTM_RATIO),
					new b2Vec2(-64.0f / Constants.PTM_RATIO, -16.0f / Constants.PTM_RATIO),
					new b2Vec2(64.0f / Constants.PTM_RATIO, -16.0f / Constants.PTM_RATIO),
					new b2Vec2(64.0f / Constants.PTM_RATIO, 16.0f / Constants.PTM_RATIO)
				};
				shape.Set(vertices, num);
			}
			
			
			
			
			// Define the dynamic body fixture.
			b2FixtureDef fixtureDef = b2FixtureDef.Create();
			
			if ( shapeCreationMethod == Constants.useDiameterOfImageForCircle) {
				
				fixtureDef.shape = shapeCircle;	
				
			} else {
				fixtureDef.shape = shape;	
				
			}
			
			fixtureDef.density = theDensity;
			fixtureDef.friction = 0.3f;
			fixtureDef.restitution =  0.1f;
			
			CreateBodyWithSpriteAndFixture(theWorld, bodyDef, fixtureDef, spriteImageName);
			
			
			if ( angle != 0 ) {
				
				int currentAngle = (int)body.Angle ;
				b2Vec2 locationInMeters = body.Position;
				body.SetTransform( locationInMeters , CCMacros.CCDegreesToRadians( currentAngle + angle  )  );
				
			}
			
			if (isStatic) {
				
				MakeBodyStatic();
			}
		}

		public void PlayBreakAnimationFromNinjaContact ()
		{
			
			if ( breaksOnNinjaContact) 
			{
				
				Schedule(StartBreakAnimation, 1.0f/30.0f);
			}
			
			
		}
		
		public void PlayBreakAnimationFromGroundContact()
		{
			
			if ( breaksOnGroundContact) {
				
				Schedule(StartBreakAnimation, 1.0f/30.0f);
			}
			
		}
		
		public void StartBreakAnimation (float delta) 
		{ 
			
			if ( currentFrame == 0) {
				
				this.RemoveBody();
				
				
			}
			
			currentFrame ++; //adds 1 every frame
			
			if (currentFrame <= framesToAnimate && addedAnimatedBreakFrames ) 
			{  //if we included frames to show for breaking and the current frame is less than the max number of frames to play
				
				if (currentFrame < 10) {

					sprite.Texture = new CCSprite(String.Format("{0}_000{1}.png", baseImageName, currentFrame)).Texture;

				} else if (currentFrame < 100) { 
					
					sprite.Texture = new CCSprite(String.Format("{0}_00{1}.png", baseImageName, currentFrame)).Texture;

				}
				
			}
			
			if (currentFrame > framesToAnimate || !addedAnimatedBreakFrames) { 
				
				//if the currentFrame equals the number of frames to animate, we remove the sprite OR if
				// the stackObject didn't include animated images for breaking
				
				this.RemoveSpriteAndBody();
				Unschedule(StartBreakAnimation);
				
			}
			
			
		}
		
		public void MakeUnScoreable() {
			
			pointValue = 0;
			CCLog.Log("points have been accumulated for this object");
		}

	}
}

