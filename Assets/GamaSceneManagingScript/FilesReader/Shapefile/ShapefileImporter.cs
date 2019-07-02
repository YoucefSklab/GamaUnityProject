using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using UnityEngine;

namespace ummisco.gama.unity.files.ShapefileImporter
{
    /// <summary>
    /// Enumeration defining the various shape types. Each shapefile
    /// contains only one type of shape (e.g., all polygons or all
    /// polylines).
    /// </summary>

    public enum ShapeType
    {
        /// <summary>
        /// Nullshape / placeholder record.
        /// </summary>
        NullShape = 0,

        /// <summary>
        /// Point record, for defining point locations such as a city.
        /// </summary>
        Point = 1,

        /// <summary>
        /// One or more sets of connected points. Used to represent roads,
        /// hydrography, etc.
        /// </summary>
        PolyLine = 3,

        /// <summary>
        /// One or more sets of closed figures. Used to represent political
        /// boundaries for countries, lakes, etc.
        /// </summary>
        Polygon = 5,

        /// <summary>
        /// A cluster of points represented by a single shape record.
        /// </summary>
        Multipoint = 8

        // Unsupported types:
        // PointZ = 11,        
        // PolyLineZ = 13,        
        // PolygonZ = 15,        
        // MultiPointZ = 18,        
        // PointM = 21,        
        // PolyLineM = 23,        
        // PolygonM = 25,        
        // MultiPointM = 28,        
        // MultiPatch = 31
    }
    
}

// END