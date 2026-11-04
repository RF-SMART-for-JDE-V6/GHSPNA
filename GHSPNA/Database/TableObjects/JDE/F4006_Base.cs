using System;
using System.Collections;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    #region F4006_Base

    /// <summary>
    /// Base Class for Table: F4006
    /// </summary>
    public class F4006_Base : DataObjectBase
    {
        #region Constructors

        /// <summary>
        /// Constructor for Table Object: F4006_Base
        /// Used for Enumerator
        /// </summary>
        public F4006_Base() : base()
        {
        }

        /// <summary>
        /// Constructor for Table Object: F4006_Base
        /// </summary>
        /// <param name="DbConnections">Data Access Layer Used by table class to Access Data and System tables</param>
        /// <param name="TableId">Table Id for storing multiple tables with same names.</param>
        public F4006_Base(DAL DbConnections, string TableId) : base(DbConnections, "F4006", TableId)
        {
            this._TableType = ((int) (TableType.Data));
            _Columns = new F4006Columns(_DataAccess.DataManipulator, this, "F4006");
            SetCurrentRow();
        }

        #endregion

        #region Columns

        /// <summary>
        /// Gets Columns to Perform Query on
        /// </summary>
        /// <value>The Columns Class F4006Columns used for querying and datamanipulation.</value>
        public F4006Columns Columns
        {
            get { return ((F4006Columns) (SetColumns)); }
            set { SetColumns = value; }
        }

        #endregion

        #region OADOCO

        /// <summary>
        /// Gets or Sets Value of OADOCO for Table Object
        /// </summary>
        /// <value>The Value of OADOCO Column from Table F4006</value>
        public virtual double OADOCO
        {
            get { return double.Parse(Columns.OADOCO.FormatColumn(this["OADOCO"]).ToString()); }
            set { Where(Columns.OADOCO, Condition.EqualTo, value); }
        }

        #endregion

        #region OADCTO

        /// <summary>
        /// Gets or Sets Value of OADCTO for Table Object
        /// </summary>
        /// <value>The Value of OADCTO Column from Table F4006</value>
        public virtual string OADCTO
        {
            get { return Convert.ToString(Columns.OADCTO.FormatColumn(this["OADCTO"])); }
            set { Where(Columns.OADCTO, Condition.EqualTo, value); }
        }

        #endregion

        #region OAKCOO

        /// <summary>
        /// Gets or Sets Value of OAKCOO for Table Object
        /// </summary>
        /// <value>The Value of OAKCOO Column from Table F4006</value>
        public virtual string OAKCOO
        {
            get { return Convert.ToString(Columns.OAKCOO.FormatColumn(this["OAKCOO"])); }
            set { Where(Columns.OAKCOO, Condition.EqualTo, value); }
        }

        #endregion

        #region OAANTY

        /// <summary>
        /// Gets or Sets Value of OAANTY for Table Object
        /// </summary>
        /// <value>The Value of OAANTY Column from Table F4006</value>
        public virtual string OAANTY
        {
            get { return Convert.ToString(Columns.OAANTY.FormatColumn(this["OAANTY"])); }
            set { Where(Columns.OAANTY, Condition.EqualTo, value); }
        }

        #endregion

        #region OAMLNM

        /// <summary>
        /// Gets or Sets Value of OAMLNM for Table Object
        /// </summary>
        /// <value>The Value of OAMLNM Column from Table F4006</value>
        public virtual string OAMLNM
        {
            get { return Convert.ToString(Columns.OAMLNM.FormatColumn(this["OAMLNM"])); }
            set { Where(Columns.OAMLNM, Condition.EqualTo, value); }
        }

        #endregion

        #region OAADD1

        /// <summary>
        /// Gets or Sets Value of OAADD1 for Table Object
        /// </summary>
        /// <value>The Value of OAADD1 Column from Table F4006</value>
        public virtual string OAADD1
        {
            get { return Convert.ToString(Columns.OAADD1.FormatColumn(this["OAADD1"])); }
            set { Where(Columns.OAADD1, Condition.EqualTo, value); }
        }

        #endregion

        #region OAADD2

        /// <summary>
        /// Gets or Sets Value of OAADD2 for Table Object
        /// </summary>
        /// <value>The Value of OAADD2 Column from Table F4006</value>
        public virtual string OAADD2
        {
            get { return Convert.ToString(Columns.OAADD2.FormatColumn(this["OAADD2"])); }
            set { Where(Columns.OAADD2, Condition.EqualTo, value); }
        }

        #endregion

        #region OAADD3

        /// <summary>
        /// Gets or Sets Value of OAADD3 for Table Object
        /// </summary>
        /// <value>The Value of OAADD3 Column from Table F4006</value>
        public virtual string OAADD3
        {
            get { return Convert.ToString(Columns.OAADD3.FormatColumn(this["OAADD3"])); }
            set { Where(Columns.OAADD3, Condition.EqualTo, value); }
        }

        #endregion

        #region OAADD4

        /// <summary>
        /// Gets or Sets Value of OAADD4 for Table Object
        /// </summary>
        /// <value>The Value of OAADD4 Column from Table F4006</value>
        public virtual string OAADD4
        {
            get { return Convert.ToString(Columns.OAADD4.FormatColumn(this["OAADD4"])); }
            set { Where(Columns.OAADD4, Condition.EqualTo, value); }
        }

        #endregion

        #region OAADDZ

        /// <summary>
        /// Gets or Sets Value of OAADDZ for Table Object
        /// </summary>
        /// <value>The Value of OAADDZ Column from Table F4006</value>
        public virtual string OAADDZ
        {
            get { return Convert.ToString(Columns.OAADDZ.FormatColumn(this["OAADDZ"])); }
            set { Where(Columns.OAADDZ, Condition.EqualTo, value); }
        }

        #endregion

        #region OACTY1

        /// <summary>
        /// Gets or Sets Value of OACTY1 for Table Object
        /// </summary>
        /// <value>The Value of OACTY1 Column from Table F4006</value>
        public virtual string OACTY1
        {
            get { return Convert.ToString(Columns.OACTY1.FormatColumn(this["OACTY1"])); }
            set { Where(Columns.OACTY1, Condition.EqualTo, value); }
        }

        #endregion

        #region OACOUN

        /// <summary>
        /// Gets or Sets Value of OACOUN for Table Object
        /// </summary>
        /// <value>The Value of OACOUN Column from Table F4006</value>
        public virtual string OACOUN
        {
            get { return Convert.ToString(Columns.OACOUN.FormatColumn(this["OACOUN"])); }
            set { Where(Columns.OACOUN, Condition.EqualTo, value); }
        }

        #endregion

        #region OAADDS

        /// <summary>
        /// Gets or Sets Value of OAADDS for Table Object
        /// </summary>
        /// <value>The Value of OAADDS Column from Table F4006</value>
        public virtual string OAADDS
        {
            get { return Convert.ToString(Columns.OAADDS.FormatColumn(this["OAADDS"])); }
            set { Where(Columns.OAADDS, Condition.EqualTo, value); }
        }

        #endregion

        #region OACRTE

        /// <summary>
        /// Gets or Sets Value of OACRTE for Table Object
        /// </summary>
        /// <value>The Value of OACRTE Column from Table F4006</value>
        public virtual string OACRTE
        {
            get { return Convert.ToString(Columns.OACRTE.FormatColumn(this["OACRTE"])); }
            set { Where(Columns.OACRTE, Condition.EqualTo, value); }
        }

        #endregion

        #region OABKML

        /// <summary>
        /// Gets or Sets Value of OABKML for Table Object
        /// </summary>
        /// <value>The Value of OABKML Column from Table F4006</value>
        public virtual string OABKML
        {
            get { return Convert.ToString(Columns.OABKML.FormatColumn(this["OABKML"])); }
            set { Where(Columns.OABKML, Condition.EqualTo, value); }
        }

        #endregion

        #region OACTR

        /// <summary>
        /// Gets or Sets Value of OACTR for Table Object
        /// </summary>
        /// <value>The Value of OACTR Column from Table F4006</value>
        public virtual string OACTR
        {
            get { return Convert.ToString(Columns.OACTR.FormatColumn(this["OACTR"])); }
            set { Where(Columns.OACTR, Condition.EqualTo, value); }
        }

        #endregion

        #region OAUSER

        /// <summary>
        /// Gets or Sets Value of OAUSER for Table Object
        /// </summary>
        /// <value>The Value of OAUSER Column from Table F4006</value>
        public virtual string OAUSER
        {
            get { return Convert.ToString(Columns.OAUSER.FormatColumn(this["OAUSER"])); }
            set { Where(Columns.OAUSER, Condition.EqualTo, value); }
        }

        #endregion

        #region OAPID

        /// <summary>
        /// Gets or Sets Value of OAPID for Table Object
        /// </summary>
        /// <value>The Value of OAPID Column from Table F4006</value>
        public virtual string OAPID
        {
            get { return Convert.ToString(Columns.OAPID.FormatColumn(this["OAPID"])); }
            set { Where(Columns.OAPID, Condition.EqualTo, value); }
        }

        #endregion

        #region OAUPMJ

        /// <summary>
        /// Gets or Sets Value of OAUPMJ for Table Object
        /// </summary>
        /// <value>The Value of OAUPMJ Column from Table F4006</value>
        public virtual decimal OAUPMJ
        {
            get { return decimal.Parse(Columns.OAUPMJ.FormatColumn(this["OAUPMJ"]).ToString()); }
            set { Where(Columns.OAUPMJ, Condition.EqualTo, value); }
        }

        #endregion

        #region OAJOBN

        /// <summary>
        /// Gets or Sets Value of OAJOBN for Table Object
        /// </summary>
        /// <value>The Value of OAJOBN Column from Table F4006</value>
        public virtual string OAJOBN
        {
            get { return Convert.ToString(Columns.OAJOBN.FormatColumn(this["OAJOBN"])); }
            set { Where(Columns.OAJOBN, Condition.EqualTo, value); }
        }

        #endregion

        #region OAUPMT

        /// <summary>
        /// Gets or Sets Value of OAUPMT for Table Object
        /// </summary>
        /// <value>The Value of OAUPMT Column from Table F4006</value>
        public virtual double OAUPMT
        {
            get { return double.Parse(Columns.OAUPMT.FormatColumn(this["OAUPMT"]).ToString()); }
            set { Where(Columns.OAUPMT, Condition.EqualTo, value); }
        }

        #endregion

        #region OALNID

        /// <summary>
        /// Gets or Sets Value of OALNID for Table Object
        /// </summary>
        /// <value>The Value of OALNID Column from Table F4006</value>
        public virtual double OALNID
        {
            get { return double.Parse(Columns.OALNID.FormatColumn(this["OALNID"]).ToString()); }
            set { Where(Columns.OALNID, Condition.EqualTo, value); }
        }

        #endregion

        /// <summary>
        /// Returns the IEnumerator Object for Fetching Multiple Records from F4006
        /// </summary>
        /// <returns>The enumerator object for F4006_Collection</returns>
        public override IEnumerator GetEnumerator()
        {
            return new F4006_Collection(this);
        }
    }

    #endregion

    #region F4006Columns

    /// <summary>
    /// Base Class for Column Class for Query Construction
    /// </summary>
    public class F4006Columns : ITableColumns
    {
        /// <summary>
        /// Query object for column OADOCO in Table.
        /// </summary>
        public Query OADOCO;

        /// <summary>
        /// Query object for column OADCTO in Table.
        /// </summary>
        public Query OADCTO;

        /// <summary>
        /// Query object for column OAKCOO in Table.
        /// </summary>
        public Query OAKCOO;

        /// <summary>
        /// Query object for column OAANTY in Table.
        /// </summary>
        public Query OAANTY;

        /// <summary>
        /// Query object for column OAMLNM in Table.
        /// </summary>
        public Query OAMLNM;

        /// <summary>
        /// Query object for column OAADD1 in Table.
        /// </summary>
        public Query OAADD1;

        /// <summary>
        /// Query object for column OAADD2 in Table.
        /// </summary>
        public Query OAADD2;

        /// <summary>
        /// Query object for column OAADD3 in Table.
        /// </summary>
        public Query OAADD3;

        /// <summary>
        /// Query object for column OAADD4 in Table.
        /// </summary>
        public Query OAADD4;

        /// <summary>
        /// Query object for column OAADDZ in Table.
        /// </summary>
        public Query OAADDZ;

        /// <summary>
        /// Query object for column OACTY1 in Table.
        /// </summary>
        public Query OACTY1;

        /// <summary>
        /// Query object for column OACOUN in Table.
        /// </summary>
        public Query OACOUN;

        /// <summary>
        /// Query object for column OAADDS in Table.
        /// </summary>
        public Query OAADDS;

        /// <summary>
        /// Query object for column OACRTE in Table.
        /// </summary>
        public Query OACRTE;

        /// <summary>
        /// Query object for column OABKML in Table.
        /// </summary>
        public Query OABKML;

        /// <summary>
        /// Query object for column OACTR in Table.
        /// </summary>
        public Query OACTR;

        /// <summary>
        /// Query object for column OAUSER in Table.
        /// </summary>
        public Query OAUSER;

        /// <summary>
        /// Query object for column OAPID in Table.
        /// </summary>
        public Query OAPID;

        /// <summary>
        /// Query object for column OAUPMJ in Table.
        /// </summary>
        public Query OAUPMJ;

        /// <summary>
        /// Query object for column OAJOBN in Table.
        /// </summary>
        public Query OAJOBN;

        /// <summary>
        /// Query object for column OAUPMT in Table.
        /// </summary>
        public Query OAUPMT;

        /// <summary>
        /// Query object for column OALNID in Table.
        /// </summary>
        public Query OALNID;

        #region .ctor Constructor

        /// <summary>
        /// Creates Query Objects for all Columns for F4006
        /// </summary>
        /// <param name="DataManipObject">Data Manipulator for columns.</param>
        /// <param name="ColumnsTable">DataTable columns will be added to.</param>
        /// <param name="TableName">Name of table.</param>
        public F4006Columns(DataManipulator DataManipObject, DataObjectBase ColumnsTable, string TableName)
        {
            this.OADOCO = new Query("OADOCO", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.OADCTO = new Query("OADCTO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAKCOO = new Query("OAKCOO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAANTY = new Query("OAANTY", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAMLNM = new Query("OAMLNM", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAADD1 = new Query("OAADD1", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAADD2 = new Query("OAADD2", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAADD3 = new Query("OAADD3", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAADD4 = new Query("OAADD4", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAADDZ = new Query("OAADDZ", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OACTY1 = new Query("OACTY1", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OACOUN = new Query("OACOUN", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAADDS = new Query("OAADDS", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OACRTE = new Query("OACRTE", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OABKML = new Query("OABKML", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OACTR = new Query("OACTR", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAUSER = new Query("OAUSER", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAPID = new Query("OAPID", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAUPMJ = new Query("OAUPMJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.OAJOBN = new Query("OAJOBN", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.OAUPMT = new Query("OAUPMT", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.OALNID = new Query("OALNID", TableName, typeof(double), DataManipObject, ColumnsTable);
        }

        #endregion

        #region Clear

        /// <summary>
        /// Resets Query Conditions for All Columns
        /// </summary>
        public override void Clear()
        {
            OADOCO.Reset();
            OADCTO.Reset();
            OAKCOO.Reset();
            OAANTY.Reset();
            OAMLNM.Reset();
            OAADD1.Reset();
            OAADD2.Reset();
            OAADD3.Reset();
            OAADD4.Reset();
            OAADDZ.Reset();
            OACTY1.Reset();
            OACOUN.Reset();
            OAADDS.Reset();
            OACRTE.Reset();
            OABKML.Reset();
            OACTR.Reset();
            OAUSER.Reset();
            OAPID.Reset();
            OAUPMJ.Reset();
            OAJOBN.Reset();
            OAUPMT.Reset();
            OALNID.Reset();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes of Query Object Conditions for All Columns
        /// </summary>
        public override void Dispose()
        {
            OADOCO.Dispose();
            OADCTO.Dispose();
            OAKCOO.Dispose();
            OAANTY.Dispose();
            OAMLNM.Dispose();
            OAADD1.Dispose();
            OAADD2.Dispose();
            OAADD3.Dispose();
            OAADD4.Dispose();
            OAADDZ.Dispose();
            OACTY1.Dispose();
            OACOUN.Dispose();
            OAADDS.Dispose();
            OACRTE.Dispose();
            OABKML.Dispose();
            OACTR.Dispose();
            OAUSER.Dispose();
            OAPID.Dispose();
            OAUPMJ.Dispose();
            OAJOBN.Dispose();
            OAUPMT.Dispose();
            OALNID.Dispose();
        }

        #endregion

        #region AddColumnsToTable

        /// <summary>
        /// Adds All Columns to Table
        /// </summary>
        public virtual void AddColumnsToTable()
        {
            this.OADOCO.AddToTable();
            this.OADCTO.AddToTable();
            this.OAKCOO.AddToTable();
            this.OAANTY.AddToTable();
            this.OAMLNM.AddToTable();
            this.OAADD1.AddToTable();
            this.OAADD2.AddToTable();
            this.OAADD3.AddToTable();
            this.OAADD4.AddToTable();
            this.OAADDZ.AddToTable();
            this.OACTY1.AddToTable();
            this.OACOUN.AddToTable();
            this.OAADDS.AddToTable();
            this.OACRTE.AddToTable();
            this.OABKML.AddToTable();
            this.OACTR.AddToTable();
            this.OAUSER.AddToTable();
            this.OAPID.AddToTable();
            this.OAUPMJ.AddToTable();
            this.OAJOBN.AddToTable();
            this.OAUPMT.AddToTable();
            this.OALNID.AddToTable();
        }

        #endregion
    }

    #endregion
}