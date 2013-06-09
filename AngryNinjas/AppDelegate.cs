using System.Reflection;
using Microsoft.Xna.Framework;
using Cocos2D;


namespace AngryNinjas
{
	public class AppDelegate : CCApplication
	{
		public AppDelegate(Game game, GraphicsDeviceManager graphics)
			: base(game, graphics)
		{
			s_pSharedApplication = this;
#if WINDOWS || MACOS || MONOMAC || LINUX || OUYA || XBOX
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
#endif
            CCDrawManager.InitializeDisplay(game, 
			                              graphics, 
			                              DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft);
			
			
			graphics.PreferMultiSampling = false;
			
			//graphics.PreferredBackBufferWidth = 480;
			//graphics.PreferredBackBufferHeight = 320;
		}
		
		/// <summary>
		/// Implement for initialize OpenGL instance, set source path, etc...
		/// </summary>
		public override bool InitInstance()
		{
			return base.InitInstance();
		}
		
		/// <summary>
		///  Implement CCDirector and CCScene init code here.
		/// </summary>
		/// <returns>
		///  true  Initialize success, app continue.
		///  false Initialize failed, app terminate.
		/// </returns>
		public override bool ApplicationDidFinishLaunching()
		{
			//initialize director
			CCDirector pDirector = CCDirector.SharedDirector;
			pDirector.SetOpenGlView();


			// 2D projection
			pDirector.Projection = CCDirectorProjection.Projection2D;

			// Enables High Res mode (Retina Display) on iPhone 4 and maintains low res on all other devices
			//if( ! [director_ enableRetinaDisplay:YES] )
            //	CCLOG(@"Retina Display Not supported");


#if WINDOWS || MACOS || MONOMAC || LINUX || OUYA || XBOX
            CCDrawManager.SetDesignResolutionSize(1024, 768, CCResolutionPolicy.ExactFit);
			#else
			CCDrawManager.SetDesignResolutionSize(480, 320, ResolutionPolicy.ShowAll);
			#endif

			// turn on display FPS
			//pDirector.DisplayStats = true;

			// set FPS. the default value is 1.0/60 if you don't call this
			pDirector.AnimationInterval = 1.0 / 60;
			
			CCScene pScene = IntroLayer.Scene;

			pDirector.RunWithScene(pScene);
			return true;
		}
		
		/// <summary>
		/// The function be called when the application enter background
		/// </summary>
		public override void ApplicationDidEnterBackground()
		{
			CCDirector.SharedDirector.Pause();
			
			// if you use SimpleAudioEngine, it must be pause
			// SimpleAudioEngine::sharedEngine()->pauseBackgroundMusic();
		}
		
		/// <summary>
		/// The function be called when the application enter foreground  
		/// </summary>
		public override void ApplicationWillEnterForeground()
		{
            GameSounds.SharedGameSounds.PreloadSounds();
            CCDirector.SharedDirector.Resume();
			
			// if you use SimpleAudioEngine, it must resume here
			// SimpleAudioEngine::sharedEngine()->resumeBackgroundMusic();
		}
	}
}