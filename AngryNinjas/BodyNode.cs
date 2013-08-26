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
	public class BodyNode : CCNode
	{
		protected b2Body body;
		protected CCSprite sprite;

		public b2Body Body { get { return body; } }
		
		public CCSprite Sprite { get { return sprite; } }

		protected void CreateBodyWithSpriteAndFixture (b2World world, b2BodyDef bodyDef,
		                                               b2FixtureDef fixtureDef, string spriteName)
		{
			// this is the meat of our class, it creates (OR recreates) the body in the world with the body definition, fixture definition and sprite name
			
			RemoveBody(); //if remove the body if it already exists
			RemoveSprite(); //if remove the sprite if it already exists
			
			sprite = new CCSprite(spriteName);
			AddChild(sprite);
			
			body = world.CreateBody(bodyDef);
			body.UserData = this;
			
			if ( fixtureDef != null) 
			{
				
				body.CreateFixture(fixtureDef);
			}

		}

		public void FadeThenRemove(float delta)
		{
			
			var seq = new CCSequence(
			                   new CCFadeTo (1.0f,0),          
			                   new CCCallFunc(RemoveSpriteAndBody));
			sprite.RunAction(seq);
		}
		
		
		public void MakeBodyStatic() {
			
			body.SetType(b2BodyType.b2_staticBody);

		}

		public void RemoveBody() 
		{
			
			if( body != null) 
			{
				body.World.DestroyBody(body);
				body = null;
				
			}
			
		}
		
		
		public void RemoveSprite()  
		{
			
			if (sprite != null) {

				RemoveAllChildrenWithCleanup(true);
				sprite = null;
				
			}
			
		}

		public void RemoveSpriteAndBody()  {
			
			CCLog.Log(@"removing sprite and body");

			RemoveSprite();
			RemoveBody();

			RemoveFromParentAndCleanup(true);
		}

	}
}

