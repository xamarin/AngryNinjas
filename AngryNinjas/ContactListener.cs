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
	public class ContactListener : b2ContactListener
	{
		public ContactListener ()
		{
		}

		public override void BeginContact (b2Contact contact)
		{
			//base.BeginContact (contact);

			b2Body bodyA = contact.GetFixtureA().Body;
			b2Body bodyB = contact.GetFixtureB().Body;
			BodyNode bodyNodeA = (BodyNode)bodyA.UserData;
			BodyNode bodyNodeB = (BodyNode)bodyB.UserData;

			////////////////////////////////////////
			////////////////////////////////////////
			//NINJA NODES WITH GROUNDPLANE
			
			if (bodyNodeA is Ninja && bodyNodeB is GroundPlane)
			{
				
				Ninja theNinja = (Ninja)bodyNodeA;
				//GroundPlane* theGroundPlane = (GroundPlane*)bodyNodeB;
				
				
				TheLevel.SharedLevel.StopDotting();
				TheLevel.SharedLevel.ShowNinjaOnGround(theNinja);
				TheLevel.SharedLevel.ProceedToNextTurn(theNinja);

			} 
			else  if (bodyNodeA is GroundPlane && bodyNodeB is Ninja)
			{
				
				Ninja theNinja = (Ninja)bodyNodeB;
				// GroundPlane* theGroundPlane = (GroundPlane*)bodyNodeA;

				TheLevel.SharedLevel.StopDotting();
				TheLevel.SharedLevel.ShowNinjaOnGround(theNinja);
				TheLevel.SharedLevel.ProceedToNextTurn(theNinja);
				
			}
			
			////////////////////////////////////////
			////////////////////////////////////////
			//NINJA NODES WITH StackObject
			
			if (bodyNodeA is Ninja && bodyNodeB is StackObject)
			{

				//[[GameSounds sharedGameSounds] playStackImpactSound];

				Ninja theNinja = (Ninja)bodyNodeA;
				StackObject theStackObject = (StackObject)bodyNodeB;
				
				TheLevel.SharedLevel.StopDotting();
				TheLevel.SharedLevel.ShowNinjaImpactingStack(theNinja);

				theStackObject.PlayBreakAnimationFromNinjaContact();

				if (theStackObject.PointValue != 0) { // if it has a score value for impact with Ninja

					TheLevel.SharedLevel.ShowPoints(theStackObject.PointValue, theStackObject.Position, theStackObject.SimpleScoreVisualFX);  //show points
					theStackObject.MakeUnScoreable(); //prevents scoring off same object twice
				}

				
			} 
			else  if (bodyNodeA is StackObject && bodyNodeB is Ninja)
			{
				
				//[[GameSounds sharedGameSounds] playStackImpactSound];
				
				Ninja theNinja = (Ninja)bodyNodeB;
				StackObject theStackObject = (StackObject)bodyNodeA;
				
				TheLevel.SharedLevel.StopDotting();
				TheLevel.SharedLevel.ShowNinjaImpactingStack(theNinja);
				
				theStackObject.PlayBreakAnimationFromNinjaContact();
				
				if (theStackObject.PointValue != 0) { // if it has a score value for impact with Ninja
					
					TheLevel.SharedLevel.ShowPoints(theStackObject.PointValue, theStackObject.Position,  theStackObject.SimpleScoreVisualFX); //show points
					theStackObject.MakeUnScoreable();  //prevents scoring off same object twice
					
				}
				
			}
			
			
			////////////////////////////////////////
			////////////////////////////////////////
			//NINJA NODES WITH Enemy
			
			if (bodyNodeA is Ninja && bodyNodeB is Enemy)
			{
				
				Ninja theNinja = (Ninja)bodyNodeA;
				Enemy theEnemy = (Enemy)bodyNodeB;
				
				TheLevel.SharedLevel.StopDotting();
				TheLevel.SharedLevel.ShowNinjaImpactingStack(theNinja);  //applies to stack objects or enemies
				
				
				
				if ( theEnemy.BreaksOnNextDamage ) {
					
					if (theEnemy.PointValue != 0) { // if it has a score value for impact with Ninja
						
						TheLevel.SharedLevel.ShowPoints(theEnemy.PointValue, theEnemy.Position, theEnemy.SimpleScoreVisualFX); //show points
						theEnemy.MakeUnScoreable();  //prevents scoring off same object twice
						
					}
					theEnemy.BreakEnemy();
					
				} else {
					
					theEnemy.DamageEnemy();
					
				}
				
				
				
				
				
			} 
			else  if (bodyNodeA is Enemy && bodyNodeB is Ninja)
			{
				
				Ninja theNinja = (Ninja)bodyNodeB;
				Enemy theEnemy = (Enemy)bodyNodeA;
				
				TheLevel.SharedLevel.StopDotting();
				TheLevel.SharedLevel.ShowNinjaImpactingStack(theNinja ); //applies to stack objects or enemies
				
				
				if ( theEnemy.BreaksOnNextDamage) {
					
					if (theEnemy.PointValue != 0) { // if it has a score value for impact with Ninja

						TheLevel.SharedLevel.ShowPoints(theEnemy.PointValue, theEnemy.Position, theEnemy.SimpleScoreVisualFX); //show points
						theEnemy.MakeUnScoreable();  //prevents scoring off same object twice
						
					}
					theEnemy.BreakEnemy();
					
				} else {
					
					theEnemy.DamageEnemy();
					
				}
				
			}
			
			
			////////////////////////////////////////
			////////////////////////////////////////
			//StackObject WITH Enemy
			
			if ( bodyNodeA is StackObject && bodyNodeB is Enemy)
			{
				
				StackObject theStackObject = (StackObject)bodyNodeA;
				Enemy theEnemy = (Enemy)bodyNodeB;
				
				if (theStackObject.IsCanDamageEnemy && theEnemy.DamagesFromDamageEnabledStackObjects ) {
					
					if ( theEnemy.BreaksOnNextDamage ) {
						
						if (theEnemy.PointValue != 0) { // if it has a score value for impact with Ninja

							TheLevel.SharedLevel.ShowPoints(theEnemy.PointValue, theEnemy.Position, theEnemy.SimpleScoreVisualFX); //show points

							theEnemy.MakeUnScoreable();  //prevents scoring off same object twice
							
						}
						theEnemy.BreakEnemy();
						
					} else {
						
						theEnemy.DamageEnemy();
						
					}
					
					
				}
				
				
			} 
			else  if ( bodyNodeA is Enemy  && bodyNodeB is StackObject )
			{
				
				StackObject theStackObject = (StackObject)bodyNodeB;
				Enemy theEnemy = (Enemy)bodyNodeA;
				
				if (theStackObject.IsCanDamageEnemy && theEnemy.DamagesFromDamageEnabledStackObjects ) {
					
					if ( theEnemy.BreaksOnNextDamage ) {
						
						if (theEnemy.PointValue != 0) { // if it has a score value for impact with Ninja

							TheLevel.SharedLevel.ShowPoints(theEnemy.PointValue, theEnemy.Position, theEnemy.SimpleScoreVisualFX); //show points

							theEnemy.MakeUnScoreable();  //prevents scoring off same object twice
							
						}
						theEnemy.BreakEnemy();
						
					} else {
						
						theEnemy.DamageEnemy();
						
					}
					
					
				}
				
				
			}
			
			////////////////////////////////////////
			////////////////////////////////////////
			//Ground WITH Enemy
			
			if (bodyNodeA is GroundPlane  && bodyNodeB is Enemy )
			{
				
				
				Enemy theEnemy = (Enemy)bodyNodeB;
				
				if ( theEnemy.DamagesFromGroundContact ) {
					
					if ( theEnemy.BreaksOnNextDamage ) {
						
						if (theEnemy.PointValue != 0) { // if it has a score value for impact with Ninja

							TheLevel.SharedLevel.ShowPoints(theEnemy.PointValue, theEnemy.Position, theEnemy.SimpleScoreVisualFX); //show points

							theEnemy.MakeUnScoreable();  //prevents scoring off same object twice
							
						}
						theEnemy.BreakEnemy();
						
					} else {
						
						theEnemy.DamageEnemy();
						
					}
					
					
				}
				
				
			} 
			else  if (bodyNodeA is Enemy  && bodyNodeB is GroundPlane )
			{
				
				
				Enemy theEnemy = (Enemy)bodyNodeA;
				
				if ( theEnemy.DamagesFromGroundContact ) {
					
					if ( theEnemy.BreaksOnNextDamage ) {
						
						if (theEnemy.PointValue != 0) { // if it has a score value for impact with Ninja

							TheLevel.SharedLevel.ShowPoints(theEnemy.PointValue, theEnemy.Position, theEnemy.SimpleScoreVisualFX); //show points

							theEnemy.MakeUnScoreable();  //prevents scoring off same object twice
							
						}
						theEnemy.BreakEnemy();
						
					} else {
						
						theEnemy.DamageEnemy();
						
					}
					
					
				}
				
				
			}
			
			
			
			
			////////////////////////////////////////
			////////////////////////////////////////
			//Ground WITH StackObject
			
			if (bodyNodeA is GroundPlane  && bodyNodeB is StackObject )
			{
				
				
				
				StackObject theStackObject = (StackObject)bodyNodeB;
				
				theStackObject.PlayBreakAnimationFromGroundContact();
				
				if (theStackObject.PointValue != 0 && theStackObject.IsBreaksOnGroundContact) 
				{ // if it has a score value for impact with Ninja
					
					TheLevel.SharedLevel.ShowPoints(theStackObject.PointValue, theStackObject.Position, theStackObject.SimpleScoreVisualFX);  //show points
					
					
					theStackObject.MakeUnScoreable(); //prevents scoring off same object twice
				}
				
				
			} 
			else  if (bodyNodeA is StackObject  && bodyNodeB is GroundPlane )
			{
				

				StackObject theStackObject = (StackObject)bodyNodeA;
				
				theStackObject.PlayBreakAnimationFromGroundContact();
				
				if (theStackObject.PointValue != 0 && theStackObject.IsBreaksOnGroundContact) { // if it has a score value for impact with Ninja
					
					TheLevel.SharedLevel.ShowPoints(theStackObject.PointValue, theStackObject.Position, theStackObject.SimpleScoreVisualFX);  //show points
					theStackObject.MakeUnScoreable();  //prevents scoring off same object twice
					
				}
				
			}

		}

		public override void PreSolve (b2Contact contact, b2Manifold oldManifold)
		{
			//throw new NotImplementedException ();
		}

		public override void PostSolve (b2Contact contact, ref b2ContactImpulse impulse)
		{
			//throw new NotImplementedException ();
		}

		public override void EndContact (b2Contact contact)
		{
			base.EndContact (contact);
		}

	}
}

