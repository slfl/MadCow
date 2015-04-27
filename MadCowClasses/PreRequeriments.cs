﻿// Copyright (C) 2011 Iker Ruiz Arnauda (Wesko)
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Nini.Config;

namespace MadCow
{
    class PreRequeriments
    {
        public static void CheckPrerequeriments()
        {
            String checkNet4 = Environment.Version.ToString();

            if (checkNet4.StartsWith("4"))
            {
                Console.WriteLine("Found .NET Framework " + checkNet4);
            }
            else
            {
                Console.WriteLine("Please update .NET Framework to version 4!");
            }
        }
    }
}
