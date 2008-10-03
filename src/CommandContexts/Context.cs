//
// Context.cs
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
using System.IO;

namespace Tsunami
{
    public enum Result
    {
        Handled,
        ShouldPop,
        Unhandled,
    }

    public abstract class Context
    {
        private Context childContext;
        private Options options = new Options();
        private Context parentContext;

        protected Context ActiveContext
        {
            get
            {
                Context c = this;
                while (c.childContext != null)
                    c = c.childContext;
                return c;
            }
        }

        protected Context BaseContext
        {
            get
            {
                Context c = this;
                while (c.Parent != null)
                    c = c.Parent;
                return c;
            }
        }

        protected Context Child
        {
            get { return childContext; }
            set { childContext = value; }
        }

        protected Options Options
        {
            get { return options; }
        }

        protected Context Parent
        {
            get { return parentContext; }
            set { parentContext = value; }
        }

        public Result Handle(string line)
        {
            Result result = ActiveContext.HandleImpl(line);

            if (result == Result.ShouldPop && ActiveContext.Parent != null)
            {
                ActiveContext.Parent.Child = null;
                return Result.Handled;
            }
            return result;
        }

        protected virtual Result HandleImpl(string line)
        {
            if (string.IsNullOrEmpty(line))
                return Result.ShouldPop;

            Option option = Options.GetSelected(line);
            if (option == null || option.Context == null)
                return Result.Unhandled;
            
            Child = option.Context;
            return Result.Handled;
        }

        public void Print(TextWriter writer)
        {
            ActiveContext.PrintImpl(writer);
        }

        protected virtual void PrintImpl(TextWriter writer)
        {
            foreach (Option o in options)
                o.Write(writer);
        }
    }
}
