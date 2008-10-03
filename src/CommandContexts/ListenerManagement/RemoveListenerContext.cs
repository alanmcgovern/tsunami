//
// RemoveListenerContext.cs
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
    class RemoveListenerContext : Context
    {
        public RemoveListenerContext(Context parent)
        {
            Parent = parent;

            foreach (Uri uri in ((GeneralContext)BaseContext).Tracker.Listeners.Keys)
                Options.Add(new Option(uri.ToString(), (Options.Count + 1).ToString()));
        }

        protected override Result HandleImpl(string line)
        {
            if (string.IsNullOrEmpty(line))
                return Result.ShouldPop;

            Option option = Options.GetSelected(line);
            if (option == null)
                return Result.Unhandled;

            ((GeneralContext)BaseContext).Tracker.RemoveListener(new Uri(option.Description));
            return Result.ShouldPop;
        }

        protected override void PrintImpl(System.IO.TextWriter writer)
        {
            writer.WriteLine("Choose the address to stop monitoring");
            base.PrintImpl(writer);
        }
    }
}
