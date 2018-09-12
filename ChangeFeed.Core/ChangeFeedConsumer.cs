// -----------------------------------------------------------------------
// <copyright file="ChangeFeedConsumer.cs" Author="Ram Chittala">
// Copyright (c) . All rights reserved. 
// </copyright>
// -----------------------------------------------------------------------
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChangeFeed.Core
{
	public class ChangeFeedConsumer
	{
		/// <summary>
		/// Gets or sets the event hub configuration.
		/// </summary>
		/// <value>
		/// The event hub configuration.
		/// </value>
		private ChangeFeedDataProcessConfiguration Config { get; set; }


		/// <summary>
		/// Gets or sets the name of the host.
		/// </summary>
		/// <value>
		/// The name of the host. it should be a worker role instance id or unique id
		/// </value>
		private string HostName { get; set; }

		
		private ChangeFeedEventHost ChangeFeedEventHost { get; set; }

		
		private DocumentFeedObserverFactory DocumentFeedObserverFactory { get; set; }



		/// <summary>
		/// Initializes a new instance of the <see cref="EventHubConsumer"/> class.
		/// </summary>
		/// <param name="hostName">Name of the host.</param>
		/// <param name="config">The configuration.</param>
		public ChangeFeedConsumer(string hostName, ChangeFeedDataProcessConfiguration config)
		{
			this.Config = config;
			this.HostName = hostName;
		}
		public async Task StartConsuming(CancellationToken cancellationToken)
		{
			Console.WriteLine($"{DateTime.Now.ToString()} > HostName {this.HostName} Started Consuming........");
			await StartChangeFeedHost();
			await Task.Delay(-1, cancellationToken);
		}

		private async Task StartChangeFeedHost()
		{
			
				ConnectionPolicy ConnectionPolicy = new ConnectionPolicy
				{
					ConnectionMode = ConnectionMode.Direct,
					ConnectionProtocol = Protocol.Tcp,
					RequestTimeout = new TimeSpan(1, 0, 0),
					MaxConnectionLimit = 1000,
					RetryOptions = new RetryOptions
					{
						MaxRetryAttemptsOnThrottledRequests = 10,
						MaxRetryWaitTimeInSeconds = 60
					}
				};


				//Customizable change feed option and host options
				ChangeFeedOptions feedOptions = new ChangeFeedOptions();

			// ie customize StartFromBeginning so change feed reads from beginning
			// can customize MaxItemCount, PartitonKeyRangeId, RequestContinuation, SessionToken and StartFromBeginning
			feedOptions.StartTime = this.Config.ChangeFeedStartTime;
			var feedHostOptions = new ChangeFeedHostOptions
			{
				// ie. customizing lease renewal interval to 15 seconds
				// can customize LeaseRenewInterval, LeaseAcquireInterval, LeaseExpirationInterval, FeedPollDelay 
				LeaseRenewInterval = TimeSpan.FromSeconds(15)
			};

		     	string hostName = Guid.NewGuid().ToString();
		    	DocumentFeedObserverFactory docObserverFactory = new DocumentFeedObserverFactory(this.Config);
			    this.ChangeFeedEventHost = new ChangeFeedEventHost(hostName, Config.SourceDocumentCollectionInfo, this.Config.LeaseDocumentCollectionInfo, feedOptions, feedHostOptions);

			    await this.ChangeFeedEventHost.RegisterObserverFactoryAsync(docObserverFactory);

			    Console.WriteLine("Running... Press enter to stop.");

		}

		/// <summary>
		/// Stops the consuming.
		/// </summary>
		/// <returns></returns>
		public async Task StopConsuming()
		{
			Console.WriteLine($"{DateTime.Now.ToString()} > HostName:{this.HostName} Stoped Consuming........");

			await this.ChangeFeedEventHost.UnregisterObserversAsync();
		}

		
	}
}
