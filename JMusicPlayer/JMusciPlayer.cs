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
using JMusicPlayer.Model;

namespace JMusicPlayer
{
    public partial class JMusicPlayer : Form
    {
        public static bool isReady = false;
        //public static Playlist playlist = new Playlist();
        public JMusicPlayer()
        {
            InitializeComponent();
            Intializer();
            StylizeDataGrid();
            if (!Playlist.IsEmpty())
            {
                displayPlaylist();
            }
        }

        private void Intializer()
        {
            WMP.Ctlenabled = false;
            WMP.windowlessVideo = true;
            WMP.stretchToFit = true;
        }

        private void StylizeDataGrid()
        {
            dataGridView.ColumnCount = 2;
            dataGridView.Columns[0].Name = "Title";
            dataGridView.Columns[0].MinimumWidth = 250;
            dataGridView.Columns[1].Name = "Duration";
            dataGridView.Columns[1].MinimumWidth = 40;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.BackgroundColor = Color.FromArgb(20, 20, 20);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(20, 20, 20);
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            trackBar.Maximum = (int)Playlist.GetDuration("dfdas");
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
            OpenFileDialog od = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV",
                DefaultExt = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV"
            };

            if (od.ShowDialog() == DialogResult.OK)
            {
                Playlist.Add(od.FileName);
                displayPlaylist();
            }
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

        private void displayPlaylist()
        {
            dataGridView.Rows.Clear();

            string[] names = Playlist.GetAllName();
            double[] durations = Playlist.GetAllDuration();

            for (int i = 0; i < names.Length; i++)
            {
                //string row = names[i] + "\t|\t" + timeToString(durations[i]);
                dataGridView.Rows.Add(names[i], timeToString(durations[i]));
            }
        }

        private string timeToString(double time)
        {
            int min = (int)time / 60;
            int sec = (int)time % 60;

            return string.Format("{0,2}:{0,2}", min.ToString("D2"), sec.ToString("D2"));
        }
    }
}
