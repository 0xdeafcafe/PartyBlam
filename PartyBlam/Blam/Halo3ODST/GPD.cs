using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text;
using System.IO;
using PartyBlam.IO;
 
namespace PartyBlam.Blam.Halo3ODST 
{ 
    public class GPD
    {
        private EndianStream _gpdStream;
        private PlayerVisuals _gpdPlayerVisuals;
        private IList<CampaignLevel> _gpdCampaignLevels;

        #region Public Access
        public Stream GPDStream
        {
            get { return _gpdStream.BaseStream; }
        }
        public PlayerVisuals GPDPlayerVisuals
        {
            get { return _gpdPlayerVisuals; }
            set { _gpdPlayerVisuals = value; }
        }
        public IList<CampaignLevel> GPDCampaignLevels
        {
            get { return _gpdCampaignLevels; }
            set { _gpdCampaignLevels = value; }
        }
        #endregion

        #region Class Declaration
        public class PlayerVisuals
        {
            public string ServiceTag { get; set; }

            public Colours ArmourPrimaryColour { get; set; }
            public Colours ArmourDetailColour { get; set; }

            public Character Character { get; set; }
            public HelmetToggle HelmetOn { get; set; }

            public EmblemForegroundToggle EmblemToggle { get; set; }
            public EmblemForeground EmblemForeground { get; set; }
            public EmblemBackground EmblemBackground { get; set; }

            public Colours EmblemForegroundColour { get; set; }
            public Colours EmblemSecondaryColour { get; set; }
            public Colours EmblemBackgroundColour { get; set; }
        }

        public class CampaignLevel
        {
            public CampaignLevelNames LevelName { get; set; }

            public CompletionState CompletionSP { get; set; }
            public CompletionState CompletionCoop { get; set; }
        }
        #endregion

        #region Enum Declaration
        public enum CompletionState
        {
            Nothing = 0x00,
            Easy = 0x01,
            Normal = 0x02,
            Heroic = 0x04,
            Legendary = 0x09
        }
        public enum CampaignLevelNames
        {
            PreparingToDrop1,
            TayariPlaza,
            PreparingToDrop2,
            KizingoBlvd,
            ONIAlphaSite,
            NMPDHQ,
            KikowaniStn,
            DataHive,
            CoastalHighway,
            MombassaStreets
        }

        public enum HelmetToggle
        {
            Off,
            On
        }
        public enum Character
        {
            Rookie,
            Mickey,
            Dutch,
            Romeo,
            Buck,
            Johnson,
            Dare
        }

        public enum Colours
        {
            Black,
            Grey,
            Snow,
            Maroon,
            Brick,
            Rose,
            Brown,
            Woodland,
            Cocoa,
            Tan,
            Khaki,
            Desert,
            Coral,
            Gold,
            Sand,
            Sage,
            Olive,
            Drab,
            Forest,
            Green,
            SeaFoam,
            Teal,
            Aqua,
            Cyan,
            Blue,
            Cobalt,
            Ice,
            Violet,
            Lavender,
            Pink
        }

        public enum EmblemForeground
        {
            SeventhColumn,
            Bullseye,
            Vortex,
            Halt,
            Spartan,
            DaBomb,
            Trinity,
            Delta,
            Rampancy,
            Stuck,
            Phoenix,
            Champion,
            JollyRoger,
            Marathon,
            Cube,
            Radioactive,
            Smiley,
            Frowney,
            Spearhead,
            Sol,
            DoubleCrescent,
            YinYang,
            Helmet,
            Triad,
            GruntSymbol,
            Surf,
            THor,
            SkullKing,
            Triplicate,
            Subnova,
            FlamingNinja,
            Pirate,
            Spades,
            Clubs,
            Diamonds,
            Hearts,
            Wasp,
            MarkOfShame,
            Snake,
            Hawk,
            Lips,
            Capsule,
            Tsantsa,
            GasMask,
            Grenade,
            SpartanHelmet,
            Cancel,
            Valkyrie,
            Drone,
            Grunt,
            GruntHead,
            Runes,
            Trident,
            CrossedSwords,
            Unicorn,
            Wolf,
            BubbleShield,
            TripMine,
            Daisho,
            Daisy,
            Crosshairs,
            TheLibrarian,
            TomCat,
            Supernova,
            FleurDeLis,
            BearClaw,
            FlamingHorns,
            BlackWidow,
            Peaceafist,
            Number1,
            Number2,
            Number3,
            Number4,
            Number5,
            Number6,
            Number7,
            Number8,
            Number9,
            Number0,
            KeepItClean,
            SuperIntendent,
            NewMombasa,
            ONI,
            Optican,
            Planet,
            Tycho,
            Chaos,
            Leo,
            Webmaster,
            Chicken,
            Bulltrue,
            Elephant,
            Zebra,
            SpartanLeague,
            Infected
        }
        public enum EmblemBackground
        {
            Blank,
            Circle,
            Diamond,
            Plus,
            Square,
            Triangle,
            VerticleStripe,
            HorizontalStripe,
            Cleft,
            CrissCross,
            Chalice,
            PointedStar8,
            Star,
            CowboyHat,
            ThickStar,
            Banner,
            Diamonds4,
            Sun,
            Hexagon,
            VerticalHexagon,
            Octagon,
            Pentagon,
            InvertedPentagon,
            RacingStripes,
            HorizontalStripes,
            Gradient,
            HorizontalGradient,
            Oval,
            VerticalOval,
            BluntDiamond,
            BluntDiamond2,
            SharpDiamond,
            SharpDiamond2,
            Aero,
            DeltaWing,
            Asterisk,
            Blam,
            Blam2,
            Shield,
            Display,
            Dreamcatcher,
            Buzzsaw,
            FourPlots,
            BallOFire,
            Cog,
            Sprocket,
            FaceMask,
            FaceMask2,
            SmallCircle,
            Cancel,
            Crown,
            SnappyHat
        }
        public enum EmblemForegroundToggle
        {
            Toggled,
            Untoggled
        }
        #endregion

        /// <summary>
        /// Initalize new instance of the Halo 3: ODST GPD
        /// </summary>
        /// <param name="fileLocation">Path to the Halo 3 'halo.gpd' extracted from a Container file.</param>
        public GPD(string fileLocation) { Initalize(new MemoryStream(File.ReadAllBytes(fileLocation))); }
        /// <summary>
        /// Initalize new instance of the Halo 3: ODST GPD
        /// </summary>
        /// <param name="fileLocation">Path to the Halo 3: ODST 'halo.gpd' extracted from a Container file.</param>
        public GPD(Stream fileStream) { Initalize(fileStream); }
        private void Initalize(Stream fileStream)
        {
            _gpdStream = new EndianStream(fileStream, Endian.BigEndian);

            if (!IsValidGPD())
            {
                Close();
                throw new Exception("Invalid Halo 3 GPD!");
            }

            LoadPlayerVisuals();
            LoadCampaignCompletion();
        }

        #region Loading Code
        /// <summary>
        /// Load the gpd's Player Visuals 
        /// </summary>
        public void LoadPlayerVisuals()
        {
            _gpdPlayerVisuals = new PlayerVisuals();

            _gpdStream.SeekTo(0x25B4);
            _gpdPlayerVisuals.ServiceTag = _gpdStream.ReadUTF16();

            _gpdStream.SeekTo(0x35E1);
            _gpdPlayerVisuals.HelmetOn = (HelmetToggle)_gpdStream.ReadByte();
            _gpdStream.SeekTo(0x35DF);
            _gpdPlayerVisuals.Character = (Character)_gpdStream.ReadByte();

            _gpdStream.SeekTo(0x35AC);
            _gpdPlayerVisuals.EmblemForeground = (EmblemForeground)_gpdStream.ReadByte();
            _gpdPlayerVisuals.EmblemBackground = (EmblemBackground)_gpdStream.ReadByte();
            _gpdPlayerVisuals.EmblemToggle = (EmblemForegroundToggle)_gpdStream.ReadByte();

            _gpdStream.SeekTo(0x35AF);
            _gpdPlayerVisuals.EmblemForegroundColour = (Colours)_gpdStream.ReadByte();
            _gpdPlayerVisuals.EmblemSecondaryColour = (Colours)_gpdStream.ReadByte();
            _gpdPlayerVisuals.EmblemBackgroundColour = (Colours)_gpdStream.ReadByte();

            _gpdStream.SeekTo(0x359F);
            _gpdPlayerVisuals.ArmourPrimaryColour = (Colours)_gpdStream.ReadByte();
            _gpdStream.SeekTo(0x35A3);
            _gpdPlayerVisuals.ArmourDetailColour = (Colours)_gpdStream.ReadByte();
        }

        /// <summary>
        /// Load the gpd's Campaign Completion
        /// </summary>
        public void LoadCampaignCompletion()
        {
            _gpdCampaignLevels = new List<CampaignLevel>();
            
            // Load all levels
            for (int spLevel = 9; spLevel >= 0; spLevel--)
            {
                int coopLevel = 9 - spLevel;
                CampaignLevel campaignLevel = new CampaignLevel();

                _gpdStream.SeekTo(0x350D - spLevel);
                campaignLevel.CompletionSP = (CompletionState)_gpdStream.ReadByte();

                _gpdStream.SeekTo(0x352E + coopLevel);
                campaignLevel.CompletionSP = (CompletionState)_gpdStream.ReadByte();

                campaignLevel.LevelName = (CampaignLevelNames)coopLevel;

                _gpdCampaignLevels.Add(campaignLevel);
            }
        }
        #endregion

        #region Update Code
        /// <summary>
        /// Update the gpd's Player Visuals, Campaign Completion and Campaign Skulls
        /// </summary>
        public void Update()
        {
            UpdatePlayerVisuals();
            UpdateCampaignCompletion();
        }

        /// <summary>
        /// Update the gpd's Player Visuals 
        /// </summary>
        public void UpdatePlayerVisuals()
        {
            _gpdStream.SeekTo(0x25B4);
            _gpdStream.WriteUTF16(_gpdPlayerVisuals.ServiceTag);
            _gpdStream.SeekTo(0x35BE);
            _gpdStream.WriteUTF16(_gpdPlayerVisuals.ServiceTag);

            _gpdStream.SeekTo(0x35E1);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.HelmetOn);
            _gpdStream.SeekTo(0x35DF);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.Character);

            _gpdStream.SeekTo(0x35AC);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.EmblemForeground);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.EmblemBackground);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.EmblemToggle);

            _gpdStream.SeekTo(0x35AF);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.EmblemForegroundColour);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.EmblemSecondaryColour);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.EmblemBackgroundColour);

            _gpdStream.SeekTo(0x359F);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.ArmourPrimaryColour);
            _gpdStream.SeekTo(0x35A3);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.ArmourDetailColour);
        }

        /// <summary>
        /// Update the gpd's Campaign Completion
        /// </summary>
        public void UpdateCampaignCompletion()
        {
            // Write all things
            for (int spLevel = 9; spLevel >= 0; spLevel--)
            {
                int coopLevel = 9 - spLevel;
                CampaignLevel campaignLevel = _gpdCampaignLevels[coopLevel];

                _gpdStream.SeekTo(0x350D - spLevel);
                _gpdStream.WriteByte((byte)campaignLevel.CompletionSP);

                _gpdStream.SeekTo(0x352E + coopLevel);
                _gpdStream.WriteByte((byte)campaignLevel.CompletionSP);

                _gpdCampaignLevels.Add(campaignLevel);
            }
        }
        #endregion

        public bool IsValidGPD()
        {
            _gpdStream.SeekTo(0x00);
            string magic = _gpdStream.ReadAscii(0x04);
            _gpdStream.SeekTo(0x344A);
            string gameName = _gpdStream.ReadAscii(0x0D);

            if (magic != "XDBF" && gameName != "Halo 3: ODST")
                return false;
            else
                return true;
        }
        public void Close()
        {
            _gpdStream.Close();
        }
    }
}