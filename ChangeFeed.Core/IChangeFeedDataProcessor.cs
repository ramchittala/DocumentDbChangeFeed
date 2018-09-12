// -----------------------------------------------------------------------
// <copyright file="IChangeFeedDataProcessor.cs" Author="Ram Chittala">
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
   public interface IChangeFeedDataProcessor
    {
        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="batchData">The batch data.</param>
        /// <param name="checkPoint">The check point.</param>
        /// <returns></returns>
        Task ExecuteAsync(ChangeFeedBatch batchData, Func<Task> checkPoint);
    }
}
