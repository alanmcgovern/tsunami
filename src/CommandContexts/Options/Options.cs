//
// Options.cs
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
using System.Collections.Generic;

namespace Tsunami
{
    public class Options : IEnumerable<Option>
    {
        private List<Option> options = new List<Option>();

        public int Count
        {
            get { return options.Count; }
        }
        public Option this[int index]
        {
            get { return options[index]; }
        }

        public void Add(Option option)
        {
            option.Activators.Insert(0, (options.Count + 1).ToString());
            options.Add(option);

        }

        public void AddRange(IEnumerable<Option> options)
        {
            foreach (Option o in options)
                Add(o);
        }

        public Option Find (Predicate<Option> predicate)
        {
            return options.Find(predicate);
        }

        internal Option GetSelected(string line)
        {
            Option option = Find(delegate(Option o) {
                return o.Activators.Exists(delegate(string s) { return s.Equals(line); });
            });

            if (option != null)
                return option;

            int index = -1;
            if (!int.TryParse(line, out index) || index < 0 || index > Count)
                return null;

            return this[index];
        }

        public IEnumerator<Option> GetEnumerator()
        {
            return options.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
