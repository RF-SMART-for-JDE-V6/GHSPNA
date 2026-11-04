using System;
using System.Data;
using System.Collections;
using ICS.Database.TableObjects.JDE;
using ICS.Database.TableObjects.RFS;
using ICS.Display;
using ICS.ERPI.JDE;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using System.Collections.Generic;
using System.Linq;
using ICS.Stoneridge.Wireless.JDE.ERPI;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.ShopFloor
{
    /// <summary>
    /// Screen Control Class For: JDEWorkOrderCompletion Workflow
    /// </summary>
    public partial class StoneridgeWorkOrderCompletionSerial
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
        //private F4102 _ItemBranchTable; // JDE item/Branch
        private F41021 _ItemLocationTable; // jde item/lot/location

        private F4108 _LotMasterTable; // jde lot master

        private F46011 _ItemUOMProfileTable;

        private F46L10 _LicensePlateMasterTable;

        private F4801 _WorkOrderMasterTable; // jde work order master

        private RFS_DataManipulator _DataManipulatorTable;

        private RFS_PrinterDefinition _DefinedPrintersTable; // RFS Defined Printers

        private RFS_PrintXref _PrinterCrossReferenceTable;

        private RFS_NextNumber _NextNumberTable;

        private DataSet _GeneratedLicensePlateNumbersDataSet;

        private DataTable _GeneratedLicensePlateNumbersTable;

        private F46L001 _LicensePlateNumberingConstantsTable;

        //new tables

        private Database.TableObjects.WorkOrderCompletion WO;

        private Database.TableObjects.F3112 _WorkOrderRoutingTable;

        private Database.TableObjects.F5846001 _SerialTable;

        private Database.TableObjects.F3109 _LineItemMasterTable;

        private Database.TableObjects.F3111 _PartsListTable;

        private F4101 _IssueItemMasterTable;

        private F4102 _IssueItemBranchTable;

        private F4100 _IssueLocationMasterTable;

        private F41002 _UnitOfMeasureConversionTable1;

        private F0101 _AddresBookTable;

        private Database.TableObjects.F00095 _JDEReservationTable;

        #endregion

        #region Collect/Work data

        private string _PrinterName;

        private string _ListLookupType; // denotes whether user is looking up "Location" or "Lot" Info

        private string _BranchPlant; // work order Branch

        private double _WorkOrderNumber; // work order number

        private string _DocumentType; // work order type

        private string _ShiftCode; // shift code

        private string _PrimaryItemNumber; // primary item number

        private string _Location; // issue location

        private string _ScreenFormattedLocation;

        private string _LotNumber; // lot/serial number

        private string _LotSerialExpDate; // lot/serial expiration date

        private string _ToLotGrade;

        private string _ToLotPotency;

        private double _TransactionQuantity; // Used to hold the scrapped quantity and UOM converted

        private string _UnitOfMeasure; // to the primary

        private double _CompQtyUnconverted; // Used to hold the completed quantity and UOM entered

        private string _CompUOMUnconverted; // by the user before being converted to primary

        private double _ScrapQuantity; // Used to hold the scrapped quantity and UOM converted

        private string _ScrapUnitOfMeasure; // to the primary

        private double _ScrapQtyUnconverted; // Used to hold the scrapped quantity and UOM entered

        private string _ScrapUOMUnconverted; // by the user before being converted to primary

        private double _ShortItemNumber; // short item number

        private string _LongItemNumber; // second item number

        private string _ThirdItemNumber; // third item number

        private string _LotStatus; // lot status

        private string _PrinterAddress;

        private string _UnitOfMeasureListType;

        private string _IsLocationControlActive; // Y/N flag to denote if whse is using location control

        private int _SerialNumbersScanned; // s/n scanned

        private string _LicensePlate;

        private string _LicensePlateProcessingType = "";

        private string _ShiftCodesSearchType;

        private bool _FromSerialNumberEntry = false;

        private string _LastDateEntered = " ";

        private string _LocationSeparator;

        private string[] _AllowedLotHoldCodes = null; // allowed lot hold codes

        private string[] _AllowedWorkOrderStatusCodes = null;

        private bool _StartOfOrder = false;

        private bool _FromGenLP = false;

        private string _ERPUpdateVersion;

        private bool _LPLicense = false;

        private string[] _CaseUOM;

        private int _ContainerType;

        private string _ExpRet;

        private string _NextSerialNumber;

        private string _DefaultSerialNumber;

        //issues variables
        private bool _AllowOverIssue;

        private string[] _IssueTypesAllowed = null;

        private double _PartsListQuantityInPrimary;

        private List<PartsListRecord> _ItemsToIssue = new List<PartsListRecord>();

        #endregion

        #region ERPI

        private WOTracking _WOTrackingTransaction = new WOTracking();

        private WOIssues _WOIssuesTransaction = new WOIssues();

        private RFS_TxCallUBE _CallUbe = new RFS_TxCallUBE();

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Workflow : JDEWorkOrderCompletion
        /// </summary>
        /// <param name="userSession">Contains UserSession info for controling screen display and DAL.</param>
        public StoneridgeWorkOrderCompletionSerial(UserSession userSession) : base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

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
            //_ItemBranchTable = new F4102(UserSession.DataAccess, "");
            _ItemLocationTable = new F41021(UserSession.DataAccess, "");
            _LotMasterTable = new F4108(UserSession.DataAccess, "");
            _ItemUOMProfileTable = new F46011(UserSession.DataAccess, "");
            _WorkOrderMasterTable = new F4801(UserSession.DataAccess, "");
            _DataManipulatorTable = new RFS_DataManipulator(UserSession.DataAccess, "");
            _DefinedPrintersTable = new RFS_PrinterDefinition(UserSession.DataAccess, "");
            _PrinterCrossReferenceTable = new RFS_PrintXref(UserSession.DataAccess, "");
            _NextNumberTable = new RFS_NextNumber(UserSession.DataAccess, "");
            _LicensePlateNumberingConstantsTable = new F46L001(UserSession.DataAccess, "");
            _tblUOMConversion2 = new F41003(UserSession.DataAccess, "");
            _WorkOrderRoutingTable = new Database.TableObjects.F3112(UserSession.DataAccess, "");
            _SerialTable = new Database.TableObjects.F5846001(UserSession.DataAccess, "");
            _LineItemMasterTable = new Database.TableObjects.F3109(UserSession.DataAccess, "");
            _PartsListTable = new Database.TableObjects.F3111(UserSession.DataAccess, "");
            _IssueItemMasterTable = new F4101(UserSession.DataAccess, "Issue");
            _IssueItemBranchTable = new F4102(UserSession.DataAccess, "Issue");
            _IssueLocationMasterTable = new F4100(UserSession.DataAccess, "Issue");
            _UnitOfMeasureConversionTable1 = new F41002(UserSession.DataAccess, "");
            _AddresBookTable = new F0101(UserSession.DataAccess, "");
            _JDEReservationTable = new Database.TableObjects.F00095(UserSession.DataAccess, "");

            #endregion

            ClearTransaction();
            // Setting up workflow options
            char[] Delimiter = { ' ' };
            string sTemp = WFOptions.RetrieveOptionValue("AllowedWorkOrderStatus");
            _AllowedWorkOrderStatusCodes = sTemp.Split(Delimiter);
            sTemp = WFOptions.RetrieveOptionValue("AllowedLotHoldCodes");
            _AllowedLotHoldCodes = sTemp.Split(Delimiter);
            sTemp = WFOptions.RetrieveOptionValue("CaseUOM");
            _CaseUOM = sTemp.Split(Delimiter);

            //issues workflow options
            sTemp = WFOptions.RetrieveOptionValue("IssueTypeAllowed");
            _IssueTypesAllowed = sTemp.Split(Delimiter);
            _AllowOverIssue = WFOptions.GetYesNoWFOptionValue("AllowOverIssues");

            // License plate tables
            if (UserSession.IsModuleLicensed("JLP"))
            {
                _LPLicense = true;
                _LicensePlateMasterTable = new F46L10(UserSession.DataAccess, "");
            }

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
                }
                else
                {
                    _IsLocationControlActive = "N";
                }

                _LicensePlateNumberingConstantsTable.ResetAll();
                _LicensePlateNumberingConstantsTable.BranchPlant = _BranchPlant;
            }
            else
            {
                Error("MCU", _BranchPlant);
            }
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

        #region CollectEmployee

        private bool CollectEmployee_ValidateMethod(string sInput)
        {
            if (!_AddresBookTable.RetrieveAddress(sInput, "E"))
            {
                throw new RFS_Exception("EmployeeNumber");
            }

            return true;
        }

        private void EmployeeList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _AddresBookTable.ShowEmployees();
            EmployeeList.Table = _AddresBookTable;
            EmployeeList.DataColumnForValidation = "ABAN8";
        }

        #endregion

        #region CollectShiftCode

        //*****************************************************************
        // CollectShiftCode
        //*****************************************************************

        #region CollectShiftCode_EnterEvent

        /// <summary>
        /// Enter event for the CollectShiftCode Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectShiftCode_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_UserDefinedCodesTable.ValidateUDC("00  ", "SH", e.UserInput.ToString().ToUpper()))
            {
                _ShiftCode = _UserDefinedCodesTable.DRKY;
                UserSession.ScreenHandler.ScreenMap.Add("ShiftCodeDescription", _UserDefinedCodesTable.DRDL01);
            }
            else
            {
                Error("SHIFTCODE", e.UserInput.ToString());
            }
        } // end CollectShiftCode_EnterEvent

        #endregion

        #region CollectShiftCode_ListEvent

        /// <summary>
        /// ListEvent for the CollectShiftCode Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectShiftCode_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ShiftCodesSearchType = "Shift";
            _UserDefinedCodesTable.ResetAll();
            _UserDefinedCodesTable.DRSY = "00  ";
            _UserDefinedCodesTable.DRRT = "SH";
            _UserDefinedCodesTable.Fetch();
        } // end CollectShiftCode_ListEvent

        #endregion

        #region UserDefinedCodesList

        //*****************************************************************
        // UserDefinedCodesList
        //*****************************************************************

        #region UserDefinedCodesList_EnterEvent

        /// <summary>
        /// Enter event for the UserDefinedCodesList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserDefinedCodesList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UserDefinedCodesTable.UpdateCurrentRow();
            _FromSerialNumberEntry = false;
            switch (_ShiftCodesSearchType)
            {
                case "Shift":
                    _ShiftCode = _UserDefinedCodesTable.DRKY;
                    ScreenMap.Add("ShiftCode", _ShiftCode);
                    Next(_CollectWorkOrderNumber);
                    break;
                case "LotStatus":
                    _LotStatus = _UserDefinedCodesTable.DRKY;
                    CheckLotProcessType();
                    break;
                case "Grade":
                    _ToLotGrade = _UserDefinedCodesTable.DRKY;
                    ProcessTransaction();
                    break;
            }
        } // end UserDefinedCodesList_EnterEvent

        #endregion

        #endregion

        #endregion

        #region CollectWorkOrderNumber

        //*****************************************************************
        // CollectWorkOrderNumber
        //*****************************************************************

        #region CollectWorkOrderNumber_EnterEvent

        /// <summary>
        /// Enter event for the CollectWorkOrderNumber Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectWorkOrderNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _WorkOrderNumber = e.UserInput.ToDouble();

            if (_JDEReservationTable.DoJDERecordLocksExist("F4801", _WorkOrderNumber.ToString()))
            {
                throw new RFS_Exception("RECORDRESERVED");
            }

            // Checking to see if the WorkOrderNumber entered is blank
            if (_WorkOrderNumber.ToString().Length == 0)
            {
                Error("BLANKSNOTALLOWED");
            }

            // BD015237 - Validate the work order number entered against the work order master
            // taking into account the Allowed Status Codes
            else if (_WorkOrderMasterTable.IsWorkOrderValid(_WorkOrderNumber, _AllowedWorkOrderStatusCodes))
            {
                // Validating that the Work Order Number entered exists for the specified branch
                if (_WorkOrderMasterTable.WAMMCU.Trim() == _BranchPlant.Trim())
                {
                    _DocumentType = _WorkOrderMasterTable.WADCTO;
                    _WorkOrderNumber = _WorkOrderMasterTable.WADOCO;
                    LoadScreenMap();
                }
                else
                {
                    Error("BranchMismatch");
                }
            }
            else
            {
                Error("INVALIDWONUMBER", e.UserInput.ToString());
            }
        } // end CollectWorkOrderNumber_EnterEvent

        #endregion

        #region WorkOrderMasterList

        //*****************************************************************
        // WorkOrderMasterList
        //*****************************************************************

        #region WorkOrderMasterList_EnterEvent

        /// <summary>
        /// Enter event for the WorkOrderMasterList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkOrderMasterList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _WorkOrderMasterTable.UpdateCurrentRow();
            _WorkOrderNumber = _WorkOrderMasterTable.WADOCO;
            _DocumentType = _WorkOrderMasterTable.WADCTO;
            LoadScreenMap();
            Next(_VerifyWorkOrderInformation);
        } // end WorkOrderMasterList_EnterEvent

        #endregion

        #endregion

        #endregion

        #region VerifyWorkOrderInformation

        //*****************************************************************
        // VerifyWorkOrderInformation
        //*****************************************************************

        #region VerifyWorkOrderInformation_EnterEvent

        /// <summary>
        /// Enter event for the VerifyWorkOrderInformation Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VerifyWorkOrderInformation_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // This screen is a display for verification only
            // User presses ENTER to continue or hits the BACK button
            if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
            {
                ScreenHandler.DefaultText = "1"; // completed quantity is not yet known
            }
        } // end VerifyWorkOrderInformation_EnterEvent

        #endregion

        #endregion

        #region CollectQuantity

        //*****************************************************************
        // CollectQuantity
        //*****************************************************************

        /// <summary>
        /// CollectQuantity: Some Comments
        /// </summary>
        private void CollectQuantity_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            double ConversionFactor = 0;
            if (WFOptions.RetrieveOptionValue("PalletUOM").Trim() != "")
            {
                foreach (string sUOM in _CaseUOM)
                {
                    if (sUOM != "")
                    {
                        ConversionFactor =
                            this._tblUOMConversion.ConvertToPrimaryUOM(_BranchPlant, _ShortItemNumber, WFOptions.RetrieveOptionValue("PalletUOM").Trim(),
                                sUOM);
                        Log.WriteLine(5, "Conversion Factor after call to ConvertToPrimaryUOM = " + ConversionFactor.ToString());
                        Log.WriteLine(5, "Branch/Plant = " + _BranchPlant);
                        Log.WriteLine(5, "Short Item Number = " + _ShortItemNumber.ToString());
                        Log.WriteLine(5, "From U/M = " + WFOptions.RetrieveOptionValue("PalletUOM").Trim());
                        Log.WriteLine(5, "To U/M = " + sUOM.ToString());
                        if (ConversionFactor == 0)
                        {
                            ConversionFactor =
                                _tblUOMConversion2.ConvertToPrimaryUOM(WFOptions.RetrieveOptionValue("PalletUOM").Trim(), sUOM.ToString());
                        }

                        if (ConversionFactor > 0)
                            break;
                    }
                }
            }

            if (ConversionFactor > 0)
                ScreenHandler.DefaultText = ConversionFactor.ToString();
            else
                ScreenHandler.DefaultText = "";
        }

        #region CollectQuantity_EnterEvent

        /// <summary>
        /// Enter event for the CollectQuantity Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectQuantity_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ScrapQuantity = 0; // reset in case user backs up from scrap screen
            _ScrapUnitOfMeasure = "";

            try
            {
                // Call calculator function
                string CalculatedQuantity = _ItemLocationTable.CalcQty(e.UserInput.ToString().ToUpper());

                // If quantity entered cannot be calculated, throw an error
                if (CalculatedQuantity == null)
                {
                    Error("INVALIDQUANTITY", e.UserInput.ToString());
                }

                // Checking to see if the quantity is negative and that negatives are allowed
                else if ((Convert.ToDecimal(CalculatedQuantity) >= 0) ||
                         WFOptions.GetYesNoWFOptionValue("AllowNegativeCompletion"))
                {
                    _CompQtyUnconverted = _TransactionQuantity = Convert.ToDouble(CalculatedQuantity);
                    ScreenMap.Add("QuantityCompleted", CalculatedQuantity);
                    ScreenHandler.DefaultText =
                        _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("DefaultUOM"));

                    _CollectQuantity.Validate(CalculatedQuantity);
                }
                else
                {
                    Error("NEGATIVEENTRY", e.UserInput.ToString());
                }
            }
            catch
            {
                Error("INVALIDQUANTITY");
            } // end catch
        } // end CollectQuantity_EnterEvent

        #endregion

        #endregion

        #region CollectUnitOfMeasure

        //*****************************************************************
        // CollectUnitOfMeasure
        //*****************************************************************

        #region CollectUnitOfMeasure_EnterEvent

        /// <summary>
        /// Enter event for the CollectUnitOfMeasure Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectUnitOfMeasure_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            string sDefaultUOM = _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("DefaultUOM"));

            _CompUOMUnconverted = e.UserInput.ToString().ToUpper(); // for licensing plating

            // BD015244 - Checking that the defaulted UOM is overrideable and that the UOM entered
            // matches the default.
            if (WFOptions.GetYesNoWFOptionValue("AllowUOMOverride") ||
                (sDefaultUOM == _CompUOMUnconverted))
            {
                ProcessUOMEnter();
            }
            else
            {
                Error("UOMOverride", sDefaultUOM);
            }
        } // end CollectUnitOfMeasure_EnterEvent

        #endregion

        #region ProcessUOMEnter

        /// <summary>
        /// ProcessUOMEnter is used to process the UOM entered by the user.  It performs
        /// UOM conversions if the UOM entered does not match the Item's Primary.  It also
        /// checks tolerance and exceeding the work order ordered quantity.
        /// </summary>
        private void ProcessUOMEnter()
        {
            double ConversionFactor = 0;

            ConversionFactor = _tblUOMConversion.ConvertUnitOfMeasure(_BranchPlant,
                _ShortItemNumber,
                _CompUOMUnconverted,
                _CollectItemNumber.ItemsTable.IMUOM1);
            if (ConversionFactor == 0)
            {
                Error("UNITOFMEASURE", _CompUOMUnconverted);
            }
            else
            {
                // Convert quantity and UOM to primary for JDE update and stored originals for later use
                Log.WriteLine(5, "Transaction Quantity before applying conversion factor = " + _TransactionQuantity.ToString());
                _TransactionQuantity = _CompQtyUnconverted * ConversionFactor;
                Log.WriteLine(5, "Transaction Quantity after applying conversion factor = " + _TransactionQuantity.ToString());

                ScreenMap.Add("UOMComp", _CompUOMUnconverted); // Map the user specified UOM
                _UnitOfMeasure = _CollectItemNumber.ItemsTable.IMUOM1;

                if (WFOptions.RetrieveOptionValue("PalletUOM").Trim().Length > 0)
                {
                    // CM032935:
                    // Now that we have the quantity in the primary UOM, make sure user has
                    //  not picked more than the max quantity on a single pallet.
                    Log.WriteLine(5, "Pallet UOM from workflow options = \"" + WFOptions.RetrieveOptionValue("PalletUOM").Trim() + "\".");
                    double dMaxPalletQty = _tblUOMConversion.ConvertToPrimaryUOM(_BranchPlant,
                        _ShortItemNumber,
                        WFOptions.RetrieveOptionValue("PalletUOM").Trim(), 1);
                    if (dMaxPalletQty == 0)
                    {
                        Log.WriteLine(5, "Could not find conversion to primary for UOM specified in workflow option \"PalletUOM\".");
                        Error("UNITOFMEASURE", _UnitOfMeasure);
                        return;
                    }

                    Log.WriteLine(5, "Maximum pallet quantity in primary UOM = " + dMaxPalletQty.ToString() + ".");
                    if (dMaxPalletQty < _TransactionQuantity)
                    {
                        Log.WriteLine(5, "Error in ProcessUOMEnter(): Quantity exceeds maximum pallet quantity.");
                        Error("OverQty", _UnitOfMeasure);
                        return;
                    }
                }

                // BD015262 - If the order is over completed and the workflow is set to check, issue error
                if (OverCompleted(_CompUOMUnconverted, ConversionFactor))
                {
                    Error("OVERCOMPLETE", _UnitOfMeasure);
                }

                // Checking Tolerance
                else if (ExceedsTolerance(_CompUOMUnconverted, ConversionFactor))
                {
                    _UnitOfMeasureListType = "Comp";
                    Next(_DisplayToleranceWarning);
                }
                else
                {
                    Next(GetNextScreenForCompUOM());
                }

                _CollectUnitOfMeasure.Validate(_UnitOfMeasure);
            }
        } // end ProcessUOMEnter

        #endregion

        #region CollectUnitOfMeasure_ListEvent

        /// <summary>
        /// List event for the CollectUnitOfMeasure Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectUnitOfMeasure_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureListType = "Comp";
            _tblUOMConversion.ResetAll();
            _tblUOMConversion.UMITM = _WorkOrderMasterTable.WAITM;
            _tblUOMConversion.Fetch();
        } // end CollectUnitOfMeasure_ListEvent

        #endregion

        #region UnitOfMeasureList_EnterEvent

        /// <summary>
        /// Enter event for the UnitOfMeasureList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnitOfMeasureList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            string sDefaultUOM = _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("DefaultUOM"));

            // user has selected a record from the list
            _tblUOMConversion.UpdateCurrentRow();

            // BD015244 - Checking that the defaulted UOM is overrideable and that the UOM entered
            // matches the default.
            if (WFOptions.GetYesNoWFOptionValue("AllowUOMOverride") ||
                (sDefaultUOM == _tblUOMConversion.UMUM))
            {
                switch (_UnitOfMeasureListType.Trim())
                {
                    case "Comp":
                        _CompUOMUnconverted = _tblUOMConversion.UMUM; // for licensing plating
                        ProcessUOMEnter();
                        break;
                    case "Scrap":
                        //_ScrapUnitOfMeasure = _tblUOMConversion.UMUM;
                        _ScrapUOMUnconverted = _tblUOMConversion.UMUM;
                        ProcessScrapUOMEnter();
                        break;
                    case "Dual":
                        _CollectDualUnitOfMeasure.Validate(_tblUOMConversion.UMUM);
                        break;
                } // end switch
            }
            else
            {
                Error("UOMOverride", sDefaultUOM);
            }
        } // end UnitOfMeasureList_EnterEvent

        #endregion

        #region GetNextScreenForCompUOM

        /// <summary>
        /// Returns the screen to which the user should be directed after a valid completed UOM
        /// is specified.  Valid is defined as not creating Over Complete or outside of the Tolerance.
        /// If the workflow option is not set to check over completion or the user accecpts the
        /// tolerance, then this is also considered valid.
        /// </summary>
        /// <returns>Screen to which the user should be directed after a valid completed UOM.
        /// The return value should most likely be used in a call to Next.</returns>
        private RFSScreenComponent GetNextScreenForCompUOM()
        {
            /* since we are not sending s/n with scrap, this is no longer required 
             * 
           // for serialized items, at this point we need to capture each S/N
           if ((_ItemBranchTable.IBSRCE.Trim().Length > 0) && 
              (Convert.ToInt16(_ItemBranchTable.IBSRCE) > Convert.ToInt16(LotSerialDesignations.LotRequired)))
           {
              if (_TransactionQuantity == 0)
              {
                 return (_CollectQuantityScrapped);
              } 
              else
              {
                 _ScrapQuantity = 0;
                 _ScrapUnitOfMeasure = "";

                 // If License Plating is Enabled and a license plate item is fetched, go to the
                 // CollectLicensePlate Screen; otherwise go to the CollectLocation screen.
                 if ((UserSession.IsModuleLicensed("JLP")) &&  
                     (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()))
                 {
                    return (_CollectLicensePlate);
                 } 
                 else
                 {
                    return (_CollectLocation);
                 } 
              } 
           } 
           else
           { */
            //return (_CollectQuantityScrapped); 
            return (_CollectDualQuantity);
            //} 
        } // end

        #endregion

        #endregion

        #region CollectDualQuantity

        #region CollectDualQuantity_LoadEvent

        private void CollectDualQuantity_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _CollectDualQuantity.Reset();
            _CollectDualUnitOfMeasure.Reset();

            if (!_CollectItemNumber.IsDualQtyRequired)
            {
                _CollectDualQuantity.CancelLoad = true;
                _CollectDualUnitOfMeasure.CancelLoad = true;
                ScreenHandler.ScreenStack.Pop();
                Next(_CollectQuantityScrapped);
            }
            else
            {
                _CollectDualQuantity.CancelLoad = false;
                _CollectDualUnitOfMeasure.CancelLoad = false;
            }
        }

        #endregion

        #endregion

        #region CollectDualUnitOfMeasure

        #region CollectUnitOfMeasure_ListEvent

        /// <summary>
        /// List event for the CollectUnitOfMeasure Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectDualUnitOfMeasure_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureListType = "Dual";
            _tblUOMConversion.ResetAll();
            _tblUOMConversion.UMITM = _WorkOrderMasterTable.WAITM;
            _tblUOMConversion.Fetch();
        } // end CollectUnitOfMeasure_ListEvent

        #endregion

        #endregion

        #region CollectQuantityScrapped

        //*****************************************************************
        // CollectQuantityScrapped
        //*****************************************************************

        #region CollectQuantityScrapped_EnterEvent

        /// <summary>
        /// Enter event for the CollectQuantityScrapped Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectQuantityScrapped_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                // Call calculator function
                string CalculatedQuantity = _ItemLocationTable.CalcQty(e.UserInput.ToString().ToUpper());

                // If quantity entered cannot be calculated, throw an error
                if (CalculatedQuantity == null)
                {
                    Error("INVALIDQUANTITY", e.UserInput.ToString());
                }

                // Checking to see if the quantity is negative and that negatives are allowed
                else if (Convert.ToDecimal(CalculatedQuantity) < 0)
                {
                    Error("NEGATIVEENTRY", e.UserInput.ToString());
                }
                else
                {
                    // Call calculator function
                    _ScrapQtyUnconverted = _ScrapQuantity = Convert.ToDouble(CalculatedQuantity);
                    ScreenMap.Add("QuantityScrapped", CalculatedQuantity);

                    if (_ScrapQuantity == 0)
                    {
                        // If both the completed quantity and the scrapped quantity are blank,
                        // issue error and return to the completed quantity screen
                        if (_TransactionQuantity == 0)
                        {
                            Error("ZEROQUANTITIES");
                        }
                        else if ((_LPLicense)
                                 && (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive())
                                 && (_ItemUOMProfileTable.ALicenPlateItem(_BranchPlant, _ShortItemNumber, _CompUOMUnconverted)))
                        {
                            Next(_CollectLicensePlate);
                        }
                        else
                        {
                            Next(CollectContainerType);
                        }
                    }
                    else
                    {
                        Next(_CollectScrapUnitOfMeasure,
                            _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("DefaultUOM")));
                    }
                }
            }
            catch
            {
                Error("INVALIDQUANTITY");
            } // end catch
        } // end CollectQuantityScrapped_EnterEvent

        #endregion

        #endregion

        #region CollectScrapUnitOfMeasure

        //*****************************************************************
        // CollectScrapUnitOfMeasure
        //*****************************************************************

        #region CollectScrapUnitOfMeasure_EnterEvent

        /// <summary>
        /// Enter event for the CollectScrapUnitOfMeasure Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectScrapUnitOfMeasure_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            string sDefaultUOM = _CollectItemNumber.ItemsTable.DefaultUOM(WFOptions.RetrieveOptionValue("DefaultUOM"));

            _ScrapUOMUnconverted = e.UserInput.ToString().ToUpper();

            // BD015244 - Checking that the defaulted UOM is overrideable and that the UOM entered
            // matches the default.
            if (WFOptions.GetYesNoWFOptionValue("AllowUOMOverride") ||
                (sDefaultUOM == _ScrapUOMUnconverted))
            {
                ProcessScrapUOMEnter();
            }
            else
            {
                Error("UOMOverride", sDefaultUOM);
            }
        } // end CollectScrapUnitOfMeasure_EnterEvent

        #endregion

        #region ProcessScrapUOMEnter

        /// <summary>
        /// ProcessScrapUOMEnter is used to process the scrap UOM entered by the user.  It
        /// performs UOM conversions if the UOM entered does not match the Item's Primary.
        /// </summary>
        private void ProcessScrapUOMEnter()
        {
            double ConversionFactor = 0;

            ConversionFactor = _tblUOMConversion.ConvertUnitOfMeasure(_BranchPlant,
                _ShortItemNumber,
                _ScrapUOMUnconverted,
                _CollectItemNumber.ItemsTable.IMUOM1);
            if (ConversionFactor == 0)
            {
                Error("UNITOFMEASURE", _ScrapUOMUnconverted);
            }
            else
            {
                // Convert quantity and UOM to primary for JDE update and stored originals for later use
                Log.WriteLine(5, "Scrapped Quantity before applying conversion factor = " + _ScrapQuantity.ToString());
                _ScrapQuantity = _ScrapQtyUnconverted * ConversionFactor;
                Log.WriteLine(5, "Scrapped Quantity after applying conversion factor = " + _ScrapQuantity.ToString());
                _ScrapUnitOfMeasure = _CollectItemNumber.ItemsTable.IMUOM1;
                ScreenMap.Add("UOMScrap", _ScrapUnitOfMeasure); // Map the UOM it becomes the primary

                // If the order is over completed and the workflow is set to check, issue error
                if (OverCompleted(_ScrapUOMUnconverted, ConversionFactor))
                {
                    Error("OVERCOMPLETE", _ScrapUnitOfMeasure);
                }

                // Checking Tolerance
                // else if (ExceedsTolerance(_ScrapUOMUnconverted, ConversionFactor))
                // {
                //    _UnitOfMeasureListType = "Scrap";
                //    Next(_DisplayToleranceWarning);
                //   } 

                // Checking to see if the user has completed anything for the Work Order.  If they have
                // not, then there is no need to collect a license plate.
                else if (_TransactionQuantity != 0 &&
                         (_LPLicense) &&
                         (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()) &&
                         _ItemUOMProfileTable.ALicenPlateItem(_BranchPlant, _ShortItemNumber, _CompUOMUnconverted))
                {
                    Next(_CollectLicensePlate);
                }
                else
                {
                    Next(CollectContainerType);
                }
            }
        } // end ProcessScrapUOMEnter

        #endregion

        #region CollectScrapUnitOfMeasure_ListEvent

        /// <summary>
        /// List event for the CollectScrapUnitOfMeasure Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectScrapUnitOfMeasure_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureListType = "Scrap";
            _tblUOMConversion.ResetAll();
            _tblUOMConversion.UMITM = _WorkOrderMasterTable.WAITM;
            _tblUOMConversion.Fetch();
        } // end CollectScrapUnitOfMeasure_ListEvent

        #endregion

        #endregion

        #region CollectContainerType

        private bool CollectContainerType_ValidateMethod(string ContainerTypeInput)
        {
            switch (ContainerTypeInput)
            {
                case "R":
                    _ContainerType = 0;
                    ScreenMap.Add("ContainerType", "Returnable");
                    break;
                case "E":
                    _ContainerType = 1;
                    ScreenMap.Add("ContainerType", "Expendable");
                    break;
                default:
                    throw new RFS_Exception("CONTAINER");
            }

            switch (_ContainerType)
            {
                case 0:
                    _ExpRet = "E";
                    break;
                case 1:
                    _ExpRet = "R";
                    break;
                default:
                    _ExpRet = "";
                    break;
            }

            return true;
        }

        #endregion

        #region CollectLocation

        //*****************************************************************
        // CollectLocation
        //*****************************************************************

        #region CollectLocation_EnterEvent

        /// <summary>
        /// Enter event for the CollectLocation Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLocation_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _Location = e.UserInput.ToString().ToUpper();
            LocationEnterKey();
        } // end CollectLocation_EnterEvent

        #endregion

        #region LocationEnterKey

        /// <summary>
        /// LocationEnterKey handles the Location entered by the user by means of validation.
        /// </summary>
        private void LocationEnterKey()
        {
            // Checking to see if the Location specified by the user is empty is blanks are not allowed.
            if (WFOptions.GetYesNoWFOptionValue("AllowBlankLocation") || (_Location.Trim().Length != 0))
            {
                _LocationMasterTable.ResetAll();
                _LocationMasterTable.MCU = _CollectBranch.BranchPlantsTable.MCMCU;
                _LocationMasterTable.inLOCN = _Location;
                if (_IsLocationControlActive == "Y")
                {
                    if (_LocationMasterTable.ValidateLocation())
                    {
                        _Location = _LocationMasterTable.stbllocn;
                        _ScreenFormattedLocation = _LocationMasterTable.sscnlocn;
                        ScreenMap.Add("LOCATION", _ScreenFormattedLocation);
                        // will need to retrieve item branch again since it may have been cleared on a lookup
                        _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber);
                        // IF IBOT1Y = 'Y', prompt for potency
                        if (_CollectItemNumber.ItemBranchesTable.IBOT1Y.ToString().Trim().ToUpper() == "Y")
                            Next(_CollectPotency);
                        else
                        {
                            // otherwise, if IBOT2Y = 'Y', prompt for grade
                            if (_CollectItemNumber.ItemBranchesTable.IBOT2Y.ToString().Trim().ToUpper() == "Y")
                                Next(_CollectGrade);
                            else
                                ProcessTransaction();
                        }
                    }
                    else
                    {
                        Error("LOCATION");
                    }
                }
                else
                {
                    _ScreenFormattedLocation = _Location;
                    ScreenMap.Add("LOCATION", _ScreenFormattedLocation);
                    // will need to retrieve item branch again since it may have been cleared on a lookup
                    _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber);
                    // IF IBOT1Y = 'Y', prompt for potency
                    if (_CollectItemNumber.ItemBranchesTable.IBOT1Y.ToString().Trim().ToUpper() == "Y")
                        Next(_CollectPotency);
                    else
                    {
                        // otherwise, if IBOT2Y = 'Y', prompt for grade
                        if (_CollectItemNumber.ItemBranchesTable.IBOT2Y.ToString().Trim().ToUpper() == "Y")
                            Next(_CollectGrade);
                        else
                            ProcessTransaction();
                    }
                }
            }
            else
            {
                Error("BLANKSNOTALLOWED");
            }
        } // end LocationEnterKey

        #endregion

        #region CollectLocation_KeyEnter

        /// <summary>
        /// Key event for the CollectLocation Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLocation_KeyEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (e.KeyPressed)
            {
                case ActionKey.F2: // user hits F2 - display empty locations
                    if (e.KeyPressed == ActionKey.F2)
                    {
                        _LocationMasterTable.ShowEmptyLocations(_BranchPlant);
                        _EmptyLocationsList.Show();
                    }

                    break;
                default:
                    Error("JDEWrongKey");
                    break;
            } // end switch
        } // end CollectLocation_KeyEvent

        #endregion

        #region EmptyLocationList_EnterEvent

        private void EmptyLocationList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // User has selected a record from the list
            _LocationMasterTable.UpdateCurrentRow();
            _LocationMasterTable.inLOCN = _LocationMasterTable.LMLOCN;
            _LocationMasterTable.MCU = _CollectBranch.BranchPlantsTable.MCMCU;
            _LocationMasterTable.ResetAll();
            _LocationMasterTable.ValidateLocation();
            _Location = _LocationMasterTable.stbllocn;
            _ScreenFormattedLocation = _LocationMasterTable.sscnlocn;
            ScreenMap.Add("LOCATION", _ScreenFormattedLocation);
            // will need to retrieve item branch again since it may have been cleared on a lookup
            _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber);
            // IF IBOT1Y = 'Y', prompt for potency
            if (_CollectItemNumber.ItemBranchesTable.IBOT1Y.ToString().Trim().ToUpper() == "Y")
                Next(_CollectPotency);
            else
            {
                // otherwise, if IBOT2Y = 'Y', prompt for grade
                if (_CollectItemNumber.ItemBranchesTable.IBOT2Y.ToString().Trim().ToUpper() == "Y")
                    Next(_CollectGrade);
                else
                    ProcessTransaction();
            }
        } // end EmptyLocationList_EnterEvent

        #endregion

        #region CollectLocation_ListEvent

        /// <summary>
        /// List event for the CollectLocation Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLocation_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ListLookupType = "Location";
            _ItemLocationTable.RetrieveItemLocationRecords(_BranchPlant,
                _ShortItemNumber,
                WFOptions.GetYesNoWFOptionValue("ShowZeroOnHand") ? "Y" : "N");
        } // end CollectLocation_ListEvent

        #endregion

        #endregion

        #region CollectGrade

        #region CollectGrade_EnterEvent

        /// <summary>
        /// Some Comments
        /// </summary>
        private void CollectGrade_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_UserDefinedCodesTable.ValidateUDC("40  ", "LG", e.UserInput.ToString().ToUpper()))
            {
                _ToLotGrade = _UserDefinedCodesTable.DRKY;
                ProcessTransaction();
            }
            else
            {
                Error("Grade", e.UserInput.ToString());
            }
        }

        #endregion

        #region CollectGrade_ListEvent

        /// <summary>
        /// ListEvent for the CollectGrade Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectGrade_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ShiftCodesSearchType = "Grade";
            _UserDefinedCodesTable.ResetAll();
            _UserDefinedCodesTable.DRSY = "40  ";
            _UserDefinedCodesTable.DRRT = "LG";
            _UserDefinedCodesTable.Fetch();
        } // end CollectGrade_ListEvent

        #endregion

        private void CollectGrade_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(_CollectLocation);
        }

        #endregion

        #region CollectPotency

        #region CollectPotency_EnterEvent

        /// <summary>
        /// Some Comments
        /// </summary>
        private void CollectPotency_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ToLotPotency = e.UserInput.ToString().ToUpper();
            // otherwise, if IBOT2Y = 'Y', prompt for grade
            if (_CollectItemNumber.ItemBranchesTable.IBOT2Y.ToString().Trim().ToUpper() == "Y")
                Next(_CollectGrade);
            else
                ProcessTransaction();
        }

        #endregion

        private void CollectPotency_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(_CollectLocation);
        }

        #endregion

        #region ProcessTransaction

        /// <summary>
        /// Directs the user to the collect lot or serial screens based on the process type.
        /// If the process type is equal to a system generated serial number, then the
        /// transaction is sent to the ERP.  An item with no process type triggers the
        /// transaction to be sent to the ERP as well.
        /// </summary>
        private void ProcessTransaction()
        {
            if (_CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber))
            {
                CheckLotProcessType();
            }
            else
            {
                Error("ITEMBRANCH");
            }
        } // end ProcessTransaction

        #endregion

        #region CollectLicensePlate

        #region CollectLicensePlate_EnterEvent

        /// <summary>
        /// Enter event for the CollectLicensePlate Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLicensePlate_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlate = e.UserInput.ToString().ToUpper();
            LicensePlateEnter();
        } // end CollectLicensePlate_EnterEvent

        #endregion

        #region LicensePlateEnter

        /// <summary>
        /// LicensePlateEnter handles the validation of the License Plate entered
        /// ensuring that it is not a location.  A License Plate should not contain
        /// delimiters like a location would.
        /// </summary>
        private void LicensePlateEnter()
        {
            // If License Plate is blank, collect Location
            if (_LicensePlate.Trim().Length == 0)
            {
                _LicensePlate = "";
            }

            // Check to see if we are scanning a license plate (no delimiters)
            else if (_LicensePlate.IndexOf(_LocationSeparator) == -1)
            {
                //Validate that the work order item/lot is valid on the entered license plate
                //or that the license plate is empty/does not exist
                using (F46L11 tempTbl = new F46L11(UserSession.DataAccess, "TempForValidateLPItemToAdd"))
                {
                    if (!tempTbl.ValidateLPItemToAdd(_LicensePlate, _ShortItemNumber, _LotNumber))
                    {
                        if (!tempTbl.ValidateLPItemToAdd(_LicensePlate, _ShortItemNumber)) Error("ItemNotOnLP");
                        else Error("LotNotOnLP");

                        return;
                    }
                }

                // If a license plate, validate that it exists
                _LicensePlateMasterTable.LMMCU = _BranchPlant;
                _LicensePlateMasterTable.LMLPNU = _LicensePlate;
                if (_LicensePlateMasterTable.Fetch())
                {
                    _LicensePlateProcessingType = "E"; // existing

                    // If the license plate is found, then use that location
                    _Location = _LicensePlateMasterTable.LMLOCN;
                    LocationEnterKey();
                }
                else
                {
                    Next(_ConfirmNewLicensePlate);
                    return;
                }
            }
            else
            {
                _Location = _LicensePlate;
                _LicensePlate = "";
            }
        } // end LicensePlateEnter

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
            _LicensePlateProcessingType = "N"; // New
            Next(_CollectLocation);
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

        #region CollectLicensePlate_LoadEvent

        private void CollectLicensePlate_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (!_ItemUOMProfileTable.ALicenPlateItem(_BranchPlant, _ShortItemNumber))
            {
                _CollectLicensePlate.CancelLoad = true;
                _LicensePlate = "";
                Next(_CollectLocation);
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
        } // end CollectLicensePlate_KeyEvent

        #endregion

        #region CollectLicensePlate_ListEvent

        /// <summary>
        /// List event for the CollectLicensePlate Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLicensePlate_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_LicensePlateMasterTable == null)
                _LicensePlateMasterTable = new F46L10(UserSession.DataAccess, "");

            _LicensePlateMasterTable.ResetAll();
            _LicensePlateMasterTable.LMMCU = _BranchPlant;
            _LicensePlateMasterTable.Fetch();
        } // end CollectLicensePlate_ListEvent

        #endregion

        #region LicensePlateMasterList

        //*****************************************************************
        // LicensePlateMasterList
        //*****************************************************************

        #region LicensePlateMasterList_EnterEvent

        /// <summary>
        /// Enter event for the LicensePlateMasterList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LicensePlateMasterList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlateMasterTable.UpdateCurrentRow();
            _LicensePlate = _LicensePlateMasterTable.LMLPNU;
            LicensePlateEnter();
        } // end LicensePlateMasterList_EnterEvent

        #endregion

        #endregion

        #endregion

        #region CheckLotProcessType

        /// <summary>
        /// Used to encapsulate the Lot Process check to determine if the item is lot or serial
        /// controlled.  If it is lot controlled, go to CollectLot.  If it is serial controlled,
        /// go to CollectSerials where the determination will be made to collect serials or
        /// send the transaction to the ERP system to generate the serial number.  If the item is
        /// neither lot or serial controlled, send the transaction to the ERP system.
        /// </summary>
        private void CheckLotProcessType()
        {
            // Check to see if this is a lot/serial controlled item
            if (_CollectItemNumber.ItemBranchesTable.IBSRCE.Trim().Length > 0)
            {
                // Check to see if this is a lot controlled item, and if so, display the lot
                // verification screen
                if ((Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) >= Convert.ToInt16(LotSerialDesignations.LotOptional)) &&
                    (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) <= Convert.ToInt16(LotSerialDesignations.LotRequired)))
                {
                    if (WFOptions.GetYesNoWFOptionValue("DefaultWorkOrderToLot"))
                    {
                        ScreenHandler.DefaultText = _WorkOrderNumber.ToString();
                    }

                    Next(_CollectLot);
                }

                // Serialized Logic
                else if (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) >= Convert.ToInt16(LotSerialDesignations.SerialOptional))
                {
                    CollectSerials();
                }
            }
            else
            {
                _LotNumber = " ";

                SendToERP();

                if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                {
                    // BD015238 - Set Label quantity to zero if the transaction quantity is negative
                    ScreenHandler.DefaultText = (_CompQtyUnconverted < 0) ? "0" : Math.Ceiling(_CompQtyUnconverted).ToString();
                }

                Next(_PrintBarCodeLabels);
            }
        } // end CheckLotProcessType

        #endregion

        #region CollectLot

        //*****************************************************************
        // CollectLot
        //*****************************************************************

        #region CollectLot_EnterEvent

        /// <summary>
        /// Enter event for the CollectLot Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLot_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotNumber = e.UserInput.ToString().ToUpper();
            ProcessLotNumberEnter();
        } // end CollectLot_EnterEvent

        #endregion

        #region ProcessLotNumberEnter

        /// <summary>
        /// ProcessLotNumberEnter is used to validate that a lot was specified if the
        /// process type (IBSRCE) is equal to three (Assigned Manually).  If the lot
        /// is not found in the F4108, then an Expiration Date must be retrieved,
        /// directs the user to CollectLotSerialExpDate screen.
        /// </summary>
        private void ProcessLotNumberEnter()
        {
            _FromSerialNumberEntry = false;
            ScreenMap.Add("LotNumber", _LotNumber);

            // If a lot is required (process type = 3) and no lot was specified, display
            // an error to the user
            if ((_CollectItemNumber.ItemBranchesTable.IBSRCE.Trim().Length > 0) &&
                (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) == 3) && (_LotNumber.Trim().Length == 0))
            {
                Error("LotRequired");

                return;
            }

            // If the Lot number is not found in the F4108, but a lot was specified and lots are not
            // optional, send the user to the CollectLotSerialExpDate screen
            _ShortItemNumber = _CollectItemNumber.ItemsTable.IMITM;
            if (_LotMasterTable.RetrieveLotMaster(_BranchPlant, _ShortItemNumber, _LotNumber))
            {
                if (_ItemLocationTable.IsLotOnHold(_BranchPlant, _ShortItemNumber, _Location, _LotNumber,
                        _AllowedLotHoldCodes))
                {
                    Error("HELDLOT");

                    return;
                }
            }
            else
            {
                if (_LotNumber.Trim().Length > 0)
                {
                    Next(_CollectLotSerialExpDate);

                    return;
                }

                if ((_CollectItemNumber.ItemBranchesTable.IBSRCE.Trim().Length > 0) &&
                    (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) > 0))
                {
                    Next(_CollectLotSerialExpDate);

                    return;
                }
            }

            SendToERP();

            if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
            {
                // BD015238 - Set Label quantity to zero if the transaction quantity is negative
                ScreenHandler.DefaultText = (_CompQtyUnconverted < 0) ? "0" : Math.Ceiling(_CompQtyUnconverted).ToString();
            }

            Next(_PrintBarCodeLabels);
        } // end ProcessLotNumberEnter

        #endregion

        #region CollectLot_ListEvent

        /// <summary>
        /// List event for the CollectLot Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLot_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ListLookupType = "Lot";
            _ItemLocationTable.RetrieveItemSerialRecords(_BranchPlant,
                _CollectItemNumber.ItemsTable.IMITM,
                _Location,
                WFOptions.GetYesNoWFOptionValue("ShowZeroOnHand") ? "Y" : "N",
                _AllowedLotHoldCodes);
        } // end CollectLot_ListEvent

        #endregion

        #endregion

        #region CollectLotSerialExpDate

        /// <summary>
        /// Enter event for the LotSerialExpDate Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotSerialExpDate_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotSerialExpDate = e.UserInput.ToString();
            if (!DateTimeUtils.ValidateAndFormat(ref _LotSerialExpDate,
                    WFOptions.RetrieveOptionValue("DateFormat")))
            {
                Error("EXPDATE", e.UserInput.ToString());
                return;
            }

            // if this is a lot process type of 5 or 6 (system assigned serial numbers
            // update all using this date
            switch (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE))
            {
                case 5: // system assigned
                case 6: // system assigned
                    double qtytosend = _TransactionQuantity;

                    for (_SerialNumbersScanned = 0; _SerialNumbersScanned < qtytosend; _SerialNumbersScanned++)
                    {
                        // Convert quantity to primary for JDE update
                        _UnitOfMeasure = _CollectItemNumber.ItemsTable.IMUOM1;
                        _TransactionQuantity = 1;
                        SendToERP();
                        _LotNumber = "";
                        _LotSerialExpDate = "";
                    } 

                    ScreenMap.Add("SERIALNUMBERSCAN", _SerialNumbersScanned.ToString());
                    Next(_PrintBarCodeLabels);
                    return;
            }

            if ((_CollectItemNumber.ItemBranchesTable.IBSRCE.Trim().Length > 0) &&
                (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) > Convert.ToInt16(LotSerialDesignations.LotRequired)))
            {
                _FromSerialNumberEntry = true;
                SerialNumberEditUpdate();
            }
            else
            {
                SendToERP();

                if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                {
                    // BD015238 - Set Label quantity to zero if the transaction quantity is negative
                    ScreenHandler.DefaultText = (_CompQtyUnconverted < 0) ? "0" : Math.Ceiling(_CompQtyUnconverted).ToString();
                }

                Next(_PrintBarCodeLabels);
            }
        } // end CollectLotSerialExpDate_EnterEvent

        #endregion

        #region CollectSerialNumber

        //*****************************************************************
        // CollectSerialNumber
        //*****************************************************************

        #region CollectSerialNumber_EnterEvent

        /// <summary>
        /// Enter event for the CollectSerialNumber Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectSerialNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotNumber = e.UserInput.ToString();
            ScreenMap.Add("LotNumber", _LotNumber);

            if(!WFOptions.GetYesNoWFOptionValue("AllowSerialOverride") && !string.IsNullOrWhiteSpace(_DefaultSerialNumber))
			{
				if (!_DefaultSerialNumber.Trim().Equals(_LotNumber))
				{
                    throw new RFS_Exception("Override not allowed!");
				}
			}

            // If Serial Number does not exist, collect the expiration date
            if (_ItemLocationTable.ValidateItemLot(_BranchPlant, _ShortItemNumber, _LotNumber) == false)
            {
                _ItemLocationTable.LIPQOH = 0;
                Next(_CollectLotSerialExpDate, _LastDateEntered);
            }

            // If the Serial Number exists, and has quantity on hand, display an error message
            else if (_ItemLocationTable.LIPQOH > 0)
            {
                Error("SNEXISTS", e.UserInput.ToString());
            }
            else
            {
                /* TODO - Change to user array of allowed hold codes
                if ((opts["AllowHeldAdjustments"].ToString() == "N") && (ItemLocation.LILOTS.ToString().Trim().Length > 0))
                   Error("HELDLOT");
                else
                   bContinue = true;
                   */
                SerialNumberEditUpdate();
            }
        } // end CollectSerialNumber_EnterEvent

        #endregion

        #region SerialNumberEditUpdate

        /// <summary>
        /// SerialNumberEditUpdate validates that the serial number is not expired
        /// if expired serials are not allowed; otherwise the serial edit line is
        /// created and sent to the ERP system.
        /// </summary>
        private void SerialNumberEditUpdate()
        {
            if (_LotMasterTable.RetrieveLotMaster(_BranchPlant, _ShortItemNumber, _LotNumber) &&
                (DateTimeUtils.JDEJulian(_LotMasterTable.IOMMEJ) < System.DateTime.Today) &&
                !WFOptions.GetYesNoWFOptionValue("AllowExpiredLotAdj"))
            {
                Error("EXPIREDLOT", _LotNumber);
            }
            else
            {
                _SerialNumbersScanned += 1;
                ScreenMap.Add("SERIALNUMBERSCAN", _SerialNumbersScanned.ToString());
                _FromSerialNumberEntry = true;

                //first look for any parts list records to be issued
                if (_PartsListTable.ShowPartsList(_WorkOrderNumber.ToString(),
                        _DocumentType,
                        _IssueTypesAllowed,
                        _AllowOverIssue,
                        WFOptions.RetrieveOptionValue("OrderBOMBy")))
                {
                    _ItemsToIssue.Clear();

                    for (int i = 0; i < _PartsListTable.Rows.Count; i++)
                    {
                        _PartsListTable.CurrentRow = _PartsListTable.Rows[i];

                        _ItemsToIssue.Add(new PartsListRecord()
                        {
                            UniqueId = _PartsListTable.WMUKID,
                            ShortItemNumber = _PartsListTable.WMCPIT
                        });
                    }

                    if (WFOptions.GetYesNoWFOptionValue("SerialScanMode"))
                    {
                        Next(CollectIssueLocation);
                    }
                    else
                    {
                        Next(CollectIssueItemNumber);
                    }

                    return;
                }
                else
                {
                    SendToERP();

                    if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                    {
                        ScreenHandler.DefaultText = "1";
                    }

                    Next(_PrintBarCodeLabels);
                }
            }
        } // end SerialNumberEditUpdate

        #endregion

        #region CollectSerials

        /// <summary>
        /// Used to encapsulate the logic used to collect serial numbers.  If the process type is
        /// system assigned (5 or 6), then a transaction is sent to the ERP with a quantity of 1
        /// for the total _TransactionQuantity.
        /// </summary>
        private void CollectSerials()
        {
            switch (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE))
            {
                case 5: // system assigned
                case 6: // system assigned
                    Next(_CollectLotSerialExpDate);
                    break;
                case 4: // s/n optional
                case 7: // capture each S/N
                    ScreenMap.Add("SERIALNUMBERSCAN", "0");
                    ScreenMap.Add("QUANTITY", _TransactionQuantity.ToString());
                    Next(_CollectSerialNumber);
                    break;
            } // end switch
        } // end CollectSerials

        #endregion

        #endregion

        #region ItemLocationList

        //*****************************************************************
        // ItemLocationList
        //*****************************************************************

        #region ItemLocationList_EnterEvent

        /// <summary>
        /// Enter event for the ItemLocationList Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemLocationList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemLocationTable.UpdateCurrentRow();
            if (_ListLookupType == "Location")
            {
                // user has selected a record from the list
                _LocationMasterTable.ResetAll();
                _LocationMasterTable.inLOCN = _ItemLocationTable.LILOCN;
                _LocationMasterTable.MCU = _CollectBranch.BranchPlantsTable.MCMCU;
                _LocationMasterTable.ValidateLocation();
                _Location = _LocationMasterTable.stbllocn;
                _ScreenFormattedLocation = _LocationMasterTable.sscnlocn;
                ScreenMap.Add("LOCATION", _ScreenFormattedLocation);
                // will need to retrieve item branch again since it may have been cleared on a lookup
                _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber);
                // IF IBOT1Y = 'Y', prompt for potency
                if (_CollectItemNumber.ItemBranchesTable.IBOT1Y.ToString().Trim().ToUpper() == "Y")
                    Next(_CollectPotency);
                else
                {
                    // otherwise, if IBOT2Y = 'Y', prompt for grade
                    if (_CollectItemNumber.ItemBranchesTable.IBOT2Y.ToString().Trim().ToUpper() == "Y")
                        Next(_CollectGrade);
                    else
                        ProcessTransaction();
                }
            }
            else
            {
                _LotNumber = _ItemLocationTable.LILOTN;
                ProcessLotNumberEnter();
            }
        } // end ItemLocationList_EnterEvent

        #endregion

        #endregion

        #region OverCompleted

        /// <summary>
        /// OverCompleted checks to see if the OverComplete workflow option is set to 'Y', which
        /// indicates that the work order should be checked for over completion.  Over completion
        /// is determined if the scrap plus transaction (completed) quantity is greater
        /// than the difference of the work order's WAUORG, WASOQS, and WASOCN respectively
        /// </summary>
        /// <returns>Returns true if work order has been over completed and the OverComplete
        /// workflow option is set to 'Y'; otherwise, false</returns>
        private bool OverCompleted(string sUOM, double dConvFactor)
        {
            double dTotalTxDocoQty;

            // If the UOM passed matches the document's UOM, do not fetch the UOM conversion as it
            // was already passed in to the method
            if (sUOM != _WorkOrderMasterTable.WAUOM)
            {
                // Converting the total transactional quantity to the document UOM by getting the
                // conversion from document to primary and dividing the result to go back to doc
                // because I figured it may be easier to find a conversion to primary than to doc.
                dConvFactor = _tblUOMConversion.ConvertUnitOfMeasure(_BranchPlant,
                    _ShortItemNumber,
                    _WorkOrderMasterTable.WAUOM,
                    _CollectItemNumber.ItemsTable.IMUOM1);
                if (dConvFactor == 0)
                {
                    // Allow the user to proceed despite not being able to correctly test the quantity
                    // entered by the user for the order.  This may need changing to throw an exception.
                    if (WFOptions.GetYesNoWFOptionValue("AllowOverComplete"))
                    {
                        _ERPUpdateVersion = WFOptions.RetrieveOptionValue("ERPCompleteUpdateVersion").Trim();

                        return (false);
                    }
                    else
                    {
                        throw (new RFS_Exception("Unable to convert the WAUOM to the item's primary\n" +
                                                 "in order to correctly check for over completion.",
                            "INVOVERCOMPCONV"));
                    }
                }
            }

            // Converting the Tx Qty, which includes the completed and scrapped quantity in the
            // primary UOM, to the Doco UOM for over-completion check
            dTotalTxDocoQty = (_TransactionQuantity + _ScrapQuantity) / dConvFactor;

            // If this is a partial completion, then use the partial completion WFO version
            _ERPUpdateVersion =
                WFOptions.RetrieveOptionValue((dTotalTxDocoQty < (_WorkOrderMasterTable.WAUORG -
                                                                  _WorkOrderMasterTable.WASOQS -
                                                                  _WorkOrderMasterTable.WASOCN))
                    ? "ERPPartialUpdateVersion"
                    : "ERPCompleteUpdateVersion");

            return (!WFOptions.GetYesNoWFOptionValue("AllowOverComplete") &&
                    dTotalTxDocoQty > (_WorkOrderMasterTable.WAUORG -
                                       _WorkOrderMasterTable.WASOQS -
                                       _WorkOrderMasterTable.WASOCN));
        } // end OverCompleted

        #endregion

        #region Tolerance Logic

        #region ExceedsTolerance

        /// <summary>
        /// Checks to see if the Tolerance set up in the workflow option TolerancePercent
        /// is less than the percentage between the _TransactionQuantity and the Open WorkOrder
        /// Quantity.
        /// </summary>
        /// <param name="sUOM">UOM entered by the user</param>
        /// <param name="dConvFactor">Conversion factor to get to the primary UOM from the
        /// UOM entered by the user</param>
        /// <returns>True is tolerance is exceeded; otherwise, false</returns>
        private bool ExceedsTolerance(string sUOM, double dConvFactor)
        {
            // Tolerance Quantity: Ordered qty * (TolerancePercent / 100)
            double dTolQty = _WorkOrderMasterTable.WAUORG *
                             (WFOptions.GetDoubleWFOptionValue("TolerancePercent") / 100);

            // Open quantity on the work order:  Ordered qty - (completed qty + canceled qty)
            double dWOQtyOpen = _WorkOrderMasterTable.WAUORG - (_WorkOrderMasterTable.WASOQS +
                                                                _WorkOrderMasterTable.WASOCN);

            // Total transaction quantity including the completed and scrapped converted to primary
            double dTotalTxQty = _TransactionQuantity + _ScrapQuantity;

            // If the UOM passed matches the document's UOM, do not fetch the UOM conversion as it
            // was already passed in to the method
            if (sUOM != _WorkOrderMasterTable.WAUOM)
            {
                // Converting the total transactional quantity to the document UOM by getting the
                // conversion from document to primary and dividing the result to go back to doc
                // because I figured it may be easier to find a conversion to primary than to doc.
                dConvFactor = _tblUOMConversion.ConvertUnitOfMeasure(_BranchPlant,
                    _ShortItemNumber,
                    _WorkOrderMasterTable.WAUOM,
                    _CollectItemNumber.ItemsTable.IMUOM1);
                if (dConvFactor == 0)
                {
                    throw (new RFS_Exception("Unable to convert the WAUOM to the item's primary\n" +
                                             "in order to correctly check the tolerance.",
                        "INVTOLCONV"));
                }
            }

            dTotalTxQty /= dConvFactor; // Converting the Tx Qty to the Document UOM for Tolerance Check

            Log.WriteLine(5, "Tolerance Checking");
            Log.WriteLine(5, "WAUORG = " + _WorkOrderMasterTable.WAUORG.ToString());
            Log.WriteLine(5, "WASOQS = " + _WorkOrderMasterTable.WASOQS.ToString());
            Log.WriteLine(5, "WASOCN = " + _WorkOrderMasterTable.WASOCN.ToString());
            Log.WriteLine(5, "(WAUORG - WASOQS) - WASOCN = WorkOrderQuantityOpen");
            Log.WriteLine(5, "WorkOrderOpenQuantity = " + dWOQtyOpen.ToString());

            return ((dTotalTxQty > (dWOQtyOpen + dTolQty)) || (dTotalTxQty < (dWOQtyOpen - dTolQty)));
        } // end ExceedsTolerance

        #endregion

        #region DisplayToleranceWarning

        //*****************************************************************
        // DisplayToleranceWarning
        //*****************************************************************

        #region DisplayToleranceWarning_EnterEvent

        /// <summary>
        /// Enter event for the DisplayToleranceWarning Screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayToleranceWarning_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // If the user accepts the tolerance, determine the next screen; otherwise, always
            // go back to collect the completed quantity
            if (e.UserInput.ToBool())
            {
                // Determine the next screen based on whether or not the user just entered the
                // completed or scrapped data
                if (_UnitOfMeasureListType == "Comp")
                {
                    Next(GetNextScreenForCompUOM());
                }
                else
                {
                    Next(_CollectLocation);
                }
            }
            else
            {
                Back(_CollectQuantity);
            }
        } // end DisplayToleranceWarning_EnterEvent

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

            ScreenMap.Add("WorkOrderNumber", _WorkOrderNumber.ToString());
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
            }

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
                    } 
                }
                else
                {
                    PrintBarCode();
                }

                // Determining which screen to go to.  If the user is collect serials, go back
                // to finish collecting serials until all serials have been scanned; otherwise,
                // start the workflow over again by collecting a new work order.
                if (_FromSerialNumberEntry && (_SerialNumbersScanned < _TransactionQuantity))
                {
                    Back(_CollectSerialNumber);
                }
                else
                {
                    ClearTransaction();
                    Back(_CollectWorkOrderNumber);
                }
            }
            else
            {
                Error(this.PrintSystem.ErrorMessageVocabulary);
            }
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
            _StartOfOrder = false;
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
                PrintBarCode();

                if (_StartOfOrder)
                {
                    Next(_CollectQuantity);
                }
                else
                {
                    if ((_FromSerialNumberEntry) && (_SerialNumbersScanned < _TransactionQuantity))
                    {
                        Back(_CollectSerialNumber);
                    }
                    else
                    {
                        ClearTransaction();
                        Back(_CollectWorkOrderNumber);
                    }
                }
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
                    }

                    // Checking to make sure that the max number of labels is not exceeded
                    else if (PrintSystem.Copies > WFOptions.GetInt32WFOptionValue("MaxLabels"))
                    {
                        Error("MAXEXCEEDED", e.UserInput.ToString());
                    }
                    else
                    {
                        // Trying to locate the Label Format to get the default printer
                        PrintSystem.Reset();
                        PrintSystem.Branch = _BranchPlant;
                        PrintSystem.ItemNumber = _PrimaryItemNumber;
                        PrintSystem.Location = _LocationMasterTable.stbllocn;
                        if (PrintSystem.DefaultPrinter.Trim().Length == 0)
                        {
                            Error("LabelFormat");
                        }
                        else
                        {
                            Next(_CollectPrinterStartOfOrder, PrintSystem.DefaultPrinter);
                        }
                    }
                }
                catch (RFS_Exception ex)
                {
                    Error(ex.ErrorName);
                } // end catch
                catch
                {
                    Error("QUANTITY", e.UserInput.ToString());
                } // end catch
            }
            else
            {
                Next(_CollectQuantity);
            }
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
            if (_DefinedPrintersTable.ValidateUserPrinter(UserSession.UserId, _PrinterName))
            {
                _PrinterAddress = _DefinedPrintersTable.Address;
                PrintBarCode();
            }
            else
            {
                Error("INVALIDPRINTER");
            }
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
                    }
                    else if (PrintSystem.Copies > WFOptions.GetInt32WFOptionValue("MaxLabels"))
                    {
                        Error("MAXEXCEEDED", e.UserInput.ToString());
                    }
                    else
                    {
                        PrintSystem.Reset();
                        PrintSystem.Branch = _BranchPlant;
                        PrintSystem.ItemNumber = _PrimaryItemNumber;
                        PrintSystem.Location = _LocationMasterTable.stbllocn;
                        if (PrintSystem.DefaultPrinter.Trim().Length == 0)
                        {
                            Error("LabelFormat");
                        }
                        else
                        {
                            Next(_CollectPrinter, PrintSystem.DefaultPrinter);
                        }
                    }
                }
                catch (RFS_Exception ex)
                {
                    Error(ex.ErrorName);
                } // end catch
                catch
                {
                    Error("QUANTITY", e.UserInput.ToString());
                } // end catch
            }
            else if ((_FromSerialNumberEntry) && (_SerialNumbersScanned < _TransactionQuantity))
            {
                Back(_CollectSerialNumber);
            }
            else
            {
                ClearTransaction();
                Back(_CollectWorkOrderNumber);
            }
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
            _ItemLocationTable.RetrievePrimaryLocation(_BranchPlant, _ShortItemNumber);
            DateTime PackDate = DateTime.Now.AddHours(-2);
            PrintSystem.DropFilePathOverride = WFOptions.RetrieveOptionValue("DropFilePath");
            PrintSystem.PrinterAddress = _PrinterAddress;
            PrintSystem.PrinterName = _PrinterName;

            if ((_LPLicense)
                && (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive()))
            {
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LicensePlate", _LicensePlate, false);
            }

            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_DocumentNumber", _WorkOrderMasterTable.WADOCO.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_DocumentType", _WorkOrderMasterTable.WADCTO, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ThirdItemNumber", _CollectItemNumber.ItemsTable.IMAITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SRIPartNumber", _CollectItemNumber.ItemsTable.IMLITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ShortItemNumber", _CollectItemNumber.ItemsTable.IMITM.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PartDesc", _CollectItemNumber.ItemsTable.IMDSC1, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Branch", _CollectBranch.BranchPlantsTable.MCMCU, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotNumber", _LotNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity","1", false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_UnitOfMeasure", _UnitOfMeasure, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LocationMasterTable", _LocationMasterTable.sscnlocn, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_PrimaryItemNumber", _PrimaryItemNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotNumber", _LotNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotSerialExpDate", _LotSerialExpDate, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SerialNumber", _NextSerialNumber, false);
            _SerialTable.GetSerialNumber(_BranchPlant, _NextSerialNumber, false);
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
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PackTime", PackDate.ToString("HH:mm:ss"), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Shift", _ShiftCode, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Packer", UserSession.UserId, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Reprint", "ORIGINAL", false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PrimaryLocation", _ItemLocationTable.LILOCN, false);

            PrintSystem.ExtraPrintData.NewRow();
            if (!PrintSystem.Print())
            {
                DisplayMessage(PrintSystem.ErrorMessageVocabulary);
            }
        } // end PrintBarCode

        #endregion

        #endregion

        #region SendToERP

        private void setToLocation()
        {
            _WOTrackingTransaction.Location = _Location.Trim();
            if (_LocationMasterTable.ValidateLocation(_CollectBranch.BranchPlantsTable.MCMCU.Trim(), _Location))
            {
                _WOTrackingTransaction.ToLocnPutZone = _LocationMasterTable.LMPZON.Trim();
                _WOTrackingTransaction.ToLocnPickZone = _LocationMasterTable.LMKZON.Trim();
                _WOTrackingTransaction.ToLocnReplZone = _LocationMasterTable.LMZONR.Trim();
                _WOTrackingTransaction.ToLocnDimGroup = _LocationMasterTable.LMSTY1.Trim();
            }
        }

        private void setItem()
        {
            _WOTrackingTransaction.PrimaryItemNumber = _PrimaryItemNumber;
            _WOTrackingTransaction.ShortItemNumber = _ShortItemNumber.ToString();
            _WOTrackingTransaction.SecondItemNumber = _LongItemNumber;
            _WOTrackingTransaction.ThirdItemNumber = _ThirdItemNumber;
            _WOTrackingTransaction.ItemDescription1 = _CollectItemNumber.ItemsTable.IMDSC1; // Needed for WOW
            _WOTrackingTransaction.ItemDescription2 = _CollectItemNumber.ItemsTable.IMDSC1; // Needed for WOW
            _WOTrackingTransaction.ItemDimGroup = _CollectItemNumber.ItemBranchesTable.IBPRP6.Trim();
            _WOTrackingTransaction.ItemLotProcType = _CollectItemNumber.ItemBranchesTable.IBSRCE;
        }

        private void setIssueLocation(PartsListRecord partsListRecord)
        {
            _WOIssuesTransaction.Location = partsListRecord.Location;
            _WOIssuesTransaction.FromLocation = partsListRecord.Location;
            if (_LocationMasterTable.ValidateLocation(_BranchPlant.Trim(), partsListRecord.Location))
            {
                _WOIssuesTransaction.FromLocnPutZone = _LocationMasterTable.LMPZON.Trim();
                _WOIssuesTransaction.FromLocnPickZone = _LocationMasterTable.LMKZON.Trim();
                _WOIssuesTransaction.FromLocnReplZone = _LocationMasterTable.LMZONR.Trim();
                _WOIssuesTransaction.FromLocnDimGroup = _LocationMasterTable.LMSTY1.Trim();
            }
        }
        private void setIssueItem(PartsListRecord partsListRecord)
        {
            _WOIssuesTransaction.ShortItemNumber = partsListRecord.ShortItemNumber.ToString();

            if (_IssueItemMasterTable.FetchItem(partsListRecord.ShortItemNumber))
            {
                _WOIssuesTransaction.SecondItemNumber = _IssueItemMasterTable.IMLITM;
                _WOIssuesTransaction.ThirdItemNumber = _IssueItemMasterTable.IMAITM;
                _WOIssuesTransaction.ItemDescription1 = _IssueItemMasterTable.IMDSC1;
                _WOIssuesTransaction.ItemDescription2 = _IssueItemMasterTable.IMDSC2;
                _IssueItemBranchTable.ValidateItemBranch(_BranchPlant.Trim(), partsListRecord.ShortItemNumber);
                _WOIssuesTransaction.ItemDimGroup = _IssueItemBranchTable.IBPRP6.Trim();
                _WOIssuesTransaction.ItemLotProcType = _IssueItemBranchTable.IBSRCE;
            }
        }

        private void setFGItem()
        {
            F4101 FGItem = new F4101(UserSession.DataAccess, "FGItem");
            FGItem.FetchItem(_WorkOrderMasterTable.WAITM);

            switch (_CollectBranch.WarehouseConstantsTable._DesignatedPrimaryItemNumber)
            {
                case PrimaryItemNumberDesignations.ShortNumber:
                    _WOIssuesTransaction.FGPrimaryItemNumber = FGItem.IMITM.ToString();
                    break;
                case PrimaryItemNumberDesignations.LongNumber:
                    _WOIssuesTransaction.FGPrimaryItemNumber = FGItem.IMLITM;
                    break;
                case PrimaryItemNumberDesignations.ThirdNumber:
                    _WOIssuesTransaction.FGPrimaryItemNumber = FGItem.IMAITM;
                    break;
            } // end switch

            _WOIssuesTransaction.FGShortItemNumber = FGItem.IMITM.ToString();
            _WOIssuesTransaction.FGSecondItemNumber = FGItem.IMLITM;
            _WOIssuesTransaction.FGThirdItemNumber = FGItem.IMAITM;
            _WOIssuesTransaction.FGItemDescription1 = FGItem.IMDSC1;
            _WOIssuesTransaction.FGItemDescription2 = FGItem.IMDSC2;
            _CollectItemNumber.ItemBranchesTable.ValidateItemBranch(_BranchPlant.Trim(), FGItem.IMITM);
            _WOIssuesTransaction.FGItemDimGroup = _CollectItemNumber.ItemBranchesTable.IBPRP6.Trim();
            _WOIssuesTransaction.FGItemLotProcType = _CollectItemNumber.ItemBranchesTable.IBSRCE;
            FGItem.Dispose();
        }

        /// <summary>
        /// SendToErp initializes the ERP transaction fields and sends the transaction
        /// to the ERP system plugin.
        /// </summary>
        private void SendToERP()
        {
            if (_JDEReservationTable.DoJDERecordLocksExist("F4801", _WorkOrderNumber.ToString()))
			{
                _ItemsToIssue.Find(x => x.ShortItemNumber == _ShortItemNumber).IsCollected = false;
                throw new RFS_Exception("RECORDRESERVED");
            }

            Log.Debug("<***> SendToERP()");

            _WorkOrderRoutingTable.GetWOData(_BranchPlant, _WorkOrderNumber, WFOptions.RetrieveOptionValue("DiscreteOrRepetitive"));

            _LineItemMasterTable.ResetAll();
            _LineItemMasterTable.IPMMCU = _BranchPlant;
            _LineItemMasterTable.IPLITM = _LongItemNumber;
            _LineItemMasterTable.Fetch();

            string stationID = UserSession.ClientInfo.ClientIP;

            stationID = stationID.Replace(".", "");
            if (stationID.Length > 10)
            {
                stationID = stationID.Substring(stationID.Length - 10);
            }

            int NextNumber;
            //string NextSerialNumber;
            _NextNumberTable.GetNextNumber("InternalSerial", out NextNumber, out _NextSerialNumber);
            DateTime TransDate = DateTime.Now.AddHours(-2);
            WO = new Database.TableObjects.WorkOrderCompletion();
            WO.UserID = UserSession.CurrentUser.LoginName;
            WO.Batch = TransDate.ToString("yyMMddHHmmssfff");
            WO.CDate = "1" + TransDate.ToString("yy") + TransDate.DayOfYear.ToString("000");
            WO.WorkOrder = _WorkOrderNumber.ToString();
            WO.WOType = _WorkOrderMasterTable.WADCTO;
            WO.Part = _LongItemNumber;
            WO.Branch = _BranchPlant;
            WO.LineCell = _WorkOrderRoutingTable.WLLINE.ToString(); //_LineItemMasterTable.IPLINE;
            if (WFOptions.RetrieveOptionValue("DiscreteOrRepetitive") == "2")
            {
                WO.TypeFlag = "1";
                //WO.SeqNumber = (_WorkOrderRoutingTable.WLOPSQ * 100).ToString();//(_OperationSequence * 100).ToString(); 
            }
            else
            {
                WO.TypeFlag = "2";
                //WO.SeqNumber = (_OperationSequence * 100).ToString();
            }

            WO.AddressID = "7777"; //_WorkOrderRoutingTable.WLAN8.ToString();
            WO.RequestDate = _WorkOrderMasterTable.WADRQJ.ToString();
            WO.OrderQty = _WorkOrderMasterTable.WAUORG.ToString();
            WO.BoxQty = "1";
            WO.StationID = stationID;
            WO.CTime = TransDate.ToString("HHmmss");
            WO.Location = _Location.Trim();
            //if (WFOptions.RetrieveOptionValue("DiscreteOrRepetitive") == "2")
            //{
            //    WO.PayPoint = _WorkOrderRoutingTable.WLBFPF.ToString();
            //}
            //else
            //{
            //    WO.PayPoint = "";
            //}
            WO.Shift = _ShiftCode.Trim();
            WO.SerialNumber = _NextSerialNumber;
            WO.LotNumber = _LotNumber;
            WO.ShortItem = _ShortItemNumber.ToString();
            WO.Company = _CollectBranch.BranchPlantsTable.MCCO;
            WO.UOM = _UnitOfMeasure;
            WO.ReturnableExpendable = _ContainerType.ToString();
            WO.WorkflowID = "StoneridgeWorkOrderCompletionSerial";
            WO.ProcessID = "WOComp";
            WO.ScrapQty = _ScrapQuantity.ToString();
            WO.ScrapSeq = _WorkOrderRoutingTable.WLOPSQ.ToString();
            WO.F4801Z1 = WFOptions.RetrieveOptionValue("WriteToF4801Z1", true) == "Y" ? "1" : "0";
          
            //insert into custom table
            _SerialTable.InsertSerial(WO);
          
            //JDE Transaction
            Transactions.Add(_WOTrackingTransaction);

            Transactions.Branch = _CollectBranch.BranchPlantsTable.MCMCU.Trim().ToUpperInvariant();
            _WOTrackingTransaction.Initialize();
            setItem();
            setToLocation();
            _WOTrackingTransaction.Branch = _CollectBranch.BranchPlantsTable.MCMCU;
            _WOTrackingTransaction.EmployeeNumber = _AddresBookTable.ABAN8.ToString();
            _WOTrackingTransaction.ShiftCode = _ShiftCode.Trim();
            _WOTrackingTransaction.CurrentOperationSequence = _WorkOrderRoutingTable.WLOPSQ.ToString();
            _WOTrackingTransaction.OperationType = _WorkOrderRoutingTable.WLOPSC;
            _WOTrackingTransaction.WorkCenter = _WorkOrderRoutingTable.WLMCU;
            _WOTrackingTransaction.TypeOfHours = string.Empty;
            _WOTrackingTransaction.DocumentNumber = _WorkOrderNumber.ToString();
            _WOTrackingTransaction.DocumentType = _DocumentType;
            _WOTrackingTransaction.QuantityCompleted = _TransactionQuantity.ToString();

            // for serialized items, at this point we need to capture each S/N
            if (_FromSerialNumberEntry || ((_CollectItemNumber.ItemBranchesTable.IBSRCE.Trim().Length > 0) &&
                (Convert.ToInt16(_CollectItemNumber.ItemBranchesTable.IBSRCE) > Convert.ToInt16(LotSerialDesignations.LotRequired))))
            {
                _WOTrackingTransaction.QuantityCompleted = "1";
            }

            _WOTrackingTransaction.CompletedUOM = _UnitOfMeasure;
            _WOTrackingTransaction.QuantityScrapped = _ScrapQuantity.ToString();
            _WOTrackingTransaction.ScrappedUOM = _ScrapUnitOfMeasure;
            _WOTrackingTransaction.Lot = _LotNumber;
            _WOTrackingTransaction.LotExpDate_YYYYMMDD = _LotSerialExpDate;
            _WOTrackingTransaction.RFUserID = UserSession.CurrentUser.LoginName;
            _WOTrackingTransaction.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
            _WOTrackingTransaction.Version = _ERPUpdateVersion.Trim();
            _WOTrackingTransaction.LicensePlateNumber = _LicensePlate;
            _WOTrackingTransaction.NewOrExistingLPN = _LicensePlateProcessingType;

            if (_FromSerialNumberEntry)
            {
                _WOTrackingTransaction.UserInput01 = _LotNumber;
            }

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();

            // clear out scrap in case we are completing serialized inventory
            _ScrapQuantity = 0;
            _ScrapUnitOfMeasure = "";

            if (!Transactions.Error && _ItemsToIssue.Count > 0)
            {
                Transactions.ClearTransactions();
                Transactions.Add(_WOIssuesTransaction);

                Transactions.Branch = _BranchPlant.Trim().ToUpperInvariant();
                _WOIssuesTransaction.Initialize();

                _WOIssuesTransaction.Branch = _BranchPlant;
                _WOIssuesTransaction.DocumentNumber = _WorkOrderNumber.ToString();
                _WOIssuesTransaction.DocumentType = _DocumentType;
                _WOIssuesTransaction.ComponentBranch = _BranchPlant;
                _WOIssuesTransaction.RFUserID = UserSession.CurrentUser.LoginName;
                _WOIssuesTransaction.TransDate_YYYYMMDD = System.DateTime.Now.ToString("yyyyMMdd");
                _WOIssuesTransaction.Version = WFOptions.RetrieveOptionValue("ERPIssueUpdateVersion");
                setFGItem();

                //set UserInput01 to finished good serial
                _WOIssuesTransaction.UserInput01 = _LotNumber;


                for (int i = 0; i < _ItemsToIssue.Count; i++)
                {
                    PartsListRecord partsListRecord = _ItemsToIssue[i];
                    setIssueLocation(partsListRecord);
                    setIssueItem(partsListRecord);

                    _WOIssuesTransaction.UniqueID = partsListRecord.UniqueId.ToString();
                    _WOIssuesTransaction.Lot = partsListRecord.LotNumber;
                    _WOIssuesTransaction.Quantity = partsListRecord.Quantity.ToString();
                    _WOIssuesTransaction.UOM = partsListRecord.UnitOfMeasure;

                    if(i < _ItemsToIssue.Count - 1)
                    {
                        _WOIssuesTransaction.AddToBoundaryWithData();
                    }

                }

                Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
                DisplayMessages();
            }

            if (!Transactions.Error && WFOptions.GetYesNoWFOptionValue("EnablePackingList"))
			{
                Transactions.ClearTransactions();
                Transactions.Add(_CallUbe);

                Transactions.Branch = _BranchPlant.Trim().ToUpperInvariant();
                _CallUbe.Initialize();

                _CallUbe.UBEName = "R553111";
                _CallUbe.UBEVersion = WFOptions.RetrieveOptionValue("PackingListVersion");
                _CallUbe.DataSelection1 = _WorkOrderNumber.ToString();
                _CallUbe.DataSelection2 = _LotNumber;

                Transactions.Send("N");
            }
        }

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

        public StoneridgeWorkOrderCompletionSerial()
            : base()
        {
            InitializeComponent();
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
            _WorkOrderMasterTable.ShowWorkOrders(_AllowedWorkOrderStatusCodes, _BranchPlant);
        }

        private void _CollectSerialNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            DefaultRFSGeneratedSerialNumber();
        }

        private void DefaultRFSGeneratedLotNumber()
        {
            if (WFOptions.GetYesNoWFOptionValue("RFSGenLotSerialNumber"))
            {
                int NextNumber = 0;
                string FormattedNextNumber = "";
                if (_NextNumberTable.GetNextNumber("LotNumber", out NextNumber, out FormattedNextNumber))
                {
                    ScreenHandler.DefaultText = FormattedNextNumber;
                }
            }
        }

        private void DefaultRFSGeneratedSerialNumber()
        {
            if (WFOptions.GetYesNoWFOptionValue("RFSGenLotSerialNumber"))
            {
                int NextNumber = 0;
                string FormattedNextNumber = "";
                if (_NextNumberTable.GetNextNumber("LotNumber", out NextNumber, out FormattedNextNumber))
                {
                    _DefaultSerialNumber = $"{DateTime.Now.ToString("yyyyMMdd")}{FormattedNextNumber}";
                    ScreenHandler.DefaultText = _DefaultSerialNumber;                    
                }
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

        //issues program

        #region CollectIssueItemNumber

        private bool CollectIssueItemNumber_ValidateMethod(string sInput)
        {
            if (sInput.Length == 0)
            {
                throw new RFS_Exception("ITEMNUMBER");
            }

            if (_IssueItemMasterTable.RetrieveItem(_BranchPlant, sInput,
                    _CollectBranch.PrimaryItemDesignator.ToString()) != null)
            {
                if (_IssueItemBranchTable.ValidateItemBranch(_BranchPlant, _IssueItemMasterTable.IMITM))
                { 

                    if (_ItemsToIssue.Count(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM && x.IsCollected == true) == 1)
                    {
                        throw new RFS_Exception("Already Scanned!");
                    }
                    
                    if (_PartsListTable.RetrievePartsListRecord(_WorkOrderNumber,
                                 _DocumentType,
                                 _IssueItemMasterTable.IMITM,
                                 _AllowOverIssue,
                                 _IssueTypesAllowed,
                                 WFOptions.RetrieveOptionValue("OrderBOMBy")))
                    
                
                    {
                        if (!WFOptions.GetYesNoWFOptionValue("SerialScanMode"))
                        {
                            _IssueLocationMasterTable.ResetAll();
                            _IssueLocationMasterTable.MCU = _BranchPlant;
                            _IssueLocationMasterTable.inLOCN = _PartsListTable.WMLOCN;
                            if (_IssueLocationMasterTable.ValidateLocation())
                            {
                                ScreenHandler.ScreenMap.Add("IssueCommittedLocation", _IssueLocationMasterTable.sscnlocn);
                            }
                        }

                        // done to make sure we only select open parts list records
                        if (_PartsListTable.RetrieveOpenPartsListRecord(_WorkOrderNumber, _DocumentType, _IssueItemMasterTable.IMITM, _IssueTypesAllowed, WFOptions.RetrieveOptionValue("OrderBOMBy")))
                        {
                            // Get Open Row
                            PartsListEditing();
                            return true;
                        }
                        else if (_PartsListTable.RetrievePartsListRecord(_WorkOrderNumber.ToString(),
                                     _DocumentType,
                                     _IssueTypesAllowed,
                                     _AllowOverIssue,
                                     WFOptions.RetrieveOptionValue("OrderBOMBy"), _IssueItemMasterTable.IMITM))
                        {
                            // If Over issue get a closed row.
                            PartsListEditing();
                            return true;
                        }
                        else
                        {
                            throw new RFS_Exception("ClosedLine");
                        }
                    }
                    else
                    {
                        throw new RFS_Exception("NOTONPARTSLIST");
                    }
                }
                else
                {
                    throw new RFS_Exception("ITEMBRANCH");
                }
            }
            else
            {
                throw new RFS_Exception("ITEMNUMBER");
            }
        }

        private void WorkOrderPartsList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PartsListTable.ShowPartsList(_WorkOrderNumber.ToString(),
                _DocumentType,
                _IssueTypesAllowed,
                _AllowOverIssue,
                WFOptions.RetrieveOptionValue("OrderBOMBy"));
            WorkOrderPartsList.Table = _PartsListTable;
            WorkOrderPartsList.DataColumnForValidation = "WMCPIT";
        }

        #endregion

        #region CollectIssueLocation

        private bool CollectIssueLocation_ValidateMethod(string sInput)
        {
            if (WFOptions.GetYesNoWFOptionValue("AllowBlankLocation") || (sInput.Trim().Length > 0))
            {
                _IssueLocationMasterTable.ResetAll();
                _IssueLocationMasterTable.MCU = _BranchPlant;
                _IssueLocationMasterTable.inLOCN = sInput.ToUpper();
                if (!_IssueLocationMasterTable.ValidateLocation())
                {
                    throw new RFS_Exception("LOCATION");
                }

				if (WFOptions.GetYesNoWFOptionValue("SerialScanMode"))
				{
                    Next(CollectIssueSerialNumber);
                    return true;
				}

                _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM).Location = _IssueLocationMasterTable.stbllocn;
                PartsListRecord partsListRecord = _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM);

                if (!_ItemLocationTable.RetrieveSpecificRecord(_BranchPlant,
                        _IssueItemMasterTable.IMITM,
                        partsListRecord.Location))
                {
                    throw new RFS_Exception("LOTLOCATION");
                }

                _LotStatus = _ItemLocationTable.LILOTS;
                ScreenHandler.ScreenMap.Add("IssueRemainingQty",
                    (_PartsListTable.WMUORG - _PartsListTable.WMTRQT).ToString());
                ScreenHandler.ScreenMap.Add("IssueRemainingUOM", _PartsListTable.WMUM);

                ScreenHandler.ScreenMap.Add("ISSUELOCATION", partsListRecord.Location);
                Next(IssueLotControlledItem() ? CollectIssueLotNumber : CollectIssueQuantity);
                return true;
            }
            else
            {
                throw new RFS_Exception("BLANKSNOTALLOWED");
            }            
        }

        #endregion

        #region CollectIssueLotNumber

        private bool CollectIssueLotNumber_ValidateMethod(string sInput)
        {
            PartsListRecord partsListRecord = _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM);

            //Check that item and lot exist for location.
            if (_ItemLocationTable.ValidateLotLocn(_BranchPlant,
                    _IssueItemMasterTable.IMITM,
                    sInput,
                    partsListRecord.Location))
            {
                // if the processing option is set to not allow held lot issues, issue error
                if (HeldLot())
                {
                    throw new RFS_Exception("HELDLOT");
                }
                else
                {
                    _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM).LotNumber = _ItemLocationTable.LILOTN;

                    ScreenHandler.ScreenMap.Add("RemainingQty",
                        (_PartsListTable.WMUORG - _PartsListTable.WMTRQT).ToString());
                    ScreenHandler.ScreenMap.Add("RemainingUOM", _PartsListTable.WMUM);
                    Next(CollectIssueQuantity);
                }
            }
            else
            {
                throw new RFS_Exception("LOTNUMBER");
            }

            return true;
        }

        #endregion

        #region CollectIssueQuantity

        private bool CollectIssueQuantity_ValidateMethod(string sInput)
        {
            // Call calculator function
            try
            {
                string calculatedQuantity = _ItemLocationTable.CalcQty(sInput.ToUpper());

                if (calculatedQuantity == null)
                {
                    throw new RFS_Exception("INVALIDQUANTITY");
                }
                else if (Convert.ToDouble(calculatedQuantity) >= 0)
                {
                    _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM).Quantity = Convert.ToDouble(calculatedQuantity);

                    ScreenHandler.ScreenMap.Add("ISSUEQUANTITY",calculatedQuantity);

                    //BD012006
                    ScreenHandler.DefaultText = _PartsListTable.WMUM;
                    return true;
                }
                else
                {
                    throw new RFS_Exception("NEGATIVEENTRY");
                }
            }
            catch
            {
                throw new RFS_Exception("INVALIDQUANTITY");
            }
        }

        #endregion

        #region CollectIssueUnitOfMeasure

        private bool CollectIssueUnitOfMeasure_ValidateMethod(string sInput)
        {
            double dConvFactor = 0;

            _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM).UnitOfMeasure = sInput;

            PartsListRecord partsListRecord = _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM);

            dConvFactor = _UnitOfMeasureConversionTable1.ConvertUnitOfMeasure(_BranchPlant,
                partsListRecord.ShortItemNumber,
                partsListRecord.UnitOfMeasure,
                _IssueItemMasterTable.IMUOM1);
            if (dConvFactor == 0)
            {
                throw new RFS_Exception("UNITOFMEASURE");
            }

            //BD012006
            else if (!WFOptions.GetYesNoWFOptionValue("AllowUOMOverride") &&
                     (partsListRecord.UnitOfMeasure != _PartsListTable.WMUM))
            {
                throw new RFS_Exception("UOMOverride");
            }
            else if ((partsListRecord.Quantity > _PartsListQuantityInPrimary) &&
                     !WFOptions.GetYesNoWFOptionValue("AllowOverIssues"))
            {
                throw new RFS_Exception("OverIssue");
            }

            // for serialized items, at this point we need to capture each S/N
            else if (IssueSerializedItem())
            {
                ScreenHandler.ScreenMap.Add("ISSUESERIALNUMBERSCAN", "0");
                ScreenHandler.ScreenMap.Add("ISSUEQUANTITY", partsListRecord.Quantity.ToString());

                if (!WFOptions.GetYesNoWFOptionValue("SerialScanMode"))
                {
                    Next(CollectIssueSerialNumber);
                }

                return true;
            }
            else
            {
                Log.Debug("Issue Transaction Quantity before applying conversion factor = " +
                          partsListRecord.Quantity.ToString());
                partsListRecord.Quantity *= dConvFactor;
                Log.Debug("Issue Transaction Quantity after applying conversion factor = " +
                          partsListRecord.Quantity.ToString());
                Log.Debug("Parts List Quantity = " + _PartsListQuantityInPrimary.ToString());

                // convert quantity to primary for JDE update
                partsListRecord.UnitOfMeasure = _IssueItemMasterTable.IMUOM1;

                // verify availability
                _ItemLocationTable.RetrieveSpecificRecord(_BranchPlant,
                    partsListRecord.ShortItemNumber,
                    partsListRecord.Location,
                    partsListRecord.LotNumber);

                double LooseQtyAvail = 0;
                double LicPlateQtyAvail = 0;

                switch (WFOptions.RetrieveOptionValue("AvailabilityCheck"))
                {
                    case "A": // available quantity
                        LooseQtyAvail = Convert.ToDouble(_ItemLocationTable.Availability.ToString());
                        // for hard-committed items, we need to back out the transaction quantity since
                        // it is represented in the committed calcs
                        if ((_PartsListTable.WMCOMM == "H") || (_PartsListTable.WMCOMM == "C"))
                        {
                            if ((_BranchPlant.Trim() == _PartsListTable.WMCMCU.Trim()) &&
                                (partsListRecord.Location.Trim() == _PartsListTable.WMLOCN.Trim()) &&
                                (partsListRecord.LotNumber.Trim() == _PartsListTable.WMLOTN.Trim()))
                            {
                                LooseQtyAvail += _TransactionQuantity;
                            }
                        }

                        break;
                    case "O": // on hand quantity
                        LooseQtyAvail = _ItemLocationTable.LIPQOH;
                        break;
                    case "I":
                        LooseQtyAvail = _TransactionQuantity;
                        break;
                } // end switch

                Log.Debug("Available Quantity = " + LooseQtyAvail.ToString());
                // if this is a license plate item, get the current availability for the

                // if user has scanned a license plate, check LP availability; otherwise, check loose
                // availability.  However, if item is hard committed,
                if ((_PartsListTable.WMCOMM == "H" || (_PartsListTable.WMCOMM == "S")) &&
                    (WFOptions.RetrieveOptionValue("AvailabilityCheck") == "A"))
                {
                    if ((_BranchPlant.Trim() == _PartsListTable.WMCMCU.Trim()) &&
                        (partsListRecord.Location.Trim() == _PartsListTable.WMLOCN.Trim()) &&
                        (partsListRecord.LotNumber.Trim() == _PartsListTable.WMLOTN.Trim()))
                    {
                        partsListRecord.IsCollected = true;
                        if (_ItemsToIssue.Count(x => x.IsCollected == false) > 0)
                        {
                            if (WFOptions.GetYesNoWFOptionValue("SerialScanMode"))
                            {
                                ScreenHandler.DefaultText = string.Empty;
                                RestartLoop(CollectIssueSerialNumber);
                            }
                            else
                            {
                                RestartLoop(CollectIssueItemNumber);
                            }
                        }
                        else
                        {
                            SendToERP();
                        }

                        return true;
                    }
                }

                if (partsListRecord.Quantity > LooseQtyAvail)
                {
                    // Removed check for LP since it is not supported in WorkOrderIssues per BD020076
                    throw new RFS_Exception("AVAILABLEQTY");
                }

                partsListRecord.IsCollected = true;
                if (_ItemsToIssue.Count(x => x.IsCollected == false)> 0)
                {
                    if (WFOptions.GetYesNoWFOptionValue("SerialScanMode"))
                    {
                        ScreenHandler.DefaultText = string.Empty;
                        RestartLoop(CollectIssueSerialNumber);
                    }
                    else
                    {
                        RestartLoop(CollectIssueItemNumber);
                    }
                }
                else
                {
                    SendToERP();
                }

                return true;
            }
        }

        #endregion

        #region CollectIssueSerialNumber

        private void CollectIssueSerialNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (WFOptions.GetYesNoWFOptionValue("SerialScanMode"))
            {
                ScreenMap.Add("SerialPrompt", "Label Serial No.");
            }
			else
			{
                ScreenMap.Add("SerialPrompt", "Issue Serial No.");
            }

            ScreenHandler.DefaultText = string.Empty;
        }

        private bool CollectIssueSerialNumber_ValidateMethod(string sInput)
        {
            if (WFOptions.GetYesNoWFOptionValue("SerialScanMode"))
            {
                if (!_SerialTable.GetSerialNumber(_BranchPlant, sInput, true))
				{
                    throw new RFS_Exception("Invalid Serial!");
				}

                try
                {
                    //get item, quantity, UOM from table
                    sInput = _SerialTable.LPLOTN;
                    CollectIssueItemNumber_ValidateMethod(_SerialTable.LPLITM);
                    CollectIssueQuantity_ValidateMethod(_SerialTable.LPTQTY.ToString());
                    CollectIssueUnitOfMeasure_ValidateMethod(_SerialTable.LPUOM);
                }
                catch (RFS_Exception ex)
				{
                    ScreenHandler.DefaultText = string.Empty;
                    RestartLoop(CollectIssueSerialNumber);
                    Error(ex.ErrorName);
                    return true;
				}

                _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM).Location = _IssueLocationMasterTable.stbllocn;

            }

            _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM).LotNumber = sInput;
            _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM).SerialNumber = sInput;

            PartsListRecord partsListRecord = _ItemsToIssue.Find(x => x.ShortItemNumber == _IssueItemMasterTable.IMITM);

            ScreenHandler.ScreenMap.Add("LotNumber", partsListRecord.LotNumber);

            // determine if the serial number scanned exists
            if (_ItemLocationTable.ValidateLotLocn(_BranchPlant, partsListRecord.ShortItemNumber, partsListRecord.LotNumber, partsListRecord.Location))
            {
                // if the processing option is set to not allow held lot issues, issue error
                if (HeldLot())
                {
                    throw new RFS_Exception("HELDLOT", _LotNumber);
                }

                // for negative adjustments, need to verify availability
                double LooseQtyAvail = 0;
                double LicPlateQtyAvail = 0;

                switch (WFOptions.RetrieveOptionValue("AvailabilityCheck"))
                {
                    case "A": // available quantity
                        LooseQtyAvail = Convert.ToDouble(_ItemLocationTable.Availability.ToString());
                        break;
                    case "O": // on hand quantity
                        LooseQtyAvail = _ItemLocationTable.LIPQOH;
                        break;
                    case "I":
                        LooseQtyAvail = partsListRecord.Quantity;
                        break;
                } // end switch

                if ((_PartsListTable.WMCOMM == "H" || (_PartsListTable.WMCOMM == "S")) &&
                    (WFOptions.RetrieveOptionValue("AvailabilityCheck") == "A"))
                {
                    if ((_BranchPlant.Trim() == _PartsListTable.WMCMCU.Trim()) &&
                        (_Location.Trim() == _PartsListTable.WMLOCN.Trim()) &&
                        (_LotNumber.Trim() == _PartsListTable.WMLOTN.Trim()))
                    {
                        partsListRecord.IsCollected = true;
                        if (_ItemsToIssue.Count(x => x.IsCollected == false) > 0)
                        {
                            if (WFOptions.GetYesNoWFOptionValue("SerialScanMode"))
                            {
                                ScreenHandler.DefaultText = string.Empty;
                                RestartLoop(CollectIssueSerialNumber);
                            }
                            else
                            {
                                RestartLoop(CollectIssueItemNumber);
                            }
                        }
                        else
                        {
                            SendToERP();

                            if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                            {
                                // BD015238 - Set Label quantity to zero if the transaction quantity is negative
                                ScreenHandler.DefaultText = (_CompQtyUnconverted < 0) ? "0" : Math.Ceiling(_CompQtyUnconverted).ToString();
                            }

                            Next(_PrintBarCodeLabels);
                        }
                    }
                }

                if (LooseQtyAvail <= 0)
                {
                    Error("AVAILABLEQTY", _LotNumber);
                }
                else
                {
                    partsListRecord.IsCollected = true;
                    if (_ItemsToIssue.Count(x => x.IsCollected == false) > 0)
                    {
                        if (WFOptions.GetYesNoWFOptionValue("SerialScanMode"))
                        {
                            ScreenHandler.DefaultText = string.Empty;
                            RestartLoop(CollectIssueSerialNumber);
                        }
                        else
                        {
                            RestartLoop(CollectIssueItemNumber);
                        }
                    }
                    else
                    {
                        SendToERP();

                        if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
                        {
                            // BD015238 - Set Label quantity to zero if the transaction quantity is negative
                            ScreenHandler.DefaultText = (_CompQtyUnconverted < 0) ? "0" : Math.Ceiling(_CompQtyUnconverted).ToString();
                        }

                        Next(_PrintBarCodeLabels);
                    }
                }
            }
            else
            {
                Error("INVALIDSERIALNUMBER", _LotNumber);
            }

            return true;
        }

        #endregion

        #region ConvertPartsListQuantityToPrimaryUOM

        /// <summary>
        /// Converts the Parts List Quantity to the Primary UOM where the Parts List Quantity is
        /// WMUORG - WMTRQT.
        /// </summary>
        private void ConvertPartsListQuantityToPrimaryUOM()
        {
            // convert the part list remaining quantity to primary u/m
            double dConvFactor = 0;

            _PartsListQuantityInPrimary = _PartsListTable.WMUORG - _PartsListTable.WMTRQT;

            dConvFactor = _UnitOfMeasureConversionTable1.ConvertUnitOfMeasure(_PartsListTable.WMCMCU,
                                                                              _IssueItemMasterTable.IMITM,
                                                                              _PartsListTable.WMUM,
                                                                              _IssueItemMasterTable.IMUOM1);
            if (dConvFactor != 0)
            {
                _PartsListQuantityInPrimary *= dConvFactor;
            } // end if
        } // end ConvertPartsListQuantityToPrimaryUOM

        #endregion

        #region PartsListEditing

        /// <summary>
        ///
        /// </summary>
        private void PartsListEditing()
        {
            if ((_PartsListTable.WMTRQT >= _PartsListTable.WMUORG) && (!_AllowOverIssue))
            {
                throw new RFS_Exception("FullyIssued");
            }

            if ((_PartsListTable.WMQNTA == 999999999999999) || ((_PartsListTable.WMTRQT == 0) && (_PartsListTable.WMUORG == 0)))
            {
                throw new RFS_Exception("FullyIssued");
            }
            else
            {
                bool bFound = true;

                // convert the part list remaining quantity to primary u/m
                double dConvFactor = 0;

                _PartsListQuantityInPrimary = _PartsListTable.WMUORG - _PartsListTable.WMTRQT;

                dConvFactor = _UnitOfMeasureConversionTable1.ConvertUnitOfMeasure(_PartsListTable.WMCMCU,
                    _IssueItemMasterTable.IMITM,
                    _PartsListTable.WMUM,
                    _IssueItemMasterTable.IMUOM1);
                if (dConvFactor != 0)
                {
                    _PartsListQuantityInPrimary *= dConvFactor;
                }

                if (_IssueTypesAllowed[0].ToString().Trim().Length > 0)
                {
                    bFound = false;
                    for (int i = 0; i < _IssueTypesAllowed.Length; i++)
                    {
                        if (_IssueTypesAllowed[i].Trim() == _PartsListTable.WMITC.Trim())
                        {
                            bFound = true;
                            break;
                        }
                    }
                }

                if (bFound)
                {
                    // load screen map
                    ScreenHandler.ScreenMap.Add("WorkOrderNumber", _WorkOrderNumber.ToString());
                    ScreenHandler.ScreenMap.Add("IssueCommittedLot", _PartsListTable.WMLOTN);
                    ScreenHandler.ScreenMap.Add("IssueItemNumber", _IssueItemMasterTable.PrimaryItemNumber);
                    ScreenHandler.ScreenMap.Add("OrderDate",
                        DateTimeUtils.JDEJulian(_WorkOrderMasterTable.WATRDJ).ToString("d"));
                    ScreenHandler.ScreenMap.Add("RequestedDate",
                        DateTimeUtils.JDEJulian(_WorkOrderMasterTable.WADRQJ).ToString("d"));
                }
                else
                {
                    throw new RFS_Exception("ISSUETYPECODE");
                }
            }
        } // end PartsListEditing

        #endregion

        #region HeldLot

        /// <summary>
        /// Determines if the lot is held or not.
        /// </summary>
        /// <returns>Returns true if the Lot Status is not one of the allowed Lot Hold Codes within
        /// the Workflow Options.</returns>
        private bool HeldLot()
        {
            bool bRetval = true;

            if (_ItemLocationTable.LILOTS.Trim().Length == 0)
            {
                bRetval = false;
            } 
            else if (_AllowedLotHoldCodes[0].ToString().Trim() == "*") // allow all
            {
                bRetval = false;
            } 
            else
            {
                for (int i = 0; i < _AllowedLotHoldCodes.Length; i++)
                {
                    if (_ItemLocationTable.LILOTS.Trim() == _AllowedLotHoldCodes[i].ToString().Trim())
                    {
                        bRetval = false;
                        i = _AllowedLotHoldCodes.Length; // force end
                    } 
                } 
            } 

            return (bRetval);
        } 

        #endregion

        #region IssueLotControlledItem

        /// <summary>
        /// Check to see if the Item is a Lot Controlled Item.
        /// </summary>
        /// <returns>Returns true if the Lot's Process Type is 0 - 3; otherwise, false.</returns>
        private bool IssueLotControlledItem()
        {
            return ((_IssueItemBranchTable.IBSRCE.Trim().Length > 0) &&
                    (Convert.ToInt16(_IssueItemBranchTable.IBSRCE) >= 0) &&
                    (Convert.ToInt16(_IssueItemBranchTable.IBSRCE) <= Convert.ToInt16(LotSerialDesignations.LotRequired)));
        }

        #endregion

        #region IssueSerializedItem

        /// <summary>
        /// Check to see if the Item is a Serial Controlled Item.
        /// </summary>
        /// <returns>Returns true if the Lot's Process Type is greater than 3; otherwise, false.</returns>
        private bool IssueSerializedItem()
        {
            // for serialized items, at this point we need to capture each S/N
            return ((_IssueItemBranchTable.IBSRCE.Trim().Length > 0) &&
                    (Convert.ToInt16(_IssueItemBranchTable.IBSRCE) > Convert.ToInt16(LotSerialDesignations.LotRequired)));
        } // end SerializedItem

		#endregion

		#endregion
	}

	public class PartsListRecord
    {
        public double UniqueId { get; set; }

        public double ShortItemNumber { get; set; }

        public string Location { get; set; }

        public string LotNumber { get; set; }

        public string SerialNumber { get; set; }

        public double Quantity { get; set; }

        public string UnitOfMeasure { get; set; }

        public bool IsCollected { get; set; }
    }
}