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
    public class TheStack : CCNode
    {
        b2World world;
        int currentLevel;

        int stackLocationX;  //starting location of stack on X
        int stackLocationY; //starting location of stack on Y

        int stackAdjustmentX;  //use for adjusting on a per level basis
        int stackAdjustmentY; //use for adjusting on a per level basis

        public TheStack(b2World theWorld)
        {
            InitStackWithWorld(theWorld);
        }

        void InitStackWithWorld(b2World theWorld)
        {
            this.world = theWorld;

            if (TheLevel.SharedLevel.IS_IPAD)
            {
                stackLocationX = 1400;  //base X starting point for the entire stack on the iPad (make further tweaks using the  stackAdjustmentX var in the buildLevel function per level
                stackLocationY = 100; //base Y starting point for the entire stack on the iPad (make further tweaks using the  stackAdjustmentY var in the buildLevel function per level
            }
            else
            {

                stackLocationX = 900;  //base X starting point for the entire stack on the iPhone (make further tweaks using the  stackAdjustmentX var in the buildLevel function per level
                stackLocationY = 35; //base Y starting point for the entire stack on the iPhone (make further tweaks using the  stackAdjustmentY var in the buildLevel function per level
            }

            currentLevel = GameData.SharedData.Level;
            if (currentLevel % 2 == 0)
            {
                BuildLevel2();
            }
            else
            {
                BuildLevel1();
            }
        }

        void BuildTestLevel()
        {

            if (TheLevel.SharedLevel.IS_IPAD)
            {

                stackAdjustmentX = 0; // if you want to further adjust the stack's starting X location then change this value  (ipad)
                stackLocationX = stackLocationX - stackAdjustmentX;

                stackAdjustmentY = 0; // if you want to further adjust the stack's starting X location then change this value  (iphone)
                stackLocationY = stackLocationY - stackAdjustmentY;


            }
            else
            {

                stackAdjustmentX = 0; // if you want to further adjust the stack's starting X location then change this value  (iphone)
                stackLocationX = stackLocationX - stackAdjustmentX;

                stackAdjustmentY = 0; // if you want to further adjust the stack's starting X location then change this value  (iphone)
                stackLocationY = stackLocationY - stackAdjustmentY;


            }


            #region woodshape1
            var object1 = new StackObject(world,
                                          new CCPoint(0 + stackLocationX, 65 + stackLocationY),
                                          "woodShape1",
                                          false,
                                          true,
                                          true,
                                          false,
                                          0.25f,
                                          CreationMethod.ShapeOfSourceImage,
                                          90,
                                          false,
                                          100,
                                          BreakEffect.SmokePuffs);
            AddChild(object1, Constants.DepthStack);

            object1 = new StackObject(world,
                                                      new CCPoint(95 + stackLocationX, 65 + stackLocationY),
                                                      "woodShape1",
                                                      false,
                                                      true,
                                                      true,
                                                      false,
                                                      0.25f,
                                                      CreationMethod.ShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      BreakEffect.SmokePuffs);
            AddChild(object1, Constants.DepthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(47 + stackLocationX, 145 + stackLocationY),
                                                      "woodShape1",
                                                      false,
                                                      true,
                                                      true,
                                                      false,
                                                      0.25f,
                                                      CreationMethod.ShapeOfSourceImage,
                                                      0,
                                                      false,
                                                      100,
                                                      BreakEffect.SmokePuffs);
            AddChild(object1, Constants.DepthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(0 + stackLocationX, 225 + stackLocationY),
                                                      "woodShape1",
                                                      false,
                                                      true,
                                                      true,
                                                      false,
                                                      0.25f,
                                                      CreationMethod.ShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      BreakEffect.SmokePuffs);
            AddChild(object1, Constants.DepthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(95 + stackLocationX, 225 + stackLocationY),
                                                      "woodShape1",
                                                      false,
                                                      true,
                                                      true,
                                                      false,
                                                      0.25f,
                                                      CreationMethod.ShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      BreakEffect.SmokePuffs);
            AddChild(object1, Constants.DepthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(50 + stackLocationX, 305 + stackLocationY),
                                                      "woodShape1",
                                                      false,
                                                      true,
                                                      true,
                                                      false,
                                                      0.25f,
                                                      CreationMethod.ShapeOfSourceImage,
                                                      0,
                                                      false,
                                                      100,
                                                      BreakEffect.SmokePuffs);
            AddChild(object1, Constants.DepthStack);
            #endregion
            #region Medium Triangle Test Object
            var object8 = new StackObject(world,
                                          new CCPoint(0 + stackLocationX, 300 + stackLocationY),
                                          "triangleMedium",
                                          false,
                                          true,
                                          false,
                                          false,
                                          0.25f,
                                          CreationMethod.Triangle,
                                          0,
                                          false,
                                          100,
                                          BreakEffect.Explosion);
            AddChild(object8, Constants.DepthStack);
            object8 = new StackObject(world,
                                          new CCPoint(95 + stackLocationX, 345 + stackLocationY),
                                          "triangleMedium",
                                          false,
                                          true,
                                          false,
                                          false,
                                          0.25f,
                                          CreationMethod.Triangle,
                                          0,
                                          false,
                                          100,
                                          BreakEffect.Explosion);
            AddChild(object8, Constants.DepthStack);
            #endregion
        }

        void BuildLevel1()
        {
            if (TheLevel.SharedLevel.IS_IPAD)
            {

                stackAdjustmentX = -350; // if you want to further adjust the stack's starting X location then change this value  (ipad)
                stackLocationX = stackLocationX - stackAdjustmentX;

                stackAdjustmentY = 0; // if you want to further adjust the stack's starting X location then change this value  (iphone)
                stackLocationY = stackLocationY - stackAdjustmentY;


            }
            else
            {
#if ANDROID
                stackAdjustmentX = 100; // if you want to further adjust the stack's starting X location then change this value  (iphone)
#else
                stackAdjustmentX = -100; // if you want to further adjust the stack's starting X location then change this value  (iphone)
#endif
                stackLocationX = stackLocationX - stackAdjustmentX;


                stackAdjustmentY = 0; // if you want to further adjust the stack's starting X location then change this value  (iphone)
                stackLocationY = stackLocationY - stackAdjustmentY;

            }

                StackObject object1 = new StackObject(world , new CCPoint( 0 + stackLocationX , 65 + stackLocationY) , "woodShape1" , false, true  , true  , false , 0.25f , CreationMethod.ShapeOfSourceImage , 90 , false , 100 , BreakEffect.SmokePuffs);
   AddChild(object1, Constants.DepthStack);
    
    StackObject object2 = new StackObject(world , new CCPoint(95 + stackLocationX , 65 + stackLocationY) , "woodShape1" , false, true  , true , false , 0.25f , CreationMethod.ShapeOfSourceImage , 90 , false , 100 , BreakEffect.SmokePuffs);
   AddChild(object2, Constants.DepthStack);
    
    StackObject object3 = new StackObject(world , new CCPoint(47 + stackLocationX, 145 + stackLocationY) , "woodShape1" , false, true  , true , false , 0.25f , CreationMethod.ShapeOfSourceImage , 0 , false , 100 , BreakEffect.Explosion);
   AddChild(object3, Constants.DepthStack);
    
    StackObject object4 = new StackObject(world , new CCPoint( 0 + stackLocationX, 225 + stackLocationY) , "woodShape1" , false, true  , true , false , 0.25f , CreationMethod.ShapeOfSourceImage , 90 , false , 100 , BreakEffect.Explosion);
   AddChild(object4, Constants.DepthStack);
    
    StackObject object5 = new StackObject(world , new CCPoint(95 + stackLocationX, 225 + stackLocationY) , "woodShape1" , false, true  , true , false , 0.25f , CreationMethod.ShapeOfSourceImage , 90 , false , 100 , BreakEffect.Explosion);
   AddChild(object5, Constants.DepthStack);
    
    StackObject object6 = new StackObject(world , new CCPoint(50 + stackLocationX, 305 + stackLocationY) , "woodShape1" , false, true  , true , false , 0.25f , CreationMethod.ShapeOfSourceImage , 0 , false , 100 , BreakEffect.SmokePuffs);
   AddChild(object6, Constants.DepthStack);
    
    StackObject object7 = new StackObject(world , new CCPoint(0 + stackLocationX , 345 + stackLocationY) , "triangleMedium" , false, true  , false , true , 0.25f , CreationMethod.Triangle , 0 , false , 100 , BreakEffect.SmokePuffs) ;
   AddChild(object7, Constants.DepthStack);
    
    StackObject object8 = new StackObject(world , new CCPoint(95 + stackLocationX, 345 + stackLocationY) , "triangleMedium" , false, true , false , true , 0.25f , CreationMethod.Triangle , 0 , false , 100 , BreakEffect.Explosion);
   AddChild(object8, Constants.DepthStack);
    
    StackObject object9 = new StackObject(world , new CCPoint(50 + stackLocationX, 350 + stackLocationY) , "triangleLarge" , false, true , false , true , 0.25f , CreationMethod.Triangle , 180 , false , 500 , BreakEffect.SmokePuffs);
   AddChild(object9, Constants.DepthStack);
    
    StackObject object10 = new StackObject(world , new CCPoint(25 + stackLocationX, 394 + stackLocationY) , "triangleSmall" , false, true , false , false , 0.25f , CreationMethod.Triangle , 0 , false , 100 , BreakEffect.Explosion);
   AddChild(object10, Constants.DepthStack);
    
    StackObject object11 = new StackObject(world , new CCPoint(75 + stackLocationX, 394 + stackLocationY) , "triangleSmall" , false, true , false , false , 0.25f , CreationMethod.Triangle , 0 , false , 100 , BreakEffect.SmokePuffs);
   AddChild(object11, Constants.DepthStack);

            Enemy enemy1 = new Enemy(world,
                                      new CCPoint(45 + stackLocationX, 200 + stackLocationY),
                                      "mutantPepper",
                                      true,
                                      true,
                                      true,
                                      1,
                                      true,
                                      10,
                                      1.0f,
                                      CreationMethod.ShapeOfSourceImageButSlightlySmaller,
                                      10000,
                                      BreakEffect.SmokePuffs);

            AddChild(enemy1, Constants.DepthStack);
        }


        void BuildLevel2()
        {
            if (TheLevel.SharedLevel.IS_IPAD)
            {

                stackAdjustmentX = 0; // if you want to further adjust the stack's starting X location then change this value  (ipad)
                stackLocationX = stackLocationX - stackAdjustmentX;

                stackAdjustmentY = 0; // if you want to further adjust the stack's starting X location then change this value  (iphone)
                stackLocationY = stackLocationY - stackAdjustmentY;


            }
            else
            {

#if ANDROID
                stackAdjustmentX = 200; // if you want to further adjust the stack's starting X location then change this value  (iphone)
#else
                stackAdjustmentX = -100; // if you want to further adjust the stack's starting X location then change this value  (iphone)
#endif
                stackLocationX = stackLocationX - stackAdjustmentX;


                stackAdjustmentY = 0; // if you want to further adjust the stack's starting X location then change this value  (iphone)
                stackLocationY = stackLocationY - stackAdjustmentY;

            }


            #region marble
            var object1 = new StackObject(world,
                                          new CCPoint(40 + stackLocationX, 40 + stackLocationY),
                                          "marbleSquare",
                                          false,
                                          false,
                                          false,
                                          false,
                                          0.25f,
                                          CreationMethod.ShapeOfSourceImage,
                                          90,
                                          true,
                                          0,
                                          BreakEffect.None);
            AddChild(object1, Constants.DepthStack);
            object1 = new StackObject(world,
                                          new CCPoint(190 + stackLocationX, 40 + stackLocationY),
                                          "marbleSquare",
                                          false,
                                          false,
                                          false,
                                          false,
                                          0.25f,
                                          CreationMethod.ShapeOfSourceImage,
                                          90,
                                          true,
                                          0,
                                          BreakEffect.None);
            AddChild(object1, Constants.DepthStack);
            #endregion
            #region stone pillars
            object1 = new StackObject(world,
                                          new CCPoint(9 + stackLocationX, 125 + stackLocationY),
                                          "stonePillar",
                                          false,
                                          false,
                                          false,
                                          false,
                                          0.25f,
                                          CreationMethod.ShapeOfSourceImage,
                                          90,
                                          true,
                                          0,
                                          BreakEffect.None);
            AddChild(object1, Constants.DepthStack);
            object1 = new StackObject(world,
                                          new CCPoint(27 + stackLocationX, 220 + stackLocationY),
                                          "stonePillar",
                                          false,
                                          false,
                                          false,
                                          false,
                                          0.25f,
                                          CreationMethod.ShapeOfSourceImage,
                                          90,
                                          true,
                                          0,
                                          BreakEffect.None);
            AddChild(object1, Constants.DepthStack);
            #endregion
            #region wood shapes
            object1 = new StackObject(world,
                                                      new CCPoint(113 + stackLocationX, 88 + stackLocationY),
                                                      "woodShape2",
                                                      false,
                                                      true,
                                                      false,
                                                      false,
                                                      0.25f,
                                                      CreationMethod.ShapeOfSourceImage,
                                                      0,
                                                      false,
                                                      100,
                                                      BreakEffect.Explosion);
            AddChild(object1, Constants.DepthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(57 + stackLocationX, 128 + stackLocationY),
                                                      "woodShape4",
                                                      false,
                                                      true,
                                                      false,
                                                      true,
                                                      0.25f,
                                                      CreationMethod.ShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      BreakEffect.Explosion);
            AddChild(object1, Constants.DepthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(114 + stackLocationX, 128 + stackLocationY),
                                                      "woodShape4",
                                                      false,
                                                      true,
                                                      false,
                                                      false,
                                                      0.25f,
                                                      CreationMethod.ShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      BreakEffect.Explosion);
            AddChild(object1, Constants.DepthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(168 + stackLocationX, 128 + stackLocationY),
                                                      "woodShape4",
                                                      false,
                                                      true,
                                                      false,
                                                      false,
                                                      0.25f,
                                                      CreationMethod.ShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      BreakEffect.Explosion);
            AddChild(object1, Constants.DepthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(124 + stackLocationX, 166 + stackLocationY),
                                                      "woodShape3",
                                                      false,
                                                      true,
                                                      false,
                                                      false,
                                                      0.25f,
                                                      CreationMethod.ShapeOfSourceImage,
                                                      0,
                                                      false,
                                                      100,
                                                      BreakEffect.Explosion);
            AddChild(object1, Constants.DepthStack);
            #endregion


            StackObject object11 = new StackObject(world, new CCPoint(45 + stackLocationX, 210 + stackLocationY), "woodShape4", false, true, false, true, 0.25f, CreationMethod.ShapeOfSourceImage, 90, false, 100, BreakEffect.Explosion);
            AddChild(object11, Constants.DepthStack);

            StackObject object12 = new StackObject(world, new CCPoint(95 + stackLocationX, 210 + stackLocationY), "woodShape4", false, true, false, true, 0.25f, CreationMethod.ShapeOfSourceImage, 90, false, 100, BreakEffect.Explosion);
            AddChild(object12, Constants.DepthStack);

            StackObject object13 = new StackObject(world, new CCPoint(145 + stackLocationX, 210 + stackLocationY), "woodShape4", false, true, false, true, 0.25f, CreationMethod.ShapeOfSourceImage, 90, false, 100, BreakEffect.Explosion);
            AddChild(object13, Constants.DepthStack);

            StackObject object14 = new StackObject(world, new CCPoint(200 + stackLocationX, 210 + stackLocationY), "woodShape4", false, true, false, true, 0.25f, CreationMethod.ShapeOfSourceImage, 90, false, 100, BreakEffect.Explosion);
            AddChild(object14, Constants.DepthStack);

            StackObject object15 = new StackObject(world, new CCPoint(80 + stackLocationX, 250 + stackLocationY), "stonePillar", false, false, false, true, .5f, CreationMethod.ShapeOfSourceImage, 90, false, 0, BreakEffect.None);
            AddChild(object15, Constants.DepthStack);

            StackObject object16 = new StackObject(world, new CCPoint(180 + stackLocationX, 250 + stackLocationY), "stonePillar", false, false, false, true, .5f, CreationMethod.ShapeOfSourceImage, 90, false, 0, BreakEffect.None);
            AddChild(object16, Constants.DepthStack);


            StackObject object17 = new StackObject(world, new CCPoint(95 + stackLocationX, 285 + stackLocationY), "triangleMedium", false, true, false, true, 0.25f, CreationMethod.Triangle, 0, false, 100, BreakEffect.Explosion);
            AddChild(object17, Constants.DepthStack);

            StackObject object18 = new StackObject(world, new CCPoint(181 + stackLocationX, 285 + stackLocationY), "triangleMedium", false, true, false, true, 0.25f, CreationMethod.Triangle, 0, false, 100, BreakEffect.Explosion);
            AddChild(object18, Constants.DepthStack);

            StackObject object19 = new StackObject(world, new CCPoint(138 + stackLocationX, 280 + stackLocationY), "triangleSmall", false, true, false, true, 0.25f, CreationMethod.Triangle, 180, false, 500, BreakEffect.Explosion);
            AddChild(object19, Constants.DepthStack);


            StackObject object5 = new StackObject(world, new CCPoint(137 + stackLocationX, 340 + stackLocationY), "stonePillar", false, false, false, true, .5f, CreationMethod.ShapeOfSourceImage, 0, false, 0, BreakEffect.None);
            AddChild(object5, Constants.DepthStack);



            Enemy enemy1 = new Enemy(world,
                                      new CCPoint(117 + stackLocationX, 45 + stackLocationY),
                                      "mutantPepper",
                                      true,
                                      false,
                                      true,
                                      3,
                                      true,
                                      10,
                                      1.0f,
                                      CreationMethod.ShapeOfSourceImage,
                                      10000,
                                      BreakEffect.SmokePuffs);

            AddChild(enemy1, Constants.DepthStack);
            enemy1 = new Enemy(world,
                                      new CCPoint(206 + stackLocationX, 120 + stackLocationY),
                                      "mutantPepper",
                                      false,
                                      true,
                                      true,
                                      3,
                                      true,
                                      10,
                                      1.0f,
                                      CreationMethod.ShapeOfSourceImageButSlightlySmaller,
                                      10000,
                                      BreakEffect.SmokePuffs);

            AddChild(enemy1, Constants.DepthStack);
        }



    }
}

