//
// AddListenerContext.cs
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
    class AddListenerContext : Context
    {
        bool valid = true;
        bool alreadyListening;
        
        public AddListenerContext(Context parent)
        {
            Parent = parent;
        }

        protected override Result HandleImpl(string line)
        {
            if (string.IsNullOrEmpty(line))
                return Result.ShouldPop;

            Uri uri;
            valid = Uri.TryCreate(line, UriKind.Absolute, out uri);
            if (!valid)
                return Result.Handled;

            TrackerHost host = ((GeneralContext)BaseContext).Tracker;
            
            alreadyListening = host.Listeners.ContainsKey(uri);
            if (alreadyListening)
                return Result.Handled;
            
            host.AddListener(uri);
            return Result.ShouldPop;
        }

        protected override void PrintImpl(System.IO.TextWriter writer)
        {
            if (valid && !alreadyListening)
            {
                writer.WriteLine("Enter the new address to monitor");
            }
            else if (valid && alreadyListening)
            {
                writer.WriteLine("This address is already in use, please try again");
            }
            else
            {
                writer.WriteLine("The address you entered was invalid, please try again");
            }

            writer.WriteLine("The address should be in the form: http://ip_or_hostname:port/address");
        }
    }
}
