
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace JMusicPlayer.Model
{
    class Song
    {
        public string name;
        public string path;
        public Song next;
        private readonly TagLib.File tagFile;
        //private static Image defaultImage = Image.FromFile("../images/default.png");
        
        public Song(string path)
        {
            this.path = path;
            this.name = Path.GetFileName(path);
            this.next = null;
            this.tagFile = TagLib.File.Create(path);
        }
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
        public string Genrne
        {
            get
            {
                if (tagFile.Tag.FirstGenre is null)
                    return null;
                else return
                    tagFile.Tag.FirstGenre.Trim();
            }
        }
        //public System.Drawing.Image LoadImageSong
        //{
        //    get
        //    {
        //        TagLib.IPicture firstPicture = tagFile.Tag.Pictures.FirstOrDefault();
        //        System.Drawing.Image image;

        //        if (firstPicture != null && firstPicture.Data.Data.Length != 0)
        //        {
        //            var mStream = new MemoryStream();
        //            byte[] pData = firstPicture.Data.Data;
        //            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
        //            image = System.Drawing.Image.FromStream(mStream);
        //            mStream.Dispose();
        //        }
        //        else
        //        {
        //            image = defaultImage;// 
        //        }
        //        return image;
        //    }
        //}
        public double Duration
        {
            get => tagFile.Properties.Duration.TotalSeconds;
        }
    }
}
