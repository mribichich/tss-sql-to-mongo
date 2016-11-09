namespace TssSqlToMongo.ReadModel.Translators
{
    using TssSqlToMongo.Core;
    using TssSqlToMongo.Data.Entities;
    using TssSqlToMongo.ReadModel.Events;

    public class ReaderAddedToDeviceToReaderDbTranslator : ITranslator<ReaderAddedToDevice, ReaderDb>
    {
        public ReaderDb Translate(ReaderAddedToDevice @from)
        {
            return new ReaderDb()
            {
                Id = @from.ReaderId,
                Number = @from.Number
            };
        }

        public ReaderDb Translate(ReaderAddedToDevice t, ReaderDb tr)
        {
            throw new System.NotImplementedException();
        }
    }
}