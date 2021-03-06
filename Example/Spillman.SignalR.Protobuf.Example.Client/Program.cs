﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.AspNetCore.SignalR.Client;
using Unofficial.SignalR.Protobuf.Test.Core;

namespace Spillman.SignalR.Protobuf.Example.Client
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                var client = new HubConnectionBuilder()
                    .WithUrl("http://localhost:55581/realtime")
                    .AddProtobufProtocol(
                        new Dictionary<int, Type>
                        {
                            [0] = typeof(TestMessage)
                        }
                    )
                    .Build();

                client.On<List<IMessage>>(
                    "HandleTestMessage",
                    message =>
                    {
                        Debug.WriteLine($"Client received {message.Count} items!");
                    }
                );

                await client.StartAsync();
                await client.InvokeAsync(
                    "HandleTestMessage", 
                    new TestMessage { Value = "This came from the client!" }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
