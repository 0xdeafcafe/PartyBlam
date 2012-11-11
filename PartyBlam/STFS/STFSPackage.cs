using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PartyBlam.IO;

namespace PartyBlam.STFS
{
    /// <summary>
    /// THIS SHIT IS ALL BROKEN, APART FRUM HEADER AND METADATA. FILES AND BLOCKS ARE JUST FUCKING
    /// RETARDED. DUN USE, K BRO.
    /// </summary>
    public class STFSExplorer
    {
        private EndianStream _stfsStream;
        private xHeader _stfsHeader;
        private xMetaData _stfsMetaData;
        private IList<xFileListing> _stfsFileListing;
        private IList<xBlockEntry> _stfsBlockEntry;

        #region Public Access
        public Stream Stream
        {
            get { return _stfsStream.BaseStream; }
        }
        public xHeader Header
        {
            get { return _stfsHeader; }
            set { _stfsHeader = value; }
        }
        public xMetaData MetaData
        {
            get { return _stfsMetaData; }
            set { _stfsMetaData = value; }
        }
        public IList<xFileListing> FileListing
        {
            get { return _stfsFileListing; }
            set { _stfsFileListing = value; }
        }
        public IList<xBlockEntry> BlockEntryList
        {
            get { return _stfsBlockEntry; }
        }
        #endregion

        /// <summary>
        /// Initalize new instance of the STFSExplorer
        /// </summary>
        /// <param name="fileName">Path of the STFS Container you wish to open.</param>
        public STFSExplorer(string fileName)
        {
            Stream rootStream = new MemoryStream(File.ReadAllBytes(fileName));
            Initalize(rootStream);
        }
        /// <summary>
        /// Initalize new instance of the STFSExplorer
        /// </summary>
        /// <param name="fileStream">Stream of the STFS Container</param>
        public STFSExplorer(Stream fileStream)
        {
            Initalize(fileStream);
        }
        /// <summary>
        /// Initalize the Class
        /// </summary>
        /// <param name="fileStream">Stream of the STFS Container</param>
        private void Initalize(Stream fileStream)
        {
            _stfsStream = new EndianStream(fileStream, Endian.BigEndian);

            LoadHeader();
            LoadMetaData();
            LoadFileListing();
            LoadEmbeddedFilesFromFileListing();
            LoadBlockTable();
        }

        public class xHeader
        {
            public string Magic { get; set; }
            public PackageType PackageType { get; set; }

            // "CON " files
            public byte[] PublicKeyCertificateSize { get; set; }
            public byte[] CertificateOwnerConsoleID { get; set; }
            public string CertificateOwnerConsolePartNumber { get; set; }
            public CertOwnerConsoleType CertiicateOwnerConsoleType { get; set; }
            public string CertificateDateOfGeneration { get; set; }
            public byte[] PublicExponent { get; set; }
            public byte[] PublicModulus { get; set; }
            public byte[] CertificateSignature { get; set; }
            public byte[] Signature { get; set; }

            // LIVE or PIRS files
            public byte[] PackageSignature { get; set; }
        }
        public class xMetaData
        {
            public LicensingEntry LicensingEntries { get; set; }
            public byte[] ContentID { get; set; }
            public uint HeaderSize { get; set; }
            public ContentTypes ContentType { get; set; }
            public int MetadataVersion { get; set; }
            public long ContentSize { get; set; }
            public uint MediaID { get; set; }
            public int Version { get; set; }
            public int BaseVersion { get; set; }
            public uint TitleID { get; set; }
            public PackagePlatform Platform { get; set; }
            public byte ExecutableType { get; set; }
            public byte DiskNumber { get; set; }
            public byte DiskInSet { get; set; }
            public uint SaveGameID { get; set; }
            public byte[] ConsoleID { get; set; }
            public byte[] ProfileID { get; set; }
            public xVolumeDescription VolumeDescriptor { get; set; }
            public int DataFileCount { get; set; }
            public long DataFileCombinedSize { get; set; }
            public byte[] DeviceID { get; set; }
            public string DisplayName { get; set; }
            public string DisplayDescription { get; set; }
            public string PublisherName { get; set; }
            public string TitleName { get; set; }
            public TransferFlags TransferFlags { get; set; }
            public xMetaImage ThumbnailImage { get; set; }
            public xMetaImage TitleThumbnailImage { get; set; }

            public class xVolumeDescription
            {
                public byte VolumeDescriptorSize { get; set; }
                public byte Reserved { get; set; }
                public byte BlockSeperation { get; set; }
                public short FileTableBlockCount { get; set; }
                public int FileTableBlockNumber { get; set; }
                public byte[] TopHashTableHash { get; set; }
                public int TotalAllocatedBlockCount { get; set; }
                public int TotalUnallocatedBlockCount { get; set; }
            }
            public class xMetaImage
            {
                public int ImageSize { get; set; }
                public byte[] Image { get; set; }
            }
            public class LicensingEntry
            {
                public long LicenseID { get; set; }
                public int LicenseBits { get; set; }
                public int LicenseFlags { get; set; }
            }
        }
        public class xFileListing
        {
            public string FileName { get; set; }
            public byte FileNameLengthwFlags { get; set; }
            public int NumberOfAllocatedBlocks { get; set; }
            public int NumberOfAllocatedBlocks2 { get; set; }
            public int StartingBlockNumber { get; set; }
            public short PathIndicator { get; set; }
            public uint SizeOfFile { get; set; }
            public int ModifiedTimeDate { get; set; }
            public int AccessTimeDate { get; set; }

            public xFile EmbeddedFile { get; set; }

            public class xFile
            {
                public byte[] File { get; set; }
                public long StartingOffset { get; set; }
                public uint Size { get; set; }

                public bool CancelTransfer = false;

                public byte[] ExtractFile(STFSExplorer stfs)
                {
                    return RemoveHashBlocks(File, stfs);
                }
                public void ExtractFile(STFSExplorer stfs, string fileName)
                {
                    System.IO.File.WriteAllBytes(fileName, RemoveHashBlocks(File, stfs));
                }
                public void ExtractFileAsync(STFSExplorer stfs, string fileName, out int progress)
                {
                    progress = 0;

                    byte[] file = RemoveHashBlocks(File, stfs);
                    EndianStream embeddedStream = new EndianStream(new MemoryStream(file), Endian.BigEndian);
                    EndianStream fileStream = new EndianStream(System.IO.File.Create(fileName), Endian.BigEndian);

                    int buffer = 0x1000;
                    while (fileStream.Position < embeddedStream.Length)
                    {
                        if (CancelTransfer)
                            break;

                        if (fileStream.Position + buffer > fileStream.Length)
                            buffer = (int)fileStream.Length - (int)fileStream.Position;
                        fileStream.WriteBlock(embeddedStream.ReadBlock(buffer));

                        // Update Progress
                        progress = RandomFunctions.GetPercentage((int)fileStream.Position, (int)embeddedStream.Length);
                    }

                    fileStream.Close();
                    embeddedStream.Close();
                }

                private byte[] RemoveHashBlocks(byte[] fileArray, STFSExplorer stfs)
                {
                    long startOffset = StartingOffset;
                    long endOffset = StartingOffset + Size;

                    List<long> hashBlockOffsets = new List<long>();
                    foreach (xBlockEntry block in stfs._stfsBlockEntry) 
                        if (block.IsHashBlock)
                            if (block.BlockOffset >= startOffset && block.BlockOffset <= endOffset)
                                hashBlockOffsets.Add(block.BlockOffset - (hashBlockOffsets.Count * 0x1000));
                    
                    if (hashBlockOffsets.Count > 0)
                    {
                        byte[] newFileAray = new byte[fileArray.Length - (hashBlockOffsets.Count * 0x1000)];

                        int newFileOffset = 0;
                        for (int offset = 0; offset < fileArray.Length; offset++)
                            if (hashBlockOffsets.Count > 0 && hashBlockOffsets[0] == offset)
                            {
                                offset += 0x1000;
                                hashBlockOffsets.RemoveAt(0);
                            }
                            else
                            {
                                newFileAray[newFileOffset] = fileArray[offset];
                                newFileOffset++;
                            }

                        return newFileAray;
                    }
                    else
                        return fileArray;
                }
            }
        }
        public class xBlockEntry
        {
            public long BlockOffset { get; set; }
            public int BlockIndex { get; set; }
            public xHashBlockEntry HashBlockEntry { get; set; }

            public bool IsHashBlock { get; set; }
            public IList<xHashBlockEntry> HashBlocks { get; set; }
        }
        public class xHashBlockEntry
        {
            public byte[] SHA1Hash { get; set; }
            public BlockStatus StatusByte { get; set; }
            public int NextBlock { get; set; }
        }

        /// <summary>
        /// Load the STFS Container's Header
        /// </summary>
        public void LoadHeader()
        {
            _stfsHeader = new xHeader();
            _stfsStream.SeekTo(0x00);
            _stfsHeader.Magic = _stfsStream.ReadAscii(0x04);
            _stfsHeader.PackageType = GetPackageType(_stfsHeader.Magic);

            _stfsStream.SeekTo(0x04);
            switch (_stfsHeader.PackageType)
            {
                case PackageType.CON:
                    _stfsHeader.PublicKeyCertificateSize = _stfsStream.ReadBlock(0x02);
                    _stfsHeader.CertificateOwnerConsoleID = _stfsStream.ReadBlock(0x05);
                    _stfsHeader.CertificateOwnerConsolePartNumber = _stfsStream.ReadAscii(0x14).Trim();
                    _stfsHeader.CertiicateOwnerConsoleType = (CertOwnerConsoleType)_stfsStream.ReadByte();
                    _stfsHeader.CertificateDateOfGeneration = _stfsStream.ReadAscii(0x08);
                    _stfsHeader.PublicExponent = _stfsStream.ReadBlock(0x04);
                    _stfsHeader.PublicModulus = _stfsStream.ReadBlock(0x80);
                    _stfsHeader.CertificateSignature = _stfsStream.ReadBlock(0x100);
                    _stfsHeader.Signature = _stfsStream.ReadBlock(0x80);
                    break;
                case PackageType.PIRS: case PackageType.LIVE:
                    _stfsHeader.PackageSignature = _stfsStream.ReadBlock(0x100);
                    break;
            }
        }

        /// <summary>
        /// Load the STFS Container's MetaData
        /// </summary>
        public void LoadMetaData()
        {
            _stfsMetaData = new xMetaData();

            _stfsStream.SeekTo(0x22C);

            // Read License Entries
            _stfsMetaData.LicensingEntries = new xMetaData.LicensingEntry();
            _stfsMetaData.LicensingEntries.LicenseID = _stfsStream.ReadInt64();
            _stfsMetaData.LicensingEntries.LicenseBits = _stfsStream.ReadInt32();
            _stfsMetaData.LicensingEntries.LicenseFlags = _stfsStream.ReadInt32();

            _stfsStream.SeekTo(0x32C);
            _stfsMetaData.ContentID = _stfsStream.ReadBlock(0x14);
            _stfsMetaData.HeaderSize = _stfsStream.ReadUInt32();
            _stfsMetaData.ContentType = (ContentTypes)_stfsStream.ReadInt32();
            _stfsMetaData.MetadataVersion = _stfsStream.ReadInt32();
            _stfsMetaData.ContentSize = _stfsStream.ReadInt64();
            _stfsMetaData.MediaID = _stfsStream.ReadUInt32();
            _stfsMetaData.Version = _stfsStream.ReadInt32();
            _stfsMetaData.BaseVersion = _stfsStream.ReadInt32();
            _stfsMetaData.TitleID = _stfsStream.ReadUInt32();
            _stfsMetaData.Platform = (PackagePlatform)_stfsStream.ReadByte();
            _stfsMetaData.ExecutableType = _stfsStream.ReadByte();
            _stfsMetaData.DiskNumber = _stfsStream.ReadByte();
            _stfsMetaData.DiskInSet = _stfsStream.ReadByte();
            _stfsMetaData.SaveGameID = _stfsStream.ReadUInt32();
            _stfsMetaData.ConsoleID = _stfsStream.ReadBlock(0x5);
            _stfsMetaData.ProfileID = _stfsStream.ReadBlock(0x8);

            _stfsMetaData.VolumeDescriptor = new xMetaData.xVolumeDescription();
            _stfsMetaData.VolumeDescriptor.VolumeDescriptorSize = _stfsStream.ReadByte();
            _stfsStream.ReadByte();
            _stfsStream.ReadByte();
            _stfsMetaData.VolumeDescriptor.FileTableBlockCount = _stfsStream.ReadInt16();
            _stfsMetaData.VolumeDescriptor.FileTableBlockNumber = _stfsStream.ReadInt24();
            _stfsMetaData.VolumeDescriptor.TopHashTableHash = _stfsStream.ReadBlock(0x14);
            _stfsMetaData.VolumeDescriptor.TotalAllocatedBlockCount = _stfsStream.ReadInt32();
            _stfsMetaData.VolumeDescriptor.TotalUnallocatedBlockCount = _stfsStream.ReadInt32();

            _stfsStream.SeekTo(0x39D);
            _stfsMetaData.DataFileCount = _stfsStream.ReadInt32();
            _stfsMetaData.DataFileCombinedSize = _stfsStream.ReadInt64();

            _stfsStream.SeekTo(0x3FD);
            _stfsMetaData.DeviceID = _stfsStream.ReadBlock(0x14);
            _stfsMetaData.DisplayName = _stfsStream.ReadUTF16();

            _stfsStream.SeekTo(0xD11);
            _stfsMetaData.DisplayDescription = _stfsStream.ReadUTF16();

            _stfsStream.SeekTo(0x1611);
            _stfsMetaData.PublisherName = _stfsStream.ReadUTF16();

            _stfsStream.SeekTo(0x1691);
            _stfsMetaData.TitleName = _stfsStream.ReadUTF16();

            _stfsStream.SeekTo(0x1711);
            _stfsMetaData.TransferFlags = (TransferFlags)_stfsStream.ReadByte();

            _stfsMetaData.ThumbnailImage = new xMetaData.xMetaImage();
            _stfsMetaData.TitleThumbnailImage = new xMetaData.xMetaImage();

            _stfsMetaData.ThumbnailImage.ImageSize = _stfsStream.ReadInt32();
            _stfsMetaData.TitleThumbnailImage.ImageSize = _stfsStream.ReadInt32();

            _stfsMetaData.ThumbnailImage.Image = _stfsStream.ReadBlock(_stfsMetaData.ThumbnailImage.ImageSize);
            _stfsStream.SeekTo(0x571A);
            _stfsMetaData.TitleThumbnailImage.Image = _stfsStream.ReadBlock(_stfsMetaData.TitleThumbnailImage.ImageSize);
        }

        /// <summary>
        /// Load the STFS Container's File Listing
        /// </summary>
        public void LoadFileListing()
        {
            _stfsFileListing = new List<xFileListing>();

            int baseOffset = BlockToOffset(ComputeDataBlockNumber(_stfsMetaData.VolumeDescriptor.FileTableBlockNumber));
            _stfsStream.SeekTo(baseOffset);

            for (int entry = 0; entry < _stfsMetaData.VolumeDescriptor.FileTableBlockCount; entry++)
            {
                xFileListing newListing = new xFileListing();

                newListing.FileName = _stfsStream.ReadAscii(0x28);
                if (String.IsNullOrEmpty(newListing.FileName))
                    break;
                newListing.FileNameLengthwFlags = _stfsStream.ReadByte();
                newListing.NumberOfAllocatedBlocks = _stfsStream.ReadInt24();
                newListing.NumberOfAllocatedBlocks2 = _stfsStream.ReadInt24();
                _stfsStream.Endianness = Endian.LittleEndian;
                newListing.StartingBlockNumber = _stfsStream.ReadInt24();
                _stfsStream.Endianness = Endian.BigEndian;
                newListing.PathIndicator = _stfsStream.ReadInt16();
                newListing.SizeOfFile = _stfsStream.ReadUInt32();
                newListing.ModifiedTimeDate = _stfsStream.ReadInt32();
                newListing.AccessTimeDate = _stfsStream.ReadInt32();

                _stfsFileListing.Add(newListing);
            }
        }

        /// <summary>
        /// Load the STFS Container's Embedded Files from the FileListing
        /// </summary>
        public void LoadEmbeddedFilesFromFileListing()
        {
            uint currentEntryOffset = 0xC000 + 0x1000;

            foreach (xFileListing file in _stfsFileListing)
            {
                _stfsStream.SeekTo(currentEntryOffset);

                byte[] embededFile = new byte[file.SizeOfFile];
                _stfsStream.ReadBlock(embededFile, (int)file.SizeOfFile);

                xFileListing.xFile xEmbededFile = new xFileListing.xFile()
                {
                    File = embededFile,
                    Size = file.SizeOfFile,
                    StartingOffset = currentEntryOffset
                };
                file.EmbeddedFile = xEmbededFile;

                currentEntryOffset = (uint)Math.Ceiling((float)(currentEntryOffset + file.SizeOfFile) / 0x1000) * 0x1000;
            }
        }

        /// <summary>
        /// Load the STFS Container's Blocks
        /// </summary>
        public void LoadBlockTable()
        {
            _stfsBlockEntry = new List<xBlockEntry>();

            int countTillNextHashBlock = 0x00;
            int timesHitHashTable = 0x00;
            for (float offset = 0xC000; offset < _stfsStream.Length; offset += 0x1000)
            {
                if (countTillNextHashBlock == 0xAA)
                {
                    // Hash Block
                    xBlockEntry block = new xBlockEntry();
                    block.IsHashBlock = true;
                    block.BlockIndex = (int)(offset / 0x1000) - 0xC;
                    block.BlockOffset = (long)offset;
                    block.HashBlocks = new List<xHashBlockEntry>();

                    for (int hashBlock = 0x00; hashBlock < 0xAA; hashBlock++)
                    {
                        int startOffset = (int)offset + (0x18 * hashBlock);
                        _stfsStream.SeekTo(startOffset);

                        xHashBlockEntry entry = new xHashBlockEntry();
                        entry.SHA1Hash = _stfsStream.ReadBlock(0x14);
                        entry.StatusByte = (BlockStatus)_stfsStream.ReadByte();
                        entry.NextBlock = _stfsStream.ReadInt24();

                        block.HashBlocks.Add(entry);

                        int blockEntryIndex = (timesHitHashTable * 0xAA) + hashBlock;
                        _stfsBlockEntry[blockEntryIndex].HashBlockEntry = entry;
                    }

                    _stfsBlockEntry.Add(block);
                    countTillNextHashBlock = 0x01;
                    timesHitHashTable++;
                }
                else
                {
                    // Regular Block
                    xBlockEntry block = new xBlockEntry();
                    block.BlockIndex = (int)(offset / 0x1000) - 0xC;
                    block.BlockOffset = (long)offset;
                    block.IsHashBlock = false;
                    _stfsBlockEntry.Add(block);

                    countTillNextHashBlock++;
                }
            }
        }


        /// <summary>
        /// Get the type of package from the Magic (CON , PIRS, LIVE)
        /// </summary>
        /// <param name="Magic">The Magic of the file, ASCII String</param>
        /// <returns>The Package Type</returns>
        public PackageType GetPackageType(string Magic)
        {
            switch (Magic)
            {
                case "CON ": return PackageType.CON;
                case "PIRS": return PackageType.PIRS;
                case "LIVE": return PackageType.LIVE;
                default: throw new Exception("Not a valid Xbox 360 Container. Invalid Magic");
            }
        }

        /// <summary>
        /// From Free60 - (http://free60.org/STFS#File_Listing). Calculates the DataBlockNumber from the 'File Table Block Number 
        /// </summary>
        /// <param name="xBlock">The 'File Table Block Number'</param>
        public int ComputeDataBlockNumber(int xBlock)
        {
            int xBlockShift;
            if (((MetaData.HeaderSize + 0xFFF) & 0xF000) == 0xB000)
                xBlockShift = 1;
            else
                if ((MetaData.VolumeDescriptor.BlockSeperation & 1) == 1)
                    xBlockShift = 0;
                else
                    xBlockShift = 1;

            int xBase = ((xBlock + 0xAA) / 0xAA);
            if (this.Header.PackageType == PackageType.CON)
                xBase = (xBase << xBlockShift);
            int xReturn = (xBase + xBlock);

            if (xBlock > 0xAA)
            {
                xBase = ((xBlock + 0x70E4) / 0x70E4);
                if (this.Header.PackageType == PackageType.CON)
                    xBase = (xBase << xBlockShift);
                xReturn += xBase;

                if (xBlock > 0x70E4)
                {
                    xBase = ((xBlock + 0x4AF768) / 0x4AF768);
                    if (this.Header.PackageType == (PackageType)xBlockShift)
                        xBase = (xBase << 1);

                    xReturn = (xReturn + xBase);
                }
            }

            return xReturn;


        }
        /// <summary>
        /// From Free60 - (http://free60.org/STFS#File_Listing). Calculates the Offset of the Specified Block
        /// </summary>
        /// <param name="xBlock">The block to convert.</param>
        /// <returns></returns>
        public int BlockToOffset(int xBlock)
        {
            int xReturn = 0;
            if (xBlock > 0xFFFFFF)
                xReturn = -1;
            else
                xReturn = ((((int)MetaData.HeaderSize + 0xFFF) & 0xF000) + (xBlock << 12));
            return xReturn;
        }
    }

    public enum ContentTypes
    {
        ArcadeTitle = 0xD0000,
        AvatarItem = 0x9000,
        CacheFile = 0x40000,
        CommunityGame = 0x2000000,
        GameDemo = 0x80000,
        GamerPicture = 0x20000,
        GameTitle = 0xA0000,
        GameTrailer = 0xC0000,
        GameVideo = 0x400000,
        InstalledGame = 0x4000,
        Installer = 0xB0000,
        IPTVPauseBuffer = 0x2000,
        LicenseStore = 0xF0000,
        MarketplaceContent = 0x2,
        Movie = 0x100000,
        MusicVideo = 0x300000,
        PodcastVideo = 0x500000,
        Profile = 0x10000,
        Publisher = 0x3,
        SavedGame = 0x1,
        StorageDownload = 0x50000,
        Theme = 0x30000,
        TV = 0x200000,
        Video = 0x90000,
        ViralVideo = 0x600000,
        XboxDownload = 0x70000,
        XboxOriginalGame = 0x5000,
        XboxSavedGame = 0x60000,
        Xbox360Title = 0x1000,
        XboxTitle = 0x5000,
        XNA = 0xE0000
    }
    public enum PackagePlatform
    {
        X360 = 0x02,
        PC = 0x04
    }
    public enum TransferFlags
    {
        DeviceandProfileIDTranser = 0x00,
        MoveOnlyTransfer = 0x20,
        DeviceIDTransfer = 0x40,
        ProfileIDTransfer = 0x80,
        None = 0xC0
    }
    public enum PackageType
    {
        CON ,
        PIRS,
        LIVE
    }
    public enum BlockStatus
    {
        UnusedBlock = 0x00,
        FreeBlock = 0x40,
        UsedBlock = 0x80,
        NewlyAllocatedBlock = 0xC0
    }
    public enum CertOwnerConsoleType
    {
        Devkit = 0x01,
        Retail = 0x02
    }
}
