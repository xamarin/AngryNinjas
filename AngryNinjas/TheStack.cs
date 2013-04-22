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
                                          Constants.useShapeOfSourceImage,
                                          90,
                                          false,
                                          100,
                                          Constants.breakEffectSmokePuffs);
            AddChild(object1, Constants.depthStack);

            object1 = new StackObject(world,
                                                      new CCPoint(95 + stackLocationX, 65 + stackLocationY),
                                                      "woodShape1",
                                                      false,
                                                      true,
                                                      true,
                                                      false,
                                                      0.25f,
                                                      Constants.useShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      Constants.breakEffectSmokePuffs);
            AddChild(object1, Constants.depthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(47 + stackLocationX, 145 + stackLocationY),
                                                      "woodShape1",
                                                      false,
                                                      true,
                                                      true,
                                                      false,
                                                      0.25f,
                                                      Constants.useShapeOfSourceImage,
                                                      0,
                                                      false,
                                                      100,
                                                      Constants.breakEffectSmokePuffs);
            AddChild(object1, Constants.depthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(0 + stackLocationX, 225 + stackLocationY),
                                                      "woodShape1",
                                                      false,
                                                      true,
                                                      true,
                                                      false,
                                                      0.25f,
                                                      Constants.useShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      Constants.breakEffectSmokePuffs);
            AddChild(object1, Constants.depthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(95 + stackLocationX, 225 + stackLocationY),
                                                      "woodShape1",
                                                      false,
                                                      true,
                                                      true,
                                                      false,
                                                      0.25f,
                                                      Constants.useShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      Constants.breakEffectSmokePuffs);
            AddChild(object1, Constants.depthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(50 + stackLocationX, 305 + stackLocationY),
                                                      "woodShape1",
                                                      false,
                                                      true,
                                                      true,
                                                      false,
                                                      0.25f,
                                                      Constants.useShapeOfSourceImage,
                                                      0,
                                                      false,
                                                      100,
                                                      Constants.breakEffectSmokePuffs);
            AddChild(object1, Constants.depthStack);
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
                                          Constants.useTriangle,
                                          0,
                                          false,
                                          100,
                                          Constants.breakEffectExplosion);
            AddChild(object8, Constants.depthStack);
            object8 = new StackObject(world,
                                          new CCPoint(95 + stackLocationX, 345 + stackLocationY),
                                          "triangleMedium",
                                          false,
                                          true,
                                          false,
                                          false,
                                          0.25f,
                                          Constants.useTriangle,
                                          0,
                                          false,
                                          100,
                                          Constants.breakEffectExplosion);
            AddChild(object8, Constants.depthStack);
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

                StackObject object1 = new StackObject(world , new CCPoint( 0 + stackLocationX , 65 + stackLocationY) , "woodShape1" , false, true  , true  , false , 0.25f , Constants.useShapeOfSourceImage , 90 , false , 100 , Constants.breakEffectSmokePuffs);
   AddChild(object1, Constants.depthStack);
    
    StackObject object2 = new StackObject(world , new CCPoint(95 + stackLocationX , 65 + stackLocationY) , "woodShape1" , false, true  , true , false , 0.25f , Constants.useShapeOfSourceImage , 90 , false , 100 , Constants.breakEffectSmokePuffs);
   AddChild(object2, Constants.depthStack);
    
    StackObject object3 = new StackObject(world , new CCPoint(47 + stackLocationX, 145 + stackLocationY) , "woodShape1" , false, true  , true , false , 0.25f , Constants.useShapeOfSourceImage , 0 , false , 100 , Constants.breakEffectExplosion);
   AddChild(object3, Constants.depthStack);
    
    StackObject object4 = new StackObject(world , new CCPoint( 0 + stackLocationX, 225 + stackLocationY) , "woodShape1" , false, true  , true , false , 0.25f , Constants.useShapeOfSourceImage , 90 , false , 100 , Constants.breakEffectExplosion);
   AddChild(object4, Constants.depthStack);
    
    StackObject object5 = new StackObject(world , new CCPoint(95 + stackLocationX, 225 + stackLocationY) , "woodShape1" , false, true  , true , false , 0.25f , Constants.useShapeOfSourceImage , 90 , false , 100 , Constants.breakEffectExplosion);
   AddChild(object5, Constants.depthStack);
    
    StackObject object6 = new StackObject(world , new CCPoint(50 + stackLocationX, 305 + stackLocationY) , "woodShape1" , false, true  , true , false , 0.25f , Constants.useShapeOfSourceImage , 0 , false , 100 , Constants.breakEffectSmokePuffs);
   AddChild(object6, Constants.depthStack);
    
    StackObject object7 = new StackObject(world , new CCPoint(0 + stackLocationX , 345 + stackLocationY) , "triangleMedium" , false, true  , false , true , 0.25f , Constants.useTriangle , 0 , false , 100 , Constants.breakEffectSmokePuffs) ;
   AddChild(object7, Constants.depthStack);
    
    StackObject object8 = new StackObject(world , new CCPoint(95 + stackLocationX, 345 + stackLocationY) , "triangleMedium" , false, true , false , true , 0.25f , Constants.useTriangle , 0 , false , 100 , Constants.breakEffectExplosion);
   AddChild(object8, Constants.depthStack);
    
    StackObject object9 = new StackObject(world , new CCPoint(50 + stackLocationX, 350 + stackLocationY) , "triangleLarge" , false, true , false , true , 0.25f , Constants.useTriangle , 180 , false , 500 , Constants.breakEffectSmokePuffs);
   AddChild(object9, Constants.depthStack);
    
    StackObject object10 = new StackObject(world , new CCPoint(25 + stackLocationX, 394 + stackLocationY) , "triangleSmall" , false, true , false , false , 0.25f , Constants.useTriangle , 0 , false , 100 , Constants.breakEffectExplosion);
   AddChild(object10, Constants.depthStack);
    
    StackObject object11 = new StackObject(world , new CCPoint(75 + stackLocationX, 394 + stackLocationY) , "triangleSmall" , false, true , false , false , 0.25f , Constants.useTriangle , 0 , false , 100 , Constants.breakEffectSmokePuffs);
   AddChild(object11, Constants.depthStack);

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
                                      Constants.useShapeOfSourceImageButSlightlySmaller,
                                      10000,
                                      Constants.breakEffectSmokePuffs);

            AddChild(enemy1, Constants.depthStack);
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
                                          Constants.useShapeOfSourceImage,
                                          90,
                                          true,
                                          0,
                                          Constants.breakEffectNone);
            AddChild(object1, Constants.depthStack);
            object1 = new StackObject(world,
                                          new CCPoint(190 + stackLocationX, 40 + stackLocationY),
                                          "marbleSquare",
                                          false,
                                          false,
                                          false,
                                          false,
                                          0.25f,
                                          Constants.useShapeOfSourceImage,
                                          90,
                                          true,
                                          0,
                                          Constants.breakEffectNone);
            AddChild(object1, Constants.depthStack);
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
                                          Constants.useShapeOfSourceImage,
                                          90,
                                          true,
                                          0,
                                          Constants.breakEffectNone);
            AddChild(object1, Constants.depthStack);
            object1 = new StackObject(world,
                                          new CCPoint(27 + stackLocationX, 220 + stackLocationY),
                                          "stonePillar",
                                          false,
                                          false,
                                          false,
                                          false,
                                          0.25f,
                                          Constants.useShapeOfSourceImage,
                                          90,
                                          true,
                                          0,
                                          Constants.breakEffectNone);
            AddChild(object1, Constants.depthStack);
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
                                                      Constants.useShapeOfSourceImage,
                                                      0,
                                                      false,
                                                      100,
                                                      Constants.breakEffectExplosion);
            AddChild(object1, Constants.depthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(57 + stackLocationX, 128 + stackLocationY),
                                                      "woodShape4",
                                                      false,
                                                      true,
                                                      false,
                                                      true,
                                                      0.25f,
                                                      Constants.useShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      Constants.breakEffectExplosion);
            AddChild(object1, Constants.depthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(114 + stackLocationX, 128 + stackLocationY),
                                                      "woodShape4",
                                                      false,
                                                      true,
                                                      false,
                                                      false,
                                                      0.25f,
                                                      Constants.useShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      Constants.breakEffectExplosion);
            AddChild(object1, Constants.depthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(168 + stackLocationX, 128 + stackLocationY),
                                                      "woodShape4",
                                                      false,
                                                      true,
                                                      false,
                                                      false,
                                                      0.25f,
                                                      Constants.useShapeOfSourceImage,
                                                      90,
                                                      false,
                                                      100,
                                                      Constants.breakEffectExplosion);
            AddChild(object1, Constants.depthStack);
            object1 = new StackObject(world,
                                                      new CCPoint(124 + stackLocationX, 166 + stackLocationY),
                                                      "woodShape3",
                                                      false,
                                                      true,
                                                      false,
                                                      false,
                                                      0.25f,
                                                      Constants.useShapeOfSourceImage,
                                                      0,
                                                      false,
                                                      100,
                                                      Constants.breakEffectExplosion);
            AddChild(object1, Constants.depthStack);
            #endregion


            StackObject object11 = new StackObject(world, new CCPoint(45 + stackLocationX, 210 + stackLocationY), "woodShape4", false, true, false, true, 0.25f, Constants.useShapeOfSourceImage, 90, false, 100, Constants.breakEffectExplosion);
            AddChild(object11, Constants.depthStack);

            StackObject object12 = new StackObject(world, new CCPoint(95 + stackLocationX, 210 + stackLocationY), "woodShape4", false, true, false, true, 0.25f, Constants.useShapeOfSourceImage, 90, false, 100, Constants.breakEffectExplosion);
            AddChild(object12, Constants.depthStack);

            StackObject object13 = new StackObject(world, new CCPoint(145 + stackLocationX, 210 + stackLocationY), "woodShape4", false, true, false, true, 0.25f, Constants.useShapeOfSourceImage, 90, false, 100, Constants.breakEffectExplosion);
            AddChild(object13, Constants.depthStack);

            StackObject object14 = new StackObject(world, new CCPoint(200 + stackLocationX, 210 + stackLocationY), "woodShape4", false, true, false, true, 0.25f, Constants.useShapeOfSourceImage, 90, false, 100, Constants.breakEffectExplosion);
            AddChild(object14, Constants.depthStack);

            StackObject object15 = new StackObject(world, new CCPoint(80 + stackLocationX, 250 + stackLocationY), "stonePillar", false, false, false, true, .5f, Constants.useShapeOfSourceImage, 90, false, 0, Constants.breakEffectNone);
            AddChild(object15, Constants.depthStack);

            StackObject object16 = new StackObject(world, new CCPoint(180 + stackLocationX, 250 + stackLocationY), "stonePillar", false, false, false, true, .5f, Constants.useShapeOfSourceImage, 90, false, 0, Constants.breakEffectNone);
            AddChild(object16, Constants.depthStack);


            StackObject object17 = new StackObject(world, new CCPoint(95 + stackLocationX, 285 + stackLocationY), "triangleMedium", false, true, false, true, 0.25f, Constants.useTriangle, 0, false, 100, Constants.breakEffectExplosion);
            AddChild(object17, Constants.depthStack);

            StackObject object18 = new StackObject(world, new CCPoint(181 + stackLocationX, 285 + stackLocationY), "triangleMedium", false, true, false, true, 0.25f, Constants.useTriangle, 0, false, 100, Constants.breakEffectExplosion);
            AddChild(object18, Constants.depthStack);

            StackObject object19 = new StackObject(world, new CCPoint(138 + stackLocationX, 280 + stackLocationY), "triangleSmall", false, true, false, true, 0.25f, Constants.useTriangle, 180, false, 500, Constants.breakEffectExplosion);
            AddChild(object19, Constants.depthStack);


            StackObject object5 = new StackObject(world, new CCPoint(137 + stackLocationX, 340 + stackLocationY), "stonePillar", false, false, false, true, .5f, Constants.useShapeOfSourceImage, 0, false, 0, Constants.breakEffectNone);
            AddChild(object5, Constants.depthStack);



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
                                      Constants.useShapeOfSourceImage,
                                      10000,
                                      Constants.breakEffectSmokePuffs);

            AddChild(enemy1, Constants.depthStack);
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
                                      Constants.useShapeOfSourceImageButSlightlySmaller,
                                      10000,
                                      Constants.breakEffectSmokePuffs);

            AddChild(enemy1, Constants.depthStack);
        }



    }
}

