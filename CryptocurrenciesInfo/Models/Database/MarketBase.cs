using System.ComponentModel.DataAnnotations;

namespace CryptocurrenciesInfo.Models.DataBase;

public class MarketBase : IEquatable<MarketBase>, IEqualityComparer<MarketBase>
{
    [MinLength(1)] public virtual required string Name { get; set; }

    [MinLength(1)] public virtual required string Base { get; set; }

    [MinLength(1)] public virtual required string Target { get; set; }

    public bool Equals(MarketBase? x, MarketBase? y)
    {
        return x is not null && x.Equals(y);
    }

    public int GetHashCode(MarketBase obj)
    {
        return HashCode.Combine(obj.Name.ToLower(), obj.Base.ToLower(), obj.Target.ToLower());
    }

    public virtual bool Equals(MarketBase? other)
    {
        return other is not null && Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase)
                                 && Base.Equals(other.Base, StringComparison.CurrentCultureIgnoreCase)
                                 && Target.Equals(other.Target, StringComparison.CurrentCultureIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as MarketBase);
    }

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }
}