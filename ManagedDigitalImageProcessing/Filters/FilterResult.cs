using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters
{
    public sealed class FilterResult
    {
        public PgmHeader Header { get; set; }

        public byte[] XData { get; set; }
        public byte[] YData { get; set; }
    }
}
