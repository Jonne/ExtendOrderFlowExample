using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;

namespace Plugin.ExtendOrderFlow.Pipelines
{
    public class WaitingForPaymentOrderMinionPipeline : CommercePipeline<Order, Order>, IWaitingForPaymentOrderMinionPipeline, IPipeline<Order, Order, CommercePipelineExecutionContext>, IPipelineBlock<Order, Order, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
        public WaitingForPaymentOrderMinionPipeline(IPipelineConfiguration<IWaitingForPaymentOrderMinionPipeline> configuration, ILoggerFactory loggerFactory)
          : base(configuration, loggerFactory)
        {
        }
    }
}
