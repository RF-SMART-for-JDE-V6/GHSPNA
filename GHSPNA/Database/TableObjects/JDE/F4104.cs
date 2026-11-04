using System;
using System.Collections.Generic;
using ICS.Database;
using ICS.Database.TableObjects.RFS;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    /// <summary>
    /// A derived class for Enterprise 1 Customer Item Number Table - F4104
    /// </summary>
    public class F4104 : ICS.Database.TableObjects.JDE.F4104
    {
        #region Class Level Variables

        private string[] _XrefTypes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new F4104
        /// </summary>
        public F4104() : base()
        {
        }

        /// <summary>
        /// Initializes a new F4104 for the provided Data Access Layer and Table ID
        /// </summary>
        /// <param name="dbConnections">Database Acces to the Data and System Tables</param>
        /// <param name="tableID">ID appended to the front of the table name to allow for multiple instances of the same table to exist in the UserDS</param>
        public F4104(DAL dbConnections, string tableID) : base(dbConnections, tableID)
        {
        }

        #endregion

        #region Properties

        #region XrefTypes

        /// <summary>
        /// Get the collection of Xref Types to filter by
        /// </summary>
        protected IEnumerable<string> XrefTypes
        {
            get
            {
                if (_XrefTypes == null)
                {
                    try
                    {
                        _XrefTypes = RFS_Environment.GetEnvironmentSetting(DataAccess, DataAccess.CurrentEnvironment, "XrefTypes").Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    }
                    catch
                    {
                        //Should we fail to retrieve the value, default to an empty collection
                        _XrefTypes = new string [0];
                    }
                }

                return _XrefTypes;
            }
        }

        #endregion

        #endregion

        #region RetrieveCustomerItem

        /// <summary>
        /// Retrieves the Customer's Item record from the F4104.
        /// </summary>
        /// <param name="addressNumber">Address Number (IVAN8)</param>
        /// <param name="shortItemNumber">Short Item Number (IVITM)</param>
        /// <param name="xrefType">Cross Reference Type (IVXRT)</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool RetrieveCustomerItem(double addressNumber, double shortItemNumber, string xrefType)
        {
            return RetrieveCustomerItem(addressNumber, shortItemNumber, xrefType, -1);
        }

        /// <summary>
        /// Retrieves the Customer's Item record from the F4104, taking into account the provided Order Date as well to
        /// make sure the item is within the effective (IVEFTJ) and expiration date (IVEXDJ).
        /// </summary>
        /// <param name="addressNumber">Address Number (IVAN8)</param>
        /// <param name="shortItemNumber">Short Item Number (IVITM)</param>
        /// <param name="xrefType">Cross Reference Type (IVXRT)</param>
        /// <param name="julianOrderDate">Order Date in Julian format (IVEFTJ and IVEXDJ)</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool RetrieveCustomerItem(double addressNumber, double shortItemNumber, string xrefType, decimal julianOrderDate)
        {
            ResetAll();

            this.IVAN8 = addressNumber;
            this.IVITM = shortItemNumber;
            this.IVXRT = xrefType;

            if (julianOrderDate > -1)
            {
                AppendToWhere(String.Format("AND (IVEFTJ <= {0}) AND (IVEXDJ >= {0})", julianOrderDate));
            }

            return Fetch();
        }

        #endregion
    }
}