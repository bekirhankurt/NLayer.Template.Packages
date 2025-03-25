namespace Persistence.Dynamics.Concrete;

public class Dynamic
{
    public IEnumerable<Sort> Sorts { get; set; }
    public Filter Filter { get; set; }

    public Dynamic()
    {
        
    }

    public Dynamic(IEnumerable<Sort> sorts, Filter filter)
    {
        
    }
}