/* **********************************************************************************************************************
 * Project Name:    The Waver
 * Team Mentor:     Dr. Chang Yun, Assistant Professor at the University of Houston
 * Team members:    Carlos Lacayo.......Team leader, UI & Model designer, and Programmer
 *                  Evan Susanto........Lead Programmer
 *                  Beata Wlodarczyk....Assistant Programmer and Documentater
 *                  Tri Chu.............Assistant Programmer
 * University of Houston | Houston, Texas, USA
 * This game is presented to Microsoft Imagine Cup 2010
 * Reference: -A great book that guide us to build this game is:
 *             Beginning XNA 3.0 Game Programming by A. Lobao, B. Evangelista, A. Antonio Leal de Farias, and R. Grootjans
 *            -3D Model of the Golden Gate bridge was provided by Google 3D Warehouse.
 *            -Songs was provided by Freeplay Music website
 *            -Sound effects was provided by Freeloop soundeffects website
 *            -Explosion & Particles effects resources is from XNA Community Game Platform
 * YouTube link: http://www.youtube.com/watch?v=2pHQxX2N-Gg
 *************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using TheWaver.Core;
using TheWaver.StoryLevel01;
using TheWaver.StoryLevel02;
using TheWaver.Particles;

namespace TheWaver
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //VARIABLES////////////////////////
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        float aspectRatio;
        protected TimeSpan elapsedTime = TimeSpan.Zero;

        //GAME SCENES
        protected GameScene activeScene;
        protected IntroScene introScene;
        protected MainMenu mainMenu;
        protected SetupShip setupShip;
        protected CheckEmbark checkEmbark;
        protected AboutScene aboutScene;
        protected ControllersMenu controllersMenu;
        //////////////////////////////////////////
        //PROLOGUE SCENES
        protected Prologue01 prologue01;
        protected Prologue02 prologue02;
        protected Prologue03 prologue03;
        protected Prologue04 prologue04;
        //////////////////////////////////////////
        //PROLOGUE ENTITY
        protected Texture2D prologueTop;
        //////////////////////////////////////////
        //PROLOGUE LEVEL
        protected LevelPrologue levelPrologue;
        //////////////////////////////////////////
        //STORY SCENES - LEVEL 01
        protected Story01 story01;
        protected Story02 story02;
        protected Story03 story03;
        protected Story04 story04;
        protected Story05 story05;
        //////////////////////////////////////////
        //LEVEL 01
        protected level01 lev01;
        protected level01_description level1READER;
        //////////////////////////////////////////
        //BOSS STORYLINE
        protected BossStory01 bossStory01;
        protected BossStory02 bossStory02;
        protected BossStoryObj bossStoryObj;
        //////////////////////////////////////////
        //LEVEL 01.2 - FULL POWER/////////////////
        protected Level01_02 level01_02;
        //////////////////////////////////////////
        //LEVEL 01.3 - THE BOSS VARIABLES////////
        protected Texture2D levelBackground0102;
        protected Level01_03 level01_03;
        //////////////////////////////////////////
        //LEVEL 02 STORYLINE//////////////////////
        protected LevelStory0201 levelStory0201;
        protected LevelStory0202 levelStory0202;
        protected LevelStory0203 levelStory0203;
        protected LevelStory0204 levelStory0204;
        protected LevelStory0205 levelStory0205;
        //////////////////////////////////////////
        //LEVEL 02////////////////////////////////
        protected Level0201PROMPT level0201PROMPT;  //Displays the level description
        protected Level02_01 level02_01;              //Level 02.1 gameplay
        protected Level0202PROMPT level0202PROMPT;  //Displays the level description
        //protected Level02_02 level02_02;              //Level 02.2 gameplay
        //////////////////////////////////////////
        //TEXTURES
        protected Texture2D STATE_INTRO, STATE_CONTROLLERS, STATE_MAINMENU, STATE_UPGRADESHIP;
        protected Texture2D STATE_PROLOGUE01, STATE_PROLOGUE02, STATE_PROLOGUECONTROLLS, STATE_PROLOGUEMISSION, STATE_state_MISSIONACCOMPLISH_PROLOGUE, SFBAYTEXTURE;
        protected Texture2D STATE_STORY01, STATE_STORY02, STATE_STORY03, STATE_STORY04, STATE_STORY05;
        protected Texture2D STATE_BOSSSTORY01, STATE_BOSSSTORY02, STATE_BOSSSTORYOBJ;
        protected Texture2D STATE_LEVEL01MISSION, STATE_LEVEL0101_COMPLETE, STATE_LEVEL0102_COMPLETE;
        protected Texture2D STATE_MISSIONFAIL;
        protected Texture2D STATE_MISSIONACCOMPLISH;
        protected Texture2D STATE_ABOUTSCENE, STATE_CREDITS, credit_title, blackBackground;
        protected Texture2D STATE_PAUSE, STATE_ERROR01, STATE_ERROR02;
        protected Texture2D ocean, ocean1, oceanLevel01_Background, oceanLevel02;
        protected Texture2D sideBar, sideBar_Level0102,sideBar02, level02_02BACKGROUND;
        protected Texture2D radar, radarAnimation, radarStick;
        protected Texture2D playerShip, playerShipSideView;
        protected Texture2D hullGreen, hullYellow, hullOrange, hullRed;
        protected Texture2D box01, box02;
        protected Texture2D oilSpillTexture, oilRig, oilRig02;
        protected Texture2D enemyJetFighter, enemyJetFighterTopView, enemyGunboat_leftPoint;
        protected Texture2D statusUpdateENEMY01, statusUpdateMAYDAY, statusUpdateBoxes, bossShip01;
        protected Texture2D bossStatus_GREEN, bossStatus_YELLOW, bossStatus_ORANGE, bossStatus_RED;
        protected Texture2D plasma, cannon, explosionEffect, smokeEffect;
        protected Texture2D crosshair, chopperDown, alpha01c, alpha02c, alpha03c;
        protected Texture2D bottomStatusBar, loadingCrewBar;
        protected Texture2D playerLeft, playerUp, playerDown;
        protected Texture2D LEVELSTORYSTATE0201, LEVELSTORYSTATE0202, LEVELSTORYSTATE0203, LEVELSTORYSTATE0204, 
            LEVELSTORYSTATE0205, DISPLAYLEVEL0201, DISPLAYLEVEL0202, LEVEL0201_MISSIONSUCESS, LEVEL0201_MISSIONFAIL;
        protected Texture2D boss, missile, missile1;
        protected Texture2D crosshair2;
        protected Texture2D smokeTexture;

        //MODELS
        Model displayPlayer;

        //FONTS
        private SpriteFont FMainMenu_LARGE, FMainMenu_SMALL, level01Fonts, boss01Fonts;

        //SOUNDS
        private AudioLibrary audio;

        //Global Ship Status Variable
        private ShipStatus shipStatus;

        //INPUT
        protected KeyboardState oldKeyboardState;
        protected GamePadState oldGamePadState;

        //Credits state
        Credits_Before credits_before;
        Credits credits;

        ParticleSystem EXPOLSION_PARTICLES;
        ParticleSystem SMOKE_PARTICLES;

        /********************************************************************/
        public Game1() //CONSTRUCTOR
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //EXPLOSION AND SMOKE EFFECTS.............................
            EXPOLSION_PARTICLES = new ExplosionParticleSystem(this, 1, "explosionTexture");
            Components.Add(EXPOLSION_PARTICLES);
            SMOKE_PARTICLES = new ExplosionSmokeParticleSystem(this, 1, "smoke");
            Components.Add(SMOKE_PARTICLES);

            //Setting the screen-size resolutin
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //graphics.IsFullScreen = true;//Full screen

            //Windows Title
            Window.Title = "The Waver";

            // Used for input handling
            oldKeyboardState = Keyboard.GetState();
            oldGamePadState = GamePad.GetState(PlayerIndex.One);

        }
        /********************************************************************/
        protected override void Initialize() //NOTHING HERE
        {
            base.Initialize();
        }
        /********************************************************************/
        protected override void UnloadContent(){}//NOTHING HERE
        /********************************************************************/
        protected override void LoadContent()
        {
            //Load the content for the Scrolling background

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Draws each scene
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            // Load Audio Elements
            audio = new AudioLibrary();
            audio.LoadContent(Content);
            Services.AddService(typeof(AudioLibrary), audio);

            shipStatus = new ShipStatus();
            Services.AddService(typeof(ShipStatus), shipStatus);

            //ERROR OR QUESTION STATES IMAGE LOADER
            STATE_ERROR01 = Content.Load<Texture2D>("error01");
            STATE_ERROR02 = Content.Load<Texture2D>("error02");

            //state_INTRO image is going to display at the introScene scene (state)
            STATE_INTRO = Content.Load<Texture2D>("state_INTRO");
            introScene = new IntroScene(this, STATE_INTRO);
            Components.Add(introScene);


            //state_MAINMENU image is going to display at the mainMenu scene (state)
            FMainMenu_LARGE = Content.Load<SpriteFont>("FontsMainMenu_LARGE");
            FMainMenu_SMALL = Content.Load<SpriteFont>("FontsMainMenu_SMALL");
            STATE_MAINMENU = Content.Load<Texture2D>("state_MainMenu");
            mainMenu = new MainMenu(this, STATE_MAINMENU, FMainMenu_SMALL, FMainMenu_LARGE);
            Components.Add(mainMenu);

            //state_CONTROLLERS image is going to display at the controllersMenu scene (state)
            STATE_CONTROLLERS = Content.Load<Texture2D>("state_controllers");
            controllersMenu = new ControllersMenu(this, STATE_CONTROLLERS);
            Components.Add(controllersMenu);

            //Ship Upgrade State
            STATE_UPGRADESHIP = Content.Load<Texture2D>("state_AddFeature");

            displayPlayer = Content.Load<Model>("model_PLAYERSHIP");//Display the larger playership at the customizing menu
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            setupShip = new SetupShip(this, STATE_UPGRADESHIP, STATE_ERROR01 ,displayPlayer, FMainMenu_LARGE, aspectRatio);//temporary font; try to change it!
            Components.Add(setupShip);

            checkEmbark = new CheckEmbark(this, STATE_ERROR01);
            Components.Add(checkEmbark);
            

            //This section will be to load the storyline of the game (before they play and after customize)
            STATE_STORY01 = Content.Load<Texture2D>("intro_Story01");
            story01 = new Story01(this, STATE_STORY01);
            Components.Add(story01);

            STATE_STORY02 = Content.Load<Texture2D>("intro_Story02");
            story02 = new Story02(this, STATE_STORY02);
            Components.Add(story02);

            STATE_STORY03 = Content.Load<Texture2D>("intro_Story03");
            story03 = new Story03(this, STATE_STORY03);
            Components.Add(story03);

            STATE_STORY04 = Content.Load<Texture2D>("intro_Story04");
            story04 = new Story04(this, STATE_STORY04);
            Components.Add(story04);

            STATE_STORY05 = Content.Load<Texture2D>("intro_Story05");
            story05 = new Story05(this, STATE_STORY05);
            Components.Add(story05);
            ///////////////////END OF STORY////////////////////////////////

            ////////////////////PROLOGUE DISPLAY////////////////////////////
            STATE_PROLOGUE01 = Content.Load<Texture2D>("intro_PROLOGUE01");
            prologue01 = new Prologue01(this, STATE_PROLOGUE01);
            Components.Add(prologue01);

            STATE_PROLOGUE02 = Content.Load<Texture2D>("intro_PROLOGUE02");
            prologue02 = new Prologue02(this, STATE_PROLOGUE02);
            Components.Add(prologue02);

            STATE_PROLOGUECONTROLLS = Content.Load<Texture2D>("intro_PROLOGUE03");
            prologue04 = new Prologue04(this, STATE_PROLOGUECONTROLLS);
            Components.Add(prologue04);

            STATE_PROLOGUEMISSION = Content.Load<Texture2D>("state_Level00PROLOGUE");
            prologue03 = new Prologue03(this, STATE_PROLOGUEMISSION);
            Components.Add(prologue03);

            //PROLOGUE LEVEL
            SFBAYTEXTURE = Content.Load<Texture2D>("sfBay");
            prologueTop = Content.Load<Texture2D>("prologueTop");
            oilSpillTexture = Content.Load<Texture2D>("oilDot");
            level01Fonts = Content.Load<SpriteFont>("level01Fonts");
            STATE_PAUSE = Content.Load<Texture2D>("state_PAUSE");
            STATE_MISSIONFAIL = Content.Load<Texture2D>("state_MISSIONFAIL");
            STATE_MISSIONACCOMPLISH = Content.Load<Texture2D>("state_MISSIONACCOMPLISH");
            sideBar = Content.Load<Texture2D>("sideBar");
            radar = Content.Load<Texture2D>("sideBar_RADAR");
            radarStick = Content.Load<Texture2D>("radarStick");
            playerShip = Content.Load<Texture2D>("playerShip");
            hullGreen = Content.Load<Texture2D>("playerHull_green");
            hullYellow = Content.Load<Texture2D>("playerHull_yellow");
            hullOrange = Content.Load<Texture2D>("playerHull_orange");
            hullRed = Content.Load<Texture2D>("playerHull_red");
            box01 = Content.Load<Texture2D>("box01");
            box02 = Content.Load<Texture2D>("box02");
            STATE_state_MISSIONACCOMPLISH_PROLOGUE = Content.Load<Texture2D>("state_MISSIONACCOMPLISH_PROLOGUE");
            plasma = Content.Load<Texture2D>("plasmaBlue");
            cannon = Content.Load<Texture2D>("cannon");
            crosshair = Content.Load<Texture2D>("crossHair");
            playerLeft = Content.Load<Texture2D>("playerShipLeftSide");
            playerDown = Content.Load<Texture2D>("playerShipDown");
            playerUp = Content.Load<Texture2D>("playerShipUp");
            levelPrologue = new LevelPrologue(this, playerShip, playerLeft, playerDown, playerUp, crosshair, graphics, cannon, plasma, oilSpillTexture, STATE_PAUSE, STATE_state_MISSIONACCOMPLISH_PROLOGUE, SFBAYTEXTURE, prologueTop, level01Fonts);
            Components.Add(levelPrologue);

            //////////////////////////////////////LEVEL 01////////////////////////////////////////////////////////////
            //LEVEL 01 Mission Objectives
            STATE_LEVEL01MISSION = Content.Load<Texture2D>("state_Level01Mission");
            level1READER = new level01_description(this, STATE_LEVEL01MISSION);
            Components.Add(level1READER);

            //LEVEL 01.1
            statusUpdateBoxes = Content.Load<Texture2D>("statusUpdateBoxes");
            statusUpdateENEMY01 = Content.Load<Texture2D>("statusUpdateENEMYQUOTE01");
            statusUpdateMAYDAY = Content.Load<Texture2D>("statusUpdateMAYDAY");
            oilRig02 = Content.Load<Texture2D>("oilRig02");
            enemyJetFighterTopView = Content.Load<Texture2D>("fighterJet01");
            STATE_LEVEL0101_COMPLETE = Content.Load<Texture2D>("level0101_COMPLETE");
            oceanLevel01_Background = Content.Load<Texture2D>("oceanBackground_Level01");
            lev01 = new level01(this, playerShip, playerLeft, playerDown, playerUp, crosshair, graphics, cannon,
                plasma, oceanLevel01_Background, oilRig02, sideBar, statusUpdateBoxes, statusUpdateMAYDAY, statusUpdateENEMY01, hullGreen, hullYellow,
                hullOrange, hullRed, oilSpillTexture, STATE_PAUSE, STATE_LEVEL0101_COMPLETE, STATE_MISSIONFAIL, 
                box01, box02, enemyJetFighterTopView, level01Fonts);
            Components.Add(lev01);

            //BOSS STORY//////////////////////////////////////////////////////
            STATE_BOSSSTORY01 = Content.Load<Texture2D>("level01Boss_Story01");
            bossStory01 = new BossStory01(this,STATE_BOSSSTORY01);
            Components.Add(bossStory01);

            STATE_BOSSSTORY02 = Content.Load<Texture2D>("level01_BossStory02");
            bossStory02 = new BossStory02(this, STATE_BOSSSTORY02);
            Components.Add(bossStory02);

            STATE_BOSSSTORYOBJ = Content.Load<Texture2D>("state_Leve02Mission");
            bossStoryObj = new BossStoryObj(this, STATE_BOSSSTORYOBJ);
            Components.Add(bossStoryObj);
            //////////////END-OF-STORY///////////////////////////////////

            //LEVEL 1.2////////////////////////////////////////////////////////////////
            STATE_LEVEL0102_COMPLETE = Content.Load<Texture2D>("level0102_Complete");
            sideBar_Level0102 = Content.Load<Texture2D>("sideBar_level0102");
            enemyGunboat_leftPoint = Content.Load<Texture2D>("enemyGunboat_leftPoint");
            ocean1 = Content.Load<Texture2D>("ocean1");
            level01_02 = new Level01_02(this,playerShip,playerLeft,playerDown,playerUp,crosshair,graphics,cannon,plasma,
                sideBar_Level0102, box01, box02, ocean1, hullGreen, hullOrange, hullYellow, hullRed, STATE_PAUSE, STATE_LEVEL0102_COMPLETE, STATE_MISSIONFAIL,
                enemyJetFighterTopView, EXPOLSION_PARTICLES, SMOKE_PARTICLES, level01Fonts);
            Components.Add(level01_02);

            //LEVEL 1.3 - THE BOSS///////////////////////////////////////////////////
            playerShipSideView = Content.Load<Texture2D>("playerShipSide");
            enemyJetFighter = Content.Load<Texture2D>("fighterJet04");
            levelBackground0102 = Content.Load<Texture2D>("level0102_background");
            oilRig = Content.Load<Texture2D>("oilRig01");
            bottomStatusBar = Content.Load<Texture2D>("bottomSideBar");
            bossShip01 = Content.Load<Texture2D>("eShip03");
            missile = Content.Load<Texture2D>("missile05");
            missile1 = Content.Load<Texture2D>("missile");
            bossStatus_GREEN = Content.Load<Texture2D>("bossGreen");
            bossStatus_YELLOW = Content.Load<Texture2D>("bossYellow");
            bossStatus_ORANGE = Content.Load<Texture2D>("bossOrange");
            bossStatus_RED = Content.Load<Texture2D>("bossRed");
            boss01Fonts = Content.Load<SpriteFont>("boss01Fonts");
            crosshair2 = Content.Load<Texture2D>("crossHair");
            level01_03 = new Level01_03(this, ref graphics, ref smokeTexture, playerShipSideView, enemyJetFighter, bossShip01, missile, missile1, crosshair2,
                cannon, plasma, bottomStatusBar, STATE_PAUSE, STATE_MISSIONACCOMPLISH, STATE_MISSIONFAIL, levelBackground0102, oilRig,
                hullGreen, hullYellow, hullOrange, hullRed, bossStatus_GREEN, bossStatus_YELLOW, bossStatus_ORANGE,
                bossStatus_RED, level01Fonts, boss01Fonts);
            Components.Add(level01_03);

            /////////////////////////LEVEL 02////////////////////////////////////////////////////////////////
            LEVELSTORYSTATE0201 = Content.Load<Texture2D>("level02s01");
            levelStory0201 = new LevelStory0201(this, LEVELSTORYSTATE0201);
            Components.Add(levelStory0201);

            LEVELSTORYSTATE0202 = Content.Load<Texture2D>("level02s02");
            levelStory0202 = new LevelStory0202(this, LEVELSTORYSTATE0202);
            Components.Add(levelStory0202);

            LEVELSTORYSTATE0203 = Content.Load<Texture2D>("level02s03");
            levelStory0203 = new LevelStory0203(this, LEVELSTORYSTATE0203);
            Components.Add(levelStory0203);

            LEVELSTORYSTATE0204 = Content.Load<Texture2D>("level02s04");
            levelStory0204 = new LevelStory0204(this, LEVELSTORYSTATE0204);
            Components.Add(levelStory0204);

            LEVELSTORYSTATE0205 = Content.Load<Texture2D>("level02s05");
            levelStory0205 = new LevelStory0205(this, LEVELSTORYSTATE0205);
            Components.Add(levelStory0205);

            DISPLAYLEVEL0201 = Content.Load<Texture2D>("state_Level02");
            level0201PROMPT = new Level0201PROMPT(this, DISPLAYLEVEL0201);
            Components.Add(level0201PROMPT);

            chopperDown = Content.Load<Texture2D>("chopperDown");
            oceanLevel02 = Content.Load<Texture2D>("oceanLevel02");
            sideBar02 = Content.Load<Texture2D>("sideBarLevel0201");
            alpha01c = Content.Load<Texture2D>("alpha1");
            alpha02c = Content.Load<Texture2D>("alpha2");
            alpha03c = Content.Load<Texture2D>("alpha3");
            loadingCrewBar = Content.Load<Texture2D>("loadingCrewBar");
            LEVEL0201_MISSIONFAIL = Content.Load<Texture2D>("missionFail_state_Level0201");
            LEVEL0201_MISSIONSUCESS = Content.Load<Texture2D>("missionAccomplish_state_Level0201");
            level02_01 = new Level02_01(this, oceanLevel02, playerShip, playerLeft, playerDown, playerUp, sideBar02,
                graphics, chopperDown, alpha01c, alpha02c, alpha03c, loadingCrewBar, STATE_PAUSE, LEVEL0201_MISSIONSUCESS, LEVEL0201_MISSIONFAIL, level01Fonts);
            Components.Add(level02_01);

            DISPLAYLEVEL0202 = Content.Load<Texture2D>("state_Level02_2");
            level0202PROMPT = new Level0202PROMPT(this, DISPLAYLEVEL0202);
            Components.Add(level0202PROMPT);

            //////////////END-OF-STORY///////////////////////////////////

            ///////////////CREDITS//////////////////////////////////////
            credit_title = Content.Load<Texture2D>("endingTitle");
            blackBackground = Content.Load<Texture2D>("blackBackground");
            credits_before = new Credits_Before(this, credit_title, blackBackground, level01Fonts);
            Components.Add(credits_before);

            STATE_CREDITS = Content.Load<Texture2D>("state_credits");
            credits = new Credits(this, STATE_CREDITS);
            Components.Add(credits);
            //////////////END OF CREDITS///////////////////////////////

            
            //ABOUT SCENE
            STATE_ABOUTSCENE = Content.Load<Texture2D>("state_about");
            aboutScene = new AboutScene(this, STATE_ABOUTSCENE);
            Components.Add(aboutScene);


            //Start the game in the start Scene :)
            //level01_03.Show();
            //activeScene = level01_03;
            introScene.Show();
            activeScene = introScene;

        }
        /********************************************************************/
        protected override void Update(GameTime gameTime)// Updates the controllers
        {
            //Handles the controller of the menus
            HandleScenesInput();
            base.Update(gameTime);
        }
        /********************************************************************/
        protected void ShowScene(GameScene scene)
        {
            activeScene.Hide();
            activeScene = scene;
            scene.Show();
        }
        /********************************************************************/
        private void HandleStartSceneInput()
        {
            if (CheckEnterA())
            {
                audio.MenuSelect.Play();
                switch (mainMenu.SelectedMenuIndex)
                {
                    case 0:
                        ShowScene(setupShip);//"Let the game begin.." -Jigsaw, from the movie Saw
                        break;
                    case 1:
                        ShowScene(controllersMenu);//Controllers help
                        break;
                    case 2:
                        ShowScene(aboutScene);//Credits should go here!
                        break;
                    case 3:
                        Exit();//Exits the game
                        break;
                }
            }
        }
        /********************************************************************/
        /// <summary>
        /// Handle input of all game scenes.This function is derive at the ..Update() Method
        /// </summary>
        private void HandleScenesInput()
        {
            // Handle Start Scene Input
            if (activeScene == mainMenu)
                HandleStartSceneInput();//Main menu controllers

            else if (activeScene == introScene)//Introduction of the game
            {
                if (CheckEnterA())
                    ShowScene(mainMenu);//Press enter to return to main menu
            }
            else if (activeScene == aboutScene)
            {
                if (CheckEnterA())
                    ShowScene(mainMenu);//Press enter to return to main menu
            }
            else if (activeScene == controllersMenu)
            {
                if (CheckEnterA())
                    ShowScene(mainMenu);//Press enter to return to main menu
            }

             //SETUP SHIP SCENE
            else if (activeScene == setupShip)
                PreGameHandleInput();//Users will select the amount of parts to the ship

            else if (activeScene == checkEmbark)
                CHECK_EMBARK_KEYS();

            //PROLOGUE INTRO STORY
            else if ((activeScene == prologue01))
                PROLOGUE01KEY();
            else if ((activeScene == prologue02))
                PROLOGUE02KEY();
            else if ((activeScene == prologue03))
                PROLOGUE03KEY();
            else if ((activeScene == prologue04))
                PROLOGUE04KEY();

            //PROLOGUE LEVEL PLAY
            else if (activeScene == levelPrologue)
                PROLOGUEPlayInput();

            //LEVEL 01 INTRO STORY
            else if ((activeScene == story01))
                StoryLineInput();//This function will give the user to skip the story or go frame-by-frame
            else if ((activeScene == story02))
                StoryLineInput02();
            else if ((activeScene == story03))
                StoryLineInput03();
            else if ((activeScene == story04))
                StoryLineInput04();
            else if ((activeScene == story05))
                StoryLineInput05();
            else if (activeScene == level1READER)
                AfterStoryLineInput();

            //LEVEL 01
            else if (activeScene == lev01)//Game play controllers
                GamePlayInput();

           //LEVEL 01.2->RUSH HOUR
            else if (activeScene == bossStory01)//Game play controllers
                BOSSSTORY01KEY();
            else if (activeScene == bossStory02)//Game play controllers
                BOSSSTORY02KEY();
            else if (activeScene == bossStoryObj)//Game play controllers
                BOSSSTORY03KEY();

            else if (activeScene == level01_02)
                LEVEL_0102_KEY();

            //LEVEL 01.2->BOSS LEVEL
            else if (activeScene == level01_03)
                GamePlayInputBoss0102();

            //LEVEL 02
            else if (activeScene == levelStory0201)//Game play controllers
                LEVEL02_STORY01();
            else if (activeScene == levelStory0202)//Game play controllers
                LEVEL02_STORY02();
            else if (activeScene == levelStory0203)//Game play controllers
                LEVEL02_STORY03();
            else if (activeScene == levelStory0204)//Game play controllers
                LEVEL02_STORY04();
            else if (activeScene == levelStory0205)//Game play controllers
                LEVEL02_STORY05();
            else if (activeScene == level0201PROMPT)//Game play controllers
                LEVEL02_PROMPT0201();
            //LEVEL 02.1
            else if (activeScene == level02_01)//Game play controllers
                LEVEL02_PLAY();
            else if (activeScene == level0202PROMPT)
                Level0202PROMPT();
            //LEVEL 02.2
            //else if (activeScene == level02_02)

            //CREDITS
            else if (activeScene == credits_before)
                BEFORE_CREDITSKEY();
            else if (activeScene == credits)
                CREDITSKEY();
        }
        /********************************************************************/
        //CONTROLLERS FUNCTIONS*********************************************/
        
        //////////INTRO SCENE////////////////////////////////////////////////////
        private void DEMOKEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(mainMenu);
        }
        /////////////////////////////////////////////////////////////////////////
        private bool CheckEnterA()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool result = (oldKeyboardState.IsKeyDown(Keys.Enter) &&
                (keyboardState.IsKeyUp(Keys.Enter)));
            result |= (oldGamePadState.Buttons.A == ButtonState.Pressed) &&
                      (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;

            return result;
        }
        private void StoryLineInput()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(story02);//CHANGE TO SLIDE 2
            if (backKey)
                ShowScene(mainMenu);//Exit the level and returns to the main menu

        }
        private void StoryLineInput02()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(story03);//Goes to story line 3
            else if (backKey)
                ShowScene(story01);

        }
        private void StoryLineInput03()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(story04);//Goes to story line 4
            else if (backKey)
                ShowScene(story02);
        }
        private void StoryLineInput04()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(story05);
            else if (backKey)
                ShowScene(story03);
        }
        private void StoryLineInput05()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(level1READER);//Skip the slide and let the game begins!!! Muwahaha!
            else if (backKey)
                ShowScene(story04);
        }
        private void PROLOGUE01KEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(prologue02);//Skip the slide and let the game begins!!! Muwahaha!
            else if (backKey)
                ShowScene(mainMenu);
            //ShowScene(prologue02)
            //ShowScene(level01_03)
        }
        private void PROLOGUE02KEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(prologue03);//Skip the slide and let the game begins!!! Muwahaha!
            else if (backKey)
                ShowScene(prologue01);
        }
        private void PROLOGUE03KEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(prologue04);//Skip the slide and let the game begins!!! Muwahaha!
            else if (backKey)
                ShowScene(prologue02);
        }
        private void PROLOGUE04KEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(levelPrologue);//Skip the slide and let the game begins!!! Muwahaha!
            else if (backKey)
                ShowScene(prologue03);
        }
        private void AfterStoryLineInput()
        {
            // Get the Keyboard and GamePad state
            KeyboardState keyboardState = Keyboard.GetState();
            if ((keyboardState.IsKeyDown(Keys.Space)))
            {
                lev01.SpeedPower = setupShip.SpeedPower;
                ShowScene(lev01);//Skip the slide and let the game begins!!! Muwahaha!
            }
        }
        //This function takes you to the game after reading the mission
        private void PreGameHandleInput()////////////////CHANGE HERE/////////////////////////////
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (enterKey)
            {
                if (setupShip.TotalPowerValue == 0)
                {
                    levelPrologue.SpeedPower = setupShip.SpeedPower;
                    ShowScene(prologue01);//prologue01//level02_01
                }
                else
                    ShowScene(checkEmbark);
            }
            if (backKey)
            {
                setupShip.Reset();//Resets the crew value
                ShowScene(mainMenu);
            }
        }
        private void CHECK_EMBARK_KEYS()//ERROR KEY STATE
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (enterKey)
                ShowScene(setupShip);
        }
        ////////////////////////////////////////////////////////////////////////////////////////
        private void PROLOGUEPlayInput()//GamePlay controller
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;

            if (enterKey)
            {
                audio.MenuBack.Play();
                levelPrologue.Paused = !levelPrologue.Paused;
            }

            if (levelPrologue.MissionComplete == true)
            {
                if (spaceKey)
                {
                    ShowScene(story01);
                }
            }

            if (backKey)//Exit the game to the main menu
            {
                setupShip.Reset();//Resets the crew value
                ShowScene(mainMenu);//Goes back to the main menu
                MediaPlayer.Play(audio.StartMusic);
            }
        }
        private void GamePlayInput()//GamePlay controller
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;

            if (enterKey)
            {
                audio.MenuBack.Play();
                lev01.Paused = !lev01.Paused;
            }

            if (backKey)//Exit the game to the main menu
            {
                setupShip.Reset();//Resets the crew value
                ShowScene(mainMenu);//Goes back to the main menu
                MediaPlayer.Play(audio.StartMusic);
            }
            if (lev01.isMissionFail == true)
            {
                if (keyboardState.IsKeyDown(Keys.R))
                {
                    //RESETS THE LEVEL IF FAIL
                    setupShip.Reset();
                    ShowScene(lev01);
                }
            }
            if (lev01.isMissionAccomplish == true)
            {
                if (spaceKey)
                    ShowScene(bossStory01);//Goes to the boss story/level
            }

            //if (spaceKey)
            //{
            //    ShowScene(level01_03);//Face the boss
            //}
        }
        private void BOSSSTORY01KEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(bossStory02);//Skip the slide and let the game begins!!! Muwahaha!
        }
        private void BOSSSTORY02KEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(bossStoryObj);//Skip the slide and let the game begins!!! Muwahaha!
            else if (backKey)
                ShowScene(bossStory01);
        }
        private void BOSSSTORY03KEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(level01_02);//Skip the slide and let the game begins!!! Muwahaha!
            else if (backKey)
                ShowScene(bossStory02);
        }
        private void LEVEL_0102_KEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;

            if (enterKey)
            {
                audio.MenuBack.Play();
                level01_02.Paused = !level01_02.Paused;
            }

            if (backKey)//Exit the game to the main menu
            {
                setupShip.Reset();//Resets the crew value
                ShowScene(mainMenu);//Goes back to the main menu
                MediaPlayer.Play(audio.StartMusic);
            }
            if (level01_02.isMissionFail == true)
            {
                if (keyboardState.IsKeyDown(Keys.R))
                {
                    //RESETS THE LEVEL IF FAIL
                    setupShip.Reset();
                    ShowScene(level01_02);
                }
            }
            if (level01_02.isMissionAccomplish == true)
            {
                if (spaceKey)
                {
                    MediaPlayer.Play(audio.Level02Transition);
                    ShowScene(level01_03);//Goes to the boss level
                }
            }
        }

        private void GamePlayInputBoss0102()//GamePlay controller
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;

            if (enterKey)
            {
                audio.MenuBack.Play();
                level01_03.Paused = !level01_03.Paused;
            }

            if (backKey)//Exit the game to the main menu
            {
                setupShip.Reset();//Resets the crew value
                ShowScene(mainMenu);//Goes back to the main menu
                MediaPlayer.Play(audio.StartMusic);
            }
            if (level01_03.isMissionFail == true)
            {
                if (keyboardState.IsKeyDown(Keys.R))
                {
                    //RESETS THE LEVEL IF FAIL
                    setupShip.Reset();
                    ShowScene(level01_03);
                }
            }
            if (level01_03.isMissionAccomplish == true)
            {
                if (spaceKey)
                {
                    MediaPlayer.Play(audio.Level02Transition);
                    ShowScene(levelStory0201);//Goes to the boss story/level
                }
            }
        }

        //LEVEL 2 CONTROLLERS//////////////////////////////////////////////////
        private void LEVEL02_STORY01()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(levelStory0202);
        }
        private void LEVEL02_STORY02()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(levelStory0203);
            else if (backKey)
                ShowScene(levelStory0201);
        }
        private void LEVEL02_STORY03()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(levelStory0204);
            else if (backKey)
                ShowScene(levelStory0202);
        }
        private void LEVEL02_STORY04()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(levelStory0205);
            else if (backKey)
                ShowScene(levelStory0203);
        }
        private void LEVEL02_STORY05()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(level0201PROMPT);//GO TO LEVEL0201 GAMEPLAY!
            else if (backKey)
                ShowScene(levelStory0204);
        }
        private void LEVEL02_PROMPT0201()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(level02_01);//GO TO LEVEL0201 GAMEPLAY!
            else if (backKey)
                ShowScene(levelStory0204);
        }
        //LEVEL 02.1
        private void LEVEL02_PLAY()
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;

            if (enterKey)
            {
                audio.MenuBack.Play();
                level02_01.Paused = !level02_01.Paused;
            }

            if (backKey)//Exit the game to the main menu
            {
                setupShip.Reset();//Resets the crew value
                ShowScene(mainMenu);//Goes back to the main menu
                MediaPlayer.Play(audio.StartMusic);
            }
            if (level02_01.isMissionFail == true)
            {
                if (keyboardState.IsKeyDown(Keys.R))
                {
                    //RESETS THE LEVEL IF FAIL
                    setupShip.Reset();
                    ShowScene(level02_01);
                }
            }
            if (level02_01.isMissionAccomplish == true)
            {
                if (spaceKey)
                    ShowScene(level0202PROMPT);//Goes to the boss story/level
            }
        }
        //LEVEL 02.2 PROMPT
        void Level0202PROMPT()
        {
                        // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(credits_before);//GO TO LEVEL0201 GAMEPLAY!
            else if (backKey)
                ShowScene(mainMenu);
        }
        //LEVEL 02.2
        //LEVEL 02.3
        //////////////////////////////////////////////////////////////////////
        //LEVEL 3/////////////////////////////////////////////////////////////
        //LEVEL 3.1
        //LEVEL 3.2
        /////////////////////////////////////////////////////////////////////
        //CREDITS////////////////////////////////////////////////////////////
        void BEFORE_CREDITSKEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
           // if (spaceKey)

            if(credits_before.LoadingBar > 10)
                ShowScene(credits);//Returns to the main menu
        }
        void CREDITSKEY()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            //mgameTime.TotalGameTime.TotalMilliseconds*mdbLastTpress > 200
            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) && (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) && (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) && (gamepadState.Buttons.Start == ButtonState.Released);

            bool spaceKey = (oldKeyboardState.IsKeyDown(Keys.Space) && (keyboardState.IsKeyUp(Keys.Space)));
            enterKey |= (oldGamePadState.Buttons.A == ButtonState.Pressed) && (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (spaceKey)
                ShowScene(introScene);//Returns to the main menu
        }
        ////////////////////////////////////////////////////////////////////////
        /********************************************************************/
        protected override void Draw(GameTime gameTime)// Draws the game
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            spriteBatch.End();
        }
        /********************************************************************/
    }
}
