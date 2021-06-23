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
    // View: Windows Form
    public partial class JMusicPlayer : Form
    {
        // Variables
        public static bool isReady = false;
        public static bool isPlayed = false;
        public static bool isSearch = false;
        public static string selectedSong;
        public static string[] searchedList;
        public static string sTitle, sArtist, sAlbum, sGenre;
        //public static Playlist playlist = new Playlist();
        public JMusicPlayer()
        {
            InitializeComponent();
            Initializer();
            StylizeDataGrid();
            if (!Playlist.IsEmpty())
            {
                displayPlaylist();
            }
        }

        // Media Player Initialize
        private void Initializer()
        {
            WMP.Ctlenabled = false;
            WMP.windowlessVideo = true;
            WMP.stretchToFit = true;
        }

        // Stylize DataGrid
        private void StylizeDataGrid()
        {
            dataGridView.ColumnCount = 2;
            dataGridView.Columns[0].Name = "Title";
            dataGridView.Columns[0].MinimumWidth = 250;
            dataGridView.Columns[1].Name = "Duration";
            dataGridView.Columns[1].MinimumWidth = 30;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.BackgroundColor = Color.FromArgb(20, 20, 20);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(20, 20, 20);
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

        }

        // catch Window media player Something changed
        private void WMP_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            trackBar.Value = 0;
            trackBar.Maximum = stringToTime(WMP.Ctlcontrols.currentItem.durationString);
            labelTotal.Text = WMP.Ctlcontrols.currentItem.durationString;
        }

        // catch WMP state change
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

        // Button close Clicked
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // minimize button clicked
        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Play button clicked
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (selectedSong != null && isPlayed && flowLayoutPanelPlaylist.Visible)
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
                    //Controller.AutoPlayStarts(WMP);
                    WMP.Ctlcontrols.play();
                    PlayPauseIcon(false);
                }
            }
        }

        // Previous button clicked
        private void buttonBack_Click(object sender, EventArgs e)
        {
            Controller.PlayPreviousTrack(WMP);
        }

        // Next button clicked
        private void buttonNext_Click(object sender, EventArgs e)
        {
            Controller.PlayNextTrack(WMP);
        }

        // Stop button clicked
        private void buttonStop_Click(object sender, EventArgs e)
        {
            selectedSong = "";
            WMP.Ctlcontrols.stop();
            trackBar.Value = 0;
            labelCurrent.Text = "00:00";
        }

        // Menu button clicked
        private void buttonPlaylist_Click(object sender, EventArgs e)
        {
            tableLayoutPanelSearch.Visible = false;
            if (flowLayoutPanelPlaylist.Visible == false) flowLayoutPanelPlaylist.Visible = true;
            else flowLayoutPanelPlaylist.Visible = false;
            displayPlaylist();
        }

        // Playlist pane visible option
        private void flowLayoutPanelPlaylist_MouseDown(object sender, MouseEventArgs e)
        {
            if (!flowLayoutPanelPlaylist.Bounds.Contains(e.Location))
            {
                flowLayoutPanelPlaylist.Visible = false;
            }
        }

        // Open and add song
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

        // Delete a song
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView.CurrentCell.RowIndex;
            Playlist.Remove(dataGridView.Rows[selectedIndex].Cells[0].Value.ToString());
            displayPlaylist();
        }

        // Open Search option
        private void buttonSearchPopup_Click(object sender, EventArgs e)
        {
            // ComboBox setup
            if (!Playlist.IsEmpty()) setUpComboBox();

            if (tableLayoutPanelSearch.Visible == false)
            {
                tableLayoutPanelSearch.Visible = true;
            }
            else tableLayoutPanelSearch.Visible = false;
        }

        // Set each tag into the combobox.
        private void setUpComboBox()
        {
            comboBoxArtist.Items.Clear();
            comboBoxAlbum.Items.Clear();
            comboBoxGenre.Items.Clear();
            // Artist
            if (Controller.getProperty(1) != null)
            {
                comboBoxArtist.Items.AddRange(Controller.getProperty(1));
            }
            // Album
            if (Controller.getProperty(2) != null)
            {
                comboBoxAlbum.Items.AddRange(Controller.getProperty(2));
            }
            // Genre
            foreach (string item in Controller.getProperty(3))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    comboBoxGenre.Items.Add(item);
                }
            }
        }

        // close Search option when click outside of the option page
        private void tableLayoutPanelSearch_MouseDown(object sender, MouseEventArgs e)
        {
            //if (!tableLayoutPanelSearch.Bounds.Contains(e.Location))
            //{
            //    tableLayoutPanelSearch.Visible = false;
            //}
        }

        // Save list to CSV
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

        // Load CSV playlist file
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
                string[] row = new string[colCount + 1];

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

        // Search Option Page

        private void comboBoxArtist_SelectedIndexChanged(object sender, EventArgs e)
        {
            sArtist = comboBoxArtist.SelectedItem.ToString();
        }

        private void comboBoxAlbum_SelectedIndexChanged(object sender, EventArgs e)
        {
            sAlbum = comboBoxAlbum.SelectedItem.ToString();
        }

        private void comboBoxGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            sGenre = comboBoxGenre.SelectedItem.ToString();
        }

        // Search the Song
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (textBoxTitle != null) sTitle = textBoxTitle.Text;
            isSearch = true;
            searchedList = Search.search(sTitle, sArtist, sAlbum, sGenre);
            if (searchedList == null)
            {
                MessageBox.Show("There is No Matched Song.");
            }
            else
            {
                displayPlaylist();
            }
            isSearch = false;
            tableLayoutPanelSearch.Visible = false;
        }

        // Display playlist in to the DataGridView
        private void displayPlaylist()
        {
            dataGridView.Rows.Clear();
            string[] names;
            double[] durations;
            if (isSearch)
            {
                durations = new double[searchedList.Length];
                names = searchedList;
                for (int i = 0; i < searchedList.Length; i++)
                {
                    durations[i] = Playlist.GetDuration(Playlist.SearchByName(searchedList[i]));
                }
            }
            else
            {
                names = Playlist.GetAllName();
                durations = Playlist.GetAllDuration();
            }

            for (int i = 0; i < names.Length; i++)
            {
                dataGridView.Rows.Add(names[i], timeToString(durations[i]));
            }
        }

        // Change time to formatted string 00:00
        private string timeToString(double time)
        {
            int min = (int)time / 60;
            int sec = (int)time % 60;

            return string.Format("{0,2}:{0,2}", min.ToString("D2"), sec.ToString("D2"));
        }

        // 00:00 string to time
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

        // Toggle Play and Pause Icon
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

        // Timer Tick every second
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            trackBar.Value = stringToTime(WMP.Ctlcontrols.currentPositionString);
            labelCurrent.Text = WMP.Ctlcontrols.currentPositionString;
        }

        // get Track Bar value
        private void trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            WMP.Ctlcontrols.currentPosition = trackBar.Value;
            UpdateTimer.Start();
        }

        // Title bar Drag and Move
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        // Move window form when click and drag
        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, new IntPtr(HT_CAPTION), IntPtr.Zero);
            }
        }

        // Sort the list
        private void buttonSort_Click(object sender, EventArgs e)
        {
            Playlist.SortPlaylist();
            displayPlaylist();
        }

        private void trackBar_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateTimer.Stop();
        }

        // get list value, when click the list
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView.CurrentCell.RowIndex;
            if (selectedIndex != 0)
            {
                selectedSong = dataGridView.Rows[selectedIndex].Cells[0].Value.ToString();
                isPlayed = true;
            }
            
        }

        // Play music when double Click the list
        private void dataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Controller.OpenMedia(Playlist.SearchByName(dataGridView.Rows[dataGridView.CurrentCell.RowIndex].Cells[0].Value.ToString()), WMP);
            selectedSong = "";
            isPlayed = false;
        }

    }
}
