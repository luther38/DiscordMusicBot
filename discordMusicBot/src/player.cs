﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using NAudio.Wave;

namespace discordMusicBot.src
{
    class player
    {
        private DiscordClient _client; //load discord client info
        private IAudioClient _nAudio; //load naudio client

        //used to play the music to the room
        public async Task SendAudio(string filepath, Channel voiceChannel)
        {
            // When we use the !play command, it'll start this method

            // The comment below is how you'd find the first voice channel on the server "Somewhere"
            //var a = _client.FindServers("Waifu Lounge").
             var voiceChannel2 = _client.FindServers("Somewhere").FirstOrDefault().VoiceChannels.FirstOrDefault();
            // Since we already know the voice channel, we don't need that.
            // So... join the voice channel:
            _nAudio = await _client.GetService<AudioService>().Join(voiceChannel);

            // Simple try and catch.
            try
            {

                var channelCount = _client.GetService<AudioService>().Config.Channels; // Get the number of AudioChannels our AudioService has been configured to use.
                var OutFormat = new WaveFormat(48000, 16, channelCount); // Create a new Output Format, using the spec that Discord will accept, and with the number of channels that our client supports.

                using (var MP3Reader = new Mp3FileReader(filepath)) // Create a new Disposable MP3FileReader, to read audio from the filePath parameter
                using (var resampler = new MediaFoundationResampler(MP3Reader, OutFormat)) // Create a Disposable Resampler, which will convert the read MP3 data to PCM, using our Output Format
                {
                    resampler.ResamplerQuality = 60; // Set the quality of the resampler to 60, the highest quality
                    int blockSize = OutFormat.AverageBytesPerSecond / 50; // Establish the size of our AudioBuffer
                    byte[] buffer = new byte[blockSize];
                    int byteCount;
                    // Add in the "&& playingSong" so that it only plays while true. For our cheesy skip command.
                    // AGAIN
                    // WARNING
                    // YOU NEED
                    // vvvvvvvvvvvvvvv
                    // opus.dll
                    // libsodium.dll
                    // ^^^^^^^^^^^^^^^
                    // If you do not have these, this will not work.
                    while ((byteCount = resampler.Read(buffer, 0, blockSize)) > 0 ) // Read audio into our buffer, and keep a loop open while data is present
                    {
                        if (byteCount < blockSize)
                        {
                            // Incomplete Frame
                            for (int i = byteCount; i < blockSize; i++)
                                buffer[i] = 0;
                        }

                        _nAudio.Send(buffer, 0, blockSize); // Send the buffer to Discord
                    }
                    await _nAudio.Disconnect();
                }
            }
            catch
            {
                System.Console.WriteLine("Something went wrong. :(");
            }
            await _nAudio.Disconnect();
        }

    }
}
