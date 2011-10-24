﻿namespace ManagedDigitalImageProcessing.PGM
{
    /// <summary>
    /// The header of a %PGM file.
    /// </summary>
    public sealed class PgmHeader
    {
        /// <summary>
        /// Gets or sets the height of the file.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets the width of the file.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }
    }
}
