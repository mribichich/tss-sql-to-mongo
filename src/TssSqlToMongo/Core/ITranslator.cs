namespace TssSqlToMongo.Core
{
    public interface ITranslator<in T, TR>
    {
        TR Translate(T t);

        TR Translate(T t, TR tr);
    }
}