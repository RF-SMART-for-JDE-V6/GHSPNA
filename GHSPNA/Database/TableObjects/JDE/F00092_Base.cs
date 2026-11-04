using System;
using System.Collections;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    #region F00092_Base

    /// <summary>
    /// Base Class for Table: F00092
    /// </summary>
    public class F00092_Base : DataObjectBase
    {
        #region Constructors

        /// <summary>
        /// Constructor for Table Object: F00092_Base
        /// Used for Enumerator
        /// </summary>
        public F00092_Base() : base()
        {
        }

        /// <summary>
        /// Constructor for Table Object: F00092_Base
        /// </summary>
        /// <param name="DbConnections">Data Access Layer Used by table class to Access Data and System tables</param>
        /// <param name="TableId">Table Id for storing multiple tables with same names.</param>
        public F00092_Base(DAL DbConnections, string TableId) : base(DbConnections, "F00092", TableId)
        {
            this._TableType = ((int) (TableType.Data));
            _Columns = new F00092Columns(_DataAccess.DataManipulator, this, "F00092");
            SetCurrentRow();
        }

        #endregion

        #region Columns

        /// <summary>
        /// Gets Columns to Perform Query on
        /// </summary>
        /// <value>The Columns Class F00092Columns used for querying and datamanipulation.</value>
        public F00092Columns Columns
        {
            get { return ((F00092Columns) (SetColumns)); }
            set { SetColumns = value; }
        }

        #endregion

        #region T3SDB

        /// <summary>
        /// Gets or Sets Value of T3SDB for Table Object
        /// </summary>
        /// <value>The Value of T3SDB Column from Table F00092</value>
        public virtual string T3SDB
        {
            get { return (Convert.ToString(Columns.T3SDB.FormatColumn(this["T3SDB"]))); }
            set { Where(Columns.T3SDB, Condition.EqualTo, value); }
        }

        #endregion

        #region T3TYDT

        /// <summary>
        /// Gets or Sets Value of T3TYDT for Table Object
        /// </summary>
        /// <value>The Value of T3TYDT Column from Table F00092</value>
        public virtual string T3TYDT
        {
            get { return (Convert.ToString(Columns.T3TYDT.FormatColumn(this["T3TYDT"]))); }
            set { Where(Columns.T3TYDT, Condition.EqualTo, value); }
        }

        #endregion

        #region T3SBA1

        /// <summary>
        /// Gets or Sets Value of T3SBA1 for Table Object
        /// </summary>
        /// <value>The Value of T3SBA1 Column from Table F00092</value>
        public virtual string T3SBA1
        {
            get { return (Convert.ToString(Columns.T3SBA1.FormatColumn(this["T3SBA1"]))); }
            set { Where(Columns.T3SBA1, Condition.EqualTo, value); }
        }

        #endregion

        #region T3KCO

        /// <summary>
        /// Gets or Sets Value of T3KCO for Table Object
        /// </summary>
        /// <value>The Value of T3KCO Column from Table F00092</value>
        public virtual string T3KCO
        {
            get { return (Convert.ToString(Columns.T3KCO.FormatColumn(this["T3KCO"]))); }
            set { Where(Columns.T3KCO, Condition.EqualTo, value); }
        }

        #endregion

        #region T3SBA2

        /// <summary>
        /// Gets or Sets Value of T3SBA2 for Table Object
        /// </summary>
        /// <value>The Value of T3SBA2 Column from Table F00092</value>
        public virtual string T3SBA2
        {
            get { return (Convert.ToString(Columns.T3SBA2.FormatColumn(this["T3SBA2"]))); }
            set { Where(Columns.T3SBA2, Condition.EqualTo, value); }
        }

        #endregion

        #region T3MCU

        /// <summary>
        /// Gets or Sets Value of T3MCU for Table Object
        /// </summary>
        /// <value>The Value of T3MCU Column from Table F00092</value>
        public virtual string T3MCU
        {
            get { return (Convert.ToString(Columns.T3MCU.FormatColumn(this["T3MCU"]))); }
            set { Where(Columns.T3MCU, Condition.EqualTo, value); }
        }

        #endregion

        #region T3SBN1

        /// <summary>
        /// Gets or Sets Value of T3SBN1 for Table Object
        /// </summary>
        /// <value>The Value of T3SBN1 Column from Table F00092</value>
        public virtual double T3SBN1
        {
            get { return double.Parse(Convert.ToString(Columns.T3SBN1.FormatColumn(this["T3SBN1"]))); }
            set { Where(Columns.T3SBN1, Condition.EqualTo, value); }
        }

        #endregion

        #region T3SBN2

        /// <summary>
        /// Gets or Sets Value of T3SBN2 for Table Object
        /// </summary>
        /// <value>The Value of T3SBN2 Column from Table F00092</value>
        public virtual double T3SBN2
        {
            get { return double.Parse(Convert.ToString(Columns.T3SBN2.FormatColumn(this["T3SBN2"]))); }
            set { Where(Columns.T3SBN2, Condition.EqualTo, value); }
        }

        #endregion

        #region T3UKID

        /// <summary>
        /// Gets or Sets Value of T3UKID for Table Object
        /// </summary>
        /// <value>The Value of T3UKID Column from Table F00092</value>
        public virtual double T3UKID
        {
            get { return double.Parse(Convert.ToString(Columns.T3UKID.FormatColumn(this["T3UKID"]))); }
            set { Where(Columns.T3UKID, Condition.EqualTo, value); }
        }

        #endregion

        #region T3UORG

        /// <summary>
        /// Gets or Sets Value of T3UORG for Table Object
        /// </summary>
        /// <value>The Value of T3UORG Column from Table F00092</value>
        public virtual double T3UORG
        {
            get { return double.Parse(Convert.ToString(Columns.T3UORG.FormatColumn(this["T3UORG"]))); }
            set { Where(Columns.T3UORG, Condition.EqualTo, value); }
        }

        #endregion

        #region T3EFT

        /// <summary>
        /// Gets or Sets Value of T3EFT for Table Object
        /// </summary>
        /// <value>The Value of T3EFT Column from Table F00092</value>
        public virtual decimal T3EFT
        {
            get { return decimal.Parse(Convert.ToString(Columns.T3EFT.FormatColumn(this["T3EFT"]))); }
            set { Where(Columns.T3EFT, Condition.EqualTo, value); }
        }

        #endregion

        #region T3RORN

        /// <summary>
        /// Gets or Sets Value of T3RORN for Table Object
        /// </summary>
        /// <value>The Value of T3RORN Column from Table F00092</value>
        public virtual string T3RORN
        {
            get { return (Convert.ToString(Columns.T3RORN.FormatColumn(this["T3RORN"]))); }
            set { Where(Columns.T3RORN, Condition.EqualTo, value); }
        }

        #endregion

        #region T3RCTO

        /// <summary>
        /// Gets or Sets Value of T3RCTO for Table Object
        /// </summary>
        /// <value>The Value of T3RCTO Column from Table F00092</value>
        public virtual string T3RCTO
        {
            get { return (Convert.ToString(Columns.T3RCTO.FormatColumn(this["T3RCTO"]))); }
            set { Where(Columns.T3RCTO, Condition.EqualTo, value); }
        }

        #endregion

        #region T3KY

        /// <summary>
        /// Gets or Sets Value of T3KY for Table Object
        /// </summary>
        /// <value>The Value of T3KY Column from Table F00092</value>
        public virtual string T3KY
        {
            get { return (Convert.ToString(Columns.T3KY.FormatColumn(this["T3KY"]))); }
            set { Where(Columns.T3KY, Condition.EqualTo, value); }
        }

        #endregion

        #region T3EFTE

        /// <summary>
        /// Gets or Sets Value of T3EFTE for Table Object
        /// </summary>
        /// <value>The Value of T3EFTE Column from Table F00092</value>
        public virtual decimal T3EFTE
        {
            get { return decimal.Parse(Convert.ToString(Columns.T3EFTE.FormatColumn(this["T3EFTE"]))); }
            set { Where(Columns.T3EFTE, Condition.EqualTo, value); }
        }

        #endregion

        #region T3AMTU

        /// <summary>
        /// Gets or Sets Value of T3AMTU for Table Object
        /// </summary>
        /// <value>The Value of T3AMTU Column from Table F00092</value>
        public virtual double T3AMTU
        {
            get { return double.Parse(Convert.ToString(Columns.T3AMTU.FormatColumn(this["T3AMTU"]))); }
            set { Where(Columns.T3AMTU, Condition.EqualTo, value); }
        }

        #endregion

        #region T3AMTV

        /// <summary>
        /// Gets or Sets Value of T3AMTV for Table Object
        /// </summary>
        /// <value>The Value of T3AMTV Column from Table F00092</value>
        public virtual double T3AMTV
        {
            get { return double.Parse(Convert.ToString(Columns.T3AMTV.FormatColumn(this["T3AMTV"]))); }
            set { Where(Columns.T3AMTV, Condition.EqualTo, value); }
        }

        #endregion

        #region T3RMK

        /// <summary>
        /// Gets or Sets Value of T3RMK for Table Object
        /// </summary>
        /// <value>The Value of T3RMK Column from Table F00092</value>
        public virtual string T3RMK
        {
            get { return (Convert.ToString(Columns.T3RMK.FormatColumn(this["T3RMK"]))); }
            set { Where(Columns.T3RMK, Condition.EqualTo, value); }
        }

        #endregion

        #region T3RMK2

        /// <summary>
        /// Gets or Sets Value of T3RMK2 for Table Object
        /// </summary>
        /// <value>The Value of T3RMK2 Column from Table F00092</value>
        public virtual string T3RMK2
        {
            get { return (Convert.ToString(Columns.T3RMK2.FormatColumn(this["T3RMK2"]))); }
            set { Where(Columns.T3RMK2, Condition.EqualTo, value); }
        }

        #endregion

        #region T3UPDJ

        /// <summary>
        /// Gets or Sets Value of T3UPDJ for Table Object
        /// </summary>
        /// <value>The Value of T3UPDJ Column from Table F00092</value>
        public virtual decimal T3UPDJ
        {
            get { return decimal.Parse(Convert.ToString(Columns.T3UPDJ.FormatColumn(this["T3UPDJ"]))); }
            set { Where(Columns.T3UPDJ, Condition.EqualTo, value); }
        }

        #endregion

        #region T3DYUD

        /// <summary>
        /// Gets or Sets Value of T3DYUD for Table Object
        /// </summary>
        /// <value>The Value of T3DYUD Column from Table F00092</value>
        public virtual double T3DYUD
        {
            get { return double.Parse(Convert.ToString(Columns.T3DYUD.FormatColumn(this["T3DYUD"]))); }
            set { Where(Columns.T3DYUD, Condition.EqualTo, value); }
        }

        #endregion

        #region T3AN8

        /// <summary>
        /// Gets or Sets Value of T3AN8 for Table Object
        /// </summary>
        /// <value>The Value of T3AN8 Column from Table F00092</value>
        public virtual double T3AN8
        {
            get { return double.Parse(Convert.ToString(Columns.T3AN8.FormatColumn(this["T3AN8"]))); }
            set { Where(Columns.T3AN8, Condition.EqualTo, value); }
        }

        #endregion

        #region T3KYWW

        /// <summary>
        /// Gets or Sets Value of T3KYWW for Table Object
        /// </summary>
        /// <value>The Value of T3KYWW Column from Table F00092</value>
        public virtual string T3KYWW
        {
            get { return (Convert.ToString(Columns.T3KYWW.FormatColumn(this["T3KYWW"]))); }
            set { Where(Columns.T3KYWW, Condition.EqualTo, value); }
        }

        #endregion

        #region T3USER

        /// <summary>
        /// Gets or Sets Value of T3USER for Table Object
        /// </summary>
        /// <value>The Value of T3USER Column from Table F00092</value>
        public virtual string T3USER
        {
            get { return (Convert.ToString(Columns.T3USER.FormatColumn(this["T3USER"]))); }
            set { Where(Columns.T3USER, Condition.EqualTo, value); }
        }

        #endregion

        #region T3PID

        /// <summary>
        /// Gets or Sets Value of T3PID for Table Object
        /// </summary>
        /// <value>The Value of T3PID Column from Table F00092</value>
        public virtual string T3PID
        {
            get { return (Convert.ToString(Columns.T3PID.FormatColumn(this["T3PID"]))); }
            set { Where(Columns.T3PID, Condition.EqualTo, value); }
        }

        #endregion

        #region T3JOBN

        /// <summary>
        /// Gets or Sets Value of T3JOBN for Table Object
        /// </summary>
        /// <value>The Value of T3JOBN Column from Table F00092</value>
        public virtual string T3JOBN
        {
            get { return (Convert.ToString(Columns.T3JOBN.FormatColumn(this["T3JOBN"]))); }
            set { Where(Columns.T3JOBN, Condition.EqualTo, value); }
        }

        #endregion

        #region T3UPMJ

        /// <summary>
        /// Gets or Sets Value of T3UPMJ for Table Object
        /// </summary>
        /// <value>The Value of T3UPMJ Column from Table F00092</value>
        public virtual decimal T3UPMJ
        {
            get { return decimal.Parse(Convert.ToString(Columns.T3UPMJ.FormatColumn(this["T3UPMJ"]))); }
            set { Where(Columns.T3UPMJ, Condition.EqualTo, value); }
        }

        #endregion

        #region T3TDAY

        /// <summary>
        /// Gets or Sets Value of T3TDAY for Table Object
        /// </summary>
        /// <value>The Value of T3TDAY Column from Table F00092</value>
        public virtual double T3TDAY
        {
            get { return double.Parse(Convert.ToString(Columns.T3TDAY.FormatColumn(this["T3TDAY"]))); }
            set { Where(Columns.T3TDAY, Condition.EqualTo, value); }
        }

        #endregion

        #region T3RMK3

        /// <summary>
        /// Gets or Sets Value of T3RMK3 for Table Object
        /// </summary>
        /// <value>The Value of T3RMK3 Column from Table F00092</value>
        public virtual string T3RMK3
        {
            get { return (Convert.ToString(Columns.T3RMK3.FormatColumn(this["T3RMK3"]))); }
            set { Where(Columns.T3RMK3, Condition.EqualTo, value); }
        }

        #endregion

        #region T3UPTM

        /// <summary>
        /// Gets or Sets Value of T3UPTM for Table Object
        /// </summary>
        /// <value>The Value of T3UPTM Column from Table F00092</value>
        public virtual double T3UPTM
        {
            get { return double.Parse(Convert.ToString(Columns.T3UPTM.FormatColumn(this["T3UPTM"]))); }
            set { Where(Columns.T3UPTM, Condition.EqualTo, value); }
        }

        #endregion

        #region GetEnumerator

        /// <summary>
        /// Returns the IEnumerator Object for Fetching Multiple Records from F00092
        /// </summary>
        /// <returns>The enumerator object for F00092_Collection</returns>
        public override IEnumerator GetEnumerator()
        {
            return new F00092_Collection(this);
        }

        #endregion
    }

    #endregion

    #region F00092Columns

    /// <summary>
    /// Base Class for Column Class for Query Construction
    /// </summary>
    public class F00092Columns : ITableColumns
    {
        /// <summary>
        /// Query object for column T3SDB in Table.
        /// </summary>
        public Query T3SDB;

        /// <summary>
        /// Query object for column T3TYDT in Table.
        /// </summary>
        public Query T3TYDT;

        /// <summary>
        /// Query object for column T3SBA1 in Table.
        /// </summary>
        public Query T3SBA1;

        /// <summary>
        /// Query object for column T3KCO in Table.
        /// </summary>
        public Query T3KCO;

        /// <summary>
        /// Query object for column T3SBA2 in Table.
        /// </summary>
        public Query T3SBA2;

        /// <summary>
        /// Query object for column T3MCU in Table.
        /// </summary>
        public Query T3MCU;

        /// <summary>
        /// Query object for column T3SBN1 in Table.
        /// </summary>
        public Query T3SBN1;

        /// <summary>
        /// Query object for column T3SBN2 in Table.
        /// </summary>
        public Query T3SBN2;

        /// <summary>
        /// Query object for column T3UKID in Table.
        /// </summary>
        public Query T3UKID;

        /// <summary>
        /// Query object for column T3UORG in Table.
        /// </summary>
        public Query T3UORG;

        /// <summary>
        /// Query object for column T3EFT in Table.
        /// </summary>
        public Query T3EFT;

        /// <summary>
        /// Query object for column T3RORN in Table.
        /// </summary>
        public Query T3RORN;

        /// <summary>
        /// Query object for column T3RCTO in Table.
        /// </summary>
        public Query T3RCTO;

        /// <summary>
        /// Query object for column T3KY in Table.
        /// </summary>
        public Query T3KY;

        /// <summary>
        /// Query object for column T3EFTE in Table.
        /// </summary>
        public Query T3EFTE;

        /// <summary>
        /// Query object for column T3AMTU in Table.
        /// </summary>
        public Query T3AMTU;

        /// <summary>
        /// Query object for column T3AMTV in Table.
        /// </summary>
        public Query T3AMTV;

        /// <summary>
        /// Query object for column T3RMK in Table.
        /// </summary>
        public Query T3RMK;

        /// <summary>
        /// Query object for column T3RMK2 in Table.
        /// </summary>
        public Query T3RMK2;

        /// <summary>
        /// Query object for column T3UPDJ in Table.
        /// </summary>
        public Query T3UPDJ;

        /// <summary>
        /// Query object for column T3DYUD in Table.
        /// </summary>
        public Query T3DYUD;

        /// <summary>
        /// Query object for column T3AN8 in Table.
        /// </summary>
        public Query T3AN8;

        /// <summary>
        /// Query object for column T3KYWW in Table.
        /// </summary>
        public Query T3KYWW;

        /// <summary>
        /// Query object for column T3USER in Table.
        /// </summary>
        public Query T3USER;

        /// <summary>
        /// Query object for column T3PID in Table.
        /// </summary>
        public Query T3PID;

        /// <summary>
        /// Query object for column T3JOBN in Table.
        /// </summary>
        public Query T3JOBN;

        /// <summary>
        /// Query object for column T3UPMJ in Table.
        /// </summary>
        public Query T3UPMJ;

        /// <summary>
        /// Query object for column T3TDAY in Table.
        /// </summary>
        public Query T3TDAY;

        /// <summary>
        /// Query object for column T3RMK3 in Table.
        /// </summary>
        public Query T3RMK3;

        /// <summary>
        /// Query object for column T3UPTM in Table.
        /// </summary>
        public Query T3UPTM;

        #region .ctor Constructor

        /// <summary>
        /// Creates Query Objects for all Columns for F00092
        /// </summary>
        /// <param name="DataManipObject">Data Manipulator for columns.</param>
        /// <param name="ColumnsTable">DataTable columns will be added to.</param>
        /// <param name="TableName">Name of table.</param>
        public F00092Columns(DataManipulator DataManipObject, DataObjectBase ColumnsTable, string TableName)
        {
            this.T3SDB = new Query("T3SDB", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3TYDT = new Query("T3TYDT", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3SBA1 = new Query("T3SBA1", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3KCO = new Query("T3KCO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3SBA2 = new Query("T3SBA2", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3MCU = new Query("T3MCU", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3SBN1 = new Query("T3SBN1", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.T3SBN2 = new Query("T3SBN2", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.T3UKID = new Query("T3UKID", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.T3UORG = new Query("T3UORG", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.T3EFT = new Query("T3EFT", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.T3RORN = new Query("T3RORN", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3RCTO = new Query("T3RCTO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3KY = new Query("T3KY", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3EFTE = new Query("T3EFTE", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.T3AMTU = new Query("T3AMTU", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.T3AMTV = new Query("T3AMTV", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.T3RMK = new Query("T3RMK", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3RMK2 = new Query("T3RMK2", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3UPDJ = new Query("T3UPDJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.T3DYUD = new Query("T3DYUD", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.T3AN8 = new Query("T3AN8", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.T3KYWW = new Query("T3KYWW", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3USER = new Query("T3USER", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3PID = new Query("T3PID", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3JOBN = new Query("T3JOBN", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3UPMJ = new Query("T3UPMJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.T3TDAY = new Query("T3TDAY", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.T3RMK3 = new Query("T3RMK3", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.T3UPTM = new Query("T3UPTM", TableName, typeof(double), DataManipObject, ColumnsTable);
        }

        #endregion

        #region Clear

        /// <summary>
        /// Resets Query Conditions for All Columns
        /// </summary>
        public override void Clear()
        {
            T3SDB.Reset();
            T3TYDT.Reset();
            T3SBA1.Reset();
            T3KCO.Reset();
            T3SBA2.Reset();
            T3MCU.Reset();
            T3SBN1.Reset();
            T3SBN2.Reset();
            T3UKID.Reset();
            T3UORG.Reset();
            T3EFT.Reset();
            T3RORN.Reset();
            T3RCTO.Reset();
            T3KY.Reset();
            T3EFTE.Reset();
            T3AMTU.Reset();
            T3AMTV.Reset();
            T3RMK.Reset();
            T3RMK2.Reset();
            T3UPDJ.Reset();
            T3DYUD.Reset();
            T3AN8.Reset();
            T3KYWW.Reset();
            T3USER.Reset();
            T3PID.Reset();
            T3JOBN.Reset();
            T3UPMJ.Reset();
            T3TDAY.Reset();
            T3RMK3.Reset();
            T3UPTM.Reset();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes of Query Object Conditions for All Columns
        /// </summary>
        public override void Dispose()
        {
            T3SDB.Dispose();
            T3TYDT.Dispose();
            T3SBA1.Dispose();
            T3KCO.Dispose();
            T3SBA2.Dispose();
            T3MCU.Dispose();
            T3SBN1.Dispose();
            T3SBN2.Dispose();
            T3UKID.Dispose();
            T3UORG.Dispose();
            T3EFT.Dispose();
            T3RORN.Dispose();
            T3RCTO.Dispose();
            T3KY.Dispose();
            T3EFTE.Dispose();
            T3AMTU.Dispose();
            T3AMTV.Dispose();
            T3RMK.Dispose();
            T3RMK2.Dispose();
            T3UPDJ.Dispose();
            T3DYUD.Dispose();
            T3AN8.Dispose();
            T3KYWW.Dispose();
            T3USER.Dispose();
            T3PID.Dispose();
            T3JOBN.Dispose();
            T3UPMJ.Dispose();
            T3TDAY.Dispose();
            T3RMK3.Dispose();
            T3UPTM.Dispose();
        }

        #endregion

        #region AddColumnsToTable

        /// <summary>
        /// Adds All Columns to Table
        /// </summary>
        public virtual void AddColumnsToTable()
        {
            this.T3SDB.AddToTable();
            this.T3TYDT.AddToTable();
            this.T3SBA1.AddToTable();
            this.T3KCO.AddToTable();
            this.T3SBA2.AddToTable();
            this.T3MCU.AddToTable();
            this.T3SBN1.AddToTable();
            this.T3SBN2.AddToTable();
            this.T3UKID.AddToTable();
            this.T3UORG.AddToTable();
            this.T3EFT.AddToTable();
            this.T3RORN.AddToTable();
            this.T3RCTO.AddToTable();
            this.T3KY.AddToTable();
            this.T3EFTE.AddToTable();
            this.T3AMTU.AddToTable();
            this.T3AMTV.AddToTable();
            this.T3RMK.AddToTable();
            this.T3RMK2.AddToTable();
            this.T3UPDJ.AddToTable();
            this.T3DYUD.AddToTable();
            this.T3AN8.AddToTable();
            this.T3KYWW.AddToTable();
            this.T3USER.AddToTable();
            this.T3PID.AddToTable();
            this.T3JOBN.AddToTable();
            this.T3UPMJ.AddToTable();
            this.T3TDAY.AddToTable();
            this.T3RMK3.AddToTable();
            this.T3UPTM.AddToTable();
        }

        #endregion
    }

    #endregion
}