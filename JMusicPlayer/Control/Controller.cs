using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMusicPlayer.Model;
using JMusicPlayer.Control;
using System.Windows.Forms;

namespace JMusicPlayer
{
    static class Controller
    {
        private static string[] wmpSupportedFormats = { ".wav", ".aac", ".wma", ".wmv", ".avi", ".mp3", ".mp4", ".mov", ".mkv", ".WAV", ".AAC", ".WMA", ".WMV", ".AVI", ".MPG", ".MPEG", ".MP3", ".MP4", ".MOV" };
        public static string currentTrack { get; set; }

        public static void AutoPlayStarts(AxWMPLib.AxWindowsMediaPlayer player)
        {
            OpenMedia(Playlist.GetFirstTrack, player);
            currentTrack = Playlist.GetFirstTrack;
        }

        public static void PlayNextTrack(AxWMPLib.AxWindowsMediaPlayer player)
        {
            currentTrack = Playlist.SelectNextSong(currentTrack);
            OpenMedia(currentTrack, player);
        }

        public static void PlayPreviousTrack(AxWMPLib.AxWindowsMediaPlayer player)
        {
            currentTrack = Playlist.SelectPreviousSong(currentTrack);
            OpenMedia(currentTrack, player);
        }

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

    }
}
