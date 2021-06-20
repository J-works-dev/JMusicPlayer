using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
