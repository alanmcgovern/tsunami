//The MIT License
//
//Copyright (c) <2008> Alan McGovern <alan.mcgovern@gmail.com>
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.


using System;
using System.Collections.Generic;
using System.IO;
using Mono.Terminal;
using MonoTorrent.Tracker;
using MonoTorrent.TorrentWatcher;
using MonoTorrent.Common;
using MonoTorrent.Tracker.Listeners;

namespace Tsunami
{
    public class TrackerHost
    {
        private Context context;
        private LineEditor editor = new LineEditor("Tsunami tracker");
        private Queue<string> history = new Queue<string>();
        private Dictionary<Uri, HttpListener> listeners = new Dictionary<Uri, HttpListener>();
        private Tracker tracker = new Tracker();
        private Dictionary<string, ITorrentWatcher> watchers = new Dictionary<string, ITorrentWatcher>();

        public Dictionary<Uri, HttpListener> Listeners
        {
            get { return listeners; }
        }

        public Tracker Tracker
        {
            get { return tracker; }
        }

        public Dictionary<string, ITorrentWatcher> Watchers
        {
            get { return watchers; }
        }

        public TrackerHost()
        {
            context = new GeneralContext(this);
            editor.AutoCompleteEvent += delegate {
                // Use this event to allow the tab key to refresh the screen
                Process("__RefreshTheScreen__");
                return null;
            };
        }

        public void Run()
        {
            string s;
            Console.Clear();
            context.Print(Console.Out);
            Console.Out.WriteLine();

            while ((s = editor.Edit("tsunami>", "")) != null)
                if (!Process(s))
                    return;
        }

        private bool Process(string s)
        {
            bool r=false;
            
            // If we're refreshing the screen, then ignore the input string input
            if (s != "__RefreshTheScreen__")
                r = context.Handle(s) != Result.ShouldPop;

            if (!r && s != "__RefreshTheScreen__")
                return r;
            
            Console.Clear();
            context.Print(Console.Out);
            Console.Out.WriteLine();

            // HACK - If i clear the console, i need to rewrite the
            // prompt otherwise things end up out of sync with getline.cs
            if (s == "__RefreshTheScreen__")
                Console.Write("tsunami>");
            return r;
        }

        private void AddHistory(string line)
        {
            lock (history)
            {
                if (history.Count > 15)
                    history.Dequeue();
                history.Enqueue(line);
            }
        }

        private void AddHistory(string line, params object[] args)
        {
            AddHistory(string.Format(line, args));
        }

        internal void AddListener(Uri uri)
        {
            string url = uri.ToString();
            if (!url.EndsWith("/"))
                url = url + "/";

            HttpListener listener = new HttpListener(url);
            tracker.RegisterListener(listener);
            listener.Start();
            listeners.Add(uri, listener);
        }

        internal void AddWatcher(string line)
        {
            TorrentFolderWatcher watcher = new TorrentFolderWatcher(line, "*.torrent");
            watcher.TorrentFound += TorrentFound;
            watcher.ForceScan();
            watcher.Start();
            watchers.Add(line, watcher);
        }

        internal void RemoveListener(Uri uri)
        {
            HttpListener listener = listeners[uri];
            tracker.UnregisterListener(listener);
            listener.Stop();
            listeners.Remove(uri);
        }

        internal void RemoveWatcher(string line)
        {
            if (!watchers.ContainsKey(line))
                return;

            ITorrentWatcher watcher = watchers[line];
            watcher.Stop();
            watcher.TorrentFound -= TorrentFound;
            watchers.Remove(line);
        }

        void TorrentFound(object o, TorrentWatcherEventArgs e)
        {
            Torrent torrent = null;
            try
            {
                torrent = Torrent.Load(e.TorrentPath);
            }
            catch(Exception ex)
            {
                AddHistory("Could not load: {0}. Reason: {1}", e.TorrentPath, ex.Message);
            }

            InfoHashTrackable trackable = new InfoHashTrackable(torrent);
            tracker.Add(trackable);
        }
    }
}
