using System.Collections.Generic;
using System.Linq;
using JMusicPlayer.Model;
using System.Windows.Forms;

namespace JMusicPlayer
{
    // Main Controller Class
    static class Controller
    {
        // Window Media Player supported formats
        private static string[] wmpSupportedFormats = { ".wav", ".aac", ".wma", ".wmv", ".avi", ".mp3", ".mp4", ".mov", ".mkv", ".WAV", ".AAC", ".WMA", ".WMV", ".AVI", ".MPG", ".MPEG", ".MP3", ".MP4", ".MOV" };
        public static string currentTrack { get; set; }

        // Method for Auto Play
        public static void AutoPlayStarts(AxWMPLib.AxWindowsMediaPlayer player)
        {
            OpenMedia(Playlist.GetFirstTrack, player);
            currentTrack = Playlist.GetFirstTrack;
        }

        // Play next song
        public static void PlayNextTrack(AxWMPLib.AxWindowsMediaPlayer player)
        {
            currentTrack = Playlist.SelectNextSong(currentTrack);
            OpenMedia(currentTrack, player);
        }

        // Play previous song
        public static void PlayPreviousTrack(AxWMPLib.AxWindowsMediaPlayer player)
        {
            currentTrack = Playlist.SelectPreviousSong(currentTrack);
            OpenMedia(currentTrack, player);
        }
        
        // Method for playing the song
        public static void OpenMedia(string MediaAddress, AxWMPLib.AxWindowsMediaPlayer Player)
        {
            if (MediaAddress != null)
            {
                Player.Visible = true;
                Player.URL = MediaAddress;
                Player.Ctlcontrols.play();
                //currentMediaAddress = MediaAddress;
                currentTrack = MediaAddress;
                JMusicPlayer.isReady = true;
            }
            else
            {
                MessageBox.Show("No Media To Play");
            }
        }

        // get properties of song
        public static string[] getProperty(int sel)
        {
            List<string> items = new List<string>();
            string[] properties;
            string[] names = Playlist.GetAllName();

            for (int i = 0; i < Playlist.Count; i++)
            {
                properties = Playlist.GetSongProperties(names[i]);
                items.Add(properties[sel]);
            }
            string[] result = items.Distinct().ToArray();
            foreach (string item in result)
            {
                if (item != null || item != "")
                {
                    return result;
                }
            }
            return null;
        }
    }
}
