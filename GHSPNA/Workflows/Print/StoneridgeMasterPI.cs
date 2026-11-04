using System;
using System.Data;
using System.Collections;
using ICS.Database.TableObjects.JDE;
using ICS.Database.TableObjects.RFS;
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;
using ICS.Display;
using ICS.ERPI.JDE;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using System.Linq;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Print
{
    /// <summary>
    /// Screen Control Class For: StoneridgeMasterPI Workflow
    /// </summary>
    public class StoneridgeMasterPI : ICS.Workflow.Base.WorkflowDesign.Components.RFSWorkflowDesigner
    {
        #region Fields

        #region Tables

        // define tables used
        private F0005 _UserDefinedCodesTable; // JDE user defined codes

        //private F0006 _BranchPlantTable; // JDE Branch Master
        private F4100 _LocationMasterTable; // JDE location master

        //private F41001 _WarehouseControlTable; // Warehouse Constants Table
        private F41002 _tblUOMConversion; // jde UOM conversion tables

        private F41003 _tblUOMConversion2; // jde UOM conversion tables
        //private F4101 _ItemMasterTable; // JDE item master

        private F41021 _ItemLocationTable; // jde item/lot/location

        private F4108 _LotMasterTable; // jde lot master

        private F46011 _ItemUOMProfileTable;

        private F46L10 _LicensePlateMasterTable;

        private CustomJDETables.F4801 _WorkOrderMasterTable; // jde work order master

        private RFS_DataManipulator _DataManipulatorTable;

        private RFS_PrinterDefinition _DefinedPrintersTable; // RFS Defined Printers

        private RFS_PrintXref _PrinterCrossReferenceTable;

        private RFS_NextNumber _NextNumberTable;

        private DataSet _GeneratedLicensePlateNumbersDataSet;

        private DataTable _GeneratedLicensePlateNumbersTable;

        private F46L001 _LicensePlateNumberingConstantsTable;

        private CustomJDETables.F4102 _ItemBranchTable; // JDE item/Branch

        private CustomJDETables.F4211 _SalesOrderDetailTable;

        private CustomJDETables.F3112 _WorkOrderRoutingTable;

        private CustomJDETables.F3112Z1 _WorkOrderTimeTableZfile;

        private CustomJDETables.F4801Z1 _WorkOrderMasterTableZfile;

        private CustomJDETables.F5846001 _SerialTable;

        private CustomJDETables.F3109 _LineItemMasterTable;

        private CustomJDETables.F5642001 _JobNumberTableZfile;

        private CustomJDETables.F49211 _SalesOrderDetailTagTable;

        private CustomJDETables.F59RFCTL _PalletSerialTable;

        private CustomJDETables.F56PI001 _PISerialTable;

        #endregion

        #region Screens

        ICS.Workflow.Base.JDE.Components.BranchPlantScreen _CollectBranch;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _PrintBarCodeLabels;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _PrintBarCodeLabelsStartOfOrder;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPrinter;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPrinterStartOfOrder;

        // list/lookup screens
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _BranchPlantList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _DefinedPrintersTableList;

        #endregion

        #region Collect/Work data

        private string _PrinterName;

        private string _BranchPlant; // work order Branch

        private string _PrimaryItemNumber; // primary item number

        private string _Location; // issue location

        private string _LotNumber; // lot/serial number

        private string _LotSerialExpDate; // lot/serial expiration date

        private string _ToLotGrade;

        private string _ToLotPotency;

        private double _TransactionQuantity; // Used to hold the scrapped quantity and UOM converted

        private string _UnitOfMeasure; // to the primary

        private bool _StartOfOrder;

        private double _ScrapQuantity; // Used to hold the scrapped quantity and UOM converted

        private string _ScrapUnitOfMeasure; // to the primary

        private double _ShortItemNumber; // short item number

        private string _LongItemNumber; // second item number

        private string _ThirdItemNumber; // third item number

        private string _LotStatus; // lot status

        private string _PrinterAddress;

        private string _IsLocationControlActive; // Y/N flag to denote if whse is using location control

        private int _SerialNumbersScanned; // s/n scanned

        private string _LicensePlate;

        private string _LicensePlateProcessingType = "";

        private bool _FromSerialNumberEntry = false;

        private string _LocationSeparator;

        private string[] _AllowedLotHoldCodes = null; // allowed lot hold codes

        private string[] _AllowedWorkOrderStatusCodes = null;

        private bool _FromGenLP = false;

        private bool _LPLicense = false;

        private string[] _CaseUOM;

        private double _PalletNumber;

        private string _SerialNumber;

        private double _TagNumber;

        #endregion

        protected ICS.Workflow.Base.JDE.Components.ItemScreen _CollectItemNumber;

        private string[] _LPEDER;

        protected GenericScreen CollectPalletNumber;

        protected GenericScreen CollectCartonSerial;

        protected GenericScreen RemoveCartonSerial;

        protected ICS.Workflow.Base.JDE.Components.YesNoResponseScreen ConfirmSerialRemove;

        protected GenericScreen CollectLocation;

        private DataTable _ScannedData;

        protected ICS.Workflow.Base.JDE.Components.YesNoResponseScreen ConfirmDelete;

        #region ERPI

        private ICS.ERPI.JDE.WOCompletion _WOCompletionTransaction = new WOCompletion();

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Workflow : StoneridgeMasterPI
        /// </summary>
        /// <param name="userSession">Contains UserSession info for controling screen display and DAL.</param>
        public StoneridgeMasterPI(UserSession userSession) : base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        } // end StoneridgeMasterPI constructor

        #endregion

        #region Dispose

        /// <summary>
        /// Used to perform any actions necessary during the destruction of an object of
        /// this class like cleanup, etc.
        /// </summary>
        public override void Dispose()
        {
            _GeneratedLicensePlateNumbersDataSet.Dispose();
            ScreenMap.ClearAllWorkflows();
            Transactions.ClearTransactions();
            ScreenMap.Clear("Error");
            base.Dispose();
        } // end Dispose

        #endregion

        #region Methods

        #region Initialize

        /// <summary>
        /// Initializes Workflow and Sets screens and Table Objects.
        /// Declares Next with starting screen
        /// </summary>
        protected override void Initialize()
        {
            #region Table Inits

            _UserDefinedCodesTable = new F0005(UserSession.DataAccess, "");
            //_BranchPlantTable = new F0006(UserSession.DataAccess, "");
            _LocationMasterTable = new F4100(UserSession.DataAccess, "");
            //_WarehouseControlTable = new F41001(UserSession.DataAccess, "");
            _tblUOMConversion = new F41002(UserSession.DataAccess, "");
            //_ItemMasterTable = new F4101(UserSession.DataAccess, "");

            _ItemLocationTable = new F41021(UserSession.DataAccess, "");
            _LotMasterTable = new F4108(UserSession.DataAccess, "");
            _ItemUOMProfileTable = new F46011(UserSession.DataAccess, "");
            _WorkOrderMasterTable = new CustomJDETables.F4801(UserSession.DataAccess, "");
            _DataManipulatorTable = new RFS_DataManipulator(UserSession.DataAccess, "");
            _DefinedPrintersTable = new RFS_PrinterDefinition(UserSession.DataAccess, "");
            _PrinterCrossReferenceTable = new RFS_PrintXref(UserSession.DataAccess, "");
            _NextNumberTable = new RFS_NextNumber(UserSession.DataAccess, "");
            _LicensePlateNumberingConstantsTable = new F46L001(UserSession.DataAccess, "");
            _tblUOMConversion2 = new F41003(UserSession.DataAccess, "");

            _ItemBranchTable = new CustomJDETables.F4102(UserSession.DataAccess, "");
            _SalesOrderDetailTable = new CustomJDETables.F4211(UserSession.DataAccess, "");
            _WorkOrderRoutingTable = new CustomJDETables.F3112(UserSession.DataAccess, "");
            _WorkOrderTimeTableZfile = new CustomJDETables.F3112Z1(UserSession.DataAccess, "");
            _SerialTable = new CustomJDETables.F5846001(UserSession.DataAccess, "");
            _LineItemMasterTable = new CustomJDETables.F3109(UserSession.DataAccess, "");
            _JobNumberTableZfile = new CustomJDETables.F5642001(UserSession.DataAccess, "");
            _SalesOrderDetailTagTable = new CustomJDETables.F49211(UserSession.DataAccess, "");
            _PalletSerialTable = new CustomJDETables.F59RFCTL(UserSession.DataAccess, "");
            _PISerialTable = new CustomJDETables.F56PI001(UserSession.DataAccess, "");
            _ScannedData = new DataTable();
            _ScannedData.Columns.Add("Serial", typeof(string));
            _ScannedData.Columns.Add("LongItem", typeof(string));
            _ScannedData.Columns.Add("Quantity", typeof(double));
            _ScannedData.Columns.Add("ShortNumber", typeof(double));
            _ScannedData.Columns.Add("Location", typeof(string));
            _ScannedData.Columns.Add("UOM", typeof(string));

            #endregion

            ClearTransaction();
            // Setting up workflow options
            char[] Delimiter = {' '};
            string sTemp = WFOptions.RetrieveOptionValue("AllowedWorkOrderStatus");
            _AllowedWorkOrderStatusCodes = sTemp.Split(Delimiter);
            sTemp = WFOptions.RetrieveOptionValue("AllowedLotHoldCodes");
            _AllowedLotHoldCodes = sTemp.Split(Delimiter);
            sTemp = WFOptions.RetrieveOptionValue("CaseUOM");
            _CaseUOM = sTemp.Split(Delimiter);
            _LPEDER = new string[]
            {
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
            };
            // License plate tables
            if (UserSession.IsModuleLicensed("JLP"))
            {
                _LPLicense = true;
                _LicensePlateMasterTable = new F46L10(UserSession.DataAccess, "");
            } // end if

            CreateDataSet();
        } // end Initialize

        #endregion

        #region CollectBranch

        //*****************************************************************
        // CollectBranch
        //*****************************************************************

        #region CollectBranch_EnterEvent

        /// <summary>
        /// Enter event for the Collect Branch Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectBranch_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _BranchPlant = e.UserInput.ToString().ToUpper();
            BranchPlantEnter();
        }

        private void BranchPlantEnter()
        {
            _CollectBranch.BranchPlantsTable.ResetAll();
            _CollectBranch.BranchPlantsTable.BranchPlant = _BranchPlant;

            // Validate the Branch entered against the F0006 table
            if (_CollectBranch.BranchPlantsTable.ValidBranchPlant())
            {
                _CollectBranch.Validate(_BranchPlant);
                _BranchPlant = _DataManipulatorTable.FormatData("F0006", "MCMCU", _CollectBranch.BranchPlantsTable.MCMCU);
                _CollectBranch.WarehouseConstantsTable.ResetAll();
                _CollectBranch.WarehouseConstantsTable.CIMCU = _BranchPlant;
                _CollectBranch.WarehouseConstantsTable.Fetch();
                if (_CollectBranch.WarehouseConstantsTable.Rows.Count > 0)
                {
                    _LocationSeparator = _CollectBranch.WarehouseConstantsTable.CISEPL;
                    _IsLocationControlActive = _CollectBranch.WarehouseConstantsTable.CILCTL;
                } // end if
                else
                {
                    _IsLocationControlActive = "N";
                } // end else

                _LicensePlateNumberingConstantsTable.ResetAll();
                _LicensePlateNumberingConstantsTable.BranchPlant = _BranchPlant;
                //if(WFOptions.RetrieveOptionValue("LabelCreation") == "3")
                //{
                //    Next(CollectCartonSerial);
                //}
            } // end if
            else
            {
                Error("MCU", _BranchPlant);
            } // end else
        } // end CollectBranch_EnterEvent

        #endregion

        #region BranchPlantList

        //*****************************************************************
        // BranchPlantList
        //*****************************************************************

        #region BranchPlantList_EnterEvent

        /// <summary>
        /// Enter event for the BranchPlantList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BranchPlantList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CollectBranch.BranchPlantsTable.UpdateCurrentRow();
            _BranchPlant = _DataManipulatorTable.FormatData("F0006", "MCMCU", _CollectBranch.BranchPlantsTable.MCMCU);
            BranchPlantEnter();
        } // end BranchPlantList_EnterEvent

        #endregion

        #endregion

        #endregion

        #region LoadScreenMap

        /// <summary>
        /// LoadScreenMap is used to initialize data and set ScreenMaps such as the Primary
        /// Item Number and Location, WorkOrderNumber, etc.
        /// </summary>
        private void LoadScreenMap()
        {
            //Set item screen info even though we aren't showing it
            // b/c other screens have reference to it.
            _CollectItemNumber.Validate(_WorkOrderMasterTable.WALITM);

            _ShortItemNumber = Convert.ToDouble(_WorkOrderMasterTable.WAITM);
            _ThirdItemNumber = _WorkOrderMasterTable.WAAITM;
            _LongItemNumber = _WorkOrderMasterTable.WALITM;

            //ScreenMap.Add("WorkOrderNumber", _WorkOrderNumber.ToString());
            ScreenMap.Add("ItemNumber", _LongItemNumber);

            _CollectItemNumber.ItemsTable.ResetAll();
            _CollectItemNumber.ItemsTable.IMITM = _ShortItemNumber;
            _CollectItemNumber.ItemsTable.Fetch();
            switch (_CollectBranch.WarehouseConstantsTable._DesignatedPrimaryItemNumber)
            {
                case PrimaryItemNumberDesignations.ShortNumber:
                    _PrimaryItemNumber = _CollectItemNumber.ItemsTable.IMITM.ToString();
                    break;
                case PrimaryItemNumberDesignations.LongNumber:
                    _PrimaryItemNumber = _CollectItemNumber.ItemsTable.IMLITM;
                    break;
                case PrimaryItemNumberDesignations.ThirdNumber:
                    _PrimaryItemNumber = _CollectItemNumber.ItemsTable.IMAITM;
                    break;
            } // end switch

            _LocationMasterTable.ResetAll();
            _LocationMasterTable.inLOCN = _ItemLocationTable.RetrievePrimaryLocation(_BranchPlant, _ShortItemNumber);
            _LocationMasterTable.MCU = _BranchPlant;
            if (_LocationMasterTable.ValidateLocation())
            {
                ScreenMap.Add("PrimaryLocation", _LocationMasterTable.sscnlocn);
            } // end if

            _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber);
            _LotNumber = _WorkOrderMasterTable.WALOTN;
        } // end LoadScreenMap

        #endregion

        #region ClearTransaction

        /// <summary>
        /// ClearTransactions clears the data fields to start a new transaction.
        /// </summary>
        private void ClearTransaction()
        {
            _ShortItemNumber = 0; // short item
            _PrimaryItemNumber = ""; // primary item
            _LongItemNumber = ""; // second item number
            _ThirdItemNumber = ""; // third item number
            _LotStatus = ""; // lot status
            _Location = ""; // location
            _LotNumber = ""; // lot number
            _LotSerialExpDate = ""; // lot/serial expiration date
            _TransactionQuantity = 0; // quantity Completed
            _UnitOfMeasure = ""; // completed unit of measure
            _ScrapQuantity = 0; // quantity scrapped
            _ScrapUnitOfMeasure = ""; // scrap uom
            _SerialNumbersScanned = 0;
            _LicensePlateProcessingType = "";
            _LicensePlate = "";
            _FromSerialNumberEntry = false;
            _ToLotGrade = "";
            _ToLotPotency = "";
            ScreenMap.Add("LotNumber", _LotNumber);
        } // end ClearTransaction

        #endregion ClearTransaction

        #region Label Printing

        //*****************************************************************
        // Label Printing
        //*****************************************************************

        #region CollectPrinter

        //*****************************************************************
        // CollectPrinter
        //*****************************************************************

        #region CollectPrinter_EnterEvent

        /// <summary>
        /// Enter event for the CollectPrinter Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectPrinter_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // Validating that the printer exists for the environment in which they are logged.
            _PrinterName = e.UserInput.ToString().Trim();
            if (this.PrintSystem.ValidPrinter(_PrinterName))
            {
                _PrinterAddress = PrintSystem.PrinterAddress;

                if (_FromGenLP)
                {
                    // Print a label for each License Plate created
                    for (int i = 0; i < _GeneratedLicensePlateNumbersTable.Rows.Count; i++)
                    {
                        _LicensePlate = _GeneratedLicensePlateNumbersTable.Rows[i]["Number"].ToString().Trim();
                        PrintBarCode();
                    } // end for
                } // end if
                else
                {
                    PrintBarCode();
                } // end else

                // Determining which screen to go to.  If the user is collect serials, go back
                // to finish collecting serials until all serials have been scanned; otherwise,
                // start the workflow over again by collecting a new work order.
                if (_FromSerialNumberEntry && (_SerialNumbersScanned < _TransactionQuantity))
                {
                    //Back(_CollectSerialNumber);
                } // end if
                else
                {
                    ClearTransaction();
                    //Back(_CollectWorkOrderNumber);
                } // end else
            } // end if
            else
            {
                Error(this.PrintSystem.ErrorMessageVocabulary);
            } // end else
        } // end CollectPrinter_EnterEvent

        #endregion

        #region CollectPrinter_ListEvent

        /// <summary>
        /// List event for the CollectPrinter Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectPrinter_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            //_StartOfOrder = false;
            _DefinedPrintersTable.FetchPrintersByUserWorkflow(UserSession.UserId, WorkflowID);
            _DefinedPrintersTableList.Show();
        } // end CollectPrinter_ListEvent

        #endregion

        #endregion

        #region DefinedPrinters

        //*****************************************************************
        // DefinedPrinters
        //*****************************************************************

        #region DefinedPrinters_EnterEvent

        /// <summary>
        /// Enter event for the DefinedPrinters Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefinedPrinters_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DefinedPrintersTable.UpdateCurrentRow();
            _PrinterName = _DefinedPrintersTable.PrinterName;
            if (this.PrintSystem.ValidPrinter(_PrinterName))
            {
                _PrinterAddress = PrintSystem.PrinterAddress;
                //PrintBarCode();
                Next(_PrintBarCodeLabelsStartOfOrder);
                /*if (_StartOfOrder)
                {
                    Next(_CollectQuantity);
                } // end if
                else
                {
                    if ((_FromSerialNumberEntry) && (_SerialNumbersScanned < _TransactionQuantity))
                    {
                        Back(_CollectSerialNumber);
                    } // end if
                    else
                    {
                        ClearTransaction();
                        Back(_CollectWorkOrderNumber);
                    } // end else
                } // end else*/
            }
            else
                Error(this.PrintSystem.ErrorMessageVocabulary);
        } // end DefinedPrinters_EnterEvent

        #endregion

        #endregion

        #region StartOfOrder Label Printing

        #region PrintBarCodeLabelsStartOfOrder_EnterEvent

        /// <summary>
        /// Enter event for the PrintBarCodeLabelsStartOfOrder Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintBarCodeLabelsStartOfOrder_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if ((e.UserInput.ToString().Trim().Length > 0) && (e.UserInput.ToString().Trim() != "0"))
            {
                try
                {
                    PrintSystem.Copies = e.UserInput.ToInt32();

                    // Ensuring that a negative label quantity is not entered
                    if (PrintSystem.Copies < 0)
                    {
                        throw (new ArgumentException("Negative label qty entered"));
                    } // end if

                    // Checking to make sure that the max number of labels is not exceeded
                    else if (PrintSystem.Copies > WFOptions.GetInt32WFOptionValue("MaxLabels"))
                    {
                        Error("MAXEXCEEDED", e.UserInput.ToString());
                    } // end else if
                    else
                    {
                        Log.Debug($"<***> DefaultPrinter {PrintSystem.DefaultPrinter}");
                        // Trying to locate the Label Format to get the default printer
                        PrintSystem.Reset();
                        PrintSystem.Branch = _BranchPlant;
                        PrintSystem.ItemNumber = _PrimaryItemNumber;
                        PrintSystem.Location = _LocationMasterTable.stbllocn;
                        if (PrintSystem.DefaultPrinter.Trim().Length == 0)
                        {
                            Error("LabelFormat");
                        } // end if
                        else
                        {
                            switch (WFOptions.RetrieveOptionValue("LabelCreation"))
                            {
                                case "1":
                                    Next(CollectLocation);
                                    break;
                                case "2":
                                    Next(CollectCartonSerial);
                                    break;
                                case "3":
                                    Next(CollectCartonSerial);
                                    break;
                            }
                        } // end else
                    } // end else
                } // end try
                catch (RFS_Exception ex)
                {
                    Error(ex.ErrorName);
                } // end catch
                catch
                {
                    Error("QUANTITY", e.UserInput.ToString());
                } // end catch
            } // end if
            else
            {
                //Next(_CollectQuantity);
            } // end else
        } // end PrintBarCodeLabelsStartOfOrder_EnterEvent

        #endregion

        #region CollectPrinterStartOfOrder_EnterEvent

        /// <summary>
        /// Enter event for the CollectPrinterStartOfOrder Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectPrinterStartOfOrder_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PrinterName = e.UserInput.ToString().Trim();
            if (PrintSystem.ValidPrinter(_PrinterName))
            {
                _PrinterAddress = _DefinedPrintersTable.Address;
            }
            /*if (_DefinedPrintersTable.ValidateUserPrinter(UserSession.UserId, _PrinterName))
            {
                _PrinterAddress = _DefinedPrintersTable.Address;
                Log.Debug($"<***> SoO PrinterAddress {_PrinterAddress}");
                //PrintBarCode();
            }*/ // end if
            else
            {
                Error("INVALIDPRINTER");
            } // end else
        } // end CollectPrinterStartOfOrder_EnterEvent

        #endregion

        #region CollectPrinterStartOfOrder_ListEvent

        /// <summary>
        /// List event for the CollectPrinterStartOfOrder Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectPrinterStartOfOrder_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _StartOfOrder = true;
            _DefinedPrintersTable.FetchPrintersByUserWorkflow(UserSession.UserId, WorkflowID);
        } // end CollectPrinterStartOfOrder_ListEvent

        #endregion

        #endregion

        #region PrintBarCodeLabels

        //*****************************************************************
        // PrintBarCodeLabels
        //*****************************************************************

        #region PrintBarCodeLabels_EnterEvent

        /// <summary>
        /// Enter event for the PrintBarCodeLabels Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintBarCodeLabels_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToString().Trim().Length > 0 && e.UserInput.ToString().Trim() != "0")
            {
                try
                {
                    PrintSystem.Copies = e.UserInput.ToInt32();
                    if (PrintSystem.Copies < 0)
                    {
                        throw (new ArgumentException("Negative label qty entered"));
                    } // end if
                    else if (PrintSystem.Copies > WFOptions.GetInt32WFOptionValue("MaxLabels"))
                    {
                        Error("MAXEXCEEDED", e.UserInput.ToString());
                    } // end else if
                    else
                    {
                        PrintSystem.Reset();
                        PrintSystem.Branch = _BranchPlant;
                        PrintSystem.ItemNumber = _PrimaryItemNumber;
                        PrintSystem.Location = _LocationMasterTable.stbllocn;
                        if (PrintSystem.DefaultPrinter.Trim().Length == 0)
                        {
                            Error("LabelFormat");
                        } // end if
                        else
                        {
                            Next(_CollectPrinter, PrintSystem.DefaultPrinter);
                        } // end else
                    } // end else
                } // end try
                catch (RFS_Exception ex)
                {
                    Error(ex.ErrorName);
                } // end catch
                catch
                {
                    Error("QUANTITY", e.UserInput.ToString());
                } // end catch
            } // end if
            else if ((_FromSerialNumberEntry) && (_SerialNumbersScanned < _TransactionQuantity))
            {
                //Back(_CollectSerialNumber);
            } // end else if
            else
            {
                ClearTransaction();
                //Back(_CollectWorkOrderNumber);
            } // end else
        } // end PrintBarCodeLabels_EnterEvent

        #endregion

        #endregion

        #region PrintBarCode

        /// <summary>
        /// PrintBarCode initializes the Printing object with the Printer name, address, etc.
        /// and sets the field data for the dropfile and prints the dropfile.
        /// </summary>
        private void PrintBarCode()
        {
            DateTime PackDate = new DateTime();
            if (_BranchPlant.Trim().ToUpper() == "73W01")
            {
                PackDate = DateTime.Now.AddHours(-2);
            }
            else
            {
                PackDate = DateTime.Now;
            }

            PrintSystem.Reset();
            PrintSystem.DropFilePathOverride = WFOptions.RetrieveOptionValue("DropFilePath");
            PrintSystem.PrinterAddress = _PrinterAddress;
            PrintSystem.PrinterName = _PrinterName;
            PrintSystem.ContainerType = "";
            if ((_LPLicense)
                && (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()))
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LicensePlate", _LicensePlate, false);
            } // end if

            //reset all of the job number label data
            for (int i = 1; i < 33; i++)
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_JobNumber{i}", "", false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_PartNumber{i}", "", false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_PartQty{i}", "", false);
            }

            /*PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_DocumentNumber", _WorkOrderMasterTable.WADOCO.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_DocumentType", _WorkOrderMasterTable.WADCTO, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ThirdItemNumber", _CollectItemNumber.ItemsTable.IMAITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_SecondItemNumber", _CollectItemNumber.ItemsTable.IMLITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ShortItemNumber", _CollectItemNumber.ItemsTable.IMITM.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Description1", _CollectItemNumber.ItemsTable.IMDSC1, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Branch", _CollectBranch.BranchPlantsTable.MCMCU, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotNumber", _LotNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_TransactionQuantity", _TransactionQuantity.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_UnitOfMeasure", _UnitOfMeasure, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LocationMasterTable", _LocationMasterTable.sscnlocn, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_PrimaryItemNumber", _PrimaryItemNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotNumber", _LotNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotSerialExpDate", _LotSerialExpDate, false);

            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SRIPartNumber", JNComp.PartNumber, false);// _CollectItemNumber.ItemsTable.IMLITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PartDesc", JNComp.PartDescription, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity", _TransactionQuantity.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SerialNumber", _NextSerialNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_JobNumber", JNComp.JobNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_WorkOrder", JNComp.WorkOrderNumber, false);
            _SerialTable.GetSerialNumber(_BranchPlant, JNComp.SerialNumber, false);
            */
            if (_SerialTable.LPURCD == "0")
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_CartonType", "R", false);
            }
            else
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_CartonType", "E", false);
            }

            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateDay", PackDate.Day.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateMonth", PackDate.Month.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateYear", PackDate.Year.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PackTime", PackDate.TimeOfDay.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Packer", UserSession.UserId, false);

            PrintSystem.ExtraPrintData.NewRow();
            if (!PrintSystem.Print())
            {
                DisplayMessage(PrintSystem.ErrorMessageVocabulary);
            } // end if
        } // end PrintBarCode

        #endregion

        #region PrintMasterCartonLabel

        private void PrintMasterCartonLabel()
        {
            var partQuery = (from p in _ScannedData.AsEnumerable()
                select p.Field<string>("LongItem")).Distinct();
            Log.Debug($"<***> partQuery {partQuery.Count()}");

            DateTime PackDate = new DateTime();
            if (_BranchPlant.Trim().ToUpper() == "73W01")
            {
                PackDate = DateTime.Now.AddHours(-2);
            }
            else
            {
                PackDate = DateTime.Now;
            }

            foreach (var part in partQuery)
            {
                DataTable labelData = new DataTable();
                var query = from l in _ScannedData.AsEnumerable()
                    where l.Field<string>("LongItem") == part
                    select l;
                labelData = query.CopyToDataTable();
                Log.Debug($"<***>ShortItemNumber {labelData.Rows[0]["ShortNumber"].ToString()}");
                Log.Debug($"<***>LongItemNumber {labelData.Rows[0]["LongItem"].ToString()}");
                _ShortItemNumber = double.Parse(labelData.Rows[0]["ShortNumber"].ToString());
                _LongItemNumber = labelData.Rows[0]["LongItem"].ToString();
                double totalQuantity = 0;
                totalQuantity = (from q in labelData.AsEnumerable()
                    select q.Field<double>("Quantity")).Sum();
                PrintSystem.Reset();
                PrintSystem.DropFilePathOverride = WFOptions.RetrieveOptionValue("DropFilePath");
                PrintSystem.PrinterAddress = _PrinterAddress;
                PrintSystem.PrinterName = _PrinterName;
                PrintSystem.ContainerType = "M";
                if ((_LPLicense)
                    && (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()))
                {
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LicensePlate", _LicensePlate, false);
                } // end if

                int NextNumber;
                string NextSerialNumber;
                _NextNumberTable.GetNextNumber("InventoryTag", out NextNumber, out NextSerialNumber);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SerialNumber", NextSerialNumber, false);
                int serialcount = 1;
                //reset all of the job number label data
                for (int i = 1; i < 129; i++)
                {
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_SerialNumber{i}", "", false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_SerialQty{i}", "", false);
                }

                //_SerialTable.GetSerialNumbers(_BranchPlant, _PalletNumber.ToString());
                Log.Debug($"<***> _ScannedData.Rows.Count = {labelData.Rows.Count}");
                foreach (DataRow dr in labelData.Rows)
                {
                    Log.Debug($"<***>SerialNumber = {dr["Serial"].ToString()}");

                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_SerialNumber{serialcount}", dr["Serial"].ToString(), false);
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_SerialQty{serialcount}", dr["Quantity"].ToString(), false);
                    serialcount++;
                }

                Log.Debug($"<***>ASR_PartNumber {part}");
                Log.Debug($"<***>ASR_Location {_Location} ");
                Log.Debug($"<***>ASR_TotalBoxes {labelData.Rows.Count.ToString()}");
                Log.Debug($"<***>ASR_TotalQuantity {totalQuantity.ToString()}");
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PartNumber", part, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Location", _Location, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_TotalBoxes", labelData.Rows.Count.ToString(), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_TotalQuantity", totalQuantity.ToString(), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateDay", PackDate.Day.ToString(), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateMonth", PackDate.Month.ToString(), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateYear", PackDate.Year.ToString(), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PackTime", PackDate.ToString("HH:mm"), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Packer", UserSession.UserId, false);

                PrintSystem.ExtraPrintData.NewRow();
                if (!PrintSystem.Print())
                {
                    DisplayMessage(PrintSystem.ErrorMessageVocabulary);
                }
                else
                {
                    _PISerialTable.InsertPITag(_BranchPlant, _ShortItemNumber.ToString(), _LongItemNumber, _Location, totalQuantity.ToString(), NextSerialNumber, UserSession.UserId, "MstTag");
                    foreach (DataRow dr in labelData.Rows)
                    {
                        int TagNumber;
                        string NextTagNumber;
                        _NextNumberTable.GetNextNumber("InventoryTag", out TagNumber, out NextTagNumber);
                        _PISerialTable.InsertPITag(_BranchPlant, _ShortItemNumber.ToString(), _LongItemNumber, _Location, dr["Quantity"].ToString(), NextTagNumber, UserSession.UserId, dr["Serial"].ToString(), NextSerialNumber, "MstTag");
                    }
                }
            } // end PrintMasterCartonLabel
        }

        #endregion

        #region ReprintMasterCartonLabel

        private void RePrintMasterCartonLabel(double TagNumber, string PartNumber, string Location)
        {
            TagNumber = TagNumber / 100;
            PrintSystem.Reset();
            PrintSystem.DropFilePathOverride = WFOptions.RetrieveOptionValue("DropFilePath");
            PrintSystem.PrinterAddress = _PrinterAddress;
            PrintSystem.PrinterName = _PrinterName;
            PrintSystem.ContainerType = "M";
            double TotalQty = 0;
            DateTime PackDate = new DateTime();
            if (_BranchPlant.Trim().ToUpper() == "73W01")
            {
                PackDate = DateTime.Now.AddHours(-2);
            }
            else
            {
                PackDate = DateTime.Now;
            }

            int serialcount = 1;
            //reset all of the job number label data
            for (int i = 1; i < 129; i++)
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_SerialNumber{i}", "", false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_SerialQty{i}", "", false);
            }

            foreach (DataRow dr in _PISerialTable.Rows)
            {
                _PISerialTable.CurrentRow = dr;

                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_SerialNumber{serialcount}", _PISerialTable.PIDL02, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData($"ASR_SerialQty{serialcount}", _PISerialTable.PITRQT.ToString(), false);
                TotalQty += _PISerialTable.PITRQT;
                serialcount++;
            }

            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SerialNumber", TagNumber.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PartNumber", PartNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Location", Location, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_TotalBoxes", _PISerialTable.Rows.Count.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_TotalQuantity", TotalQty.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateDay", PackDate.Day.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateMonth", PackDate.Month.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateYear", PackDate.Year.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PackTime", PackDate.ToString("HH:mm"), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Packer", UserSession.UserId, false);

            PrintSystem.ExtraPrintData.NewRow();
            if (!PrintSystem.Print())
            {
                DisplayMessage(PrintSystem.ErrorMessageVocabulary);
            }
        }

        #endregion

        #endregion

        #region SendToERP

        private void setToLocation()
        {
            _WOCompletionTransaction.Location = _Location.Trim();
            if (_LocationMasterTable.ValidateLocation(_CollectBranch.BranchPlantsTable.MCMCU.Trim(), _Location))
            {
                _WOCompletionTransaction.ToLocnPutZone = _LocationMasterTable.LMPZON.Trim();
                _WOCompletionTransaction.ToLocnPickZone = _LocationMasterTable.LMKZON.Trim();
                _WOCompletionTransaction.ToLocnReplZone = _LocationMasterTable.LMZONR.Trim();
                _WOCompletionTransaction.ToLocnDimGroup = _LocationMasterTable.LMSTY1.Trim();
            }
        }

        private void setItem()
        {
            _WOCompletionTransaction.PrimaryItemNumber = _PrimaryItemNumber;
            _WOCompletionTransaction.ShortItemNumber = _ShortItemNumber.ToString();
            _WOCompletionTransaction.SecondItemNumber = _LongItemNumber;
            _WOCompletionTransaction.ThirdItemNumber = _ThirdItemNumber;
            _WOCompletionTransaction.ItemDescription1 = _CollectItemNumber.ItemsTable.IMDSC1; // Needed for WOW
            _WOCompletionTransaction.ItemDescription2 = _CollectItemNumber.ItemsTable.IMDSC1; // Needed for WOW
            _WOCompletionTransaction.ItemDimGroup = _CollectItemNumber.ItemBranchesTable.IBPRP6.Trim();
            _WOCompletionTransaction.ItemLotProcType = _CollectItemNumber.ItemBranchesTable.IBSRCE;
        }
        /// <summary>
        /// SendToErp initializes the ERP transaction fields and sends the transaction
        /// to the ERP system plugin.
        /// </summary>

        #endregion SendToERP

        #region CreateDataSet

        /// <summary>
        /// CreateDataSet creates a data set for the GeneratedLicensePlateNumbers with
        /// a table for the GeneratedLicensePlateNumbers that has one field (Number).
        /// </summary>
        private void CreateDataSet()
        {
            _GeneratedLicensePlateNumbersDataSet = new DataSet("GeneratedLicensePlateNumbersDataSet");
            _GeneratedLicensePlateNumbersTable = new DataTable("GeneratedLicensePlateNumbersTable");
            _GeneratedLicensePlateNumbersTable.Columns.Add("Number");
            _GeneratedLicensePlateNumbersDataSet.Tables.Add(_GeneratedLicensePlateNumbersTable);
        }

        public StoneridgeMasterPI()
            : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._PrintBarCodeLabelsStartOfOrder = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectBranch = new ICS.Workflow.Base.JDE.Components.BranchPlantScreen(this);
            this._BranchPlantList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectPrinterStartOfOrder = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._DefinedPrintersTableList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectItemNumber = new ICS.Workflow.Base.JDE.Components.ItemScreen(this);
            this._CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._PrintBarCodeLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectPalletNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectCartonSerial = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.RemoveCartonSerial = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ConfirmSerialRemove = new ICS.Workflow.Base.JDE.Components.YesNoResponseScreen(this);
            this.CollectLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ConfirmDelete = new ICS.Workflow.Base.JDE.Components.YesNoResponseScreen(this);
            // 
            // _PrintBarCodeLabelsStartOfOrder
            // 
            this._PrintBarCodeLabelsStartOfOrder.ScreenName = "PrintBarCodeLabelsStartOfOrder";
            this._PrintBarCodeLabelsStartOfOrder.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabelsStartOfOrder_EnterEvent);
            this._PrintBarCodeLabelsStartOfOrder.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._PrintBarCodeLabelsStartOfOrder_LoadEvent);
            this._PrintBarCodeLabelsStartOfOrder.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this._PrintBarCodeLabelsStartOfOrder_F2Method);
            // 
            // _CollectBranch
            // 
            this._CollectBranch.ListScreen = this._BranchPlantList;
            this._CollectBranch.ListScreenControl = this._BranchPlantList;
            this._CollectBranch.NextScreen = this._CollectPrinterStartOfOrder;
            this._CollectBranch.ScreenName = "CollectBranch";
            this._CollectBranch.WarehouseConstantsTable = null;
            this._CollectBranch.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectBranch_EnterEvent);
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
            // 
            // _CollectPrinterStartOfOrder
            // 
            this._CollectPrinterStartOfOrder.ListScreen = this._DefinedPrintersTableList;
            this._CollectPrinterStartOfOrder.ListScreenControl = this._DefinedPrintersTableList;
            this._CollectPrinterStartOfOrder.NextScreen = this._PrintBarCodeLabelsStartOfOrder;
            this._CollectPrinterStartOfOrder.ScreenName = "CollectPrinterStartOfOrder";
            this._CollectPrinterStartOfOrder.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinterStartOfOrder_EnterEvent);
            this._CollectPrinterStartOfOrder.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinterStartOfOrder_ListEvent);
            this._CollectPrinterStartOfOrder.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this._CollectPrinterStartOfOrder_F2Method);
            // 
            // _DefinedPrintersTableList
            // 
            this._DefinedPrintersTableList.ListScreenParent = this._CollectPrinterStartOfOrder;
            this._DefinedPrintersTableList.ParentScreen = this._CollectPrinterStartOfOrder;
            this._DefinedPrintersTableList.ScreenName = "FRFS_Printers";
            this._DefinedPrintersTableList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.DefinedPrinters_EnterEvent);
            this._DefinedPrintersTableList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._DefinedPrintersTableList_LoadEvent);
            // 
            // _CollectItemNumber
            // 
            this._CollectItemNumber.BranchScreen = this._CollectBranch;
            this._CollectItemNumber.ItemBranchesTable = null;
            this._CollectItemNumber.ItemsTable = null;
            this._CollectItemNumber.ScreenName = "CollectItemNumber";
            // 
            // _CollectPrinter
            // 
            this._CollectPrinter.ScreenName = "CollectPrinter";
            this._CollectPrinter.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_EnterEvent);
            this._CollectPrinter.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_ListEvent);
            // 
            // _PrintBarCodeLabels
            // 
            this._PrintBarCodeLabels.ScreenName = "PrintBarCodeLabels";
            this._PrintBarCodeLabels.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabels_EnterEvent);
            // 
            // CollectPalletNumber
            // 
            this.CollectPalletNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectPalletNumber_ValidateMethod);
            this.CollectPalletNumber.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this.CollectPalletNumber_F2Method);
            // 
            // CollectCartonSerial
            // 
            this.CollectCartonSerial.ScreenName = "CollectCartonSerial";
            this.CollectCartonSerial.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectCartonSerial_EnterValidationEvent);
            this.CollectCartonSerial.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this.CollectCartonSerial_F2Method);
            this.CollectCartonSerial.F6Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this.CollectCartonSerial_F6Method);
            // 
            // RemoveCartonSerial
            // 
            this.RemoveCartonSerial.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.RemoveCartonSerial_EnterValidationEvent);
            // 
            // ConfirmSerialRemove
            // 
            this.ConfirmSerialRemove.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmSerialRemove_EnterValidationEvent);
            // 
            // CollectLocation
            // 
            this.CollectLocation.ScreenName = "CollectLocation";
            this.CollectLocation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_EnterValidationEvent);
            // 
            // ConfirmDelete
            // 
            this.ConfirmDelete.ScreenName = "ConfirmDelete";
            this.ConfirmDelete.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmDelete_EnterValidationEvent);
            // 
            // StoneridgeMasterPI
            // 
            this.StartScreen = this._CollectBranch;
            this.WorkflowID = "StoneridgeMasterPI";
        }

        private void _LicensePlateMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _ItemBalanceList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _EmptyLocationsList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _EmptyLocationsList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _DefinedPrintersTableList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _LocationMasterList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _LocationMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _UnitOfMeasureConversionList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _UserDefinedCodesList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _BranchPlantList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CollectBranch.BranchPlantsTable.FetchBranchPlants();
        }

        private void _WorkOrderMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _WorkOrderMasterTable.ShowWorkOrders(_AllowedWorkOrderStatusCodes, _BranchPlant, WFOptions.RetrieveOptionValue("WODocType"));
        }

        private void _CollectSerialNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            DefaultRFSGeneratedLotNumber();
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

        private void _CollectLot_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            DefaultRFSGeneratedLotNumber();
        }

        private void _CollectLotSerialExpDate_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("DateFormat", WFOptions.RetrieveOptionValue("DateFormat").ToString());
        }

        // end CreateDataSet

        #endregion

        #endregion

        private void _PrintBarCodeLabelsStartOfOrder_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WFOptions.RetrieveOptionValue("DeafaultCartonLabelPerCarton");
        }

        private bool CollectPalletNumber_ValidateMethod(string PalletNumber)
        {
            if (double.TryParse(PalletNumber, out _PalletNumber))
            {
                if (_PalletSerialTable.GetMasterCarton(_BranchPlant, PalletNumber))
                {
                    ScreenMap.Add("Serial", "");
                    ScreenMap.Add("PalletNumber", PalletNumber);
                    Next(CollectCartonSerial);
                }
                else
                {
                    throw new RFS_Exception("SERIALNOTAVAILBLE");
                }
            }
            else
            {
                throw new RFS_Exception("Not Numeric");
            }

            return true;
        }

        private void CollectCartonSerial_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _SerialNumber = e.UserInput.ToString();
            switch (WFOptions.RetrieveOptionValue("LabelCreation"))
            {
                case "1":
                    if (_SerialTable.GetSerialNumber(_BranchPlant, _SerialNumber, false))
                    {
                        if (!_PISerialTable.GetTagNumber(_BranchPlant, double.Parse(_SerialNumber), true))
                        {
                            if (!_PISerialTable.GetTagNumber(_BranchPlant, _SerialNumber, true))
                            {
                                var findSerial = from a in _ScannedData.AsEnumerable()
                                    where a.Field<string>("Serial") == _SerialNumber
                                    select a;
                                if (findSerial.Count() > 0)
                                {
                                    ScreenMap.Add("Serial", "");
                                    throw new RFS_Exception("ALREADYSCANNED");
                                }
                                else
                                {
                                    DataRow drItem = _ScannedData.NewRow();
                                    drItem["Serial"] = _SerialTable.LPLPNU;
                                    drItem["LongItem"] = _SerialTable.LPLITM;
                                    drItem["ShortNumber"] = _SerialTable.LPITM;
                                    drItem["Quantity"] = _SerialTable.LPTQTY;
                                    drItem["Location"] = _Location;
                                    drItem["UOM"] = _SerialTable.LPUOM;
                                    _ScannedData.Rows.Add(drItem);
                                    ScreenMap.Add("Serial", _SerialNumber + " Added");
                                }

                                /*if (_SerialTable.LPPLPNU.Trim() == _PalletNumber.ToString())
                                {
                                    ScreenMap.Add("Serial", "");
                                    throw new RFS_Exception("ALREADYSCANNED");
                                }
                                if (_SerialTable.LPPLPNU.Trim() != "")
                                {
                                    ScreenMap.Add("Serial", "");
                                    throw new RFS_Exception("ONMASTERPALLET");

                                }
                                if (_SerialTable.AddtoMasterPallet(_BranchPlant, _SerialNumber, _PalletNumber.ToString()))
                                {
                                    ScreenMap.Add("Serial", _SerialNumber + " Added");
                                    Next(CollectCartonSerial);
                                }
                                else
                                {
                                    ScreenMap.Add("Serial", "");
                                    throw new RFS_Exception("SERIALNOTADDED");
                                }*/
                                Next(CollectCartonSerial);
                            }
                            else
                            {
                                ScreenMap.Add("Serial", "");
                                throw new RFS_Exception("ALREADYSCANNED");
                            }
                        }
                        else
                        {
                            ScreenMap.Add("Serial", "");
                            throw new RFS_Exception("ALREADYSCANNED");
                        }
                    }
                    else
                    {
                        ScreenMap.Add("Serial", "");
                        throw new RFS_Exception("NOCTNFOUND");
                    }

                    break;
                case "2":
                    if (_PISerialTable.GetTagNumber(_BranchPlant, _SerialNumber, true))
                    {
                        double TagNumber = _PISerialTable.PIGNUM2;
                        string PartNumber = _PISerialTable.PILITM;
                        string Location = _PISerialTable.PILOCN;

                        if (_PISerialTable.ReprintMasterTag(_BranchPlant, TagNumber))
                        {
                            RePrintMasterCartonLabel(TagNumber, PartNumber, Location);
                            ScreenMap.Add("Serial", "");
                            Next(CollectCartonSerial);
                        }
                    }
                    else
                    {
                        ScreenMap.Add("Serial", "");
                        throw new RFS_Exception("TAGNOTFOUND");
                    }

                    break;
                case "3":
                    if (_PISerialTable.GetTagNumber(_BranchPlant, double.Parse(_SerialNumber), true))
                    {
                        if (_PISerialTable.GetTagNumber(_BranchPlant, double.Parse(_SerialNumber)))
                        {
                            _TagNumber = _PISerialTable.PITGG;
                            ScreenMap.Add("_TagNumber", _TagNumber.ToString());
                            Next(ConfirmDelete);
                        }
                        else
                        {
                            throw new RFS_Exception("ALREADYSCANNED");
                        }
                    }
                    else
                    {
                        ScreenMap.Add("Serial", "");
                        throw new RFS_Exception("TAGNOTFOUND");
                    }

                    break;
            }
        }

        #region F2 Methods

        private bool CollectCartonSerial_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("Serial", "");
            Next(RemoveCartonSerial);
            return true;
        }

        private bool CollectPalletNumber_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("Serial", "");
            Next(RemoveCartonSerial);
            return true;
        }

        private bool _CollectPrinterStartOfOrder_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("Serial", "");
            Next(RemoveCartonSerial);
            return true;
        }

        private bool _PrintBarCodeLabelsStartOfOrder_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("Serial", "");
            Next(RemoveCartonSerial);
            return true;
        }

        #endregion

        #region RemoveCartonSerial

        private void RemoveCartonSerial_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _SerialNumber = e.UserInput.ToString();
            if (_SerialTable.GetSerialNumber(_BranchPlant, _SerialNumber, false))
            {
                if (_SerialTable.LPPLPNU.Trim() != "")
                {
                    ScreenMap.Add("_PalletNumber", _SerialTable.LPPLPNU);
                    ScreenMap.Add("_SerialNumber", _SerialNumber);
                    Next(ConfirmSerialRemove);
                }
                else
                {
                    ScreenMap.Add("Serial", "");
                    throw new RFS_Exception("NOMASTERFOUND");
                }
            }
            else
            {
                ScreenMap.Add("Serial", "");
                throw new RFS_Exception("NOCTNFOUND");
            }
        }

        #endregion

        private void ConfirmSerialRemove_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateRemoveCarton(e.UserInput.ToBool());
        }

        private void validateRemoveCarton(bool confirm)
        {
            if (confirm)
            {
                if (_SerialTable.RemovefromMasterPallet(_BranchPlant, _SerialNumber, _SerialTable.LPPLPNU))
                {
                    ScreenMap.Add("Serial", _SerialNumber + " Removed");
                    Next(RemoveCartonSerial);
                }
                else
                {
                    throw new RFS_Exception("NOTREMOVED");
                }
            }
            else if (!confirm)
            {
                RestartLoop(RemoveCartonSerial);
            }
            else
            {
                throw new RFS_Exception("InvalidEntry");
            }
        }

        private bool CollectCartonSerial_F6Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            PrintMasterCartonLabel();
            ScreenMap.Add("Serial", "");
            _ScannedData.Clear();
            RestartLoop(CollectLocation);
            return true;
        }

        private void CollectLocation_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _Location = e.UserInput.ToString();
            if (_Location.Trim() != "")
            {
                _LocationMasterTable.ResetAll();
                _LocationMasterTable.MCU = _BranchPlant;
                _LocationMasterTable.inLOCN = _Location;
                if (_LocationMasterTable.ValidateLocation())
                {
                    ScreenMap.Add("_Location", _Location);
                    Next(CollectCartonSerial);
                    //PrintPILabels();
                }
                else
                {
                    throw new RFS_Exception("INVALIDLOCATION");
                }
            }
            else
            {
                throw new RFS_Exception("BLANKSNOTALLOWED");
            }
        }

        private void ConfirmDelete_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ValidateRemoveTag(e.UserInput.ToBool());
        }

        private void ValidateRemoveTag(bool Confirm)
        {
            if (Confirm)
            {
                if (_PISerialTable.ReprintMasterTag(_BranchPlant, _TagNumber))
                {
                    RePrintMasterCartonLabel(_TagNumber, _PISerialTable.PILITM, _PISerialTable.PILOCN);
                }

                int NextNumber;
                string NextSerialNumber;

                _NextNumberTable.GetNextNumber("PIHistory", out NextNumber, out NextSerialNumber);
                Log.Debug($"<***>Next Number {NextNumber} {NextSerialNumber}");
                if (_PISerialTable.DeleteMasterTag(_BranchPlant, _TagNumber, NextNumber, UserSession.UserId, _TagNumber * 100))
                {
                    RestartLoop(CollectCartonSerial);
                }
                else
                {
                    throw new RFS_Exception("NOTREMOVED");
                }
            }
            else
            {
                RestartLoop(CollectCartonSerial);
            }
        }
    } // end StoneridgeMasterPI Class
} // end ICS.Workflow.Base.JDE.ShopFloor Namespace