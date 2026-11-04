using System;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    #region F00092

    /// <summary>
    /// Creates Class for Enterprise 1 Supplemental Data Table - F00092
    /// </summary>
    public class F00092 : F00092_Base
    {
        #region Constructors

        /// <summary>
        /// Constructor for DataAcces to Table: F00092
        /// </summary>
        public F00092() : base()
        {
        }

        /// <summary>
        /// Constructor for DataAcces to Table: F00092
        /// </summary>
        /// <param name="DbConnections">Data Access to Data and System Tables used by Table Object Class</param>
        /// <param name="TableId">Id used internally in DataSet if more than one instance of this table is needed.</param>
        public F00092(DAL DbConnections, string TableId) : base(DbConnections, TableId)
        {
        }

        #endregion

        #region RetrieveAddressData

        /// <summary>
        /// Attempts to retrieve Address Data from the table
        /// </summary>
        /// <param name="databaseCode">Database Code (T3SDB)</param>
        /// <param name="addressNumber">Address Number (T3KY)</param>     
        /// <param name="shipToAddressNumber">Ship-To Address Number (T3AN8)</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool RetrieveAddressData(string databaseCode, double addressNumber, double shipToAddressNumber)
        {
            ResetAll();

            this.T3SDB = databaseCode;
            this.T3KY = addressNumber.ToString();
            this.T3AN8 = shipToAddressNumber;

            return Fetch();
        }

        #endregion

        #region RetrieveAddressCode

        /// <summary>
        /// Attempts to retrieve Address Codes from the table
        /// </summary>
        /// <param name="databaseCode">Database Code (T3SDB)</param>
        /// <param name="addressNumber">Address Number (T3UORG)</param>
        /// <param name="shipToAddressNumber">Ship-To Address Number (T3AN8)</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool RetrieveAddressCode(string databaseCode, double addressNumber, double shipToAddressNumber)
        {
            ResetAll();

            this.T3SDB = databaseCode;
            this.T3UORG = addressNumber;
            this.T3AN8 = shipToAddressNumber;

            return Fetch();
        }

        #endregion

        #region RetrieveGeneralData

        /// <summary>
        /// Retrieves 10 General Data fields from the F00092, joining with F40R11 and F40R10 using
        /// the provided Demand Unique ID 
        /// </summary>
        /// <param name="demandUKID">Demand Unique ID (DDDEID)</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool RetrieveGeneralData(double demandUKID)
        {
            ResetAll();

            for (int i = 11; i <= 20; i++)
            {
                SQL = String.Format("SELECT TOP 1 {0} FROM {1} ", ColumnsForSelect, GetFullTableName());
                SQL = String.Format("INNER JOIN {0} ON (DHSDID = T3SBN1) INNER JOIN {1} ON (DDUKID = DHUKID) ", GetFullTableName("F40R10"), GetFullTableName("F40R11"));
                SQL = String.Format("WHERE (T3SDB = 'DH') AND (T3TYDT = 'ED') AND (T3KY = '{0}Z') AND (DDDEID = {1}) ", i, demandUKID);

                if (i < 20) //if not the last union
                {
                    SQL = "UNION ALL\n";
                }
            }

            return Fetch();
        }

        #endregion
    }

    #endregion

    #region F00092_Collection

    /// <summary>
    /// Provides Collection for F00092
    /// </summary>
    public class F00092_Collection : DataObjectBase_Collection
    {
        #region Constructor

        /// <summary>
        /// Constructor for Collection Access for Table: F00092
        /// </summary>
        /// <param name="clsBase">Base Class F00092_Base Object Used in Collection</param>
        public F00092_Collection(F00092_Base clsBase) : base(clsBase)
        {
        }

        #endregion

        #region Properties

        #region Current

        /// <summary>
        /// Gets Current Row in DataSet.
        /// Used to Enumerate Through Collection of F00092.
        /// </summary>
        /// <value>The F00092 Object Used by IEnumerator</value>
        public override Object Current
        {
            get
            {
                F00092 temp = new F00092();

                if (index <= -1)
                {
                    throw new InvalidOperationException();
                }

                temp.SetColumns = thisObj.SetColumns;
                temp.CurrentRow = thisObj.Rows[index];
                return temp;
            }
        }

        #endregion

        #endregion
    }

    #endregion
}