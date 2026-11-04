using System;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    #region F460131

    /// <summary>
    /// Creates Class for Enterprise 1 Standard Pack Recommendation Rules Table - F460131
    /// </summary>
    public class F460131 : F460131_Base
    {
        #region Constructors

        /// <summary>
        /// Constructor for DataAcces to Table: F460131
        /// </summary>
        public F460131() : base()
        {
        }

        /// <summary>
        /// Constructor for DataAcces to Table: F460131
        /// </summary>
        /// <param name="DbConnections">Data Access to Data and System Tables used by Table Object Class</param>
        /// <param name="TableId">Id used internally in DataSet if more than one instance of this table is needed.</param>
        public F460131(DAL DbConnections, string TableId) : base(DbConnections, TableId)
        {
        }

        #endregion

        #region RetrieveStandardPack

        /// <summary>
        /// Attempts to retrieve the Standard Pack for the Item and Address
        /// </summary>
        /// <param name="shortItemNumber">Short Item Number (SPITM)</param>
        /// <param name="addressNumber">Address Number (SPAN8)</param>
        /// <param name="shipToAddressNumber">Ship-To Address Number (SPSHAN)</param>
        /// <returns>The Standard Pack (SPSTPK) or 0, if the fetch is unsuccessful</returns>
        public double RetrieveStandardPack(double shortItemNumber, double addressNumber, double shipToAddressNumber)
        {
            ResetAll();

            this.SPITM = shortItemNumber;
            this.SPAN8 = addressNumber;
            this.SPSHAN = shipToAddressNumber;

            return (Fetch() ? SPSTPK : 0);
        }

        public double RetrieveStandardPack(double shortItemNumber, double addressNumber)
        {
            ResetAll();

            this.SPITM = shortItemNumber;
            this.SPAN8 = addressNumber;

            return (Fetch() ? SPSTPK : 0);
        }

        public double RetrieveStandardPack(double shortItemNumber)
        {
            ResetAll();

            this.SPITM = shortItemNumber;

            return (Fetch() ? SPSTPK : 0);
        }

        #endregion
    }

    #endregion

    #region F460131_Collection

    /// <summary>
    /// Provides Collection for F460131
    /// </summary>
    public class F460131_Collection : DataObjectBase_Collection
    {
        #region Constructor

        /// <summary>
        /// Constructor for Collection Access for Table: F460131
        /// </summary>
        /// <param name="clsBase">Base Class F460131_Base Object Used in Collection</param>
        public F460131_Collection(F460131_Base clsBase) : base(clsBase)
        {
        }

        #endregion

        #region Properties

        #region Current

        /// <summary>
        /// Gets Current Row in DataSet.
        /// Used to Enumerate Through Collection of F460131.
        /// </summary>
        /// <value>The F460131 Object Used by IEnumerator</value>
        public override Object Current
        {
            get
            {
                F460131 temp = new F460131();

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