using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PartyBlam;
using PartyBlam.Blam.Halo3;
using PartyBlam.STFS;

namespace WinFormsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Halo 3
        private void featurefilmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PartyBlam.Blam.Halo3.Film film = new PartyBlam.Blam.Halo3.Film(ofd.FileName);
            }
        }
        private void sandboxmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PartyBlam.Blam.Halo3.Usermap usermap = new PartyBlam.Blam.Halo3.Usermap(ofd.FileName);
            }
        }
        private void halo3gpdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PartyBlam.Blam.Halo3.GPD gpd = new PartyBlam.Blam.Halo3.GPD(ofd.FileName);
            }
        }
       
        private void injectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    PartyBlam.Blam.Halo3.ScreenShot sShot = new PartyBlam.Blam.Halo3.ScreenShot(ofd.FileName);

            //    OpenFileDialog oofd = new OpenFileDialog();
            //    if (oofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        sShot.InjectScreenshot(File.ReadAllBytes(oofd.FileName));
            //        sShot.Update();
            //    }

            //    sShot.Close();
            //}
        }
        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PartyBlam.Blam.Halo3.ScreenShot sShot = new PartyBlam.Blam.Halo3.ScreenShot(ofd.FileName);

                File.WriteAllBytes(@"C:/Users/Alex/Desktop/test_1.jpg", sShot.ExtractScreenshot());

                sShot.Close();
            }
        }
        #endregion

        #region Halo 3: ODST
        private void featurefilmToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PartyBlam.Blam.Halo3ODST.Film film = new PartyBlam.Blam.Halo3ODST.Film(ofd.FileName);
            }
        }
        private void halo3odstgpdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PartyBlam.Blam.Halo3ODST.GPD gpd = new PartyBlam.Blam.Halo3ODST.GPD(ofd.FileName);
            }
        }

        private void extractPlayerEntry1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach(string file in ofd.FileNames)
                {
                    FileInfo fi = new FileInfo(file);

                    PartyBlam.Blam.Halo3ODST.Film film = new PartyBlam.Blam.Halo3ODST.Film(file);
                    film.ExtractPlayerEntry(1, @"C:\Users\Alex\Desktop\Halo 3 ODST Film Research\Colors\Armour Primary\Player Entries\" + fi.Name + ".playerentry");
                }
            }

            MessageBox.Show("Done");
        }
        #endregion

        #region Halo: Reach
        private void featurefilmToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PartyBlam.Blam.Reach.Film film = new PartyBlam.Blam.Reach.Film(ofd.FileName);
            }
        }
        
        private void extractToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PartyBlam.Blam.Reach.ScreenShot sShot = new PartyBlam.Blam.Reach.ScreenShot(ofd.FileName);

                OpenFileDialog oofd = new OpenFileDialog();
                if (oofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //sShot.InjectScreenshot(File.ReadAllBytes(oofd.FileName));

                    sShot.Update();
                }

                sShot.Close();
            }
        }
        private void injectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PartyBlam.Blam.Reach.ScreenShot sShot = new PartyBlam.Blam.Reach.ScreenShot(ofd.FileName);

                File.WriteAllBytes(@"C:/Users/Alex/Desktop/test_1.jpg", sShot.ExtractScreenshot());

                sShot.Close();
            }
        }
        #endregion

        #region STFS gay shit
        private void sTFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                STFSExplorer stfs = new PartyBlam.STFS.STFSExplorer(ofd.FileName);

                stfs.FileListing[0].EmbeddedFile.ExtractFile(stfs, @"C:/Users/Alex/Desktop/usermap");
                
                //stfs.FileListing[0].EmbeddedFile.ExtractFile(stfs, @"C:/Users/Alex/Desktop/gamestate.hdr");
                //stfs.FileListing[1].EmbeddedFile.ExtractFile(stfs, @"C:/Users/Alex/Desktop/mmiof.bmf");

                //stfs.FileListing[0].EmbeddedFile.ExtractFileAsync(stfs, @"C:/Users/Alex/Desktop/mmiof.bmf", out progressBar1.Value);
            }
        }
        #endregion

        private void rawBLFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PartyBlam.Blam.RawBLF blf = new PartyBlam.Blam.RawBLF(ofd.FileName);
            }
        }
    }
}
