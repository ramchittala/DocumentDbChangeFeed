// -----------------------------------------------------------------------
// <copyright file="DocumentFeedObserver.cs" Author="Ram Chittala">
// Copyright (c) . All rights reserved. 
// </copyright>
// -----------------------------------------------------------------------
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Threading;

namespace ChangeFeed.Core
{
    /// <summary>
    /// This class implements the IChangeFeedObserver interface and is used to observe 
    /// changes on change feed. ChangeFeedEventHost will create as many instances of 
    /// this class as needed. 
    /// </summary>
    public class DocumentFeedObserver : IChangeFeedObserver
    {
        private static int totalDocs = 0;
       

        private readonly Func<ChangeFeedObserverContext, Document, Task> _checkpoint;

        private IChangeFeedDataProcessor ChangeFeedDataProcessor { get; set; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public ChangeFeedObserverContext Context { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFeedObserver" /> class.
        /// Saves input DocumentClient and DocumentCollectionInfo parameters to class fields
        /// </summary>
        /// <param name="client"> Client connected to destination collection </param>
        /// <param name="destCollInfo"> Destination collection information </param>
        public DocumentFeedObserver(ChangeFeedDataProcessConfiguration dataProcessConfiguration)
        {
            this.ChangeFeedDataProcessor = dataProcessConfiguration.ChangeFeedDataProcessor;
            var checkpointStrategy = CreateBatchCheckpointStrategy(dataProcessConfiguration.BatchSize);
            _checkpoint = (context, data) => checkpointStrategy(context.CheckpointAsync, data);
        }

        /// <summary>
        /// Called when change feed observer is opened; 
        /// this function prints out observer partition key id. 
        /// </summary>
        /// <param name="context">The context specifying partition for this observer, etc.</param>
        /// <returns>A Task to allow asynchronous execution</returns>
        public Task OpenAsync(ChangeFeedObserverContext context)
        {
            this.Context = context;
            Console.WriteLine("Observer opened for partition Key Range: {0}", context.PartitionKeyRangeId);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Called when change feed observer is closed; 
        /// this function prints out observer partition key id and reason for shut down. 
        /// </summary>
        /// <param name="context">The context specifying partition for this observer, etc.</param>
        /// <param name="reason">Specifies the reason the observer is closed.</param>
        /// <returns>A Task to allow asynchronous execution</returns>
        public Task CloseAsync(ChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
        {
            Console.WriteLine("Observer closed, {0}", context.PartitionKeyRangeId);
            Console.WriteLine("Reason for shutdown, {0}", reason);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// When document changes are available on change feed, changes are copied to destination connection; 
        /// this function prints out the changed document ID. 
        /// </summary>
        /// <param name="context">The context specifying partition for this observer, etc.</param>
        /// <param name="docs">The documents changed.</param>
        /// <returns>A Task to allow asynchronous execution</returns>
        public async Task ProcessChangesAsync(ChangeFeedObserverContext context, IReadOnlyList<Document> docs)
        {
            Console.WriteLine("Change feed: PartitionId {0} total {1} doc(s)", context.PartitionKeyRangeId, Interlocked.Add(ref totalDocs, docs.Count));
            foreach (Document doc in docs)
            {
                Console.WriteLine(doc.Id.ToString());
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(doc.Id.ToString());
				// if you want process each record, write your implemention here
                     // write your implemention here

				//if you want to process batch 
                await _checkpoint(context, doc);
            }
        }

        internal Func<Func<Task>, Document, Task> CreateBatchCheckpointStrategy(int batchSize)
        {
            int batchCounter = 1;
            var changeFeedBatchdata = new ChangeFeedBatch();
            return async (checkpoint, data) =>
            {
                var doc = (dynamic)data;
                changeFeedBatchdata.Docs.Add(data);
                batchCounter++;
                if (batchCounter > batchSize)
                {
					//Process your batch data and call check point
                    await this.ChangeFeedDataProcessor.ExecuteAsync(changeFeedBatchdata, checkpoint);
                    batchCounter = 1;
                    changeFeedBatchdata = new ChangeFeedBatch();
                }
            };

        }

    }
}
