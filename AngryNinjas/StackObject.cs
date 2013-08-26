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
	public class StackObject : BodyNode
	{
		b2World theWorld;
		string spriteImageName;
		string baseImageName;
		CCPoint initialLocation;
		
		bool addedAnimatedBreakFrames;
		public bool IsBreaksOnGroundContact { get; set; }
		public bool IsBreaksOnNinjaContact { get; set; }
		public bool IsCanDamageEnemy { get; set; }
		public bool IsStatic { get; set; }  //will always be static
		float theDensity;
		CreationMethod shapeCreationMethod;
		
		int angle;
		
		int currentFrame;
		int framesToAnimate;
		
		public int  PointValue { get; set; }
		public BreakEffect  SimpleScoreVisualFX { get; set; } //defined in Constants.cs

		public StackObject (b2World world, 
		                    CCPoint location,
		                    string spriteFileName,
		                    bool breaksOnGround,
		                    bool breaksFromNinja,
		                    bool hasAnimatedBreakFrames,
		                    bool damagesEnemy,
		                    float density,
		                    CreationMethod createHow,
		                    int angleChange,
		                    bool makeImmovable,
		                    int points,
		                    BreakEffect simpleScoreVisualFXType)
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
		                   CreationMethod createHow,
		                   int angleChange,
		                   bool makeImmovable,
		                   int points,
		                   BreakEffect simpleScoreVisualFXType)
		{
			this.theWorld = world;
			this.initialLocation = location;
			this.baseImageName = spriteFileName;
			this.spriteImageName =  String.Format("{0}.png", baseImageName);
			
			this.IsBreaksOnGroundContact = breaksOnGround; 
			this.IsBreaksOnNinjaContact = breaksFromNinja; 
			this.addedAnimatedBreakFrames = hasAnimatedBreakFrames;
			this.IsCanDamageEnemy = damagesEnemy;
			this.theDensity = density;
			this.shapeCreationMethod = createHow;
			this.angle = angleChange;
			this.IsStatic = makeImmovable;
			
			this.currentFrame = 0;
			this.framesToAnimate = 10;
			
			this.PointValue = points ;
			this.SimpleScoreVisualFX = simpleScoreVisualFXType;
			
			CreateObject();
				
		}

		void CreateObject()
		{
			
			
			// Define the dynamic body.
			var bodyDef = new b2BodyDef();
			bodyDef.type = b2BodyType.b2_dynamicBody; //or you could use b2_staticBody

			bodyDef.position.Set(initialLocation.X/Constants.PTM_RATIO, initialLocation.Y/Constants.PTM_RATIO);
			
			var shape = new b2PolygonShape();
			var shapeCircle = new b2CircleShape();
			
			if (shapeCreationMethod == CreationMethod.DiameterOfImageForCircle) {
				
				var tempSprite = new CCSprite(spriteImageName);
				var radiusInMeters = (tempSprite.ContentSize.Width / Constants.PTM_RATIO) * 0.5f;
				
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
			
			else if ( shapeCreationMethod == CreationMethod.Triangle) {
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 3;
				b2Vec2[] vertices = {
					new b2Vec2((tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom left corner
					new b2Vec2( (tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom right corner
					new b2Vec2( 0.0f / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 )/ Constants.PTM_RATIO) // top center of image
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == CreationMethod.TriangleRightAngle) {
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 3;
				b2Vec2[] vertices = {
					new b2Vec2((tempSprite.ContentSize.Width / 2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO),  //top right corner
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO), //top left corner
					new b2Vec2( (tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 )/ Constants.PTM_RATIO) //bottom left corner
				};
				
				shape.Set(vertices, num);
			}
			
			else if ( shapeCreationMethod == CreationMethod.Trapezoid) {
				var tempSprite = new CCSprite(spriteImageName);
				
				var num = 4;
				b2Vec2[] vertices = {
					new b2Vec2((tempSprite.ContentSize.Width / 4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO),  //top of image, 3/4's across
					new b2Vec2( (tempSprite.ContentSize.Width / -4 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / 2 ) / Constants.PTM_RATIO),  //top of image, 1/4's across
					new b2Vec2((tempSprite.ContentSize.Width / -2 ) / Constants.PTM_RATIO, (tempSprite.ContentSize.Height / -2 ) / Constants.PTM_RATIO), //bottom left corner
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
			fixtureDef.restitution =  0.1f;
			
			CreateBodyWithSpriteAndFixture(theWorld, bodyDef, fixtureDef, spriteImageName);
			
			
			if ( angle != 0 ) {
				
				int currentAngle = (int)body.Angle ;
				b2Vec2 locationInMeters = body.Position;
				body.SetTransform( locationInMeters , CCMacros.CCDegreesToRadians( currentAngle + angle  )  );
				
			}
			
			if (IsStatic) {
				
				MakeBodyStatic();
			}
		}

		public void PlayBreakAnimationFromNinjaContact ()
		{
			
			if ( IsBreaksOnNinjaContact) 
			{
				
				Schedule(StartBreakAnimation, 1.0f/30.0f);
			}
			
			
		}
		
		public void PlayBreakAnimationFromGroundContact()
		{
			
			if ( IsBreaksOnGroundContact) {
				
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
		
		public void MakeUnScoreable() 
		{
			
			PointValue = 0;
			CCLog.Log("Points have been accumulated for this object");
		}

	}
}

