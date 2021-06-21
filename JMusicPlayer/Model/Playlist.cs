using System;

namespace JMusicPlayer.Model
{
    static class Playlist
    {
        static Song head;
        static Song tail;
        static int count;

        public static int Count { get { return count; } }

        public static string GetHeadData { get { return head.path; } }

        //public static Playlist()
        //{
        //    head = null;
        //    tail = null;
        //}

        public static bool IsEmpty()
        {
            if (count <= 0) return true;
            return false;
        }

        public static string GetFirstTrack
        {
            get
            {
                if (IsEmpty()) return null;
                return GetHeadData;
            }
        }

        public static void Add(string path)
        {
            if (!String.IsNullOrWhiteSpace(path))
            {
                Song node = new Song(path);

                if (head == null)
                {
                    head = node;
                    tail = node;
                    count++;
                }
                else
                {
                    tail.next = node;
                    tail = tail.next;
                    count++;
                }
            }
        }

        public static void AddList(string[] paths)
        {
            foreach (string path in paths)
            {
                Add(path);
            }
        }

        public static void Remove(string val)
        {
            Song t = head;
            if (t.name == val)
            {
                head = head.next;
                count--;
                return;
            }
            while (t.next != null)
            {
                if (t.next.name == val)
                {
                    t.next = t.next.next;
                    count--;
                    return;
                }
                t = t.next;
            }
        }

        public static string SearchByName(string _name)
        {
            Song t = head;

            while (t != null)
            {
                if (t.name == _name)
                {
                    return t.path;
                }
                t = t.next;
            }
            return null;
        }

        public static double GetDuration(string _path)
        {
            Song t = head;

            while (t != null)
            {
                if (t.path == _path)
                {
                    return t.Duration;
                }
                t = t.next;
            }
            return 0;
        }

        public static string SelectNextSong(string _path)
        {
            Song t = head;

            while (t != null)
            {
                if (t.path == _path)
                {
                    if (t.next != null) return t.next.path;
                }
                t = t.next;
            }
            return head.path; // Repeat Playlist from First Song
        }

        public static string SelectPreviousSong(string _path)
        {
            Song t = head;

            if (t.path == _path) return tail.path;

            while(t != null)
            {
                if (t.next.path == _path)
                {
                    return t.path;
                }
                t = t.next;
                if (t.next == null) return t.path;
            }
            return null;
        }

        public static string SelectRandomSong()
        {
            Song t = head;

            Random rnd = new Random();
            int index = rnd.Next(0, count - 1);

            for (int i = 0; i <= index; i++)
            {
                if (i == index) return t.path;
                t = t.next;
            }
            return null;
        }

        public static string[] GetAllSongs()
        {
            Song t = head;
            string[] songs = new string[count];

            for (int i = 0; i < count; i++)
            {
                songs[i] = t.path;
                t = t.next;
            }
            return songs;
        }

        public static string[] GetAllName()
        {
            Song t = head;
            string[] names = new string[count];

            for (int i = 0; i < count; i++)
            {
                names[i] = t.name;
                t = t.next;
            }
            return names;
        }

        public static double[] GetAllDuration()
        {
            Song t = head;
            double[] duration = new double[count];

            for (int i = 0; i < count; i++)
            {
                duration[i] = t.Duration;
                t = t.next;
            }
            return duration;
        }
    }
}
