namespace MdControl;

/// <summary>
/// Helper class to understand if a collection has been changed, including re-ordering.
/// For collection elements, you MUST either overridde `GetHashCode()`, or use overloaded constructor
/// to extract proper hash from each item.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CollectionHash<T> {
    private long _hash;
    private Func<T, object> transform = t => t!;

    public CollectionHash() { }
    public CollectionHash(Func<T, object> transform) => this.transform = transform;

    public bool NeedUpdate(IEnumerable<T> items) {
        unchecked {
            // don't use Sum(), it may cause ArithmeticOverflow even inside "unchecked" context
            long newHash = items.Aggregate(0, (acc, next, i) => acc + (i + 1) * transform(next).GetHashCode());
            if (newHash != _hash) {
                _hash = newHash;
                return true;
            }
            return false;
        }
    }
}

public static class Tommy {
    public static A Aggregate<T, A>(this IEnumerable<T> src, A seed, Func<A, T, int, A> f) {
        var acc = seed;
        var i = 0;
        foreach (var item in src) acc = f(acc, item, i++);
        return acc;
    }
}
