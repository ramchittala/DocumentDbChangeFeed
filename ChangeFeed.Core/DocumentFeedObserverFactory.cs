// -----------------------------------------------------------------------
// <copyright file="DocumentFeedObserverFactory.cs" Author="Ram Chittala">
// Copyright (c) . All rights reserved. 
// </copyright>
// -----------------------------------------------------------------------
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeFeed.Core
{
    /// <summary>
    /// Factory class to create instance of document feed observer. 
    /// </summary>
    public class DocumentFeedObserverFactory : IChangeFeedObserverFactory
    {
      

        private ChangeFeedDataProcessConfiguration DataProcessConfiguration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFeedObserverFactory" /> class.
        /// Saves input DocumentClient and DocumentCollectionInfo parameters to class fields
        /// </summary>
        /// <param name="destCollInfo">Destination collection information</param>
        public DocumentFeedObserverFactory(ChangeFeedDataProcessConfiguration dataProcessConfiguration)
        {
            this.DataProcessConfiguration = dataProcessConfiguration;
        }

        /// <summary>
        /// Creates document observer instance with client and destination collection information
        /// </summary>
        /// <returns>DocumentFeedObserver with client and destination collection information</returns>
        public IChangeFeedObserver CreateObserver()
        {
            var newObserver = new DocumentFeedObserver(this.DataProcessConfiguration);
            return newObserver;
        }
    }
}
