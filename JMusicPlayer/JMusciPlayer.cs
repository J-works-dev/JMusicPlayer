using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JMusicPlayer.Control;

namespace JMusicPlayer
{
    public partial class JMusicPlayer : Form
    {
        public JMusicPlayer()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {

        }

        private void buttonBack_Click(object sender, EventArgs e)
        {

        }

        private void buttonNext_Click(object sender, EventArgs e)
        {

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {

        }

        private void buttonPlaylist_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanelPlaylist.Visible == false) flowLayoutPanelPlaylist.Visible = true;
            else flowLayoutPanelPlaylist.Visible = false;
        }

        private void flowLayoutPanelPlaylist_MouseDown(object sender, MouseEventArgs e)
        {
            if (!flowLayoutPanelPlaylist.Bounds.Contains(e.Location))
            {
                flowLayoutPanelPlaylist.Visible = false;
            }
        }

        // Playlist Page
        private void buttonOpen_Click(object sender, EventArgs e)
        {

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {

        }

        private void buttonSearchPopup_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanelSearch.Visible == false)
            {
                tableLayoutPanelSearch.Visible = true;
                Search searchSong = new Search();
            }
            else tableLayoutPanelSearch.Visible = false;
        }

        private void tableLayoutPanelSearch_MouseDown(object sender, MouseEventArgs e)
        {
            if (!tableLayoutPanelSearch.Bounds.Contains(e.Location))
            {
                tableLayoutPanelSearch.Visible = false;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {

        }

        // Search Page
        private void comboBoxTitle_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxArtist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxAlbum_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxGenre_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {

        }

        
    }
}
