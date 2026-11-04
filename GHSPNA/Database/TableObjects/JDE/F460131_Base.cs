using System;
using System.Collections;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    #region F460131_Base

    /// <summary>
    /// Base Class for Table: F460131
    /// </summary>
    public class F460131_Base : DataObjectBase
    {
        #region Constructors

        /// <summary>
        /// Constructor for Table Object: F460131_Base
        /// Used for Enumerator
        /// </summary>
        public F460131_Base() : base()
        {
        }

        /// <summary>
        /// Constructor for Table Object: F460131_Base
        /// </summary>
        /// <param name="DbConnections">Data Access Layer Used by table class to Access Data and System tables</param>
        /// <param name="TableId">Table Id for storing multiple tables with same names.</param>
        public F460131_Base(DAL DbConnections, string TableId) : base(DbConnections, "F460131", TableId)
        {
            this._TableType = ((int) (TableType.Data));
            _Columns = new F460131Columns(_DataAccess.DataManipulator, this, "F460131");
            SetCurrentRow();
        }

        #endregion

        #region Columns

        /// <summary>
        /// Gets Columns to Perform Query on
        /// </summary>
        /// <value>The Columns Class F460131Columns used for querying and datamanipulation.</value>
        public F460131Columns Columns
        {
            get { return ((F460131Columns) (SetColumns)); }
            set { SetColumns = value; }
        }

        #endregion

        #region SPMCU

        /// <summary>
        /// Gets or Sets Value of SPMCU for Table Object
        /// </summary>
        /// <value>The Value of SPMCU Column from Table F460131</value>
        public virtual string SPMCU
        {
            get { return (Convert.ToString(Columns.SPMCU.FormatColumn(this["SPMCU"]))); }
            set { Where(Columns.SPMCU, Condition.EqualTo, value); }
        }

        #endregion

        #region SPAN8

        /// <summary>
        /// Gets or Sets Value of SPAN8 for Table Object
        /// </summary>
        /// <value>The Value of SPAN8 Column from Table F460131</value>
        public virtual double SPAN8
        {
            get { return double.Parse(Convert.ToString(Columns.SPAN8.FormatColumn(this["SPAN8"]))); }
            set { Where(Columns.SPAN8, Condition.EqualTo, value); }
        }

        #endregion

        #region SPSHAN

        /// <summary>
        /// Gets or Sets Value of SPSHAN for Table Object
        /// </summary>
        /// <value>The Value of SPSHAN Column from Table F460131</value>
        public virtual double SPSHAN
        {
            get { return double.Parse(Convert.ToString(Columns.SPSHAN.FormatColumn(this["SPSHAN"]))); }
            set { Where(Columns.SPSHAN, Condition.EqualTo, value); }
        }

        #endregion

        #region SPITM

        /// <summary>
        /// Gets or Sets Value of SPITM for Table Object
        /// </summary>
        /// <value>The Value of SPITM Column from Table F460131</value>
        public virtual double SPITM
        {
            get { return double.Parse(Convert.ToString(Columns.SPITM.FormatColumn(this["SPITM"]))); }
            set { Where(Columns.SPITM, Condition.EqualTo, value); }
        }

        #endregion

        #region SPPRP6

        /// <summary>
        /// Gets or Sets Value of SPPRP6 for Table Object
        /// </summary>
        /// <value>The Value of SPPRP6 Column from Table F460131</value>
        public virtual string SPPRP6
        {
            get { return (Convert.ToString(Columns.SPPRP6.FormatColumn(this["SPPRP6"]))); }
            set { Where(Columns.SPPRP6, Condition.EqualTo, value); }
        }

        #endregion

        #region SPUOM

        /// <summary>
        /// Gets or Sets Value of SPUOM for Table Object
        /// </summary>
        /// <value>The Value of SPUOM Column from Table F460131</value>
        public virtual string SPUOM
        {
            get { return (Convert.ToString(Columns.SPUOM.FormatColumn(this["SPUOM"]))); }
            set { Where(Columns.SPUOM, Condition.EqualTo, value); }
        }

        #endregion

        #region SPEQT2

        /// <summary>
        /// Gets or Sets Value of SPEQT2 for Table Object
        /// </summary>
        /// <value>The Value of SPEQT2 Column from Table F460131</value>
        public virtual string SPEQT2
        {
            get { return (Convert.ToString(Columns.SPEQT2.FormatColumn(this["SPEQT2"]))); }
            set { Where(Columns.SPEQT2, Condition.EqualTo, value); }
        }

        #endregion

        #region SPEQTY

        /// <summary>
        /// Gets or Sets Value of SPEQTY for Table Object
        /// </summary>
        /// <value>The Value of SPEQTY Column from Table F460131</value>
        public virtual string SPEQTY
        {
            get { return (Convert.ToString(Columns.SPEQTY.FormatColumn(this["SPEQTY"]))); }
            set { Where(Columns.SPEQTY, Condition.EqualTo, value); }
        }

        #endregion

        #region SPSTPK

        /// <summary>
        /// Gets or Sets Value of SPSTPK for Table Object
        /// </summary>
        /// <value>The Value of SPSTPK Column from Table F460131</value>
        public virtual double SPSTPK
        {
            get { return double.Parse(Convert.ToString(Columns.SPSTPK.FormatColumn(this["SPSTPK"]))); }
            set { Where(Columns.SPSTPK, Condition.EqualTo, value); }
        }

        #endregion

        #region SPCEQT

        /// <summary>
        /// Gets or Sets Value of SPCEQT for Table Object
        /// </summary>
        /// <value>The Value of SPCEQT Column from Table F460131</value>
        public virtual string SPCEQT
        {
            get { return (Convert.ToString(Columns.SPCEQT.FormatColumn(this["SPCEQT"]))); }
            set { Where(Columns.SPCEQT, Condition.EqualTo, value); }
        }

        #endregion

        #region SPRECSQ

        /// <summary>
        /// Gets or Sets Value of SPRECSQ for Table Object
        /// </summary>
        /// <value>The Value of SPRECSQ Column from Table F460131</value>
        public virtual double SPRECSQ
        {
            get { return double.Parse(Convert.ToString(Columns.SPRECSQ.FormatColumn(this["SPRECSQ"]))); }
            set { Where(Columns.SPRECSQ, Condition.EqualTo, value); }
        }

        #endregion

        #region SPC1FU

        /// <summary>
        /// Gets or Sets Value of SPC1FU for Table Object
        /// </summary>
        /// <value>The Value of SPC1FU Column from Table F460131</value>
        public virtual string SPC1FU
        {
            get { return (Convert.ToString(Columns.SPC1FU.FormatColumn(this["SPC1FU"]))); }
            set { Where(Columns.SPC1FU, Condition.EqualTo, value); }
        }

        #endregion

        #region SPCFC1

        /// <summary>
        /// Gets or Sets Value of SPCFC1 for Table Object
        /// </summary>
        /// <value>The Value of SPCFC1 Column from Table F460131</value>
        public virtual string SPCFC1
        {
            get { return (Convert.ToString(Columns.SPCFC1.FormatColumn(this["SPCFC1"]))); }
            set { Where(Columns.SPCFC1, Condition.EqualTo, value); }
        }

        #endregion

        #region SPCFS1

        /// <summary>
        /// Gets or Sets Value of SPCFS1 for Table Object
        /// </summary>
        /// <value>The Value of SPCFS1 Column from Table F460131</value>
        public virtual double SPCFS1
        {
            get { return double.Parse(Convert.ToString(Columns.SPCFS1.FormatColumn(this["SPCFS1"]))); }
            set { Where(Columns.SPCFS1, Condition.EqualTo, value); }
        }

        #endregion

        #region SPD1FU

        /// <summary>
        /// Gets or Sets Value of SPD1FU for Table Object
        /// </summary>
        /// <value>The Value of SPD1FU Column from Table F460131</value>
        public virtual decimal SPD1FU
        {
            get { return decimal.Parse(Convert.ToString(Columns.SPD1FU.FormatColumn(this["SPD1FU"]))); }
            set { Where(Columns.SPD1FU, Condition.EqualTo, value); }
        }

        #endregion

        #region SPURCD

        /// <summary>
        /// Gets or Sets Value of SPURCD for Table Object
        /// </summary>
        /// <value>The Value of SPURCD Column from Table F460131</value>
        public virtual string SPURCD
        {
            get { return (Convert.ToString(Columns.SPURCD.FormatColumn(this["SPURCD"]))); }
            set { Where(Columns.SPURCD, Condition.EqualTo, value); }
        }

        #endregion

        #region SPURDT

        /// <summary>
        /// Gets or Sets Value of SPURDT for Table Object
        /// </summary>
        /// <value>The Value of SPURDT Column from Table F460131</value>
        public virtual decimal SPURDT
        {
            get { return decimal.Parse(Convert.ToString(Columns.SPURDT.FormatColumn(this["SPURDT"]))); }
            set { Where(Columns.SPURDT, Condition.EqualTo, value); }
        }

        #endregion

        #region SPURAT

        /// <summary>
        /// Gets or Sets Value of SPURAT for Table Object
        /// </summary>
        /// <value>The Value of SPURAT Column from Table F460131</value>
        public virtual double SPURAT
        {
            get { return double.Parse(Convert.ToString(Columns.SPURAT.FormatColumn(this["SPURAT"]))); }
            set { Where(Columns.SPURAT, Condition.EqualTo, value); }
        }

        #endregion

        #region SPURAB

        /// <summary>
        /// Gets or Sets Value of SPURAB for Table Object
        /// </summary>
        /// <value>The Value of SPURAB Column from Table F460131</value>
        public virtual double SPURAB
        {
            get { return double.Parse(Convert.ToString(Columns.SPURAB.FormatColumn(this["SPURAB"]))); }
            set { Where(Columns.SPURAB, Condition.EqualTo, value); }
        }

        #endregion

        #region SPURRF

        /// <summary>
        /// Gets or Sets Value of SPURRF for Table Object
        /// </summary>
        /// <value>The Value of SPURRF Column from Table F460131</value>
        public virtual string SPURRF
        {
            get { return (Convert.ToString(Columns.SPURRF.FormatColumn(this["SPURRF"]))); }
            set { Where(Columns.SPURRF, Condition.EqualTo, value); }
        }

        #endregion

        #region SPUSER

        /// <summary>
        /// Gets or Sets Value of SPUSER for Table Object
        /// </summary>
        /// <value>The Value of SPUSER Column from Table F460131</value>
        public virtual string SPUSER
        {
            get { return (Convert.ToString(Columns.SPUSER.FormatColumn(this["SPUSER"]))); }
            set { Where(Columns.SPUSER, Condition.EqualTo, value); }
        }

        #endregion

        #region SPPID

        /// <summary>
        /// Gets or Sets Value of SPPID for Table Object
        /// </summary>
        /// <value>The Value of SPPID Column from Table F460131</value>
        public virtual string SPPID
        {
            get { return (Convert.ToString(Columns.SPPID.FormatColumn(this["SPPID"]))); }
            set { Where(Columns.SPPID, Condition.EqualTo, value); }
        }

        #endregion

        #region SPJOBN

        /// <summary>
        /// Gets or Sets Value of SPJOBN for Table Object
        /// </summary>
        /// <value>The Value of SPJOBN Column from Table F460131</value>
        public virtual string SPJOBN
        {
            get { return (Convert.ToString(Columns.SPJOBN.FormatColumn(this["SPJOBN"]))); }
            set { Where(Columns.SPJOBN, Condition.EqualTo, value); }
        }

        #endregion

        #region SPUPMJ

        /// <summary>
        /// Gets or Sets Value of SPUPMJ for Table Object
        /// </summary>
        /// <value>The Value of SPUPMJ Column from Table F460131</value>
        public virtual decimal SPUPMJ
        {
            get { return decimal.Parse(Convert.ToString(Columns.SPUPMJ.FormatColumn(this["SPUPMJ"]))); }
            set { Where(Columns.SPUPMJ, Condition.EqualTo, value); }
        }

        #endregion

        #region SPTDAY

        /// <summary>
        /// Gets or Sets Value of SPTDAY for Table Object
        /// </summary>
        /// <value>The Value of SPTDAY Column from Table F460131</value>
        public virtual double SPTDAY
        {
            get { return double.Parse(Convert.ToString(Columns.SPTDAY.FormatColumn(this["SPTDAY"]))); }
            set { Where(Columns.SPTDAY, Condition.EqualTo, value); }
        }

        #endregion

        #region SPMPGP

        /// <summary>
        /// Gets or Sets Value of SPMPGP for Table Object
        /// </summary>
        /// <value>The Value of SPMPGP Column from Table F460131</value>
        public virtual string SPMPGP
        {
            get { return (Convert.ToString(Columns.SPMPGP.FormatColumn(this["SPMPGP"]))); }
            set { Where(Columns.SPMPGP, Condition.EqualTo, value); }
        }

        #endregion

        #region GetEnumerator

        /// <summary>
        /// Returns the IEnumerator Object for Fetching Multiple Records from F460131
        /// </summary>
        /// <returns>The enumerator object for F460131_Collection</returns>
        public override IEnumerator GetEnumerator()
        {
            return new F460131_Collection(this);
        }

        #endregion
    }

    #endregion

    #region F460131Columns

    /// <summary>
    /// Base Class for Column Class for Query Construction
    /// </summary>
    public class F460131Columns : ITableColumns
    {
        /// <summary>
        /// Query object for column SPMCU in Table.
        /// </summary>
        public Query SPMCU;

        /// <summary>
        /// Query object for column SPAN8 in Table.
        /// </summary>
        public Query SPAN8;

        /// <summary>
        /// Query object for column SPSHAN in Table.
        /// </summary>
        public Query SPSHAN;

        /// <summary>
        /// Query object for column SPITM in Table.
        /// </summary>
        public Query SPITM;

        /// <summary>
        /// Query object for column SPPRP6 in Table.
        /// </summary>
        public Query SPPRP6;

        /// <summary>
        /// Query object for column SPUOM in Table.
        /// </summary>
        public Query SPUOM;

        /// <summary>
        /// Query object for column SPEQT2 in Table.
        /// </summary>
        public Query SPEQT2;

        /// <summary>
        /// Query object for column SPEQTY in Table.
        /// </summary>
        public Query SPEQTY;

        /// <summary>
        /// Query object for column SPSTPK in Table.
        /// </summary>
        public Query SPSTPK;

        /// <summary>
        /// Query object for column SPCEQT in Table.
        /// </summary>
        public Query SPCEQT;

        /// <summary>
        /// Query object for column SPRECSQ in Table.
        /// </summary>
        public Query SPRECSQ;

        /// <summary>
        /// Query object for column SPC1FU in Table.
        /// </summary>
        public Query SPC1FU;

        /// <summary>
        /// Query object for column SPCFC1 in Table.
        /// </summary>
        public Query SPCFC1;

        /// <summary>
        /// Query object for column SPCFS1 in Table.
        /// </summary>
        public Query SPCFS1;

        /// <summary>
        /// Query object for column SPD1FU in Table.
        /// </summary>
        public Query SPD1FU;

        /// <summary>
        /// Query object for column SPURCD in Table.
        /// </summary>
        public Query SPURCD;

        /// <summary>
        /// Query object for column SPURDT in Table.
        /// </summary>
        public Query SPURDT;

        /// <summary>
        /// Query object for column SPURAT in Table.
        /// </summary>
        public Query SPURAT;

        /// <summary>
        /// Query object for column SPURAB in Table.
        /// </summary>
        public Query SPURAB;

        /// <summary>
        /// Query object for column SPURRF in Table.
        /// </summary>
        public Query SPURRF;

        /// <summary>
        /// Query object for column SPUSER in Table.
        /// </summary>
        public Query SPUSER;

        /// <summary>
        /// Query object for column SPPID in Table.
        /// </summary>
        public Query SPPID;

        /// <summary>
        /// Query object for column SPJOBN in Table.
        /// </summary>
        public Query SPJOBN;

        /// <summary>
        /// Query object for column SPUPMJ in Table.
        /// </summary>
        public Query SPUPMJ;

        /// <summary>
        /// Query object for column SPTDAY in Table.
        /// </summary>
        public Query SPTDAY;

        /// <summary>
        /// Query object for column SPMPGP in Table.
        /// </summary>
        public Query SPMPGP;

        #region .ctor Constructor

        /// <summary>
        /// Creates Query Objects for all Columns for F460131
        /// </summary>
        /// <param name="DataManipObject">Data Manipulator for columns.</param>
        /// <param name="ColumnsTable">DataTable columns will be added to.</param>
        /// <param name="TableName">Name of table.</param>
        public F460131Columns(DataManipulator DataManipObject, DataObjectBase ColumnsTable, string TableName)
        {
            this.SPMCU = new Query("SPMCU", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPAN8 = new Query("SPAN8", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.SPSHAN = new Query("SPSHAN", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.SPITM = new Query("SPITM", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.SPPRP6 = new Query("SPPRP6", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPUOM = new Query("SPUOM", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPEQT2 = new Query("SPEQT2", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPEQTY = new Query("SPEQTY", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPSTPK = new Query("SPSTPK", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.SPCEQT = new Query("SPCEQT", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPRECSQ = new Query("SPRECSQ", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.SPC1FU = new Query("SPC1FU", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPCFC1 = new Query("SPCFC1", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPCFS1 = new Query("SPCFS1", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.SPD1FU = new Query("SPD1FU", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.SPURCD = new Query("SPURCD", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPURDT = new Query("SPURDT", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.SPURAT = new Query("SPURAT", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.SPURAB = new Query("SPURAB", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.SPURRF = new Query("SPURRF", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPUSER = new Query("SPUSER", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPPID = new Query("SPPID", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPJOBN = new Query("SPJOBN", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.SPUPMJ = new Query("SPUPMJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.SPTDAY = new Query("SPTDAY", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.SPMPGP = new Query("SPMPGP", TableName, typeof(string), DataManipObject, ColumnsTable);
        }

        #endregion

        #region Clear

        /// <summary>
        /// Resets Query Conditions for All Columns
        /// </summary>
        public override void Clear()
        {
            SPMCU.Reset();
            SPAN8.Reset();
            SPSHAN.Reset();
            SPITM.Reset();
            SPPRP6.Reset();
            SPUOM.Reset();
            SPEQT2.Reset();
            SPEQTY.Reset();
            SPSTPK.Reset();
            SPCEQT.Reset();
            SPRECSQ.Reset();
            SPC1FU.Reset();
            SPCFC1.Reset();
            SPCFS1.Reset();
            SPD1FU.Reset();
            SPURCD.Reset();
            SPURDT.Reset();
            SPURAT.Reset();
            SPURAB.Reset();
            SPURRF.Reset();
            SPUSER.Reset();
            SPPID.Reset();
            SPJOBN.Reset();
            SPUPMJ.Reset();
            SPTDAY.Reset();
            SPMPGP.Reset();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes of Query Object Conditions for All Columns
        /// </summary>
        public override void Dispose()
        {
            SPMCU.Dispose();
            SPAN8.Dispose();
            SPSHAN.Dispose();
            SPITM.Dispose();
            SPPRP6.Dispose();
            SPUOM.Dispose();
            SPEQT2.Dispose();
            SPEQTY.Dispose();
            SPSTPK.Dispose();
            SPCEQT.Dispose();
            SPRECSQ.Dispose();
            SPC1FU.Dispose();
            SPCFC1.Dispose();
            SPCFS1.Dispose();
            SPD1FU.Dispose();
            SPURCD.Dispose();
            SPURDT.Dispose();
            SPURAT.Dispose();
            SPURAB.Dispose();
            SPURRF.Dispose();
            SPUSER.Dispose();
            SPPID.Dispose();
            SPJOBN.Dispose();
            SPUPMJ.Dispose();
            SPTDAY.Dispose();
            SPMPGP.Dispose();
        }

        #endregion

        #region AddColumnsToTable

        /// <summary>
        /// Adds All Columns to Table
        /// </summary>
        public virtual void AddColumnsToTable()
        {
            this.SPMCU.AddToTable();
            this.SPAN8.AddToTable();
            this.SPSHAN.AddToTable();
            this.SPITM.AddToTable();
            this.SPPRP6.AddToTable();
            this.SPUOM.AddToTable();
            this.SPEQT2.AddToTable();
            this.SPEQTY.AddToTable();
            this.SPSTPK.AddToTable();
            this.SPCEQT.AddToTable();
            this.SPRECSQ.AddToTable();
            this.SPC1FU.AddToTable();
            this.SPCFC1.AddToTable();
            this.SPCFS1.AddToTable();
            this.SPD1FU.AddToTable();
            this.SPURCD.AddToTable();
            this.SPURDT.AddToTable();
            this.SPURAT.AddToTable();
            this.SPURAB.AddToTable();
            this.SPURRF.AddToTable();
            this.SPUSER.AddToTable();
            this.SPPID.AddToTable();
            this.SPJOBN.AddToTable();
            this.SPUPMJ.AddToTable();
            this.SPTDAY.AddToTable();
            this.SPMPGP.AddToTable();
        }

        #endregion
    }

    #endregion
}