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
	public class StartPlatform : BodyNode
	{
		b2World theWorld;
		string spriteImageName;
		CCPoint initialLocation;

		public StartPlatform (b2World world, CCPoint location, string spriteFileName)
		{
			InitWithWorld(world, location, spriteFileName);
		}

		private void InitWithWorld (b2World world, CCPoint location, string spriteFileName) 
		{
			
			this.theWorld = world;
			this.initialLocation = location;
			this.spriteImageName = spriteFileName;
			
			
			CreatePlatform();
						
		}
		
		void CreatePlatform() {
			
			// Define the dynamic body.
			var bodyDef = new b2BodyDef();
			bodyDef.type = b2BodyType.b2_staticBody; //or you could use b2_staticBody
			
			bodyDef.position.Set(initialLocation.X/Constants.PTM_RATIO, initialLocation.Y/Constants.PTM_RATIO);

			var shape = new b2PolygonShape();
			
			var num = 4;
			b2Vec2[] vertices = {
				new b2Vec2(-102.0f / Constants.PTM_RATIO, -49.5f / Constants.PTM_RATIO),
				new b2Vec2(-113.0f / Constants.PTM_RATIO, -81.5f / Constants.PTM_RATIO),
				new b2Vec2(113.0f / Constants.PTM_RATIO, -84.5f / Constants.PTM_RATIO),
				new b2Vec2(106.0f / Constants.PTM_RATIO, -47.5f / Constants.PTM_RATIO)
			};
			
			shape.Set(vertices, num);
			
			
			// Define the dynamic body fixture.
			var fixtureDef = new b2FixtureDef();
			fixtureDef.shape = shape;	
			fixtureDef.density = 1.0f;
			fixtureDef.friction = 0.3f;
			fixtureDef.restitution =  0.1f;
			
			CreateBodyWithSpriteAndFixture(theWorld, bodyDef, fixtureDef, spriteImageName);
			
			//CONTINUING TO ADD BODY SHAPE....
			
			// THIS IS THE Sling base....
			
			//row 1, col 1
			var num2 = 4;
			b2Vec2[] vertices2 = {
				new b2Vec2(41.0f / Constants.PTM_RATIO, -6.5f / Constants.PTM_RATIO),
				new b2Vec2(35.0f / Constants.PTM_RATIO, -57.5f / Constants.PTM_RATIO),
				new b2Vec2(57.0f / Constants.PTM_RATIO, -65.5f / Constants.PTM_RATIO),
				new b2Vec2(49.0f / Constants.PTM_RATIO, -7.5f / Constants.PTM_RATIO)
			};
			
			shape.Set(vertices2, num2);
			fixtureDef.shape = shape;
			body.CreateFixture(fixtureDef);
			
		}

	}
}

