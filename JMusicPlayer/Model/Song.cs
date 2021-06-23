
using System;
using System.IO;
using System.Windows.Forms;

namespace JMusicPlayer.Model
{
    // Song class is used as a Node
    class Song
    {
        // Variables
        public string name;
        public string path;
        public Song next;
        private readonly TagLib.File tagFile; // using 3rd Party library for get mp3 file tag
        
        // Constructor
        public Song(string path)
        {
            this.path = path;
            this.name = Path.GetFileName(path);
            this.next = null;
            try
            {
                this.tagFile = TagLib.File.Create(path);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e);
            }
        }
        // Getters
        public string Title
        {
            get
            {
                if (tagFile.Tag.Title is null)
                    return null;
                else return tagFile.Tag.Title.Trim();
            }
        }
        public string Artist
        {
            get
            {
                if (tagFile.Tag.FirstPerformer is null)
                    return string.Empty;
                else
                    return tagFile.Tag.FirstPerformer.Trim();
            }
        }
        public string Album
        {
            get
            {
                if (tagFile.Tag.Album is null)
                    return string.Empty;
                else
                    return tagFile.Tag.Album.Trim();
            }
        }
        public string Genre
        {
            get
            {
                if (tagFile.Tag.FirstGenre is null)
                    return null;
                else return
                    tagFile.Tag.FirstGenre.Trim();
            }
        }

        public double Duration
        {
            get => tagFile.Properties.Duration.TotalSeconds;
        }
    }
}
