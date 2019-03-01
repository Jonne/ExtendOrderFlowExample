using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.ManagedLists;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;
using System;
using System.Threading.Tasks;

namespace Plugin.ExtendOrderFlow.Pipelines.Blocks
{
    [PipelineDisplayName("ExtendOrderFlow.block.MoveOrderToWaitingForPayment")]
    public class MoveOrderToWaitingForPayment : PipelineBlock<Order, Order, CommercePipelineExecutionContext>
    {
        public override Task<Order> Run(Order arg, CommercePipelineExecutionContext context)
        {
            KnownOrderStatusPolicy policy1 = context.GetPolicy<KnownOrderStatusPolicy>();
            if (!arg.Status.Equals(policy1.Pending, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(arg);
            }

            arg.Status = "WaitingForPayment";
            arg.GetComponent<TransientListMembershipsComponent>().Memberships.Add("WaitingForPaymentOrders");
            return Task.FromResult(arg);
        }
    }
}
