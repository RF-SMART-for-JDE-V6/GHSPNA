using System.Text;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    /// <summary>
    /// A derived class for JDE Table - F00095
    /// </summary>
    public class F00095 : ICS.Database.TableObjects.JDE.F00095
    {
        #region Constructors

        /// <summary>
        /// Initializes a new F00095
        /// </summary>
        /// <remarks>Used for Enumerator</remarks>
        public F00095() : base()
        {
        }

        /// <summary>
        /// Initializes a new F00095 for the provided Data Access Layer and Table ID
        /// </summary>
        /// <param name="dbConnections">Database Access to the Data and System Tables</param>
        /// <param name="tableID">ID appended to the front of the table name to allow for multiple instances of the same table to exist in the UserDS</param>
        public F00095(DAL dbConnections, string tableID) : base(dbConnections, tableID)
        {
        }

        #endregion

        #region Methods

        public virtual bool DoJDERecordLocksExist(string sTableName, string key)
        {
            StringBuilder genKey = new StringBuilder();

            genKey.Append(key);

            ResetAll();

            A1GENKEY = genKey.ToString();
            A1OBNM = sTableName;

            return Fetch();
        }

        #endregion
    }
}