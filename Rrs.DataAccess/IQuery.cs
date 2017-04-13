namespace Rrs.DataAccess
{
    public interface IQuery<T>
    {
        T Execute();
    }
}
