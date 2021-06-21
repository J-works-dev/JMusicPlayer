using System;
using System.Collections;
using System.Collections.Generic;

namespace JMusicPlayer.Model
{
    static class Playlist
    {
        static Song head;
        static Song tail;
        static int count;
        public static Hashtable SongProperties = new Hashtable();
        public static int Count { get { return count; } }
        private static string[] sortingArray;

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
                //SongProperties.Add(node.name, GetSongProperties(node.name));
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

        public static string[] GetSongProperties(string name)
        {
            Song t = head;
            string[] result = new string[4];

            while (t != null)
            {
                if (t.name == name)
                {
                    result[0] = t.Title;
                    result[1] = t.Artist;
                    result[2] = t.Album;
                    result[3] = t.Genre;
                }
                t = t.next;
            }
            return result;
        }

        public static void SortPlaylist()
        {
            sortingArray = GetAllName();
            sort(sortingArray, 0, sortingArray.Length - 1);
            List<string> paths = new List<string>();
            foreach (string name in sortingArray)
            {
                paths.Add(SearchByName(name));
            }
            foreach (string name in sortingArray)
            {
                Remove(name);
            }
            AddList(paths.ToArray());
        }

        private static void merge(string[] arr, int l, int m, int r)
        {
            // Find sizes of two subarrays to be merged
            int n1 = m - l + 1;
            int n2 = r - m;

            // Create temp arrays
            string[] L = new string[n1];
            string[] R = new string[n2];
            int i, j;

            // Copy data to temp arrays
            for (i = 0; i < n1; ++i)
                L[i] = arr[l + i];
            for (j = 0; j < n2; ++j)
                R[j] = arr[m + 1 + j];

            // Merge the temp arrays

            // Initial indexes of first and second subarrays
            i = 0;
            j = 0;

            // Initial index of merged subarry array
            int k = l;
            while (i < n1 && j < n2)
            {
                if (L[i].CompareTo(R[j]) < 1)
                {
                    arr[k] = L[i];
                    i++;
                }
                else
                {
                    arr[k] = R[j];
                    j++;
                }
                k++;
            }

            // Copy remaining elements of L[] if any
            while (i < n1)
            {
                arr[k] = L[i];
                i++;
                k++;
            }

            // Copy remaining elements of R[] if any
            while (j < n2)
            {
                arr[k] = R[j];
                j++;
                k++;
            }
        }
        
        private static void sort(string[] arr, int l, int r)
        {
            if (l < r)
            {
                // Find the middle point
                int m = l + (r - l) / 2;

                // Sort first and second halves
                sort(arr, l, m);
                sort(arr, m + 1, r);

                // Merge the sorted halves
                merge(arr, l, m, r);
            }
        }
    }
}
