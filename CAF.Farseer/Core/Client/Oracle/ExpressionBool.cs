﻿using CAF.FS.Core.Data;
using CAF.FS.Core.Infrastructure;

namespace CAF.FS.Core.Client.Oracle
{
    public class ExpressionBool : Common.ExpressionBool
    {
        public ExpressionBool(BaseQueueManger queueManger, Queue queue) : base(queueManger, queue) { }
    }
}
