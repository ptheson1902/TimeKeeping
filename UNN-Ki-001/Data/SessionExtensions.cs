using Newtonsoft.Json;
using System.Linq.Expressions;

namespace UNN_Ki_001.Data
{
    public static class SessionExtensions
    {
        // セッションにオブジェクトを書き込む
        public static void SetObj<TObject>(this ISession session, string key, TObject obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            session.SetString(key, json);
        }

        // セッションからオブジェクトを読み込む
        public static TObject? GetObject<TObject>(this ISession session, string key)
        {
            string? json = session.GetString(key);
            return string.IsNullOrEmpty(json) ? default :JsonConvert.DeserializeObject<TObject>(json);
        }
    }

    public static class LinqExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>
                (this IQueryable<TSource> Source, bool Condition, Expression<Func<TSource, bool>> Predicate)
        {
            if (Condition)
                return Source.Where(Predicate);
            else
                return Source;
        }
    }
}
