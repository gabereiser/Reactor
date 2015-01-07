﻿//
// RLog.cs
//
// Author:
//       Gabriel Reiser <gabriel@reisergames.com>
//
// Copyright (c) 2015 2014
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.IO;
namespace Reactor
{
    internal static class RLog
    {
        internal static StreamWriter Writer;

        internal static void Init()
        {
            #if DEBUG
            Writer = new StreamWriter(new FileStream("./debug.log", FileMode.Create));
            #endif
        }

        internal static void Dispose()
        {
            #if DEBUG
            Writer.Close();
            #endif
        }

        internal static void Info(string message)
        {
            #if DEBUG
            Writer.WriteLineAsync("INFO: "+DateTime.Now.ToString() + " : "+message);
            #endif
        }

        internal static void Warn(string message)
        {
            #if DEBUG
            Writer.WriteLineAsync("WARN: "+DateTime.Now.ToString() + " : "+message);
            #endif
        }

        internal static void Error(string message)
        {
            #if DEBUG
            Writer.WriteLineAsync("ERROR: "+DateTime.Now.ToString() + " : "+message);
            #endif
        }

        internal static void Debug(string message)
        {
            #if DEBUG
            Writer.WriteLineAsync("DEBUG: "+DateTime.Now.ToString() + " : "+message);
            #endif
        }
    }
}

