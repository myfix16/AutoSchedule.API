using System.Threading.Tasks;

namespace AutoSchedule.Core.Helpers
{
    public interface IDataProviderAsync<T>
    {
        public Task<T> GetDataAsync();
    }
}
