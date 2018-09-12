// -----------------------------------------------------------------------
// <copyright file="Program.cs" Author="Ram Chittala">
// Copyright (c) . All rights reserved. 
// </copyright>
// -----------------------------------------------------------------------
using ChangeFeed.Core;
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
	class Program
	{
		private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		static void Main(string[] args)
		{

			InitiateChangeFeed().Wait();
		}

		static async Task InitiateChangeFeed()
		{
			string monitoredUri = ConfigurationManager.AppSettings["Source-Docdb-Uri"];
			string monitoredSecretKey = ConfigurationManager.AppSettings["Source-Docdb-Key"];
			string monitoredDbName = "your-Source-DbName";
			string monitoredCollectionName = "Your-Sourcce-CollecionName";


			
			string leaseUri = ConfigurationManager.AppSettings["Dest-Docdb-Uri"];
			string leaseSecretKey = ConfigurationManager.AppSettings["Dest-Docdb-Key"];
			string leaseDbName = "your lease db name"; //for Checkpointing
			string leaseCollectionName = "your lease db collection";


			var changeFeedConfig = new ChangeFeedDataProcessConfiguration();

			changeFeedConfig.SourceDocumentCollectionInfo = new DocumentCollectionInfo
			{
				Uri = new Uri(monitoredUri),
				MasterKey = monitoredSecretKey,
				DatabaseName = monitoredDbName,
				CollectionName = monitoredCollectionName
			};
			changeFeedConfig.LeaseDocumentCollectionInfo = new DocumentCollectionInfo
			{
				Uri = new Uri(leaseUri),
				MasterKey = leaseSecretKey,
				DatabaseName = leaseDbName,
				CollectionName = leaseCollectionName
			};
			changeFeedConfig.BatchSize = 1000; //if you want porcess by batch
			changeFeedConfig.ChangeFeedStartTime = DateTime.Now.AddHours(-1); //to start change feed form hour ago
			changeFeedConfig.ChangeFeedDataProcessor = new SampleChangeFeedDataProcessor();
			string hostName = Guid.NewGuid().ToString();
			var changeFeedConsumer = new ChangeFeedConsumer(hostName, changeFeedConfig);
			await changeFeedConsumer.StartConsuming(cancellationTokenSource.Token);
			Console.WriteLine("Running... Press enter to stop.");
			Console.ReadLine();
			await changeFeedConsumer.StopConsuming();
			
		}
	}
}
