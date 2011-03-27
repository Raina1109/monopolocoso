using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;


namespace TheWaver
{
    public class AudioLibrary
    {
        private SoundEffect explosion;
        private SoundEffect newMeteor;
        private SoundEffect menuBack;
        private SoundEffect menuSelect;
        private SoundEffect menuScroll;
        private SoundEffect powerGet;
        private SoundEffect powerShow;
        private SoundEffect healthSound;
        private SoundEffect failSound;
        private SoundEffect alertSound;
        private SoundEffect explosion02;
        private SoundEffect danger;
        private SoundEffect plasmaFire;
        private Song backMusic;
        private Song startMusic;
        private Song storyLineMusic;
        private Song bossStoryLine01Music;
        private Song level02Transition;
        private Song aboveAndBeyond;
        private Song creditsMusic;
        private Song baliCrossroads;
        private Song lastWarriorStanding;

        //SoundEffect
        public SoundEffect Explosion{get { return explosion; }}
        public SoundEffect NewMeteor{get { return newMeteor; } }
        public SoundEffect MenuBack{get { return menuBack; }}
        public SoundEffect MenuSelect{get { return menuSelect; }}
        public SoundEffect MenuScroll{get { return menuScroll; }}
        public SoundEffect PowerGet{get { return powerGet; }}
        public SoundEffect PowerShow{get { return powerShow; }}
        public SoundEffect HealthSound { get { return healthSound; } }
        public SoundEffect FailSound { get { return failSound; } }
        public SoundEffect AlertSound { get { return alertSound; } }
        public SoundEffect Explosion02 { get { return explosion02; } }
        public SoundEffect Danger { get { return danger; } }
        public SoundEffect PlasmaFire { get { return plasmaFire; } }

        //Music
        public Song BackMusic{get { return backMusic; }}
        public Song StartMusic{get { return startMusic; }}
        public Song StoryLineMusic { get { return storyLineMusic; } }
        public Song BossStoryLine01Music { get { return bossStoryLine01Music; } }
        public Song Level02Transition { get { return level02Transition; } }
        public Song AboveAndBeyond { get { return aboveAndBeyond; } }
        public Song CreditsMusic { get { return creditsMusic; } }
        public Song BaliCrossroads { get { return baliCrossroads; } }
        public Song LastWarriorStanding { get { return lastWarriorStanding; } }

        public void LoadContent(ContentManager Content)
        {
            explosion = Content.Load<SoundEffect>("explosion");
            explosion02 = Content.Load<SoundEffect>("explosion02");
            newMeteor = Content.Load<SoundEffect>("newmeteor");
            backMusic = Content.Load<Song>("Hells on Wheels");
            startMusic = Content.Load<Song>("Starships");
            bossStoryLine01Music = Content.Load<Song>("BlackFlag60");
            storyLineMusic = Content.Load<Song>("Dark Water Beast");
            level02Transition = Content.Load<Song>("BloodDeep");
            aboveAndBeyond = Content.Load<Song>("AboveAndBeyond");
            creditsMusic = Content.Load<Song>("endingCredits_Music");
            baliCrossroads = Content.Load<Song>("BaliCrossroads");
            lastWarriorStanding = Content.Load<Song>("LastWarriorStanding");
            menuBack = Content.Load<SoundEffect>("menu_back");
            menuSelect = Content.Load<SoundEffect>("menu_select3");
            menuScroll = Content.Load<SoundEffect>("menu_scroll");
            powerShow = Content.Load<SoundEffect>("powershow");
            powerGet = Content.Load<SoundEffect>("powerget");
            alertSound = Content.Load<SoundEffect>("alertSound");
            healthSound = Content.Load<SoundEffect>("healthSound");
            failSound = Content.Load<SoundEffect>("MissionFailSound");
            danger = Content.Load<SoundEffect>("danger");
            plasmaFire = Content.Load<SoundEffect>("laserFire");
        }
    }
}
