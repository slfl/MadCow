﻿// Copyright (C) 2011 MadCow Project
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
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Nini.Config;
using System.IO;
using System.Text.RegularExpressions;

namespace MadCow
{
    class Diablo
    {
        public static void Play()
        {
            if (File.Exists(Program.madcowINI))
            {
                try
                {
                    IConfigSource source = new IniConfigSource(Program.madcowINI);
                    String Src = source.Configs["DiabloPath"].Get("D3Path");

                    if (ProcessFinder.FindProcess("Mooege") == false)
                    {
                        if (File.Exists(Compile.currentMooegeExePath))
                        {
                            Console.WriteLine("Starting Mooege..");
                            Process Mooege = new Process();
                            Mooege.StartInfo = new ProcessStartInfo(Compile.currentMooegeExePath);   
                            Mooege.Start();

                            Thread.Sleep(3000); //We sleep so our ErrorFinder has time to parse Mooege logs.
                            if (ErrorFinder.SearchLogs("Fatal") == true)
                            {
                                Console.WriteLine("Closing Mooege due Fatal Exception");
                                ProcessFinder.KillProcess("Mooege");
                            }
                            else
                            {
                                Console.WriteLine("Starting Diablo..");
                                Process Diablo3 = new Process();
                                Diablo3.StartInfo = new ProcessStartInfo(Src);
                                Diablo3.StartInfo.Arguments = " -launch";
                                Diablo3.Start();
                                //We save this repository for LastPlayed function.
                                source.Configs["LastPlay"].Set("Repository", Compile.currentMooegeExePath);
                                source.Save();
                                Process D3Patcher = new Process();
                                D3Patcher.StartInfo = new ProcessStartInfo(Program.programPath + @"\BnetPatcher\Bnet.Patcher.exe");
                                D3Patcher.Start();
                            }
                        }
                        else
                        {
                            Console.WriteLine("[Error] Couldn't find selected repository binaries."
                            + "\nTry updating the repository again.");
                        }
                    }
                    else //If Mooege is running we kill it and start it again.
                    {
                        Console.WriteLine("Killing Mooege Process..");
                        ProcessFinder.KillProcess("Mooege");
                        Console.WriteLine("Starting Mooege..");
                        Process Mooege = new Process();
                        Mooege.StartInfo = new ProcessStartInfo(Compile.currentMooegeExePath);
                        Mooege.Start();

                        Thread.Sleep(3000);
                        if (ErrorFinder.SearchLogs("Fatal") == true)
                        {
                            Console.WriteLine("Closing Mooege due Fatal Exception");
                            ProcessFinder.KillProcess("Mooege");
                        }
                        else
                        {
                            Console.WriteLine("Starting Diablo..");
                            Process Diablo3 = new Process();
                            Diablo3.StartInfo = new ProcessStartInfo(Src);
                            Diablo3.StartInfo.Arguments = " -launch";
                            Diablo3.Start();
                            //We save this repository for LastPlayed function.
                            source.Configs["LastPlay"].Set("Repository", Compile.currentMooegeExePath);
                            source.Save();
                            Process D3Patcher = new Process();
                            D3Patcher.StartInfo = new ProcessStartInfo(Program.programPath + @"\BnetPatcher\Bnet.Patcher.exe");
                            D3Patcher.Start();
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("[ERROR] Could not launch Diablo. (Diablo.cs)" + "\nPlease report this error in the forum.");
                }
            }
            //If madcow.ini aint found.
            else
            {
                Console.WriteLine("[ERROR] Could not find MadCow config file. (Diablo.cs)" + "\nPlease report this error in the forum.");
            }
        }
    }
}
