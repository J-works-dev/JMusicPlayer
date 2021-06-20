using System;

namespace JMusicPlayer.Model
{
    class Playlist
    {
        Song head;
        Song tail;
        int count;

        public int Count { get { return count; } }

        public string GetHeadData { get { return head.path; } }

        public Playlist()
        {
            head = null;
            tail = null;
        }

        public bool IsEmpty()
        {
            if (count <= 0) return true;
            return false;
        }

        public void Add(string path)
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

        public void AddList(string[] paths)
        {
            foreach (string path in paths)
            {
                Add(path);
            }
        }

        public void Remove(string val)
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

        public string SearchByName(string _name)
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

        public string SelectNextSong(string _path)
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

        public string SelectPreviousSong(string _path)
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

        public string SelectRandomSong()
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

        public string[] GetAllSongs()
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
    }
}
