using System;

namespace AngryNinjas
{
	public static class Constants
	{
		 
		public const int PTM_RATIO = 32;
		//depth levels
			
		public const int depthClouds = -300;
		public const int depthParticles = -299;
		public const int depthHills = -298;
		public const int depthFloor = -290;
		public const int depthScore = -285;
			
		public const int depthWhiteDots = -205;

		public const int depthStrapBack  = -202;
		public const int depthPlatform = -201;
		public const int depthNinjas = -200;
		public const int depthStack = -199;
			
			
		public const int depthSlingShotFront = -179;
		public const int depthStrapFront = -178;
			
		public const int depthVisualFx = 50;
		public const int depthPointScore = 100;

		//tags
		
		public const int tagForWhiteDotsEvenNumberedTurn = 1000;
		public const int tagForWhiteDotsOddNumberedTurn = 2000;

		//creation methods
		
		public const int useShapeOfSourceImage = 0;  //use for illustrations that are squares/ and rectangles and cropped right up to the edge. Will work with any size, vector points are in all four corners
		
		public const int useTriangle = 1; //vector points are in the bottom left, bottom right, and top center of the image, (for example image triangleTall.png, triangleLarge.png, triangleMedium.png, triangleSmall.png )
		public const int useTriangleRightAngle = 2; //vector points are in the top right corner, top left corner, and bottom left corner of the source image, (for example triangleRightAngle.png ) 
		
		public const int useTrapezoid = 3; //vector points are in the bottom left corner, bottom right corner, and 1/4 and 3/4's across the top of the image, (for example trapezoid.png ) 
		
		public const int useDiameterOfImageForCircle = 4;  //use for illustrations that are perfect circles and cropped right up to the edge of the circle on all sides. Will work with any size
		
		public const int useHexagon = 5; //any size image with a hexagon can work, scale up or down the hexagonShape.png as a template to get the location of each edge.
		public const int useOctagon = 6; //any size image with a octagon can work, scale up or down the octagonShape.png as a template to get the location of each edge.
		public const int usePentagon = 7; //any size image with a pentagon can work, scale up or down the pentagon.png as a template to get the location of each edge.
		
		public const int useParallelogram = 8; // vector points are in the bottom left corner, and 3/4's across the bottom of the image, top right corner, and 1/4 across the top (for example parallelogram.png ) 
		
		public const int useShapeOfSourceImageButSlightlySmaller = 9; //defines vector shapes about 80% the size of the source (still a square or rectangle just slightly smaller) . Good for Enemies. 


		
		
		public const int customCoordinates1 = 101;  //use your own custom coordinates from a program like Vertex Helper Pro 
		public const int customCoordinates2 = 102; //use your own custom coordinates from a program like Vertex Helper Pro
		public const int customCoordinates3 = 103; //use your own custom coordinates from a program like Vertex Helper Pro
		
		//remember when defining custom coordinates, the vec points much be defined in counter clockwise order, 8 points at most, and make a convex shape (i.e. every point must be able to touch every other point)
		
		
		public const int breakEffectNone = 0;
		public const int breakEffectSmokePuffs = 1;
		public const int breakEffectExplosion = 2;

		
		//sounds
		
		public const int kFrogSounds = 1;
		public const int kInsectSounds = 2;


	}
}

