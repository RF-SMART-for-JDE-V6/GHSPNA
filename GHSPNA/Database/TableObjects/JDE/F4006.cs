using System;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    #region F4006

    /// <summary>
    /// Creates Class for F4006
    /// </summary>
    public class F4006 : F4006_Base
    {
        #region Constructors

        /// <summary>
        /// Constructor for DataAcces to Table: F4006
        /// </summary>
        public F4006() : base()
        {
        }

        /// <summary>
        /// Constructor for DataAcces to Table: F4006
        /// </summary>
        /// <param name="DbConnections">Data Access to Data and System Tables used by Table Object Class</param>
        /// <param name="TableId">Id used internally in DataSet if more than one instance of this table is needed.</param>
        public F4006(DAL DbConnections, string TableId) : base(DbConnections, TableId)
        {
        }

        #endregion

        #region RetrieveByOrderNumber

        /// <summary>
        /// Queries the F4006 for the records based on the Order Number, Type, and Company provided (where
        /// the OAANTY = 2)
        /// </summary>
        /// <param name="OrderNumber">Order Number (OADOCO)</param>
        /// <param name="OrderType">Order Type (OADCTO)</param>
        /// <param name="OrderCompany">Order Company (OAKCOO)</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool RetrieveByOrderNumber(double OrderNumber, string OrderType, string OrderCompany)
        {
            ResetAll();

            this.OADOCO = OrderNumber;
            this.OADCTO = OrderType;
            this.OAKCOO = OrderCompany;
            this.OAANTY = "2";

            return (Fetch());
        }

        #endregion
    }

    #endregion

    #region F4006_Collection

    /// <summary>
    /// Provides Collection for F4006
    /// </summary>
    public class F4006_Collection : DataObjectBase_Collection
    {
        #region Constructor

        /// <summary>
        /// Constructor for Collection Access for Table: F4006
        /// </summary>
        /// <param name="clsBase">Base Class F4006_Base Object Used in Collection</param>
        public F4006_Collection(F4006_Base clsBase) : base(clsBase)
        {
        }

        #endregion

        #region Properties

        #region Current

        /// <summary>
        /// Gets Current Row in DataSet.
        /// Used to Enumerate Through Collection of F4006.
        /// </summary>
        /// <value>The F4006 Object Used by IEnumerator</value>
        public override Object Current
        {
            get
            {
                F4006 temp = new F4006();

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