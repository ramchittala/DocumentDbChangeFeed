// -----------------------------------------------------------------------
// <copyright file="ChangeFeedDataProcessConfiguration.cs" Author="Ram Chittala">
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
    public class ChangeFeedDataProcessConfiguration
    {
       public int BatchSize { get; set; }

       public IChangeFeedDataProcessor ChangeFeedDataProcessor { get; set; }

		public DocumentCollectionInfo SourceDocumentCollectionInfo { get; set; }

		public DocumentCollectionInfo LeaseDocumentCollectionInfo { get; set; }

		public DateTime ChangeFeedStartTime { get; set; }
	}
}
