
namespace Bitsmith
{
    public interface IEndpointAction
    {
        EndpointOption Endpoint { get; set; }
        void Execute();
    }
}
