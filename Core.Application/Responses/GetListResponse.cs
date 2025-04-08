using Persistence.Paging.Concrete;

namespace Application.Responses;

public class GetListResponse<T> : BasePageableModel
{
    private IList<T>? _items;
    

    public IList<T> Items
    {
        get => this._items ??= (IList<T>) new List<T>();
        set => this._items = value;
    }
}