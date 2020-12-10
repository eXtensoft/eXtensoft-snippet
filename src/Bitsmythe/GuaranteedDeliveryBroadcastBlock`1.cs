using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Bitsmythe
{
    public class GuaranteedDeliveryBroadcastBlock<T> : IPropagatorBlock<T, T>
    {
        private BroadcastBlock<T> _BroadcastBlock;
        private Task _Completion;

        public GuaranteedDeliveryBroadcastBlock(Func<T,T> cloningFunction)
        {
            _BroadcastBlock = new BroadcastBlock<T>(cloningFunction);
            _Completion = _BroadcastBlock.Completion;
        }
        public Task Completion => _Completion;

        public void Complete()
        {
            ((ITargetBlock<T>)_BroadcastBlock).Complete();
        }

        public T ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
        {
            throw new NotImplementedException();
        }

        public void Fault(Exception exception)
        {
            ((ITargetBlock<T>)_BroadcastBlock).Fault(exception);
        }

        public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
        {
            var bufferBlock = new BufferBlock<T>();
            var d1 =_BroadcastBlock.LinkTo(bufferBlock,linkOptions);
            var d2 = bufferBlock.LinkTo(target, linkOptions);
            _Completion.ContinueWith(a => bufferBlock.Completion);
            return new DisposableDisposer(d1, d2);
        }

        public DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept)
        {
            return ((ITargetBlock<T>)_BroadcastBlock).OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }

        public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            throw new NotImplementedException();
        }

        public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            throw new NotSupportedException("The method should not be called. The producer is a BufferBlock<>");
        }
    }
}
