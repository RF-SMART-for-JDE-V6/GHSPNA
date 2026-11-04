#region svn header
/*
  $HeadURL: https://serena.icsfl.com:8443/svn/jde/Wireless-JDE/branches/5.3/Wireless/Workflows/Receiving/StoneridgePOReceipts.cs $:
  $Author: sstroble@ICSNT $:
  $Rev: 20304 $:
  $Date: 2017-03-15 15:03:48 -0600 (Wed, 15 Mar 2017) $:
*/
#endregion

#region Usings

using System;
using System.Data;

using ICS.Database.TableObjects.JDE;
using ICS.Database.TableObjects.RFS;
using ICS.Display;
using ICS.ERPI.JDE;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;
using System.Data.SqlClient;
using ICS.Security;
using System.Text;

#endregion


namespace ICS.Stoneridge.Wireless.JDE.Workflow.Receiving
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class StoneridgePOReceipts : ICS.Workflow.Base.WorkflowDesign.Components.RFSWorkflowDesigner
    {
        #region Fields

        #region Tables

        // Oracle/PeopleSoft/JDE/Tables
        private F0005 _UserDefinedCodesTable; // user defined codes
        //private F0006 _BranchPlantTable; // Branch/plant
        private F4100 _LocationMasterTable; // location master
        private F4108 _LotMasterTable; // lot master
        private F4102 _ItemBranchTable; // item Branch
        private F41021 _ItemLocationTable; // item locations
        private F4301 _PurchaseOrderHeaderTable; // purchase order header
        private CustomJDETables.F4311 _PurchaseOrderDetailTable; // purchase order detail
        private F41002 _UnitOfMeasureConversionTable1; // unit of measure conversions
        private F41003 _UnitOfMeasureConversionTable2; // unit of measure conversion default
        private F43090 _ReceiptRouteTable; // receipt routes
        private F4104 _ItemCrossReferenceTable; // jde item cross reference
        //private F41001 _WarehouseConstants; // Warehouse Constants Table
        private F46L10 _LicensePlateMasterTable;
        private F46011 _ItemUOMProfileTable;
        private F0101 _AddressBookTable;
        private F4211 _SalesOrderTable;
        private RFS_DataManipulator _DataManipulatorTable; // RFS data manipulator
        private RFS_ERPRecordLocks _BusinessDataRecordLockTable; // RFS ERP Record locks
        private RFS_PrinterDefinition _DefinedPrintersTable; // RFS Defined Printers
        private RFS_NextNumber _NextNumberTable;
        private RFS_PrintXref _PrinterCrossReferenceTable;
        private F46L001 _LicensePlateNumberingConstantsTable;
        private DataSet _GeneratedLicensePlateNumbersDataSet;
        private DataTable _GeneratedLicensePlateNumbersTable;
        private CustomJDETables.F5846001 _SerialTable;

        #endregion

        #region Screens

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _LocationMasterList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _ItemMasterList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _ItemLocationList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _EmptyLocationsList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _BranchPlantList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _PurchaseOrderHeaderList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _PurchaseOrderDetailList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _PurchaseOrderItemList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _UnitOfMeasureConversionList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _UserDefinedCodesList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _DefinedPrintersList;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _LicensePlateMasterList;

        ICS.Workflow.Base.JDE.Components.BranchPlantScreen _CollectBranch;
        //ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectBranch;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectOrderNumber;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectOrderType;
        ICS.Workflow.Base.JDE.Components.ItemScreen _CollectItemNumber;
        //ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectItemNumber;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectContainerNumber;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectSupplierRemarks;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLineNumber;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLotNumber;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectMemoLot1;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectMemoLot2;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectVendorLot;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLotSerialExpirationDate;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectSerialNumber;
        ICS.Workflow.Base.JDE.Components.QuantityScreen _CollectQuantity;
        //ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectQuantity;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLocation;
        ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen _CollectUnitOfMeasure;
        //ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectUnitOfMeasure;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _ReceiptRoutingMessage;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectByItem;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _UnderReceiptWarning;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _OverReceiptWarning;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPrinter;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _PrintBarCodeLabels;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _BusinessDataRecordLockInformation;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLicensePlate;
        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _ConfirmNewLicensePlate;

        #endregion

        #region Collected/Working Data
        private string _LotGrade = string.Empty;
        private string _LotPotency = string.Empty;
        private string _CountryOfOrigin = string.Empty;
        public bool DefaultLotGradePotency
        {
            get
            {
                return WFOptions.GetYesNoWFOptionValue("DefaultLotGradePotency");
            }
        }
        public bool DefaultCountryOfOrigin
        {
            get
            {
                return WFOptions.GetYesNoWFOptionValue("DefaultCountryOfOrigin");
            }
        }
        private string _Location;
        private string _SearchType;
        private bool RoutingMessageDisplayed;
        private string _LotNumber;
        private string _ScreenFormattedLocation;
        private double _OnHandQuantity;
        private bool _FromSerialNumberEntry;
        private double _TransactionQuantity;
        private double _SerialNumberQuantity;
        private double _OriginalQuantityEntered;
        private double _TotalReceiptQuantity;
        private string[] _AllowedNextStatusCodes = null;
        private string[] _NonStockLineTypes = null;
        private string _NextStatusCode;
        private string _BranchPlant = " ";
        private string _DetailBranchPlant;
        private string _DocumentNumber;
        private string _DocumentType;
        private double _LineNumber;
        private string _LongItemNumber;
        private string _ThirdItemNumber;
        private string _PrimaryItemNumber;
        private double _ShortItemNumber;
        private string _UnitOfMeasureAsEntered;
        private string _LotProcessType;
        private string _ExpirationDate;
        private string _Container;
        private string _Remarks;
        private string _LicensePlate;
        private int _SerialNumbersScanned;
        private string _LastDateEntered;
        private string _MemoLot1;
        private string _MemoLot2;
        private string _VendorLot;
        private string _PackingSlip;
        private string _Revision;
        private string _DefaultUnitOfMeasure;
        private string[] _AllowedDocTypes = null;
        private string _LocationSeparator;
        private string _LicensePlateProcessingType = "";
        private string _DefaultLocation;
        // BE012073 - default quantity for barcodes
        private string _BarcodeUnitOfMeasure;
        private string _BarcodeUOMForPrinting;
        private string _PrinterAddress;
        private string _Copies;
        private string _LocationLookupType;
        private bool _BusinessDataLocked = false;
        private int _MaxLabelQty;
        private string _SendFlag;
        private string _PrinterName;
        private double _OpenQuantity;
        private double _OpenQuantityInPrimaryUnitOfMeasure;
        private bool _FromGenLP = false;
        private bool _LPLicense = false;
        protected ICS.Workflow.Base.JDE.Components.QuantityScreen _CollectDualQuantity;
        protected ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen _CollectDualUnitOfMeasure; //CC025677

        protected double _DualQuantity = 0;
        protected string _DualUnitOfMeasure = "";

        protected string[] _SalesOrderStatusCodes;
        #endregion

        #region ERPI

        private ICS.ERPI.JDE.POReceiving _ReceivingTransaction = new POReceiving();

        #endregion
        protected GenericScreen CollectCountryOfOrigin;

        #endregion
        protected GenericList CollectCountryOfOrigin_List;
        protected GenericScreen CollectLotPotency;
        protected GenericScreen CollectLotGrade;
        protected GenericList CollectLotGrade_List;
        protected GenericList MultiItemPOList;
        protected GenericScreen CollectPackingSlip;

        #region Properties

        #region F00095 (JDE Reservation)
        protected ICS.Database.TableObjects.JDE.F00095 _f00095;

        public virtual ICS.Database.TableObjects.JDE.F00095 JDEReservationTable
        {

            #region get
            get
            {
                if (_f00095 == null)
                {
                    _f00095 = new ICS.Database.TableObjects.JDE.F00095(DataAccess, string.Empty);
                }

                return _f00095;
            }
            #endregion
            #region set
            set
            {
                _f00095 = value;
            }
            #endregion

        }

        #endregion


        #endregion

        #region Constructors

        /// <summary>
        /// JDE Receipts by Order/Item
        /// </summary>
        /// <param name="userSession"></param>
        public StoneridgePOReceipts(UserSession userSession)
            : base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        } // end StoneridgePOReceipts constructor

        public StoneridgePOReceipts()
            : base()
        {
            InitializeComponent();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Unlocks the Purchase Order Detail (F4311) record and clears any necessary objects when
        /// the object is being disposed.
        /// </summary>
        public override void Dispose()
        {
            try
            {
                _PurchaseOrderDetailTable.UnLockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment);
            } // end try
            catch (Exception ex)
            {
                Log.LogException(ex);
            } // end catch

            _GeneratedLicensePlateNumbersDataSet.Dispose();
            UserSession.ScreenHandler.ScreenMap.ClearAllWorkflows();
            Transactions.ClearTransactions();
            UserSession.ScreenHandler.ScreenMap.Clear("Error");

            base.Dispose();
        } // end Dipose

        #endregion

        #region Methods

        #region InitializeComponent

        private void InitializeComponent()
        {
            this._DefinedPrintersList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectLicensePlate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._LicensePlateMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._PrintBarCodeLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ItemLocationList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectSerialNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectLotSerialExpirationDate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectMemoLot1 = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectMemoLot2 = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectVendorLot = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectLotPotency = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectLotGrade = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectLotGrade_List = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectCountryOfOrigin = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectCountryOfOrigin_List = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectQuantity = new ICS.Workflow.Base.JDE.Components.QuantityScreen(this);
            this._CollectUnitOfMeasure = new ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen(this);
            this._CollectBranch = new ICS.Workflow.Base.JDE.Components.BranchPlantScreen(this);
            this._BranchPlantList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectOrderNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._PurchaseOrderHeaderList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectOrderType = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._UserDefinedCodesList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectContainerNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectSupplierRemarks = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectItemNumber = new ICS.Workflow.Base.JDE.Components.ItemScreen(this);
            this._PurchaseOrderItemList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._UnitOfMeasureConversionList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectDualQuantity = new ICS.Workflow.Base.JDE.Components.QuantityScreen(this);
            this._CollectDualUnitOfMeasure = new ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen(this);
            this._CollectLotNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._BusinessDataRecordLockInformation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._PurchaseOrderDetailList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectLineNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ReceiptRoutingMessage = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ItemMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectByItem = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._UnderReceiptWarning = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._LocationMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._EmptyLocationsList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._OverReceiptWarning = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ConfirmNewLicensePlate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.MultiItemPOList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectPackingSlip = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // _DefinedPrintersList
            // 
            this._DefinedPrintersList.ListScreenParent = this._CollectPrinter;
            this._DefinedPrintersList.ParentScreen = this._CollectPrinter;
            this._DefinedPrintersList.ScreenName = "FRFS_Printers";
            this._DefinedPrintersList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.FRFSPrinters_EnterEvent);
            this._DefinedPrintersList.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.FRFSPrinters_BackEvent);
            this._DefinedPrintersList.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.FRFSPrinters_BackEvent);
            this._DefinedPrintersList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._DefinedPrintersList_LoadEvent);
            // 
            // _CollectPrinter
            // 
            this._CollectPrinter.ListScreen = this._DefinedPrintersList;
            this._CollectPrinter.ListScreenControl = this._DefinedPrintersList;
            this._CollectPrinter.ScreenName = "CollectPrinter";
            this._CollectPrinter.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_EnterEvent);
            this._CollectPrinter.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _CollectLicensePlate
            // 
            this._CollectLicensePlate.ListScreen = this._LicensePlateMasterList;
            this._CollectLicensePlate.ListScreenControl = this._LicensePlateMasterList;
            this._CollectLicensePlate.NextScreen = this._PrintBarCodeLabels;
            this._CollectLicensePlate.ScreenName = "CollectLicensePlate";
            this._CollectLicensePlate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_EnterEvent);
            this._CollectLicensePlate.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_BackEvent);
            this._CollectLicensePlate.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectLicensePlate.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_LoadEvent);
            // 
            // _LicensePlateMasterList
            // 
            this._LicensePlateMasterList.ListScreenParent = this._CollectLicensePlate;
            this._LicensePlateMasterList.ParentScreen = this._CollectLicensePlate;
            this._LicensePlateMasterList.ScreenName = "LicensePlateMaster_List";
            this._LicensePlateMasterList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LicensePlateMasterList_EnterEvent);
            this._LicensePlateMasterList.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LicensePlateMasterList_BackEvent);
            this._LicensePlateMasterList.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LicensePlateMasterList_BackEvent);
            this._LicensePlateMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._LicensePlateMasterList_LoadEvent);
            // 
            // _PrintBarCodeLabels
            // 
            this._PrintBarCodeLabels.NextScreen = this._CollectPrinter;
            this._PrintBarCodeLabels.ScreenName = "PrintBarCodeLabels";
            this._PrintBarCodeLabels.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabels_EnterEvent);
            this._PrintBarCodeLabels.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabels_BackEvent);
            this._PrintBarCodeLabels.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _CollectLocation
            // 
            this._CollectLocation.ListScreen = this._ItemLocationList;
            this._CollectLocation.ListScreenControl = this._ItemLocationList;
            this._CollectLocation.NextScreen = this._CollectQuantity;
            this._CollectLocation.ScreenName = "CollectLocation";
            this._CollectLocation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_EnterEvent);
            this._CollectLocation.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.BackEvent_UnlockERP);
            this._CollectLocation.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectLocation.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_ListEvent);
            this._CollectLocation.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectLocation_LoadEvent);
            this._CollectLocation.KeyEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_KeyEvent);
            // 
            // _ItemLocationList
            // 
            this._ItemLocationList.ListScreenParent = this._CollectLotNumber;
            this._ItemLocationList.ParentScreen = this._CollectLotNumber;
            this._ItemLocationList.ScreenName = "F41021_List";
            this._ItemLocationList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ItemLocationList_EnterEvent);
            this._ItemLocationList.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ItemLocationList_BackEvent);
            this._ItemLocationList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ItemLocationList_LoadEvent);
            // 
            // _CollectSerialNumber
            // 
            this._CollectSerialNumber.ListScreen = this._ItemLocationList;
            this._CollectSerialNumber.ListScreenControl = this._ItemLocationList;
            this._CollectSerialNumber.NextScreen = this._CollectLotSerialExpirationDate;
            this._CollectSerialNumber.ScreenName = "CollectSerialNumber";
            this._CollectSerialNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSerialNumber_EnterEvent);
            this._CollectSerialNumber.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectSerialNumber.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSerialNumber_ListEvent);
            this._CollectSerialNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectSerialNumber_LoadEvent);
            // 
            // _CollectLotSerialExpirationDate
            // 
            this._CollectLotSerialExpirationDate.NextScreen = this._CollectMemoLot1;
            this._CollectLotSerialExpirationDate.ScreenName = "CollectLotSerialExpDate";
            this._CollectLotSerialExpirationDate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotSerialExpDate_EnterEvent);
            this._CollectLotSerialExpirationDate.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotSerialExpDate_BackEvent);
            this._CollectLotSerialExpirationDate.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectLotSerialExpirationDate.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectLotSerialExpirationDate_LoadEvent);
            // 
            // _CollectMemoLot1
            // 
            this._CollectMemoLot1.NextScreen = this._CollectMemoLot2;
            this._CollectMemoLot1.ScreenName = "CollectMemoLot1";
            this._CollectMemoLot1.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectMemoLot1_EnterEvent);
            this._CollectMemoLot1.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _CollectMemoLot2
            // 
            this._CollectMemoLot2.NextScreen = this._CollectVendorLot;
            this._CollectMemoLot2.ScreenName = "CollectMemoLot2";
            this._CollectMemoLot2.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectMemoLot2_EnterEvent);
            this._CollectMemoLot2.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _CollectVendorLot
            // 
            this._CollectVendorLot.NextScreen = this.CollectLotPotency;
            this._CollectVendorLot.ScreenName = "CollectVendorLot";
            this._CollectVendorLot.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectVendorLot_EnterEvent);
            this._CollectVendorLot.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // CollectLotPotency
            // 
            this.CollectLotPotency.NextScreen = this.CollectLotGrade;
            this.CollectLotPotency.ScreenName = "CollectLotPotency";
            this.CollectLotPotency.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotPotency_LoadEvent);
            this.CollectLotPotency.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectLotPotency_ValidateMethod);
            // 
            // CollectLotGrade
            // 
            this.CollectLotGrade.ListScreen = this.CollectLotGrade_List;
            this.CollectLotGrade.ListScreenControl = this.CollectLotGrade_List;
            this.CollectLotGrade.NextScreen = this.CollectCountryOfOrigin;
            this.CollectLotGrade.ScreenName = "CollectLotGrade";
            this.CollectLotGrade.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotGrade_BackValidationEvent);
            this.CollectLotGrade.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotGrade_LoadEvent);
            this.CollectLotGrade.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectLotGrade_ValidateMethod);
            // 
            // CollectLotGrade_List
            // 
            this.CollectLotGrade_List.DataColumnForValidation = "DRKY";
            this.CollectLotGrade_List.ListScreenParent = this.CollectLotGrade;
            this.CollectLotGrade_List.ParentScreen = this.CollectLotGrade;
            this.CollectLotGrade_List.ScreenName = "F0005_List";
            this.CollectLotGrade_List.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotGrade_List_LoadEvent);
            // 
            // CollectCountryOfOrigin
            // 
            this.CollectCountryOfOrigin.ListScreen = this.CollectCountryOfOrigin_List;
            this.CollectCountryOfOrigin.ListScreenControl = this.CollectCountryOfOrigin_List;
            this.CollectCountryOfOrigin.NextScreen = this._CollectLocation;
            this.CollectCountryOfOrigin.ScreenName = "CollectCountryOfOrigin";
            this.CollectCountryOfOrigin.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectCountryOfOrigin_BackValidationEvent);
            this.CollectCountryOfOrigin.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectCountryOfOrigin_LoadEvent);
            this.CollectCountryOfOrigin.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectCountryOfOrigin_ValidateMethod);
            // 
            // CollectCountryOfOrigin_List
            // 
            this.CollectCountryOfOrigin_List.DataColumnForValidation = "DRKY";
            this.CollectCountryOfOrigin_List.ListScreenParent = this.CollectCountryOfOrigin;
            this.CollectCountryOfOrigin_List.ParentScreen = this.CollectCountryOfOrigin;
            this.CollectCountryOfOrigin_List.ScreenName = "F0005_List";
            this.CollectCountryOfOrigin_List.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectCountryOfOrigin_List_LoadEvent);
            // 
            // _CollectQuantity
            // 
            this._CollectQuantity.ItemScreen = null;
            this._CollectQuantity.NextScreen = this._CollectUnitOfMeasure;
            this._CollectQuantity.ScreenName = "CollectQuantity";
            this._CollectQuantity.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.BackEvent_UnlockERP);
            this._CollectQuantity.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectQuantity.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectQuantity_LeavingEvent);
            // 
            // _CollectUnitOfMeasure
            // 
            this._CollectUnitOfMeasure.BranchScreen = this._CollectBranch;
            this._CollectUnitOfMeasure.DisableToleranceCheck = true;
            this._CollectUnitOfMeasure.DualQtyScreen = null;
            this._CollectUnitOfMeasure.ItemScreen = this._CollectItemNumber;
            this._CollectUnitOfMeasure.ListScreen = this._UnitOfMeasureConversionList;
            this._CollectUnitOfMeasure.ListScreenControl = this._UnitOfMeasureConversionList;
            this._CollectUnitOfMeasure.NextScreen = this._CollectDualQuantity;
            this._CollectUnitOfMeasure.OriginalQtyScreen = this._CollectQuantity;
            this._CollectUnitOfMeasure.OriginalUOMScreen = null;
            this._CollectUnitOfMeasure.ScreenName = "CollectUnitOfMeasure";
            this._CollectUnitOfMeasure.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectUnitOfMeasure.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectUnitOfMeasure_ListEvent);
            this._CollectUnitOfMeasure.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.ValidateUnitOfMeasure);
            this._CollectUnitOfMeasure.UOMConversionsList.ScreenName = "F41002_List";
            // 
            // _CollectBranch
            // 
            this._CollectBranch.ListScreen = this._BranchPlantList;
            this._CollectBranch.ListScreenControl = this._BranchPlantList;
            this._CollectBranch.NextScreen = this._CollectOrderNumber;
            this._CollectBranch.ScreenName = "CollectBranch";
            this._CollectBranch.WarehouseConstantsTable = null;
            this._CollectBranch.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectBranch.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectBranch.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectBranch_LeavingEvent);
            this._CollectBranch.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.ValidateBranch);
            this._CollectBranch.BranchPlantsList.BackScreen = this._CollectBranch;
            this._CollectBranch.BranchPlantsList.ScreenName = "F0006_List";
            // 
            // _BranchPlantList
            // 
            this._BranchPlantList.ListScreenParent = this._CollectBranch;
            this._BranchPlantList.ParentScreen = this._CollectBranch;
            this._BranchPlantList.ScreenName = "F0006_List";
            this._BranchPlantList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.BranchPlantList_EnterEvent);
            this._BranchPlantList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._BranchPlantList_LoadEvent);
            this._BranchPlantList.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._BranchPlantList_LeavingEvent);
            // 
            // _CollectOrderNumber
            // 
            this._CollectOrderNumber.ListScreen = this._PurchaseOrderHeaderList;
            this._CollectOrderNumber.ListScreenControl = this._PurchaseOrderHeaderList;
            this._CollectOrderNumber.NextScreen = this._CollectOrderType;
            this._CollectOrderNumber.ScreenName = "CollectOrderNumber";
            this._CollectOrderNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectOrderNumber_EnterEvent);
            this._CollectOrderNumber.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.BackEvent_UnlockERP);
            this._CollectOrderNumber.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _PurchaseOrderHeaderList
            // 
            this._PurchaseOrderHeaderList.ListScreenParent = this._CollectOrderNumber;
            this._PurchaseOrderHeaderList.ParentScreen = this._CollectOrderNumber;
            this._PurchaseOrderHeaderList.ScreenName = "F4301_List";
            this._PurchaseOrderHeaderList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PurchaseOrderHeaderList_EnterEvent);
            this._PurchaseOrderHeaderList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._PurchaseOrderHeaderList_LoadEvent);
            // 
            // _CollectOrderType
            // 
            this._CollectOrderType.ListScreen = this._UserDefinedCodesList;
            this._CollectOrderType.ListScreenControl = this._UserDefinedCodesList;
            this._CollectOrderType.NextScreen = this._CollectContainerNumber;
            this._CollectOrderType.ScreenName = "CollectOrderType";
            this._CollectOrderType.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectOrderType_EnterEvent);
            this._CollectOrderType.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectOrderType.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectOrderType_LeavingEvent);
            // 
            // _UserDefinedCodesList
            // 
            this._UserDefinedCodesList.ListScreenParent = this._CollectOrderType;
            this._UserDefinedCodesList.ParentScreen = this._CollectOrderType;
            this._UserDefinedCodesList.ScreenName = "F0005_List";
            this._UserDefinedCodesList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UserDefinedCodesList_EnterEvent);
            this._UserDefinedCodesList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._UserDefinedCodesList_LoadEvent);
            // 
            // _CollectContainerNumber
            // 
            this._CollectContainerNumber.NextScreen = this._CollectSupplierRemarks;
            this._CollectContainerNumber.ScreenName = "CollectContainerNumber";
            this._CollectContainerNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectContainerNumber_EnterEvent);
            this._CollectContainerNumber.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _CollectSupplierRemarks
            // 
            this._CollectSupplierRemarks.NextScreen = this._CollectItemNumber;
            this._CollectSupplierRemarks.ScreenName = "CollectSupplierRemarks";
            this._CollectSupplierRemarks.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSupplierRemarks_EnterEvent);
            this._CollectSupplierRemarks.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectSupplierRemarks.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectSupplierRemarks_LeavingEvent);
            // 
            // _CollectItemNumber
            // 
            this._CollectItemNumber.BranchScreen = this._CollectBranch;
            this._CollectItemNumber.ItemBranchesTable = null;
            this._CollectItemNumber.ItemsTable = null;
            this._CollectItemNumber.ListScreen = this._PurchaseOrderItemList;
            this._CollectItemNumber.ListScreenControl = this._PurchaseOrderItemList;
            this._CollectItemNumber.NextScreen = this._CollectLocation;
            this._CollectItemNumber.ScreenName = "CollectItemNumber";
            this._CollectItemNumber.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectItemNumber.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectItemNumber_ListEvent);
            this._CollectItemNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.ValidateItemNumber);
            // 
            // _PurchaseOrderItemList
            // 
            this._PurchaseOrderItemList.ListScreenParent = this._CollectItemNumber;
            this._PurchaseOrderItemList.ParentScreen = this._CollectItemNumber;
            this._PurchaseOrderItemList.ScreenName = "F4311_List";
            this._PurchaseOrderItemList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PurchaseOrderItemList_EnterEvent);
            this._PurchaseOrderItemList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._PurchaseOrderItemList_LoadEvent);
            // 
            // _UnitOfMeasureConversionList
            // 
            this._UnitOfMeasureConversionList.ListScreenParent = this._CollectDualUnitOfMeasure;
            this._UnitOfMeasureConversionList.ParentScreen = this._CollectDualUnitOfMeasure;
            this._UnitOfMeasureConversionList.ScreenName = "F41002_List";
            this._UnitOfMeasureConversionList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UnitOfMeasureList_EnterEvent);
            this._UnitOfMeasureConversionList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._UnitOfMeasureConversionList_LoadEvent);
            // 
            // _CollectDualQuantity
            // 
            this._CollectDualQuantity.IsDualQuantityScreen = true;
            this._CollectDualQuantity.ItemScreen = this._CollectItemNumber;
            this._CollectDualQuantity.NextScreen = this._CollectDualUnitOfMeasure;
            this._CollectDualQuantity.ScreenMapKey = "DualQuantity";
            this._CollectDualQuantity.ScreenName = "CollectDualQuantity";
            this._CollectDualQuantity.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectDualQuantity_LoadEvent);
            this._CollectDualQuantity.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectDualQuantity_LeavingEvent);
            // 
            // _CollectDualUnitOfMeasure
            // 
            this._CollectDualUnitOfMeasure.BranchScreen = this._CollectBranch;
            this._CollectDualUnitOfMeasure.DualQtyScreen = this._CollectDualQuantity;
            this._CollectDualUnitOfMeasure.IsDualUnitOfMeasureScreen = true;
            this._CollectDualUnitOfMeasure.ItemScreen = this._CollectItemNumber;
            this._CollectDualUnitOfMeasure.ListScreen = this._UnitOfMeasureConversionList;
            this._CollectDualUnitOfMeasure.ListScreenControl = this._UnitOfMeasureConversionList;
            this._CollectDualUnitOfMeasure.NextScreen = this._PrintBarCodeLabels;
            this._CollectDualUnitOfMeasure.OriginalQtyScreen = this._CollectQuantity;
            this._CollectDualUnitOfMeasure.OriginalUOMScreen = this._CollectUnitOfMeasure;
            this._CollectDualUnitOfMeasure.ScreenMapKey = "DualUnitOfMeasure";
            this._CollectDualUnitOfMeasure.ScreenName = "CollectDualUnitOfMeasure";
            this._CollectDualUnitOfMeasure.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectDualUnitOfMeasure_ListEvent);
            this._CollectDualUnitOfMeasure.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectDualUnitOfMeasure_LeavingEvent);
            this._CollectDualUnitOfMeasure.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.ValidateDualUnitOfMeasure);
            this._CollectDualUnitOfMeasure.UOMConversionsList.ScreenName = "F41002_List";
            // 
            // _CollectLotNumber
            // 
            this._CollectLotNumber.ListScreen = this._ItemLocationList;
            this._CollectLotNumber.ListScreenControl = this._ItemLocationList;
            this._CollectLotNumber.NextScreen = this._CollectLotSerialExpirationDate;
            this._CollectLotNumber.ScreenName = "CollectLotNumber";
            this._CollectLotNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotNumber_EnterEvent);
            this._CollectLotNumber.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotNumber_BackEvent);
            this._CollectLotNumber.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectLotNumber.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotNumber_ListEvent);
            this._CollectLotNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectLotNumber_LoadEvent);
            // 
            // _BusinessDataRecordLockInformation
            // 
            this._BusinessDataRecordLockInformation.ScreenName = "FLockInfo";
            this._BusinessDataRecordLockInformation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._BusinessDataRecordLockInformation_EnterValidationEvent);
            this._BusinessDataRecordLockInformation.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _PurchaseOrderDetailList
            // 
            this._PurchaseOrderDetailList.ListScreenParent = this._CollectLineNumber;
            this._PurchaseOrderDetailList.ParentScreen = this._CollectLineNumber;
            this._PurchaseOrderDetailList.ScreenName = "F4311_List";
            this._PurchaseOrderDetailList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PurchaseOrderDetailList_EnterEvent);
            this._PurchaseOrderDetailList.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._PurchaseOrderDetailList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._PurchaseOrderDetailList_LoadEvent);
            // 
            // _CollectLineNumber
            // 
            this._CollectLineNumber.ListScreen = this._PurchaseOrderDetailList;
            this._CollectLineNumber.ListScreenControl = this._PurchaseOrderDetailList;
            this._CollectLineNumber.NextScreen = this._CollectLocation;
            this._CollectLineNumber.ScreenName = "CollectLineNumber";
            this._CollectLineNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLineNumber_EnterEvent);
            this._CollectLineNumber.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            this._CollectLineNumber.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLineNumber_ListEvent);
            // 
            // _ReceiptRoutingMessage
            // 
            this._ReceiptRoutingMessage.NextScreen = this._CollectLocation;
            this._ReceiptRoutingMessage.ScreenName = "ReceiptRoutingMessage";
            this._ReceiptRoutingMessage.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ReceiptRoutingMessage_EnterEvent);
            this._ReceiptRoutingMessage.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ReceiptRoutingMessage_BackEvent);
            this._ReceiptRoutingMessage.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _ItemMasterList
            // 
            this._ItemMasterList.ListScreenParent = this._CollectByItem;
            this._ItemMasterList.ParentScreen = this._CollectByItem;
            this._ItemMasterList.ScreenName = "F4101_List";
            this._ItemMasterList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ItemMasterList_EnterEvent);
            this._ItemMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ItemMasterList_LoadEvent);
            // 
            // _CollectByItem
            // 
            this._CollectByItem.ListScreen = this._ItemMasterList;
            this._CollectByItem.ListScreenControl = this._ItemMasterList;
            this._CollectByItem.NextScreen = this._CollectOrderNumber;
            this._CollectByItem.ScreenName = "CollectByItem";
            this._CollectByItem.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectByItem_EnterEvent);
            this._CollectByItem.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _UnderReceiptWarning
            // 
            this._UnderReceiptWarning.ScreenName = "UnderReceiptWarning";
            this._UnderReceiptWarning.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UnderReceiptWarning_EnterEvent);
            this._UnderReceiptWarning.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _LocationMasterList
            // 
            this._LocationMasterList.ScreenName = "F4100_List";
            this._LocationMasterList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LocationMasterList_EnterEvent);
            this._LocationMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._LocationMasterList_LoadEvent);
            this._LocationMasterList.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._LocationMasterList_LeavingEvent);
            // 
            // _EmptyLocationsList
            // 
            this._EmptyLocationsList.BackScreen = this._CollectLocation;
            this._EmptyLocationsList.ScreenName = "F4100Empty_List";
            this._EmptyLocationsList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.EmptyLocationList_EnterEvent);
            this._EmptyLocationsList.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._EmptyLocationsList_ExitValidationEvent);
            this._EmptyLocationsList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._EmptyLocationsList_LoadEvent);
            this._EmptyLocationsList.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._EmptyLocationsList_LeavingEvent);
            // 
            // _OverReceiptWarning
            // 
            this._OverReceiptWarning.ScreenName = "OverReceiptWarning";
            this._OverReceiptWarning.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.OverReceiptWarning_EnterEvent);
            this._OverReceiptWarning.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // _ConfirmNewLicensePlate
            // 
            this._ConfirmNewLicensePlate.ScreenName = "ConfirmNewLicensePlate";
            this._ConfirmNewLicensePlate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmNewLicensePlate_EnterEvent);
            this._ConfirmNewLicensePlate.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmNewLicensePlate_BackEvent);
            this._ConfirmNewLicensePlate.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ApplicationExit_ExitEvent);
            // 
            // MultiItemPOList
            // 
            this.MultiItemPOList.ScreenName = "MultiItemPOList";
            this.MultiItemPOList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.MultiItemPOList_EnterValidationEvent);
            this.MultiItemPOList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.MultiItemPOList_LoadEvent);
            this.MultiItemPOList.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.MultiItemPOList_LeavingEvent);
            // 
            // CollectPackingSlip
            // 
            this.CollectPackingSlip.BackScreen = this._CollectQuantity;
            this.CollectPackingSlip.ScreenName = "CollectPackingSlip";
            this.CollectPackingSlip.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPackingSlip_EnterValidationEvent);
            // 
            // StoneridgePOReceipts
            // 
            this.StartScreen = this._CollectBranch;
            this.WorkflowID = "StoneridgePOReceipts";

        }


        private void _EmptyLocationsList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _EmptyLocationsList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _LocationMasterList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _LocationMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _ItemMasterList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _PurchaseOrderDetailList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _PurchaseOrderDetailList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _UnitOfMeasureConversionList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _UnitOfMeasureConversionList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _PurchaseOrderItemList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _PurchaseOrderItemList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _UserDefinedCodesList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _PurchaseOrderHeaderList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _ItemLocationList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _ItemLocationList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _LicensePlateMasterList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _DefinedPrintersList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }


        #endregion

        #region Initialize

        /// <summary>
        /// Initializes tables, screens, etc.
        /// </summary>
        protected override void Initialize()
        {
            #region Tables
            _ItemLocationTable = new F41021(UserSession.DataAccess, "");
            //_BranchPlantTable = new F0006(UserSession.DataAccess, "");
            _PurchaseOrderHeaderTable = new F4301(UserSession.DataAccess, "");
            _PurchaseOrderDetailTable = new CustomJDETables.F4311(UserSession.DataAccess, "");
            _SerialTable = new CustomJDETables.F5846001(UserSession.DataAccess, "");
            // * BD025857: Base - PO Receipts turns on all datacolumns for the F4311 
            // An earlier SCR that denoted drop file differences in the order of the columns
            // between Receipts by Item and Receipts by Order was addressed by the following line
            // _PurchaseOrderDetailTable.Columns.AddColumnsToTable();
            // however, this caused problems when running against 8.11 and 8.12 
            // changed to ColumnsForSelect in order to retrieve in same sequence each time
            AddColumnsForLabelDropFile();

            _UserDefinedCodesTable = new F0005(UserSession.DataAccess, "");
            _UnitOfMeasureConversionTable1 = new F41002(UserSession.DataAccess, "");
            _UnitOfMeasureConversionTable2 = new F41003(UserSession.DataAccess, "");
            _ItemBranchTable = new F4102(UserSession.DataAccess, "");
            _LocationMasterTable = new F4100(UserSession.DataAccess, "");
            _LotMasterTable = new F4108(UserSession.DataAccess, "");
            _ReceiptRouteTable = new F43090(UserSession.DataAccess, "");
            _DataManipulatorTable = new RFS_DataManipulator(UserSession.DataAccess, "");
            _ItemCrossReferenceTable = new F4104(UserSession.DataAccess, "");
            _BusinessDataRecordLockTable = new RFS_ERPRecordLocks(UserSession.DataAccess, "");
            _DefinedPrintersTable = new RFS_PrinterDefinition(UserSession.DataAccess, "");
            _PrinterCrossReferenceTable = new RFS_PrintXref(UserSession.DataAccess, "");
            //_WarehouseConstants = new F41001(UserSession.DataAccess, "");
            _ItemUOMProfileTable = new F46011(UserSession.DataAccess, "");
            _NextNumberTable = new RFS_NextNumber(UserSession.DataAccess, "");
            _LicensePlateNumberingConstantsTable = new F46L001(UserSession.DataAccess, "");
            _AddressBookTable = new F0101(UserSession.DataAccess, "");
            _SalesOrderTable = new F4211(UserSession.DataAccess, "");
            #endregion
            #region Screens
            // screens
            #endregion
            #region Screen Events
            // screen events
            // Events For CollectPrinter
            // Events For PrintBarCodeLabels
            #endregion
            #region WFOptions
            char[] Delimiter = { ' ' };
            _AllowedNextStatusCodes = WFOptions.RetrieveOptionValue("IncomingStatus").Split(Delimiter);
            _SalesOrderStatusCodes = WFOptions.RetrieveOptionValue("SalesOrderStatus").Split(Delimiter);
            _NonStockLineTypes = WFOptions.RetrieveOptionValue("NonStockLineTypes").Split(Delimiter);
            _AllowedDocTypes = WFOptions.RetrieveOptionValue("AllowedDocumentTypes").Split(Delimiter);
            // license plate tables
            if (LicensePlating())
            {
                _LicensePlateMasterTable = new F46L10(UserSession.DataAccess, "");
                _LPLicense = true; //CC025677
            }
            #endregion

            _FromSerialNumberEntry = false;

            CreateDataSet();
        }

        #endregion

        #region AddColumnsForLabelDropFile
        /// <summary>
        /// Insures that columns in drop file are in same sequence for receipts by item and receipts by order
        /// </summary>
        private void AddColumnsForLabelDropFile()
        {
            //Adding all columns to the underlying data table prior to running any queries to preserver the order for label printing\
            //and taking into account the column types so that the correct data manipulation will be applied
            _PurchaseOrderDetailTable.Columns.AddColumnsToTable();
        }
        #endregion

        #region Application Exit

        /// <summary>
        /// Clears out any default input and exits application
        /// </summary>
        private void ApplicationExit_ExitEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_BusinessDataLocked)
            {
                _PurchaseOrderDetailTable.UnLockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment);
            }
            _BusinessDataLocked = false;
            Exit();
        }

        #endregion


        #region BackEvent_UnlockERP

        /// <summary>
        /// Clears out any default input and backs up 1 screen
        /// </summary>
        private void BackEvent_UnlockERP(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_BusinessDataLocked)
            {
                _PurchaseOrderDetailTable.UnLockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment);
            }
            _BusinessDataLocked = false;
            Back();
        }

        #endregion

        #region CollectBranch

        #region Validation Method

        protected bool ValidateBranch(string sBranch)
        {
            UserSession.ScreenHandler.DefaultText = "";

            // allow a blank branch if performing receipts by order.
            // Receipts by item requires a branch to determine primary item number
            if ((sBranch.Trim().Length > 0) || (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper() == "I"))
            {
                // validate the Branch entered against the F0006 table
                _CollectBranch.BranchPlantsTable.ResetAll();
                _CollectBranch.BranchPlantsTable.BranchPlant = sBranch;
                if (_CollectBranch.BranchPlantsTable.ValidBranchPlant())
                {
                    _BranchPlant = _DataManipulatorTable.FormatData("F0006", "MCMCU", _CollectBranch.BranchPlantsTable.MCMCU);
                    _DetailBranchPlant = _BranchPlant; // initialize
                    LoadBranchPlantData(); //CC025852
                    _CollectBranch.ScreenInput = _BranchPlant.Trim();
                    return true;
                }
                else
                {
                    this._CollectBranch.ErrorMessage = "MCU";
                    return false;
                }
            }
            else
            {
                // blank branch and receiving by order
                return true;
            }
        }

        #endregion

        //CC025852
        private void LoadBranchPlantData()
        {
            UserSession.ScreenHandler.ScreenMap.Add("Branch", _DetailBranchPlant);
            UserSession.ScreenHandler.ScreenMap.Add("DetailBranchPlant", _DetailBranchPlant);
            _CollectBranch.WarehouseConstantsTable.DeterminePrimaryItem(_DetailBranchPlant);
            _LocationSeparator = _CollectBranch.LocationSeparator;
            // if doing receipts by order, prompt for order number next,
            // otherwise, prompt for item number next
            //UserSession.ScreenHandler.DefaultText = this.WorkflowOptions["ReceiptType"].ToString().ToUpper();
            _LicensePlateNumberingConstantsTable.ResetAll();
            _LicensePlateNumberingConstantsTable.BranchPlant = _DetailBranchPlant;

        }

        private void _CollectBranch_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (WFOptions.RetrieveOptionValue("ReceiptType") == "P")
            {
                Next(_CollectOrderNumber);
            }
            else
            {
                Next(_CollectByItem);
            }
        }

        private void BranchPlantList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CollectBranch.BranchPlantsTable.UpdateCurrentRow();
            _BranchPlant = _DataManipulatorTable.FormatData("F0006", "MCMCU", _CollectBranch.BranchPlantsTable.MCMCU);
            ValidateBranch(_CollectBranch.BranchPlantsTable.MCMCU);
        }


        #endregion

        #region CollectOrderNumber

        private void CollectOrderNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DocumentNumber = (string)e.UserInput;
            try
            {
                if (_DocumentNumber.Trim().Length == 0)
                {
                    Error("BLANKSNOTALLOWED");
                }
                else
                {
                    Log.Debug($"(***) {_DocumentNumber}");
                    /*****************************************************
                     Check to see if MANMAN PO Number exists in 4311 table
                     *****************************************************/
                    double PONumber = 0;
                    if(double.TryParse(e.UserInput, out PONumber))
                    {
                        _PurchaseOrderDetailTable.ResetAll();
                        _PurchaseOrderDetailTable.PDDOCO = PONumber;
                        _PurchaseOrderDetailTable.PDNXTR = "400";
                        _PurchaseOrderDetailTable.Fetch();
                        if (_PurchaseOrderDetailTable.Rows.Count > 0)
                        {
                            _DocumentNumber = _PurchaseOrderDetailTable.PDDOCO.ToString();
                        }
                        else
                        {
                            _PurchaseOrderDetailTable.ResetAll();
                            _PurchaseOrderDetailTable.PDVR01 = e.UserInput;
                            _PurchaseOrderDetailTable.PDNXTR = "400";
                            _PurchaseOrderDetailTable.Fetch();
                            if (_PurchaseOrderDetailTable.Rows.Count > 0)
                            {
                                PONumber = Convert.ToDouble(_PurchaseOrderDetailTable.PDDOCO);
                                _DocumentNumber = _PurchaseOrderDetailTable.PDDOCO.ToString();
                                UserSession.ScreenHandler.ScreenMap.Add("ORDERNUMBER", _DocumentNumber);
                            }
                        }
                    }
                    else
                    {
                        _PurchaseOrderDetailTable.ResetAll();
                        _PurchaseOrderDetailTable.PDVR01 = e.UserInput;
                        _PurchaseOrderDetailTable.PDNXTR = "400";
                        _PurchaseOrderDetailTable.Fetch();
                        if (_PurchaseOrderDetailTable.Rows.Count > 0)
                        {
                            PONumber = Convert.ToDouble(_PurchaseOrderDetailTable.PDDOCO);
                            _DocumentNumber = _PurchaseOrderDetailTable.PDDOCO.ToString();
                            UserSession.ScreenHandler.ScreenMap.Add("ORDERNUMBER", _DocumentNumber);
                        }
                    }
                    /***********************************************************/
                    //CC039477: CBI PO receipts header/detail branch issue--use F4311 branch instead of F4301 branch
                    _PurchaseOrderDetailTable.ResetAll();
                    if (_BranchPlant.Trim().Length > 0)
                        _PurchaseOrderDetailTable.PDMCU = _BranchPlant;

                    _PurchaseOrderDetailTable.PDDOCO = PONumber;
                    _PurchaseOrderDetailTable.Fetch();
                    Log.Debug("(***) PO Detail Fetch");

                    //// validate the order number entered against the P/O header table
                    //_PurchaseOrderHeaderTable.ResetAll();

                    //if (_BranchPlant.Trim().Length > 0) //CC025852
                    //    _PurchaseOrderHeaderTable.PHMCU = _BranchPlant;

                    //_PurchaseOrderHeaderTable.PHDOCO = Convert.ToDouble(e.UserInput);
                    //_PurchaseOrderHeaderTable.Fetch();
                    //if (_PurchaseOrderHeaderTable.Rows.Count > 0)
                    if (_PurchaseOrderDetailTable.Rows.Count > 0)
                    {
                        if (JDEReservationTable.DoJDERecordLocksExist("F4311", _PurchaseOrderDetailTable.PDDOCO.ToString(), _PurchaseOrderDetailTable.PDDCTO, _PurchaseOrderDetailTable.PDKCOO))
                        {
                            Error("RECORDRESERVED");
                            return;
                        }

                        if (!_PurchaseOrderDetailTable.ShowOpenLines(
                                _AllowedNextStatusCodes,
                                _PurchaseOrderDetailTable.PDDOCO.ToString(),
                                null,
                                null,
                                _BranchPlant))
                        {
                            Error("NoOpenLines");
                            return;
                        }

                        
                        _PurchaseOrderDetailTable.ResetAll();
                        _PurchaseOrderDetailTable.PDDOCO = PONumber;
                        _PurchaseOrderDetailTable.PDNXTR = "400";
                        if (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper() == "I")
                            _PurchaseOrderDetailTable.PDITM = _ShortItemNumber;

                        if (_BranchPlant.Trim().Length > 0) //CC025852
                            _PurchaseOrderDetailTable.PDMCU = _BranchPlant;

                        _PurchaseOrderDetailTable.Fetch();
                        if (_PurchaseOrderDetailTable.Rows.Count == 0)
                        {
                            if (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper() == "I")
                                Error("ITEMNOTONORDER");
                            else
                            {
                                Error("ORDERNUMBER", PONumber.ToString());
                            }
                                
                            return;
                        }
                        ScreenMap.Add("ORDERNUMBER", _PurchaseOrderDetailTable.PDDOCO.ToString());
                        // if the document number is unique across all document types, then bypass
                        // the document type screen
                        if (WFOptions.GetYesNoWFOptionValue("UniqueDocNumber"))
                        {
                            _DocumentType = _PurchaseOrderDetailTable.PDDCTO;
                            UserSession.ScreenHandler.ScreenMap.Add("DOCTYPE", _DocumentType);
                            Log.Debug($"(***) ValidDocumentType = {ValidDocumentType()}");
                            if (ValidDocumentType())
                            {
                                _PurchaseOrderHeaderTable.ResetAll();
                                _PurchaseOrderHeaderTable.PHDOCO = _PurchaseOrderDetailTable.PDDOCO;
                                _PurchaseOrderHeaderTable.PHDCTO = _PurchaseOrderDetailTable.PDDCTO;
                                _PurchaseOrderHeaderTable.PHKCOO = _PurchaseOrderDetailTable.PDKCOO;
                                _PurchaseOrderHeaderTable.Fetch();

                                //Next(_CollectContainerNumber);
                                int DistinctItemCount;
                                using (var DistinctItemCountQuery = new CustomJDETables.F4311(DataAccess, "DistinctItemCountQuery"))
                                {
                                    DistinctItemCountQuery.DistinctItemPO(_PurchaseOrderDetailTable.PDKCOO, _PurchaseOrderDetailTable.PDDOCO, _PurchaseOrderDetailTable.PDDCTO, _AllowedNextStatusCodes);
                                    DistinctItemCount = DistinctItemCountQuery.Rows.Count;
                                }
                                Log.Debug($"(***) DistinctItemCount {DistinctItemCount}");
                                if (DistinctItemCount == 1)
                                {
                                    _CollectContainerNumber.DefaultTextForScreen = _PurchaseOrderDetailTable.PDCNID;   //What is container number
                                    _CollectOrderType.DefaultTextForScreen = _PurchaseOrderDetailTable.PDDCTO;
                                    _CollectSupplierRemarks.DefaultTextForScreen = "";
                                    _CollectItemNumber.DefaultTextForScreen = _PurchaseOrderDetailTable.PDLITM;
                                    Next(_CollectContainerNumber);
                                }
                                else
                                {
                                    if (DistinctItemCount > 1)
                                        Next(MultiItemPOList);
                                    else
                                        Error("NoOpenLines");
                                }
                            }
                            else
                            {
                                Error("ORDERNUMBER", PONumber.ToString());
                            }
                        }
                    }
                    else
                        Error("INVALIDPONUMBER", _DocumentNumber);

                }
            }
            catch
            {
                Error("CatchInvalidPONumber", _DocumentNumber);
            }
        }

        private void PurchaseOrderHeaderList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // CC038953: Kinross - PO Receipt workflow error 
            //  If receiving by-item, we are querying F4311 and not F4301.
            //  So, make sure we do set F4301 info.
            switch (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper())
            {
                case "P": // receipts by P/O
                    _PurchaseOrderHeaderTable.UpdateCurrentRow();
                    break;
                case "I": // receipts by Item 
                    _PurchaseOrderDetailTable.UpdateCurrentRow();

                    _PurchaseOrderHeaderTable.ResetAll();
                    _PurchaseOrderHeaderTable.PHKCOO = _PurchaseOrderDetailTable.PDKCOO;
                    _PurchaseOrderHeaderTable.PHDOCO = _PurchaseOrderDetailTable.PDDOCO;
                    _PurchaseOrderHeaderTable.PHDCTO = _PurchaseOrderDetailTable.PDDCTO;
                    _PurchaseOrderHeaderTable.Fetch();

                    break;
            }

            _DocumentNumber = _PurchaseOrderHeaderTable.PHDOCO.ToString();
            _DocumentType = _PurchaseOrderHeaderTable.PHDCTO;

            UserSession.ScreenHandler.ScreenMap.Add("ORDERNUMBER", _DocumentNumber);
            UserSession.ScreenHandler.ScreenMap.Add("DOCTYPE", _DocumentType);
            if (JDEReservationTable.DoJDERecordLocksExist("F4311", _PurchaseOrderHeaderTable.PHDOCO.ToString(), _PurchaseOrderHeaderTable.PHDCTO, _PurchaseOrderHeaderTable.PHKCOO))
            {
                Error("RECORDRESERVED");
            }
            else
            {
                if (ValidDocumentType())
                {
                    //Next(_CollectContainerNumber);
                    _PurchaseOrderDetailTable.ResetAll();
                    _PurchaseOrderDetailTable.PDKCOO = _PurchaseOrderHeaderTable.PHKCOO;
                    _PurchaseOrderDetailTable.PDDOCO = _PurchaseOrderHeaderTable.PHDOCO;
                    _PurchaseOrderDetailTable.PDDCTO = _PurchaseOrderHeaderTable.PHDCTO;
                    _PurchaseOrderDetailTable.Fetch();
                    int DistinctItemCount;
                    using (var DistinctItemCountQuery = new CustomJDETables.F4311(DataAccess, "DistinctItemCountQuery"))
                    {
                        DistinctItemCountQuery.DistinctItemPO(_PurchaseOrderDetailTable.PDKCOO, _PurchaseOrderDetailTable.PDDOCO, _PurchaseOrderDetailTable.PDDCTO, _AllowedNextStatusCodes);
                        DistinctItemCount = DistinctItemCountQuery.Rows.Count;
                    }
                    if (DistinctItemCount == 1)
                    {
                        _CollectContainerNumber.DefaultTextForScreen = _PurchaseOrderDetailTable.PDCNID;   //What is container number
                        _CollectSupplierRemarks.DefaultTextForScreen = "";
                        _CollectItemNumber.DefaultTextForScreen = _PurchaseOrderDetailTable.PDLITM;
                        Next(_CollectContainerNumber);
                    }
                    else
                    {
                        Next(MultiItemPOList);
                    }
                }
                else
                    Error("ORDERNUMBER", e.UserInput.ToString());
            }

        }



        #endregion

        #region Collect Packing Slip Info
        private void CollectPackingSlip_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PackingSlip = e.UserInput.ToString().ToUpper();
            ProcessThisReceipt();
        }
        #endregion

        #region CollectOrderType

        private void CollectOrderType_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // validate the order number and order type against the P/O header table
            _DocumentType = e.UserInput.ToString().ToUpper();
            _PurchaseOrderHeaderTable.ResetAll();

            _PurchaseOrderHeaderTable.PHDOCO = Convert.ToDouble(_DocumentNumber);
            _PurchaseOrderHeaderTable.PHDCTO = _DocumentType;
            _PurchaseOrderHeaderTable.Fetch();
            if (_PurchaseOrderHeaderTable.Rows.Count > 0)
            {
                _PurchaseOrderDetailTable.ResetAll();
                _PurchaseOrderDetailTable.PDDOCO = Convert.ToDouble(_DocumentNumber);
                _PurchaseOrderDetailTable.PDDCTO = _DocumentType;
                _PurchaseOrderDetailTable.Fetch();
                if (_PurchaseOrderDetailTable.Rows.Count == 0)
                {
                    Error("ORDERNUMBER", e.UserInput.ToString());
                    return;
                }
                if (!ValidDocumentType())
                {
                    Error("ORDERNUMBER", e.UserInput.ToString());
                    return;
                }
            }
            else
                Error("INVALIDPONUMBER", _DocumentType);

        }



        private void UserDefinedCodesList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UserDefinedCodesTable.UpdateCurrentRow();
            _DocumentType = _UserDefinedCodesTable.DRKY.Trim();
            _PurchaseOrderHeaderTable.ResetAll();
            _PurchaseOrderHeaderTable.PHMCU = _CollectBranch.BranchPlantsTable.MCMCU;
            _PurchaseOrderHeaderTable.PHDOCO = Convert.ToDouble(_DocumentNumber);
            _PurchaseOrderHeaderTable.PHDCTO = _DocumentType;
            _PurchaseOrderHeaderTable.Fetch();
            if (_PurchaseOrderHeaderTable.Rows.Count > 0)
            {
                if (ValidDocumentType())
                {
                    Next(_CollectContainerNumber);
                }
                else
                {

                    Error("ORDERNUMBER", _DocumentType);
                }
            }
            else
            {
                Error("INVALIDPONUMBER", _DocumentType);
            }
        }



        #endregion

        #region CollectContainerNumber

        private void CollectContainerNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _Container = (string)e.UserInput;

        }

        #endregion

        #region CollectSupplierRemarks

        private void CollectSupplierRemarks_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _Remarks = (string)e.UserInput;

        }

        #endregion

        #region CollectItemNumber

        protected bool ValidateItemNumber(string sItem)
        {
            Log.Debug("(***) ValidateItemNumber");
            bool wasItemFound = true;
            int iItem = 0;
            // if the item number is blank,
            // then prompt the user for the line number
            if ((sItem.Trim().Length == 0) &&
               (WFOptions.RetrieveOptionValue("ReceiptType") == "P"))
            {
                Next(_CollectLineNumber);
                return true;
            }// this case fixes a workflow error when item is > an int 
           else if (_CollectBranch.WarehouseConstantsTable._DesignatedPrimaryItemNumber == PrimaryItemNumberDesignations.ShortNumber && 
                   (!int.TryParse(sItem, out iItem)))
            {   
                _CollectItemNumber.ErrorMessage = "ITEMNUMBER";  
                return false;
            }

            _CollectItemNumber.ItemsTable.ResetAll();
            _ItemCrossReferenceTable.ResetAll();
            // use the returned result to display the proper item number
            _PrimaryItemNumber = _CollectItemNumber.ItemsTable.RetrieveItem(_BranchPlant, sItem, _CollectBranch.PrimaryItemDesignator.ToString());
            if (_PrimaryItemNumber == null)
            {
                wasItemFound = false;
                _PrimaryItemNumber = sItem.Trim();  // for non-stock
                _ShortItemNumber = 0; //CS8396 - Added ShortItemNumber so it is no longer conditionally set and validation fetch will be correctly checked; fixes 'item not on order' error
            }
            else
            {
                _ShortItemNumber = _CollectItemNumber.ItemsTable.IMITM; //CS8396 - Added else when PrimaryItemNumber is not null to assign F4101.IMITM value to ShortItemNumber variable so it is no longer conditionally set and validation fetch will be correctly checked  - fixes 'item not on order' error
            }
            if (_PrimaryItemNumber != null)
            {
                SetUpPOSQL(_PrimaryItemNumber);
                _PurchaseOrderDetailTable.Fetch();
                if (_PurchaseOrderDetailTable.Rows.Count > 0)
                {
                    if (_PurchaseOrderDetailTable.IsLocked(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment))
                    {
                        DisplayLockInformation();
                        // Line is locked, but return true so as not to signal error
                        // to screen handler.
                        return true;
                    }
                    else
                    {
                        if (_PurchaseOrderDetailTable.LockRecord(_BusinessDataRecordLockTable,
                           UserSession.ScreenHandler.Environment,
                           UserSession.CurrentUser.LoginName,
                           UserSession.ScreenHandler.WorkflowID))
                        {
                            _BusinessDataLocked = true;
                            // ERPLocks.IsRecordLocked(ThisUserSession.ScreenHandler.Environment.ToString(),  PODetail.DataAccess.Data.TableOwner.ToString()   , "F4311", " ");
                            _LineNumber = _PurchaseOrderDetailTable.PDLNID;
                            _ShortItemNumber = _PurchaseOrderDetailTable.PDITM;
                            _LongItemNumber = _PurchaseOrderDetailTable.PDLITM;
                            _ThirdItemNumber = _PurchaseOrderDetailTable.PDAITM;
                            _DefaultLocation = _PurchaseOrderDetailTable.PDPDP4;
                            IsBackordered(_ShortItemNumber);
                            GetItemMaster();
                            _ItemBranchTable.ResetAll();
                            if (StockItem())
                            {
                                if (_ItemBranchTable.ValidateItemBranch(_DetailBranchPlant, _ShortItemNumber))
                                {
                                    _LotProcessType = _ItemBranchTable.IBSRCE;
                                    ItemEntryCompleted();
                                    return true;
                                }
                                else
                                {
                                    _CollectItemNumber.DefaultTextForScreen = sItem;
                                    _CollectItemNumber.ErrorMessage = "ITEMBRANCH";
                                    return false;
                                }

                            }
                            else
                            {
                                _LotProcessType = "";
                                ItemEntryCompleted();
                                return true;
                            }
                        }
                        return true;
                    }
                }
                else if (!wasItemFound)
                {
                    _CollectItemNumber.ErrorMessage = "ITEMNUMBER";
                    return false;
                }
                //CS8396 - Changed elseif to ReceiptType is not an item on order to fix item not on order error
                else if (WFOptions.RetrieveOptionValue("ReceiptType") == "I" || !_PurchaseOrderDetailTable.IsItemOnOrder(_DetailBranchPlant, _ShortItemNumber, double.Parse(_DocumentNumber)))
                {
                    //Could not find order in F4311/PO Details
                    _CollectItemNumber.ErrorMessage = "ITEMNOTONORDER";
                    return false;
                }
                else
                {
                    _CollectItemNumber.ErrorMessage = "ITEMCOMPLETE";
                    return false;
                }
            }
            else
            {
                // primary item number was null
                _CollectItemNumber.ErrorMessage = "ITEMNUMBER";
                return false;
            }

        }


        #region ItemEntryCompleted
        private void ItemEntryCompleted()
        {
            if ((InReceiptRouting()) && (RoutingMessageDisplayed == false))
            {
                RoutingMessageDisplayed = true;
                Next(_ReceiptRoutingMessage);
                return;
            }
            else
            {
                // do not need to collect location on non-stock item
                if (!StockItem())
                {
                    _LotProcessType = "";
                    _LotNumber = "";
                    _Location = "";
                    Log.Debug("(***) _CollectQTY");
                    //Next(CollectPackingSlip);
                    Next(_CollectQuantity);
                    return;
                }
                if (_LotProcessType.Trim().Length > 0)
                {
                    if (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper() == "P")
                    {
                        if (PromptForLot())
                        {
                            Next(_CollectLotNumber);
                            return;
                        }
                        if (PromptForMemoLot())
                        {
                            Next(_CollectMemoLot1);
                            return;
                        }
                    }
                }

                _LotNumber = " ";
                DefaultPrimary();

            }
        }

        #endregion

        private void CollectItemNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _SearchType = "ByItem";
            switch (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper())
            {
                case "P": //CC025852
                    _PurchaseOrderDetailTable.ShowOpenLines(_AllowedNextStatusCodes, _DocumentNumber, _DocumentType, "", _BranchPlant);
                    break;
                case "I": //CC025852
                    _PurchaseOrderDetailTable.ShowOpenItems(_AllowedNextStatusCodes, _BranchPlant, e.UserInput.ToString().ToUpper());
                    break;
            }

        }

        private void PurchaseOrderDetailList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PurchaseOrderDetailTable.UpdateCurrentRow();
            _PurchaseOrderDetailTable.ShowOpenLines(
                _AllowedNextStatusCodes,
                _DocumentNumber,
                _DocumentType,
                _PurchaseOrderDetailTable.PDLNID.ToString(),
                _BranchPlant);
            if (_PurchaseOrderDetailTable.IsLocked(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment))
            {
                DisplayLockInformation();
            }
            else if (_PurchaseOrderDetailTable.LockRecord(_BusinessDataRecordLockTable,
               UserSession.ScreenHandler.Environment,
               UserSession.CurrentUser.LoginName,
               UserSession.ScreenHandler.WorkflowID))
            {

                _BusinessDataLocked = true;
                _LineNumber = _PurchaseOrderDetailTable.PDLNID;
                _ShortItemNumber = _PurchaseOrderDetailTable.PDITM;
                _LongItemNumber = _PurchaseOrderDetailTable.PDLITM;
                _ThirdItemNumber = _PurchaseOrderDetailTable.PDAITM;
                IsBackordered(_ShortItemNumber);
                UserSession.ScreenHandler.ScreenMap.Add("ORDERNUMBER", _PurchaseOrderDetailTable.PDDOCO.ToString());
                UserSession.ScreenHandler.ScreenMap.Add("DOCTYPE", _PurchaseOrderDetailTable.PDDCTO);
                GetItemMaster();
                UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _PrimaryItemNumber);
                _ItemBranchTable.ResetAll();

                if (StockItem())
                {
                    if (_ItemBranchTable.ValidateItemBranch(_DetailBranchPlant, _ShortItemNumber))
                        ShowNextScreen();
                    else
                        Error("ITEMBRANCH", e.UserInput.ToString());
                }
                else
                    ShowNextScreen();
            }
            else
                Error("UNABLETOLOCK", e.UserInput.ToString());
        }

        private void IsBackordered(double _ShortItemNumber)
        {
            if (_SalesOrderTable.IsItemBackOrdered(_BranchPlant, _ShortItemNumber, _SalesOrderStatusCodes))
            {
                ScreenMap.Add("Backordered", ScreenHandler.RetrieveVocabularyOverride("Backordered"));
            }
            else
            {
                ScreenMap.Add("Backordered", "");
            }
        }


        #endregion

        #region f4311ItemList

        private void PurchaseOrderItemList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PurchaseOrderDetailTable.UpdateCurrentRow();
            _PurchaseOrderDetailTable.ShowOpenLines(
                _AllowedNextStatusCodes,
                _DocumentNumber,
                _DocumentType,
                _PurchaseOrderDetailTable.PDLNID.ToString(),
                _BranchPlant);
            if (_PurchaseOrderDetailTable.IsLocked(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment))
                DisplayLockInformation();
            else
            {
                if (_PurchaseOrderDetailTable.LockRecord(_BusinessDataRecordLockTable,
                    UserSession.ScreenHandler.Environment,
                    UserSession.CurrentUser.LoginName,
                    UserSession.ScreenHandler.WorkflowID))
                {
                    _BusinessDataLocked = true;
                    _LineNumber = _PurchaseOrderDetailTable.PDLNID;
                    _ShortItemNumber = _PurchaseOrderDetailTable.PDITM;
                    _LongItemNumber = _PurchaseOrderDetailTable.PDLITM;
                    _ThirdItemNumber = _PurchaseOrderDetailTable.PDAITM;
                    IsBackordered(_ShortItemNumber);
                    if (!StockItem())
                    {
                        _LotProcessType = "";
                        _Location = "";
                        _LotNumber = "";
                        ScreenMap.Add("ITEMNUMBER", _PurchaseOrderDetailTable.PDLITM);
                        Next(_CollectQuantity);
                        return;
                    }
                    GetItemMaster();
                    UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _PrimaryItemNumber);

                    _ItemBranchTable.ResetAll();
                    if (_ItemBranchTable.ValidateItemBranch(_DetailBranchPlant, _ShortItemNumber))
                    {
                        switch (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper())
                        {
                            case "P": // receipts by P/O
                                ShowNextScreen();
                                break;
                            case "I":
                                _DocumentNumber = _PurchaseOrderDetailTable.PDDOCO.ToString();
                                _DocumentType = _PurchaseOrderDetailTable.PDDCTO;
                                UserSession.ScreenHandler.ScreenMap.Add("ORDERNUMBER", _PurchaseOrderDetailTable.PDDOCO.ToString());
                                UserSession.ScreenHandler.ScreenMap.Add("DOCTYPE", _PurchaseOrderDetailTable.PDDCTO);
                                Next(_CollectContainerNumber);
                                break;
                        }
                    }
                    else
                    {
                        Error("ITEMBRANCH");
                        return;
                    }
                }
                else
                    Error("UNABLETOLOCK", e.UserInput.ToString());
            }
        }

        #endregion

        #region CollectByItem

        private void CollectByItem_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToString().Trim().Length == 0)
            {
                Error("ITEMNUMBER", e.UserInput.ToString());
                return;
            }
            _CollectItemNumber.ItemsTable.ResetAll();
            _PrimaryItemNumber = e.UserInput.ToString();
            switch (_CollectBranch.PrimaryItemDesignator)
            {
                case PrimaryItemNumberDesignations.ShortNumber:
                    _CollectItemNumber.ItemsTable.IMITM = Convert.ToDouble(_PrimaryItemNumber);
                    break;
                case PrimaryItemNumberDesignations.LongNumber:
                    _CollectItemNumber.ItemsTable.IMLITM = _PrimaryItemNumber;
                    break;
                case PrimaryItemNumberDesignations.ThirdNumber:
                    _CollectItemNumber.ItemsTable.IMAITM = _PrimaryItemNumber;
                    break;
            }
            if (_CollectItemNumber.ItemsTable.RetrieveItem(_BranchPlant, _PrimaryItemNumber, _CollectBranch.PrimaryItemDesignator.ToString()) != null)
            {

                _ShortItemNumber = _CollectItemNumber.ItemsTable.IMITM;
                _ItemBranchTable.ResetAll();
                if (_ItemBranchTable.ValidateItemBranch(_DetailBranchPlant, _ShortItemNumber))
                {
                    _LotProcessType = _ItemBranchTable.IBSRCE;
                    if (_ItemBranchTable.IsItemCurrentlyOnOrder(_BranchPlant, _ShortItemNumber))
                    {
                        ScreenMap.Add("ITEMNUMBER", _PrimaryItemNumber);
                        IsBackordered(_ShortItemNumber);
                        Next(_CollectOrderNumber);
                        return;
                    }
                    else
                    {
                        Error("ITEMNOTONORDER", e.UserInput.ToString());
                        return;

                    }

                }
                else
                    Error("ItemBranch", e.UserInput.ToString());
            }
            else
                Error("ITEMNUMBER", e.UserInput.ToString());

        }



        private void ItemMasterList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CollectItemNumber.ItemsTable.UpdateCurrentRow();
            _ShortItemNumber = _CollectItemNumber.ItemsTable.IMITM;
            _LongItemNumber = _CollectItemNumber.ItemsTable.IMLITM;
            _ThirdItemNumber = _CollectItemNumber.ItemsTable.IMAITM;

            switch (_CollectBranch.PrimaryItemDesignator)
            {
                case PrimaryItemNumberDesignations.ShortNumber:
                    UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ShortItemNumber.ToString());
                    _PrimaryItemNumber = _ShortItemNumber.ToString();
                    break;
                case PrimaryItemNumberDesignations.LongNumber:
                    UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _LongItemNumber);
                    _PrimaryItemNumber = _LongItemNumber;
                    break;
                case PrimaryItemNumberDesignations.ThirdNumber:
                    UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ThirdItemNumber);
                    _PrimaryItemNumber = _ThirdItemNumber;
                    break;
            }
            _ItemBranchTable.ResetAll();
            if (_ItemBranchTable.ValidateItemBranch(_DetailBranchPlant, _ShortItemNumber))
            {
                IsBackordered(_ShortItemNumber);
                _LotProcessType = _ItemBranchTable.IBSRCE;
                Next(_CollectOrderNumber);
            }
            else
            {
                Error("ITEMBRANCH");
            }
        }


        #endregion

        #region LicensePlateMasterList
        private void LicensePlateMasterList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlateMasterTable.UpdateCurrentRow();
            _LicensePlate = _LicensePlateMasterTable.LMLPNU;
            LicensePlateEnter();

        }
        private void LicensePlateMasterList_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(_CollectLicensePlate);
        }
        private void LicensePlateMasterList_ExitEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(_CollectLicensePlate);
        }

        #endregion

        #region ReceiptRoutingMessage

        private void ReceiptRoutingMessage_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotProcessType = _ItemBranchTable.IBSRCE;
            // check to see if this is a lot controlled item, and if so, display the lot
            // verification screen
            _Location = _ReceiptRouteTable.PCNROU;
            ShowNextScreen();
        }

        private void ReceiptRoutingMessage_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            RoutingMessageDisplayed = false;
            if (_BusinessDataLocked)
            {
                _PurchaseOrderDetailTable.UnLockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment);
            }
            _BusinessDataLocked = false;
            if (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper() == "P")
            {
                Next(_CollectItemNumber);
            }
            else
            {
                Back(_CollectSupplierRemarks);
            }

        }
        #endregion

        #region CollectLineNumber

        private void CollectLineNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                _LineNumber = Convert.ToDouble(e.UserInput.ToString());
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                _LineNumber = 0;
                Error("LINENOTONORDER", e.UserInput.ToString());
                return;
            }
            // if the item number is blank,
            // then prompt the user for the line number
            if (e.UserInput.ToString().Trim().Length == 0)
            {
                Error("BLANKSNOTALLOWED");
            }
            else
            {
                //CC025852
                if (_BranchPlant.Trim().Length > 0)
                    _PurchaseOrderDetailTable.ShowOpenLines(_AllowedNextStatusCodes, _DocumentNumber, _DocumentType, _LineNumber.ToString().Trim(), _BranchPlant);
                else
                    _PurchaseOrderDetailTable.ShowOpenLines(_AllowedNextStatusCodes, _DocumentNumber, _DocumentType, _LineNumber.ToString().Trim());

                if (_PurchaseOrderDetailTable.Rows.Count > 0)
                {
                    if (_PurchaseOrderDetailTable.IsLocked(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment))
                    {
                        DisplayLockInformation();
                    }
                    else
                    {
                        if (_PurchaseOrderDetailTable.LockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment,
                           UserSession.CurrentUser.LoginName,
                           UserSession.ScreenHandler.WorkflowID))
                        {
                            _BusinessDataLocked = true;
                            _LineNumber = _PurchaseOrderDetailTable.PDLNID;
                            _ShortItemNumber = _PurchaseOrderDetailTable.PDITM;
                            _LongItemNumber = _PurchaseOrderDetailTable.PDLITM;
                            _ThirdItemNumber = _PurchaseOrderDetailTable.PDAITM;
                            IsBackordered(_ShortItemNumber);
                            GetItemMaster();
                            _ItemBranchTable.ResetAll();
                            if (!StockItem())
                            {
                                ShowNextScreen();
                                return;
                            }
                            if (_ItemBranchTable.ValidateItemBranch(_DetailBranchPlant, _ShortItemNumber))
                                ShowNextScreen();
                            else
                                Error("ITEMBRANCH");
                        }
                    }
                }
                else
                {
                    LoadFieldInError(_LineNumber.ToString());
                    Error("LINENOTONORDER");
                }
            }
        }

        private void CollectLineNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _SearchType = "ByLine";

            //CC025852
            if (_BranchPlant.Trim().Length > 0)
                _PurchaseOrderDetailTable.ShowOpenLines(_AllowedNextStatusCodes, _DocumentNumber, _DocumentType, "", _BranchPlant);
            else
                _PurchaseOrderDetailTable.ShowOpenLines(_AllowedNextStatusCodes, _DocumentNumber, _DocumentType, "");
        }

        #endregion

        #region CollectLotNumber

        private void CollectLotNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotNumber = e.UserInput.ToString().ToUpper();
            LotNumberEnter();
        }

        private void LotNumberEnter()
        {
            UserSession.ScreenHandler.ScreenMap.Add("LOTNUMBER", _LotNumber);
            if (_LotNumber.Trim().Length == 0)
            {
                // If the Lot is blank, and it is required, throw an error; otherwise,
                // go to the Collect Location screen
                if (LotRequired())
                {
                    Error("LOTREQUIRED");
                    return;
                }
                // if this is a lot optional item and no lot has been entered, bypass date screen
                if (_LotProcessType == "0")
                {
                    DefaultPrimary();
                    Next(_CollectLocation);
                    return;
                }

                return;
            }

            if ((_LotProcessType.Trim().Length > 0) &&
              (_LotProcessType.Trim() == "1" || _LotProcessType.Trim() == "2") &&
              (!WFOptions.GetYesNoWFOptionValue("AllowLotOverride")))
            {
                Error("OverrideNotAllowed");
                return;
            }

            // BD012675 - If the Lot exists within the Lot Master, use the Expiration associated;
            // otherwise, prompt for the Lot expiration
            if (_LotMasterTable.RetrieveLotMaster(_DetailBranchPlant, _ShortItemNumber, _LotNumber))
            {
                DateTime wdt = DateTimeUtils.JDEJulian(_LotMasterTable.IOMMEJ);
                UserSession.ScreenHandler.ScreenMap.Add("ExpDate", DateTimeUtils.JDEJulian(_LotMasterTable.IOMMEJ).ToString(this.WFOptions._WorkflowOptions["DateFormat"].ToString()));
                // If the processing option is set to not allow adjustments to expired lots
                // and the lot is expired - issue error
                if ((DateTimeUtils.JDEJulian(_LotMasterTable.IOMMEJ) < System.DateTime.Now) && (!WFOptions.GetYesNoWFOptionValue("AllowExpiredLotAdj")))
                {
                    Error("EXPIREDLOT");
                    return;
                }
                _ExpirationDate = DateTimeUtils.JDEJulian(_LotMasterTable.IOMMEJ).ToString("yyyyMMdd");
                Next(_CollectMemoLot1);
                return;
            }
            UserSession.ScreenHandler.DefaultText = "";
        }

        private void CollectLotNumber_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            UserSession.ScreenHandler.ScreenMap.Add("LOTNUMBER", "");
            UserSession.ScreenHandler.ScreenMap.Add("ExpDate", " ");
            BackEvent_UnlockERP(sender, e);
        }

        private void CollectLotNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LocationLookupType = "Lot";
            _ItemLocationTable.RetrieveItemLocationRecords(_DetailBranchPlant, _CollectItemNumber.ItemsTable.IMITM, WFOptions.RetrieveOptionValue("ShowZeroOnHand"));

        }

        private void ItemLocationList_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (_LocationLookupType)
            {
                case "Lot":
                    Back(_CollectLotNumber);
                    break;
                case "LocationsForItem":
                    Back(_CollectLocation);
                    break;
                case "SerialNumbersForItem":
                    Back(_CollectSerialNumber);
                    break;
            }
        }
        #endregion

        #region CollectSerialNumber

        private void CollectSerialNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotNumber = e.UserInput.ToString().ToUpper();
            SerialNumberEnter();
        }

        private void SerialNumberEnter()
        {
            if (_LotNumber.Trim().Length == 0)
            {
                if (LotRequired())
                {
                    Error("SNREQUIRED");
                    return;
                }
            }
            else
            {
                if ((_LotProcessType.Trim().Length > 0) &&
                   (_LotProcessType.Trim() == "5" || _LotProcessType.Trim() == "6") &&
                   (!WFOptions.GetYesNoWFOptionValue("AllowLotOverride")))
                {
                    Error("OverrideNotAllowed");
                    return;
                }
            }
            // determine if the serial number scanned exists on a negative adjustment
            _ItemLocationTable.ResetAll();

            _ItemLocationTable.LIMCU = _DetailBranchPlant;
            _ItemLocationTable.LIITM = _CollectItemNumber.ItemsTable.IMITM;
            _ItemLocationTable.LILOTN = _LotNumber;
            if (_ItemLocationTable.ValidateItemLot() == false)
                _ItemLocationTable.LIPQOH = 0;

            if ((Convert.ToInt16(_LotProcessType) > Convert.ToInt16(LotSerialDesignations.SerialOptional))
               && (_ItemLocationTable.LIPQOH > 0))
            {
                LoadFieldInError(_LotNumber);
                Error("SNEXISTS");
                return;
            }



        }
        private void CollectSerialNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LocationLookupType = "SerialNumbersForItem";
            _ItemLocationTable.RetrieveItemLocationRecords(_DetailBranchPlant, _CollectItemNumber.ItemsTable.IMITM, WFOptions.RetrieveOptionValue("ShowZeroOnHand"));

        }
        #endregion

        #region DefaultBarcodeQuantity
        private void DefaultBarcodeQuantity()
        {
            if (_BarcodeUnitOfMeasure.Trim().Length > 0)
            {
                if (_UnitOfMeasureAsEntered.ToUpper().Trim() != _BarcodeUnitOfMeasure.ToUpper().Trim())
                {
                    double WorkQuantity = _UnitOfMeasureConversionTable1.ConvertToDocumentUOM(_DetailBranchPlant,
                       _ShortItemNumber,
                       _UnitOfMeasureAsEntered,
                       _BarcodeUnitOfMeasure,
                       _TotalReceiptQuantity,
                       _CollectItemNumber.ItemsTable.IMUOM1);
                    // if the return value was zero, then a conversion was not found, don't change the quantity
                    if (WorkQuantity != 0)
                    {
                        // quantities less than zero do not make sense for labels
                        if (WorkQuantity < 1)
                            WorkQuantity = 1;
                        UserSession.ScreenHandler.DefaultText = Convert.ToInt32(WorkQuantity).ToString();
                        UserSession.ScreenHandler.ScreenMap.Add("BarcodeUnitOfMeasure", _BarcodeUnitOfMeasure);
                        return;
                    }
                }
            }
            UserSession.ScreenHandler.DefaultText = "";
            //UserSession.ScreenHandler.DefaultText = Convert.ToInt32(_TotalReceiptQuantity).ToString();
            UserSession.ScreenHandler.ScreenMap.Add("BarcodeUnitOfMeasure", _UnitOfMeasureAsEntered);
        }

        #endregion

        #region CollectLotSerialExpDate

        private void CollectLotSerialExpDate_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            DateTimeUtils dtu = new DateTimeUtils();
            _ExpirationDate = "";
            try
            {
                if (e.UserInput.ToString().Trim().Length > 0)
                {
                    // validate date
                    if (!DateTimeUtils.ValidateDate(e.UserInput.ToString(), WFOptions.RetrieveOptionValue("DateFormat")))
                    {
                        LoadFieldInError(e.UserInput.ToString());
                        Error("EXPDATE");
                        return;
                    }
                    _ExpirationDate = dtu.YearAsString + dtu.MonthOfYearAsString + dtu.DayOfMonthAsString;
                    UserSession.ScreenHandler.ScreenMap.Add("ExpDate", e.UserInput.ToString());
                    if ((DateTimeUtils.JDEJulian(Convert.ToDecimal(DateTimeUtils.ConvertToJulian(_ExpirationDate)))) < DateTime.Today)
                    {
                        LoadFieldInError(e.UserInput.ToString());
                        Error("EXPDATE");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                LoadFieldInError(e.UserInput.ToString());
                Error("EXPDATE");
                return;
            }
            _ItemBranchTable.ValidateItemBranch(_DetailBranchPlant, _ShortItemNumber);
            if ((_ItemBranchTable.IBSRCE.Trim().Length > 0) &&
               (Convert.ToInt16(_ItemBranchTable.IBSRCE) >= Convert.ToInt16(LotSerialDesignations.SerialOptional)))
            {
                _FromSerialNumberEntry = true;
                _SerialNumbersScanned += 1;
                _LastDateEntered = dtu.MonthOfTheYear.ToString() + "-" + dtu.DayOfTheMonth.ToString() + "-" + dtu.Year.ToString();
                UserSession.ScreenHandler.ScreenMap.Add("SERIALNUMBERSCAN", _SerialNumbersScanned.ToString());
                // convert quantity to primary for JDE update
                _CollectUnitOfMeasure.Validate(_CollectItemNumber.ItemsTable.IMUOM1);
                _TransactionQuantity = 1;
                _SendFlag = "Now";
                if (JDEReservationTable.DoJDERecordLocksExist("F4311", _PurchaseOrderHeaderTable.PHDOCO.ToString(), _PurchaseOrderHeaderTable.PHDCTO, _PurchaseOrderHeaderTable.PHKCOO))
                {
                    _PurchaseOrderDetailTable.UnLockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment);
                    Error("RECORDRESERVED");
                    RestartLoop(_CollectOrderNumber);
                }
                else if (IsItemSerlialized())
                {
                    Next(_CollectMemoLot1);
                }
                else
                {
                    Log.Debug("Brad5");
                    SendToERP();

                    if (!Transactions.Error)
                    {
                        if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                        {
                            UserSession.ScreenHandler.DefaultText = "1";
                        }
                        Next(_PrintBarCodeLabels);
                    }
                    else
                    {
                        RestartLoop(_CollectBranch);
                    }
                    return;
                }
            }

        }
        private void CollectLotSerialExpDate_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            UserSession.ScreenHandler.ScreenMap.Add("ExpDate", " ");
            Back();
        }
        #endregion

        #region CollectLicensePlate

        #region CollectLicensePlate_LoadEvent

        private void CollectLicensePlate_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if(InReceiptRouting() || !_ItemUOMProfileTable.ALicenPlateItem(_BranchPlant, _ShortItemNumber))
            {
                _CollectLicensePlate.CancelLoad = true;
                _LicensePlate = ""; 
                ProcessThisReceipt();
                return;
            }
            if (WFOptions.GetYesNoWFOptionValue("RFSGenLicensePlateNumber"))
            {
                int NextNumber = 0;
                string FormattedNextNumber = "";

                if (_NextNumberTable.GetNextNumber("LicensePlate", out NextNumber, out FormattedNextNumber))
                {
                    ScreenHandler.DefaultText = FormattedNextNumber.ToString();
                }
                else
                {
                    Log.WriteLine(5, "Could not generate license plate number during CollectLicensePlate screen's Load event.");
                }
            }
        }

        #endregion

        private void CollectLicensePlate_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlate = e.UserInput.ToString().ToUpper();
            LicensePlateEnter();
        }

        private void LicensePlateEnter()
        {
            ScreenMap.Add("LicensePlate", _LicensePlate);
            // First, check to see if we are scanning a license plate (no delimiters)
            if (_LicensePlate.Trim().Length == 0)
            {
                ProcessThisReceipt();
                return;
            }

            // if location separators are present, user scanned location.
            if (_LicensePlate.IndexOf(_LocationSeparator) >= 1)
            {
                _Location = _LicensePlate;
                _LicensePlate = "";
                //ReceivingLocationEnterKey();
                ProcessThisReceipt();
                return;
            }

            // If a license plate,
            //    validate that it exists
            _LicensePlateMasterTable.LMMCU = _BranchPlant;
            _LicensePlateMasterTable.LMLPNU = _LicensePlate;
            if (!_LicensePlateMasterTable.Fetch())
            {
                // If a license plate, validate that it exists.
                // If it doesn't exist, take user to warning screen
                Next(_ConfirmNewLicensePlate);
                return;
            }

            //Validate that the item is valid on the target license plate
            using (F46L11 tempTbl = new F46L11(UserSession.DataAccess, "TempForValidateLPItemToAdd"))
            {
                if (!tempTbl.ValidateLPItemToAdd(_LicensePlate, _ShortItemNumber, _LotNumber))
                {
                    if (!tempTbl.ValidateLPItemToAdd(_LicensePlate, _ShortItemNumber)) Error("ItemNotOnLP");
                    else Error("LotNotOnLP");

                    return;
                }
            }

            _LicensePlateProcessingType = "E";
            if (_Location.Trim() != _LicensePlateMasterTable.LMLOCN.Trim())
            {
                Error("LOCATIONMISMATCH", _LicensePlate);
                return;
            }
            _Location = _LicensePlateMasterTable.LMLOCN;
            // ReceivingLocationEnterKey();
            ProcessThisReceipt();
        }

        private void CollectLicensePlate_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlate = "";
            Back();
        }
        #endregion

        #region ConfirmNewLicensePlate

        private void ConfirmNewLicensePlate_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                if (!e.UserInput.ToBool())
                {
                    ConfirmNewLicensePlate_BackEvent(sender, e);
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                Error("INVALIDINPUTFORTYPE");
                return;
            }

            _FromGenLP = true;
            _LicensePlateProcessingType = "N"; // new license plate

            ProcessThisReceipt();
        }

        private void ConfirmNewLicensePlate_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Clear("LicensePlate");
            _LicensePlate = string.Empty;
            _FromGenLP = false;
            _LicensePlateProcessingType = string.Empty;

            Back();
        }

        #endregion

        #region CollectMemoLot1

        private void CollectMemoLot1_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _MemoLot1 = (string)e.UserInput;
            ScreenMap.Add("MEMOLOT1", _MemoLot1);
        }

        #endregion

        #region CollectMemoLot2

        private void CollectMemoLot2_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _MemoLot2 = (string)e.UserInput;
            ScreenMap.Add("MEMOLOT2", _MemoLot2);
        }

        #endregion

        #region CollectVendorLot

        private void CollectVendorLot_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _VendorLot = (string)e.UserInput;
            ScreenMap.Add("VENDORLOT", _VendorLot);
            if (!IsItemSerlialized())
            {
                DefaultPrimary();
            }
        }

        #endregion

        #region new for 5.1 Koos Lot requirements
        #region Lot Potency
        private bool CollectLotPotency_ValidateMethod(string potency)
        {
            double lotPotency = potency.GetDouble();
            if (lotPotency < _ItemBranchTable.IBFRMP)
            {
                throw new RFS_Exception("UnderMinimum");
            }
            if (lotPotency > _ItemBranchTable.IBTHRP)
            {
                throw new RFS_Exception("OverMaximum");
            }
            _LotPotency = lotPotency.ToString();
            ScreenMap.Add("LOTPOTENCY", _LotPotency);
            Next(CollectLotGrade);
            return true;
        }

        private void CollectLotPotency_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_ItemBranchTable.IBOT1Y != "Y")
            {
                _LotPotency = string.Empty;
                CollectLotPotency.CancelLoad = true;
                Next(CollectLotGrade);
                return;
            }
            if (DefaultLotGradePotency)
            {
                ScreenHandler.DefaultText = _ItemBranchTable.IBSTDP.ToString();
            }
        }
        #endregion
        #region Lot Grade
        private void CollectLotGrade_BackValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(_CollectVendorLot);
        }
        private bool CollectLotGrade_ValidateMethod(string lotGrade)
        {
            lotGrade = lotGrade.Trim().ToUpper();
            if (!_UserDefinedCodesTable.ValidateUDC("40", "LG", lotGrade))
            {
                throw new RFS_Exception("LotGrade");
            }
            if (lotGrade.CompareTo(_ItemBranchTable.IBFRGD.Trim().ToUpper()) < 0)
            {
                throw new RFS_Exception("UnderMinimum");
            }
            if (lotGrade.CompareTo(_ItemBranchTable.IBTHGD.Trim().ToUpper()) > 0)
            {
                throw new RFS_Exception("OverMaximum");
            }
            _LotGrade = _UserDefinedCodesTable.DRKY;
            ScreenMap.Add("LOTGRADE", _LotGrade);
            Next(CollectCountryOfOrigin);
            return true;
        }

        private void CollectLotGrade_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_ItemBranchTable.IBOT2Y != "Y")
            {
                _LotGrade = string.Empty;
                CollectLotGrade.CancelLoad = true;
                Next(CollectCountryOfOrigin);
                return;
            }
            if (DefaultLotGradePotency)
            {
                ScreenHandler.DefaultText = _ItemBranchTable.IBSTDG;
            }
        }

        private void CollectLotGrade_List_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            CollectLotGrade_List.Table = _UserDefinedCodesTable;
            _UserDefinedCodesTable.FetchUDCs("40", "LG");
            // Remove Rows outside of the boundary. Since IBM DB2 Uses EBCDIC we need to remove rows in the C# not the SQL. 
            for (int i = 0; i < _UserDefinedCodesTable.Rows.Count; i++)
            {
                if ((_UserDefinedCodesTable.Rows[i]["DRKY"].ToString().Trim().ToUpper().CompareTo(_ItemBranchTable.IBFRGD.Trim().ToUpper()) < 0) ||
                    (_UserDefinedCodesTable.Rows[i]["DRKY"].ToString().Trim().ToUpper().CompareTo(_ItemBranchTable.IBTHGD.Trim().ToUpper()) > 0))
                {
                    _UserDefinedCodesTable.Rows[i].Delete();
                }
            }
            _UserDefinedCodesTable.ThisDataTable.AcceptChanges();
        }
        #endregion
        #region Country Of Origin
        private void CollectCountryOfOrigin_BackValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_ItemBranchTable.IBOT1Y != "Y")
            {
                Back(_CollectVendorLot);
            }
            else
            {
                Back(CollectLotPotency); 
            }
        }

        private void CollectCountryOfOrigin_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CountryOfOrigin = string.Empty;
            if (_ItemBranchTable.IBCOORE != "1")
            {
                CollectCountryOfOrigin.CancelLoad = true;
                if (IsItemSerlialized())
                {
                    Log.Debug("Brad6");
                    SendToERP();
                    if (!Transactions.Error)
                    {
                        if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                        {
                            UserSession.ScreenHandler.DefaultText = "1";
                        }
                        Next(_PrintBarCodeLabels);
                    }
                    else
                    {
                        RestartLoop(_CollectBranch);
                    }
                    return;
                }

                Next(_CollectLocation);
            }
            else if (DefaultCountryOfOrigin)
            {
                ScreenHandler.DefaultText = _ItemBranchTable.IBORIG.Trim();
            }
        }

        private bool CollectCountryOfOrigin_ValidateMethod(string countryOfOrigin)
        {
            if (!_UserDefinedCodesTable.ValidateUDC("00", "CN", countryOfOrigin))
            {
                throw new RFS_Exception("CountryOfOrigin");
            }
            _CountryOfOrigin = _UserDefinedCodesTable.DRKY;
            if (IsItemSerlialized())
            {
                Log.Debug("Brad7");
                SendToERP();

                if (!Transactions.Error)
                {
                    if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                    {
                        UserSession.ScreenHandler.DefaultText = "1";
                    }
                    Next(_PrintBarCodeLabels);
                }
                else
                {
                    RestartLoop(_CollectBranch);
                }

                return true;
            }

            Next(_CollectLocation);
            ScreenMap.Add("COUNTRYOFORIGIN", _CountryOfOrigin);
            return true;
        }

        private void CollectCountryOfOrigin_List_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            CollectCountryOfOrigin_List.Table = _UserDefinedCodesTable;
            _UserDefinedCodesTable.FetchUDCs("00", "CN");
        }
        #endregion
        #endregion

        #region CollectLocation
        private void _CollectLocation_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            //DefaultPrimary();
            if (!InReceiptRouting())
            {
                ScreenHandler.DefaultText = this.WorkflowOptions["DefaultReceivingLocation"].ToString().ToUpper();
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Inspection", "", false);
            }
            else
            {
                //AGCH Begin Change ispectgion logint to Default location
                if (this.WorkflowOptions["Followinspectionlogic"].ToString().ToUpper() == "Y")
                {
                    ScreenHandler.DefaultText = this.WorkflowOptions["InspectionLocation"].ToString().ToUpper();
                }
                else
                {
                    //ScreenHandler.DefaultText = "INSPECTION";
                    ScreenHandler.DefaultText =  this.WorkflowOptions["DefaultReceivingLocation"].ToString().ToUpper();
                }
                //AGCH END

                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Inspection", "INSPECTION", false);
               

            }
        }
        private void CollectLocation_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ReceivingLocationEnterKey(e);
        }

        private void ReceivingLocationEnterKey(ScreenEventArgs e)
        {
            // validate the receiving location
            if ((!WFOptions.GetYesNoWFOptionValue("AllowBlankLocation")) && (e.UserInput.ToString().Trim().Length == 0))
            {
                Error("BLANKSNOTALLOWED");
            }
            else
            {
                _LocationMasterTable.ResetAll();
                _LocationMasterTable.MCU = _DetailBranchPlant;
                _LocationMasterTable.inLOCN = e.UserInput.ToString().ToUpper();
                if (StockItem())
                {
                    if (_LocationMasterTable.ValidateLocation())
                    {
                        _Location = _LocationMasterTable.stbllocn;
                        _ScreenFormattedLocation = _LocationMasterTable.sscnlocn;
                        _ItemLocationTable.ResetAll();
                        _ItemLocationTable.LIMCU = _DetailBranchPlant;
                        _ItemLocationTable.LIITM = _ShortItemNumber;
                        _ItemLocationTable.LILOCN = _Location;
                        _ItemLocationTable.Fetch();
                        if (_ItemLocationTable.Rows.Count > 0)
                        {
                            _OnHandQuantity = _ItemLocationTable.LIPQOH;
                        }
                        else
                        {
                            _OnHandQuantity = 0;
                        }
                        UserSession.ScreenHandler.ScreenMap.Add("LOCATION", _ScreenFormattedLocation);

                    }
                    else
                        Error("LOCATION", e.UserInput.ToString());

                }
                else
                {
                    _Location = _PurchaseOrderDetailTable.PDLOCN;
                    UserSession.ScreenHandler.ScreenMap.Add("LOCATION", _Location);

                }

            }
        }
        private void CollectLocation_KeyEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (e.KeyPressed)
            {
                case ActionKey.F2: // user hits F2 - display empty locations
                    if (e.KeyPressed == ActionKey.F2)
                    {
                        _LocationMasterTable.ShowEmptyLocations(_DetailBranchPlant);
                        Next(_EmptyLocationsList);
                    }
                    break;
                case ActionKey.Enter:
                    ReceivingLocationEnterKey(e);
                    break;
                default:
                    Error("JDEWrongKey");
                    break;
            }
        }

        private void CollectLocation_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LocationLookupType = "LocationsForItem";
            _ItemLocationTable.RetrieveItemLocationRecords(_DetailBranchPlant, _CollectItemNumber.ItemsTable.IMITM, WFOptions.RetrieveOptionValue("ShowZeroOnHand"));

        }

        private void LocationMasterList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LocationMasterTable.UpdateCurrentRow();
            _LocationMasterTable.MCU = _DetailBranchPlant;
            _LocationMasterTable.inLOCN = _LocationMasterTable.LMLOCN;
            _LocationMasterTable.ResetAll();
            _LocationMasterTable.ValidateLocation();
            _Location = _LocationMasterTable.stbllocn;
            _ScreenFormattedLocation = _LocationMasterTable.sscnlocn;
            UserSession.ScreenHandler.ScreenMap.Add("LOCATION", _ScreenFormattedLocation);
            Next(_CollectQuantity);
        }


        #endregion

        #region CollectQuantity

        #region CollectQuantity_LeavingEvent

        private void CollectQuantity_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _TransactionQuantity = _CollectQuantity.Quantity;
            _TotalReceiptQuantity = _TransactionQuantity;
            Log.Debug($"(***) Transaction QTY {_TransactionQuantity.ToString()}");
            UserSession.ScreenHandler.ScreenMap.Add("QUANTITY", _TransactionQuantity.ToString());
            UserSession.ScreenHandler.DefaultText = _PurchaseOrderDetailTable.PDUOM;
            _DefaultUnitOfMeasure = _PurchaseOrderDetailTable.PDUOM;
            _BarcodeUnitOfMeasure = _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("BarcodeQuantityUOM"));
            _OriginalQuantityEntered = _TransactionQuantity; // need for barcode labels when dealing with license plates
        }

        #endregion

        #endregion

        #region CollectUnitOfMeasure

        private bool ValidateUnitOfMeasure(string sUOM)
        {
            double ConversionFactor = 1;
            #region Stock Items
            if (StockItem())
            {
                if (!_CollectUnitOfMeasure.Validate(sUOM))
                {
                    return false;
                }
                ConversionFactor = _UnitOfMeasureConversionTable1.ConvertUnitOfMeasure(_BranchPlant, _ShortItemNumber, _CollectUnitOfMeasure.UnitOfMeasure, _CollectItemNumber.ItemsTable.IMUOM1);
                if (ConversionFactor != 0)
                {
                    if ((!WFOptions.GetYesNoWFOptionValue("AllowUOMOverride"))
                       && (_CollectUnitOfMeasure.UnitOfMeasure != _DefaultUnitOfMeasure))
                    {
                        _CollectUnitOfMeasure.ErrorMessage = "UOMOverride";
                        return false;
                    }
                    UserSession.ScreenHandler.ScreenMap.Add("QUANTITY", _TransactionQuantity.ToString());
                    UserSession.ScreenHandler.ScreenMap.Add("UNITOFMEASURE", _CollectUnitOfMeasure.UnitOfMeasure);
                    _UnitOfMeasureAsEntered = _CollectUnitOfMeasure.UnitOfMeasure;
                    _SerialNumberQuantity = _TransactionQuantity * ConversionFactor;
                    ConvertOpenQuantityToPrimary();
                }
                else
                {
                    _CollectUnitOfMeasure.ErrorMessage = "UNITOFMEASURE";
                    return false;
                }
            }
            #endregion
            #region Non-Stock Items

            else
            {
                // Validate that UOM entered does exist in F0005 if item is non-stock
                if (_UserDefinedCodesTable.ValidateUDC("00", "UM", sUOM))
                    _CollectUnitOfMeasure.ScreenInput = sUOM;
                else
                {
                    _CollectUnitOfMeasure.ErrorMessage = "UNITOFMEASURE";
                    return false;
                }
                _OpenQuantityInPrimaryUnitOfMeasure = _PurchaseOrderDetailTable.PDUOPN;
            }
            #endregion
            #region Over Receiving Check
            if (OverReceiving(_TransactionQuantity * ConversionFactor))
            {
                if (!WFOptions.GetYesNoWFOptionValue("AllowOverReceipt"))
                {
                    _CollectUnitOfMeasure.ErrorMessage = "OVERRECEIPT";
                    return false;
                }
                else
                {
                    if (WFOptions.GetYesNoWFOptionValue("DisplayOverWarning"))
                    {
                        Next(_OverReceiptWarning);
                        _CollectUnitOfMeasure.ScreenInput = _CollectUnitOfMeasure.UnitOfMeasure;
                        return true;
                    }
                }
            }
            #endregion
            #region Under Receiving Check
            if ((UnderReceiving(_TransactionQuantity * ConversionFactor)) && (WFOptions.GetYesNoWFOptionValue("DisplayShortWarning")))
            {
                Next(_UnderReceiptWarning);
                _CollectUnitOfMeasure.ScreenInput = _CollectUnitOfMeasure.UnitOfMeasure;
                return true;
            }
            #endregion
            #region License Plating Check
            // Check license plating	
            if ((_LPLicense) &&
                (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive())) //CC025677
            {
                Next(_CollectLicensePlate);
                _CollectUnitOfMeasure.ScreenInput = _CollectUnitOfMeasure.UnitOfMeasure;
                return true;
            }

            #endregion

            _CollectUnitOfMeasure.ScreenInput = _CollectUnitOfMeasure.UnitOfMeasure;
            return true;
        }

        private void UnitOfMeasureList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // user has selected a record from the list.
            _UnitOfMeasureConversionTable1.UpdateCurrentRow();

            // Are we coming from the Collect UOM screen?!
            _CollectUnitOfMeasure.Validate(_UnitOfMeasureConversionTable1.UMUM);
            if (ValidateUnitOfMeasure(_CollectUnitOfMeasure.UnitOfMeasure))
            {
                _CollectUnitOfMeasure.ScreenInput = _CollectUnitOfMeasure.UnitOfMeasure;
            }
            else
            {
                Error(_CollectUnitOfMeasure.ErrorMessage, true);
            }
        }

        #endregion

        #region ConvertOpenQuantityToPrimary
        private void ConvertOpenQuantityToPrimary()
        {

            // attempt to retrieve the conversion from the DataManipulator table
            // convert the open quantity to primary
            _OpenQuantity = _PurchaseOrderDetailTable.PDUOPN;
            _OpenQuantityInPrimaryUnitOfMeasure = 0;
            var ConversionFactor = _UnitOfMeasureConversionTable1.ConvertUnitOfMeasure(_BranchPlant,
                _ShortItemNumber,
                _PurchaseOrderDetailTable.PDUOM,
                _CollectItemNumber.ItemsTable.IMUOM1);
            if (ConversionFactor == 0)
            {
                ConversionFactor = _UnitOfMeasureConversionTable1.ConvertToPrimaryUOM(_BranchPlant,
                        _ShortItemNumber,
                        _PurchaseOrderDetailTable.PDUOM);
            }
            Log.WriteLine(5, "Transaction Quantity before applying conversion factor = " + _PurchaseOrderDetailTable.PDUOPN.ToString());
            _OpenQuantityInPrimaryUnitOfMeasure = _PurchaseOrderDetailTable.PDUOPN * ConversionFactor;
            Log.WriteLine(5, "Transaction Quantity before applying conversion factor = " + _OpenQuantityInPrimaryUnitOfMeasure.ToString());

        }

        #endregion

        #region UnderReceiptWarning

        private void UnderReceiptWarning_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToBool())
            {
                if ((_LPLicense) &&
                    (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive())) //CC025677
                {
                    Next(_CollectLicensePlate);
                    return;
                }

                //Did we come from the Dual UOM screen?  If so, we've reached the end.
                if ((ICS.Display.Screen)ScreenHandler.ScreenStack.ToArray().GetValue(1) == _CollectDualUnitOfMeasure.Screen)
                {
                    ProcessThisReceipt();
                }
                else
                {
                    Next(_CollectDualQuantity);
                }
            }
            else
            {
                //Did we come from the Dual UOM screen?  If so, we need to go back to the
                // dual qty screen
                if ((ICS.Display.Screen)ScreenHandler.ScreenStack.ToArray().GetValue(1) == _CollectDualUnitOfMeasure.Screen)
                {
                    Back(_CollectDualQuantity);
                }
                else
                {
                    Back(_CollectQuantity);
                }
            }
        }

        #endregion

        #region OverReceiptWarning

        private void OverReceiptWarning_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToBool())
            {
                if ((_LPLicense) &&
                     (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive())) //CC025677
                {
                    Next(_CollectLicensePlate);
                    return;
                }
                //Did we come from the Dual UOM screen?  If so, we've reached the end.
                if ((ICS.Display.Screen)ScreenHandler.ScreenStack.ToArray().GetValue(1) == _CollectDualUnitOfMeasure.Screen)
                {
                    ProcessThisReceipt();
                }
                else
                {
                    Next(_CollectDualQuantity);
                }
            }
            else
            {
                //Did we come from the Dual UOM screen?  If so, we need to go back to the
                // dual qty screen
                if ((ICS.Display.Screen)ScreenHandler.ScreenStack.ToArray().GetValue(1) == _CollectDualUnitOfMeasure.Screen)
                {
                    Back(_CollectDualQuantity);
                }
                else
                {
                    Back(_CollectQuantity);
                }
            }
        }

        #endregion

        #region ProcessThisReceipt
        private void ProcessThisReceipt()
        {
            // serialized items
            if (StockItem())
            {
                if ((_LotProcessType.Trim().Length > 0) &&
                   (Convert.ToInt16(_LotProcessType) >= Convert.ToInt16(LotSerialDesignations.SerialOptional)))
                {

                    // for system assigned date and system assigned next number (5 & 6)
                    switch (Convert.ToInt16(_LotProcessType))
                    {
                        case 5:      // system assigned
                        case 6:      // system assigned
                            _SerialNumbersScanned = 0;
                            _FromSerialNumberEntry = true;
                            if (!WFOptions.GetYesNoWFOptionValue("AllowLotOverride"))
                            {
                                _SerialNumbersScanned += 1;
                                // convert quantity to primary for JDE update
                                _CollectUnitOfMeasure.Validate(_CollectItemNumber.ItemsTable.IMUOM1);
                                _TransactionQuantity = 1;
                                // call web service to update PeopleSoft
                                _SendFlag = "Now";

                                if (JDEReservationTable.DoJDERecordLocksExist("F4311", _PurchaseOrderHeaderTable.PHDOCO.ToString(), _PurchaseOrderHeaderTable.PHDCTO, _PurchaseOrderHeaderTable.PHKCOO))
                                {
                                    _PurchaseOrderDetailTable.UnLockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment);
                                    Error("RECORDRESERVED");
                                    RestartLoop(_CollectOrderNumber);
                                }
                                else
                                {
                                    Log.Debug("Brad1");
                                    SendToERP();

                                    if (!Transactions.Error)
                                    {
                                    if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                                    {
                                        UserSession.ScreenHandler.ScreenMap.Add("BarcodeUnitOfMeasure", _CollectItemNumber.ItemsTable.IMUOM1);
                                        UserSession.ScreenHandler.DefaultText = "1";
                                    }
                                    Next(_PrintBarCodeLabels);
                                    }
                                    else
                                    {
                                        RestartLoop(_CollectBranch);
                                    }
                                }
                            }
                            else
                            {
                                UserSession.ScreenHandler.ScreenMap.Add("SERIALNUMBERSCAN", "0");
                                UserSession.ScreenHandler.ScreenMap.Add("QUANTITY", _SerialNumberQuantity.ToString());
                                Next(_CollectSerialNumber);
                            }
                            break;
                        // for s/n optional or s/n required (4 & 7)
                        case 4:   // s/n optional
                        case 7:   // capture each S/N
                            UserSession.ScreenHandler.ScreenMap.Add("SERIALNUMBERSCAN", "0");
                            UserSession.ScreenHandler.ScreenMap.Add("QUANTITY", _SerialNumberQuantity.ToString());
                            Next(_CollectSerialNumber);
                            break;

                    }
                    return;
                }
            }
            // otherwise, call web service to update PeopleSoft
            _SendFlag = "Now";

            if (JDEReservationTable.DoJDERecordLocksExist("F4311", _PurchaseOrderHeaderTable.PHDOCO.ToString(), _PurchaseOrderHeaderTable.PHDCTO, _PurchaseOrderHeaderTable.PHKCOO))
            {
                _PurchaseOrderDetailTable.UnLockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment);
                Error("RECORDRESERVED");
                RestartLoop(_CollectOrderNumber);
            }
            else
            {
                if (!_FromGenLP || _LicensePlate.Trim().Length > 0)
                {
                    Log.Debug("Brad2");
                    SendToERP();
                    if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                        DefaultBarcodeQuantity();
                    _FromSerialNumberEntry = false;
                    if (!Transactions.Error)
                    {
                        Next(_PrintBarCodeLabels);
                    }
                    else
                    {
                        RestartLoop(_CollectBranch);
                    }

                }
                else
                    SendLicPlateToERP();
            }
        }
        #endregion

        #region PrintBarCodeLabels

        /// <summary>
        /// PrintBarCodeLabels: Some Comments
        /// </summary>

        #region PrintBarCodeLabels_EnterEvent

        /// <summary>
        /// Some Comments
        /// </summary>
        private void PrintBarCodeLabels_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            decimal labelQty = 0;

            try
            {
                _MaxLabelQty = WFOptions.GetInt32WFOptionValue("MaxLabels");
            }
            catch
            {
                _MaxLabelQty = -1;
            }
            _Copies = e.UserInput.ToString().Trim();

            //bypass printing if user enteres 0 or blank on collect label qty screen
            if (_Copies == "0")
            {
                RestartLoop(_CollectBranch);
                //ReturnFromTransaction();
                return;
            }

            try
            {
                labelQty = Convert.ToDecimal(_Copies);
                if (labelQty < 0) throw new ArgumentException("Negative label qty entered");

                if (_MaxLabelQty < 0 || labelQty <= _MaxLabelQty)
                {
                    PrintSystem.Reset();
                    PrintSystem.Branch = _BranchPlant.Trim();
                    PrintSystem.ItemNumber = _PrimaryItemNumber;
                    PrintSystem.Location = _LocationMasterTable.stbllocn;

                    if (PrintSystem.DefaultPrinter.Trim().Length == 0)
                    {
                        Error("LabelFormat");

                    }
                    else if (PrintSystem.ValidPrinter(PrintSystem.DefaultPrinter))
                        UserSession.ScreenHandler.DefaultText = PrintSystem.DefaultPrinter;
                }
                else
                {
                    Error("MAXEXCEEDED", e.UserInput.ToString());
                }
            }
            catch
            {
                Error("QUANTITY", e.UserInput.ToString());
            }
        }
        private void PrintBarCodeLabels_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ReturnFromTransaction();
        }

        #endregion

        #endregion

        #region CollectPrinter

        /// <summary>
        /// CollectPrinter: Some Comments
        /// </summary>

        #region CollectPrinter_EnterEvent

        /// <summary>
        /// Some Comments
        /// </summary>
        private void CollectPrinter_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PrinterName = e.UserInput.ToString();
            if (PrintSystem.ValidPrinter(_PrinterName))
            {
                _PrinterAddress = PrintSystem.PrinterAddress;

                if (!_FromGenLP || _GeneratedLicensePlateNumbersTable.Rows.Count == 0)
                    PrintBarCode();
                else
                {
                    for (int i = 0; i < _GeneratedLicensePlateNumbersTable.Rows.Count; i++)
                    {
                        _LicensePlate = _GeneratedLicensePlateNumbersTable.Rows[i]["Number"].ToString().Trim();
                        PrintBarCode();
                        PrintSystem.Reset();
                        PrintSystem.Branch = _BranchPlant;
                        PrintSystem.ItemNumber = _PrimaryItemNumber;
                        PrintSystem.Location = _LocationMasterTable.stbllocn;
                    }
                }
                //ReturnFromTransaction();
                _FromGenLP = false;
                ClearTransaction();
                _BusinessDataLocked = false;
                RestartLoop(_CollectBranch);
                return;
            }
            else
                Error(PrintSystem.ErrorMessageVocabulary,
                   e.UserInput.ToString());

        }

        #endregion

        #region CollectPrinter_ListEvent


        private void FRFSPrinters_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DefinedPrintersTable.UpdateCurrentRow();
            _PrinterName = _DefinedPrintersTable.PrinterName;
            if (PrintSystem.ValidPrinter(_PrinterName))
            {
                _PrinterAddress = PrintSystem.PrinterAddress;
                if (!_FromGenLP || _GeneratedLicensePlateNumbersTable.Rows.Count == 0)
                    PrintBarCode();
                else
                {
                    for (int i = 0; i < _GeneratedLicensePlateNumbersTable.Rows.Count; i++)
                    {
                        _LicensePlate = _GeneratedLicensePlateNumbersTable.Rows[i]["Number"].ToString().Trim();
                        PrintBarCode();
                        PrintSystem.Reset();
                        PrintSystem.Branch = _BranchPlant;
                        PrintSystem.ItemNumber = _PrimaryItemNumber;
                        PrintSystem.Location = _LocationMasterTable.stbllocn;
                    }
                }
                RestartLoop(_CollectBranch);
                //ReturnFromTransaction();
            }
            else
                Error(PrintSystem.ErrorMessageVocabulary);
        }

        private void FRFSPrinters_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(_CollectPrinter);
        }
        #endregion

        #endregion

        #region ItemLocationList_EnterEvent

        private void ItemLocationList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemLocationTable.UpdateCurrentRow();
            switch (_LocationLookupType)
            {
                case "Lot":
                    _LotNumber = _ItemLocationTable.LILOTN;
                    LotNumberEnter();
                    break;
                case "LocationsForItem":
                    _Location = _ItemLocationTable.LILOCN;
                    _LocationMasterTable.ValidateLocation(this._BranchPlant, _ItemLocationTable.LILOCN);

                    UserSession.ScreenHandler.ScreenMap.Add("LOCATION", _LocationMasterTable.sscnlocn);
                    Next(_CollectQuantity);
                    break;
                case "SerialNumbersForItem":
                    _LotNumber = _ItemLocationTable.LILOTN;
                    SerialNumberEnter();
                    break;
            }
        }

        #endregion

        #region EmptyLocationList_EnterEvent

        private void EmptyLocationList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // user has selected a record from the list
            _LocationMasterTable.UpdateCurrentRow();
            _LocationMasterTable.MCU = _DetailBranchPlant;
            _LocationMasterTable.inLOCN = _LocationMasterTable.LMLOCN;
            _LocationMasterTable.ResetAll();
            _LocationMasterTable.ValidateLocation();
            _Location = _LocationMasterTable.stbllocn;
            _ScreenFormattedLocation = _LocationMasterTable.stbllocn;
            UserSession.ScreenHandler.ScreenMap.Add("LOCATION", _ScreenFormattedLocation);
            Next(_CollectQuantity);
        }



        #endregion

        #region OverReceiving

        private bool OverReceiving(double ReceivedQuantity)
        {
            // check for overreceiving
            Log.WriteLine(5, "Received Quantity = " + ReceivedQuantity.ToString());
            Log.WriteLine(5, "Open Qty in Primary U/M = " + _OpenQuantityInPrimaryUnitOfMeasure.ToString());
            if (ReceivedQuantity > _OpenQuantityInPrimaryUnitOfMeasure)
                return true;
            else
                return false;
        }

        #endregion

        #region UnderReceiving

        private bool UnderReceiving(double ReceivedQuantity)
        {
            // check for overreceiving
            if (ReceivedQuantity < _OpenQuantityInPrimaryUnitOfMeasure)
                return true;
            else
                return false;
        }

        #endregion

        #region SendLicPlateToERP

        /// <summary>
        /// for each license plate to generate, send qty of "1" in the L/P U/M to the ERP,
        /// and save the returned license plate number in a record set.
        /// when finished, prompt the user for barcodes, and if they choose to print,
        /// the code will loop thru the record set in CollectPrinter method
        /// </summary>
        private void SendLicPlateToERP()
        {
            double _TotalQuantity = _TransactionQuantity;

            // Generate License Plate for each receipt U/M
            _GeneratedLicensePlateNumbersDataSet.Clear();
            for (int i = 0; i < _TotalQuantity; i++)
            {
                if (WFOptions.GetYesNoWFOptionValue("RFSGenLicensePlateNumber"))
                {
                    int NextNumber = 0;
                    string FormattedNextNumber = "";

                    if (_NextNumberTable.GetNextNumber("LicensePlate", out NextNumber, out FormattedNextNumber))
                    {
                        _LicensePlate = FormattedNextNumber;
                    } // end if
                    else
                    {                       // Reset the License Plate to prevent duplicates
                        _LicensePlate = "";  // in the event the License Plate is not correctly
                    } // end else           // generated
                } // end if

                DataRow dr = _GeneratedLicensePlateNumbersDataSet.Tables["GeneratedLicensePlateNumbersTable"].NewRow();
                _TransactionQuantity = 1;
                Log.Debug("Brad3");
                SendToERP();
                dr["Number"] = _LicensePlate;
                _GeneratedLicensePlateNumbersTable.Rows.Add(dr);
            } // end for

            if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
            {
                DefaultBarcodeQuantity();
            } // end if

            if (!Transactions.Error)
            {
                Next(_PrintBarCodeLabels);
            }
            else
            {
                RestartLoop(_CollectBranch);
            }
        } // end SendLicPlateToERP

        #endregion

        #region SendToERP

        private void setSoldTo()
        {
            _ReceivingTransaction.VendSoldTo = _PurchaseOrderDetailTable.PDAN8.ToString();
            _AddressBookTable.RetrieveAddress(_PurchaseOrderDetailTable.PDAN8.ToString());
            _ReceivingTransaction.VendSoldToDescr = _AddressBookTable.ABALPH.Trim();
        }
        private void setShipTo()
        {
            _ReceivingTransaction.VendShipTo = _PurchaseOrderDetailTable.PDSHAN.ToString();
            _AddressBookTable.RetrieveAddress(_PurchaseOrderDetailTable.PDSHAN.ToString());
            _ReceivingTransaction.VendShipToDescr = _AddressBookTable.ABALPH.Trim();
        }
        private void setToLocation()
        {
            _ReceivingTransaction.Location = _Location;
            if (_LocationMasterTable.ValidateLocation(_DetailBranchPlant.Trim(), _Location))
            {
                _ReceivingTransaction.ToLocnPutZone = _LocationMasterTable.LMPZON.Trim();
                _ReceivingTransaction.ToLocnPickZone = _LocationMasterTable.LMKZON.Trim();
                _ReceivingTransaction.ToLocnReplZone = _LocationMasterTable.LMZONR.Trim();
                _ReceivingTransaction.ToLocnDimGroup = _LocationMasterTable.LMSTY1.Trim();
            }
        }
        private void setItem()
        {
            _ReceivingTransaction.PrimaryItemNumber = _PrimaryItemNumber;
            _ReceivingTransaction.ShortItemNumber = _PurchaseOrderDetailTable.PDITM.ToString();
            _ReceivingTransaction.SecondItemNumber = _LongItemNumber;
            _ReceivingTransaction.ThirdItemNumber = _ThirdItemNumber;
            _ReceivingTransaction.ItemDescription1 = _PurchaseOrderDetailTable.PDDSC1; // Needed for WOW
            _ReceivingTransaction.ItemDescription2 = _PurchaseOrderDetailTable.PDDSC2; // Needed for WOW
            // CS7340 - When receiving non-stock items, there is not a F4102 record.
			// We must check that the query succeeds before setting the transaction properties.
			if (_ItemBranchTable.ValidateItemBranch(_DetailBranchPlant.Trim(), _PurchaseOrderDetailTable.PDITM))
			{
				_ReceivingTransaction.ItemDimGroup = _ItemBranchTable.IBPRP6.Trim();
				_ReceivingTransaction.ItemLotProcType = _ItemBranchTable.IBSRCE;
			}            
        }
        private void SendToERP()
        {
            int count = 1;
            RoutingMessageDisplayed = false;
            Transactions.Add(_ReceivingTransaction);
            
            Transactions.Branch = _DetailBranchPlant.Trim().ToUpperInvariant();
            _ReceivingTransaction.Initialize();
            #region Edit Line
            setItem();
            setToLocation();
            setSoldTo();
            setShipTo();
            _ReceivingTransaction.BranchPlant = _DetailBranchPlant;
            _ReceivingTransaction.OrderCompany = _PurchaseOrderDetailTable.PDKCOO;
            _ReceivingTransaction.OrderType = _PurchaseOrderDetailTable.PDDCTO;
            _ReceivingTransaction.OrderNumber = _PurchaseOrderDetailTable.PDDOCO.ToString();
            _ReceivingTransaction.LineNumber = _PurchaseOrderDetailTable.PDLNID.ToString(); //Pass manipulated line number value instead of RAW
            _ReceivingTransaction.ContainerID = _PackingSlip;
            _ReceivingTransaction.RFSUserID = UserSession.CurrentUser.LoginName;
            _ReceivingTransaction.SupplierRemark = _Remarks;

            //using the ContainerID field to store the packing slip number
            //if (_Container.Trim().Equals(string.Empty) && WFOptions.GetYesNoWFOptionValue("DefaultContainerID"))
            //    _ReceivingTransaction.ContainerID = _PurchaseOrderDetailTable.PDCNID;
            //else
            //    _ReceivingTransaction.ContainerID = _Container;
            //_ReceivingTransaction.LicensePlateNumber = _LicensePlate;

            _ReceivingTransaction.NewOrExistingLPN = _LicensePlateProcessingType;
            _ReceivingTransaction.UOM = _CollectUnitOfMeasure.UnitOfMeasure;
            _BarcodeUOMForPrinting = _CollectUnitOfMeasure.UnitOfMeasure;
            if (_CollectUnitOfMeasure.UnitOfMeasure != _PurchaseOrderDetailTable.PDUOM)
            {
                double WorkQuantity = _UnitOfMeasureConversionTable1.ConvertToDocumentUOM(_DetailBranchPlant,
                   _ShortItemNumber, _CollectUnitOfMeasure.UnitOfMeasure, _PurchaseOrderDetailTable.PDUOM, _TransactionQuantity, _CollectItemNumber.ItemsTable.IMUOM1);
                // if the return value was zero, then a conversion was not found, don't change the quantity
                if (WorkQuantity != 0)
                {
                    _TransactionQuantity = WorkQuantity;
                    _ReceivingTransaction.UOM = _PurchaseOrderDetailTable.PDUOM;
                    _BarcodeUOMForPrinting = _PurchaseOrderDetailTable.PDUOM;
                }
                else
                {
                    double ConversionFactor = _UnitOfMeasureConversionTable1.ConvertToPrimaryUOM(_DetailBranchPlant,
                        _ShortItemNumber,
                        _CollectUnitOfMeasure.UnitOfMeasure);
                    if (ConversionFactor != 0)
                    {
                        _TransactionQuantity = _TransactionQuantity * ConversionFactor;
                        _ReceivingTransaction.UOM = _PurchaseOrderDetailTable.PDUOM;
                        _BarcodeUOMForPrinting = _PurchaseOrderDetailTable.PDUOM;
                    }
                    else
                    {
                        ConversionFactor = _UnitOfMeasureConversionTable2.ConvertToPrimaryUOM(_CollectUnitOfMeasure.UnitOfMeasure, _PurchaseOrderDetailTable.PDUOM);
                        if (ConversionFactor != 0)
                        {
                            _TransactionQuantity = _TransactionQuantity * ConversionFactor;
                            _ReceivingTransaction.UOM = _PurchaseOrderDetailTable.PDUOM;
                            _BarcodeUOMForPrinting = _PurchaseOrderDetailTable.PDUOM;
                        }
                    }
                }
            }

            _ReceivingTransaction.QtyToReceive = _TransactionQuantity.ToString();
            #region Dual Unit Of Measure

            _ReceivingTransaction.SecondaryQty = _DualQuantity.ToString();
            _ReceivingTransaction.SecondaryUOM = _DualUnitOfMeasure;

            if (_DualUnitOfMeasure != _PurchaseOrderDetailTable.PDUOM)
            {
                double DualWorkQuantity = _UnitOfMeasureConversionTable1.ConvertToDocumentUOM(_DetailBranchPlant,
                   _ShortItemNumber, _DualUnitOfMeasure, _PurchaseOrderDetailTable.PDUOM, _DualQuantity, _CollectItemNumber.ItemsTable.IMUOM1);
                // if the return value was zero, then a conversion was not found, don't change the quantity
                if (DualWorkQuantity != 0)
                {
                    _ReceivingTransaction.SecondaryQty = DualWorkQuantity.ToString();
                    _ReceivingTransaction.SecondaryUOM = _PurchaseOrderDetailTable.PDUOM;
                }
                else
                {
                    double ConversionFactor = _UnitOfMeasureConversionTable2.ConvertToPrimaryUOM(_DualUnitOfMeasure, _PurchaseOrderDetailTable.PDUOM);
                    if (ConversionFactor != 0)
                    {
                        _ReceivingTransaction.SecondaryQty = Convert.ToString(_DualQuantity * ConversionFactor);
                        _ReceivingTransaction.SecondaryUOM = _PurchaseOrderDetailTable.PDUOM;
                    }
                }
            }

            #endregion
            _ReceivingTransaction.RcptRoutingInitiated = "";
            _ReceivingTransaction.RFSUserID = UserSession.CurrentUser.LoginName;
            _ReceivingTransaction.Lot = _LotNumber;
            _ReceivingTransaction.LotStatus = WFOptions.RetrieveOptionValue("LotStatusValue");
            _ReceivingTransaction.LotProcessType = _LotProcessType.Trim() == "" ? " " : _LotProcessType; // Needed for WOW
            _ReceivingTransaction.MemoLotField1 = _MemoLot1;
            _ReceivingTransaction.MemoLotField2 = _MemoLot2;
            _ReceivingTransaction.VendorLotNumber = _VendorLot;
            _ReceivingTransaction.LotExpDate = DateTime.Now.AddYears(10).ToString("yyyyMMdd");
            //_ReceivingTransaction.LotExpDate = _ExpirationDate;
            _ReceivingTransaction.TransDate_YYYYMMDD = System.DateTime.Now.ToString("yyyyMMdd");
            _ReceivingTransaction.Version = WFOptions.RetrieveOptionValue("ERPUpdateVersion");
            _ReceivingTransaction.Mode = "1";
            _ReceivingTransaction.EditLineCount = count.ToString();
            _ReceivingTransaction.CountryOfOrigin = _CountryOfOrigin.Trim();
            _ReceivingTransaction.LotGrade = _LotGrade.Trim();
            _ReceivingTransaction.LotPotency = _LotPotency.Trim();
            #endregion

            #region End Doc
            _ReceivingTransaction.AddToBoundaryWithData();
            _ReceivingTransaction.Mode = "2";
            #endregion

            #region Clear Cache
            _ReceivingTransaction.AddToBoundaryWithData();
            _ReceivingTransaction.Mode = "3";
            #endregion

            if (_SendFlag == "Now")
            {
                Transactions.Send(WFOptions.RetrieveOptionValue("SyncTransaction"));
                DisplayMessages();
                _LotNumber = _ReceivingTransaction.ReturnField("Lot", 0, 0);
                _ExpirationDate = _ReceivingTransaction.ReturnField("LotExpDate", 0, 0);
                if (_BusinessDataLocked)
                    _PurchaseOrderDetailTable.UnLockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment);
            }
        }
        private void SendToERP2(string NextSerialNumber, double QTY)
        {
            int count = 1;
            RoutingMessageDisplayed = false;
            Transactions.Add(_ReceivingTransaction);

            Transactions.Branch = _DetailBranchPlant.Trim().ToUpperInvariant();
            _ReceivingTransaction.Initialize();
            #region Edit Line
            setItem();
            setToLocation();
            setSoldTo();
            setShipTo();
            _ReceivingTransaction.BranchPlant = _DetailBranchPlant;
            _ReceivingTransaction.OrderCompany = _PurchaseOrderDetailTable.PDKCOO;
            _ReceivingTransaction.OrderType = _PurchaseOrderDetailTable.PDDCTO;
            _ReceivingTransaction.OrderNumber = _PurchaseOrderDetailTable.PDDOCO.ToString();
            _ReceivingTransaction.LineNumber = _PurchaseOrderDetailTable.PDLNID.ToString(); //Pass manipulated line number value instead of RAW
            _ReceivingTransaction.ContainerID = _PackingSlip;

            _ReceivingTransaction.SupplierRemark = _Remarks;

            //using the ContainerID field to store the packing slip number
            //if (_Container.Trim().Equals(string.Empty) && WFOptions.GetYesNoWFOptionValue("DefaultContainerID"))
            //    _ReceivingTransaction.ContainerID = _PurchaseOrderDetailTable.PDCNID;
            //else
            //    _ReceivingTransaction.ContainerID = _Container;
            //_ReceivingTransaction.LicensePlateNumber = _LicensePlate;

            _ReceivingTransaction.NewOrExistingLPN = _LicensePlateProcessingType;
            _ReceivingTransaction.UOM = _CollectUnitOfMeasure.UnitOfMeasure;
            _BarcodeUOMForPrinting = _CollectUnitOfMeasure.UnitOfMeasure;
            if (_CollectUnitOfMeasure.UnitOfMeasure != _PurchaseOrderDetailTable.PDUOM)
            {
                double WorkQuantity = _UnitOfMeasureConversionTable1.ConvertToDocumentUOM(_DetailBranchPlant,
                   _ShortItemNumber, _CollectUnitOfMeasure.UnitOfMeasure, _PurchaseOrderDetailTable.PDUOM, _TransactionQuantity, _CollectItemNumber.ItemsTable.IMUOM1);
                // if the return value was zero, then a conversion was not found, don't change the quantity
                if (WorkQuantity != 0)
                {
                    _TransactionQuantity = WorkQuantity;
                    _ReceivingTransaction.UOM = _PurchaseOrderDetailTable.PDUOM;
                    _BarcodeUOMForPrinting = _PurchaseOrderDetailTable.PDUOM;
                }
                else
                {
                    double ConversionFactor = _UnitOfMeasureConversionTable1.ConvertToPrimaryUOM(_DetailBranchPlant,
                        _ShortItemNumber,
                        _CollectUnitOfMeasure.UnitOfMeasure);
                    if (ConversionFactor != 0)
                    {
                        _TransactionQuantity = _TransactionQuantity * ConversionFactor;
                        _ReceivingTransaction.UOM = _PurchaseOrderDetailTable.PDUOM;
                        _BarcodeUOMForPrinting = _PurchaseOrderDetailTable.PDUOM;
                    }
                    else
                    {
                        ConversionFactor = _UnitOfMeasureConversionTable2.ConvertToPrimaryUOM(_CollectUnitOfMeasure.UnitOfMeasure, _PurchaseOrderDetailTable.PDUOM);
                        if (ConversionFactor != 0)
                        {
                            _TransactionQuantity = _TransactionQuantity * ConversionFactor;
                            _ReceivingTransaction.UOM = _PurchaseOrderDetailTable.PDUOM;
                            _BarcodeUOMForPrinting = _PurchaseOrderDetailTable.PDUOM;
                        }
                    }
                }
            }

            _ReceivingTransaction.QtyToReceive = QTY.ToString();

            #region Dual Unit Of Measure

            _ReceivingTransaction.SecondaryQty = _DualQuantity.ToString();
            _ReceivingTransaction.SecondaryUOM = _DualUnitOfMeasure;

            if (_DualUnitOfMeasure != _PurchaseOrderDetailTable.PDUOM)
            {
                double DualWorkQuantity = _UnitOfMeasureConversionTable1.ConvertToDocumentUOM(_DetailBranchPlant,
                   _ShortItemNumber, _DualUnitOfMeasure, _PurchaseOrderDetailTable.PDUOM, _DualQuantity, _CollectItemNumber.ItemsTable.IMUOM1);
                // if the return value was zero, then a conversion was not found, don't change the quantity
                if (DualWorkQuantity != 0)
                {
                    _ReceivingTransaction.SecondaryQty = DualWorkQuantity.ToString();
                    _ReceivingTransaction.SecondaryUOM = _PurchaseOrderDetailTable.PDUOM;
                }
                else
                {
                    double ConversionFactor = _UnitOfMeasureConversionTable2.ConvertToPrimaryUOM(_DualUnitOfMeasure, _PurchaseOrderDetailTable.PDUOM);
                    if (ConversionFactor != 0)
                    {
                        _ReceivingTransaction.SecondaryQty = Convert.ToString(_DualQuantity * ConversionFactor);
                        _ReceivingTransaction.SecondaryUOM = _PurchaseOrderDetailTable.PDUOM;
                    }
                }
            }

            #endregion


            _ReceivingTransaction.RcptRoutingInitiated = "";
            _ReceivingTransaction.RFSUserID = UserSession.CurrentUser.LoginName;
            _ReceivingTransaction.Lot = NextSerialNumber;
            _ReceivingTransaction.LotStatus = WFOptions.RetrieveOptionValue("LotStatusValue");
            _ReceivingTransaction.LotProcessType = _LotProcessType.Trim() == "" ? " " : _LotProcessType; // Needed for WOW
            _ReceivingTransaction.MemoLotField1 = _MemoLot1;
            _ReceivingTransaction.MemoLotField2 = _MemoLot2;
            _ReceivingTransaction.VendorLotNumber = _VendorLot;
            _ReceivingTransaction.LotExpDate = _ExpirationDate;
            _ReceivingTransaction.TransDate_YYYYMMDD = System.DateTime.Now.ToString("yyyyMMdd");
            _ReceivingTransaction.Version = WFOptions.RetrieveOptionValue("ERPUpdateVersion");
            _ReceivingTransaction.Mode = "1";
            _ReceivingTransaction.EditLineCount = count.ToString();
            _ReceivingTransaction.CountryOfOrigin = _CountryOfOrigin.Trim();
            _ReceivingTransaction.LotGrade = _LotGrade.Trim();
            _ReceivingTransaction.LotPotency = _LotPotency.Trim();
            #endregion

            #region End Doc
            _ReceivingTransaction.AddToBoundaryWithData();
            _ReceivingTransaction.Mode = "2";
            #endregion

            #region Clear Cache
            _ReceivingTransaction.AddToBoundaryWithData();
            _ReceivingTransaction.Mode = "3";
            #endregion

            if (_SendFlag == "Now")
            {
                Transactions.Send(WFOptions.RetrieveOptionValue("SyncTransaction"));
                DisplayMessages();
                _LotNumber = _ReceivingTransaction.ReturnField("Lot", 0, 0);
                _ExpirationDate = _ReceivingTransaction.ReturnField("LotExpDate", 0, 0);
                if (_BusinessDataLocked)
                    _PurchaseOrderDetailTable.UnLockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment);
            }

        }

        #endregion

        #region ClearTransaction

        // clear out transactional fields and return to item number screen
        private void ClearTransaction()
        {
            _LineNumber = 0;
            _LongItemNumber = "";
            _ShortItemNumber = 0;
            _LotNumber = "";
            _MemoLot1 = "";
            _MemoLot2 = "";
            _VendorLot = "";
            _Location = "";
            _ScreenFormattedLocation = "";
            _TransactionQuantity = 0;
            _LotProcessType = "";
            _ExpirationDate = "";
            _SerialNumbersScanned = 0;
            _LastDateEntered = "";
            _LicensePlate = "";
            _LicensePlateProcessingType = "";
            _OriginalQuantityEntered = 0;
            _LotGrade = string.Empty;
            _LotPotency = string.Empty;
            _CountryOfOrigin = string.Empty;
            _DualQuantity = 0;
            _DualUnitOfMeasure = "";

            UserSession.ScreenHandler.ScreenMap.Clear("ITEMNUMBER");
            UserSession.ScreenHandler.ScreenMap.Clear("LotNumber");
            UserSession.ScreenHandler.ScreenMap.Clear("SerialNumberScan");
            UserSession.ScreenHandler.ScreenMap.Clear("Quantity");
            UserSession.ScreenHandler.ScreenMap.Clear("UnitOfMeasure");
            UserSession.ScreenHandler.ScreenMap.Clear("LineNumber");
            UserSession.ScreenHandler.ScreenMap.Clear("Description1");

        }

        #endregion

        #region GetItemMaster

        private void GetItemMaster()
        {
            // retrieve item master
            _CollectItemNumber.ItemsTable.ResetAll();
            _CollectItemNumber.ItemsTable.IMITM = _PurchaseOrderDetailTable.PDITM;
            if (StockItem())
            {
                if (_CollectItemNumber.ItemsTable.Fetch())
                {
                    switch (_CollectBranch.PrimaryItemDesignator)
                    {
                        case PrimaryItemNumberDesignations.ShortNumber:
                            _PrimaryItemNumber = _CollectItemNumber.ItemsTable.IMITM.ToString();
                            UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _CollectItemNumber.ItemsTable.IMITM.ToString());
                            break;
                        case PrimaryItemNumberDesignations.LongNumber:
                            _PrimaryItemNumber = _CollectItemNumber.ItemsTable.IMLITM;
                            UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _CollectItemNumber.ItemsTable.IMLITM);
                            break;
                        case PrimaryItemNumberDesignations.ThirdNumber:
                            _PrimaryItemNumber = _CollectItemNumber.ItemsTable.IMAITM;
                            UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _CollectItemNumber.ItemsTable.IMAITM);
                            break;
                    }
                }
                else
                {
                    return;
                }
                _DetailBranchPlant = _DataManipulatorTable.FormatData("F0006", "MCMCU", _PurchaseOrderDetailTable.PDMCU);
                LoadBranchPlantData(); //CC025852
                UserSession.ScreenHandler.ScreenMap.Add("Branch", _DetailBranchPlant);
                UserSession.ScreenHandler.ScreenMap.Add("DetailBranchPlant", _DetailBranchPlant);
                _CollectBranch.WarehouseConstantsTable.DeterminePrimaryItem(_DetailBranchPlant);

                _LocationMasterTable.ResetAll();
                _LocationMasterTable.inLOCN = _ItemLocationTable.RetrievePrimaryLocation(_BranchPlant, _ShortItemNumber);
                _LocationMasterTable.MCU = _DetailBranchPlant;
                if (_LocationMasterTable.ValidateLocation())
                    UserSession.ScreenHandler.ScreenMap.Add("PrimaryLocation", _LocationMasterTable.sscnlocn);
                else
                    UserSession.ScreenHandler.ScreenMap.Add("PrimaryLocation", " ");

            }
            else
            {
                _ShortItemNumber = 0;
                _PrimaryItemNumber = _PurchaseOrderDetailTable.PDLITM;
                UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _PrimaryItemNumber);
                UserSession.ScreenHandler.ScreenMap.Add("PrimaryLocation", " ");
            }

        }


        #endregion

        #region SetUpPOSQL

        private void SetUpPOSQL(string sUserInput)
        {
            _PurchaseOrderDetailTable.ResetAll();
            // validate that this item exists on the P/O detail table
            // since we may be looking at multiple NXTR conditions,
            // the SQL WHERE clause must be constructed

            for (int i = 0; i < _AllowedNextStatusCodes.Length; i++)
            {
                // first time thru, leave the initial AND/OR blank
                if (i == 0)
                {
                    _PurchaseOrderDetailTable.Where("(", _PurchaseOrderDetailTable.Columns.PDDOCO, ICS.Database.Condition.EqualTo, Convert.ToDouble(_DocumentNumber), "");
                    _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDDCTO, ICS.Database.Condition.EqualTo, _DocumentType, "");

                    switch (_CollectBranch.WarehouseConstantsTable._DesignatedPrimaryItemNumber)
                    {
                        case PrimaryItemNumberDesignations.ShortNumber:
                            _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDITM, ICS.Database.Condition.EqualTo, Convert.ToInt32(sUserInput), "");
                            break;
                        case PrimaryItemNumberDesignations.LongNumber:
                            _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDLITM, ICS.Database.Condition.EqualTo, sUserInput.ToUpper(), "");
                            break;
                        case PrimaryItemNumberDesignations.ThirdNumber:
                            _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDAITM, ICS.Database.Condition.EqualTo, sUserInput.ToUpper(), "");
                            break;
                        default:
                            _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDAITM, ICS.Database.Condition.EqualTo, sUserInput.ToUpper(), "");
                            break;
                    }

                    //CC025852
                    // if the user has entered a branch on the initial screen, then use this to filter the detail
                    if (_BranchPlant.Trim().Length > 0)
                    {
                        _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDNXTR, ICS.Database.Condition.EqualTo, _AllowedNextStatusCodes[i], "");
                        _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDUOPN, ICS.Database.Condition.GreaterThan, 0, ""); // CC032309
                        _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDMCU, ICS.Database.Condition.EqualTo, _BranchPlant, ")");
                    }
                    else
                    {
                        _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDNXTR, ICS.Database.Condition.EqualTo, _AllowedNextStatusCodes[i], "");
                        _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDUOPN, ICS.Database.Condition.GreaterThan, 0, ")"); // CC032309
                    }
                }
                else
                {
                    // other cycles, add the "OR" clause
                    _PurchaseOrderDetailTable.Where("OR (", _PurchaseOrderDetailTable.Columns.PDDOCO, ICS.Database.Condition.EqualTo, Convert.ToDouble(_DocumentNumber), "");
                    _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDDCTO, ICS.Database.Condition.EqualTo, _DocumentType, "");

                    switch (_CollectBranch.WarehouseConstantsTable._DesignatedPrimaryItemNumber)
                    {
                        case PrimaryItemNumberDesignations.ShortNumber:
                            _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDITM, ICS.Database.Condition.EqualTo, Convert.ToInt32(sUserInput), "");
                            break;
                        case PrimaryItemNumberDesignations.LongNumber:
                            _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDLITM, ICS.Database.Condition.EqualTo, sUserInput.ToUpper(), "");
                            break;
                        case PrimaryItemNumberDesignations.ThirdNumber:
                            _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDAITM, ICS.Database.Condition.EqualTo, sUserInput.ToUpper(), "");
                            break;
                        default:
                            _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDAITM, ICS.Database.Condition.EqualTo, sUserInput.ToUpper(), "");
                            break;
                    }

                    //CC025852
                    // if the user has entered a branch on the initial screen, then use this to filter the detail
                    if (_BranchPlant.Trim().Length > 0)
                    {
                        _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDNXTR, ICS.Database.Condition.EqualTo, _AllowedNextStatusCodes[i], "");
                        _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDUOPN, ICS.Database.Condition.GreaterThan, 0, ""); // CC032309
                        _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDMCU, ICS.Database.Condition.EqualTo, _BranchPlant, ")");
                    }
                    else
                    {
                        _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDNXTR, ICS.Database.Condition.EqualTo, _AllowedNextStatusCodes[i], "");
                        _PurchaseOrderDetailTable.Where("AND", _PurchaseOrderDetailTable.Columns.PDUOPN, ICS.Database.Condition.GreaterThan, 0, ")"); // CC032309
                    }
                }
            }
        }
        #endregion

        #region InReceiptRouting

        private bool InReceiptRouting()
        {
            Log.Debug("(***) In Receipt Routing");
            bool ReturnValue = false;
            _ReceiptRouteTable.ResetAll();
            // check to see if item is in receipt routing
            _ReceiptRouteTable.PCAN8 = _PurchaseOrderDetailTable.PDAN8;
            _ReceiptRouteTable.PCMCU = _CollectBranch.BranchPlantsTable.MCMCU;
            _ReceiptRouteTable.PCITM = _ShortItemNumber;
            if (_ReceiptRouteTable.Fetch())
            {
                if (_ReceiptRouteTable.PCNROU.Trim().Length > 0)
                {
                    DateTime FromDate = ICS.Utilities.DateTimeUtils.JDEJulian(_ReceiptRouteTable.PCCEFJ);
                    DateTime ThruDate = ICS.Utilities.DateTimeUtils.JDEJulian(_ReceiptRouteTable.PCCXPJ);
                    if ((System.DateTime.Now >= FromDate) && (System.DateTime.Now <= ThruDate))
                        ReturnValue = true;
                }
            }
            // retrieve route description
            if (ReturnValue)
            {
                _UserDefinedCodesTable.ResetAll();
                //_UserDefinedCodesTable.DRSY = "43  ";
                //_UserDefinedCodesTable.DRRT = "RC";
                _UserDefinedCodesTable.Where("", _UserDefinedCodesTable.Columns.DRSY, ICS.Database.Condition.EqualTo, "43  ", "");
                _UserDefinedCodesTable.Where("AND", _UserDefinedCodesTable.Columns.DRRT, ICS.Database.Condition.EqualTo, "RC", "");
                _UserDefinedCodesTable.Where("AND", _UserDefinedCodesTable.Columns.DRKY, ICS.Database.Condition.Like, "%" + _ReceiptRouteTable.PCNROU.Trim() + "%", "");

                //_UserDefinedCodesTable.DRKY = _ReceiptRouteTable.PCNROU;
                if (!_UserDefinedCodesTable.Fetch())
                    _UserDefinedCodesTable.DRDL01 = "Not Found";
            }
            return ReturnValue;
        }

        #endregion

        #region DefaultPrimary

        private void DefaultPrimary()
        {

            UserSession.ScreenHandler.DefaultText = "";
            // retrieve the item's primary location record
            _ItemLocationTable.ResetAll();
            _ItemLocationTable.LIMCU = _CollectBranch.BranchPlantsTable.MCMCU;
            _ItemLocationTable.LIITM = _CollectItemNumber.ItemsTable.IMITM;
            _ItemLocationTable.LIPBIN = "P";
            if (_ItemLocationTable.Fetch())
            {
                _LocationMasterTable.ResetAll();
                _LocationMasterTable.MCU = _DetailBranchPlant;
                _LocationMasterTable.inLOCN = _ItemLocationTable.LILOCN;
                if (_LocationMasterTable.ValidateLocation())
                    UserSession.ScreenHandler.ScreenMap.Add("PrimaryLocation", _LocationMasterTable.sscnlocn);
                // is workflow option set to default in the item's primary location?
                if (WFOptions.GetYesNoWFOptionValue("SuggestFromPrimary"))
                {
                    UserSession.ScreenHandler.DefaultText = _ItemLocationTable.LILOCN;
                }
                // if the WFO for defaulting to the sales order location is set, override all else
                if (WFOptions.GetYesNoWFOptionValue("SuggestFromPurchaseOrder"))
                {
                    UserSession.ScreenHandler.DefaultText = _PurchaseOrderDetailTable.PDLOCN;
                }
            }

        }

        #endregion

        #region DisplayLockInformation()

        private void DisplayLockInformation()
        {
            UserSession.ScreenHandler.ScreenMap.Add("ORDERNUMBER", _PurchaseOrderDetailTable.PDDOCO.ToString());
            UserSession.ScreenHandler.ScreenMap.Add("DOCTYPE", _PurchaseOrderDetailTable.PDDCTO);
            UserSession.ScreenHandler.ScreenMap.Add("LineNumber", _PurchaseOrderDetailTable.PDLNID.ToString());
            switch (_CollectBranch.WarehouseConstantsTable._DesignatedPrimaryItemNumber)
            {
                case PrimaryItemNumberDesignations.ShortNumber:
                    UserSession.ScreenHandler.ScreenMap.Add("ItemNumber", _PurchaseOrderDetailTable.PDITM.ToString());
                    break;
                case PrimaryItemNumberDesignations.LongNumber:
                    UserSession.ScreenHandler.ScreenMap.Add("ItemNumber", _PurchaseOrderDetailTable.PDLITM);
                    break;
                case PrimaryItemNumberDesignations.ThirdNumber:
                    UserSession.ScreenHandler.ScreenMap.Add("ItemNumber", _PurchaseOrderDetailTable.PDAITM);
                    break;
            }
            UserSession.ScreenHandler.ScreenMap.Add("Description1", _PurchaseOrderDetailTable.PDDSC1);
            UserSession.ScreenHandler.ScreenMap.Add("UserID", _BusinessDataRecordLockTable.UserID);
            UserSession.ScreenHandler.ScreenMap.Add("WorkflowID", _BusinessDataRecordLockTable.WorkflowID);

            Next(_BusinessDataRecordLockInformation);
        }

        #endregion

        #region PrintBarCode

        private void PrintBarCode()
        {
            PrintSystem.Reset();
            PrintSystem.Branch = _BranchPlant.Trim();
            PrintSystem.DropFilePathOverride = WFOptions.RetrieveOptionValue("DropFilePath");
            PrintSystem.PrinterAddress = _PrinterAddress;
            PrintSystem.PrinterName = _PrinterName;

            int NoofLabels;
            try
            {
                NoofLabels = Convert.ToInt32(_Copies);
            }

            catch
            {                
                NoofLabels = 0;
            }
            
            for(int i=1; i <= NoofLabels; i++)
            {
                int NextNumber;
                string NextSerialNumber;
                if (_NextNumberTable.GetNextNumber("SerialNumber", out NextNumber, out NextSerialNumber))
                {
                    PrintSystem.Reset();

                    if ((_LPLicense) && (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()))
                    {
                        PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LicensePlate", _LicensePlate, false);
                    }
                    PrintSystem.ContainerType = "";
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ThirdItemNumber", _ThirdItemNumber, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Container", _Container, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LongItemNumber", _LongItemNumber, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotNumber", _LotNumber, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_TransactionQuantity", _TransactionQuantity.ToString(), false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_UnitOfMeasure", _BarcodeUOMForPrinting, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ExpirationDate", _ExpirationDate, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LocationMasterTable", _LocationMasterTable.sscnlocn, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_PrimaryItemNumber", _PrimaryItemNumber, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_MemoLot1", _MemoLot1, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_MemoLot2", _MemoLot2, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_VendorLot", _VendorLot, false);
                    
                    double QTY = 0;
                    // Stoneridge Speicific Variables
                    if (_PurchaseOrderDetailTable.PDUOM.ToUpper() == "EA")
                    {
                        if (i < NoofLabels)
                        {
                            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Barcode_Qty", Math.Floor(_TransactionQuantity / NoofLabels).ToString(), false);
                            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Qty", Math.Floor(_TransactionQuantity / NoofLabels).ToString(), false);
                            Log.Debug($"<***> _TransactionQuantity = {_TransactionQuantity}");
                            QTY = Math.Floor(_TransactionQuantity / NoofLabels);
                            Log.Debug("&*&*Row Number =" + i + "QTY = " + Math.Floor(_TransactionQuantity / NoofLabels).ToString());
                        }
                        else
                        {
                            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Barcode_Qty", (_TransactionQuantity - ((NoofLabels - 1) * Math.Floor(_TransactionQuantity / NoofLabels))).ToString(), false);
                            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Qty", (_TransactionQuantity - ((NoofLabels - 1) * Math.Floor(_TransactionQuantity / NoofLabels))).ToString(), false);
                            Log.Debug($"<***> _TransactionQuantity = {_TransactionQuantity}");
                            QTY = _TransactionQuantity - ((NoofLabels - 1) * Math.Floor(_TransactionQuantity / NoofLabels));
                            Log.Debug("&*&*Row Number =" + i + "QTY = " + (_TransactionQuantity - ((NoofLabels - 1) * Math.Floor(_TransactionQuantity / NoofLabels))).ToString());
                        }
                    }
                    else
                    {
                        //PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Qty", Math.Round((_TransactionQuantity / NoofLabels), 2).ToString(), false);
                        if(Math.Abs(Math.Round((_TransactionQuantity / NoofLabels), 5) % 1) <= double.Epsilon)
                        {
                            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Barcode_Qty", Math.Round((_TransactionQuantity / NoofLabels), 5).ToString(), false);
                        }
                        else
                        {
                            PrintSystem.ContainerType = "D";
                        }
                        PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Qty", Math.Round((_TransactionQuantity / NoofLabels), 5).ToString(), false);
                        QTY = Math.Round((_TransactionQuantity / NoofLabels), 5);
                        //Log.Debug("&*&*Row Number =" + i + "QTY = " + Math.Round((_TransactionQuantity / NoofLabels), 2).ToString());
                        Log.Debug("&*&*Row Number =" + i + "QTY = " + (_TransactionQuantity / NoofLabels).ToString());
                    }
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_LITM", _LongItemNumber, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Date_Receipt", DateTime.Now.ToString("MMddyyyy"), false);
                    //PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Inspect", "Inspect", false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Lot", _LotNumber, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_PO", _DocumentNumber, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Revision", _PurchaseOrderDetailTable.PDMERL, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Serial", NextSerialNumber, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_DefaultLocation", _DefaultLocation, false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_TotalQTY", "", false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_LabelCount", $"{i} of {NoofLabels}", false);
                    PrintSystem.Copies = 1;
                    PrintSystem.ExtraPrintData.NewRow();

                    
                    //agch 20240423 ISNP begin
                    if (InReceiptRouting())
                    {
                        _SerialTable.InsertSerial(NextSerialNumber, _LongItemNumber, _ShortItemNumber.ToString(), _BranchPlant, _PurchaseOrderDetailTable.PDUOM.ToUpper(), QTY, "E",
                             UserSession.CurrentUser.LoginName, WorkflowID, _PurchaseOrderDetailTable.PDCO, _PurchaseOrderDetailTable.PDDOCO.ToString(),
                             _PurchaseOrderDetailTable.PDDCTO, _LotNumber, "INSP");
                    }

                    else

                    {
                        _SerialTable.InsertSerial(NextSerialNumber, _LongItemNumber, _ShortItemNumber.ToString(), _BranchPlant, _PurchaseOrderDetailTable.PDUOM.ToUpper(), QTY, "E",
                                                UserSession.CurrentUser.LoginName, WorkflowID, _PurchaseOrderDetailTable.PDCO, _PurchaseOrderDetailTable.PDDOCO.ToString(),
                                                _PurchaseOrderDetailTable.PDDCTO, string.Empty);
                    }

                    //agch 20240423 ISNP begin
                    //UpdateF5846001(_LongItemNumber, _PurchaseOrderDetailTable.PDMERL, NextSerialNumber, QTY, _LotNumber);
                    if (!PrintSystem.Print())
                    {
                        DisplayMessage(PrintSystem.ErrorMessageVocabulary);
                    }
                }
            }
            if(WFOptions.RetrieveOptionValue("TotalLabelPrint").ToString() == "Y")
            {
                int NextNumber;
                string NextSerialNumber;
                _NextNumberTable.GetNextNumber("SerialNumber", out NextNumber, out NextSerialNumber);
                PrintSystem.Reset();
                if (_PurchaseOrderDetailTable.PDUOM.ToUpper() == "EA")
                {
                    PrintSystem.ContainerType = "";
                }
                else
                {
                    PrintSystem.ContainerType = "D";
                }
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_LITM", _LongItemNumber, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Date_Receipt", DateTime.Now.ToString("MMddyyyy"), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Lot", "TOTAL QTY", false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_PO", _DocumentNumber, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Revision", _PurchaseOrderDetailTable.PDMERL, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_DefaultLocation", _DefaultLocation, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Qty", _TransactionQuantity.ToString(), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Barcode_Qty",_TransactionQuantity.ToString(), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Serial", NextSerialNumber, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_TotalQTY", "TOTAL QTY", false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_LabelCount", $"{NoofLabels} Cartons", false);
                //revisar
                //agch 20240423 ISNP begin
                if (InReceiptRouting())
                {
                    _SerialTable.InsertSerial(NextSerialNumber, _LongItemNumber, _ShortItemNumber.ToString(), _BranchPlant, _PurchaseOrderDetailTable.PDUOM.ToUpper(), _TransactionQuantity, "E",
                         UserSession.CurrentUser.LoginName, WorkflowID, _PurchaseOrderDetailTable.PDCO, _PurchaseOrderDetailTable.PDDOCO.ToString(),
                         _PurchaseOrderDetailTable.PDDCTO, _LotNumber, "INSP");
                }

                else

                {
                    _SerialTable.InsertSerial(NextSerialNumber, _LongItemNumber, _ShortItemNumber.ToString(), _BranchPlant, _PurchaseOrderDetailTable.PDUOM.ToUpper(), _TransactionQuantity, "E",
                                                UserSession.CurrentUser.LoginName, WorkflowID, _PurchaseOrderDetailTable.PDCO, _PurchaseOrderDetailTable.PDDOCO.ToString(),
                                                _PurchaseOrderDetailTable.PDDCTO, string.Empty);
                }
                //AGCH end
                PrintSystem.Copies = 1;
                PrintSystem.ExtraPrintData.NewRow();

                if (!PrintSystem.Print())
                {
                    DisplayMessage(PrintSystem.ErrorMessageVocabulary);
                }
            }
        }

        #endregion

        #region ShowNextScreen
        private void ShowNextScreen()
        {
            if ((InReceiptRouting()) && (RoutingMessageDisplayed == false))
            {
                RoutingMessageDisplayed = true;
                Next(_ReceiptRoutingMessage);
            }
            else
            {
                if (StockItem())
                    _LotProcessType = _ItemBranchTable.IBSRCE;
                else
                {
                    _LotNumber = "";
                    _LotProcessType = "";
                    _Location = "";
                    Next(_CollectQuantity);
                }
                // check to see if this is a lot controlled item, and if so, display the lot
                // verification screen

                if (PromptForLot())
                {
                    Next(_CollectLotNumber);
                    return;
                }
                if (PromptForMemoLot())
                {
                    Next(_CollectMemoLot1);
                    return;
                }
                _LotNumber = " ";
                DefaultPrimary();
                Next(_CollectLocation);
            }
        }

        #endregion

        #region ReturnFromTransaction

        private void ReturnFromTransaction()
        {

            if (_FromSerialNumberEntry)
            {
                switch (Convert.ToInt16(_LotProcessType))
                {
                    case 5:      // system assigned
                    case 6:      // system assigned
                        if (!WFOptions.GetYesNoWFOptionValue("AllowLotOverride"))
                        {
                            _LotNumber = "";
                            _ExpirationDate = "";
                            while (_SerialNumbersScanned < _SerialNumberQuantity)
                            {
                                _SerialNumbersScanned += 1;
                                // convert quantity to primary for JDE update
                                _CollectUnitOfMeasure.Validate(_CollectItemNumber.ItemsTable.IMUOM1);
                                _TransactionQuantity = 1;
                                // call web service to update PeopleSoft
                                //if (_SerialNumbersScanned == _TotalReceiptQuantity)
                                _SendFlag = "Now";
                                if (JDEReservationTable.DoJDERecordLocksExist("F4311", _PurchaseOrderHeaderTable.PHDOCO.ToString(), _PurchaseOrderHeaderTable.PHDCTO, _PurchaseOrderHeaderTable.PHKCOO))
                                {
                                    _PurchaseOrderDetailTable.UnLockRecord(_BusinessDataRecordLockTable, UserSession.ScreenHandler.Environment);
                                    Error("RECORDRESERVED");
                                    RestartLoop(_CollectOrderNumber);
                                }
                                else
                                { 
                                    SendToERP();
                                    if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                                    {
                                        UserSession.ScreenHandler.ScreenMap.Add("BarcodeUnitOfMeasure", _CollectItemNumber.ItemsTable.IMUOM1);
                                        UserSession.ScreenHandler.DefaultText = "1";
                                    }
                                    Back(_PrintBarCodeLabels);
                                }

                                return;
                            }
                        }
                        else
                        {
                            if (_SerialNumbersScanned < _SerialNumberQuantity)
                            {
                                UserSession.ScreenHandler.DefaultText = "";
                                Back(_CollectSerialNumber);
                                return;
                            }
                        }
                        break;
                    default:

                        if (_SerialNumbersScanned < _SerialNumberQuantity)
                        {
                            UserSession.ScreenHandler.DefaultText = "";
                            Back(_CollectSerialNumber);
                            return;
                        }
                        break;
                }
            }

            _FromGenLP = false;

            ClearTransaction();
            _BusinessDataLocked = false;
            // if no more items exist to be received, then go back to document number screen
            switch (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper())
            {
                //CS8448 - Changed Back to RestartLoop in switch statement for ReturnFromTransaction method because after
                //         receipt the workflow would prompt the user for the PO number instead of taking user back to
                //         F4311 List - previously fixed in 5.2.1 by ccox
                case "P":
                    if (_PurchaseOrderDetailTable.ShowOpenLines(_AllowedNextStatusCodes, _DocumentNumber, _DocumentType, ""))
                        RestartLoop(_CollectItemNumber);
                    else
                        RestartLoop(_CollectOrderNumber);
                        break;
                case "I":
                    RestartLoop(_CollectByItem);
                    break;
            }
        }

        #endregion

        #region PromptForLot
        private bool PromptForLot()
        {
            int LotType;
            if (_LotProcessType.Trim().Length > 0)
            {
                LotType = Convert.ToInt16(_LotProcessType);
                if ((LotType >= Convert.ToInt16(LotSerialDesignations.LotOptional)) &&
                   (LotType <= Convert.ToInt16(LotSerialDesignations.LotRequired)))
                {
                    if ((_LotProcessType.Trim() == "1" || _LotProcessType.Trim() == "2") &&
                       (!WFOptions.GetYesNoWFOptionValue("AllowLotOverride")))
                        return false;
                    else
                        return true;
                }
                else
                    return false;

            }
            else
                return false;
        }
        #endregion

        #region PromptForMemoLot
        private bool PromptForMemoLot()
        {
            int LotType;
            if (_LotProcessType.Trim().Length > 0)
            {
                LotType = Convert.ToInt16(_LotProcessType);
                if ((LotType >= Convert.ToInt16(LotSerialDesignations.LotOptional)) &&
                   (LotType <= Convert.ToInt16(LotSerialDesignations.LotRequired)))
                {
                    return true;
                }
                return false;

            }
            else
                return false;
        }

        private bool IsItemSerlialized()
        {
            return !string.IsNullOrWhiteSpace(_LotProcessType)
                   && _LotProcessType.GetInt() >= Convert.ToInt32(LotSerialDesignations.SerialOptional)
                   && _LotProcessType.GetInt() <= Convert.ToInt32(LotSerialDesignations.SerialRequired);
        }

        #endregion

        #region LotRequired
        private bool LotRequired()
        {
            int LotType;
            if (_LotProcessType.Trim().Length > 0)
            {
                LotType = Convert.ToInt16(_LotProcessType);
                if ((LotType == Convert.ToInt16(LotSerialDesignations.LotRequired)) ||
                   (LotType == Convert.ToInt16(LotSerialDesignations.SerialRequired)))
                    return true;
                else
                    return false;

            }
            else
                return false;
        }
        #endregion

        #region StockItem
        private bool StockItem()
        {
            bool StockItem = true;
            // if this is a non-stock line, set flag
            for (int i = 0; i < _NonStockLineTypes.Length; i++)
            {
                if (_PurchaseOrderDetailTable.PDLNTY.Trim() == _NonStockLineTypes[i].Trim())
                    StockItem = false;
            }
            return (StockItem);
        }
        #endregion

        #region ValidDocumentType
        // determine whether document type of order is allowed
        // based on the workflow options.
        private bool ValidDocumentType()
        {

            bool found = false;

            for (int i = 0; i < _AllowedDocTypes.Length; i++)
            {
                if (_AllowedDocTypes[i].Trim().Length > 0)
                {
                    if (_DocumentType == _AllowedDocTypes[i].ToUpper().Trim())
                        found = true;
                }
                else
                    found = true;
            }

            return found;


        }
        #endregion

        #region CreateDataSet
        private void CreateDataSet()
        {
            _GeneratedLicensePlateNumbersDataSet = new DataSet("GeneratedLicensePlateNumbersDataSet");
            _GeneratedLicensePlateNumbersTable = new DataTable("GeneratedLicensePlateNumbersTable");
            _GeneratedLicensePlateNumbersTable.Columns.Add("Number");
            _GeneratedLicensePlateNumbersDataSet.Tables.Add(_GeneratedLicensePlateNumbersTable);

        }


        #endregion

        #region LicensePlating
        private bool LicensePlating()
        {
            var isModuleLicensed = UserSession.IsModuleLicensed("JLP");
            Log.Debug($"isModuleLicensed = {isModuleLicensed}.");
            if (isModuleLicensed)
                return true;
            else
                return false;
        }
        #endregion

        #region UpdateF5846001
        private void UpdateF5846001(string Part, string Revision, string SerialNumber, double Quantity, string Lot)
        {
            DateTime PrintDate = DateTime.Now;
            string connectionString = "";

            string server = RFSAppSettings.AppSettings["DataServer"]?.ToString();
            string database = RFSAppSettings.AppSettings["DataDB"]?.ToString();
            string user = RFSAppSettings.AppSettings["DataUser"]?.ToString();
            string password = RFSAppSettings.AppSettings["DataPass"]?.ToString();
            RC4Engine engine = new RC4Engine();

            if (!string.IsNullOrWhiteSpace(password))
            {

                engine.EncryptionKey = "orcdarco+3+u5SIyXrRPG5CRn7NsUaUVMoW6ASuhlCY=";
                engine.CryptedText = Encoding.UTF8.GetString(Convert.FromBase64String(password));
                engine.Decrypt();
            }

            connectionString= $"Server={server};Database={database};User Id={user};Password={engine.InClearText}";

            string tablePrefix = "";
            switch (this.WorkflowOptions["SerialFileEnv"].ToString().ToUpper())
            {
                case "TR":
                   // connectionString = "Data Source=NAJDTSQLCL;Initial Catalog=JDE_TR;User=TRDTA;Password=TRDTA;";
                    tablePrefix = "TR";
                    break;
                case "CRP":
                    //connectionString = "Data Source=NAJDTSQLCL;Initial Catalog=JDE_CRP;User=CRPDTA;Password=CRPDTA;";
                    tablePrefix = "CRP";
                    break;
                case "PROD":
                    //connectionString = "Data Source=NAJDPSQLCL.stoneridge.com;Initial Catalog=JDE_PRODUCTION;User=JDEPACK;Password=K^bbmX^xU!FnAKa9TG#EXS5F#;";
                    tablePrefix = "PROD";
                    break;
            }
            SqlConnection cnJDE = new SqlConnection(connectionString);
            //agch 20240328
            //string Sql = $"INSERT INTO {tablePrefix}DTA.F5846001 (LPLITM, LPMERL, LP74DPD, LPLPNU, LPITQTY, LPLOTN, LPMCU, LPITM, LPEDER) VALUES ('@Part','@Revision', @PrintDate, '@SerialNumber', @Quantity, '@Lot', '@BranchPlant', @ShortItem, '')";
            string Sql = $"INSERT INTO {tablePrefix}DTA.F5846001 (LPLITM, LPMERL, LP74DPD, LPLPNU, LPITQTY, LPLOTN, LPMCU, LPITM, LPEDER,LPCPRGP2) VALUES ('@Part','@Revision', @PrintDate, '@SerialNumber', @Quantity, '@Lot', '@BranchPlant', @ShortItem, '',@INSP)";
            Sql = Sql.Replace("@Part", Part);
            Sql = Sql.Replace("@Revision", Revision);
            Sql = Sql.Replace("@PrintDate", "1" + PrintDate.ToString("yy") + PrintDate.DayOfYear.ToString().PadLeft(3, '0'));
            Sql = Sql.Replace("@SerialNumber", SerialNumber);
            Sql = Sql.Replace("@Quantity", Quantity.ToString());
            Sql = Sql.Replace("@Lot", Lot);
            Sql = Sql.Replace("@BranchPlant", _PurchaseOrderDetailTable.PDMCU);
            Sql = Sql.Replace("@ShortItem", _PurchaseOrderDetailTable.PDITM.ToString());

            ////agch 20240328 begin include field Inspection
            if (InReceiptRouting())
            {
                Sql = Sql.Replace("@INSP", "INSP");
            }

            else
            {
                Sql = Sql.Replace("@INSP", "");

            }
            ////agch 20240328 end
            Log.Debug($"(***) SQL = {Sql}");
            SqlCommand cmd = new SqlCommand(Sql, cnJDE);
            try
            {

                cnJDE.Open();
                cmd.ExecuteNonQuery();
                cnJDE.Close();
            }
            catch(SqlException e)
            {
                Log.Debug($"(***) Sql Error {e.Message}");
            }
            finally
            {
                cnJDE.Close();
                cnJDE.Dispose();
            }


        }
        #endregion

        private void _BranchPlantList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CollectBranch.BranchPlantsTable.FetchBranchPlants();
        }



        private void _PurchaseOrderHeaderList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper())
            {
                case "P": // receipts by P/O
                    _PurchaseOrderHeaderTable.ShowOpenOrders(_BranchPlant, _AllowedNextStatusCodes, _AllowedDocTypes);
                    break;
                case "I": // receipts by Item //CC025852
                    _PurchaseOrderDetailTable.ShowOpenItems(_AllowedNextStatusCodes, _BranchPlant, _CollectItemNumber.ItemsTable.IMITM.ToString());
                    break;
            }
        }

        private void _UserDefinedCodesList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UserDefinedCodesTable.ResetAll();
            _UserDefinedCodesTable.DRSY = "00  ";
            _UserDefinedCodesTable.DRRT = "DT";
            _UserDefinedCodesTable.Fetch();
        }

        private void _CollectOrderType_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper())
            {
                case "I":
                    SetUpPOSQL(_PrimaryItemNumber);
                    _PurchaseOrderDetailTable.Fetch();
                    if (_PurchaseOrderDetailTable.Rows.Count > 0)
                    {
                        _DetailBranchPlant = _PurchaseOrderDetailTable.PDMCU;

                        LoadBranchPlantData(); //CC025852
                        UserSession.ScreenHandler.ScreenMap.Add("DetailBranchPlant", _DetailBranchPlant);
                        UserSession.ScreenHandler.ScreenMap.Add("Branch", _DetailBranchPlant);
                        _LineNumber = _PurchaseOrderDetailTable.PDLNID;
                        //_ShortItemNumber = _PurchaseOrderDetailTable.PDITM;
                        //_LongItemNumber = _PurchaseOrderDetailTable.PDLITM;
                        //_ThirdItemNumber = _PurchaseOrderDetailTable.PDAITM;

                        Next(_CollectContainerNumber);
                    }
                    else
                        Error("ITEMNOTONORDER", _PrimaryItemNumber);

                    break;
                default:
                    break;
            }
        }

        private void _CollectSupplierRemarks_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (WFOptions.RetrieveOptionValue("ReceiptType").ToUpper())
            {
                case "P":
                    Next(_CollectItemNumber);
                    break;
                case "I":
                    if ((InReceiptRouting()) && (RoutingMessageDisplayed == false))
                    {
                        RoutingMessageDisplayed = true;
                        Next(_ReceiptRoutingMessage);
                        return;
                    }

                    _LineNumber = _PurchaseOrderDetailTable.PDLNID;
                    //_ShortItemNumber = _PurchaseOrderDetailTable.PDITM;
                    //_LongItemNumber = _PurchaseOrderDetailTable.PDLITM;
                    //_ThirdItemNumber = _PurchaseOrderDetailTable.PDAITM;

                    if (PromptForLot())
                    {
                        Next(_CollectLotNumber);
                        return;
                    }
                    if (PromptForMemoLot())
                    {
                        Next(_CollectMemoLot1);
                        return;
                    }
                    _LotNumber = " ";
                    DefaultPrimary();
                    Next(_CollectLocation);

                    break;
            }
        }

        private void _DefinedPrintersList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DefinedPrintersTable.FetchPrintersByUserWorkflow(UserSession.UserId, WorkflowID);
        }

        private void _BusinessDataRecordLockInformation_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back();
        }

        private void _LicensePlateMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlateMasterTable.ResetAll();
            _LicensePlateMasterTable.LMMCU = _BranchPlant;
            _LicensePlateMasterTable.Fetch();
        }

        private void _ItemMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CollectItemNumber.ItemsTable.ShowItemsCurrentlyOnOrder(_BranchPlant);

        }

        private void _EmptyLocationsList_ExitValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(_CollectLocation);
        }

        #endregion

        private void _CollectSerialNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_PurchaseOrderDetailTable.PDLOTN.Trim().Length == 0)
                DefaultRFSGeneratedLotNumber();
            else
                ScreenHandler.DefaultText = _PurchaseOrderDetailTable.PDLOTN;
        }

        private void _CollectLotNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // Default today's date in 'yyyyMMdd' format if WFO is set to yes

            //if (_PurchaseOrderDetailTable.PDLOTN.Trim().Length == 0)
            //{
            //    DefaultRFSGeneratedLotNumber();
            //}
            //else if (WFOptions.GetYesNoWFOptionValue("DefaultLotSerial"))
            //{
            //    ScreenHandler.DefaultText = _PurchaseOrderDetailTable.PDLOTN;
            //}

            if (WFOptions.GetYesNoWFOptionValue("DefaultLotSerial"))
            {
                ScreenHandler.DefaultText = DateTime.Now.ToString("yyyyMMdd");
            }
            else
            {
                ScreenHandler.DefaultText = "";
            }
        }
        private void DefaultRFSGeneratedLotNumber()
        {
            if (WFOptions.GetYesNoWFOptionValue("RFSGenLotSerialNumber"))
            {
                int NextNumber = 0;
                string FormattedNextNumber = "";
                if (_NextNumberTable.GetNextNumber("LotNumber", out NextNumber, out FormattedNextNumber))
                    ScreenHandler.DefaultText = FormattedNextNumber;
            }
        }

        private void _CollectLotSerialExpirationDate_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("DateFormat", WFOptions.RetrieveOptionValue("DateFormat").ToString());
        }

        private void _BranchPlantList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (WFOptions.RetrieveOptionValue("ReceiptType") == "P")
            {
                Next(_CollectOrderNumber);
            }
            else
            {
                Next(_CollectByItem);
            }
        }

        private void CollectDualQuantity_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            //XE doesn't have DUALUOM columns, so we can't use that here. DUOM was removed from 3.8.2
            _CollectDualQuantity.CancelLoad = true;
            _CollectDualUnitOfMeasure.CancelLoad = true;
            Next(CollectPackingSlip);
            //ProcessThisReceipt();
        }

        private void CollectDualQuantity_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DualQuantity = _CollectDualQuantity.Quantity;
        }

        private void CollectDualUnitOfMeasure_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DualUnitOfMeasure = _CollectDualUnitOfMeasure.UnitOfMeasure;
            Next(CollectPackingSlip);
            //ProcessThisReceipt();
        }

        private void _CollectDualUnitOfMeasure_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureConversionTable1.ResetAll();
            _UnitOfMeasureConversionTable1.UMITM = _PurchaseOrderDetailTable.PDITM;
            _UnitOfMeasureConversionTable1.Where("", _UnitOfMeasureConversionTable1.Columns.UMUM, ICS.Database.Condition.NotEqual, _CollectUnitOfMeasure.UnitOfMeasure, "");
            _UnitOfMeasureConversionTable1.Fetch();
        }

        private void _CollectUnitOfMeasure_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureConversionTable1.ResetAll();
            //agch 2025
            Log.Debug($"(*#*#*) _PurchaseOrderDetailTable.PDITM {_PurchaseOrderDetailTable.PDITM}");
            _UnitOfMeasureConversionTable1.UMITM = _PurchaseOrderDetailTable.PDITM;
            _UnitOfMeasureConversionTable1.Fetch();
        }

        protected bool ValidateDualUnitOfMeasure(string sDualUOM)
        {
            double dConversionFactor = 0;
            sDualUOM = sDualUOM.Trim().ToUpper();

            //We don't need to do any validation if the UOM collected is the 
            // same as the item's primary UOM
            if (sDualUOM == _PurchaseOrderDetailTable.PDDUAL)
            {
                _CollectDualUnitOfMeasure.ScreenInput = sDualUOM;
                return true;
            }

            // Well, the UOM collected is not the same as the primary UOM.
            // Validate by getting a conversion to F4311.PDDUAL
            dConversionFactor =
                  _CollectDualUnitOfMeasure.UOMConversionsTable.ConvertToPrimaryUOM(_BranchPlant,
                                                          _ShortItemNumber,
                                                          sDualUOM,
                                                          _PurchaseOrderDetailTable.PDDUAL);

            // No conversion factor.  Try again with the defaults table
            if (dConversionFactor == 0)
            {
                dConversionFactor =
                   _CollectDualUnitOfMeasure.UOMDefaultConversionsTable.ConvertToPrimaryUOM(sDualUOM, _PurchaseOrderDetailTable.PDDUAL);
            } // end if

            // Did we get a conversion factor?
            if (dConversionFactor == 0)
            {
                // No.
                _CollectDualUnitOfMeasure.ErrorMessage = "NOUOMCONVERSION";
                _CollectDualUnitOfMeasure.Reset();
                return false;
            } // end if
            else
            {
                // We have a good UOM!
                _CollectDualUnitOfMeasure.ScreenInput = _UnitOfMeasureConversionTable1.UMUM;
                return true;
            }
        }
        #region Multiple Items on PO List Events
        private void MultiItemPOList_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            string ItemNoSelected;

            _PurchaseOrderDetailTable.UpdateCurrentRow();
            ItemNoSelected = _PurchaseOrderDetailTable.PDLITM;
            
            _PurchaseOrderDetailTable.ResetAll();
            _PurchaseOrderDetailTable.PDDOCO = _PurchaseOrderHeaderTable.PHDOCO;
            _PurchaseOrderDetailTable.PDKCOO = _PurchaseOrderHeaderTable.PHKCOO;
            _PurchaseOrderDetailTable.PDDCTO = _PurchaseOrderHeaderTable.PHDCTO;
            _PurchaseOrderDetailTable.PDLITM = ItemNoSelected;
            _PurchaseOrderDetailTable.Fetch();

            _CollectContainerNumber.DefaultTextForScreen = _PurchaseOrderDetailTable.PDCNID;   //What is container number
            _CollectSupplierRemarks.DefaultTextForScreen = "";
            _CollectItemNumber.DefaultTextForScreen = _PurchaseOrderDetailTable.PDLITM;
            Next(_CollectContainerNumber);
        }

        private void MultiItemPOList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {

        }

        private void MultiItemPOList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PurchaseOrderDetailTable.DistinctItemPO(_PurchaseOrderDetailTable.PDKCOO, _PurchaseOrderDetailTable.PDDOCO, _PurchaseOrderDetailTable.PDDCTO, _AllowedNextStatusCodes);
        }
        #endregion


    } // end StoneridgePOReceipts Class
} // end ICS.Workflow.Base.JDE.Receiving Namespace