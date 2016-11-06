namespace ConsoleApplication2.ReadModel.Translators
{
    using ConsoleApplication2.Core;
    using ConsoleApplication2.Data.Entities;
    using ConsoleApplication2.ReadModel.Events;

    public class ReaderAddedToDeviceToReaderDbTranslator : ITranslator<ReaderAddedToDevice, ReaderDb>
    {
        public ReaderDb Translate(ReaderAddedToDevice @from)
        {
            return new ReaderDb()
            {
                Id = @from.Id,
                Number = @from.Number
            };
        }

        public ReaderDb Translate(ReaderAddedToDevice t, ReaderDb tr)
        {
            throw new System.NotImplementedException();
        }
    }
}