using System.Threading.Tasks;

namespace PlateDroplet.UI.ViewModels
{
    public interface IAsyncInitialization<TResult>
    {
        Task<TResult> Initialization { get; }
    }
}