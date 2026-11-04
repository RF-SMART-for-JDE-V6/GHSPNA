using System;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    #region F40R11

    /// <summary>
    /// Creates Class for Enterprise 1 Demand Detail Table - F40R11
    /// </summary>
    public class F40R11 : F40R11_Base
    {
        #region F40R11 Constructors

        /// <summary>
        /// Constructor for DataAcces to Table: F40R11
        /// </summary>
        public F40R11() : base()
        {
        }

        /// <summary>
        /// Constructor for DataAcces to Table: F40R11
        /// </summary>
        /// <param name="DbConnections">Data Access to Data and System Tables used by Table Object Class</param>
        /// <param name="TableId">Id used internally in DataSet if more than one instance of this table is needed.</param>
        public F40R11(DAL DbConnections, string TableId) : base(DbConnections, TableId)
        {
        }

        #endregion

        #region RetrieveDemand

        /// <summary>
        /// Retrieves the Demand record for the Unique ID
        /// </summary>
        /// <param name="demandUKID">Demand Unique ID (DDDEID)</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool RetrieveDemand(double demandUKID)
        {
            ResetAll();
            this.DDDEID = demandUKID;
            return Fetch();
        }

        #endregion
    }

    #endregion

    #region F40R11_Collection

    /// <summary>
    /// Provides Collection for F40R11
    /// </summary>
    public class F40R11_Collection : DataObjectBase_Collection
    {
        #region Constructor

        /// <summary>
        /// Constructor for Collection Access for Table: F40R11
        /// </summary>
        /// <param name="clsBase">Base Class F40R11_Base Object Used in Collection</param>
        public F40R11_Collection(F40R11_Base clsBase) : base(clsBase)
        {
        }

        #endregion

        #region Properties

        #region Current

        /// <summary>
        /// Gets Current Row in DataSet.
        /// Used to Enumerate Through Collection of F40R11.
        /// </summary>
        /// <value>The F40R11 Object Used by IEnumerator</value>
        public override Object Current
        {
            get
            {
                F40R11 temp = new F40R11();

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