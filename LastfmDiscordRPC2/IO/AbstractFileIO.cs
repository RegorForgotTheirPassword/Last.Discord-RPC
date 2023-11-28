﻿using System;
using System.IO;
using LastfmDiscordRPC2.Enums;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace LastfmDiscordRPC2.IO;

public abstract class AbstractFileIO
{
    public virtual string FilePath { get; protected set; }
    
    protected static readonly string SaveFolder;
    protected static readonly object FileLock;

    static AbstractFileIO()
    {
        SaveFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        SaveFolder += OperatingSystem.CurrentOS switch
        {
            OSEnum.Windows => @"/AppData/Local/LastfmDiscordRPC",
            OSEnum.Linux => @"/.LastfmDiscordRPC",
            OSEnum.OSX => @"/Library/Application Support/LastfmDiscordRPC",
            _ => ""
        };
        
        CreateDataDirectoryIfNotExist();
        FileLock = new object();
    }
    
    protected void WriteToFile(string msg)
    {
        lock (FileLock)
        {
            File.WriteAllText(FilePath, msg);
        }
    }

    protected void AppendToFile(string msg)
    {
        lock (FileLock)
        {
            File.AppendAllText(FilePath, msg);
        }
    }

    protected bool FileExists()
    {
        lock (FileLock)
        {
            return File.Exists(FilePath);
        }
    }

    private static void CreateDataDirectoryIfNotExist()
    {
        if (!Directory.Exists(SaveFolder))
        {
            Directory.CreateDirectory(SaveFolder);
        }
    }
}