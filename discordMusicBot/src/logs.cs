﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discordMusicBot.src
{
    class logs
    {
        public configuration _config;

        private bool returnConfigLogLevel(string level)
        {
            _config = configuration.LoadFile(Directory.GetCurrentDirectory() + "\\configs\\config.json");

            switch (_config.logLevel)
            {
                case 0: // off
                    break;
                case 1: // debug catch everything
                    if (level == "Debug" ||
                        level == "Info" ||
                        level == "Error")
                        return true;
                    break;
                case 2: // info and errors
                    if (level == "Info" ||
                        level == "Error")
                        return true;
                    break;
                case 3: // errors only
                    if (level == "Error")
                        return true;
                    break;
            }
            return false;
        }

        public void logMessage(string level, string source, string msg, string user)
        {
            try
            {
                //check what the user wants returned
                bool configLevel = returnConfigLogLevel(level);

                if(configLevel == true)
                    logFile(level, source, msg, user);
            }
            catch
            {

            }            
        }

        private void logFile(string level, string source, string msg, string user)
        {
            try
            {
                //if we dont find the logs.txt make it
                if (!File.Exists("logs.txt"))
                {
                    File.Create("logs.txt");
                }

                //write our event to the logs.txt file
                using (StreamWriter txtLog = new StreamWriter("logs.txt", true))
                {
                    txtLog.WriteLine($"{DateTime.Now} - {level} - {source} - {user} - {msg}");
                }

                //also write to the console so the admin can see it and I can when debugging.
                Console.WriteLine($"{DateTime.Now} - {level} - {source} - {user} - {msg}");
            }
            catch(Exception error)
            {
                Console.WriteLine($"Error in logs.logFile. Dump: {error}");
            }

        }
    }
}
