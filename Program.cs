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
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;


namespace MadCow
{

    class Program
    {
        //Global used variables.
        public static String madcowINI = Path.GetDirectoryName(Application.ExecutablePath) + @"\Tools\madcow.ini";
        public static String programPath = Path.GetDirectoryName(Application.ExecutablePath);
        public static String desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static System.Threading.Mutex s_mutex = null;

        [STAThread]
        static void Main()
        {
            bool instantiated;

            s_mutex = new System.Threading.Mutex(false, "Binglong.My.Application.Mutex", out instantiated);

            if (instantiated)
            {
                //Forcing the culture to English, this way we can read exceptions better from foreign users.
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-us");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("MadCow is already open.");
            }
        }
    }
}