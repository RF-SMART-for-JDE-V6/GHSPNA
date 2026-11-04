using System;
using ICS.Database.TableObjects.JDE;
using ICS.Database.TableObjects.RFS;
using ICS.Display;
using ICS.ERPI.JDE;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Receiving
{
    /// <summary>
    /// Screen Control Class For: StoneridgeReceiptRouting Workflow
    /// </summary>
    public class StoneridgeReceiptRouting : ICS.Workflow.Base.WorkflowDesign.Components.RFSWorkflowDesigner
    {
        #region Class Level defs

        #region Tables

        #region ERP Tables

        // tables
        private F0005 _UserDefinedCodesTable;

        private F0006 _BranchPlantTable; // jde Branch plant master

        private F4100 _LocationMasterTable;

        private F4101 _ItemMasterTable;

        private F4102 _ItemBranchTable;

        private F41021 _ItemLocationTable;

        // CC027417 - Remove the F4311 table so that we can Receive Work Orders.
        //private F4311 _PurchaseOrderDetailTable;
        private F41002 _UnitOfMeasureConversionTable1;

        private F41003 _UnitOfMeasureConversionTable2;

        private CustomJDETables.F43092 _ReceiptRouteDetailTable;

        private F43091 _ReceiptRouteMasterTable;

        private F41001 _WarehouseConstants; // Warehouse Constants Table

        private F43121 _POReceiverTable;

        private F4104 _ItemCrossReferenceTable;

        private F46L10 _LicensePlateMasterTable;

        private F46L001 _LicensePlateNumberingConstantsTable;

        private F46011 _ItemUOMProfileTable;

        private CustomJDETables.F4311 _PurchaseOrderDetailTable; // purchase order detail

        #endregion

        #region System Tables

        private RFS_DataManipulator _DataManipulatorTable;

        private RFS_PrinterDefinition _DefinedPrintersTable; // RFS Defined Printers

        private RFS_NextNumber _NextNumberTable;

        #endregion

        #endregion

        #region Screens

        // screens
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _UnitOfMeasureConversionList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _BranchPlantList; // list of Branch/plants

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _ReceiptRouteOrdersList; // list of orders in receipt routing

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _ReceiptRouteItemList; // list of items on selected order

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _OperationsSequenceList; // list of operations sequence

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _UserDefinedCodesList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _F43092Item_List;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _LicensePlateMasterList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectBranch;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectOrderNumber;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectOrderType;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectItemNumber;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectMoveOperation;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectMoveQuantity;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectUnitOfMeasure;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLocation;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLotNumber;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectSerialNumber;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _ShortMoveWarning;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPrinter;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _PrintBarCodeLabels;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _DefinedPrintersList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _LotNumberList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLicensePlate;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _ConfirmNewLicensePlate;

        #endregion

        #region Collected data

        // collected data
        private string _ReceiptAcknowledgement;

        private double _TransactionQuantity;

        private double _KeyedTransactionQuantity;

        private string _PrinterName;

        private string _BranchPlant;

        private string _DocumentNumber;

        private string _DocumentType;

        private double _LineNumber;

        private string _LongItemNumber;

        private string _ThirdItemNumber;

        private double _ShortItemNumber;

        private double _OnHandQuantity;

        private string _UnitOfMeasure;

        private string _KeyedUnitOfMeasure;

        private string _FromOperation;

        private string _NextOperation;

        private string _NextOperationInSequence;

        private string _LotProcessType;

        private string _Location;

        private string _LotNumber = " ";

        private string _SavedNextRoute;

        private string _PrimaryItemNumber = " "; // primary item

        private string _DefaultUnitOfMeasure;

        private string _PrinterAddress;

        private string[] _AllowedDocumentType = null;

        private string[] _AllowedNextOperationCodes = null;

        private int _SerialNumbersScanned;

        private string _ThisOperation;

        private double _OpenQuantity;

        private double _OpenQuantityInPrimaryUnitOfMeasure;

        private double _RouteDetailPXNLIN;

        private string _RouteDetailPXUOM;

        private double _RouteDetailQTYO;

        private double _RouteDetailPXLNID;

        private double _RouteDetailPXQTYO;

        private double _RouteDetailPXOPRS;

        private string _RouteDetailPXKCOO;

        private double _RouteDetailPXDOCO;

        private string _RouteDetailPXDCTO;

        private string _RouteDetailPXSFXO;

        private string _RouteDetailPXOPRC;

        private bool _bFromLocation;

        private string _LicensePlate;

        private bool _FromGenLP = false;

        private bool _LPLicense = false;

        private string _LicensePlateProcessingType = "";

        private string _LocationSeparator;

        #endregion

        #region ERPI

        private ICS.ERPI.JDE.POReceiptRoute _ReceiptRouteTransaction = new POReceiptRoute();

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for Workflow : StoneridgeReceiptRouting
        /// </summary>
        /// <param name="userSession">Contains UserSession info for controling screen display and DAL.</param>
        public StoneridgeReceiptRouting(UserSession userSession)
            : base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        } // end StoneridgeReceiptRouting

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes the object and clears any necessary data.
        /// </summary>
        public override void Dispose()
        {
            Transactions.ClearTransactions();
            ScreenHandler.ScreenMap.Clear("Error");
            base.Dispose();
        } // end Dispose

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes Workflow, sets screens and Table Objects, and declares Next with starting screen
        /// </summary>
        protected override void Initialize()
        {
            #region Table inits

            _BranchPlantTable = new F0006(UserSession.DataAccess, "");
            _UserDefinedCodesTable = new F0005(UserSession.DataAccess, "");
            _UnitOfMeasureConversionTable1 = new F41002(UserSession.DataAccess, "");
            _UnitOfMeasureConversionTable2 = new F41003(UserSession.DataAccess, "");
            _ItemBranchTable = new F4102(UserSession.DataAccess, "");
            _ItemMasterTable = new F4101(UserSession.DataAccess, "");
            _LocationMasterTable = new F4100(UserSession.DataAccess, "");
            _ReceiptRouteDetailTable = new CustomJDETables.F43092(UserSession.DataAccess, "");
            // CC027417 - Remove the F4311 table so that we can Receive Work Orders.
            //_PurchaseOrderDetailTable = new F4311(UserSession.DataAccess, "");
            _DataManipulatorTable = new RFS_DataManipulator(UserSession.DataAccess, "");
            _ReceiptRouteMasterTable = new F43091(UserSession.DataAccess, "");
            _DefinedPrintersTable = new RFS_PrinterDefinition(UserSession.DataAccess, "");
            _WarehouseConstants = new F41001(UserSession.DataAccess, "");
            _ItemLocationTable = new F41021(UserSession.DataAccess, "");
            _POReceiverTable = new F43121(UserSession.DataAccess, "");
            _ItemCrossReferenceTable = new F4104(UserSession.DataAccess, "");
            _NextNumberTable = new RFS_NextNumber(UserSession.DataAccess, "");
            _LicensePlateMasterTable = new F46L10(UserSession.DataAccess, "");
            _LicensePlateNumberingConstantsTable = new F46L001(UserSession.DataAccess, "");
            _ItemUOMProfileTable = new F46011(UserSession.DataAccess, "");
            _PurchaseOrderDetailTable = new CustomJDETables.F4311(UserSession.DataAccess, "");

            #endregion

            #region Screen Inits

            #endregion

            #region Screen Events

            // Setting of Screen Events For Workflow: StoneridgeReceiptRouting
            // Events For CollectBranch
            // Events For CollectLocation
            // Events For CollectMoveOperation
            // Events For CollectMoveQuantity
            // Events For CollectOrderNumber
            // Events For CollectOrderType
            // Events For CollectItemNumber
            // Events For CollectPrinter
            // Events For CollectUnitOfMeasure
            // Events For PrintBarCodeLabels
            // orders in receipt routing
            // items on selected order
            // operation codes
            // unit of measure conversion
            // Events For CollectLotNumber
            // Events For CollectLotExpDate
            // Events For CollectSerialNumber
            // Events For CollectSNExpDate
            // Events For ShortMoveWarning

            #endregion

            ClearTransaction();
            char[] Delimiter = {' '};
            _DocumentType = WFOptions.RetrieveOptionValue("AllowedDocType");
            _AllowedDocumentType = _DocumentType.Split(Delimiter);
            _NextOperation = WFOptions.RetrieveOptionValue("ValidNextOperation");
            // license plate tables
            if (LicensePlating())
            {
                _LPLicense = true; //CC025677
            }

            _AllowedNextOperationCodes = _NextOperation.Split(Delimiter);

            ScreenMap.Add("DateFormat", WFOptions.RetrieveOptionValue("DateFormat").ToString());
        } // end Initialize

        #endregion

        #region LicensePlating

        private bool LicensePlating()
        {
            if (UserSession.IsModuleLicensed("JLP"))
                return true;
            else
                return false;
        }

        #endregion

        #region AllowedDocType

        /// <summary>
        /// Checks to see if the Document Type is allowed.
        /// </summary>
        /// <returns>Returns true if the Document Type matches an allowed document type specified
        /// in the workflow options; otherwise, false.</returns>
        private bool AllowedDocType()
        {
            bool bRetVal = true;

            if (_AllowedDocumentType.Length > 0)
            {
                for (int i = 0; i < _AllowedDocumentType.Length; i++)
                {
                    if (_AllowedDocumentType[i].Trim().Length > 0)
                    {
                        bRetVal = false;
                        if (_AllowedDocumentType[i] == _DocumentType)
                        {
                            bRetVal = true;
                            break;
                        } // end if
                    } // end if
                } // end for
            } // end if

            return (bRetVal);
        } // end AllowedDocType

        #endregion

        #region CollectBranch

        //************************************************************
        // CollectBranch
        //************************************************************

        #region CollectBranch_EnterEvent

        /// <summary>
        /// Enter event for the CollectBranch screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectBranch_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // validate the Branch entered against the F0006 table
            _BranchPlantTable.ResetAll();
            _BranchPlantTable.BranchPlant = e.UserInput.ToString().ToUpper();
            if (_BranchPlantTable.ValidBranchPlant())
            {
                _BranchPlant = _DataManipulatorTable.FormatData("F0006", "MCMCU", _BranchPlantTable.MCMCU);
                setLocationSeperator();
            } // end if
            else
            {
                Error("MCU", e.UserInput.ToString());
            } // end else
        } // end CollectBranch_EnterEvent

        private void setLocationSeperator()
        {
            _WarehouseConstants.DeterminePrimaryItem(_BranchPlant);
            _LocationSeparator = _WarehouseConstants.CISEPL;
        }

        #endregion

        #region BranchPlantList_EnterEvent

        /// <summary>
        /// Enter event for the BranchPlant List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BranchPlantList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _BranchPlantTable.UpdateCurrentRow();
            _BranchPlant = _DataManipulatorTable.FormatData("F0006", "MCMCU", _BranchPlantTable.MCMCU);
            setLocationSeperator();
            Next(_CollectOrderNumber);
        } // end BranchPlantList_EnterEvent

        #endregion

        #endregion

        #region CollectOrderNumber

        //************************************************************
        // CollectOrderNumber
        //************************************************************

        #region CollectOrderNumber_EnterEvent

        /// <summary>
        /// Enter event for the CollectOrderNumber screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectOrderNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            double intermediateOrderNumber;
            if (e.UserInput.ToString().Trim().Length > 0)
            {
                if (double.TryParse(e.UserInput.ToString().Trim(), out intermediateOrderNumber))
                {
                    if (_ReceiptRouteDetailTable.FetchPurchaseOrderRoutingSteps(_BranchPlantTable.MCMCU, intermediateOrderNumber, new string[] { }))
                    {
                        if (_ReceiptRouteDetailTable.FetchPurchaseOrderRoutingSteps(_BranchPlantTable.MCMCU, intermediateOrderNumber, _AllowedDocumentType))
                        {
                            if (_PurchaseOrderDetailTable.ShowPOLineDetails(_BranchPlantTable.MCMCU, _ReceiptRouteDetailTable.PXDOCO, _ReceiptRouteDetailTable.PXLNID, _AllowedDocumentType))
                            {
                                if (_PurchaseOrderDetailTable.Rows.Count == 1)
                                {
                                    _DocumentNumber = _ReceiptRouteDetailTable.PXDOCO.ToString();
                                    _DocumentType = _ReceiptRouteDetailTable.PXDCTO;

                                    ScreenMap.Add("ORDERNUMBER", _DocumentNumber);
                                    ScreenMap.Add("DOCTYPE", _DocumentType);

                                    //StoreLineDetails();
                                }
                                else
                                {
                                    //Next(ListOrderLines);
                                }
                            }
                            else
                            {
                                throw new RFS_Exception("OrderNotPO");
                            }
                        }
                        else
                        {
                            throw new RFS_Exception("OrderIsNotPO");
                        }
                    }
                    else
                    {
                        throw new RFS_Exception("NoReceiptforOrder");
                    }
                }
                else
                {
                    throw new RFS_Exception("NotNumeric");
                }
            }
            else
            {
                throw new RFS_Exception("BlanksNotAllowed");
            }

            /*_DocumentNumber = e.UserInput.ToString().Trim();
            if (_DocumentNumber.Length == 0)
            {
                Error("BLANKSNOTALLOWED");
            } // end if
            else if (!double.TryParse(_DocumentNumber, out intermediateOrderNumber))
            {
                Error("NotNumeric");
            } // end if
            else
            {
                // validate the order number entered against the P/O header table
                _ReceiptRouteDetailTable.ResetAll();
                _ReceiptRouteDetailTable.PXMCU = _BranchPlantTable.MCMCU;
                _ReceiptRouteDetailTable.PXDOCO = Convert.ToDouble(e.UserInput);
                _ReceiptRouteDetailTable.PXACTO = "Y";
                if (_ReceiptRouteDetailTable.Fetch())
                {
                    _DocumentNumber = _ReceiptRouteDetailTable.PXDOCO.ToString();
                    // If the document number is unique across all document types, then bypass
                    // the document type screen
                    if (WFOptions.GetYesNoWFOptionValue("UniqueDocNumber"))
                    {
                        _DocumentNumber = _ReceiptRouteDetailTable.PXDOCO.ToString();
                        _DocumentType = _ReceiptRouteDetailTable.PXDCTO;
                        if (AllowedDocType())
                        {
                            ScreenHandler.ScreenMap.Add("ORDERNUMBER", _DocumentNumber);
                            ScreenHandler.ScreenMap.Add("DOCTYPE", _DocumentType);
                            _PrimaryItemNumber = " ";
                            Next(_CollectItemNumber);
                        } // end if
                        else
                        {
                            Error("ORDERNUMBER", e.UserInput.ToString());
                        } // end else
                    } // end if
                } // end if
                else
                {
                    Error("INVALIDPONUMBER", e.UserInput.ToString());
                } // end else
            } // end else*/
        } // end CollectOrderNumber_EnterEvent

        #endregion

        #region ReceiptRouteOrdersList_EnterEvent

        /// <summary>
        /// Enter event for the ReceiptRouteOrders List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiptRouteOrdersList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ReceiptRouteDetailTable.UpdateCurrentRow();
            _DocumentNumber = _ReceiptRouteDetailTable.PXDOCO.ToString();
            _DocumentType = _ReceiptRouteDetailTable.PXDCTO;
            ScreenHandler.ScreenMap.Add("ORDERNUMBER", _DocumentNumber);
            ScreenHandler.ScreenMap.Add("DOCTYPE", _DocumentType);
            Next(_CollectItemNumber);
        } // end ReceiptRouteOrdersList_EnterEvent

        #endregion

        #region ReceiptRouteOrdersList_BackEvent

        /// <summary>
        /// Back event for the ReceiptRouteOrders List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiptRouteOrdersList_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PrimaryItemNumber = " ";
            Back();
        } // end ReceiptRouteOrdersList_BackEvent

        private void ReceiptRouteOrdersList_ExitEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Exit();
        } // end R

        #endregion

        #endregion

        #region CollectOrderType

        //************************************************************
        // CollectOrderType
        //************************************************************

        #region CollectOrderType_EnterEvent

        /// <summary>
        /// Enter event for the CollectOrderType screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectOrderType_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PrimaryItemNumber = " ";
            _DocumentType = e.UserInput.ToString().Trim().ToUpper();
            if (_DocumentType.Length == 0)
            {
                Error("BLANKSNOTALLOWED");
            } // end if
            else if (AllowedDocType())
            {
                // validate the order number entered against the P/O header table
                _ReceiptRouteDetailTable.ResetAll();
                _ReceiptRouteDetailTable.PXMCU = _BranchPlantTable.MCMCU;
                _ReceiptRouteDetailTable.PXDOCO = Convert.ToDouble(_DocumentNumber);
                _ReceiptRouteDetailTable.PXDCTO = _DocumentType;
                _ReceiptRouteDetailTable.PXACTO = "Y";
                if (!_ReceiptRouteDetailTable.Fetch())
                {
                    Error("INVALIDPONUMBER", e.UserInput.ToString());
                } // end else
            } // end if
            else
            {
                Error("INVALIDDOCTYPE", e.UserInput.ToString());
            } // end else
        } // end CollectOrderType_EnterEvent

        #endregion

        #region UserDefinedCodesList_EnterEvent

        /// <summary>
        /// Enter event for the UserDefinedCodes List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserDefinedCodesList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UserDefinedCodesTable.UpdateCurrentRow();
            _DocumentType = _UserDefinedCodesTable.DRKY.Trim();
            // validate the order number entered against the P/O header table
            _ReceiptRouteDetailTable.ResetAll();
            _ReceiptRouteDetailTable.PXMCU = _BranchPlantTable.MCMCU;
            _ReceiptRouteDetailTable.PXDOCO = Convert.ToDouble(_DocumentNumber);
            _ReceiptRouteDetailTable.PXDCTO = _DocumentType;
            _ReceiptRouteDetailTable.PXACTO = "Y";
            if (_ReceiptRouteDetailTable.Fetch())
            {
                Next(_CollectItemNumber);
            } // end if
            else
            {
                Error("INVALIDPONUMBER", _DocumentType);
            } // end else
        } // end UserDefinedCodesList_EnterEvent

        #endregion

        #endregion

        #region CollectItemNumber

        //************************************************************
        // CollectItemNumber
        //************************************************************

        #region CollectItemNumber_EnterEvent

        /// <summary>
        /// Enter event for the CollectItemNumber screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectItemNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // set up parameters for item number verification
            ItemNumberEnter(e.UserInput.ToString().ToUpper());
        } // end CollectItemNumber_EnterEvent

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sitem"></param>
        private void ItemNumberEnter(string sitem)
        {
            // use the returned result to display the proper item number
            if (sitem.Trim().Length > 0)
            {
                _PrimaryItemNumber = _ItemMasterTable.RetrieveItem(_BranchPlant,
                    sitem,
                    _WarehouseConstants._DesignatedPrimaryItemNumber.ToString());
                // if the primary item number is null, then an invalid item was scanned
                if (_PrimaryItemNumber == null)
                {
                    Error("ITEMNUMBER", sitem);
                    return;
                } // end if
            } // end if
            else
            {
                _PrimaryItemNumber = " ";
                _ShortItemNumber = 0;
                ShowLines();
                return;
            } // end else

            // map primary item number to screen
            ScreenHandler.ScreenMap.Add("ITEMNUMBER", _PrimaryItemNumber);

            // now, verify that this item exists in F4102
            _ShortItemNumber = _ItemMasterTable.IMITM;
            if (_ItemBranchTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber))
            {
                ShowLines();
            } // end if
            else
            {
                Error("ITEMBRANCH", sitem);
            } // end else
        } // end ItemNumberEnter

        #endregion

        #region F43092Item_List_EnterEvent

        /// <summary>
        /// Enter event for the F43092Item List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F43092Item_List_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ReceiptRouteDetailTable.UpdateCurrentRow();
            // CC027417 - The code removed is in ProcessSelectedItem.
            ProcessSelectedItem();
        } // end F43092Item_List_EnterEvent

        #endregion

        #region ShowLines

        /// <summary>
        /// Show Open Lines
        /// </summary>
        private void ShowLines()
        {
            // If the primary item number is blank, then we are not requesting a single item in the
            // returned list, but ALL items; otherwise, we are expecting a particular item
            if (_PrimaryItemNumber.Trim().Length == 0)
            {
                if (_ReceiptRouteDetailTable.ShowOpenLines(_BranchPlant, _DocumentNumber, _DocumentType))
                {
                    _ReceiptRouteItemList.Show();
                } // end if
                else
                {
                    Back(_CollectOrderNumber);
                } // end else
            } // end if
            else if (_ReceiptRouteDetailTable.ShowOpenLines(_BranchPlant,
                         _DocumentNumber,
                         _DocumentType,
                         _ShortItemNumber))
            {
                if (_ReceiptRouteDetailTable.Rows.Count > 1)
                {
                    _ReceiptRouteItemList.Show();
                }
                else if (_ReceiptRouteDetailTable.Rows.Count == 1)
                {
                    ProcessSelectedItem();
                }
            } // end else if
            else
            {
                Error("ITEMNOTONORDER");
            } // end else
        } // end ShowLines

        #endregion

        #region ReceiptRouteItemList_EnterEvent

        /// <summary>
        /// Enter event for the ReceiptRouteItem List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiptRouteItemList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ReceiptRouteDetailTable.UpdateCurrentRow();
            ProcessSelectedItem();
        } // end ReceiptRouteItemList_EnterEvent

        #endregion

        #region ProcessSelectedItem

        /// <summary>
        /// Process Selected Item from the F43092 List
        /// </summary>
        private void ProcessSelectedItem()
        {
            ScreenHandler.DefaultText = _ReceiptRouteDetailTable.PXOPRC;

            // CC032380 Set so that lookup later can find item and description used for screenmaps. 
            _ShortItemNumber = Convert.ToDouble(_ReceiptRouteDetailTable.CurrentRow["IMITM"]);

            // CC027417 - Use the F43121 instead of the F4311.
            _POReceiverTable.ResetAll();
            _POReceiverTable.PRMATC = "1";
            _POReceiverTable.PRKCOO = _ReceiptRouteDetailTable.PXKCOO;
            _POReceiverTable.PRDOCO = _ReceiptRouteDetailTable.PXDOCO;
            _POReceiverTable.PRDCTO = _ReceiptRouteDetailTable.PXDCTO;
            _POReceiverTable.PRSFXO = _ReceiptRouteDetailTable.PXSFXO;
            _POReceiverTable.PRLNID = _ReceiptRouteDetailTable.PXLNID;
            _POReceiverTable.PRNLIN = _ReceiptRouteDetailTable.PXNLIN;
            _Location = _ReceiptRouteDetailTable.PXLOCN;
            if (_POReceiverTable.Fetch())
            {
                _DefaultUnitOfMeasure = _POReceiverTable.PRUOM;
                ScreenHandler.ScreenMap.Add("MOVEOPERATION", _ReceiptRouteDetailTable.PXOPRC.ToString());
                _NextOperationInSequence = GetNextOperation();
                ScreenHandler.ScreenMap.Add("NEXTOPERATION", _NextOperation);
                ScreenHandler.DefaultText = _NextOperationInSequence;
                _ThisOperation = _ReceiptRouteDetailTable.PXOPRC.ToString();

                SaveFields();
                //_CollectMoveOperation.Show();
                Next(_CollectMoveOperation);
            } // end if
            else
            {
                Error("ITEMNUMBER");
            } // end else
        } // end ProcessSelectedItem

        #endregion

        #endregion

        #region CollectMoveOperation

        //************************************************************
        // CollectMoveOperation
        //************************************************************

        #region CollectMoveOperation_EnterEvent

        /// <summary>
        /// Enter event for the CollectMoveOperation screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectMoveOperation_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _NextOperation = e.UserInput.ToString().ToUpper();
            if (ValidNextOperation())
                DefaultMoveQuantity();
            else
                Error("INVALIDNEXTOPERATION", e.UserInput.ToString());
        } // end CollectMoveOperation_EnterEvent

        #endregion

        #region CollectMoveOperation_BackEvent

        /// <summary>
        /// Back event for the CollectMoveOperation screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectMoveOperation_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PrimaryItemNumber = "";
        } // end CollectMoveOperation_BackEvent

        #endregion

        #region OperationsSequenceList_EnterEvent

        /// <summary>
        /// Enter event for the OperationsSequence List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationsSequenceList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ReceiptRouteDetailTable.UpdateCurrentRow();
            _NextOperation = _ReceiptRouteDetailTable.PXOPRC;

            if (ValidNextOperation())
            {
                ScreenHandler.ScreenMap.Add("NextOperation", _NextOperation);
                DefaultMoveQuantity();
                Next(_CollectMoveQuantity);
            } // end if
            else
            {
                Error("INVALIDNEXTOPERATION");
            } // end else
        } // end OperationsSequenceList_EnterEvent

        #endregion

        #region DefaultMoveQuantity

        /// <summary>
        /// If the DefaultQuantity workflow option is set, then the quantity is defaulted.
        /// </summary>
        private void DefaultMoveQuantity()
        {
            if (WFOptions.GetYesNoWFOptionValue("DefaultQuantity"))
            {
                ScreenHandler.DefaultText = _RouteDetailQTYO.ToString();
            } // end if
        } // end DefaultMoveQuantity

        #endregion

        #endregion

        #region CollectMoveQuantity

        //************************************************************
        // CollectMoveQuantity
        //************************************************************

        #region CollectMoveQuantity_EnterEvent

        /// <summary>
        /// Enter event for the CollectMoveQuantity screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectMoveQuantity_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // Call calculator function
            try
            {
                string CalculatedQuantity = _ItemLocationTable.CalcQty(e.UserInput.ToString().ToUpper());

                if (CalculatedQuantity == null)
                {
                    Error("INVALIDQUANTITY", e.UserInput.ToString());
                } // end if
                else
                {
                    _TransactionQuantity = Convert.ToDouble(CalculatedQuantity);
                    _KeyedTransactionQuantity = _TransactionQuantity;
                    if (_TransactionQuantity < 0)
                    {
                        Error("InvalidQuantity", e.UserInput.ToString());
                        return;
                    }

                    ScreenHandler.ScreenMap.Add("QUANTITY", CalculatedQuantity);
                    Next(_CollectUnitOfMeasure, _DefaultUnitOfMeasure);
                } // end else
            } // end try
            catch
            {
                Error("INVALIDQUANTITY", e.UserInput.ToString());
            } // end catch
        } // end CollectMoveQuantity_EnterEvent

        #endregion

        #endregion

        #region CollectUnitOfMeasure

        //************************************************************
        // CollectUnitOfMeasure
        //************************************************************

        #region CollectUnitOfMeasure_EnterEvent

        /// <summary>
        /// Enter event for the CollectUnitOfMeasure screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectUnitOfMeasure_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasure = e.UserInput.ToString().ToUpper();
            _KeyedUnitOfMeasure = _UnitOfMeasure;
            ProcessUOMEnter();
        } // end CollectUnitOfMeasure_EnterEvent

        /// <summary>
        /// Process the UOM entered.
        /// </summary>
        private void ProcessUOMEnter()
        {
            _LicensePlate = "";
            double dConvFactor = 0;
            // validate unit of measure
            if (WFOptions.GetYesNoWFOptionValue("AllowUOMOverride") || (_UnitOfMeasure == _RouteDetailPXUOM))
            {
                _ItemMasterTable.FetchItem(_ShortItemNumber);
                dConvFactor = _UnitOfMeasureConversionTable1.ConvertUnitOfMeasure(_BranchPlant,
                    _ShortItemNumber,
                    _UnitOfMeasure,
                    _ItemMasterTable.IMUOM1);
                if (dConvFactor == 0)
                {
                    _TransactionQuantity = _KeyedTransactionQuantity;
                    Error("UNITOFMEASURE", _KeyedUnitOfMeasure);
                }
                else
                {
                    // convert quantity to primary for JDE update
                    Log.WriteLine(5, "Transaction Quantity before applying conversion factor = " +
                                     _TransactionQuantity.ToString());
                    _TransactionQuantity = _TransactionQuantity * dConvFactor;
                    Log.WriteLine(5, "Transaction Quantity after applying conversion factor = " +
                                     _TransactionQuantity.ToString());

                    _UnitOfMeasure = _ItemMasterTable.IMUOM1;

                    // else if going to stock for a lot controlled item
                    if (_ItemBranchTable.ValidateItemBranch(_BranchPlant, _ShortItemNumber))
                    {
                        _LotProcessType = _ItemBranchTable.IBSRCE;
                    } // end if
                    else
                    {
                        _LotProcessType = "";
                    } // end else

                    ConvertOpenQuantityToPrimary();
                    if (OverMove())
                    {
                        _TransactionQuantity = _KeyedTransactionQuantity;
                        Error("OVERMOVENOTALLOWED", _KeyedUnitOfMeasure);
                    } // end if
                    else if (ShortMove() && WFOptions.GetYesNoWFOptionValue("DisplayShortWarning"))
                    {
                        Next(_ShortMoveWarning);
                    } // end if
                    else if (_ReceiptAcknowledgement == "Y")
                    {
                        // for serialized items, at this point we need to capture each S/N
                        if ((_LotProcessType.Trim().Length > 0) &&
                            (Convert.ToInt16(_LotProcessType) > Convert.ToInt16(LotSerialDesignations.LotRequired)))
                        {
                            ScreenHandler.ScreenMap.Add("SNSCANNED", "0");
                            ScreenHandler.ScreenMap.Add("QUANTITY", _TransactionQuantity.ToString());
                            Next(_CollectLicensePlate);
                        } // end if
                        else if (PromptForLot())
                        {
                            Next(_CollectLotNumber);
                        } // end else if
                        else
                        {
                            DefaultPrimary();
                        } // end else
                    } // end else if
                    else
                    {
                        SendTXandReturn();
                    } // end else
                } // end else
            } // end if
            else
            {
                _TransactionQuantity = _KeyedTransactionQuantity;
                Error("UOMOverride", _KeyedUnitOfMeasure);
            } // end else
        } // end ProcessUOMEnter

        #endregion

        private void SendTXandReturn()
        {
            // otherwise, call web service to update PeopleSoft - no stock effect
            SendToERP(_TransactionQuantity);
            ClearTransaction();
            if (_ReceiptRouteDetailTable.ShowOpenLines(_BranchPlant, _DocumentNumber, _DocumentType))
            {
                RestartLoop(_CollectItemNumber);
            }
            else
            {
                RestartLoop(_CollectOrderNumber);
            }
        }

        #region UnitOfMeasureList_EnterEvent

        /// <summary>
        /// Enter event for the UnitOfMeasure List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnitOfMeasureList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // user has selected a record from the list
            _UnitOfMeasureConversionTable1.UpdateCurrentRow();
            _UnitOfMeasure = _UnitOfMeasureConversionTable1.UMUM;
            ProcessUOMEnter();
        } // end UnitOfMeasureList_EnterEvent

        #endregion

        #endregion

        #region CollectLocation

        //************************************************************
        // CollectLocation
        //************************************************************

        #region CollectLocation_EnterEvent

        /// <summary>
        /// Enter event for the CollectLocation screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLocation_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // validate the receiving location
            LocationEnter(e.UserInput.ToString().ToUpper());
        } // end CollectLocation_EnterEvent

        /// <summary>
        /// Process the Location Entered.
        /// </summary>
        /// <param name="Location"></param>
        private void LocationEnter(string Location)
        {
            if (WFOptions.GetYesNoWFOptionValue("AllowBlankLocation") || (Location.Trim().Length > 0))
            {
                _LocationMasterTable.ResetAll();
                _LocationMasterTable.MCU = _BranchPlant;
                _LocationMasterTable.inLOCN = Location;
                if (_LocationMasterTable.ValidateLocation())
                {
                    _Location = _LocationMasterTable.stbllocn;
                    if (_ItemLocationTable.ValidateLotLocn(_BranchPlant, _ShortItemNumber, null, _Location))
                    {
                        _OnHandQuantity = _ItemLocationTable.LIPQOH;
                    } // end if
                    else
                    {
                        _OnHandQuantity = 0;
                    } // end else

                    ScreenHandler.ScreenMap.Add("LOCATION", _LocationMasterTable.sscnlocn);
                    // for serialized items, at this point we need to capture each S/N
                    if (isSerialized())
                    {
                        ScreenHandler.ScreenMap.Add("SNSCANNED", "0");
                        ScreenHandler.ScreenMap.Add("QUANTITY", _TransactionQuantity.ToString());
                        Next(_CollectSerialNumber);
                        return;
                    }
                    else
                    {
                        Next(_CollectLicensePlate);
                    }
                } // end if
                else
                {
                    Error("LOCATION", Location);
                } // end else
            } // end if
            else
            {
                Error("BLANKSNOTALLOWED");
            } // end else
        } // end LocationEnter

        #endregion

        #endregion

        #region CollectLotNumber

        //************************************************************
        // CollectLotNumber
        //************************************************************

        #region CollectLotNumber_EnterEvent

        /// <summary>
        /// Enter event for the CollectLotNumber screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectLotNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotNumber = e.UserInput.ToString().ToUpper();
            LotNumberEnter();
        } // end CollectLotNumber_EnterEvent

        /// <summary>
        /// Process the Lot Number entered.
        /// </summary>
        private void LotNumberEnter()
        {
            if ((_LotNumber.Trim().Length == 0) && LotRequired())
            {
                Error("LOTREQUIRED");
                return;
            }

            _ItemLocationTable.ValidateItemLot(_BranchPlant, _ShortItemNumber, _LotNumber);

            DefaultPrimary();
            Next(_CollectLocation);
        } // end LotNumberEnter

        #endregion

        #region LotNumberList_EnterEvent

        /// <summary>
        /// Enter event for the LotNumber List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotNumberList_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemLocationTable.UpdateCurrentRow();
            if (_bFromLocation)
            {
                LocationEnter(_ItemLocationTable.LILOCN);
            }
            else
            {
                _LotNumber = _ItemLocationTable.LILOTN;
                LotNumberEnter();
            }
        } // end LotNumberList_EnterEvent

        #endregion

        #endregion

        #region CollectSerialNumber

        //************************************************************
        // CollectSerialNumber
        //************************************************************

        #region CollectSerialNumber_EnterEvent

        /// <summary>
        /// Enter event for the CollectSerialNumber screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectSerialNumber_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LotNumber = e.UserInput.ToString().ToUpper();
            _ItemLocationTable.LIPQOH = 0;

            if ((_ItemBranchTable.IBSRCE.Trim().Length > 0) &&
                (Convert.ToInt16(_ItemBranchTable.IBSRCE) > Convert.ToInt16(LotSerialDesignations.SerialOptional)))
            {
                SendToERP(1);
                _SerialNumbersScanned += 1;
                ScreenHandler.ScreenMap.Add("SNSCANNED", _SerialNumbersScanned.ToString());
                // for serialized items, default to '1'
                if ((_ItemBranchTable.IBSRCE.Trim().Length > 0) &&
                    (Convert.ToInt16(_ItemBranchTable.IBSRCE) > Convert.ToInt16(LotSerialDesignations.SerialOptional)))
                {
                    ScreenHandler.DefaultText = "1";
                }
            } // end if
        } // end CollectSerialNumber_EnterEvent

        #endregion

        #endregion

        #region ShortMoveWarning

        //************************************************************
        // CollectShortMoveWarning
        //************************************************************

        #region ShortMoveWarning_EnterEvent

        /// <summary>
        /// Enter event for the ShortMoveWarning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShortMoveWarning_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToBool())
            {
                if (_ReceiptAcknowledgement == "Y")
                {
                    if (PromptForLot())
                    {
                        Next(_CollectLotNumber);
                    } // end if
                    else
                    {
                        DefaultPrimary();
                        Next(_CollectLocation);
                    } // end else
                } // end if
                else
                {
                    SendToERP(_TransactionQuantity);
                    ClearTransaction();
                    if (_ReceiptRouteDetailTable.ShowOpenLines(_BranchPlant, _DocumentNumber, _DocumentType))
                    {
                        RestartLoop(_CollectItemNumber);
                    }
                    else
                    {
                        RestartLoop(_CollectOrderNumber);
                    }
                } // end else
            } // end if
            else
            {
                _TransactionQuantity = _KeyedTransactionQuantity;
                Back(_CollectMoveQuantity);
            } // end else
        } // end ShortMoveWarning_EnterEvent

        #endregion

        #endregion

        #region Label Printing

        //************************************************************
        // Label Printing
        //************************************************************

        #region PrintBarCodeLabels

        //************************************************************
        // CollectPrintBarCodeLabels
        //************************************************************

        #region PrintBarCodeLabels_EnterEvent

        private void _PrintBarCodeLabels_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (WFOptions.GetYesNoWFOptionValue("DefaultBarcodeQty"))
            {
                ScreenHandler.DefaultText = (_TransactionQuantity < 0) ? "0" : Math.Ceiling(_TransactionQuantity).ToString();
            } // end if
        }

        /// <summary>
        /// Enter event for the PrintBarCodeLabels screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintBarCodeLabels_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                //bypass printing if user enteres 0 or blank on collect label qty screen
                if (e.UserInput.ToString().Trim().Length == 0 || e.UserInput.ToString().Trim() == "0")
                {
                    if (isSerialized() &&
                        (_SerialNumbersScanned < _TransactionQuantity))
                    {
                        Back(_CollectSerialNumber);
                    } // end if
                    else
                    {
                        ClearTransaction();
                        if (_ReceiptRouteDetailTable.ShowOpenLines(_BranchPlant, _DocumentNumber, _DocumentType))
                        {
                            RestartLoop(_CollectItemNumber);
                        }
                        else
                        {
                            RestartLoop(_CollectOrderNumber);
                        }
                    } // end else
                } // end if
                else
                {
                    PrintSystem.Copies = e.UserInput.ToInt32();

                    if (PrintSystem.Copies < 0)
                    {
                        throw (new ArgumentException("Negative label qty entered"));
                    } // end if
                    else if ((WFOptions.RetrieveOptionValue("MaxLabels").Length > 0) &&
                             PrintSystem.Copies <= WFOptions.GetInt32WFOptionValue("MaxLabels"))
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
                    } // end else if
                    else
                    {
                        Error("MAXEXCEEDED", e.UserInput.ToString());
                    } // end else
                } // end else
            } // end try
            catch
            {
                Error("QUANTITY", e.UserInput.ToString());
            } // end catch
        } // end PrintBarCodeLabels_EnterEvent

        #endregion

        #region PrintBarCodeLabels_BackEvent

        /// <summary>
        /// Back event for the PrintBarCodeLabels screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintBarCodeLabels_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ClearTransaction();
            if (_ReceiptRouteDetailTable.ShowOpenLines(_BranchPlant, _DocumentNumber, _DocumentType))
            {
                RestartLoop(_CollectItemNumber);
            }
            else
            {
                RestartLoop(_CollectOrderNumber);
            }
        } // end PrintBarCodeLabels_BackEvent

        #endregion

        #endregion

        #region CollectPrinter

        //************************************************************
        // CollectPrinter
        //************************************************************

        #region CollectPrinter_EnterEvent

        /// <summary>
        /// Enter event for the CollectPrinter screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectPrinter_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PrinterName = e.UserInput.ToString();
            CollectPrinterEnter();
        } // end CollectPrinter_EnterEvent

        /// <summary>
        /// Processes the Printer entered.
        /// </summary>
        private void CollectPrinterEnter()
        {
            try
            {
                if (PrintSystem.ValidPrinter(_PrinterName))
                {
                    _PrinterAddress = PrintSystem.PrinterAddress;
                    PrintBarCode();
                    if (isSerialized() &&
                        (_SerialNumbersScanned < _TransactionQuantity))
                    {
                        Back(_CollectSerialNumber);
                    } // end if
                    else
                    {
                        ClearTransaction();
                        if (_ReceiptRouteDetailTable.ShowOpenLines(_BranchPlant, _DocumentNumber, _DocumentType))
                        {
                            RestartLoop(_CollectItemNumber);
                        }
                        else
                        {
                            RestartLoop(_CollectOrderNumber);
                        }
                    } // end else
                } // end if
                else
                {
                    Error(PrintSystem.ErrorMessageVocabulary);
                } // end else
            } // end try
            catch
            {
                if (isSerialized() &&
                    (_SerialNumbersScanned < _TransactionQuantity))
                {
                    Back(_CollectSerialNumber);
                } // end if
                else
                {
                    ClearTransaction();
                    if (_ReceiptRouteDetailTable.ShowOpenLines(_BranchPlant, _DocumentNumber, _DocumentType))
                    {
                        RestartLoop(_CollectItemNumber);
                    }
                    else
                    {
                        RestartLoop(_CollectOrderNumber);
                    }
                } // end else
            } // end catch
        } // end CollectPrinterEnter

        private bool isSerialized()
        {
            return (_ItemBranchTable.IBSRCE.Trim().Length > 0) &&
                   (Convert.ToInt16(_ItemBranchTable.IBSRCE) > Convert.ToInt16(LotSerialDesignations.SerialOptional));
        }

        #endregion

        #region FRFSPrinters_EnterEvent

        /// <summary>
        /// Enter event for the FRFSPrinters List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FRFSPrinters_EnterEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DefinedPrintersTable.UpdateCurrentRow();
            _PrinterName = _DefinedPrintersTable.PrinterName;
            CollectPrinterEnter();
        } // end FRFSPrinters_EnterEvent

        #endregion

        #endregion

        #region PrintBarCode

        /// <summary>
        /// Print the BarCode Label
        /// </summary>
        private void PrintBarCode()
        {
            PrintSystem.DropFilePathOverride = WFOptions.RetrieveOptionValue("DropFilePath");
            PrintSystem.PrinterAddress = _PrinterAddress;
            PrintSystem.PrinterName = _PrinterName;

            // CC027417 - Changed code to use the F43121 instead of F4311
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_DocumentNumber", _POReceiverTable.PRDOCO.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_DocumentType", _POReceiverTable.PRDCTO, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ThirdItemNumber", _POReceiverTable.PRAITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ShortItemNumber", _POReceiverTable.PRITM.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_SecondItemNumber", _POReceiverTable.PRLITM, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Description1", _ItemMasterTable.IMDSC1, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Description2", _ItemMasterTable.IMDSC2, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_Branch", _BranchPlantTable.MCMCU.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LotNumber", _LotNumber, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_TransactionQuantity", _TransactionQuantity.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_UnitOfMeasure", _UnitOfMeasure, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_ExpirationDate", string.Empty, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_LocationMasterTable", _LocationMasterTable.sscnlocn, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("_PrimaryItemNumber", _PrimaryItemNumber, false);
            PrintSystem.ExtraPrintData.NewRow();

            if (!PrintSystem.Print())
            {
                DisplayMessage(PrintSystem.ErrorMessageVocabulary);
            } // end if
        } // end PrintBarCode

        #endregion

        #endregion

        #region ClearTransaction

        /// <summary>
        /// Clears out transactional fields and return to item number screen
        /// </summary>
        private void ClearTransaction()
        {
            _LineNumber = 0;
            _LongItemNumber = "";
            _ShortItemNumber = 0;
            _LotNumber = "";
            _Location = "";
            _TransactionQuantity = 0;
            _UnitOfMeasure = "";
            _LotProcessType = "";
            _SerialNumbersScanned = 0;
            _ReceiptAcknowledgement = "";
            _PrimaryItemNumber = " ";
        } // end ClearTransaction

        #endregion

        #region GetNextOperation

        /// <summary>
        /// Gets the next operation.
        /// </summary>
        /// <returns>Returns the next operation.</returns>
        private string GetNextOperation()
        {
            string sOper = "";

            _ReceiptRouteMasterTable.ResetAll();
            _ReceiptRouteMasterTable.PPMCU = _BranchPlantTable.MCMCU;
            _ReceiptRouteMasterTable.PPNROU = _ReceiptRouteDetailTable.PXNROU;
            _ReceiptRouteMasterTable.AppendToWhere("Order By PPOPRS");
            if (_ReceiptRouteMasterTable.Fetch())
            {
                for (int i = 0; i < _ReceiptRouteMasterTable.Rows.Count; i++)
                {
                    if (Convert.ToDouble(_ReceiptRouteMasterTable.Rows[i]["PPOPRS"]) > Convert.ToDouble(_ReceiptRouteDetailTable.PXOPRS))
                    {
                        _ReceiptRouteMasterTable.PPOPRS = _ReceiptRouteDetailTable.PXOPRS;
                        sOper = _ReceiptRouteMasterTable.Rows[i]["PPOPRC"].ToString();
                        break;
                    } // end if
                } // end for
            } // end if

            return (sOper);
        } // end GetNextOperation

        #endregion

        #region GetItemMaster

        /// <summary>
        /// Gets the Primary Item Number, and the Primary Location.
        /// </summary>
        private void GetItemMaster()
        {
            _ItemMasterTable.ResetAll();
            _ItemMasterTable.IMITM = _ShortItemNumber;
            _ItemMasterTable.Fetch();
            switch (_WarehouseConstants._DesignatedPrimaryItemNumber)
            {
                case PrimaryItemNumberDesignations.ShortNumber: // short number
                    _PrimaryItemNumber = _ItemMasterTable.IMITM.ToString();
                    break;
                case PrimaryItemNumberDesignations.LongNumber: // second item number
                    _PrimaryItemNumber = _ItemMasterTable.IMLITM;
                    break;
                case PrimaryItemNumberDesignations.ThirdNumber: // third item number
                    _PrimaryItemNumber = _ItemMasterTable.IMAITM;
                    break;
            } // end switch

            ScreenHandler.ScreenMap.Add("ITEMNUMBER", _PrimaryItemNumber);
            ScreenHandler.ScreenMap.Add("Description", _ItemMasterTable.IMDSC1);

            _LocationMasterTable.ResetAll();
            _LocationMasterTable.inLOCN = _ItemLocationTable.RetrievePrimaryLocation(_BranchPlant,
                _ShortItemNumber);
            _LocationMasterTable.MCU = _BranchPlant;
            if (_LocationMasterTable.ValidateLocation())
            {
                ScreenHandler.ScreenMap.Add("PrimaryLocation", _LocationMasterTable.sscnlocn);
            } // end if
        } // end GetItemMaster

        #endregion

        #region ValidNextOperation

        /// <summary>
        /// Validates the Next Operation
        /// </summary>
        /// <returns></returns>
        private bool ValidNextOperation()
        {
            bool bValidNextWFO = true;
            bool bRetVal = false;

            #region is the operation entered actually valid

            _ReceiptRouteMasterTable.ResetAll();
            _ReceiptRouteMasterTable.PPMCU = _BranchPlantTable.MCMCU;
            _ReceiptRouteMasterTable.PPNROU = _SavedNextRoute;
            _ReceiptRouteMasterTable.PPOPRC = _NextOperation;
            if (_ReceiptRouteMasterTable.Fetch())

                #endregion

            {
                #region Check against the ValidNextOperation workflow option

                _ReceiptAcknowledgement = (_ReceiptRouteMasterTable.PPUPIB == "PQOH") ? "Y" : "";

                // Is the workflow "ValidNextOperation" value set (blank = all)
                if (_AllowedNextOperationCodes.Length > 0)
                {
                    for (int i = 0; i < _AllowedNextOperationCodes.Length; i++)
                    {
                        if (_AllowedNextOperationCodes[i].Trim().Length > 0)
                        {
                            bValidNextWFO = false;
                            if (_NextOperation == _AllowedNextOperationCodes[i])
                            {
                                bValidNextWFO = true;
                                break;
                            } // end if
                        } // end if
                    } // end for
                } // end if

                if (bValidNextWFO)

                    #endregion

                {
                    ScreenMap.Add("NextOperation", _NextOperation);

                    #region Is this operation in the routing?

                    // must validate using the "ToOperation" workflow value
                    //       1-only the next operation (default)
                    //       2-current or any next operatino
                    //       3-any operation
                    switch (WFOptions.RetrieveOptionValue("ToOperation"))
                    {
                        case "1":
                            bRetVal = (_NextOperation.ToUpper().Trim() == _NextOperationInSequence.ToUpper().Trim());
                            break;
                        case "2":
                            bRetVal = ((_NextOperation.ToUpper().Trim() == _ThisOperation.ToUpper().Trim()) ||
                                       (_NextOperation.ToUpper().Trim() == _NextOperationInSequence.ToUpper().Trim()));
                            break;
                        case "3":
                            bRetVal = true;
                            break;
                    } // end switch

                    #endregion
                } // end if
            } // end if

            return (bRetVal);
        } // end ValidNextOperation

        #endregion

        #region ShortMove

        /// <summary>
        /// Determines is the move is short based on the Transaction quantity versuses the Open
        /// Quantity in the Primary UOM.
        /// </summary>
        /// <returns>Returns true if the move is short; otherwise, false.</returns>
        private bool ShortMove()
        {
            // if the return value was zero, then a conversion was not found, don't change the quantity
            return (_TransactionQuantity < _OpenQuantityInPrimaryUnitOfMeasure);
        } // end ShortMove

        #endregion

        #region OverMove

        /// <summary>
        /// Check to see if they have over moved.
        /// </summary>
        /// <returns>Returns true if the Transaction Quantity exceeds the Open Quantity in the
        /// Primary UOM.</returns>
        private bool OverMove()
        {
            // if the return value was zero, then a conversion was not found, don't change the quantity
            return (_TransactionQuantity > _OpenQuantityInPrimaryUnitOfMeasure);
        } // end OverMove

        #endregion

        #region ConvertOpenQuantityToPrimary

        /// <summary>
        /// Converts the Open Quantity to the Primary UOM.
        /// </summary>
        private void ConvertOpenQuantityToPrimary()
        {
            // attempt to retrieve the conversion from the DataManipulator table
            // convert the open quantity to primary

            // CC027417 - Use the F43121 instead of the F4311.
            _OpenQuantity = _ReceiptRouteDetailTable.PXQTYO;
            _OpenQuantityInPrimaryUnitOfMeasure = 0;

            double dConvFactor;

            if (_UnitOfMeasure == _RouteDetailPXUOM)
            {
                dConvFactor = 1;
            } // end if
            else
            {
                //SCR 18591 MDossey - The FROM UOM and the TO UOM arguments were transposed in both calls to 
                //ConvertToPrimaryUOM below. This causes an OverMovement error because the transaction quantity
                //is always greater than the open quantity  from the F43092 detail record.
                dConvFactor = _UnitOfMeasureConversionTable1.ConvertToPrimaryUOM(_BranchPlant, _ShortItemNumber, _RouteDetailPXUOM, _UnitOfMeasure);
                Log.WriteLine(5, "Conversion Factor after call to ConvertToPrimaryUOM = " + dConvFactor.ToString());
                Log.WriteLine(5, "Branch/Plant = " + _BranchPlant);
                Log.WriteLine(5, "Short Item Number = " + _ShortItemNumber.ToString());
                Log.WriteLine(5, "From U/M = " + _RouteDetailPXUOM.ToString());
                Log.WriteLine(5, "To U/M = " + _UnitOfMeasure.ToString());
                if (dConvFactor == 0)
                {
                    //SCR 18591 MDossey - switched parameter order so that the From and To UOM match up correctly.
                    dConvFactor = _UnitOfMeasureConversionTable2.ConvertToPrimaryUOM(_RouteDetailPXUOM, _UnitOfMeasure);
                } // end if
            } // end else

            Log.WriteLine(5, "Conversion Factor = " + dConvFactor.ToString());

            Log.WriteLine(5, "Open Quantity before applying conversion factor = " +
                             _RouteDetailQTYO.ToString());
            _OpenQuantityInPrimaryUnitOfMeasure = _RouteDetailQTYO * dConvFactor;
            Log.WriteLine(5, "Open Quantity after applying conversion factor = " +
                             _OpenQuantityInPrimaryUnitOfMeasure.ToString());
        } // end ConvertOpenQuantityToPrimary

        #endregion

        #region DefaultPrimary

        /// <summary>
        /// Defaults the Primary Location.
        /// </summary>
        private void DefaultPrimary()
        {
            if (WFOptions.GetYesNoWFOptionValue("SuggestFromPrimary"))
            {
                GetItemMaster();

                // retrieve the item's primary location record
                ScreenHandler.DefaultText = _ItemLocationTable.RetrievePrimaryLocation(_BranchPlant,
                    _ShortItemNumber);
            } // end if
        } // end DefaultPrimary

        #endregion

        #region SaveFields

        /// <summary>
        /// Save Fields
        /// </summary>
        private void SaveFields()
        {
            _FromOperation = _ReceiptRouteDetailTable.PXOPRC.ToString();
            _SavedNextRoute = _ReceiptRouteDetailTable.PXNROU;

            _RouteDetailPXNLIN = _ReceiptRouteDetailTable.PXNLIN;
            _RouteDetailPXUOM = _ReceiptRouteDetailTable.PXUOM;
            _RouteDetailQTYO = _ReceiptRouteDetailTable.PXQTYO;
            //_RouteDetailPXLNID = FormatDeformatLine(_ReceiptRouteDetailTable.PXLNID, true);
            _RouteDetailPXLNID = _ReceiptRouteDetailTable.PXLNID;

            _RouteDetailPXQTYO = _ReceiptRouteDetailTable.PXQTYO;
            _RouteDetailPXOPRS = _ReceiptRouteDetailTable.PXOPRS;
            _RouteDetailPXKCOO = _ReceiptRouteDetailTable.PXKCOO;
            _RouteDetailPXDOCO = _ReceiptRouteDetailTable.PXDOCO;
            _RouteDetailPXDCTO = _ReceiptRouteDetailTable.PXDCTO;
            _RouteDetailPXSFXO = _ReceiptRouteDetailTable.PXSFXO;
            _RouteDetailPXOPRC = _ReceiptRouteDetailTable.PXOPRC;
            ScreenHandler.ScreenMap.Add("QuantityAtOperation", _ReceiptRouteDetailTable.PXQTYO.ToString());
            ScreenHandler.ScreenMap.Add("OperationUOM", _ReceiptRouteDetailTable.PXUOM);
            GetItemMaster();

            // CC027417 - Use the F43121 instead of the F4311.
            _LineNumber = _POReceiverTable.PRLNID;
            _ShortItemNumber = _POReceiverTable.PRITM;
            _LongItemNumber = _POReceiverTable.PRLITM;
            _ThirdItemNumber = _POReceiverTable.PRAITM;
            _UnitOfMeasure = _POReceiverTable.PRUOM;
            ScreenHandler.ScreenMap.Add("CommittedLotNumber", _POReceiverTable.PRLOTN.ToString());
        } // end SaveFields

        #endregion

        #region Format/Deformat Saved Line Number

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LineNumber"></param>
        /// <param name="Format"></param>
        /// <returns></returns>
        private double FormatDeformatLine(double LineNumber, bool Format)
        {
            // Deformat the pxlnid field - needed for lookup by routing steps
            _DataManipulatorTable.ResetAll();
            _DataManipulatorTable.TblName = "F43092";
            _DataManipulatorTable.FldName = "PXLNID";
            if (_DataManipulatorTable.Fetch())
            {
                if (_DataManipulatorTable.Decimals > 0)
                {
                    if (Format)
                    {
                        LineNumber /= _DataManipulatorTable.Decimals;
                    } // end if
                    else
                    {
                        LineNumber *= _DataManipulatorTable.Decimals;
                    } // end else
                } // end if
            } // end if

            return (LineNumber);
        } // end FormatDeformatLine

        #endregion

        #region SendToERP

        private void setToLocation()
        {
            _ReceiptRouteTransaction.StockLocation = _Location;
            if (_LocationMasterTable.ValidateLocation(_BranchPlantTable.MCMCU.ToString().Trim(), _Location))
            {
                _ReceiptRouteTransaction.ToLocnPutZone = _LocationMasterTable.LMPZON.Trim();
                _ReceiptRouteTransaction.ToLocnPickZone = _LocationMasterTable.LMKZON.Trim();
                _ReceiptRouteTransaction.ToLocnReplZone = _LocationMasterTable.LMZONR.Trim();
                _ReceiptRouteTransaction.ToLocnDimGroup = _LocationMasterTable.LMSTY1.Trim();
            }
        }

        private void setItem()
        {
            _ReceiptRouteTransaction.PrimaryItemNumber = _PrimaryItemNumber;
            _ReceiptRouteTransaction.ShortItemNumber = _ShortItemNumber.ToString();
            _ReceiptRouteTransaction.SecondItemNumber = _LongItemNumber;
            _ReceiptRouteTransaction.ThirdItemNumber = _ThirdItemNumber;
            _ItemMasterTable.FetchItem(_ShortItemNumber);
            _ReceiptRouteTransaction.ItemDescription1 = _ItemMasterTable.IMDSC1; // Needed for WOW
            _ReceiptRouteTransaction.ItemDescription2 = _ItemMasterTable.IMDSC1; // Needed for WOW
            _ItemBranchTable.ValidateItemBranch(_BranchPlantTable.MCMCU.ToString().Trim(), _ShortItemNumber);
            _ReceiptRouteTransaction.ItemDimGroup = _ItemBranchTable.IBPRP6.Trim();
            _ReceiptRouteTransaction.ItemLotProcType = _ItemBranchTable.IBSRCE;
        }

        /// <summary>
        /// Send the Transaction to the ERP system.
        /// </summary>
        private void SendToERP(double Qty)
        {
            Transactions.Add(_ReceiptRouteTransaction);

            Transactions.Branch = _BranchPlantTable.MCMCU.Trim().ToUpperInvariant();
            _ReceiptRouteTransaction.Initialize();
            setItem();
            setToLocation();
            _ReceiptRouteTransaction.BranchPlant = _BranchPlantTable.MCMCU.ToString();
            _ReceiptRouteTransaction.OrderCompany = _RouteDetailPXKCOO.ToString();
            _ReceiptRouteTransaction.OrderType = _RouteDetailPXDCTO.ToString();
            _ReceiptRouteTransaction.OrderNumber = _RouteDetailPXDOCO.ToString();
            //_ReceiptRouteTransaction.ReceiptLineNumber = _ReceiptRouteDetailTable.PXLNID.ToString();
            _ReceiptRouteTransaction.ReceiptLineNumber = _RouteDetailPXLNID.ToString();

            _ReceiptRouteTransaction.LineNumber = _RouteDetailPXLNID.ToString();

            _ReceiptRouteTransaction.QtyAtOperation = _RouteDetailQTYO.ToString();
            _ReceiptRouteTransaction.OperationSequence = _RouteDetailPXOPRS.ToString();
            _ReceiptRouteTransaction.ReceiptLineNumber = _RouteDetailPXNLIN.ToString();

            if (_UnitOfMeasure != _RouteDetailPXUOM)
            {
                double WorkQuantity = _UnitOfMeasureConversionTable1.ConvertToDocumentUOM(_BranchPlant,
                    _ShortItemNumber,
                    _UnitOfMeasure,
                    _RouteDetailPXUOM,
                    Qty,
                    _ItemMasterTable.IMUOM1);
                // if the return value was zero, then a conversion was not found, don't change the quantity
                if (WorkQuantity == 0)
                {
                    double dConvFactor = _UnitOfMeasureConversionTable2.ConvertToPrimaryUOM(_UnitOfMeasure,
                        _RouteDetailPXUOM);
                    if (dConvFactor != 0)
                    {
                        Qty = Qty * dConvFactor;
                        _UnitOfMeasure = _RouteDetailPXUOM;
                    } // end if
                } // end if
                else
                {
                    Qty = WorkQuantity;
                    _UnitOfMeasure = _RouteDetailPXUOM;
                } // end else
            } // end if

            _ReceiptRouteTransaction.Lot = _LotNumber;
            _ReceiptRouteTransaction.QtyToMove = Qty.ToString();
            _ReceiptRouteTransaction.UOM = _UnitOfMeasure;
            _ReceiptRouteTransaction.FromOperationCode = _RouteDetailPXOPRC.ToString();
            _ReceiptRouteTransaction.ToOperationCode = _NextOperation.ToUpper();
            _ReceiptRouteTransaction.ToLicensePlate = _LicensePlate;

            _ReceiptRouteTransaction.TransDate_YYYYMMDD = System.DateTime.Now.ToString("yyyyMMdd");
            _ReceiptRouteTransaction.RFUserID = UserSession.CurrentUser.LoginName;
            _ReceiptRouteTransaction.Version = WFOptions.RetrieveOptionValue("ERPUpdateVersion");
            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();
            if (_LotNumber.Trim().Length == 0)
            {
                _LotNumber = _ReceiptRouteTransaction.ReturnField("LotNumber", 0, 0);
            } // end if
        } // end SendToERP

        #endregion

        #region LoadReceivedLotNumber

        /// <summary>
        /// 
        /// </summary>
        private void LoadReceivedLotNumber()
        {
            ScreenHandler.DefaultText = "";
            _POReceiverTable.ResetAll();
            _POReceiverTable.PRMATC = "1";
            _POReceiverTable.PRKCOO = _RouteDetailPXKCOO;
            _POReceiverTable.PRDOCO = _RouteDetailPXDOCO;
            _POReceiverTable.PRDCTO = _RouteDetailPXDCTO;
            _POReceiverTable.PRSFXO = _RouteDetailPXSFXO;
            _POReceiverTable.PRLNID = _RouteDetailPXLNID;
            _POReceiverTable.PRNLIN = _RouteDetailPXNLIN;
            if (_POReceiverTable.Fetch())
            {
                _LotNumber = _POReceiverTable.PRLOTN;
                ScreenHandler.DefaultText = _LotNumber;
            } // end if
        } // end LoadReceivedLotNumber

        #endregion

        #region PromptForLot

        /// <summary>
        /// Determines if the user should be prompted for the Lot screen.
        /// </summary>
        /// <returns></returns>
        private bool PromptForLot()
        {
            int LotType;

            if (_LotProcessType.Trim().Length > 0)
            {
                LotType = Convert.ToInt16(_LotProcessType);
                if ((LotType >= Convert.ToInt16(LotSerialDesignations.LotOptional)) &&
                    (LotType <= Convert.ToInt16(LotSerialDesignations.LotRequired)))
                {
                    LoadReceivedLotNumber();

                    return (true);
                } // end if
            } // end if

            return (false);
        } // end PromptForLot

        #endregion

        #region LotRequired

        /// <summary>
        /// Determines if a lot is required
        /// </summary>
        /// <returns></returns>
        private bool LotRequired()
        {
            int LotType;
            if (_LotProcessType.Trim().Length > 0)
            {
                LotType = Convert.ToInt16(_LotProcessType);
                if ((LotType == Convert.ToInt16(LotSerialDesignations.LotRequired)) ||
                    (LotType == Convert.ToInt16(LotSerialDesignations.SerialRequired)))
                {
                    return (true);
                } // end if
            } // end if

            return (false);
        }

        public StoneridgeReceiptRouting()
            : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._LotNumberList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectLotNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._PrintBarCodeLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._DefinedPrintersList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._ReceiptRouteOrdersList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectOrderNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectOrderType = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._UserDefinedCodesList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._ReceiptRouteItemList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectMoveOperation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._OperationsSequenceList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectMoveQuantity = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectUnitOfMeasure = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._UnitOfMeasureConversionList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectItemNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._F43092Item_List = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._BranchPlantList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectBranch = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectSerialNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ShortMoveWarning = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._CollectLicensePlate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._LicensePlateMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._ConfirmNewLicensePlate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // _LotNumberList
            // 
            this._LotNumberList.ListScreenParent = this._CollectLotNumber;
            this._LotNumberList.ParentScreen = this._CollectLotNumber;
            this._LotNumberList.ScreenName = "F41021_List";
            this._LotNumberList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LotNumberList_EnterEvent);
            this._LotNumberList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LotNumberList_LoadEvent);
            // 
            // _CollectLotNumber
            // 
            this._CollectLotNumber.ListScreen = this._LotNumberList;
            this._CollectLotNumber.ListScreenControl = this._LotNumberList;
            this._CollectLotNumber.NextScreen = this._CollectLocation;
            this._CollectLotNumber.ScreenName = "CollectLotNumber";
            this._CollectLotNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotNumber_EnterEvent);
            this._CollectLotNumber.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotNumber_ListEvent);
            this._CollectLotNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLotNumber_LoadEvent);
            // 
            // _CollectLocation
            // 
            this._CollectLocation.ListScreen = this._LotNumberList;
            this._CollectLocation.ListScreenControl = this._LotNumberList;
            this._CollectLocation.NextScreen = this._PrintBarCodeLabels;
            this._CollectLocation.ScreenName = "CollectLocation";
            this._CollectLocation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_EnterEvent);
            this._CollectLocation.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_ListEvent);
            // 
            // _PrintBarCodeLabels
            // 
            this._PrintBarCodeLabels.NextScreen = this._CollectPrinter;
            this._PrintBarCodeLabels.ScreenName = "PrintBarCodeLabels";
            this._PrintBarCodeLabels.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabels_EnterEvent);
            this._PrintBarCodeLabels.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._PrintBarCodeLabels_LoadEvent);
            // 
            // _CollectPrinter
            // 
            this._CollectPrinter.ListScreen = this._DefinedPrintersList;
            this._CollectPrinter.ListScreenControl = this._DefinedPrintersList;
            this._CollectPrinter.ScreenName = "CollectPrinter";
            this._CollectPrinter.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_EnterEvent);
            // 
            // _DefinedPrintersList
            // 
            this._DefinedPrintersList.ListScreenParent = this._CollectPrinter;
            this._DefinedPrintersList.ParentScreen = this._CollectPrinter;
            this._DefinedPrintersList.ScreenName = "FRFS_Printers";
            this._DefinedPrintersList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.FRFSPrinters_EnterEvent);
            this._DefinedPrintersList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.DefinedPrintersList_LoadEvent);
            // 
            // _ReceiptRouteOrdersList
            // 
            this._ReceiptRouteOrdersList.ListScreenParent = this._CollectOrderNumber;
            this._ReceiptRouteOrdersList.NextScreen = this._UserDefinedCodesList;
            this._ReceiptRouteOrdersList.ParentScreen = this._CollectOrderNumber;
            this._ReceiptRouteOrdersList.ScreenName = "F43092_List";
            this._ReceiptRouteOrdersList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ReceiptRouteOrdersList_EnterEvent);
            this._ReceiptRouteOrdersList.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ReceiptRouteOrdersList_BackEvent);
            this._ReceiptRouteOrdersList.ExitValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ReceiptRouteOrdersList_ExitEvent);
            this._ReceiptRouteOrdersList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ReceiptRouteOrdersList_LoadEvent);
            // 
            // _CollectOrderNumber
            // 
            this._CollectOrderNumber.ListScreen = this._ReceiptRouteOrdersList;
            this._CollectOrderNumber.ListScreenControl = this._ReceiptRouteOrdersList;
            this._CollectOrderNumber.NextScreen = this._CollectOrderType;
            this._CollectOrderNumber.ScreenName = "CollectOrderNumber";
            this._CollectOrderNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectOrderNumber_EnterEvent);
            // 
            // _CollectOrderType
            // 
            this._CollectOrderType.ListScreen = this._UserDefinedCodesList;
            this._CollectOrderType.ListScreenControl = this._UserDefinedCodesList;
            this._CollectOrderType.NextScreen = this._CollectItemNumber;
            this._CollectOrderType.ScreenName = "CollectOrderType";
            this._CollectOrderType.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectOrderType_EnterEvent);
            // 
            // _UserDefinedCodesList
            // 
            this._UserDefinedCodesList.ListScreenParent = this._CollectOrderType;
            this._UserDefinedCodesList.NextScreen = this._ReceiptRouteItemList;
            this._UserDefinedCodesList.ParentScreen = this._CollectOrderType;
            this._UserDefinedCodesList.ScreenName = "F0005_List";
            this._UserDefinedCodesList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UserDefinedCodesList_EnterEvent);
            this._UserDefinedCodesList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._UserDefinedCodesList_LoadEvent);
            // 
            // _ReceiptRouteItemList
            // 
            this._ReceiptRouteItemList.NextScreen = this._CollectMoveOperation;
            this._ReceiptRouteItemList.ScreenName = "F43092Item_List";
            this._ReceiptRouteItemList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ReceiptRouteItemList_EnterEvent);
            this._ReceiptRouteItemList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ReceiptRouteItemList_LoadEvent);
            this._ReceiptRouteItemList.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ReceiptRouteItemList_LeavingEvent);
            // 
            // _CollectMoveOperation
            // 
            this._CollectMoveOperation.ListScreen = this._OperationsSequenceList;
            this._CollectMoveOperation.ListScreenControl = this._OperationsSequenceList;
            this._CollectMoveOperation.NextScreen = this._CollectMoveQuantity;
            this._CollectMoveOperation.ScreenName = "CollectMoveOperation";
            this._CollectMoveOperation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectMoveOperation_EnterEvent);
            this._CollectMoveOperation.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectMoveOperation_BackEvent);
            // 
            // _OperationsSequenceList
            // 
            this._OperationsSequenceList.ListScreenParent = this._CollectMoveOperation;
            this._OperationsSequenceList.ParentScreen = this._CollectMoveOperation;
            this._OperationsSequenceList.ScreenName = "F43092OP_List";
            this._OperationsSequenceList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.OperationsSequenceList_EnterEvent);
            this._OperationsSequenceList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._OperationsSequenceList_LoadEvent);
            // 
            // _CollectMoveQuantity
            // 
            this._CollectMoveQuantity.NextScreen = this._CollectUnitOfMeasure;
            this._CollectMoveQuantity.ScreenName = "CollectMoveQuantity";
            this._CollectMoveQuantity.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectMoveQuantity_EnterEvent);
            // 
            // _CollectUnitOfMeasure
            // 
            this._CollectUnitOfMeasure.ListScreen = this._UnitOfMeasureConversionList;
            this._CollectUnitOfMeasure.ListScreenControl = this._UnitOfMeasureConversionList;
            this._CollectUnitOfMeasure.NextScreen = this._CollectLocation;
            this._CollectUnitOfMeasure.ScreenName = "CollectUnitOfMeasure";
            this._CollectUnitOfMeasure.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectUnitOfMeasure_EnterEvent);
            // 
            // _UnitOfMeasureConversionList
            // 
            this._UnitOfMeasureConversionList.ListScreenParent = this._CollectUnitOfMeasure;
            this._UnitOfMeasureConversionList.ParentScreen = this._CollectUnitOfMeasure;
            this._UnitOfMeasureConversionList.ScreenName = "F41002_List";
            this._UnitOfMeasureConversionList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UnitOfMeasureList_EnterEvent);
            this._UnitOfMeasureConversionList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UnitOfMeasureConversionList_LoadEvent);
            // 
            // _CollectItemNumber
            // 
            this._CollectItemNumber.ListScreen = this._F43092Item_List;
            this._CollectItemNumber.ListScreenControl = this._F43092Item_List;
            this._CollectItemNumber.NextScreen = this._ReceiptRouteItemList;
            this._CollectItemNumber.ScreenName = "CollectItemNumber";
            this._CollectItemNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectItemNumber_EnterEvent);
            // 
            // _F43092Item_List
            // 
            this._F43092Item_List.ListScreenParent = this._CollectItemNumber;
            this._F43092Item_List.ParentScreen = this._CollectItemNumber;
            this._F43092Item_List.ScreenName = "F43092Item_List";
            this._F43092Item_List.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.F43092Item_List_EnterEvent);
            this._F43092Item_List.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._F43092Item_List_LoadEvent);
            // 
            // _BranchPlantList
            // 
            this._BranchPlantList.ListScreenParent = this._CollectBranch;
            this._BranchPlantList.ParentScreen = this._CollectBranch;
            this._BranchPlantList.ScreenName = "F0006_List";
            this._BranchPlantList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.BranchPlantList_EnterEvent);
            this._BranchPlantList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._BranchPlantList_LoadEvent);
            // 
            // _CollectBranch
            // 
            this._CollectBranch.ListScreen = this._BranchPlantList;
            this._CollectBranch.ListScreenControl = this._BranchPlantList;
            this._CollectBranch.NextScreen = this._CollectOrderNumber;
            this._CollectBranch.ScreenName = "CollectBranch";
            this._CollectBranch.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectBranch_EnterEvent);
            // 
            // _CollectSerialNumber
            // 
            this._CollectSerialNumber.NextScreen = this._PrintBarCodeLabels;
            this._CollectSerialNumber.ScreenName = "CollectSerialNumber";
            this._CollectSerialNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSerialNumber_EnterEvent);
            this._CollectSerialNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSerialNumber_LoadEvent);
            // 
            // _ShortMoveWarning
            // 
            this._ShortMoveWarning.NextScreen = this._CollectLocation;
            this._ShortMoveWarning.ScreenName = "ShortMoveWarning";
            this._ShortMoveWarning.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ShortMoveWarning_EnterEvent);
            // 
            // _CollectLicensePlate
            // 
            this._CollectLicensePlate.ListScreen = this._LicensePlateMasterList;
            this._CollectLicensePlate.ListScreenControl = this._LicensePlateMasterList;
            this._CollectLicensePlate.NextScreen = this._PrintBarCodeLabels;
            this._CollectLicensePlate.ScreenName = "CollectLicensePlate";
            this._CollectLicensePlate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_EnterEvent);
            this._CollectLicensePlate.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_BackEvent);
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
            // _ConfirmNewLicensePlate
            // 
            this._ConfirmNewLicensePlate.ScreenName = "ConfirmNewLicensePlate";
            this._ConfirmNewLicensePlate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmNewLicensePlate_EnterEvent);
            this._ConfirmNewLicensePlate.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmNewLicensePlate_BackEvent);
            // 
            // StoneridgeReceiptRouting
            // 
            this.StartScreen = this._CollectBranch;
            this.WorkflowID = "StoneridgeReceiptRouting";
        }

        private void _ReceiptRouteItemList_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _ReceiptRouteItemList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void _BranchPlantList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _BranchPlantTable.FetchBranchPlants();
        }

        private void _ReceiptRouteOrdersList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ReceiptRouteDetailTable.ShowOrders(_BranchPlant, _AllowedDocumentType);
        }

        private void _UserDefinedCodesList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UserDefinedCodesTable.ResetAll();
            _UserDefinedCodesTable.DRSY = "00  ";
            _UserDefinedCodesTable.DRRT = "DT";
            _UserDefinedCodesTable.Fetch();
        }

        private void _F43092Item_List_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ReceiptRouteDetailTable.ShowOpenLines(_BranchPlant, _DocumentNumber, _DocumentType);
        }

        private void _OperationsSequenceList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                _ReceiptRouteDetailTable.ShowRoutingSteps(_BranchPlant,
                    _DocumentNumber,
                    _DocumentType,
                    ScreenHandler.Environment,
                    FormatDeformatLine(_RouteDetailPXLNID, false));
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }

        private void UnitOfMeasureConversionList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureConversionTable1.ResetAll();
            _UnitOfMeasureConversionTable1.UMITM = _POReceiverTable.PRITM;
            _UnitOfMeasureConversionTable1.Fetch();
        }

        private void DefinedPrintersList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DefinedPrintersTable.FetchPrintersByUserWorkflow(UserSession.UserId, WorkflowID);
        }

        private void LotNumberList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemLocationTable.RetrieveItemLocationRecords(_BranchPlant, _ItemMasterTable.IMITM, this.WFOptions.RetrieveOptionValue("ShowZeroOnHand"));
        }

        private void CollectSerialNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
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

        private void CollectLotNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            DefaultRFSGeneratedLotNumber();
        }

        private void CollectLotNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _bFromLocation = false;
        }

        private void CollectLocation_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _bFromLocation = true;
        }
        // end LotRequired

        #endregion

        #region CollectLicensePlate

        #region CollectLicensePlate_LoadEvent

        private void CollectLicensePlate_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlateNumberingConstantsTable.ResetAll();
            _LicensePlateNumberingConstantsTable.BranchPlant = _BranchPlant;
            if ((_LPLicense) &&
                (_LicensePlateNumberingConstantsTable.IsLicensePlatingActive())) //CC025677
            {
                if (!_ItemUOMProfileTable.ALicenPlateItem(_BranchPlant, _ShortItemNumber))
                {
                    _CollectLicensePlate.CancelLoad = true;
                    _LicensePlate = "";
                    ProcessThisRoute();
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
            else
            {
                _CollectLicensePlate.CancelLoad = true;
                _LicensePlate = "";
                ProcessThisRoute();
                return;
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
                ProcessThisRoute();
                return;
            }

            // if location separators are present, user scanned location.
            if (_LicensePlate.IndexOf(_LocationSeparator) >= 1)
            {
                _Location = _LicensePlate;
                _LicensePlate = "";
                ProcessThisRoute();
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

            _LicensePlateProcessingType = "E";
            if (_Location.Trim() != _LicensePlateMasterTable.LMLOCN.Trim())
            {
                Error("LOCATIONMISMATCH", _LicensePlate);
                return;
            }

            _Location = _LicensePlateMasterTable.LMLOCN;
            // ReceivingLocationEnterKey();
            ProcessThisRoute();
        }

        private void CollectLicensePlate_BackEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlate = "";
            if (isSerialized())
            {
                Back(_CollectSerialNumber);
            }
            else
            {
                Back();
            }
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

            ProcessThisRoute();
        }

        private void ProcessThisRoute()
        {
            if (isSerialized())
            {
                Next(_CollectSerialNumber);
            }
            else if (_ReceiptAcknowledgement != "Y")
            {
                SendTXandReturn();
            }
            else
            {
                SendToERP(_TransactionQuantity);
                Next(_PrintBarCodeLabels);
            }
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

        private void _LicensePlateMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _LicensePlateMasterTable.ResetAll();
            _LicensePlateMasterTable.LMMCU = _BranchPlant;
            _LicensePlateMasterTable.Fetch();
        }

        #endregion
    } // end StoneridgeReceiptRouting Class
} // end ICS.Workflow.Base.JDE.Receiving Namespace