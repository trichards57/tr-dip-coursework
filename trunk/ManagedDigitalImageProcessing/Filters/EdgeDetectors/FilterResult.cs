using ManagedDigitalImageProcessing.PGM;

namespace ManagedDigitalImageProcessing.Filters.EdgeDetectors
{
    public sealed class SobelFilterResult
    {
        public PgmHeader Header { get; set; }

        public byte[] XData { get; set; }
        public byte[] YData { get; set; }
    }
}
