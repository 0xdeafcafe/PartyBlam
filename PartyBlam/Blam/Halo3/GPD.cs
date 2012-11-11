using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PartyBlam.IO;

namespace PartyBlam.Blam.Halo3
{
    public class GPD
    {
        private EndianStream _gpdStream;
        private PlayerVisuals _gpdPlayerVisuals;
        private IList<CampaignLevel> _gpdCampaignLevels;
        private IList<Skull> _gpdCampaignSkulls;

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
        public IList<Skull> GPDCampaignSkulls
        {
            get { return _gpdCampaignSkulls; }
            set { _gpdCampaignSkulls = value; }
        }
        #endregion

        #region Class Declaration
        public class PlayerVisuals
        {
            public string ServiceTag { get; set; }

            public Species PlayerSpecies { get; set; }

            public Spartan PlayerSpartan { get; set; }
            public Elite PlayerElite { get; set; }

            public class Spartan
            {
                public SpartanHelmet SpartanHelmet { get; set; }
                public SpartanShoulder SpartanLeftShoulder { get; set; }
                public SpartanShoulder SpartanRightShoulder { get; set; }
                public SpartanChest SpartanChest { get; set; }
            }
            public class Elite
            {
                public EliteHelmet EliteHelmet { get; set; }
                public EliteShoulder EliteLeftShoulder { get; set; }
                public EliteShoulder EliteRightShoulder { get; set; }
                public EliteChest EliteChest { get; set; }
            }

            public Colours PrimaryArmourColour { get; set; }
            public Colours SecondaryArmourColour { get; set; }
            public Colours DetailArmourColour { get; set; }

            public EmblemForegroundToggle EmblemToggle { get; set; }
            public EmblemForeground EmblemForeground { get; set; }
            public EmblemBackground EmblemBackground { get; set; }

            public Colours PrimaryEmblemColour { get; set; }
            public Colours SecondaryEmblemColour { get; set; }
            public Colours BackgroundEmblemColour { get; set; }
        }

        public class CampaignLevel
        {
            public CampaignLevelNames LevelName { get; set; }
            public CampaignProcessState ProgressSP { get; set; }

            public CompletionState CompletionSP { get; set; }
            public CompletionState CompletionCoop { get; set; }
        }

        public class Skull
        {
            public SkullName Name { get; set; }
            public string Description { get; set; }

            public bool Enabled { get; set; }
            public SkullType SkullType { get; set; }
        }
        #endregion

        #region Enum Declaration
        public enum SkullType
        {
            Gold,
            Silver
        }
        public enum SkullName
        {
            Iron,
            BlackEye,
            TouchLuck,
            Catch,
            Fog,
            Famine,
            Thunderstorm,
            Tilt,
            Mythic,
            Blind,
            Cowbell,
            GruntBirthdayParty,
            IWHBYD
        }

        public enum CompletionState
        {
            Nothing = 0x00,
            Easy = 0x01,
            Normal = 0x02,
            Heroic = 0x06,
            Legendary = 0x08
        }
        public enum CampaignProcessState
        {
            NoProgress = 0x00,
            MissionStart = 0x01,
            RallyPointAlpha = 0x03,
            RallyPointBravo = 0x07
        }
        public enum CampaignLevelNames
        {
            Arrival,
            Sierra117,
            CrowsNest,
            TsavoHighway,
            TheStorm,
            Floodgate,
            TheArk,
            TheCovenant,
            Cortana,
            Halo
        }

        public enum Species
        {
            Spartan,
            Elite
        }

        public enum EmblemForeground
        {
            SeventhColoumn,
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
            ChampionJollyRoger,
            Marathon,
            Cube,
            Radioactive,
            Smiley,
            Frowney,
            Spearhead,
            Sol,
            DoubleCrescent,
            Helmet,
            Triad,
            GruntSymbol,
            Surf,
            Thor,
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
            BruteHead,
            Runes,
            Trident,
            CrossedSwords,
            Unicorn,
            Wolf,
            BubbleShield,
            TipMine,
            Daisho,
            Daisy,
            Crosshairs,
            TheLibrarian,
            Tomcat,
            Supernova,
            FleurDeLis,
            BearClaw,
            FlamingHorns,
            BlackWidow,
            Ohnoudnt,
            Number1,
            Number2,
            Number3,
            Number4,
            Number5,
            Number6,
            Number7,
            Number8,
            Number9,
            Number0
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
            FaceMask2
        }
        public enum EmblemForegroundToggle
        {
            Toggled,
            Untoggled
        }

        public enum SpartanHelmet
        {
            MarkVI,
            CQB,
            EVA,
            Recon,
            EOD,
            Hayabusa,
            Security,
            Scout,
            ODST,
            MarkV,
            Rogue
        }
        public enum SpartanShoulder
        {
            MarkVI,
            CQB,
            EVA,
            Recon,
            EOD,
            Hayabusa,
            Security,
            Scout
        }
        public enum SpartanChest
        {
            MarkVI,
            CQB,
            EVA,
            Recon,
            Hayabusa,
            EOD,
            Scout,
            Katana,
            Bungie
        }

        public enum EliteHelmet
        {
            Combat,
            Assault,
            FlightMark,
            Ascetic,
            Commando
        }
        public enum EliteShoulder
        {
            Combat,
            Assault,
            FlightMark,
            Ascetic,
            Commando
        }
        public enum EliteChest
        {
            Combat,
            Assault,
            FlightMark,
            Ascetic,
            Commando
        }

        public enum Colours
        {
            Steel,
            Silver,
            White,
            Red,
            Mauve,
            Salmon,
            Orange,
            Coral,
            peach,
            Gold,
            Yellow,
            Pale,
            Sage,
            Green,
            Olive,
            Teal,
            Aqua,
            Cyan,
            Blue,
            Cobalt,
            Sapphire,
            Violet,
            Orchid,
            Lavender,
            Crimson,
            Rubine,
            Pink,
            Brown,
            Tan,
            Khaki
        }
        #endregion

        /// <summary>
        /// Initalize new instance of the Halo 3 GPD
        /// </summary>
        /// <param name="fileLocation">Path to the Halo 3 'halo.gpd' extracted from a Container file.</param>
        public GPD(string fileLocation) { Initalize(new MemoryStream(File.ReadAllBytes(fileLocation))); }
        /// <summary>
        /// Initalize new instance of the Halo 3 GPD
        /// </summary>
        /// <param name="fileLocation">Path to the Halo 3 'halo.gpd' extracted from a Container file.</param>
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
            LoadCampaignSkulls();
        }

        #region Loading Code
        /// <summary>
        /// Load the gpd's Player Visuals 
        /// </summary>
        public void LoadPlayerVisuals()
        {
            _gpdPlayerVisuals = new PlayerVisuals();

            _gpdStream.SeekTo(0x35C7);
            _gpdPlayerVisuals.PlayerSpecies = (Species)_gpdStream.ReadByte();
            if (_gpdPlayerVisuals.PlayerSpecies == Species.Spartan)
            {
                _gpdPlayerVisuals.PlayerSpartan = new PlayerVisuals.Spartan()
                {
                    SpartanHelmet = (SpartanHelmet)_gpdStream.ReadByte(),
                    SpartanLeftShoulder = (SpartanShoulder)_gpdStream.ReadByte(),
                    SpartanRightShoulder = (SpartanShoulder)_gpdStream.ReadByte(),
                    SpartanChest = (SpartanChest)_gpdStream.ReadByte()
                };
            }
            else
            {
                _gpdPlayerVisuals.PlayerElite = new PlayerVisuals.Elite()
                {
                    EliteHelmet = (EliteHelmet)_gpdStream.ReadByte(),
                    EliteLeftShoulder = (EliteShoulder)_gpdStream.ReadByte(),
                    EliteRightShoulder = (EliteShoulder)_gpdStream.ReadByte(),
                    EliteChest = (EliteChest)_gpdStream.ReadByte()
                };
            }

            _gpdStream.SeekTo(0x358F);
            _gpdPlayerVisuals.PrimaryArmourColour = (Colours)_gpdStream.ReadByte();
            _gpdStream.SeekTo(0x3593);
            _gpdPlayerVisuals.SecondaryArmourColour = (Colours)_gpdStream.ReadByte();
            _gpdStream.SeekTo(0x3597);
            _gpdPlayerVisuals.DetailArmourColour = (Colours)_gpdStream.ReadByte();

            _gpdStream.SeekTo(0x359B);
            _gpdPlayerVisuals.EmblemToggle = (EmblemForegroundToggle)_gpdStream.ReadByte();
            _gpdPlayerVisuals.EmblemForeground = (EmblemForeground)_gpdStream.ReadByte();
            _gpdPlayerVisuals.EmblemBackground = (EmblemBackground)_gpdStream.ReadByte();

            _gpdStream.SeekTo(0x359B);
            _gpdPlayerVisuals.PrimaryEmblemColour = (Colours)_gpdStream.ReadByte();
            _gpdPlayerVisuals.SecondaryEmblemColour = (Colours)_gpdStream.ReadByte();
            _gpdPlayerVisuals.BackgroundEmblemColour = (Colours)_gpdStream.ReadByte();

            _gpdStream.SeekTo(0x35A4);
            _gpdPlayerVisuals.ServiceTag = _gpdStream.ReadUTF16();
        }

        /// <summary>
        /// Load the gpd's Campaign Completion
        /// </summary>
        public void LoadCampaignCompletion()
        {
            _gpdCampaignLevels = new List<CampaignLevel>();

            for (int level = 0; level < 10; level++)
            {
                CampaignLevel campaignLevel = new CampaignLevel();
                campaignLevel.LevelName = (CampaignLevelNames)level;
                

                _gpdStream.SeekTo(0x34F8 + level);
                campaignLevel.CompletionSP = (CompletionState)_gpdStream.ReadByte();
                _gpdStream.SeekTo(0x3518 + level);
                campaignLevel.CompletionCoop = (CompletionState)_gpdStream.ReadByte();
                _gpdStream.SeekTo(0x354C + level);
                campaignLevel.ProgressSP = (CampaignProcessState)_gpdStream.ReadByte();

                _gpdCampaignLevels.Add(campaignLevel);
            }
        }

        /// <summary>
        /// Load the gpd's Campaign Skulls
        /// </summary>
        public void LoadCampaignSkulls()
        {
            _gpdCampaignSkulls = new List<Skull>();

            // Load Gold Skulls
            _gpdStream.SeekTo(0x356E);

            BitArray goldSkullsBitmask = new BitArray(BitConverter.GetBytes(_gpdStream.ReadInt16()));
            for (int skullBitmaskIndex = 0; skullBitmaskIndex < 9; skullBitmaskIndex++)
            {
                Skull skull = new Skull();
                skull.Enabled = goldSkullsBitmask[skullBitmaskIndex];
                skull.SkullType = SkullType.Gold;
                skull.Name = (SkullName)skullBitmaskIndex;

                #region Skill Description
                switch (skullBitmaskIndex)
                {
                    case 0:
                        skull.Description = "Death carries a heavy price...";
                        break;
                    case 1:
                        skull.Description = "Bash your way to better health.";
                        break;
                    case 2:
                        skull.Description = "Your foes always make every saving throw.";
                        break;
                    case 3:
                        skull.Description = "Pull pin. Count to three. Throw.";
                        break;
                    case 4:
                        skull.Description = "You'll miss those eyes in the back of your head.";
                        break;
                    case 5:
                        skull.Description = "Trust us. Bring a magazine.";
                        break;
                    case 6:
                        skull.Description = "Field promotions for everyone!";
                        break;
                    case 7:
                        skull.Description = "What was once resistance is now immunity.";
                        break;
                    case 8:
                        skull.Description = "Coverage under the covenant Health Plan.";
                        break;
                }
                #endregion

                _gpdCampaignSkulls.Add(skull);
            }

            _gpdStream.SeekTo(0x3573);
            goldSkullsBitmask = new BitArray(BitConverter.GetBytes((int)(_gpdStream.ReadByte() >> 1)));
            if (goldSkullsBitmask.Length != 0)
            {
                for (int skullBitmaskIndex = 0; skullBitmaskIndex < 4; skullBitmaskIndex++)
                {
                    Skull skull = new Skull();
                    skull.Enabled = goldSkullsBitmask[skullBitmaskIndex];
                    skull.SkullType = SkullType.Silver;
                    skull.Name = (SkullName)(skullBitmaskIndex + 9);

                    #region Skill Description
                    switch (skullBitmaskIndex)
                    {
                        case 0:
                            skull.Description = "Shoot from the hip.";
                            break;
                        case 1:
                            skull.Description = "More bang for your buck.";
                            break;
                        case 2:
                            skull.Description = "Light a match...";
                            break;
                        case 3:
                            skull.Description = "But a dog beat me over the fence.";
                            break;
                    }
                    #endregion

                    _gpdCampaignSkulls.Add(skull);
                }
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
            UpdateCampaignSkulls();
        }

        /// <summary>
        /// Update the gpd's Player Visuals 
        /// </summary>
        public void UpdatePlayerVisuals()
        {
            _gpdStream.SeekTo(0x35C7);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PlayerSpecies);
            if (_gpdPlayerVisuals.PlayerSpecies == Species.Spartan)
            {
                _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PlayerSpartan.SpartanHelmet);
                _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PlayerSpartan.SpartanLeftShoulder);
                _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PlayerSpartan.SpartanRightShoulder);
                _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PlayerSpartan.SpartanChest);
            }
            else
            {
                _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PlayerElite.EliteHelmet);
                _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PlayerElite.EliteLeftShoulder);
                _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PlayerElite.EliteRightShoulder);
                _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PlayerElite.EliteChest);
            }

            _gpdStream.SeekTo(0x358F);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PrimaryArmourColour);
            _gpdStream.SeekTo(0x3593);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.SecondaryArmourColour);
            _gpdStream.SeekTo(0x3597);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.DetailArmourColour);

            _gpdStream.SeekTo(0x359B);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.EmblemToggle);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.EmblemForeground);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.EmblemBackground);

            _gpdStream.SeekTo(0x359B);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.PrimaryEmblemColour);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.SecondaryEmblemColour);
            _gpdStream.WriteByte((byte)_gpdPlayerVisuals.BackgroundEmblemColour);

            _gpdStream.SeekTo(0x35A4);
            _gpdStream.WriteUTF16(_gpdPlayerVisuals.ServiceTag);
            _gpdStream.SeekTo(0x35AC);
            _gpdStream.WriteUTF16(_gpdPlayerVisuals.ServiceTag);
        }

        /// <summary>
        /// Update the gpd's Campaign Completion
        /// </summary>
        public void UpdateCampaignCompletion()
        {
            int levelID = 0;
            foreach (CampaignLevel level in _gpdCampaignLevels)
            {
                _gpdStream.SeekTo(0x34F8 + levelID);
                _gpdStream.WriteByte((byte)level.CompletionSP);
                _gpdStream.SeekTo(0x3518 + levelID);
                _gpdStream.WriteByte((byte)level.CompletionCoop);
                _gpdStream.SeekTo(0x354C + levelID);
                _gpdStream.WriteByte((byte)level.ProgressSP);

                levelID++;
            }
        }

        /// <summary>
        /// Update the gpd's Campaign Skulls
        /// </summary>
        public void UpdateCampaignSkulls()
        {
            Int16 goldBitmask = 0;
            byte silverBitmask = 0;
            Int16[] nArray = new Int16[] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x100, 0x200, 0x400, 0x800, 0x1000, 0x2000 };

            int indexGold = 0;
            int indexSilver = 0;
            foreach(Skull skull in _gpdCampaignSkulls)
            {
                if (skull.SkullType == SkullType.Gold)
                {
                    goldBitmask = (Int16)(goldBitmask + ((indexGold != 9) ? nArray[goldBitmask] : 0));
                    indexGold++;
                }
                else
                {
                    silverBitmask = (byte)(silverBitmask + ((byte)nArray[indexSilver + 1]));
                    indexSilver++;
                }
            }

            byte[] goldSkullBytes = BitConverter.GetBytes(goldBitmask);

            _gpdStream.Endianness = Endian.LittleEndian;
            _gpdStream.SeekTo(0x356E);
            _gpdStream.WriteInt16(BitConverter.ToInt16(goldSkullBytes, 0));
            _gpdStream.Endianness = Endian.BigEndian;
            
            _gpdStream.SeekTo(0x3573);
            _gpdStream.WriteByte(silverBitmask);
        }
        #endregion

        public bool IsValidGPD()
        {
            _gpdStream.SeekTo(0x00);
            string magic = _gpdStream.ReadAscii(0x04);
            _gpdStream.SeekTo(0x344A);
            string gameName = _gpdStream.ReadAscii(0x0D);

            if (magic != "XDBF" && gameName != "Halo 3")
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