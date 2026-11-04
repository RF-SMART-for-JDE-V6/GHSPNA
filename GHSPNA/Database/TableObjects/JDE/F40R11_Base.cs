using System;
using System.Collections;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    #region F40R11_Base

    /// <summary>
    /// Base Class for Table: F40R11
    /// </summary>
    public class F40R11_Base : DataObjectBase
    {
        #region Constructors

        /// <summary>
        /// Constructor for Table Object: F40R11_Base
        /// Used for Enumerator
        /// </summary>
        public F40R11_Base() : base()
        {
        }

        /// <summary>
        /// Constructor for Table Object: F40R11_Base
        /// </summary>
        /// <param name="DbConnections">Data Access Layer Used by table class to Access Data and System tables</param>
        /// <param name="TableId">Table Id for storing multiple tables with same names.</param>
        public F40R11_Base(DAL DbConnections, string TableId) : base(DbConnections, "F40R11", TableId)
        {
            this._TableType = ((int) (TableType.Data));
            _Columns = new F40R11Columns(_DataAccess.DataManipulator, this, "F40R11");
            SetCurrentRow();
        }

        #endregion

        #region Columns

        /// <summary>
        /// Gets Columns to Perform Query on
        /// </summary>
        /// <value>The Columns Class F40R11Columns used for querying and datamanipulation.</value>
        public F40R11Columns Columns
        {
            get { return ((F40R11Columns) (SetColumns)); }
            set { SetColumns = value; }
        }

        #endregion

        #region DDUKID

        /// <summary>
        /// Gets or Sets Value of DDUKID for Table Object
        /// </summary>
        /// <value>The Value of DDUKID Column from Table F40R11</value>
        public virtual double DDUKID
        {
            get { return double.Parse(Convert.ToString(Columns.DDUKID.FormatColumn(this["DDUKID"]))); }
            set { Where(Columns.DDUKID, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDEID

        /// <summary>
        /// Gets or Sets Value of DDDEID for Table Object
        /// </summary>
        /// <value>The Value of DDDEID Column from Table F40R11</value>
        public virtual double DDDEID
        {
            get { return double.Parse(Convert.ToString(Columns.DDDEID.FormatColumn(this["DDDEID"]))); }
            set { Where(Columns.DDDEID, Condition.EqualTo, value); }
        }

        #endregion

        #region DDMCU

        /// <summary>
        /// Gets or Sets Value of DDMCU for Table Object
        /// </summary>
        /// <value>The Value of DDMCU Column from Table F40R11</value>
        public virtual string DDMCU
        {
            get { return (Convert.ToString(Columns.DDMCU.FormatColumn(this["DDMCU"]))); }
            set { Where(Columns.DDMCU, Condition.EqualTo, value); }
        }

        #endregion

        #region DDAN8

        /// <summary>
        /// Gets or Sets Value of DDAN8 for Table Object
        /// </summary>
        /// <value>The Value of DDAN8 Column from Table F40R11</value>
        public virtual double DDAN8
        {
            get { return double.Parse(Convert.ToString(Columns.DDAN8.FormatColumn(this["DDAN8"]))); }
            set { Where(Columns.DDAN8, Condition.EqualTo, value); }
        }

        #endregion

        #region DDSHAN

        /// <summary>
        /// Gets or Sets Value of DDSHAN for Table Object
        /// </summary>
        /// <value>The Value of DDSHAN Column from Table F40R11</value>
        public virtual double DDSHAN
        {
            get { return double.Parse(Convert.ToString(Columns.DDSHAN.FormatColumn(this["DDSHAN"]))); }
            set { Where(Columns.DDSHAN, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDVAN

        /// <summary>
        /// Gets or Sets Value of DDDVAN for Table Object
        /// </summary>
        /// <value>The Value of DDDVAN Column from Table F40R11</value>
        public virtual double DDDVAN
        {
            get { return double.Parse(Convert.ToString(Columns.DDDVAN.FormatColumn(this["DDDVAN"]))); }
            set { Where(Columns.DDDVAN, Condition.EqualTo, value); }
        }

        #endregion

        #region DDITM

        /// <summary>
        /// Gets or Sets Value of DDITM for Table Object
        /// </summary>
        /// <value>The Value of DDITM Column from Table F40R11</value>
        public virtual double DDITM
        {
            get { return double.Parse(Convert.ToString(Columns.DDITM.FormatColumn(this["DDITM"]))); }
            set { Where(Columns.DDITM, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDETYP

        /// <summary>
        /// Gets or Sets Value of DDDETYP for Table Object
        /// </summary>
        /// <value>The Value of DDDETYP Column from Table F40R11</value>
        public virtual string DDDETYP
        {
            get { return (Convert.ToString(Columns.DDDETYP.FormatColumn(this["DDDETYP"]))); }
            set { Where(Columns.DDDETYP, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDEPER

        /// <summary>
        /// Gets or Sets Value of DDDEPER for Table Object
        /// </summary>
        /// <value>The Value of DDDEPER Column from Table F40R11</value>
        public virtual string DDDEPER
        {
            get { return (Convert.ToString(Columns.DDDEPER.FormatColumn(this["DDDEPER"]))); }
            set { Where(Columns.DDDEPER, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCRTSOSTS

        /// <summary>
        /// Gets or Sets Value of DDCRTSOSTS for Table Object
        /// </summary>
        /// <value>The Value of DDCRTSOSTS Column from Table F40R11</value>
        public virtual string DDCRTSOSTS
        {
            get { return (Convert.ToString(Columns.DDCRTSOSTS.FormatColumn(this["DDCRTSOSTS"]))); }
            set { Where(Columns.DDCRTSOSTS, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRLSNO

        /// <summary>
        /// Gets or Sets Value of DDRLSNO for Table Object
        /// </summary>
        /// <value>The Value of DDRLSNO Column from Table F40R11</value>
        public virtual string DDRLSNO
        {
            get { return (Convert.ToString(Columns.DDRLSNO.FormatColumn(this["DDRLSNO"]))); }
            set { Where(Columns.DDRLSNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRLSDJ

        /// <summary>
        /// Gets or Sets Value of DDRLSDJ for Table Object
        /// </summary>
        /// <value>The Value of DDRLSDJ Column from Table F40R11</value>
        public virtual decimal DDRLSDJ
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDRLSDJ.FormatColumn(this["DDRLSDJ"]))); }
            set { Where(Columns.DDRLSDJ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRLSTM

        /// <summary>
        /// Gets or Sets Value of DDRLSTM for Table Object
        /// </summary>
        /// <value>The Value of DDRLSTM Column from Table F40R11</value>
        public virtual double DDRLSTM
        {
            get { return double.Parse(Convert.ToString(Columns.DDRLSTM.FormatColumn(this["DDRLSTM"]))); }
            set { Where(Columns.DDRLSTM, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRQSJ

        /// <summary>
        /// Gets or Sets Value of DDRQSJ for Table Object
        /// </summary>
        /// <value>The Value of DDRQSJ Column from Table F40R11</value>
        public virtual decimal DDRQSJ
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDRQSJ.FormatColumn(this["DDRQSJ"]))); }
            set { Where(Columns.DDRQSJ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDERQSHPDJ

        /// <summary>
        /// Gets or Sets Value of DDERQSHPDJ for Table Object
        /// </summary>
        /// <value>The Value of DDERQSHPDJ Column from Table F40R11</value>
        public virtual decimal DDERQSHPDJ
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDERQSHPDJ.FormatColumn(this["DDERQSHPDJ"]))); }
            set { Where(Columns.DDERQSHPDJ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRSHT

        /// <summary>
        /// Gets or Sets Value of DDRSHT for Table Object
        /// </summary>
        /// <value>The Value of DDRSHT Column from Table F40R11</value>
        public virtual double DDRSHT
        {
            get { return double.Parse(Convert.ToString(Columns.DDRSHT.FormatColumn(this["DDRSHT"]))); }
            set { Where(Columns.DDRSHT, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRQSHPQTY

        /// <summary>
        /// Gets or Sets Value of DDRQSHPQTY for Table Object
        /// </summary>
        /// <value>The Value of DDRQSHPQTY Column from Table F40R11</value>
        public virtual double DDRQSHPQTY
        {
            get { return double.Parse(Convert.ToString(Columns.DDRQSHPQTY.FormatColumn(this["DDRQSHPQTY"]))); }
            set { Where(Columns.DDRQSHPQTY, Condition.EqualTo, value); }
        }

        #endregion

        #region DDUOM

        /// <summary>
        /// Gets or Sets Value of DDUOM for Table Object
        /// </summary>
        /// <value>The Value of DDUOM Column from Table F40R11</value>
        public virtual string DDUOM
        {
            get { return (Convert.ToString(Columns.DDUOM.FormatColumn(this["DDUOM"]))); }
            set { Where(Columns.DDUOM, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRSDJ

        /// <summary>
        /// Gets or Sets Value of DDRSDJ for Table Object
        /// </summary>
        /// <value>The Value of DDRSDJ Column from Table F40R11</value>
        public virtual decimal DDRSDJ
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDRSDJ.FormatColumn(this["DDRSDJ"]))); }
            set { Where(Columns.DDRSDJ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRSDT

        /// <summary>
        /// Gets or Sets Value of DDRSDT for Table Object
        /// </summary>
        /// <value>The Value of DDRSDT Column from Table F40R11</value>
        public virtual double DDRSDT
        {
            get { return double.Parse(Convert.ToString(Columns.DDRSDT.FormatColumn(this["DDRSDT"]))); }
            set { Where(Columns.DDRSDT, Condition.EqualTo, value); }
        }

        #endregion

        #region DDHZED

        /// <summary>
        /// Gets or Sets Value of DDHZED for Table Object
        /// </summary>
        /// <value>The Value of DDHZED Column from Table F40R11</value>
        public virtual decimal DDHZED
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDHZED.FormatColumn(this["DDHZED"]))); }
            set { Where(Columns.DDHZED, Condition.EqualTo, value); }
        }

        #endregion

        #region DDHZSD

        /// <summary>
        /// Gets or Sets Value of DDHZSD for Table Object
        /// </summary>
        /// <value>The Value of DDHZSD Column from Table F40R11</value>
        public virtual decimal DDHZSD
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDHZSD.FormatColumn(this["DDHZSD"]))); }
            set { Where(Columns.DDHZSD, Condition.EqualTo, value); }
        }

        #endregion

        #region DDASBLNFD

        /// <summary>
        /// Gets or Sets Value of DDASBLNFD for Table Object
        /// </summary>
        /// <value>The Value of DDASBLNFD Column from Table F40R11</value>
        public virtual string DDASBLNFD
        {
            get { return (Convert.ToString(Columns.DDASBLNFD.FormatColumn(this["DDASBLNFD"]))); }
            set { Where(Columns.DDASBLNFD, Condition.EqualTo, value); }
        }

        #endregion

        #region DDASBLNLOC

        /// <summary>
        /// Gets or Sets Value of DDASBLNLOC for Table Object
        /// </summary>
        /// <value>The Value of DDASBLNLOC Column from Table F40R11</value>
        public virtual string DDASBLNLOC
        {
            get { return (Convert.ToString(Columns.DDASBLNLOC.FormatColumn(this["DDASBLNLOC"]))); }
            set { Where(Columns.DDASBLNLOC, Condition.EqualTo, value); }
        }

        #endregion

        #region DDASBNO

        /// <summary>
        /// Gets or Sets Value of DDASBNO for Table Object
        /// </summary>
        /// <value>The Value of DDASBNO Column from Table F40R11</value>
        public virtual string DDASBNO
        {
            get { return (Convert.ToString(Columns.DDASBNO.FormatColumn(this["DDASBNO"]))); }
            set { Where(Columns.DDASBNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDBTHNO

        /// <summary>
        /// Gets or Sets Value of DDDMDBTHNO for Table Object
        /// </summary>
        /// <value>The Value of DDDMDBTHNO Column from Table F40R11</value>
        public virtual double DDDMDBTHNO
        {
            get { return double.Parse(Convert.ToString(Columns.DDDMDBTHNO.FormatColumn(this["DDDMDBTHNO"]))); }
            set { Where(Columns.DDDMDBTHNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCSGSHPNO

        /// <summary>
        /// Gets or Sets Value of DDCSGSHPNO for Table Object
        /// </summary>
        /// <value>The Value of DDCSGSHPNO Column from Table F40R11</value>
        public virtual string DDCSGSHPNO
        {
            get { return (Convert.ToString(Columns.DDCSGSHPNO.FormatColumn(this["DDCSGSHPNO"]))); }
            set { Where(Columns.DDCSGSHPNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDGCNT

        /// <summary>
        /// Gets or Sets Value of DDGCNT for Table Object
        /// </summary>
        /// <value>The Value of DDGCNT Column from Table F40R11</value>
        public virtual string DDGCNT
        {
            get { return (Convert.ToString(Columns.DDGCNT.FormatColumn(this["DDGCNT"]))); }
            set { Where(Columns.DDGCNT, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCUMID

        /// <summary>
        /// Gets or Sets Value of DDCUMID for Table Object
        /// </summary>
        /// <value>The Value of DDCUMID Column from Table F40R11</value>
        public virtual double DDCUMID
        {
            get { return double.Parse(Convert.ToString(Columns.DDCUMID.FormatColumn(this["DDCUMID"]))); }
            set { Where(Columns.DDCUMID, Condition.EqualTo, value); }
        }

        #endregion

        #region DDTXTREFNO

        /// <summary>
        /// Gets or Sets Value of DDTXTREFNO for Table Object
        /// </summary>
        /// <value>The Value of DDTXTREFNO Column from Table F40R11</value>
        public virtual string DDTXTREFNO
        {
            get { return (Convert.ToString(Columns.DDTXTREFNO.FormatColumn(this["DDTXTREFNO"]))); }
            set { Where(Columns.DDTXTREFNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDREQFRQ

        /// <summary>
        /// Gets or Sets Value of DDREQFRQ for Table Object
        /// </summary>
        /// <value>The Value of DDREQFRQ Column from Table F40R11</value>
        public virtual string DDREQFRQ
        {
            get { return (Convert.ToString(Columns.DDREQFRQ.FormatColumn(this["DDREQFRQ"]))); }
            set { Where(Columns.DDREQFRQ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCSTDMDPT

        /// <summary>
        /// Gets or Sets Value of DDCSTDMDPT for Table Object
        /// </summary>
        /// <value>The Value of DDCSTDMDPT Column from Table F40R11</value>
        public virtual string DDCSTDMDPT
        {
            get { return (Convert.ToString(Columns.DDCSTDMDPT.FormatColumn(this["DDCSTDMDPT"]))); }
            set { Where(Columns.DDCSTDMDPT, Condition.EqualTo, value); }
        }

        #endregion

        #region DDREQTYP

        /// <summary>
        /// Gets or Sets Value of DDREQTYP for Table Object
        /// </summary>
        /// <value>The Value of DDREQTYP Column from Table F40R11</value>
        public virtual string DDREQTYP
        {
            get { return (Convert.ToString(Columns.DDREQTYP.FormatColumn(this["DDREQTYP"]))); }
            set { Where(Columns.DDREQTYP, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCSTORDNO

        /// <summary>
        /// Gets or Sets Value of DDCSTORDNO for Table Object
        /// </summary>
        /// <value>The Value of DDCSTORDNO Column from Table F40R11</value>
        public virtual string DDCSTORDNO
        {
            get { return (Convert.ToString(Columns.DDCSTORDNO.FormatColumn(this["DDCSTORDNO"]))); }
            set { Where(Columns.DDCSTORDNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDKID

        /// <summary>
        /// Gets or Sets Value of DDDKID for Table Object
        /// </summary>
        /// <value>The Value of DDDKID Column from Table F40R11</value>
        public virtual string DDDKID
        {
            get { return (Convert.ToString(Columns.DDDKID.FormatColumn(this["DDDKID"]))); }
            set { Where(Columns.DDDKID, Condition.EqualTo, value); }
        }

        #endregion

        #region DDENDJOBSQ

        /// <summary>
        /// Gets or Sets Value of DDENDJOBSQ for Table Object
        /// </summary>
        /// <value>The Value of DDENDJOBSQ Column from Table F40R11</value>
        public virtual string DDENDJOBSQ
        {
            get { return (Convert.ToString(Columns.DDENDJOBSQ.FormatColumn(this["DDENDJOBSQ"]))); }
            set { Where(Columns.DDENDJOBSQ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDPSJOBNO

        /// <summary>
        /// Gets or Sets Value of DDPSJOBNO for Table Object
        /// </summary>
        /// <value>The Value of DDPSJOBNO Column from Table F40R11</value>
        public virtual string DDPSJOBNO
        {
            get { return (Convert.ToString(Columns.DDPSJOBNO.FormatColumn(this["DDPSJOBNO"]))); }
            set { Where(Columns.DDPSJOBNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDHZCLSNO

        /// <summary>
        /// Gets or Sets Value of DDHZCLSNO for Table Object
        /// </summary>
        /// <value>The Value of DDHZCLSNO Column from Table F40R11</value>
        public virtual string DDHZCLSNO
        {
            get { return (Convert.ToString(Columns.DDHZCLSNO.FormatColumn(this["DDHZCLSNO"]))); }
            set { Where(Columns.DDHZCLSNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDJOBSQ

        /// <summary>
        /// Gets or Sets Value of DDJOBSQ for Table Object
        /// </summary>
        /// <value>The Value of DDJOBSQ Column from Table F40R11</value>
        public virtual string DDJOBSQ
        {
            get { return (Convert.ToString(Columns.DDJOBSQ.FormatColumn(this["DDJOBSQ"]))); }
            set { Where(Columns.DDJOBSQ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDKBSNO

        /// <summary>
        /// Gets or Sets Value of DDKBSNO for Table Object
        /// </summary>
        /// <value>The Value of DDKBSNO Column from Table F40R11</value>
        public virtual string DDKBSNO
        {
            get { return (Convert.ToString(Columns.DDKBSNO.FormatColumn(this["DDKBSNO"]))); }
            set { Where(Columns.DDKBSNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDKBENO

        /// <summary>
        /// Gets or Sets Value of DDKBENO for Table Object
        /// </summary>
        /// <value>The Value of DDKBENO Column from Table F40R11</value>
        public virtual string DDKBENO
        {
            get { return (Convert.ToString(Columns.DDKBENO.FormatColumn(this["DDKBENO"]))); }
            set { Where(Columns.DDKBENO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDLABORHR

        /// <summary>
        /// Gets or Sets Value of DDLABORHR for Table Object
        /// </summary>
        /// <value>The Value of DDLABORHR Column from Table F40R11</value>
        public virtual double DDLABORHR
        {
            get { return double.Parse(Convert.ToString(Columns.DDLABORHR.FormatColumn(this["DDLABORHR"]))); }
            set { Where(Columns.DDLABORHR, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCIRV

        /// <summary>
        /// Gets or Sets Value of DDCIRV for Table Object
        /// </summary>
        /// <value>The Value of DDCIRV Column from Table F40R11</value>
        public virtual string DDCIRV
        {
            get { return (Convert.ToString(Columns.DDCIRV.FormatColumn(this["DDCIRV"]))); }
            set { Where(Columns.DDCIRV, Condition.EqualTo, value); }
        }

        #endregion

        #region DDYEAR1

        /// <summary>
        /// Gets or Sets Value of DDYEAR1 for Table Object
        /// </summary>
        /// <value>The Value of DDYEAR1 Column from Table F40R11</value>
        public virtual double DDYEAR1
        {
            get { return double.Parse(Convert.ToString(Columns.DDYEAR1.FormatColumn(this["DDYEAR1"]))); }
            set { Where(Columns.DDYEAR1, Condition.EqualTo, value); }
        }

        #endregion

        #region DDPTRLSSTS

        /// <summary>
        /// Gets or Sets Value of DDPTRLSSTS for Table Object
        /// </summary>
        /// <value>The Value of DDPTRLSSTS Column from Table F40R11</value>
        public virtual string DDPTRLSSTS
        {
            get { return (Convert.ToString(Columns.DDPTRLSSTS.FormatColumn(this["DDPTRLSSTS"]))); }
            set { Where(Columns.DDPTRLSSTS, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCSTPODJ

        /// <summary>
        /// Gets or Sets Value of DDCSTPODJ for Table Object
        /// </summary>
        /// <value>The Value of DDCSTPODJ Column from Table F40R11</value>
        public virtual decimal DDCSTPODJ
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDCSTPODJ.FormatColumn(this["DDCSTPODJ"]))); }
            set { Where(Columns.DDCSTPODJ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCSTPOTM

        /// <summary>
        /// Gets or Sets Value of DDCSTPOTM for Table Object
        /// </summary>
        /// <value>The Value of DDCSTPOTM Column from Table F40R11</value>
        public virtual double DDCSTPOTM
        {
            get { return double.Parse(Convert.ToString(Columns.DDCSTPOTM.FormatColumn(this["DDCSTPOTM"]))); }
            set { Where(Columns.DDCSTPOTM, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCSTPOLIN

        /// <summary>
        /// Gets or Sets Value of DDCSTPOLIN for Table Object
        /// </summary>
        /// <value>The Value of DDCSTPOLIN Column from Table F40R11</value>
        public virtual string DDCSTPOLIN
        {
            get { return (Convert.ToString(Columns.DDCSTPOLIN.FormatColumn(this["DDCSTPOLIN"]))); }
            set { Where(Columns.DDCSTPOLIN, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCSTPO

        /// <summary>
        /// Gets or Sets Value of DDCSTPO for Table Object
        /// </summary>
        /// <value>The Value of DDCSTPO Column from Table F40R11</value>
        public virtual string DDCSTPO
        {
            get { return (Convert.ToString(Columns.DDCSTPO.FormatColumn(this["DDCSTPO"]))); }
            set { Where(Columns.DDCSTPO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDTPUR

        /// <summary>
        /// Gets or Sets Value of DDTPUR for Table Object
        /// </summary>
        /// <value>The Value of DDTPUR Column from Table F40R11</value>
        public virtual string DDTPUR
        {
            get { return (Convert.ToString(Columns.DDTPUR.FormatColumn(this["DDTPUR"]))); }
            set { Where(Columns.DDTPUR, Condition.EqualTo, value); }
        }

        #endregion

        #region DDQTYBYPOS

        /// <summary>
        /// Gets or Sets Value of DDQTYBYPOS for Table Object
        /// </summary>
        /// <value>The Value of DDQTYBYPOS Column from Table F40R11</value>
        public virtual double DDQTYBYPOS
        {
            get { return double.Parse(Convert.ToString(Columns.DDQTYBYPOS.FormatColumn(this["DDQTYBYPOS"]))); }
            set { Where(Columns.DDQTYBYPOS, Condition.EqualTo, value); }
        }

        #endregion

        #region DDSOCN

        /// <summary>
        /// Gets or Sets Value of DDSOCN for Table Object
        /// </summary>
        /// <value>The Value of DDSOCN Column from Table F40R11</value>
        public virtual double DDSOCN
        {
            get { return double.Parse(Convert.ToString(Columns.DDSOCN.FormatColumn(this["DDSOCN"]))); }
            set { Where(Columns.DDSOCN, Condition.EqualTo, value); }
        }

        #endregion

        #region DDPSIG

        /// <summary>
        /// Gets or Sets Value of DDPSIG for Table Object
        /// </summary>
        /// <value>The Value of DDPSIG Column from Table F40R11</value>
        public virtual string DDPSIG
        {
            get { return (Convert.ToString(Columns.DDPSIG.FormatColumn(this["DDPSIG"]))); }
            set { Where(Columns.DDPSIG, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRSVLNFD

        /// <summary>
        /// Gets or Sets Value of DDRSVLNFD for Table Object
        /// </summary>
        /// <value>The Value of DDRSVLNFD Column from Table F40R11</value>
        public virtual string DDRSVLNFD
        {
            get { return (Convert.ToString(Columns.DDRSVLNFD.FormatColumn(this["DDRSVLNFD"]))); }
            set { Where(Columns.DDRSVLNFD, Condition.EqualTo, value); }
        }

        #endregion

        #region DDSTSSQIND

        /// <summary>
        /// Gets or Sets Value of DDSTSSQIND for Table Object
        /// </summary>
        /// <value>The Value of DDSTSSQIND Column from Table F40R11</value>
        public virtual string DDSTSSQIND
        {
            get { return (Convert.ToString(Columns.DDSTSSQIND.FormatColumn(this["DDSTSSQIND"]))); }
            set { Where(Columns.DDSTSSQIND, Condition.EqualTo, value); }
        }

        #endregion

        #region DDEDTCTDJ

        /// <summary>
        /// Gets or Sets Value of DDEDTCTDJ for Table Object
        /// </summary>
        /// <value>The Value of DDEDTCTDJ Column from Table F40R11</value>
        public virtual decimal DDEDTCTDJ
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDEDTCTDJ.FormatColumn(this["DDEDTCTDJ"]))); }
            set { Where(Columns.DDEDTCTDJ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDEDTCTTM

        /// <summary>
        /// Gets or Sets Value of DDEDTCTTM for Table Object
        /// </summary>
        /// <value>The Value of DDEDTCTTM Column from Table F40R11</value>
        public virtual double DDEDTCTTM
        {
            get { return double.Parse(Convert.ToString(Columns.DDEDTCTTM.FormatColumn(this["DDEDTCTTM"]))); }
            set { Where(Columns.DDEDTCTTM, Condition.EqualTo, value); }
        }

        #endregion

        #region DDOGDMDQTY

        /// <summary>
        /// Gets or Sets Value of DDOGDMDQTY for Table Object
        /// </summary>
        /// <value>The Value of DDOGDMDQTY Column from Table F40R11</value>
        public virtual double DDOGDMDQTY
        {
            get { return double.Parse(Convert.ToString(Columns.DDOGDMDQTY.FormatColumn(this["DDOGDMDQTY"]))); }
            set { Where(Columns.DDOGDMDQTY, Condition.EqualTo, value); }
        }

        #endregion

        #region DDEDTMSGNM

        /// <summary>
        /// Gets or Sets Value of DDEDTMSGNM for Table Object
        /// </summary>
        /// <value>The Value of DDEDTMSGNM Column from Table F40R11</value>
        public virtual string DDEDTMSGNM
        {
            get { return (Convert.ToString(Columns.DDEDTMSGNM.FormatColumn(this["DDEDTMSGNM"]))); }
            set { Where(Columns.DDEDTMSGNM, Condition.EqualTo, value); }
        }

        #endregion

        #region DDEDTMSGSB

        /// <summary>
        /// Gets or Sets Value of DDEDTMSGSB for Table Object
        /// </summary>
        /// <value>The Value of DDEDTMSGSB Column from Table F40R11</value>
        public virtual string DDEDTMSGSB
        {
            get { return (Convert.ToString(Columns.DDEDTMSGSB.FormatColumn(this["DDEDTMSGSB"]))); }
            set { Where(Columns.DDEDTMSGSB, Condition.EqualTo, value); }
        }

        #endregion

        #region DDEDTMSGTP

        /// <summary>
        /// Gets or Sets Value of DDEDTMSGTP for Table Object
        /// </summary>
        /// <value>The Value of DDEDTMSGTP Column from Table F40R11</value>
        public virtual string DDEDTMSGTP
        {
            get { return (Convert.ToString(Columns.DDEDTMSGTP.FormatColumn(this["DDEDTMSGTP"]))); }
            set { Where(Columns.DDEDTMSGTP, Condition.EqualTo, value); }
        }

        #endregion

        #region DDTRSRTE

        /// <summary>
        /// Gets or Sets Value of DDTRSRTE for Table Object
        /// </summary>
        /// <value>The Value of DDTRSRTE Column from Table F40R11</value>
        public virtual string DDTRSRTE
        {
            get { return (Convert.ToString(Columns.DDTRSRTE.FormatColumn(this["DDTRSRTE"]))); }
            set { Where(Columns.DDTRSRTE, Condition.EqualTo, value); }
        }

        #endregion

        #region DDTRSRTSFX

        /// <summary>
        /// Gets or Sets Value of DDTRSRTSFX for Table Object
        /// </summary>
        /// <value>The Value of DDTRSRTSFX Column from Table F40R11</value>
        public virtual string DDTRSRTSFX
        {
            get { return (Convert.ToString(Columns.DDTRSRTSFX.FormatColumn(this["DDTRSRTSFX"]))); }
            set { Where(Columns.DDTRSRTSFX, Condition.EqualTo, value); }
        }

        #endregion

        #region DDUCARTNO

        /// <summary>
        /// Gets or Sets Value of DDUCARTNO for Table Object
        /// </summary>
        /// <value>The Value of DDUCARTNO Column from Table F40R11</value>
        public virtual string DDUCARTNO
        {
            get { return (Convert.ToString(Columns.DDUCARTNO.FormatColumn(this["DDUCARTNO"]))); }
            set { Where(Columns.DDUCARTNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDVINNO

        /// <summary>
        /// Gets or Sets Value of DDVINNO for Table Object
        /// </summary>
        /// <value>The Value of DDVINNO Column from Table F40R11</value>
        public virtual string DDVINNO
        {
            get { return (Convert.ToString(Columns.DDVINNO.FormatColumn(this["DDVINNO"]))); }
            set { Where(Columns.DDVINNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDPENDDLT

        /// <summary>
        /// Gets or Sets Value of DDPENDDLT for Table Object
        /// </summary>
        /// <value>The Value of DDPENDDLT Column from Table F40R11</value>
        public virtual string DDPENDDLT
        {
            get { return (Convert.ToString(Columns.DDPENDDLT.FormatColumn(this["DDPENDDLT"]))); }
            set { Where(Columns.DDPENDDLT, Condition.EqualTo, value); }
        }

        #endregion

        #region DDSDID

        /// <summary>
        /// Gets or Sets Value of DDSDID for Table Object
        /// </summary>
        /// <value>The Value of DDSDID Column from Table F40R11</value>
        public virtual double DDSDID
        {
            get { return double.Parse(Convert.ToString(Columns.DDSDID.FormatColumn(this["DDSDID"]))); }
            set { Where(Columns.DDSDID, Condition.EqualTo, value); }
        }

        #endregion

        #region DDNETVC

        /// <summary>
        /// Gets or Sets Value of DDNETVC for Table Object
        /// </summary>
        /// <value>The Value of DDNETVC Column from Table F40R11</value>
        public virtual double DDNETVC
        {
            get { return double.Parse(Convert.ToString(Columns.DDNETVC.FormatColumn(this["DDNETVC"]))); }
            set { Where(Columns.DDNETVC, Condition.EqualTo, value); }
        }

        #endregion

        #region DDNETVCNTF

        /// <summary>
        /// Gets or Sets Value of DDNETVCNTF for Table Object
        /// </summary>
        /// <value>The Value of DDNETVCNTF Column from Table F40R11</value>
        public virtual string DDNETVCNTF
        {
            get { return (Convert.ToString(Columns.DDNETVCNTF.FormatColumn(this["DDNETVCNTF"]))); }
            set { Where(Columns.DDNETVCNTF, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCRTCLID

        /// <summary>
        /// Gets or Sets Value of DDCRTCLID for Table Object
        /// </summary>
        /// <value>The Value of DDCRTCLID Column from Table F40R11</value>
        public virtual string DDCRTCLID
        {
            get { return (Convert.ToString(Columns.DDCRTCLID.FormatColumn(this["DDCRTCLID"]))); }
            set { Where(Columns.DDCRTCLID, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMD06

        /// <summary>
        /// Gets or Sets Value of DDDMD06 for Table Object
        /// </summary>
        /// <value>The Value of DDDMD06 Column from Table F40R11</value>
        public virtual string DDDMD06
        {
            get { return (Convert.ToString(Columns.DDDMD06.FormatColumn(this["DDDMD06"]))); }
            set { Where(Columns.DDDMD06, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMD07

        /// <summary>
        /// Gets or Sets Value of DDDMD07 for Table Object
        /// </summary>
        /// <value>The Value of DDDMD07 Column from Table F40R11</value>
        public virtual string DDDMD07
        {
            get { return (Convert.ToString(Columns.DDDMD07.FormatColumn(this["DDDMD07"]))); }
            set { Where(Columns.DDDMD07, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMD08

        /// <summary>
        /// Gets or Sets Value of DDDMD08 for Table Object
        /// </summary>
        /// <value>The Value of DDDMD08 Column from Table F40R11</value>
        public virtual string DDDMD08
        {
            get { return (Convert.ToString(Columns.DDDMD08.FormatColumn(this["DDDMD08"]))); }
            set { Where(Columns.DDDMD08, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMD09

        /// <summary>
        /// Gets or Sets Value of DDDMD09 for Table Object
        /// </summary>
        /// <value>The Value of DDDMD09 Column from Table F40R11</value>
        public virtual string DDDMD09
        {
            get { return (Convert.ToString(Columns.DDDMD09.FormatColumn(this["DDDMD09"]))); }
            set { Where(Columns.DDDMD09, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMD10

        /// <summary>
        /// Gets or Sets Value of DDDMD10 for Table Object
        /// </summary>
        /// <value>The Value of DDDMD10 Column from Table F40R11</value>
        public virtual string DDDMD10
        {
            get { return (Convert.ToString(Columns.DDDMD10.FormatColumn(this["DDDMD10"]))); }
            set { Where(Columns.DDDMD10, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDREF1

        /// <summary>
        /// Gets or Sets Value of DDDMDREF1 for Table Object
        /// </summary>
        /// <value>The Value of DDDMDREF1 Column from Table F40R11</value>
        public virtual string DDDMDREF1
        {
            get { return (Convert.ToString(Columns.DDDMDREF1.FormatColumn(this["DDDMDREF1"]))); }
            set { Where(Columns.DDDMDREF1, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDREF2

        /// <summary>
        /// Gets or Sets Value of DDDMDREF2 for Table Object
        /// </summary>
        /// <value>The Value of DDDMDREF2 Column from Table F40R11</value>
        public virtual string DDDMDREF2
        {
            get { return (Convert.ToString(Columns.DDDMDREF2.FormatColumn(this["DDDMDREF2"]))); }
            set { Where(Columns.DDDMDREF2, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCTLNUM

        /// <summary>
        /// Gets or Sets Value of DDCTLNUM for Table Object
        /// </summary>
        /// <value>The Value of DDCTLNUM Column from Table F40R11</value>
        public virtual string DDCTLNUM
        {
            get { return (Convert.ToString(Columns.DDCTLNUM.FormatColumn(this["DDCTLNUM"]))); }
            set { Where(Columns.DDCTLNUM, Condition.EqualTo, value); }
        }

        #endregion

        #region DDURCD

        /// <summary>
        /// Gets or Sets Value of DDURCD for Table Object
        /// </summary>
        /// <value>The Value of DDURCD Column from Table F40R11</value>
        public virtual string DDURCD
        {
            get { return (Convert.ToString(Columns.DDURCD.FormatColumn(this["DDURCD"]))); }
            set { Where(Columns.DDURCD, Condition.EqualTo, value); }
        }

        #endregion

        #region DDURDT

        /// <summary>
        /// Gets or Sets Value of DDURDT for Table Object
        /// </summary>
        /// <value>The Value of DDURDT Column from Table F40R11</value>
        public virtual decimal DDURDT
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDURDT.FormatColumn(this["DDURDT"]))); }
            set { Where(Columns.DDURDT, Condition.EqualTo, value); }
        }

        #endregion

        #region DDURAT

        /// <summary>
        /// Gets or Sets Value of DDURAT for Table Object
        /// </summary>
        /// <value>The Value of DDURAT Column from Table F40R11</value>
        public virtual double DDURAT
        {
            get { return double.Parse(Convert.ToString(Columns.DDURAT.FormatColumn(this["DDURAT"]))); }
            set { Where(Columns.DDURAT, Condition.EqualTo, value); }
        }

        #endregion

        #region DDURAB

        /// <summary>
        /// Gets or Sets Value of DDURAB for Table Object
        /// </summary>
        /// <value>The Value of DDURAB Column from Table F40R11</value>
        public virtual double DDURAB
        {
            get { return double.Parse(Convert.ToString(Columns.DDURAB.FormatColumn(this["DDURAB"]))); }
            set { Where(Columns.DDURAB, Condition.EqualTo, value); }
        }

        #endregion

        #region DDURRF

        /// <summary>
        /// Gets or Sets Value of DDURRF for Table Object
        /// </summary>
        /// <value>The Value of DDURRF Column from Table F40R11</value>
        public virtual string DDURRF
        {
            get { return (Convert.ToString(Columns.DDURRF.FormatColumn(this["DDURRF"]))); }
            set { Where(Columns.DDURRF, Condition.EqualTo, value); }
        }

        #endregion

        #region DDUSER

        /// <summary>
        /// Gets or Sets Value of DDUSER for Table Object
        /// </summary>
        /// <value>The Value of DDUSER Column from Table F40R11</value>
        public virtual string DDUSER
        {
            get { return (Convert.ToString(Columns.DDUSER.FormatColumn(this["DDUSER"]))); }
            set { Where(Columns.DDUSER, Condition.EqualTo, value); }
        }

        #endregion

        #region DDPID

        /// <summary>
        /// Gets or Sets Value of DDPID for Table Object
        /// </summary>
        /// <value>The Value of DDPID Column from Table F40R11</value>
        public virtual string DDPID
        {
            get { return (Convert.ToString(Columns.DDPID.FormatColumn(this["DDPID"]))); }
            set { Where(Columns.DDPID, Condition.EqualTo, value); }
        }

        #endregion

        #region DDJOBN

        /// <summary>
        /// Gets or Sets Value of DDJOBN for Table Object
        /// </summary>
        /// <value>The Value of DDJOBN Column from Table F40R11</value>
        public virtual string DDJOBN
        {
            get { return (Convert.ToString(Columns.DDJOBN.FormatColumn(this["DDJOBN"]))); }
            set { Where(Columns.DDJOBN, Condition.EqualTo, value); }
        }

        #endregion

        #region DDUPMJ

        /// <summary>
        /// Gets or Sets Value of DDUPMJ for Table Object
        /// </summary>
        /// <value>The Value of DDUPMJ Column from Table F40R11</value>
        public virtual decimal DDUPMJ
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDUPMJ.FormatColumn(this["DDUPMJ"]))); }
            set { Where(Columns.DDUPMJ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDTDAY

        /// <summary>
        /// Gets or Sets Value of DDTDAY for Table Object
        /// </summary>
        /// <value>The Value of DDTDAY Column from Table F40R11</value>
        public virtual double DDTDAY
        {
            get { return double.Parse(Convert.ToString(Columns.DDTDAY.FormatColumn(this["DDTDAY"]))); }
            set { Where(Columns.DDTDAY, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCARDNO

        /// <summary>
        /// Gets or Sets Value of DDCARDNO for Table Object
        /// </summary>
        /// <value>The Value of DDCARDNO Column from Table F40R11</value>
        public virtual string DDCARDNO
        {
            get { return (Convert.ToString(Columns.DDCARDNO.FormatColumn(this["DDCARDNO"]))); }
            set { Where(Columns.DDCARDNO, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDELBATCH

        /// <summary>
        /// Gets or Sets Value of DDDELBATCH for Table Object
        /// </summary>
        /// <value>The Value of DDDELBATCH Column from Table F40R11</value>
        public virtual string DDDELBATCH
        {
            get { return (Convert.ToString(Columns.DDDELBATCH.FormatColumn(this["DDDELBATCH"]))); }
            set { Where(Columns.DDDELBATCH, Condition.EqualTo, value); }
        }

        #endregion

        #region DDCIRVQ

        /// <summary>
        /// Gets or Sets Value of DDCIRVQ for Table Object
        /// </summary>
        /// <value>The Value of DDCIRVQ Column from Table F40R11</value>
        public virtual string DDCIRVQ
        {
            get { return (Convert.ToString(Columns.DDCIRVQ.FormatColumn(this["DDCIRVQ"]))); }
            set { Where(Columns.DDCIRVQ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDPRODTYPE

        /// <summary>
        /// Gets or Sets Value of DDPRODTYPE for Table Object
        /// </summary>
        /// <value>The Value of DDPRODTYPE Column from Table F40R11</value>
        public virtual string DDPRODTYPE
        {
            get { return (Convert.ToString(Columns.DDPRODTYPE.FormatColumn(this["DDPRODTYPE"]))); }
            set { Where(Columns.DDPRODTYPE, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRTORSQ

        /// <summary>
        /// Gets or Sets Value of DDRTORSQ for Table Object
        /// </summary>
        /// <value>The Value of DDRTORSQ Column from Table F40R11</value>
        public virtual string DDRTORSQ
        {
            get { return (Convert.ToString(Columns.DDRTORSQ.FormatColumn(this["DDRTORSQ"]))); }
            set { Where(Columns.DDRTORSQ, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRTGDAT

        /// <summary>
        /// Gets or Sets Value of DDRTGDAT for Table Object
        /// </summary>
        /// <value>The Value of DDRTGDAT Column from Table F40R11</value>
        public virtual decimal DDRTGDAT
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDRTGDAT.FormatColumn(this["DDRTGDAT"]))); }
            set { Where(Columns.DDRTGDAT, Condition.EqualTo, value); }
        }

        #endregion

        #region DDRTGTIM

        /// <summary>
        /// Gets or Sets Value of DDRTGTIM for Table Object
        /// </summary>
        /// <value>The Value of DDRTGTIM Column from Table F40R11</value>
        public virtual double DDRTGTIM
        {
            get { return double.Parse(Convert.ToString(Columns.DDRTGTIM.FormatColumn(this["DDRTGTIM"]))); }
            set { Where(Columns.DDRTGTIM, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDREF3

        /// <summary>
        /// Gets or Sets Value of DDDMDREF3 for Table Object
        /// </summary>
        /// <value>The Value of DDDMDREF3 Column from Table F40R11</value>
        public virtual string DDDMDREF3
        {
            get { return (Convert.ToString(Columns.DDDMDREF3.FormatColumn(this["DDDMDREF3"]))); }
            set { Where(Columns.DDDMDREF3, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDREF4

        /// <summary>
        /// Gets or Sets Value of DDDMDREF4 for Table Object
        /// </summary>
        /// <value>The Value of DDDMDREF4 Column from Table F40R11</value>
        public virtual string DDDMDREF4
        {
            get { return (Convert.ToString(Columns.DDDMDREF4.FormatColumn(this["DDDMDREF4"]))); }
            set { Where(Columns.DDDMDREF4, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDREF5

        /// <summary>
        /// Gets or Sets Value of DDDMDREF5 for Table Object
        /// </summary>
        /// <value>The Value of DDDMDREF5 Column from Table F40R11</value>
        public virtual string DDDMDREF5
        {
            get { return (Convert.ToString(Columns.DDDMDREF5.FormatColumn(this["DDDMDREF5"]))); }
            set { Where(Columns.DDDMDREF5, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDREF6

        /// <summary>
        /// Gets or Sets Value of DDDMDREF6 for Table Object
        /// </summary>
        /// <value>The Value of DDDMDREF6 Column from Table F40R11</value>
        public virtual string DDDMDREF6
        {
            get { return (Convert.ToString(Columns.DDDMDREF6.FormatColumn(this["DDDMDREF6"]))); }
            set { Where(Columns.DDDMDREF6, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDREF7

        /// <summary>
        /// Gets or Sets Value of DDDMDREF7 for Table Object
        /// </summary>
        /// <value>The Value of DDDMDREF7 Column from Table F40R11</value>
        public virtual string DDDMDREF7
        {
            get { return (Convert.ToString(Columns.DDDMDREF7.FormatColumn(this["DDDMDREF7"]))); }
            set { Where(Columns.DDDMDREF7, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDREF8

        /// <summary>
        /// Gets or Sets Value of DDDMDREF8 for Table Object
        /// </summary>
        /// <value>The Value of DDDMDREF8 Column from Table F40R11</value>
        public virtual string DDDMDREF8
        {
            get { return (Convert.ToString(Columns.DDDMDREF8.FormatColumn(this["DDDMDREF8"]))); }
            set { Where(Columns.DDDMDREF8, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDREF9

        /// <summary>
        /// Gets or Sets Value of DDDMDREF9 for Table Object
        /// </summary>
        /// <value>The Value of DDDMDREF9 Column from Table F40R11</value>
        public virtual string DDDMDREF9
        {
            get { return (Convert.ToString(Columns.DDDMDREF9.FormatColumn(this["DDDMDREF9"]))); }
            set { Where(Columns.DDDMDREF9, Condition.EqualTo, value); }
        }

        #endregion

        #region DDDMDREF10

        /// <summary>
        /// Gets or Sets Value of DDDMDREF10 for Table Object
        /// </summary>
        /// <value>The Value of DDDMDREF10 Column from Table F40R11</value>
        public virtual string DDDMDREF10
        {
            get { return (Convert.ToString(Columns.DDDMDREF10.FormatColumn(this["DDDMDREF10"]))); }
            set { Where(Columns.DDDMDREF10, Condition.EqualTo, value); }
        }

        #endregion

        #region DDSCAC

        /// <summary>
        /// Gets or Sets Value of DDSCAC for Table Object
        /// </summary>
        /// <value>The Value of DDSCAC Column from Table F40R11</value>
        public virtual string DDSCAC
        {
            get { return (Convert.ToString(Columns.DDSCAC.FormatColumn(this["DDSCAC"]))); }
            set { Where(Columns.DDSCAC, Condition.EqualTo, value); }
        }

        #endregion

        #region DDATMCN

        /// <summary>
        /// Gets or Sets Value of DDATMCN for Table Object
        /// </summary>
        /// <value>The Value of DDATMCN Column from Table F40R11</value>
        public virtual double DDATMCN
        {
            get { return double.Parse(Convert.ToString(Columns.DDATMCN.FormatColumn(this["DDATMCN"]))); }
            set { Where(Columns.DDATMCN, Condition.EqualTo, value); }
        }

        #endregion

        #region DDOFFLNDT

        /// <summary>
        /// Gets or Sets Value of DDOFFLNDT for Table Object
        /// </summary>
        /// <value>The Value of DDOFFLNDT Column from Table F40R11</value>
        public virtual decimal DDOFFLNDT
        {
            get { return decimal.Parse(Convert.ToString(Columns.DDOFFLNDT.FormatColumn(this["DDOFFLNDT"]))); }
            set { Where(Columns.DDOFFLNDT, Condition.EqualTo, value); }
        }

        #endregion

        #region DDENGTAG1

        /// <summary>
        /// Gets or Sets Value of DDENGTAG1 for Table Object
        /// </summary>
        /// <value>The Value of DDENGTAG1 Column from Table F40R11</value>
        public virtual string DDENGTAG1
        {
            get { return (Convert.ToString(Columns.DDENGTAG1.FormatColumn(this["DDENGTAG1"]))); }
            set { Where(Columns.DDENGTAG1, Condition.EqualTo, value); }
        }

        #endregion

        #region DDENGTAGR1

        /// <summary>
        /// Gets or Sets Value of DDENGTAGR1 for Table Object
        /// </summary>
        /// <value>The Value of DDENGTAGR1 Column from Table F40R11</value>
        public virtual string DDENGTAGR1
        {
            get { return (Convert.ToString(Columns.DDENGTAGR1.FormatColumn(this["DDENGTAGR1"]))); }
            set { Where(Columns.DDENGTAGR1, Condition.EqualTo, value); }
        }

        #endregion

        #region DDENGTAG2

        /// <summary>
        /// Gets or Sets Value of DDENGTAG2 for Table Object
        /// </summary>
        /// <value>The Value of DDENGTAG2 Column from Table F40R11</value>
        public virtual string DDENGTAG2
        {
            get { return (Convert.ToString(Columns.DDENGTAG2.FormatColumn(this["DDENGTAG2"]))); }
            set { Where(Columns.DDENGTAG2, Condition.EqualTo, value); }
        }

        #endregion

        #region DDENGTAGR2

        /// <summary>
        /// Gets or Sets Value of DDENGTAGR2 for Table Object
        /// </summary>
        /// <value>The Value of DDENGTAGR2 Column from Table F40R11</value>
        public virtual string DDENGTAGR2
        {
            get { return (Convert.ToString(Columns.DDENGTAGR2.FormatColumn(this["DDENGTAGR2"]))); }
            set { Where(Columns.DDENGTAGR2, Condition.EqualTo, value); }
        }

        #endregion

        #region GetEnumerator

        /// <summary>
        /// Returns the IEnumerator Object for Fetching Multiple Records from F40R11
        /// </summary>
        /// <returns>The enumerator object for F40R11_Collection</returns>
        public override IEnumerator GetEnumerator()
        {
            return new F40R11_Collection(this);
        }

        #endregion
    }

    #endregion

    #region F40R11Columns

    /// <summary>
    /// Base Class for Column Class for Query Construction
    /// </summary>
    public class F40R11Columns : ITableColumns
    {
        /// <summary>
        /// Query object for column DDUKID in Table.
        /// </summary>
        public Query DDUKID;

        /// <summary>
        /// Query object for column DDDEID in Table.
        /// </summary>
        public Query DDDEID;

        /// <summary>
        /// Query object for column DDMCU in Table.
        /// </summary>
        public Query DDMCU;

        /// <summary>
        /// Query object for column DDAN8 in Table.
        /// </summary>
        public Query DDAN8;

        /// <summary>
        /// Query object for column DDSHAN in Table.
        /// </summary>
        public Query DDSHAN;

        /// <summary>
        /// Query object for column DDDVAN in Table.
        /// </summary>
        public Query DDDVAN;

        /// <summary>
        /// Query object for column DDITM in Table.
        /// </summary>
        public Query DDITM;

        /// <summary>
        /// Query object for column DDDETYP in Table.
        /// </summary>
        public Query DDDETYP;

        /// <summary>
        /// Query object for column DDDEPER in Table.
        /// </summary>
        public Query DDDEPER;

        /// <summary>
        /// Query object for column DDCRTSOSTS in Table.
        /// </summary>
        public Query DDCRTSOSTS;

        /// <summary>
        /// Query object for column DDRLSNO in Table.
        /// </summary>
        public Query DDRLSNO;

        /// <summary>
        /// Query object for column DDRLSDJ in Table.
        /// </summary>
        public Query DDRLSDJ;

        /// <summary>
        /// Query object for column DDRLSTM in Table.
        /// </summary>
        public Query DDRLSTM;

        /// <summary>
        /// Query object for column DDRQSJ in Table.
        /// </summary>
        public Query DDRQSJ;

        /// <summary>
        /// Query object for column DDERQSHPDJ in Table.
        /// </summary>
        public Query DDERQSHPDJ;

        /// <summary>
        /// Query object for column DDRSHT in Table.
        /// </summary>
        public Query DDRSHT;

        /// <summary>
        /// Query object for column DDRQSHPQTY in Table.
        /// </summary>
        public Query DDRQSHPQTY;

        /// <summary>
        /// Query object for column DDUOM in Table.
        /// </summary>
        public Query DDUOM;

        /// <summary>
        /// Query object for column DDRSDJ in Table.
        /// </summary>
        public Query DDRSDJ;

        /// <summary>
        /// Query object for column DDRSDT in Table.
        /// </summary>
        public Query DDRSDT;

        /// <summary>
        /// Query object for column DDHZED in Table.
        /// </summary>
        public Query DDHZED;

        /// <summary>
        /// Query object for column DDHZSD in Table.
        /// </summary>
        public Query DDHZSD;

        /// <summary>
        /// Query object for column DDASBLNFD in Table.
        /// </summary>
        public Query DDASBLNFD;

        /// <summary>
        /// Query object for column DDASBLNLOC in Table.
        /// </summary>
        public Query DDASBLNLOC;

        /// <summary>
        /// Query object for column DDASBNO in Table.
        /// </summary>
        public Query DDASBNO;

        /// <summary>
        /// Query object for column DDDMDBTHNO in Table.
        /// </summary>
        public Query DDDMDBTHNO;

        /// <summary>
        /// Query object for column DDCSGSHPNO in Table.
        /// </summary>
        public Query DDCSGSHPNO;

        /// <summary>
        /// Query object for column DDGCNT in Table.
        /// </summary>
        public Query DDGCNT;

        /// <summary>
        /// Query object for column DDCUMID in Table.
        /// </summary>
        public Query DDCUMID;

        /// <summary>
        /// Query object for column DDTXTREFNO in Table.
        /// </summary>
        public Query DDTXTREFNO;

        /// <summary>
        /// Query object for column DDREQFRQ in Table.
        /// </summary>
        public Query DDREQFRQ;

        /// <summary>
        /// Query object for column DDCSTDMDPT in Table.
        /// </summary>
        public Query DDCSTDMDPT;

        /// <summary>
        /// Query object for column DDREQTYP in Table.
        /// </summary>
        public Query DDREQTYP;

        /// <summary>
        /// Query object for column DDCSTORDNO in Table.
        /// </summary>
        public Query DDCSTORDNO;

        /// <summary>
        /// Query object for column DDDKID in Table.
        /// </summary>
        public Query DDDKID;

        /// <summary>
        /// Query object for column DDENDJOBSQ in Table.
        /// </summary>
        public Query DDENDJOBSQ;

        /// <summary>
        /// Query object for column DDPSJOBNO in Table.
        /// </summary>
        public Query DDPSJOBNO;

        /// <summary>
        /// Query object for column DDHZCLSNO in Table.
        /// </summary>
        public Query DDHZCLSNO;

        /// <summary>
        /// Query object for column DDJOBSQ in Table.
        /// </summary>
        public Query DDJOBSQ;

        /// <summary>
        /// Query object for column DDKBSNO in Table.
        /// </summary>
        public Query DDKBSNO;

        /// <summary>
        /// Query object for column DDKBENO in Table.
        /// </summary>
        public Query DDKBENO;

        /// <summary>
        /// Query object for column DDLABORHR in Table.
        /// </summary>
        public Query DDLABORHR;

        /// <summary>
        /// Query object for column DDCIRV in Table.
        /// </summary>
        public Query DDCIRV;

        /// <summary>
        /// Query object for column DDYEAR1 in Table.
        /// </summary>
        public Query DDYEAR1;

        /// <summary>
        /// Query object for column DDPTRLSSTS in Table.
        /// </summary>
        public Query DDPTRLSSTS;

        /// <summary>
        /// Query object for column DDCSTPODJ in Table.
        /// </summary>
        public Query DDCSTPODJ;

        /// <summary>
        /// Query object for column DDCSTPOTM in Table.
        /// </summary>
        public Query DDCSTPOTM;

        /// <summary>
        /// Query object for column DDCSTPOLIN in Table.
        /// </summary>
        public Query DDCSTPOLIN;

        /// <summary>
        /// Query object for column DDCSTPO in Table.
        /// </summary>
        public Query DDCSTPO;

        /// <summary>
        /// Query object for column DDTPUR in Table.
        /// </summary>
        public Query DDTPUR;

        /// <summary>
        /// Query object for column DDQTYBYPOS in Table.
        /// </summary>
        public Query DDQTYBYPOS;

        /// <summary>
        /// Query object for column DDSOCN in Table.
        /// </summary>
        public Query DDSOCN;

        /// <summary>
        /// Query object for column DDPSIG in Table.
        /// </summary>
        public Query DDPSIG;

        /// <summary>
        /// Query object for column DDRSVLNFD in Table.
        /// </summary>
        public Query DDRSVLNFD;

        /// <summary>
        /// Query object for column DDSTSSQIND in Table.
        /// </summary>
        public Query DDSTSSQIND;

        /// <summary>
        /// Query object for column DDEDTCTDJ in Table.
        /// </summary>
        public Query DDEDTCTDJ;

        /// <summary>
        /// Query object for column DDEDTCTTM in Table.
        /// </summary>
        public Query DDEDTCTTM;

        /// <summary>
        /// Query object for column DDOGDMDQTY in Table.
        /// </summary>
        public Query DDOGDMDQTY;

        /// <summary>
        /// Query object for column DDEDTMSGNM in Table.
        /// </summary>
        public Query DDEDTMSGNM;

        /// <summary>
        /// Query object for column DDEDTMSGSB in Table.
        /// </summary>
        public Query DDEDTMSGSB;

        /// <summary>
        /// Query object for column DDEDTMSGTP in Table.
        /// </summary>
        public Query DDEDTMSGTP;

        /// <summary>
        /// Query object for column DDTRSRTE in Table.
        /// </summary>
        public Query DDTRSRTE;

        /// <summary>
        /// Query object for column DDTRSRTSFX in Table.
        /// </summary>
        public Query DDTRSRTSFX;

        /// <summary>
        /// Query object for column DDUCARTNO in Table.
        /// </summary>
        public Query DDUCARTNO;

        /// <summary>
        /// Query object for column DDVINNO in Table.
        /// </summary>
        public Query DDVINNO;

        /// <summary>
        /// Query object for column DDPENDDLT in Table.
        /// </summary>
        public Query DDPENDDLT;

        /// <summary>
        /// Query object for column DDSDID in Table.
        /// </summary>
        public Query DDSDID;

        /// <summary>
        /// Query object for column DDNETVC in Table.
        /// </summary>
        public Query DDNETVC;

        /// <summary>
        /// Query object for column DDNETVCNTF in Table.
        /// </summary>
        public Query DDNETVCNTF;

        /// <summary>
        /// Query object for column DDCRTCLID in Table.
        /// </summary>
        public Query DDCRTCLID;

        /// <summary>
        /// Query object for column DDDMD06 in Table.
        /// </summary>
        public Query DDDMD06;

        /// <summary>
        /// Query object for column DDDMD07 in Table.
        /// </summary>
        public Query DDDMD07;

        /// <summary>
        /// Query object for column DDDMD08 in Table.
        /// </summary>
        public Query DDDMD08;

        /// <summary>
        /// Query object for column DDDMD09 in Table.
        /// </summary>
        public Query DDDMD09;

        /// <summary>
        /// Query object for column DDDMD10 in Table.
        /// </summary>
        public Query DDDMD10;

        /// <summary>
        /// Query object for column DDDMDREF1 in Table.
        /// </summary>
        public Query DDDMDREF1;

        /// <summary>
        /// Query object for column DDDMDREF2 in Table.
        /// </summary>
        public Query DDDMDREF2;

        /// <summary>
        /// Query object for column DDCTLNUM in Table.
        /// </summary>
        public Query DDCTLNUM;

        /// <summary>
        /// Query object for column DDURCD in Table.
        /// </summary>
        public Query DDURCD;

        /// <summary>
        /// Query object for column DDURDT in Table.
        /// </summary>
        public Query DDURDT;

        /// <summary>
        /// Query object for column DDURAT in Table.
        /// </summary>
        public Query DDURAT;

        /// <summary>
        /// Query object for column DDURAB in Table.
        /// </summary>
        public Query DDURAB;

        /// <summary>
        /// Query object for column DDURRF in Table.
        /// </summary>
        public Query DDURRF;

        /// <summary>
        /// Query object for column DDUSER in Table.
        /// </summary>
        public Query DDUSER;

        /// <summary>
        /// Query object for column DDPID in Table.
        /// </summary>
        public Query DDPID;

        /// <summary>
        /// Query object for column DDJOBN in Table.
        /// </summary>
        public Query DDJOBN;

        /// <summary>
        /// Query object for column DDUPMJ in Table.
        /// </summary>
        public Query DDUPMJ;

        /// <summary>
        /// Query object for column DDTDAY in Table.
        /// </summary>
        public Query DDTDAY;

        /// <summary>
        /// Query object for column DDCARDNO in Table.
        /// </summary>
        public Query DDCARDNO;

        /// <summary>
        /// Query object for column DDDELBATCH in Table.
        /// </summary>
        public Query DDDELBATCH;

        /// <summary>
        /// Query object for column DDCIRVQ in Table.
        /// </summary>
        public Query DDCIRVQ;

        /// <summary>
        /// Query object for column DDPRODTYPE in Table.
        /// </summary>
        public Query DDPRODTYPE;

        /// <summary>
        /// Query object for column DDRTORSQ in Table.
        /// </summary>
        public Query DDRTORSQ;

        /// <summary>
        /// Query object for column DDRTGDAT in Table.
        /// </summary>
        public Query DDRTGDAT;

        /// <summary>
        /// Query object for column DDRTGTIM in Table.
        /// </summary>
        public Query DDRTGTIM;

        /// <summary>
        /// Query object for column DDDMDREF3 in Table.
        /// </summary>
        public Query DDDMDREF3;

        /// <summary>
        /// Query object for column DDDMDREF4 in Table.
        /// </summary>
        public Query DDDMDREF4;

        /// <summary>
        /// Query object for column DDDMDREF5 in Table.
        /// </summary>
        public Query DDDMDREF5;

        /// <summary>
        /// Query object for column DDDMDREF6 in Table.
        /// </summary>
        public Query DDDMDREF6;

        /// <summary>
        /// Query object for column DDDMDREF7 in Table.
        /// </summary>
        public Query DDDMDREF7;

        /// <summary>
        /// Query object for column DDDMDREF8 in Table.
        /// </summary>
        public Query DDDMDREF8;

        /// <summary>
        /// Query object for column DDDMDREF9 in Table.
        /// </summary>
        public Query DDDMDREF9;

        /// <summary>
        /// Query object for column DDDMDREF10 in Table.
        /// </summary>
        public Query DDDMDREF10;

        /// <summary>
        /// Query object for column DDSCAC in Table.
        /// </summary>
        public Query DDSCAC;

        /// <summary>
        /// Query object for column DDATMCN in Table.
        /// </summary>
        public Query DDATMCN;

        /// <summary>
        /// Query object for column DDOFFLNDT in Table.
        /// </summary>
        public Query DDOFFLNDT;

        /// <summary>
        /// Query object for column DDENGTAG1 in Table.
        /// </summary>
        public Query DDENGTAG1;

        /// <summary>
        /// Query object for column DDENGTAGR1 in Table.
        /// </summary>
        public Query DDENGTAGR1;

        /// <summary>
        /// Query object for column DDENGTAG2 in Table.
        /// </summary>
        public Query DDENGTAG2;

        /// <summary>
        /// Query object for column DDENGTAGR2 in Table.
        /// </summary>
        public Query DDENGTAGR2;

        #region .ctor Constructor

        /// <summary>
        /// Creates Query Objects for all Columns for F40R11
        /// </summary>
        /// <param name="DataManipObject">Data Manipulator for columns.</param>
        /// <param name="ColumnsTable">DataTable columns will be added to.</param>
        /// <param name="TableName">Name of table.</param>
        public F40R11Columns(DataManipulator DataManipObject, DataObjectBase ColumnsTable, string TableName)
        {
            this.DDUKID = new Query("DDUKID", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDDEID = new Query("DDDEID", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDMCU = new Query("DDMCU", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDAN8 = new Query("DDAN8", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDSHAN = new Query("DDSHAN", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDDVAN = new Query("DDDVAN", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDITM = new Query("DDITM", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDDETYP = new Query("DDDETYP", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDEPER = new Query("DDDEPER", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDCRTSOSTS = new Query("DDCRTSOSTS", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDRLSNO = new Query("DDRLSNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDRLSDJ = new Query("DDRLSDJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDRLSTM = new Query("DDRLSTM", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDRQSJ = new Query("DDRQSJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDERQSHPDJ = new Query("DDERQSHPDJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDRSHT = new Query("DDRSHT", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDRQSHPQTY = new Query("DDRQSHPQTY", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDUOM = new Query("DDUOM", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDRSDJ = new Query("DDRSDJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDRSDT = new Query("DDRSDT", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDHZED = new Query("DDHZED", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDHZSD = new Query("DDHZSD", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDASBLNFD = new Query("DDASBLNFD", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDASBLNLOC = new Query("DDASBLNLOC", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDASBNO = new Query("DDASBNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMDBTHNO = new Query("DDDMDBTHNO", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDCSGSHPNO = new Query("DDCSGSHPNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDGCNT = new Query("DDGCNT", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDCUMID = new Query("DDCUMID", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDTXTREFNO = new Query("DDTXTREFNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDREQFRQ = new Query("DDREQFRQ", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDCSTDMDPT = new Query("DDCSTDMDPT", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDREQTYP = new Query("DDREQTYP", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDCSTORDNO = new Query("DDCSTORDNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDKID = new Query("DDDKID", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDENDJOBSQ = new Query("DDENDJOBSQ", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDPSJOBNO = new Query("DDPSJOBNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDHZCLSNO = new Query("DDHZCLSNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDJOBSQ = new Query("DDJOBSQ", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDKBSNO = new Query("DDKBSNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDKBENO = new Query("DDKBENO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDLABORHR = new Query("DDLABORHR", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDCIRV = new Query("DDCIRV", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDYEAR1 = new Query("DDYEAR1", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDPTRLSSTS = new Query("DDPTRLSSTS", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDCSTPODJ = new Query("DDCSTPODJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDCSTPOTM = new Query("DDCSTPOTM", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDCSTPOLIN = new Query("DDCSTPOLIN", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDCSTPO = new Query("DDCSTPO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDTPUR = new Query("DDTPUR", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDQTYBYPOS = new Query("DDQTYBYPOS", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDSOCN = new Query("DDSOCN", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDPSIG = new Query("DDPSIG", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDRSVLNFD = new Query("DDRSVLNFD", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDSTSSQIND = new Query("DDSTSSQIND", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDEDTCTDJ = new Query("DDEDTCTDJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDEDTCTTM = new Query("DDEDTCTTM", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDOGDMDQTY = new Query("DDOGDMDQTY", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDEDTMSGNM = new Query("DDEDTMSGNM", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDEDTMSGSB = new Query("DDEDTMSGSB", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDEDTMSGTP = new Query("DDEDTMSGTP", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDTRSRTE = new Query("DDTRSRTE", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDTRSRTSFX = new Query("DDTRSRTSFX", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDUCARTNO = new Query("DDUCARTNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDVINNO = new Query("DDVINNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDPENDDLT = new Query("DDPENDDLT", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDSDID = new Query("DDSDID", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDNETVC = new Query("DDNETVC", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDNETVCNTF = new Query("DDNETVCNTF", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDCRTCLID = new Query("DDCRTCLID", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMD06 = new Query("DDDMD06", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMD07 = new Query("DDDMD07", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMD08 = new Query("DDDMD08", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMD09 = new Query("DDDMD09", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMD10 = new Query("DDDMD10", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMDREF1 = new Query("DDDMDREF1", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMDREF2 = new Query("DDDMDREF2", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDCTLNUM = new Query("DDCTLNUM", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDURCD = new Query("DDURCD", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDURDT = new Query("DDURDT", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDURAT = new Query("DDURAT", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDURAB = new Query("DDURAB", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDURRF = new Query("DDURRF", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDUSER = new Query("DDUSER", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDPID = new Query("DDPID", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDJOBN = new Query("DDJOBN", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDUPMJ = new Query("DDUPMJ", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDTDAY = new Query("DDTDAY", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDCARDNO = new Query("DDCARDNO", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDELBATCH = new Query("DDDELBATCH", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDCIRVQ = new Query("DDCIRVQ", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDPRODTYPE = new Query("DDPRODTYPE", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDRTORSQ = new Query("DDRTORSQ", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDRTGDAT = new Query("DDRTGDAT", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDRTGTIM = new Query("DDRTGTIM", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDDMDREF3 = new Query("DDDMDREF3", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMDREF4 = new Query("DDDMDREF4", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMDREF5 = new Query("DDDMDREF5", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMDREF6 = new Query("DDDMDREF6", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMDREF7 = new Query("DDDMDREF7", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMDREF8 = new Query("DDDMDREF8", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMDREF9 = new Query("DDDMDREF9", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDDMDREF10 = new Query("DDDMDREF10", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDSCAC = new Query("DDSCAC", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDATMCN = new Query("DDATMCN", TableName, typeof(double), DataManipObject, ColumnsTable);
            this.DDOFFLNDT = new Query("DDOFFLNDT", TableName, typeof(decimal), DataManipObject, ColumnsTable);
            this.DDENGTAG1 = new Query("DDENGTAG1", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDENGTAGR1 = new Query("DDENGTAGR1", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDENGTAG2 = new Query("DDENGTAG2", TableName, typeof(string), DataManipObject, ColumnsTable);
            this.DDENGTAGR2 = new Query("DDENGTAGR2", TableName, typeof(string), DataManipObject, ColumnsTable);
        }

        #endregion

        #region Clear

        /// <summary>
        /// Resets Query Conditions for All Columns
        /// </summary>
        public override void Clear()
        {
            DDUKID.Reset();
            DDDEID.Reset();
            DDMCU.Reset();
            DDAN8.Reset();
            DDSHAN.Reset();
            DDDVAN.Reset();
            DDITM.Reset();
            DDDETYP.Reset();
            DDDEPER.Reset();
            DDCRTSOSTS.Reset();
            DDRLSNO.Reset();
            DDRLSDJ.Reset();
            DDRLSTM.Reset();
            DDRQSJ.Reset();
            DDERQSHPDJ.Reset();
            DDRSHT.Reset();
            DDRQSHPQTY.Reset();
            DDUOM.Reset();
            DDRSDJ.Reset();
            DDRSDT.Reset();
            DDHZED.Reset();
            DDHZSD.Reset();
            DDASBLNFD.Reset();
            DDASBLNLOC.Reset();
            DDASBNO.Reset();
            DDDMDBTHNO.Reset();
            DDCSGSHPNO.Reset();
            DDGCNT.Reset();
            DDCUMID.Reset();
            DDTXTREFNO.Reset();
            DDREQFRQ.Reset();
            DDCSTDMDPT.Reset();
            DDREQTYP.Reset();
            DDCSTORDNO.Reset();
            DDDKID.Reset();
            DDENDJOBSQ.Reset();
            DDPSJOBNO.Reset();
            DDHZCLSNO.Reset();
            DDJOBSQ.Reset();
            DDKBSNO.Reset();
            DDKBENO.Reset();
            DDLABORHR.Reset();
            DDCIRV.Reset();
            DDYEAR1.Reset();
            DDPTRLSSTS.Reset();
            DDCSTPODJ.Reset();
            DDCSTPOTM.Reset();
            DDCSTPOLIN.Reset();
            DDCSTPO.Reset();
            DDTPUR.Reset();
            DDQTYBYPOS.Reset();
            DDSOCN.Reset();
            DDPSIG.Reset();
            DDRSVLNFD.Reset();
            DDSTSSQIND.Reset();
            DDEDTCTDJ.Reset();
            DDEDTCTTM.Reset();
            DDOGDMDQTY.Reset();
            DDEDTMSGNM.Reset();
            DDEDTMSGSB.Reset();
            DDEDTMSGTP.Reset();
            DDTRSRTE.Reset();
            DDTRSRTSFX.Reset();
            DDUCARTNO.Reset();
            DDVINNO.Reset();
            DDPENDDLT.Reset();
            DDSDID.Reset();
            DDNETVC.Reset();
            DDNETVCNTF.Reset();
            DDCRTCLID.Reset();
            DDDMD06.Reset();
            DDDMD07.Reset();
            DDDMD08.Reset();
            DDDMD09.Reset();
            DDDMD10.Reset();
            DDDMDREF1.Reset();
            DDDMDREF2.Reset();
            DDCTLNUM.Reset();
            DDURCD.Reset();
            DDURDT.Reset();
            DDURAT.Reset();
            DDURAB.Reset();
            DDURRF.Reset();
            DDUSER.Reset();
            DDPID.Reset();
            DDJOBN.Reset();
            DDUPMJ.Reset();
            DDTDAY.Reset();
            DDCARDNO.Reset();
            DDDELBATCH.Reset();
            DDCIRVQ.Reset();
            DDPRODTYPE.Reset();
            DDRTORSQ.Reset();
            DDRTGDAT.Reset();
            DDRTGTIM.Reset();
            DDDMDREF3.Reset();
            DDDMDREF4.Reset();
            DDDMDREF5.Reset();
            DDDMDREF6.Reset();
            DDDMDREF7.Reset();
            DDDMDREF8.Reset();
            DDDMDREF9.Reset();
            DDDMDREF10.Reset();
            DDSCAC.Reset();
            DDATMCN.Reset();
            DDOFFLNDT.Reset();
            DDENGTAG1.Reset();
            DDENGTAGR1.Reset();
            DDENGTAG2.Reset();
            DDENGTAGR2.Reset();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes of Query Object Conditions for All Columns
        /// </summary>
        public override void Dispose()
        {
            DDUKID.Dispose();
            DDDEID.Dispose();
            DDMCU.Dispose();
            DDAN8.Dispose();
            DDSHAN.Dispose();
            DDDVAN.Dispose();
            DDITM.Dispose();
            DDDETYP.Dispose();
            DDDEPER.Dispose();
            DDCRTSOSTS.Dispose();
            DDRLSNO.Dispose();
            DDRLSDJ.Dispose();
            DDRLSTM.Dispose();
            DDRQSJ.Dispose();
            DDERQSHPDJ.Dispose();
            DDRSHT.Dispose();
            DDRQSHPQTY.Dispose();
            DDUOM.Dispose();
            DDRSDJ.Dispose();
            DDRSDT.Dispose();
            DDHZED.Dispose();
            DDHZSD.Dispose();
            DDASBLNFD.Dispose();
            DDASBLNLOC.Dispose();
            DDASBNO.Dispose();
            DDDMDBTHNO.Dispose();
            DDCSGSHPNO.Dispose();
            DDGCNT.Dispose();
            DDCUMID.Dispose();
            DDTXTREFNO.Dispose();
            DDREQFRQ.Dispose();
            DDCSTDMDPT.Dispose();
            DDREQTYP.Dispose();
            DDCSTORDNO.Dispose();
            DDDKID.Dispose();
            DDENDJOBSQ.Dispose();
            DDPSJOBNO.Dispose();
            DDHZCLSNO.Dispose();
            DDJOBSQ.Dispose();
            DDKBSNO.Dispose();
            DDKBENO.Dispose();
            DDLABORHR.Dispose();
            DDCIRV.Dispose();
            DDYEAR1.Dispose();
            DDPTRLSSTS.Dispose();
            DDCSTPODJ.Dispose();
            DDCSTPOTM.Dispose();
            DDCSTPOLIN.Dispose();
            DDCSTPO.Dispose();
            DDTPUR.Dispose();
            DDQTYBYPOS.Dispose();
            DDSOCN.Dispose();
            DDPSIG.Dispose();
            DDRSVLNFD.Dispose();
            DDSTSSQIND.Dispose();
            DDEDTCTDJ.Dispose();
            DDEDTCTTM.Dispose();
            DDOGDMDQTY.Dispose();
            DDEDTMSGNM.Dispose();
            DDEDTMSGSB.Dispose();
            DDEDTMSGTP.Dispose();
            DDTRSRTE.Dispose();
            DDTRSRTSFX.Dispose();
            DDUCARTNO.Dispose();
            DDVINNO.Dispose();
            DDPENDDLT.Dispose();
            DDSDID.Dispose();
            DDNETVC.Dispose();
            DDNETVCNTF.Dispose();
            DDCRTCLID.Dispose();
            DDDMD06.Dispose();
            DDDMD07.Dispose();
            DDDMD08.Dispose();
            DDDMD09.Dispose();
            DDDMD10.Dispose();
            DDDMDREF1.Dispose();
            DDDMDREF2.Dispose();
            DDCTLNUM.Dispose();
            DDURCD.Dispose();
            DDURDT.Dispose();
            DDURAT.Dispose();
            DDURAB.Dispose();
            DDURRF.Dispose();
            DDUSER.Dispose();
            DDPID.Dispose();
            DDJOBN.Dispose();
            DDUPMJ.Dispose();
            DDTDAY.Dispose();
            DDCARDNO.Dispose();
            DDDELBATCH.Dispose();
            DDCIRVQ.Dispose();
            DDPRODTYPE.Dispose();
            DDRTORSQ.Dispose();
            DDRTGDAT.Dispose();
            DDRTGTIM.Dispose();
            DDDMDREF3.Dispose();
            DDDMDREF4.Dispose();
            DDDMDREF5.Dispose();
            DDDMDREF6.Dispose();
            DDDMDREF7.Dispose();
            DDDMDREF8.Dispose();
            DDDMDREF9.Dispose();
            DDDMDREF10.Dispose();
            DDSCAC.Dispose();
            DDATMCN.Dispose();
            DDOFFLNDT.Dispose();
            DDENGTAG1.Dispose();
            DDENGTAGR1.Dispose();
            DDENGTAG2.Dispose();
            DDENGTAGR2.Dispose();
        }

        #endregion

        #region AddColumnsToTable

        /// <summary>
        /// Adds All Columns to Table
        /// </summary>
        public virtual void AddColumnsToTable()
        {
            this.DDUKID.AddToTable();
            this.DDDEID.AddToTable();
            this.DDMCU.AddToTable();
            this.DDAN8.AddToTable();
            this.DDSHAN.AddToTable();
            this.DDDVAN.AddToTable();
            this.DDITM.AddToTable();
            this.DDDETYP.AddToTable();
            this.DDDEPER.AddToTable();
            this.DDCRTSOSTS.AddToTable();
            this.DDRLSNO.AddToTable();
            this.DDRLSDJ.AddToTable();
            this.DDRLSTM.AddToTable();
            this.DDRQSJ.AddToTable();
            this.DDERQSHPDJ.AddToTable();
            this.DDRSHT.AddToTable();
            this.DDRQSHPQTY.AddToTable();
            this.DDUOM.AddToTable();
            this.DDRSDJ.AddToTable();
            this.DDRSDT.AddToTable();
            this.DDHZED.AddToTable();
            this.DDHZSD.AddToTable();
            this.DDASBLNFD.AddToTable();
            this.DDASBLNLOC.AddToTable();
            this.DDASBNO.AddToTable();
            this.DDDMDBTHNO.AddToTable();
            this.DDCSGSHPNO.AddToTable();
            this.DDGCNT.AddToTable();
            this.DDCUMID.AddToTable();
            this.DDTXTREFNO.AddToTable();
            this.DDREQFRQ.AddToTable();
            this.DDCSTDMDPT.AddToTable();
            this.DDREQTYP.AddToTable();
            this.DDCSTORDNO.AddToTable();
            this.DDDKID.AddToTable();
            this.DDENDJOBSQ.AddToTable();
            this.DDPSJOBNO.AddToTable();
            this.DDHZCLSNO.AddToTable();
            this.DDJOBSQ.AddToTable();
            this.DDKBSNO.AddToTable();
            this.DDKBENO.AddToTable();
            this.DDLABORHR.AddToTable();
            this.DDCIRV.AddToTable();
            this.DDYEAR1.AddToTable();
            this.DDPTRLSSTS.AddToTable();
            this.DDCSTPODJ.AddToTable();
            this.DDCSTPOTM.AddToTable();
            this.DDCSTPOLIN.AddToTable();
            this.DDCSTPO.AddToTable();
            this.DDTPUR.AddToTable();
            this.DDQTYBYPOS.AddToTable();
            this.DDSOCN.AddToTable();
            this.DDPSIG.AddToTable();
            this.DDRSVLNFD.AddToTable();
            this.DDSTSSQIND.AddToTable();
            this.DDEDTCTDJ.AddToTable();
            this.DDEDTCTTM.AddToTable();
            this.DDOGDMDQTY.AddToTable();
            this.DDEDTMSGNM.AddToTable();
            this.DDEDTMSGSB.AddToTable();
            this.DDEDTMSGTP.AddToTable();
            this.DDTRSRTE.AddToTable();
            this.DDTRSRTSFX.AddToTable();
            this.DDUCARTNO.AddToTable();
            this.DDVINNO.AddToTable();
            this.DDPENDDLT.AddToTable();
            this.DDSDID.AddToTable();
            this.DDNETVC.AddToTable();
            this.DDNETVCNTF.AddToTable();
            this.DDCRTCLID.AddToTable();
            this.DDDMD06.AddToTable();
            this.DDDMD07.AddToTable();
            this.DDDMD08.AddToTable();
            this.DDDMD09.AddToTable();
            this.DDDMD10.AddToTable();
            this.DDDMDREF1.AddToTable();
            this.DDDMDREF2.AddToTable();
            this.DDCTLNUM.AddToTable();
            this.DDURCD.AddToTable();
            this.DDURDT.AddToTable();
            this.DDURAT.AddToTable();
            this.DDURAB.AddToTable();
            this.DDURRF.AddToTable();
            this.DDUSER.AddToTable();
            this.DDPID.AddToTable();
            this.DDJOBN.AddToTable();
            this.DDUPMJ.AddToTable();
            this.DDTDAY.AddToTable();
            this.DDCARDNO.AddToTable();
            this.DDDELBATCH.AddToTable();
            this.DDCIRVQ.AddToTable();
            this.DDPRODTYPE.AddToTable();
            this.DDRTORSQ.AddToTable();
            this.DDRTGDAT.AddToTable();
            this.DDRTGTIM.AddToTable();
            this.DDDMDREF3.AddToTable();
            this.DDDMDREF4.AddToTable();
            this.DDDMDREF5.AddToTable();
            this.DDDMDREF6.AddToTable();
            this.DDDMDREF7.AddToTable();
            this.DDDMDREF8.AddToTable();
            this.DDDMDREF9.AddToTable();
            this.DDDMDREF10.AddToTable();
            this.DDSCAC.AddToTable();
            this.DDATMCN.AddToTable();
            this.DDOFFLNDT.AddToTable();
            this.DDENGTAG1.AddToTable();
            this.DDENGTAGR1.AddToTable();
            this.DDENGTAG2.AddToTable();
            this.DDENGTAGR2.AddToTable();
        }

        #endregion
    }

    #endregion
}