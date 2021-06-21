using JMusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JMusicPlayer.Control
{
    static class Search
    {

        public static string[] search(string title, string artist, string album, string genre)
        {
            List<string> searched = new List<string>();
            string[] properties = null;
            string[] names = Playlist.GetAllName();
            
            for (int i = 0; i < Playlist.Count; i++)
            {
                searched.Add(names[i]);
            }

            for (int i = 0; i < Playlist.Count; i++)
            {
                properties = Playlist.GetSongProperties(names[i]);
                if (!string.IsNullOrEmpty(title) && !(properties[0].Contains(title))) searched.Remove(names[i]);
                if (!string.IsNullOrEmpty(artist) && properties[1] != artist) searched.Remove(names[i]);
                if (!string.IsNullOrEmpty(album) && properties[2] != album) searched.Remove(names[i]);
                if (!string.IsNullOrEmpty(genre) && properties[3] != genre) searched.Remove(names[i]);
            }
            string[] result = searched.ToArray();
            return result;
        }
    }
}
