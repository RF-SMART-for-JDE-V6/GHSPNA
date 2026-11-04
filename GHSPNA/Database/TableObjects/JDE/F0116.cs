using System;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    /// <summary>
    /// A derived class for Enterprise 1 Address By Date Table - F0116
    /// </summary>
    public class F0116 : ICS.Database.TableObjects.JDE.F0116
    {
        #region Constructors

        /// <summary>
        /// Initializes a new F0116
        /// </summary>
        public F0116() : base()
        {
        }

        /// <summary>
        /// Initializes a new F0116 for the provided Data Access Layer and Table ID
        /// </summary>
        /// <param name="dbConnections">Database Acces to the Data and System Tables</param>
        /// <param name="tableID">ID appended to the front of the table name to allow for multiple instances of the same table to exist in the UserDS</param>
        public F0116(DAL dbConnections, string tableID) : base(dbConnections, tableID)
        {
        }

        #endregion

        #region RetrieveAddressInfo

        /// <summary>
        /// Retrieves the Address information including the Phone Number and Name
        /// </summary>
        /// <param name="addressNumber">Address Number (ALAN8)</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool RetrieveAddressInfo(double addressNumber)
        {
            ResetAll();

            SQL = String.Format("SELECT {0}, COALESCE(ABALPH, '') AS ABALPH, COALESCE(WWMLNM, '') AS WWMLNM, COALESCE(WPAR1, '') AS WPAR1, COALESCE(WPPH1, '') AS WPPH1 FROM {1} ", ColumnsForSelect, GetFullTableName());
            SQL = String.Format("INNER JOIN {0} ON (ALAN8 = WWAN8) ", GetFullTableName("F0111"));
            SQL = String.Format("LEFT OUTER JOIN {0} ON (ALAN8 = WPAN8) AND (WPIDLN = 0) AND (WPPHTP = '') ", GetFullTableName("F0115"));
            SQL = String.Format("LEFT JOIN {0} ON (ALAN8 = ABAN8) ", GetFullTableName("F0101"));

            this.ALAN8 = addressNumber;

            return Fetch();
        }

        #endregion
    }
}