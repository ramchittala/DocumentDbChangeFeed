// -----------------------------------------------------------------------
// <copyright file="SampleChangeFeedDataProcessor.cs" Author="Ram Chittala">
// Copyright (c) . All rights reserved. 
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeFeed.Core
{
    public class SampleChangeFeedDataProcessor : IChangeFeedDataProcessor
    {

        public SampleChangeFeedDataProcessor()
        {
           
        }

        /// <summary>
        /// Executes the specified messages.
        /// </summary>
        /// <param name="batchData">The batch data.</param>
        /// <param name="checkPoint">The check point.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(ChangeFeedBatch batchData, Func<Task> checkPoint)
        {
			// process your data here
			await checkPoint();
			await Task.Delay(1);
        }

       
    }
}
