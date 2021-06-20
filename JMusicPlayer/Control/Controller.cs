using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMusicPlayer.Model;

namespace JMusicPlayer.Control
{
    class Controller
    {
        private static string[] wmpSupportedFormats = { ".wav", ".aac", ".wma", ".wmv", ".avi", ".mp3", ".mp4", ".mov", ".mkv", ".WAV", ".AAC", ".WMA", ".WMV", ".AVI", ".MPG", ".MPEG", ".MP3", ".MP4", ".MOV" };
        static Playlist currentPlaylist = new Playlist();
    }
}
