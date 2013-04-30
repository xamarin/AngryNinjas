using System;

namespace AngryNinjas
{
	public static class Constants
	{
		 
		public const int PTM_RATIO = 32;
		//depth levels
			
		public const int DepthClouds = -300;
		public const int DepthParticles = -299;
		public const int DepthHills = -298;
		public const int DepthFloor = -290;
		public const int DepthScore = -285;
			
		public const int DepthWhiteDots = -205;

		public const int DepthStrapBack  = -202;
		public const int DepthPlatform = -201;
		public const int DepthNinjas = -200;
		public const int DepthStack = -199;
			
			
		public const int DepthSlingShotFront = -179;
		public const int DepthStrapFront = -178;
			
		public const int DepthVisualFx = 50;
		public const int DepthPointScore = 100;

		//tags
		
		public const int TagForWhiteDotsEvenNumberedTurn = 1000;
		public const int TagForWhiteDotsOddNumberedTurn = 2000;

	}

	//creation methods
	public enum CreationMethod 
	{
		ShapeOfSourceImage = 0,  //use for illustrations that are squares/ and rectangles and cropped right up to the edge. Will work with any size, vector points are in all four corners
		
		Triangle = 1, //vector points are in the bottom left, bottom right, and top center of the image, (for example image triangleTall.png, triangleLarge.png, triangleMedium.png, triangleSmall.png )
		TriangleRightAngle = 2, //vector points are in the top right corner, top left corner, and bottom left corner of the source image, (for example triangleRightAngle.png ) 
		
		Trapezoid = 3, //vector points are in the bottom left corner, bottom right corner, and 1/4 and 3/4's across the top of the image, (for example trapezoid.png ) 
		
		DiameterOfImageForCircle = 4,  //use for illustrations that are perfect circles and cropped right up to the edge of the circle on all sides. Will work with any size
		
		Hexagon = 5, //any size image with a hexagon can work, scale up or down the hexagonShape.png as a template to get the location of each edge.
		Octagon = 6, //any size image with a octagon can work, scale up or down the octagonShape.png as a template to get the location of each edge.
		Pentagon = 7, //any size image with a pentagon can work, scale up or down the pentagon.png as a template to get the location of each edge.
		
		Parallelogram = 8, // vector points are in the bottom left corner, and 3/4's across the bottom of the image, top right corner, and 1/4 across the top (for example parallelogram.png ) 
		
		ShapeOfSourceImageButSlightlySmaller = 9, //defines vector shapes about 80% the size of the source (still a square or rectangle just slightly smaller) . Good for Enemies. 

		CustomCoordinates1 = 101,  //use your own custom coordinates from a program like Vertex Helper Pro 
		CustomCoordinates2 = 102, //use your own custom coordinates from a program like Vertex Helper Pro
		CustomCoordinates3 = 103 //use your own custom coordinates from a program like Vertex Helper Pro

		//remember when defining custom coordinates, the vec points much be defined in counter clockwise order, 8 points at most, and make a convex shape (i.e. every point must be able to touch every other point)

	}

	// Break Effects
	public enum BreakEffect
	{
		None = 0,
		SmokePuffs = 1,
		Explosion = 2
	}

	// Ambient Sounds
	public enum AmbientFXSounds
	{
		Frogs = 1,
		Insects = 2
	}
}

