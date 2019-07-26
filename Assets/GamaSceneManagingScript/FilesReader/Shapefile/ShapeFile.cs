using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using UnityEngine;


namespace ummisco.gama.unity.files.ShapefileImporter
{
    /// <summary>
    /// The ShapeFile class represents the contents of a single
    /// ESRI shapefile. This is the class which contains functionality
    /// for reading shapefiles and their corresponding dBASE attribute
    /// files.
    /// </summary>
    /// <remarks>
    /// You can call the Read() method to import both shapes and attributes
    /// at once. Or, you can open the file stream yourself and read the file
    /// header or individual records one at a time. The advantage of this is
    /// that it allows you to implement your own progress reporting functionality,
    /// for example.
    /// </remarks>
    public class ShapeFile
    {
        #region Constants
        private const int expectedFileCode = 9994;
        #endregion Constants

        #region Private static fields
        private static byte[] intBytes = new byte[4];
        private static byte[] doubleBytes = new byte[8];
        #endregion Private static fields

        #region Private fields
        // File Header.
        private ShapeFileHeader fileHeader = new ShapeFileHeader();

        // Collection of Shapefile Records.
        private Collection<ShapeFileRecord> records = new Collection<ShapeFileRecord>();
        public Collection<ShapeFileRecord> Myrecords = new Collection<ShapeFileRecord>();
        #endregion Private fields

        #region Constructor
        /// <summary>
        /// Constructor for the ShapeFile class.
        /// </summary>
        public ShapeFile()
        {
        }
        #endregion Constructor

        #region Properties
        /// <summary>
        /// Access the file header of this shapefile.
        /// </summary>
        public ShapeFileHeader FileHeader
        {
            get { return this.fileHeader; }
        }

        /// <summary>
        /// Access the collection of records for this shapefile.
        /// </summary>
        public Collection<ShapeFileRecord> Records
        {
            get { return this.records; }
        }
        public Collection<ShapeFileRecord> MyRecords
        {
            get { return this.Myrecords; }
        }
        #endregion Properties

        #region Public methods
        /// <summary>
        /// Read both shapes and attributes from the given
        /// shapefile (and its corresponding dBASE file).
        /// This is the top-level method for reading an ESRI
        /// shapefile.
        /// </summary>
        /// <param name="fileName">Full pathname of the shapefile.</param>
        public void Read(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            // Read shapes first (geometry).
            this.ReadShapes(fileName, 2000000, 1, 2000000, 1);

            // Construct name and path of dBASE file. It's basically
            // the same name as the shapefile except with a .dbf extension.
            string dbaseFile = fileName.Replace(".shp", ".dbf");
            dbaseFile = dbaseFile.Replace(".SHP", ".DBF");

            // Read the attributes.
            this.ReadAttributes(dbaseFile);
        }

        /// <summary>
        /// Read shapes (geometry) from the given shapefile.
        /// </summary>
        /// <param name="fileName">Full pathname of the shapefile.</param>
        public void ReadShapes(string fileName, double XMa, double XMi, double YMa, double YMi)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                this.ReadShapes(stream, XMa, XMi, YMa, YMi);
            }
        }

        /// <summary>
        /// Read shapes (geometry) from the given stream.
        /// </summary>
        /// <param name="stream">Input stream for a shapefile.</param>
        public void ReadShapes(Stream stream, double XMa, double XMi, double YMa, double YMi)
        {
            // Read the File Header.
            this.ReadShapeFileHeader(stream);

            // Read the shape records.
            this.records.Clear();
            this.Myrecords.Clear();
            while (true)
            {
                try
                {
                    this.ReadShapeFileRecord(stream);
                }
                catch (IOException)
                {
                    // Stop reading when EOF exception occurs.
                    break;
                }
            }
        }

        /// <summary>
        /// Read the file header of the shapefile.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        public void ReadShapeFileHeader(Stream stream)
        {
            // File Code.
            this.fileHeader.FileCode = ShapeFile.ReadInt32_BE(stream);
            if (this.fileHeader.FileCode != ShapeFile.expectedFileCode)
            {
                string msg = String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid FileCode encountered. Expecting {0}.", ShapeFile.expectedFileCode);
                throw new ArgumentException(msg);
            }

            // 5 unused values.
            ShapeFile.ReadInt32_BE(stream);
            ShapeFile.ReadInt32_BE(stream);
            ShapeFile.ReadInt32_BE(stream);
            ShapeFile.ReadInt32_BE(stream);
            ShapeFile.ReadInt32_BE(stream);

            // File Length.
            this.fileHeader.FileLength = ShapeFile.ReadInt32_BE(stream);

            // Version.
            this.fileHeader.Version = ShapeFile.ReadInt32_LE(stream);

            // Shape Type.
            this.fileHeader.ShapeType = ShapeFile.ReadInt32_LE(stream);

            // Bounding Box.
            this.fileHeader.XMin = ShapeFile.ReadDouble64_LE(stream);
            this.fileHeader.YMin = ShapeFile.ReadDouble64_LE(stream);
            this.fileHeader.XMax = ShapeFile.ReadDouble64_LE(stream);
            this.fileHeader.YMax = ShapeFile.ReadDouble64_LE(stream);

            // Adjust the bounding box in case it is too small.
            if (Math.Abs(this.fileHeader.XMax - this.fileHeader.XMin) < 1)
            {
                this.fileHeader.XMin -= 5;
                this.fileHeader.XMax += 5;
            }
            if (Math.Abs(this.fileHeader.YMax - this.fileHeader.YMin) < 1)
            {
                this.fileHeader.YMin -= 5;
                this.fileHeader.YMax += 5;
            }

            // Skip the rest of the file header.
            stream.Seek(100, SeekOrigin.Begin);
        }

        /// <summary>
        /// Read a shapefile record.
        /// </summary>
        /// <param name="stream">Input stream.</param>




        public ShapeFileRecord ReadShapeFileRecord(Stream stream)
        {
            ShapeFileRecord record = new ShapeFileRecord();
            //  MainWindow mw = new MainWindow();
            // Record Header.

            record.RecordNumber = ShapeFile.ReadInt32_BE(stream);
            record.ContentLength = ShapeFile.ReadInt32_BE(stream);

            // Shape Type.
            record.ShapeType = ShapeFile.ReadInt32_LE(stream);

            // Read the shape geometry, depending on its type.
            switch (record.ShapeType)
            {
                case (int)ShapeType.NullShape:
                    // Do nothing.
                    break;
                case (int)ShapeType.Point:
                    ShapeFile.ReadPoint(stream, record);
                    break;
                case (int)ShapeType.PolyLine:
                    // PolyLine has exact same structure as Polygon in shapefile.
                    ShapeFile.ReadPolygon(stream, record);
                    break;
                case (int)ShapeType.Polygon:
                    ShapeFile.ReadPolygon(stream, record);
                    break;
                case (int)ShapeType.Multipoint:
                    ShapeFile.ReadMultipoint(stream, record);
                    break;
                default:
                    {
                        string msg = String.Format(System.Globalization.CultureInfo.InvariantCulture, "ShapeType {0} is not supported.", (int)record.ShapeType);
                        throw new ArgumentException(msg);
                    }
            }

            // Add the record to our internal list.
            if (record.Points.Count < 1)
            {
                return record;
            }
            else
            {
                //这里把parts里面的polygon分别传给一个record
                if (record.Parts.Count > 1)
                {
                    for (int a = 0; a < record.Parts.Count; a++)
                    {
                        ShapeFileRecord record1 = new ShapeFileRecord();
                        if (a + 1 < record.Parts.Count)
                        {
                            for (int b = record.Parts[a]; b < record.Parts[a + 1]; b++)
                            {

                                Vector2 p = new Vector2();
                                p.x = record.Points[b].x;
                                p.y = record.Points[b].y;
                                record1.Points.Add(p);

                            }
                            record1.ShapeType = 5;
                            record1.Parts.Add(record.Parts[a]);
                            this.Myrecords.Add(record1);
                        }
                        else
                        {
                            for (int b = record.Parts[a]; b < record.Points.Count; b++)
                            {

                                Vector2 p = new Vector2();
                                p.x = record.Points[b].x;
                                p.y = record.Points[b].y;
                                record1.Points.Add(p);

                            }
                            record1.ShapeType = 5;
                            record1.Parts.Add(0);
                            this.Myrecords.Add(record1);
                        }


                    }
                    this.records.Add(record);
                    return record;

                }
                this.records.Add(record);
                this.Myrecords.Add(record);
                return record;
            }
        }

        /// <summary>
        /// Read the table from specified dBASE (DBF) file and
        /// merge the rows with shapefile records.
        /// </summary>
        /// <remarks>
        /// The filename of the dBASE file is expected to follow 8.3 naming
        /// conventions. If it doesn't follow the convention, we try to
        /// determine the 8.3 short name ourselves (but the name we come up
        /// with may not be correct in all cases).
        /// </remarks>
        /// <param name="dbaseFile">Full file path of the dBASE (DBF) file.</param>
        public void ReadAttributes(string dbaseFile)
        {
            if (string.IsNullOrEmpty(dbaseFile))
                throw new ArgumentNullException("dbaseFile");

            // Check if the file exists. If it doesn't exist,
            // this is not an error.
            if (!File.Exists(dbaseFile))
                return;

            // Get the directory in which the dBASE (DBF) file resides.
            FileInfo fi = new FileInfo(dbaseFile);
            string directory = fi.DirectoryName;

            // Get the filename minus the extension.
            string fileNameNoExt = fi.Name.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            if (fileNameNoExt.EndsWith(".DBF"))
                fileNameNoExt = fileNameNoExt.Substring(0, fileNameNoExt.Length - 4);

            // Convert to a short filename (may not work in every case!).
            if (fileNameNoExt.Length > 8)
            {
                if (fileNameNoExt.Contains(" "))
                {
                    string noSpaces = fileNameNoExt.Replace(" ", "");
                    if (noSpaces.Length > 8)
                        fileNameNoExt = noSpaces;
                }
                fileNameNoExt = fileNameNoExt.Substring(0, 6) + "~1";
            }
            /*

            // Set the connection string.
            string connectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" + directory + ";Extended Properties=dBASE 5.0";

            // Set the select query.
            string selectQuery = "SELECT * FROM [" + fileNameNoExt + "#DBF];";
            //--//
            // Create a database connection object using the connection string.
            OleDbConnection connection = new OleDbConnection(connectionString);

            //SqlConnection connection = new SqlConnection(connectionString);

            // Create a database command on the connection using the select query.
            OleDbCommand command = new OleDbCommand(selectQuery, connection);

            try
            {
                // Open the connection.          
                connection.Open();

                // Create a data adapter to fill a dataset.
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = command;

                dataSet.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(dataSet);

                // Merge attributes into the shape file.
                if (dataSet.Tables.Count > 0)
                    this.MergeAttributes(dataSet.Tables[0]);
            }
            catch (OleDbException)
            {
                // Note: An exception will occur if the filename of the dBASE
                // file does not follow 8.3 naming conventions. In this case,
                // you must use its short (MS-DOS) filename.

                // Rethrow the exception.
                throw;
            }
            finally
            {
                // Dispose of connection.
                ((IDisposable)connection).Dispose();
            }

        */

        }

        /// <summary>
        /// Output the File Header in the form of a string.
        /// </summary>
        /// <returns>A string representation of the file header.</returns>
        public override string ToString()
        {
            return "ShapeFile: " + this.fileHeader.ToString();
        }
        #endregion Public methods

        #region Private methods
        /// <summary>
        /// Read a 4-byte integer using little endian (Intel)
        /// byte ordering.
        /// </summary>
        /// <param name="stream">Input stream to read.</param>
        /// <returns>The integer value.</returns>
        private static int ReadInt32_LE(Stream stream)
        {
            for (int i = 0; i < 4; i++)
            {
                int b = stream.ReadByte();
                if (b == -1)
                    throw new EndOfStreamException();
                intBytes[i] = (byte)b;
            }

            return BitConverter.ToInt32(intBytes, 0);
        }

        /// <summary>
        /// Read a 4-byte integer using big endian
        /// byte ordering.
        /// </summary>
        /// <param name="stream">Input stream to read.</param>
        /// <returns>The integer value.</returns>
        private static int ReadInt32_BE(Stream stream)
        {
            for (int i = 3; i >= 0; i--)
            {
                int b = stream.ReadByte();
                if (b == -1)
                    throw new EndOfStreamException();
                intBytes[i] = (byte)b;
            }

            return BitConverter.ToInt32(intBytes, 0);
        }

        /// <summary>
        /// Read an 8-byte double using little endian (Intel)
        /// byte ordering.
        /// </summary>
        /// <param name="stream">Input stream to read.</param>
        /// <returns>The double value.</returns>
        private static double ReadDouble64_LE(Stream stream)
        {
            for (int i = 0; i < 8; i++)
            {
                int b = stream.ReadByte();
                if (b == -1)
                    throw new EndOfStreamException();
                doubleBytes[i] = (byte)b;
            }

            return BitConverter.ToDouble(doubleBytes, 0);
        }
        public DataSet dataSet = new DataSet();

        /// <summary>
        /// Read a shapefile Point record.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <param name="record">Shapefile record to be updated.</param>
        private static void ReadPoint(Stream stream, ShapeFileRecord record)
        {
            // Points - add a single point.
            //,double XMax,double XMin,double YMax,double YMin
            Vector2 p = new Vector2();
            p.x = (float)ShapeFile.ReadDouble64_LE(stream);
            p.y = (float)ShapeFile.ReadDouble64_LE(stream);

            record.Points.Add(p);

            // Bounding Box.
            record.XMin = p.x;
            record.YMin = p.y;
            record.XMax = record.XMin;
            record.YMax = record.YMin;
        }

        /// <summary>
        /// Read a shapefile MultiPoint record.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <param name="record">Shapefile record to be updated.</param>
        private static void ReadMultipoint(Stream stream, ShapeFileRecord record)
        {
            // Bounding Box.
            record.XMin = ShapeFile.ReadDouble64_LE(stream);
            record.YMin = ShapeFile.ReadDouble64_LE(stream);
            record.XMax = ShapeFile.ReadDouble64_LE(stream);
            record.YMax = ShapeFile.ReadDouble64_LE(stream);

            // Num Points.
            int numPoints = ShapeFile.ReadInt32_LE(stream);

            // Points.           
            for (int i = 0; i < numPoints; i++)
            {
                Vector2 p = new Vector2();
                p.x = (float)ShapeFile.ReadDouble64_LE(stream);
                p.y = (float)ShapeFile.ReadDouble64_LE(stream);
                record.Points.Add(p);
            }
        }

        /// <summary>
        /// Read a shapefile Polygon record.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <param name="record">Shapefile record to be updated.</param>
        private static void ReadPolygon(Stream stream, ShapeFileRecord record)
        {
            // Bounding Box.
            record.XMin = ShapeFile.ReadDouble64_LE(stream);
            record.YMin = ShapeFile.ReadDouble64_LE(stream);
            record.XMax = ShapeFile.ReadDouble64_LE(stream);
            record.YMax = ShapeFile.ReadDouble64_LE(stream);

            // Num Parts and Points.
            int numParts = ShapeFile.ReadInt32_LE(stream);
            int numPoints = ShapeFile.ReadInt32_LE(stream);

            // Parts.           
            for (int i = 0; i < numParts; i++)
            {
                record.Parts.Add(ShapeFile.ReadInt32_LE(stream));
            }

            // Points.           
            for (int i = 0; i < numPoints; i++)
            {
                Vector2 p = new Vector2();
                p.x = (float)ShapeFile.ReadDouble64_LE(stream);
                p.y = (float)ShapeFile.ReadDouble64_LE(stream);
                //    if (p.X > XMi - (XMa - XMi) && p.X < XMa +   (XMa - XMi) && p.Y < YMa+YMa-YMi  && p.Y > YMi-YMa +YMi )
                //    {
                record.Points.Add(p);
                //    }
            }
        }

        /// <summary>
        /// Merge data rows from the given table with
        /// the shapefile records.
        /// </summary>
        /// <param name="table">Attributes table.</param>
        private void MergeAttributes(DataTable table)
        {
            // For each data row, assign it to a shapefile record.
            int index = 0;
            foreach (DataRow row in table.Rows)
            {
                if (index >= this.records.Count)
                    break;
                this.records[index].Attributes = row;
                ++index;
            }
        }
        #endregion Private methods
    }
}