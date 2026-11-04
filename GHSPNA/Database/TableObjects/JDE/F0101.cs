using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    /// <summary>
    /// A derived class for Enterprise 1 Address Book Master Table - F0101
    /// </summary>
    public class F0101 : ICS.Database.TableObjects.JDE.F0101
    {
        #region Constructors

        /// <summary>
        /// Initializes a new F0101
        /// </summary>
        public F0101() : base()
        {
        }

        /// <summary>
        /// Initializes a new F0101 for the provided Data Access Layer and Table ID
        /// </summary>
        /// <param name="dbConnections">Database Acces to the Data and System Tables</param>
        /// <param name="tableID">ID appended to the front of the table name to allow for multiple instances of the same table to exist in the UserDS</param>
        public F0101(DAL dbConnections, string tableID) : base(dbConnections, tableID)
        {
        }

        #endregion

        #region FetchAddress

        /// <summary>
        /// Retrieves the Address Record for the Address Number
        /// </summary>
        /// <param name="addressNumber">Address Number (ABAN8)</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool FetchAddress(double addressNumber)
        {
            ResetAll();
            this.ABAN8 = addressNumber;
            return Fetch();
        }

        #endregion
    }
}