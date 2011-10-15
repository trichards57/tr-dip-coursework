using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using ManagedDigitalImageProcessing.PGM.Exceptions;

namespace ManagedDigitalImageProcessing.PGM
{
    /// <summary>
    /// Loads an image out of a %PGM file.
    /// </summary>
    static class PgmLoader
    {
        /// <summary>
        /// Loads a %PGM image.
        /// </summary>
        /// <param name="instream">The stream to load the image from.</param>
        /// <returns>A PgmImage containing the loaded image.</returns>
        /// <exception cref="InvalidPgmHeaderException">Thrown if there isn't enough data in the file to fill the image defined in the header.</exception>
        /// <remarks>Not particularly safe.  Falls down if the entire file is long enough to fill the image but the actual data section (i.e. the section after the header) isn't (it will load the header in to the image).</remarks>
        public static PgmImage LoadImage(Stream instream)
        {
            var reader = new StreamReader(instream);
            var header = ReadHeader(reader);

            int length = header.Height * header.Width;

            instream.Seek(-length, SeekOrigin.End);
            var binaryReader = new BinaryReader(instream);

            var data = binaryReader.ReadBytes(length);

            if (data.Length < header.Height * header.Width)
                throw new InvalidPgmHeaderException(string.Format("Not enough data in the file. {0} bytes read.", data.Length));

            return new PgmImage { Data = data, Header = header };   
        }

        /// <summary>
        /// Reads the header from a %PGM image.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <returns>A PgmHeader loaded from <paramref name="reader"/>.</returns>
        public static PgmHeader ReadHeader(StreamReader reader)
        {
            var header = new PgmHeader();
            var line = reader.ReadLine();

            if (line != "P5")
                throw new InvalidPgmHeaderException(string.Format("First line was not 'P5' : {0}", line));

            line = reader.ReadLine();

            var parts = line.Split(new[] { ' ', '\r', '\n' });

            int col, row, maxGrey;

            if (!(int.TryParse(parts[0], out col) && int.TryParse(parts[1], out row)))
                throw new InvalidPgmHeaderException(string.Format("Couldn't read column and row definition : {0}", line));

            header.Width = col;
            header.Height = row;

            line = reader.ReadLine();

            if (!int.TryParse(line, out maxGrey))
                throw new InvalidPgmHeaderException(string.Format("Couldn't read maximum grey value : {0}", line));
            if (maxGrey > 256)
                throw new InvalidPgmHeaderException(string.Format("Maximum grey value too large : {0}", maxGrey));

            return header;
        }
    }
}
