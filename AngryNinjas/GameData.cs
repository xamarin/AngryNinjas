using System;

using Cocos2D;

// Video guide is at https://vimeo.com/cartoonsmart/angryninjasguide
// HowToGuide part 1 Images and GameData Class 00:00:00

/*
 
 The GameData class mostly maintains data through all levels of the game. 
 A class like TheLevel will get the background image to show for the current level from this class.
 
 For example, in GameData.cs you've got....
 

 string ReturnBackgroundHillsFileName() {
 
    switch (level) {
        case 0:
            backgroundHillsFileName = "background_hills.png";
            break;
        case 1:
            backgroundHillsFileName = "background_hills.png";
            break;
        case 2:
            backgroundHillsFileName = "background_hills.png";
            break;
 
        default:
            backgroundHillsFileName = "background_hills.png";
            break;
        }
 
    return backgroundHillsFileName;
 }

 
 ... so you would want to include a different name png file for various levels. 
 And right now, I've only set up 3 levels worth, so you need to do some copying any pasting in the switch statement to account for as many levels as you have. 
 
 
 
 
 IMPORTANT VARS IN THIS CLASS...
 
 level = 1;  
 eachLevelSectionIsHowManyLevels = 1;
 
 
 ...when testing your game you might want to change the starting value of level. I used 0 for testing a lot. 
 
 Also if  eachLevelSectionIsHowManyLevels  is more than 1, than that means your section buttons in the TheMenu class will jump to the first level in each section.
 
 So if  eachLevelSectionIsHowManyLevels = 3; 
 
 Then... 
 
 The first button goes to level 1
 The second button goes to level 4
 The third button goes to level 7
 
 and so on. And your player will need to complete all levels in a section before the section is unlocked.
 
 */


namespace AngryNinjas
{
	public class GameData
	{

		static GameData sharedData;

		int level;
		int maxLevels;
		
		bool soundFXMuted;
		bool voiceFXMuted;
		bool ambientFXMuted;
		bool firstRunEver;
		
		CCUserDefault defaults;
		
		string backgroundCloudsFileName;
		string backgroundHillsFileName;
		string groundPlaneFileName;
		
		int ninjasToTossThisLevel;
		
		int pointsToPassLevel;
		
		int bonusPerExtraNinja;
		
		int pointTotalForAllLevels;
		
		int levelsCompleted;
		
		int eachLevelSectionIsHowManyLevels;

		int[] highScoreLevel;

		private GameData ()
		{
			firstRunEver = true;
			
			defaults = CCUserDefault.SharedUserDefault;

			soundFXMuted = defaults.GetBoolForKey("soundFXMutedKey");   //will default to NO if there's no previous default value
			voiceFXMuted = defaults.GetBoolForKey("voiceFXMutedKey");   //will default to NO if there's no previous default value
			ambientFXMuted = defaults.GetBoolForKey("ambientFXMutedKey");   //will default to NO if there's no previous default value
			
			pointTotalForAllLevels = 0;
			
			levelsCompleted = defaults.GetIntegerForKey("levelsCompletedAllTime");
			level = 1; //use 0 to show a testbed of shapes
			
			maxLevels = 30; // change to whatever, could be more or less
			
			CCLog.Log(" Levels completed over all time: {0}", levelsCompleted  );
			
			eachLevelSectionIsHowManyLevels = 1;

			highScoreLevel = new int[30];

			for (int x = 0; x < highScoreLevel.Length; x++)
			{
				highScoreLevel[x] = defaults.GetIntegerForKey("highScoreLevel" + (x + 1));
			}

		}

		/// <summary>
		/// returns a shared instance of the GameData
		/// </summary>
		/// <value> </value>
		public static GameData SharedData
		{
			get
			{
				if (sharedData == null)
				{
					sharedData = new GameData();
				}
				return sharedData;
			}
		}

		public bool FirstRunEver
		{
			get { return firstRunEver; }
			set { firstRunEver = value; }
		}

		public int Level 
		{
			
			get { return level; }
		}

		#region Level Up
		public void levelUp()
		{
			
			level ++;
			
			if ( level > levelsCompleted ) {
				
				levelsCompleted = level - 1;
				
				defaults.SetIntegerForKey("levelsCompletedAllTime", levelsCompleted);
				defaults.Flush();

				CCLog.Log(@"Level {0} completed", levelsCompleted);
			} else {
				
				CCLog.Log(@"That level was completed before");
				
			}
			
			if( level > maxLevels) {
				
				level = 1;
			}
			
			
		}

		#endregion


		#region METHODS TheLevel Class calls to return settings for the current level being played

		public string BackgroundCloudsFileName 
		{
			get 
			{
				switch (level) {
				case 0:
					backgroundCloudsFileName = @"background_clouds.png";
					break;
				case 1:
					backgroundCloudsFileName = @"background_clouds.png";
					break;
				case 2:
					backgroundCloudsFileName = @"background_clouds.png";
					break;
				
				// keep adding new cases for each level, the default for all levels after 2 is below  (these are all the same file anyway)
				
				default:
					backgroundCloudsFileName = @"background_clouds.png";
					break;
				}
			
				return backgroundCloudsFileName;
			}
		}
		
		public string BackgroundHillsFileName 
		{
			get 
			{
				switch (level) {
				case 0:
					backgroundHillsFileName = @"background_hills.png";
					break;
				case 1:
					backgroundHillsFileName = @"background_hills.png";
					break;
				case 2:
					backgroundHillsFileName = @"background_hills.png";
					break;
				
				
				// keep adding new cases for each level, the default for all levels after 2 is below (these are all the same file anyway)
				
				default:
					backgroundHillsFileName = @"background_hills.png";
					break;
				}
			
				return backgroundHillsFileName;
			}
		}
		
		public string GroundPlaneFileName 
		{
		
			get 
			{
				switch (level) {
				case 0:
					groundPlaneFileName = @"ground_plane.png";
					break;
				case 1:
					groundPlaneFileName = @"ground_plane.png";
					break;
				case 2:
					groundPlaneFileName = @"ground_plane.png";
					break;
				
				// keep adding new cases for each level, the default for all levels after 2 is below (these are all the same file anyway)
				
				default:
					groundPlaneFileName = @"ground_plane.png";
					break;
				}
			
				return groundPlaneFileName;
			}
		}
		
		
		public int NumberOfNinjasToTossThisLevel 
		{
			get
			{
				switch (level) {
				case 0:
					ninjasToTossThisLevel = 5;
					break;
				case 1:
					ninjasToTossThisLevel = 2;
					break;
				case 2:
					ninjasToTossThisLevel = 5;
					break;
				
				// keep adding new cases for each level, the default for all levels after 2 is below
				
				default:
					ninjasToTossThisLevel = 5;
					break;
				}
			
			
			
				return ninjasToTossThisLevel;
			}
		}
		
		
		public int PointsToPassLevel 
		{
			get 
			{
			
				switch (level) {
				case 0:
					pointsToPassLevel = 8000;
					break;
				case 1:
					pointsToPassLevel = 9000;
					break;
				case 2:
					pointsToPassLevel = 20000;
					break;
				// keep adding new cases for each level, the default for all levels after 2 is below
				
				default:
					pointsToPassLevel = 10000;
					break;
				}
			
			
				return pointsToPassLevel;
			}
		}
		
		public int BonusPerExtraNinja 
		{
			get
			{
				switch (level) {
				case 0:
					bonusPerExtraNinja = 10000;
					break;
				case 1:
					bonusPerExtraNinja = 10000;
					break;
				case 2:
					bonusPerExtraNinja = 10000;
					break;
				
				// keep adding new cases for each level, the default for all levels after 2 is below
				
				default:
					bonusPerExtraNinja = 10000;
					break;
				}
			
			
				return bonusPerExtraNinja;
			}
		}

		#endregion

		#region HIGH SCORES 
		public int HighScoreForLevel 
		{
		
			get {
				return highScoreLevel[level];
			}

			set 
			{

				CCLog.Log(" checking to see if {0} is a new high score", value);
				
				if (level < highScoreLevel.Length && value > highScoreLevel[level]) {
					highScoreLevel[level] = value;
					defaults.SetIntegerForKey("highScoreLevel1", highScoreLevel[level]);
					defaults.Flush();
				}

			}
		}

		#region LEVEL JUMPING
		
		
		public bool CanYouGoToTheFirstLevelOfThisSection(int theSection)
		{
			
			var thePreviousSection = theSection - 1;
			
			if (levelsCompleted >= ( thePreviousSection * eachLevelSectionIsHowManyLevels) ) {
				
				return  true;
			}  
			else {
				
				CCLog.Log("you need to pass level {0} before jumping to here",  ( thePreviousSection * eachLevelSectionIsHowManyLevels));
				return  false;
				
			}
			
		}
		
		public void ChangeLevelToFirstInThisSection(int theSection) 
		{
			
			var thePreviousSection = theSection - 1;
			
			level = ( thePreviousSection * eachLevelSectionIsHowManyLevels) + 1;
			CCLog.Log("Level now equals {0}, which is the first level in Section: {1}", level, theSection);
		}
		
		
		
		public void AttemptToGoToFirstLevelOfSection(int theSection)
		{
			
			
			if ( CanYouGoToTheFirstLevelOfThisSection(theSection) ) {
				
				ChangeLevelToFirstInThisSection(theSection);
				
			}
			
		}
		#endregion

		#region RESET GAME / POINT TOTAL ALL LEVELS (not called ever) 
		
		public void ResetGame()
		{   //this method never gets called in my version. Not really a need to since I'm not showing the pointTotalForAllLevels ever
			
			level = 1;
			pointTotalForAllLevels = 0;
			
		}
		
		
		public void AddToPointTotalForAllLevels(int pointThisLevel)
		{  //this method gets called, but at no point am I ever showing the pointTotalForAllLevels
			
			
			pointTotalForAllLevels += pointThisLevel;
			
		}

		#endregion

		#region sounds
		
		public bool AreSoundFXMuted
		{
			
			get {
				return soundFXMuted;
			}

			set 
			{
				soundFXMuted = value;
				defaults.SetBoolForKey("soundFXMutedKey", soundFXMuted );
				defaults.Flush();
			}

		}

		#endregion

		/////////////////////////
		
		public bool AreVoiceFXMuted 
		{

			get {	
				return voiceFXMuted;
			}

			set 
			{
				voiceFXMuted = value;
				defaults.SetBoolForKey("voiceFXMutedKey",voiceFXMuted );
				defaults.Flush();
			}
			
			
			
		}

		/////////////////////////
		
		public bool AreAmbientFXMuted
		{

			get {	
				return ambientFXMuted;
			}

			set 
			{
				ambientFXMuted = value;
				defaults.SetBoolForKey("ambientFXMutedKey",ambientFXMuted );
				defaults.Flush();
			}
		}

		#endregion
	}
}

