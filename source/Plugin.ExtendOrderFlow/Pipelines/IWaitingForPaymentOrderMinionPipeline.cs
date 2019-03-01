using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;

namespace Plugin.ExtendOrderFlow.Pipelines
{
    [PipelineDisplayName("ExtendOrderFlow.pipeline.waitingforpaymentorderminion")]
    public interface IWaitingForPaymentOrderMinionPipeline : IPipeline<Order, Order, CommercePipelineExecutionContext>, IPipelineBlock<Order, Order, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
    }
}
