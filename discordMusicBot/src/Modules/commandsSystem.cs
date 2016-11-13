﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Audio;
using Discord.Modules;

namespace discordMusicBot.src.Modules
{
    internal class commandsSystem : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        private configuration _config;

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;

            _config = configuration.LoadFile("config.json");

            manager.CreateCommands("", group =>
            {
                _client.GetService<CommandService>().CreateCommand(_config.Prefix + "rm")
                    .Alias("rm")
                    .Description("Removes messages from a text channel.")
                    .Parameter("count", ParameterType.Optional)
                    .Do(async e =>
                    {
                        if (e.GetArg("count") == "")
                        {
                            await e.Channel.SendMessage($"@{e.User.Name}, Cant delete if you dont tell me how many to remove..");
                            return;
                        }

                        //make var to store messages from the server
                        Message[] messagesToDelete;

                        //convert arg to int
                        int count = int.Parse(e.GetArg("count"));

                        //tell server to download messages to memory
                        messagesToDelete = await e.Channel.DownloadMessages(count);

                        //tell bot to delete them from server
                        await e.Channel.DeleteMessages(messagesToDelete);

                        //await e.Channel.SendMessage($"@{e.User.Name}, I have added {e.GetArg("url")} to autoplaylist.txt.");
                    });

                _client.GetService<CommandService>().CreateCommand(_config.Prefix + "summon")
                    .Alias("summon")
                    .Description("Summons bot to current voice channel")
                    .Do(async e =>
                    {
                        Channel voiceChan = e.User.VoiceChannel;

                        await voiceChan.JoinAudio();
                    });
            });
        }
    }
}
