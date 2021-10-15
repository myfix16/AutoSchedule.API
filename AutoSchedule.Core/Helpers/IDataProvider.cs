namespace AutoSchedule.Core.Helpers
{
    public interface IDataProvider<out T>
    {
        public T GetData();
    }
}
