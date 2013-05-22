using System;

using Cocos2D;
using CocosDenshion;

namespace AngryNinjas
{
	public class GameSounds : CCNode
	{
		bool soundFXTurnedOff;
		bool voiceFXTurnedOff;
		bool ambientFXTurnedOff; //ambient or background music
		
		string delayedSoundName;
		
		AmbientFXSounds musicChoice;

		const string soundsFolder = "Sounds";

		static GameSounds sharedGameSounds;

		private GameSounds ()
		{
			voiceFXTurnedOff = GameData.SharedData.AreVoiceFXMuted;
			soundFXTurnedOff = GameData.SharedData.AreSoundFXMuted;
			ambientFXTurnedOff = GameData.SharedData.AreAmbientFXMuted;
		}

		/// <summary>
		/// returns a shared instance of the GameData
		/// </summary>
		/// <value> </value>
		public static GameSounds SharedGameSounds
		{
			get
			{
				if (sharedGameSounds == null)
				{
					sharedGameSounds = new GameSounds();
				}
				return sharedGameSounds;
			}
		}

		public void PreloadSounds ()
		{
			
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("grunt1.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("grunt2.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("grunt3.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("grunt4.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("grunt5.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("grunt6.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("grunt7.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("grunt8.mp3"));
			
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("break1.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("break2.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("break3.mp3"));
			
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("impact1.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("impact2.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("impact3.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("impact4.mp3"));
			SimpleAudioEngine.SharedEngine.PreloadEffect(FormatSoundFilePath("impact5.mp3"));
		}

		string FormatSoundFilePath(string sound)
		{
            string sndfile = System.IO.Path.Combine(soundsFolder,sound);
            if (sndfile.IndexOf(".mp3") > -1)
            {
                sndfile = sndfile.Substring(0, sndfile.IndexOf(".mp3"));
            }
            else if (sndfile.IndexOf(".wav") > -1)
            {
                sndfile = sndfile.Substring(0, sndfile.IndexOf(".wav"));
            }
            return (sndfile);
        }

		public bool AreSoundFXMuted
		{
			get { return soundFXTurnedOff; }
			set { soundFXTurnedOff = value; }
		}

		public bool AreVoiceFXMuted
		{
			get { return voiceFXTurnedOff; }
			set { voiceFXTurnedOff = value; }
		}

		public void PlaySoundFX(string fileToPlay)
		{
			
			if ( !soundFXTurnedOff ) {
				
				SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath (fileToPlay));
				
			}
			
		}
		
		public void PlayVoiceSoundFX(string fileToPlay) {
			
			if ( !voiceFXTurnedOff ) {
				
				SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath (fileToPlay));
				
				
			}
			
		}

		public void PlaySoundFXWithDelay(string fileToPlay, float theDelay) 
		{
			
			if ( !soundFXTurnedOff ) {
				
				delayedSoundName = fileToPlay;
				ScheduleOnce(PlayThisAfterDelay, theDelay);
				
				
			}
			
		}
		
		
		public void PlayVoiceSoundFXWithDelay(string fileToPlay, float theDelay)
		{
			
			
			
			if ( !voiceFXTurnedOff ) {
				
				delayedSoundName = fileToPlay;
				
				ScheduleOnce(PlayThisAfterDelay, theDelay);
				
			}
			
		}
		
		public void PlayThisAfterDelay (float delay)
		{
			
			SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath(delayedSoundName));
			
			
		}
		
		
		
		
		
		public void IntroTag ()
		{
			
			if ( !soundFXTurnedOff ) {
				
				SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("gong.mp3"));
				
			}
			
			
		}
		
		
		public void PlayStackImpactSound ()
		{
			
			
			if (!soundFXTurnedOff) {
				
				
				int randomNum = Cocos2D.Random.Next(0,4); //0 to 4
				
				
				switch ( randomNum ){ 
				case 0: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("impact1.mp3"));
					break;
				case 1: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("impact2.mp3"));
					break;
				case 2: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("impact3.mp3"));
					break;
				case 3: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("impact4.mp3"));
					break;
				case 4: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("impact5.mp3"));
					break;
					
					
					
				}
				
			}
			
			
		}
		
		
		public void PlayBreakSound () {
			
			
			if (!soundFXTurnedOff) {
				
				
				int randomNum = Cocos2D.Random.Next(0,2); //0 to 2
				
				
				switch ( randomNum ){ 
				case 0: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("break1.mp3"));
					break;
				case 1: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("break2.mp3"));
					break;
				case 2: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("break3.mp3"));
					break;
					
					
				}
				
			}
			
			
		}
		
		
		
		
		public void ReleaseSlingSounds ()
		{
			
			
			if (!soundFXTurnedOff) {
				
				SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("whoosh.mp3"));
				
			}
			
			
			if ( !voiceFXTurnedOff ) {
				
				
				int randomNum = Cocos2D.Random.Next(0,7); //0 to 7
				
				
				switch ( randomNum ){ 
				case 0: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("grunt1.mp3"));
					break;
				case 1: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("grunt2.mp3"));
					break;
				case 2: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("grunt3.mp3"));
					break;
				case 3: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("grunt4.mp3"));
					break;
				case 4: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("grunt5.mp3"));
					break;
				case 5: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("grunt6.mp3"));
					break;
				case 6: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("grunt7.mp3"));
					break;
				case 7: 
					SimpleAudioEngine.SharedEngine.PlayEffect(FormatSoundFilePath("grunt8.mp3"));
					break;
					
					
				}
				
			}
			
			
		}
		
		
		public void PlayBackgroundMusic(AmbientFXSounds track) {  //or AMBIENT SOUND FX
			
			musicChoice = track;

			SimpleAudioEngine.SharedEngine.StopBackgroundMusic();
			
			if ( !ambientFXTurnedOff ) {
				
				if ( musicChoice == AmbientFXSounds.Frogs ) {
					SimpleAudioEngine.SharedEngine.PlayBackgroundMusic(FormatSoundFilePath("birds.mp3"), true);
				}  else if ( musicChoice == AmbientFXSounds.Insects ) {
					SimpleAudioEngine.SharedEngine.PlayBackgroundMusic(FormatSoundFilePath("frogs.mp3"), true);
				}
				
			}
			
			
		}
		
		
		public void StopBackgroundMusic () {
			

			SimpleAudioEngine.SharedEngine.StopBackgroundMusic();
			
			ambientFXTurnedOff = true;
			
			
			
		}
		
		public void RestartBackgroundMusic () {
			
			ambientFXTurnedOff = false;
			PlayBackgroundMusic(musicChoice);    
		}
		
		


	}
}

