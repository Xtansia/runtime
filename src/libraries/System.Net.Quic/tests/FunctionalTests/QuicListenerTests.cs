﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace System.Net.Quic.Tests
{
    [Collection(nameof(DisableParallelization))]
    [ConditionalClass(typeof(QuicTestBase), nameof(QuicTestBase.IsSupported))]
    public sealed class QuicListenerTests : QuicTestBase
    {
        public QuicListenerTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task Listener_Backlog_Success()
        {
            await Task.Run(async () =>
            {
                using QuicListener listener = await CreateQuicListener();

                using QuicConnection clientConnection = await CreateQuicConnection(listener.ListenEndPoint);
                var clientStreamTask = clientConnection.ConnectAsync();

                using QuicConnection serverConnection = await listener.AcceptConnectionAsync();
                await clientStreamTask;
            }).WaitAsync(TimeSpan.FromSeconds(6));
        }

        [Fact]
        public async Task Listener_Backlog_Success_IPv6()
        {
            await Task.Run(async () =>
            {
                using QuicListener listener = await CreateQuicListener(new IPEndPoint(IPAddress.IPv6Loopback, 0));

                using QuicConnection clientConnection = await CreateQuicConnection(listener.ListenEndPoint);
                var clientStreamTask = clientConnection.ConnectAsync();

                using QuicConnection serverConnection = await listener.AcceptConnectionAsync();
                await clientStreamTask;
            }).WaitAsync(TimeSpan.FromSeconds(6));
        }

        [Fact]
        public async Task Listener_IPv6Any_Accepts_IPv4()
        {
            await Task.Run(async () =>
            {
                using QuicListener listener = await CreateQuicListener(new IPEndPoint(IPAddress.IPv6Any, 0));

                using QuicConnection clientConnection = await CreateQuicConnection(new IPEndPoint(IPAddress.Loopback, listener.ListenEndPoint.Port));
                var clientStreamTask = clientConnection.ConnectAsync();

                using QuicConnection serverConnection = await listener.AcceptConnectionAsync();
                await clientStreamTask;
            }).WaitAsync(TimeSpan.FromSeconds(6));
        }
    }
}
