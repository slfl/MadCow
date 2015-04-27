﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Bnet.Patcher
{
    class Program
    {
        #region Imports
        [DllImport("kernel32.dll", ExactSpelling = true)]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, out byte lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);
        #endregion

        #region BETA
        #region Build 0.9.0.8896.BETA
        //static Int32 offset = 0x000B4475;
        //static string version = "bcd3e50524"; //DLL Battle.net Aurora bcd3e50524_public/329 (Mar 14 2012 10:28:16)
        #endregion

        #region Build 0.10.0.9183.BETA
        //static Int32 offset = 0x000B5505;
        #endregion

        #region Build 0.11.0.9327.BETA
        //Build 0.11.0.9359.BETA
        //static Int32 offset = 0x000B5605;
        //static string version = "8eac7d44dc";
        #endregion
        #endregion

        #region Build 1.0.1.9558
        //static Int32 offset = 0x000B5952;
        //static string version = "31c8df955a";
        #endregion

        #region Build 1.0.2.9749
        //static Int32 offset = 0x000BA802;
        //static string version = "8018401a9c";
        #endregion

        #region Build 1.0.2.9858
        //static Int32 offset = 0x000BA8A2;
        //static string version = "79fef7ae8e";
        #endregion

        #region Build 1.0.2.9991
        static Int32 serverOffset = 0x000BC25C;
        static Int32 challengeOffset = 0x000BC219;
        static string version = "24e2d13e54";
        #endregion

        static void Main(string[] args)
        {
            var running = false;
            var hWnd = IntPtr.Zero;
            Console.WriteLine("Looking for Diablo III Process");
            while (!running)
            {
                foreach (var p in Process.GetProcesses())
                {
                    if (p.ProcessName == "Diablo III")
                    {
                        Console.WriteLine("Process Found!");
                        Console.WriteLine("Waiting for Diablo 3 Interface to load...");
                        while (!p.Responding)
                        {
                            //Waiting for interface to respond.
                        }
                        Console.WriteLine("Diablo3 Loaded, please wait...");
                        Thread.Sleep(3000);
                        Console.WriteLine("Applying Patch!...");

                        try
                        {
                            hWnd = OpenProcess(0x001F0FFF, false, p.Id);
                            if (hWnd == IntPtr.Zero)
                                throw new Exception("Failed to open process.");

                            var modules = p.Modules;
                            IntPtr baseAddr = IntPtr.Zero;

                            foreach (ProcessModule module in modules)
                            {
                                if (module.ModuleName == "battle.net.dll")
                                {
                                    if (module.FileVersionInfo.FileDescription == version)
                                    {
                                        baseAddr = module.BaseAddress;
                                        break;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Battle.net.dll version different than expected.");
                                        Console.WriteLine("Press any key to exit...");
                                        Console.ReadKey();
                                        System.Environment.Exit(1);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                    }
                                }
                            }

                            if (baseAddr == IntPtr.Zero)
                                throw new Exception("Failed to locate battle.net.dll");

                            var serverAddr = baseAddr.ToInt32() + serverOffset;
                            var challengeAddr = baseAddr.ToInt32() + challengeOffset;
                            var BytesWritten = IntPtr.Zero;
                            byte[] JMP = new byte[] { 0xEB };
                            Console.WriteLine("battle.net.dll address: 0x{0:X8}", baseAddr.ToInt32());
                            var prevByte = ReadByte(hWnd, serverAddr);
                            if (prevByte != 0x75)
                            {
                                running = true;
                                Console.WriteLine("File already patched or unknown battle.net.dll version.");
                            }
                            else
                            {
                                prevByte = ReadByte(hWnd, challengeAddr);
                            }
                            if (prevByte != 0x74)
                            {
                                running = true;
                                Console.WriteLine("File already patched or unknown battle.net.dll version.");
                            }     
                            else
                            {
                                WriteProcessMemory(hWnd, new IntPtr(serverAddr), JMP, 1, out BytesWritten);
                            }

                            if (BytesWritten.ToInt32() < 1)
                            {
                                running = true;
                                Console.WriteLine("Failed to write to process.");
                            }
                            else
                            {
                                WriteProcessMemory(hWnd, new IntPtr(challengeAddr), JMP, 1, out BytesWritten);
                            }
                            if (BytesWritten.ToInt32() < 1)
                            {
                                running = true;
                                Console.WriteLine("Failed to write to process.");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Diablo III succesfully patched!");
                            }
                            running = true;
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.Write(e.Message);
                        }
                        finally
                        {
                            if (hWnd != IntPtr.Zero)
                                CloseHandle(hWnd);
                        }
                    }
                }
            }
            Thread.Sleep(2000);
        }

        static byte ReadByte(IntPtr _handle, int offset)
        {
            byte result = 0;
            ReadProcessMemory(_handle, offset, out result, 1, IntPtr.Zero);
            return result;
        }
    }
}
