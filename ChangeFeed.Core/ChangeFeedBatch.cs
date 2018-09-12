// -----------------------------------------------------------------------
// <copyright file="ChangeFeedBatch.cs" Author="Ram Chittala">
// Copyright (c) . All rights reserved. 
// </copyright>
// -----------------------------------------------------------------------
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeFeed.Core
{
    public class ChangeFeedBatch
    {
        public ChangeFeedBatch()
        {
            this.Docs = new List<dynamic>();
        }

		

       public List<dynamic> Docs { get; set; }

        /// <summary>
        /// Gets or sets the maximum enqueued time UTC.
        /// </summary>
        /// <value>
        /// The maximum enqueued time UTC.
        /// </value>
        public DateTime MaxEnqueuedTimeUtc { get; set; }
    }
}
