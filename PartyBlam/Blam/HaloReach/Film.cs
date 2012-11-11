using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyBlam.IO;

namespace PartyBlam.Blam.Reach
{
    public class Film
    {
        private EndianStream _filmStream;
        private string _filmPath;
        private PlayerTable _filmPlayerTable = new PlayerTable();
        private Header _filmHeader;
        private IList<Player> _filmPlayers;

        #region Public Access
        public Stream FileStream
        {
            get { return _filmStream.BaseStream; }
        }
        public string FilmPath
        {
            get { return _filmPath; }
        }
        public Header FilmHeader
        {
            get { return _filmHeader; }
            set { _filmHeader = value; }
        }
        public IList<Player> FilmPlayers
        {
            get { return _filmPlayers; }
            set { _filmPlayers = value; }
        }
        public PlayerTable FilmPlayerTable
        {
            get { return _filmPlayerTable; }
        }
        #endregion

        #region Class Declaration
        public class PlayerTable
        {
            public long PlayerTableStartPosition = 0x01DE11;
            public long PlayerTableEntryLength = 0x108;
        }

        public class Header
        {
            public string CreationAuthor { get; set; }
            public Int32 CreationDate { get; set; }

            public string ModifiedAuthor { get; set; }
            public Int32 ModifiedDate { get; set; }

            public string FilmName { get; set; }
            public string FilmDescription { get; set; }

            public string InfoString { get; set; }

            public string GametypeName { get; set; }
            public string GametypeDescription { get; set; }

            public string MapName { get; set; }
            public string RallyPoint { get; set; }

            public string MatchmakingPlaylistName { get; set; }
            public string UsermapName { get; set; }
            public string UsermapDescription { get; set; }
        }
        public class Player
        {
            public long EntryStartLocation { get; set; }
            public int EntryIndex { get; set; }

            public string Gamertag { get; set; }
            public string ServiceTag { get; set; }

            public Species PlayerSpecies { get; set; }

            public SpartanProperties SpartanProperties { get; set; }
            public EliteProperties EliteProperties { get; set; }
            public Visuals PlayerVisuals { get; set; }
        }

        public class EliteProperties
        {
            public EliteMilitaryRank EliteMilitaryRank { get; set; }
        }
        public class SpartanProperties
        {
            public SpartanGender SpartanGender { get; set; }
            public VisorColour VisorColour { get; set; }
            public Helmet Helmet { get; set; }
            public Shoulder LeftShoulder { get; set; }
            public Shoulder RightShoulder { get; set; }
            public Chest Chest { get; set; }
            public Wrist Wrist { get; set; }
            public Utility Utility { get; set; }
            public KneeGuards KneeGuards { get; set; }
        }
        public class Visuals
        {
            public EmblemForeground EmblemForeground { get; set; }
            public EmblemForegroundToggle EmblemForegroundToggle { get; set; }
            public EmblemBackground EmblemBackground { get; set; }

            public ArmourEffect ArmourEffect { get; set; }
            public FirefightVoice FirefightVoice { get; set; }

            public Colours ArmourPrimaryColour { get; set; }
            public Colours ArmourSecondaryColour { get; set; }
            public Colours EmblemPrimaryColour { get; set; }
            public Colours EmblemSecondaryColour { get; set; }
            public Colours EmblemBackgroundColour { get; set; }
        }
        #endregion

        #region Enum Declaration
        /// <summary>
        /// Offset: 0x37
        /// </summary>
        public enum SpartanGender
        {
            Male,
            Female
        }

        /// <summary>
        /// Offset: 0x3B
        /// </summary>
        public enum Species
        {
            Spartan,
            Elite
        }

        /// <summary>
        /// Offset: 0x3A
        /// </summary>
        public enum VisorColour
        {
            Default = 0x00,
            Silver = 0x01,
            Blue = 0x02,
            Gold = 0x03,
            Black = 0x04
        }

        /// <summary>
        /// Offset: 0x47
        /// </summary>
        public enum Helmet
        {
            MarkVBase = 0x00,
            MarkVUA = 0x01,
            MarkVUAHUL = 0x02,
            CQCBase = 0x03,
            CQCCBRN = 0x04,
            CQCUAHUL = 0x05,
            MilitaryPoliceBase = 0x06,
            MilitaryPoliceCBRNHURS = 0x07,
            MilitaryPoliceHURSCNM = 0x08,
            ODSTBase = 0x09,
            ODSTUACNM = 0x0A,
            ODSTCBRNHUL = 0x0B,
            HazopBase = 0x0C,
            HazopCBRNHUL = 0x0D,
            HazopCNMI = 0x0E,
            EODBase = 0x0F,
            EODCNM = 0x10,
            EODUANUL = 0x11,
            OperatorBase = 0x12,
            OperatorUAHUL = 0x13,
            OperatorCNM = 0x14,
            GrenadierBase = 0x15,
            GrenadierUA = 0x16,
            GrenadierUAFC = 0x17,
            AirAssaultBase = 0x18,
            AirAssaultUACNM = 0x19,
            AirAssaultFCI = 0x1A,
            ScoutBase = 0x1B,
            ScoutHURS = 0x1C,
            ScoutCBRNCNM = 0x1D,
            EVABase = 0x1E,
            EVACNM = 0x1F,
            EVAAUAHUL3 = 0x20,
            JFOBase = 0x21,
            JFOHULI = 0x22,
            JFOUA = 0x23,
            CommandoBase = 0x24,
            CommandoCBRNCNM = 0x25,
            CommandoUAFCI2 = 0x26,
            MjolnirMkVBase = 0x27,
            MjolnirMkVCNM = 0x28,
            MjolnirMkVUA = 0x29,
            PilotBase = 0x2A,
            PilotHUL3 = 0x2B,
            PilotUAHUL3 = 0x2C,
            PilotHaunted = 0x2D,
            SecurityBase = 0x2E,
            SecurityUAHUL = 0x2F,
            SecurityCBRNCNM = 0x30,
            ReconBase = 0x3A,
            ReconHUL = 0x3B,
            ReconUAHUL3 = 0x3C,
            EVACBase = 0x3D,
            EVACCNM = 0x3E,
            EVACHUL3 = 0x3F,
            MjolnirMkVIBase = 0x31,
            MjolnirMkVIFCI2 = 0x32,
            MjolnirMkVIUAHULI = 0x33,
            CQBBase = 0x34,
            CQBHURSCNM = 0x35,
            CQBUAHUL = 0x36,
            GungnirBase = 0x37,
            GungnirHURS = 0x38,
            GungnirCBRN = 0x39,
        }

        /// <summary>
        /// Left  Shoulder Offset: 0x48
        /// Right Shoulder Offset: 0x49
        /// </summary>
        public enum Shoulder
        {
            Default = 0x00,
            FJPara = 0x01,
            Hazop = 0x02,
            JFO = 0x03,
            Recon = 0x04,
            UAMultiThread = 0x05,
            JumpJet = 0x06,
            Eva = 0x07,
            Gungnir = 0x08,
            ODST = 0x11,
            UABaseSecurity = 0x09,
            CQC = 0x0A,
            Operator = 0x0B,
            Commando = 0x0C,
            Grenadier = 0x0D,
            Sniper = 0x0E,
            MjolnirMkV = 0x0F,
            Security = 0x10
        }

        /// <summary>
        /// Offset: 0x4A
        /// </summary>
        public enum Chest
        {
            Default = 0x00,
            UABaseSecurity = 0x01, // Lord Zedd r cool.
            UAMultiThreat = 0x02, // Lord Zedd r cool.
            HPHalo = 0x03,
            UACounterAssault = 0x04,
            TacticalLRP = 0x05,
            TacticalRecon = 0x06,
            CollarGrenadier = 0x07,
            TacticalPatrol = 0x08,
            CollarBreacher = 0x09,
            AssaultSapper = 0x0A,
            AssaultCommando = 0x0B,
            HPParafoil = 0x0C,
            CollarGrenadierUA = 0x0D,
            UAMultuThreatW = 0x0E,
            UABaseSecurityW = 0x0F,
            CollarBreacherR = 0x10,
            HPParafoilR = 0x11,
            AssaultSapperR = 0x12,
            UAODST = 0x13
        }

        /// <summary>
        /// Offset: 0x4B
        /// </summary>
        public enum Wrist
        {
            Default = 0x00,
            UABuckler = 0x01,
            UABracer = 0x02,
            TacticalTacPad = 0x03,
            AssaultBreacher = 0x04,
            TacticalUGPS = 0x05
        }

        /// <summary>
        /// Offset: 0x4C
        /// </summary>
        public enum Utility
        {
            Default = 0x00,
            TacticalSoftCase = 0x01,
            UAChobam = 0x02,
            UANxRA = 0x03,
            TacticalTraumaKit = 0x04,
            TacticalHardCase = 0x05
        }

        /// <summary>
        /// Offset: 0x4D
        /// </summary>
        public enum KneeGuards
        {
            Default = 0x00,
            JFPara = 0x01,
            Gungnir = 0x02,
            Grenadier = 0x03
        }

        /// <summary>
        /// Offset: 0x4E
        /// </summary>
        public enum EliteMilitaryRank
        {
            Minor = 0x00,
            SpecOps = 0x01,
            Ranger = 0x02,
            Ultra = 0x03,
            Zealot = 0x04,
            General = 0x05,
            FieldMarshall = 0x06,
            Officer = 0x07
        }

        /// <summary>
        /// Offset: 0x4F
        /// </summary>
        public enum ArmourEffect
        {
            Default = 0x00,
            RedFlames = 0x01,
            BlueFlames = 0x02,
            BirthdayParty = 0x03,
            HeartAttack = 0x04,
            Pestilence = 0x05,
            InclementWeather = 0x06
        }

        /// <summary>
        /// Offset: 0x51
        /// </summary>
        public enum FirefightVoice
        {
            NobleSix = 0x00,
            CortanaAI = 0x01,
            JohnS117 = 0x02,
            GYSGTBuck = 0x03,
            SGTMAJJohnson = 0x04,
            GYSGTStacker = 0x05,
            CarterS259 = 0x06,
            KatS320 = 0x07,
            JunS226 = 0x08,
            Emile239 = 0x09,
            JorgeS052 = 0x0A,
            AuntieDotAI = 0x0B
        }

        public enum Colours
        {
            Steel,
            Silver,
            White,
            Brown,
            Tan,
            Khaki,
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
            Orchid,
            Lavender,
            Maroon,
            Brick,
            Rose,
            Rust,
            Coral,
            Peach,
            Gold,
            Yellow,
            Pale,
            UltraWhite = 0x1F
        }
        public enum EmblemBackground
        {
            Blank,
            Circle,
            Diamond,
            Plus,
            Square,
            Triangle,
            VerticalStripe,
            HorizontalStripe,
            Cleft,
            CrissCross,
            PointedStar8,
            Star,
            CowboyHat,
            ThickStar,
            Banner,
            Diamonds4,
            Sun,
            Hexagon,
            VerticalHexagon,
            Chalice,
            Octagon,
            Pentagon,
            InvertedPentagon,
            RacingStripes,
            HorizontagStripes,
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
            DreamCatcher,
            BuzzSaw,
            FourPlots,
            BalloFire,
            Cog,
            Sprocket,
            FaceMask,
            FaceMask2,
            SmallCircle,
            Cancel,
            Crown,
            SnappyHat,
            ThreePlots
        }
        public enum EmblemForeground
        {
            SeventhColumn,
            BullsEye,
            Vortex,
            Halt,
            Spartan,
            PowerIsOn,
            SpartanLeague,
            Delta,
            Noted,
            Stuck,
            Phoenix,
            Champion,
            JollyRoger,
            ActiveRooster,
            Campfire,
            RadioActive,
            Smiley,
            Frowney,
            NoCampaing,
            Sol,
            DoubleCrescent,
            TinYang,
            Helmet,
            Triad,
            CupOfDeath,
            Rose,
            Thor,
            SkullKing,
            DogTags,
            Castle,
            FlamingNinja,
            Pirate,
            Spades,
            Clubs,
            Diamonds,
            Hearts,
            Wasp,
            Mombassa,
            Drone,
            Grunt,
            Lips,
            Capsule,
            Buffalo,
            GasMask,
            Jokers,
            SpartanHelmet,
            Atomic,
            Valkyrie,
            Headshot,
            ConeD,
            Leo,
            Bulltrue,
            Runes,
            Arrowhead,
            CrossedSwords,
            Unicorn,
            Wolf,
            Anchor,
            Chaos,
            Elephant,
            Daisy,
            Crosshairs,
            Infected,
            Tomcat,
            Supernova,
            FleurDeLis,
            BearClaw,
            FlamingHorns,
            BlackWidow,
            Peacefist,
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
        public enum EmblemForegroundToggle
        {
            Toggled,
            UnToggled
        }
        #endregion

        /// <summary>
        /// Initalize new instance of the Halo: Reach Film
        /// </summary>
        /// <param name="filmPath">Path to the Halo: Reach 'feature.film' extracted from a Container file.</param>
        public Film(string filmPath)
        {
            _filmPath = filmPath;
            _filmStream = new EndianStream(new MemoryStream(File.ReadAllBytes(_filmPath)), Endian.BigEndian);

            if (!IsValidFilm())
            {
                Close();
                throw new Exception("Invalid Halo Reach Film!");
            }

            LoadHeader();

            LoadPlayerTable();
        }
        /// <summary>
        /// Initalize new instance of the Halo: Reach Film
        /// </summary>
        /// <param name="filmStream">Stream of a Halo: Reach 'feature.film' extracted from a Container file.</param>
        public Film(Stream filmStream)
        {
            _filmPath = null;
            _filmStream = new EndianStream(filmStream, Endian.BigEndian);

            if (!IsValidFilm())
            {
                Close();
                throw new Exception("Invalid Halo Reach Film!");
            }

            LoadHeader();

            LoadPlayerTable();
        }

        #region Load Code
        /// <summary>
        /// Load the Film Header
        /// </summary>
        public void LoadHeader()
        {
            _filmHeader = new Header();

            #region Creation and Modification
            // Read Creation Date
            _filmStream.SeekTo(0x7C);
            _filmHeader.CreationDate = _filmStream.ReadInt32();

            // Read Creation Author
            _filmStream.SeekTo(0x88);
            _filmHeader.CreationAuthor = _filmStream.ReadAscii();

            // Read Modification Date
            _filmStream.SeekTo(0xA0);
            _filmHeader.ModifiedDate = _filmStream.ReadInt32();

            // Read Modification Author
            _filmStream.SeekTo(0xAC);
            _filmHeader.ModifiedAuthor = _filmStream.ReadAscii();
            #endregion
            #region Film Info
            // Read Film Name
            _filmStream.SeekTo(0xC0);
            _filmHeader.FilmName = _filmStream.ReadUTF16();

            // Read Film Description
            _filmStream.SeekTo(0x1C0);
            _filmHeader.FilmDescription = _filmStream.ReadUTF16();

            // Read Info String
            _filmStream.SeekTo(0x3D8);
            _filmHeader.InfoString = _filmStream.ReadAscii();
            #endregion
            #region Gametype Info
            // Read Gametype Name
            _filmStream.SeekTo(0x998);
            _filmHeader.GametypeName = _filmStream.ReadUTF16();

            // Read Gametype Description
            _filmStream.SeekTo(0xA98);
            _filmHeader.GametypeDescription = _filmStream.ReadUTF16();
            #endregion
            #region RawMapData
            // Read Map Name
            _filmStream.SeekTo(0x1011C);
            _filmHeader.MapName = _filmStream.ReadAscii();

            // Read Map Rally Point
            _filmStream.SeekTo(0x10220);
            _filmHeader.RallyPoint = _filmStream.ReadAscii();
            #endregion
            #region MatchmakingPlaylistInfo
            // Read Playlist Name
            _filmStream.SeekTo(0x10342);
            _filmHeader.MatchmakingPlaylistName = _filmStream.ReadUTF16();

            // Read Usermap Name
            _filmStream.SeekTo(0x10460);
            _filmHeader.UsermapName = _filmStream.ReadUTF16();

            // Read Usermap Description
            _filmStream.SeekTo(0x10560);
            _filmHeader.UsermapDescription = _filmStream.ReadUTF16();
            #endregion
        }

        /// <summary>
        /// Load Player Table
        /// </summary>
        public void LoadPlayerTable()
        {
            _filmPlayers = new List<Player>();

            bool contReadingPlayerTable = true;
            int playerTableIndex = 0;
            while (contReadingPlayerTable)
            {
                long currentEntryOffset = _filmPlayerTable.PlayerTableStartPosition + (_filmPlayerTable.PlayerTableEntryLength * playerTableIndex);

                _filmStream.SeekTo(currentEntryOffset + 0x0F);
                if (String.IsNullOrWhiteSpace(_filmStream.ReadUTF16()))
                    contReadingPlayerTable = false;
                else
                {
                    Player player = new Player();
                    player.SpartanProperties = new SpartanProperties();
                    player.EliteProperties = new EliteProperties();
                    player.PlayerVisuals = new Visuals();

                    // Write PlayerTable shit
                    player.EntryStartLocation = currentEntryOffset;
                    player.EntryIndex = playerTableIndex;

                    // Read Gamertag
                    _filmStream.SeekTo(currentEntryOffset + 0x0F);
                    player.Gamertag = _filmStream.ReadUTF16();

                    // Read ServiceTag
                    _filmStream.SeekTo(currentEntryOffset + 0x53);
                    player.ServiceTag = _filmStream.ReadUTF16();

                    // Read Species
                    _filmStream.SeekTo(currentEntryOffset + 0x3B);
                    player.PlayerSpecies = (Species)_filmStream.ReadByte();

                    // Read Gender          -0x37
                    _filmStream.SeekTo(currentEntryOffset + 0x37);
                    player.SpartanProperties.SpartanGender = (SpartanGender)_filmStream.ReadByte();

                    // Read Visor Colour    -0x3A
                    _filmStream.SeekTo(currentEntryOffset + 0x3A);
                    player.SpartanProperties.VisorColour = (VisorColour)_filmStream.ReadByte();

                    // Read Helmet          -0x47
                    _filmStream.SeekTo(currentEntryOffset + 0x47);
                    player.SpartanProperties.Helmet = (Helmet)_filmStream.ReadByte();

                    // Read Shoulders       -0x48/0x49
                    player.SpartanProperties.LeftShoulder = (Shoulder)_filmStream.ReadByte();
                    player.SpartanProperties.RightShoulder = (Shoulder)_filmStream.ReadByte();

                    // Read Chests          -0x4A
                    player.SpartanProperties.Chest = (Chest)_filmStream.ReadByte();

                    // Read Wrist           -0x4B
                    player.SpartanProperties.Wrist = (Wrist)_filmStream.ReadByte();

                    // Read Utility         -0x4C
                    player.SpartanProperties.Utility = (Utility)_filmStream.ReadByte();

                    // Read Knee Guards     -0x4D
                    player.SpartanProperties.KneeGuards = (KneeGuards)_filmStream.ReadByte();

                    // Load Elite Military Level -0x4E
                    player.EliteProperties.EliteMilitaryRank = (EliteMilitaryRank)_filmStream.ReadByte();

                    // Read Armour Effect   -0x4F
                    _filmStream.SeekTo(currentEntryOffset + 0x4F);
                    player.PlayerVisuals.ArmourEffect = (ArmourEffect)_filmStream.ReadByte();

                    // Load Firefight Voice -0x51
                    _filmStream.SeekTo(currentEntryOffset + 0x51);
                    player.PlayerVisuals.FirefightVoice = (FirefightVoice)_filmStream.ReadByte();

                    // Load Emblem/Colours
                    _filmStream.SeekTo(currentEntryOffset + 0x38);
                    player.PlayerVisuals.ArmourPrimaryColour = (Colours)_filmStream.ReadByte();                     // 0x38
                    player.PlayerVisuals.ArmourSecondaryColour = (Colours)_filmStream.ReadByte();                   // 0x39
                    _filmStream.SeekTo(currentEntryOffset + 0x3F);
                    player.PlayerVisuals.EmblemForeground = (EmblemForeground)_filmStream.ReadByte();               // 0x3F
                    player.PlayerVisuals.EmblemBackground = (EmblemBackground)_filmStream.ReadByte();               // 0x40
                    player.PlayerVisuals.EmblemForegroundToggle = (EmblemForegroundToggle)_filmStream.ReadByte();   // 0x41
                    player.PlayerVisuals.EmblemPrimaryColour = (Colours)_filmStream.ReadByte();                     // 0x42
                    player.PlayerVisuals.EmblemSecondaryColour = (Colours)_filmStream.ReadByte();                   // 0x43
                    player.PlayerVisuals.EmblemBackgroundColour = (Colours)_filmStream.ReadByte();                  // 0x44

                    // Add Player to List
                    _filmPlayers.Add(player);

                    playerTableIndex++;
                }
            }
        }
        #endregion

        #region Update Code
        /// <summary>
        /// Update the film's Header, and PlayerTable
        /// </summary>
        public void Update()
        {
            UpdateHeader();
            UpdatePlayerTable();
        }

        /// <summary>
        /// Update the film's Header
        /// </summary>
        public void UpdateHeader()
        {
            #region Creation and Modification
            // Write Creation Date
            _filmStream.SeekTo(0x7C);
            _filmStream.WriteInt32(_filmHeader.CreationDate);

            // Write Creation Author
            _filmStream.SeekTo(0x88);
            _filmStream.WriteAscii(_filmHeader.CreationAuthor);

            // Write Modification Date
            _filmStream.SeekTo(0xA0);
            _filmStream.WriteInt32(_filmHeader.ModifiedDate);

            // Write Modification Author
            _filmStream.SeekTo(0xAC);
            _filmStream.WriteAscii(_filmHeader.ModifiedAuthor);
            #endregion
            #region Film Info
            // Write Film Name
            _filmStream.SeekTo(0xC0);
            _filmStream.WriteUTF16(_filmHeader.FilmName);

            // Write Film Description
            _filmStream.SeekTo(0x1C0);
            _filmStream.WriteUTF16(_filmHeader.FilmDescription);

            // Write Info String
            _filmStream.SeekTo(0x3D8);
            _filmStream.WriteAscii(_filmHeader.InfoString);
            #endregion
            #region Gametype Info
            // Write Gametype Name
            _filmStream.SeekTo(0x998);
            _filmStream.WriteUTF16(_filmHeader.GametypeName);

            // Write Gametype Description
            _filmStream.SeekTo(0xA98);
            _filmStream.WriteUTF16(_filmHeader.GametypeDescription);
            #endregion
            #region RawMapData
            // Write Map Name
            _filmStream.SeekTo(0x1011C);
            _filmStream.WriteAscii(_filmHeader.MapName);

            // Write Map Rally Point
            _filmStream.SeekTo(0x10220);
            _filmStream.WriteAscii(_filmHeader.RallyPoint);
            #endregion
            #region MatchmakingPlaylistInfo
            // Write Playlist Name
            _filmStream.SeekTo(0x10342);
            _filmStream.WriteUTF16(_filmHeader.MatchmakingPlaylistName);

            // Write Usermap Name
            _filmStream.SeekTo(0x10460);
            _filmStream.WriteUTF16(_filmHeader.UsermapName);

            // Write Usermap Description
            _filmStream.SeekTo(0x10560);
            _filmStream.WriteUTF16(_filmHeader.UsermapDescription);
            #endregion
        }

        /// <summary>
        /// Update the film's Player Table
        /// </summary>
        public void UpdatePlayerTable()
        {
            foreach (Player player in _filmPlayers)
            {
                long EntryStartLocation = player.EntryStartLocation;
                long EntryIndex = player.EntryIndex;

                // Write Gamertag 1/2
                _filmStream.SeekTo(EntryStartLocation + 0x0F);
                _filmStream.WriteUTF16(player.Gamertag);
                _filmStream.SeekTo(EntryStartLocation + 0xC7);
                _filmStream.WriteUTF16(player.Gamertag);

                // Write Service Tag
                _filmStream.SeekTo(EntryStartLocation + 0x53);
                _filmStream.WriteUTF16(player.ServiceTag);

                // Write Species
                _filmStream.SeekTo(EntryStartLocation + 0x3B);
                _filmStream.WriteByte((byte)player.PlayerSpecies);

                // Write Gender
                _filmStream.SeekTo(EntryStartLocation + 0x37);
                _filmStream.WriteByte((byte)player.SpartanProperties.SpartanGender);

                // Write Visor Colour
                _filmStream.SeekTo(EntryStartLocation + 0x3A);
                _filmStream.WriteByte((byte)player.SpartanProperties.VisorColour);

                // Write Elite/Visuals/Spartan shit
                _filmStream.SeekTo(EntryStartLocation + 0x47);
                _filmStream.WriteByte((byte)player.SpartanProperties.Helmet);
                _filmStream.WriteByte((byte)player.SpartanProperties.LeftShoulder);
                _filmStream.WriteByte((byte)player.SpartanProperties.RightShoulder);
                _filmStream.WriteByte((byte)player.SpartanProperties.Chest);
                _filmStream.WriteByte((byte)player.SpartanProperties.Wrist);
                _filmStream.WriteByte((byte)player.SpartanProperties.Utility);
                _filmStream.WriteByte((byte)player.SpartanProperties.KneeGuards);
                _filmStream.WriteByte((byte)player.EliteProperties.EliteMilitaryRank);
                _filmStream.WriteByte((byte)player.PlayerVisuals.ArmourEffect);
                _filmStream.WriteByte((byte)player.PlayerVisuals.ArmourEffect);
                _filmStream.WriteByte((byte)player.PlayerVisuals.FirefightVoice);

                // Write Emblem/Colours
                _filmStream.SeekTo(EntryStartLocation + 0x38);
                _filmStream.WriteByte((byte)player.PlayerVisuals.ArmourPrimaryColour);
                _filmStream.WriteByte((byte)player.PlayerVisuals.ArmourSecondaryColour);
                _filmStream.SeekTo(EntryStartLocation + 0x3F);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemForeground);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemBackground);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemForegroundToggle);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemPrimaryColour);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemSecondaryColour);
                _filmStream.WriteByte((byte)player.PlayerVisuals.EmblemBackgroundColour);
                
            }
        }

        /// <summary>
        /// Close the Stream
        /// </summary>
        public void Close()
        {
            _filmStream.Close();
        }
        /// <summary>
        /// Check if the Film is Valid
        /// </summary>
        /// <returns></returns>
        public bool IsValidFilm()
        {
            _filmStream.SeekTo(0x0E);
            string _blfName = _filmStream.ReadAscii();

            if (_blfName == "reach saved film")
                return true;
            else
                return false;
        }
        #endregion

        /// <summary>
        /// Extract a player entry from the film
        /// </summary>
        /// <param name="entryIndex">Entry Index (User-Friendly, Not Computer Science Specific. ie 1, not 0 for position 0)</param>
        /// <param name="filePath">Path to extract too, include file name</param>
        public void ExtractPlayerEntry(int entryIndex, string filePath)
        {
            entryIndex--;

            long playerTableStartPosition = 0x01DE11;
            long playerTableEntryLength = 0x108;

            long currentEntryOffset = playerTableStartPosition + (playerTableEntryLength * entryIndex);

            _filmStream.SeekTo(currentEntryOffset + 0x0F);
            if (String.IsNullOrWhiteSpace(_filmStream.ReadUTF16()))
            {
                Close();
                throw new Exception("Player Entry Index Doesn't Exist");
            }
            else
            {
                byte[] tableEntry = new byte[playerTableEntryLength];
                _filmStream.SeekTo(currentEntryOffset);
                _filmStream.ReadBlock(tableEntry, 0, (int)playerTableEntryLength);

                File.WriteAllBytes(filePath.Replace("/", ""), tableEntry);

                Close();
            }
        }
    }
}
