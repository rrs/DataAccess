namespace Rrs.DataAccess
{
    public interface IQuery<out T>
    {
        T Execute();
    }
}
