namespace zSpec.Pagination
{
    public interface IPaging
    {
        int Page { get; }

        int Take { get; }

        string OrderBy { get; }
    }
}