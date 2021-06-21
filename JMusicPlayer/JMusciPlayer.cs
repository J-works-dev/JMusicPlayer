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
using System.Runtime.InteropServices;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace JMusicPlayer
{
    public partial class JMusicPlayer : Form
    {
        public static bool isReady = false;
        public static bool isPlayed = false;
        public static string selectedSong;
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

        private void WMP_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            trackBar.Value = 0;
            trackBar.Maximum = stringToTime(WMP.Ctlcontrols.currentItem.durationString);
        }

        private void WMP_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (WMP.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                UpdateTimer.Start();
                PlayPauseIcon(false);
            }

            else
            {
                UpdateTimer.Stop();
                PlayPauseIcon(true);
            }

            if (WMP.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                if (isReady)
                {
                    Controller.PlayNextTrack(WMP);
                    UpdateTimer.Start();
                    PlayPauseIcon(false);
                }
            }

            if (WMP.playState == WMPLib.WMPPlayState.wmppsReady)
            {
                try
                {
                    WMP.Ctlcontrols.play();
                    UpdateTimer.Start();
                    PlayPauseIcon(false);
                }
                catch { }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (selectedSong != null && isPlayed)
            {
                Controller.OpenMedia(Playlist.SearchByName(selectedSong), WMP);
                isPlayed = false;
                selectedSong = "";
            }
            else
            {
                if (WMP.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    WMP.Ctlcontrols.pause();
                    PlayPauseIcon(true);
                }
                else
                {
                    Controller.AutoPlayStarts(WMP);
                    WMP.Ctlcontrols.play();
                    PlayPauseIcon(false);
                }
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {

        }

        private void buttonNext_Click(object sender, EventArgs e)
        {

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            selectedSong = "";
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
            int selectedIndex = dataGridView.CurrentCell.RowIndex;
            Playlist.Remove(dataGridView.Rows[selectedIndex].Cells[0].Value.ToString());
            displayPlaylist();
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
            if (!Playlist.IsEmpty())
            {
                SaveFileDialog saveDi = new SaveFileDialog
                {
                    FileName = "Playlist.csv",
                    Filter = "CSV (*.csv)|*.csv"
                };
                bool fileError = false;
                if (saveDi.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(saveDi.FileName))
                    {
                        try
                        {
                            File.Delete(saveDi.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            string[] outputCsv = Playlist.GetAllSongs();

                            File.WriteAllLines(saveDi.FileName, outputCsv, Encoding.UTF8);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "CSV Files (*.csv)|*.csv";
            if (od.ShowDialog() == DialogResult.OK)
            {
                var streamReader = new StreamReader(od.FileName);
                var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
                
                csvReader.Read();
                csvReader.ReadHeader();
                string[] headers = csvReader.HeaderRecord;
                int colCount = headers.Length;
                string value;
                string[] row = new string[colCount];

                while (csvReader.Read())
                {
                    for (int i = 0; csvReader.TryGetField<string>(i, out value); i++)
                    {
                        row[i] = value;
                    }
                    Playlist.Add(row[0]);
                }
                displayPlaylist();
            }
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

        private int stringToTime(string dur)
        {
            int timeInSec = 0;
            string[] pieces = dur.Split(':');
            if (pieces.Length == 2)
            {
                timeInSec = int.Parse(pieces[0]) * 60;
                timeInSec += int.Parse(pieces[1]);
            }

            else if (pieces.Length == 3)
            {
                timeInSec = int.Parse(pieces[0]) * 60 * 60;
                timeInSec += int.Parse(pieces[1]) * 60;
                timeInSec += int.Parse(pieces[2]);
            }
            return timeInSec;
        }

        private void PlayPauseIcon(bool ChangeToPlay)
        {
            if (ChangeToPlay)
            {
                buttonPlay.Image = Image.FromFile(@"C:\Users\Jeremy\source\repos\JMusicPlayer\JMusicPlayer\images\circled_play_96px.png");
            }

            else
            {
                buttonPlay.Image = Image.FromFile(@"C:\Users\Jeremy\source\repos\JMusicPlayer\JMusicPlayer\images\pause_96px.png");
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            trackBar.Value = stringToTime(WMP.Ctlcontrols.currentPositionString);
        }

        private void trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            WMP.Ctlcontrols.currentPosition = trackBar.Value;
            UpdateTimer.Start();
            //foCUS.Focus();
        }

        private void trackBar_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateTimer.Stop();
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView.CurrentCell.RowIndex;
            selectedSong = dataGridView.Rows[selectedIndex].Cells[0].Value.ToString();
            isPlayed = true;
        }

        private void dataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Controller.OpenMedia(Playlist.SearchByName(dataGridView.Rows[dataGridView.CurrentCell.RowIndex].Cells[0].Value.ToString()), WMP);
            selectedSong = "";
            isPlayed = false;
        }
    }
}
