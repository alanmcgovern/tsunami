//
// GeneralContext.cs
//
// Author:
//   Alan McGovern <alan.mcgovern@gmail.com>
//
// Copyright (C) 2008 Alan McGovern.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;

namespace Tsunami
{
    class GeneralContext : Context
    {
        private TrackerHost tracker;

        public TrackerHost Tracker
        {
            get { return tracker; }
        }

        public GeneralContext(TrackerHost tracker)
        {
            this.tracker = tracker;
            Options.AddRange(new Option[] {
                new Option("Manage HTTP endpoints", new ListenerContext(this)),
                new Option("Manage watched directories", new DirectoryContext(this)),
                new Option("Statistics", new StatisticsContext(this)),
                new Option("Quit", "q", "quit")
            });
        }

        protected override Result HandleImpl(string line)
        {
            base.HandleImpl(line);
            Option o = Options.GetSelected(line);
            if (o != null && o.Description == "Quit")
                return Result.ShouldPop;

            return Result.Handled;
        }

        protected override void PrintImpl(System.IO.TextWriter writer)
        {
            writer.WriteLine("General:");
            base.PrintImpl(writer);
        }
    }
}
