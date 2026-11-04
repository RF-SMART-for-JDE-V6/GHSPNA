using System;
using System.Data;
using ICS.Database.TableObjects.JDE;
using ICS.Database.TableObjects.RFS;
using ICS.Display;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using ICS.Stoneridge.Wireless.JDE.Utilities.Printing;
using CustomRFSTables = ICS.Stoneridge.Wireless.RFS.Database.TableObjects;
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Cartonization
{
    public partial class StoneridgeCartonMaintenance : ICS.Workflow.Base.WorkflowDesign.Components.RFSWorkflowDesigner
    {
        #region enums

        /// <summary>
        /// Enum holds values for different ways of traversing warehouse during picking.
        /// </summary>
        public enum BuildSequence : int
        {
            AddItem = 1,

            AddCarton = 2,

            RemoveCarton = 3,

            CartonTransfer = 4,

            ItemTransfer = 5,

            DeletePallet = 6,

            WeightsAndDimensions = 7,

            MergePallet = 8,

            UnpackPallet = 9
        }

        #endregion

        #region Class Variables

        #region Tables

        F0101 _addressBookTable;

        F0006 _branchTable;

        F4211 _salesOrderTable;

        CustomJDETables.F4620 _JDECartonTable;

        F59RFCTL _RFSCartonTable;

        F41001 _warehouseConstantsTable;

        F4101 _itemMasterTable;

        F4102 _itemBranchTable;

        F41021 _itemLocationTable;

        F41002 _uomConversionTable;

        F41003 _uomConversionTable2;

        F4108 _lotTable;

        DataTable _printItems;

        RFS_DataManipulator _dataManipulator;

        RFS_PrinterDefinition _printersTable;

        RFS_Vocabulary _vocabTable;

        #endregion

        private StoneridgePrinting _StoneridgePrinting;

        private GenCartonPrintData _GenCartonPrintData;

        string _printerAddress = string.Empty;

        string[] _allowedStatus;

        BuildSequence _sequence = BuildSequence.AddCarton;

        bool _forceBranch = true;

        double _toCartonId = double.MinValue;

        double _cartonId = double.MinValue;

        double _fromCartonId = double.MinValue;

        string _toCartonCode = string.Empty;

        string _toCartonSerial = string.Empty;

        string _p4620Version = string.Empty;

        string _primaryItem = string.Empty;

        double _shortItem = double.MinValue;

        double _quantity = double.MinValue;

        double _quantityInPrimary = double.MinValue;

        ICS.ERPI.JDE.MaintainCarton _maintainCarton;

        ICS.ERPI.JDE.UpdateCartonWtandDim _weightDim;

        double _pickedInPrimary = double.MinValue;

        double _cartonQuanityInPrimary = double.MinValue;

        double _availableQuantityInPrimary = double.MinValue;

        double _weight = double.MinValue;

        double _length = double.MinValue;

        double _width = double.MinValue;

        double _height = double.MinValue;

        bool _defaultDim = false;

        bool _defaultWeight = false;

        int _maxLabels = 0;

        int _maxItemsPerLabel = 1;

        string _defaultUOM = string.Empty;

        private string _serialNumberType = string.Empty;

        private string UnpackPalletNumber = string.Empty;

        private double UnpackPalletCrossReferenceNumber = -1;

        private string UnpackSerialNumber = string.Empty;

        private double UnpackUKID = -1;

        private int UnpackLabelType = 0;

        private int NumberOfUnpackLabels;

        #region Allowed Unpack Sales Order Statuses

        // Private WFO variable
        private string[] _wfoAllowedUnpackSalesOrderStatuses;

        // Public WFO variable (Generate New If NULL)
        public string[] AllowedUnpackSalesOrderStatuses
        {
            get { return _wfoAllowedUnpackSalesOrderStatuses ?? (_wfoAllowedUnpackSalesOrderStatuses = WFOptions.RetrieveOptionValue("AllowUnpackSOStatus").Split(' ')); }
        }

        #endregion

        #region Unpacked Sales Order Status

        // Private WFO variable
        private string _wfoUnpackedSalesOrderStatus;

        // Public WFO variable (Generate New If NULL)
        public string UnpackedSalesOrderStatus
        {
            get { return _wfoUnpackedSalesOrderStatus ?? (_wfoUnpackedSalesOrderStatus = WFOptions.RetrieveOptionValue("UnpackedSOStatus")); }
        }

        #endregion

        #endregion

        #region Constructors

        public StoneridgeCartonMaintenance(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeCartonMaintenance()
            : base()
        {
            InitializeComponent();
        }

        #endregion

        #region Initialize

        protected override void Initialize()
        {
            _StoneridgePrinting = new StoneridgePrinting(UserSession);

            _StoneridgePrinting.PrintXrefPatterns.Reset();
            _StoneridgePrinting.PrintXrefPatterns.Add((CustomRFSTables.PrintXrefType.EnvironmentID | CustomRFSTables.PrintXrefType.WorkflowID | CustomRFSTables.PrintXrefType.Version | CustomRFSTables.PrintXrefType.Branch |
                                                       CustomRFSTables.PrintXrefType.ContainerType | CustomRFSTables.PrintXrefType.Customer | CustomRFSTables.PrintXrefType.Item | CustomRFSTables.PrintXrefType.Location));
            _StoneridgePrinting.PrintXrefPatterns.Add((CustomRFSTables.PrintXrefType.EnvironmentID | CustomRFSTables.PrintXrefType.WorkflowID | CustomRFSTables.PrintXrefType.Version | CustomRFSTables.PrintXrefType.Branch |
                                                       CustomRFSTables.PrintXrefType.ContainerType | CustomRFSTables.PrintXrefType.Customer | CustomRFSTables.PrintXrefType.Item | CustomRFSTables.PrintXrefType.BlankLocation));
            _StoneridgePrinting.PrintXrefPatterns.Add((CustomRFSTables.PrintXrefType.EnvironmentID | CustomRFSTables.PrintXrefType.WorkflowID | CustomRFSTables.PrintXrefType.Version | CustomRFSTables.PrintXrefType.Branch |
                                                       CustomRFSTables.PrintXrefType.ContainerType | CustomRFSTables.PrintXrefType.Customer | CustomRFSTables.PrintXrefType.BlankItem | CustomRFSTables.PrintXrefType.BlankLocation));
            _StoneridgePrinting.PrintXrefPatterns.Add((CustomRFSTables.PrintXrefType.EnvironmentID | CustomRFSTables.PrintXrefType.WorkflowID | CustomRFSTables.PrintXrefType.Version | CustomRFSTables.PrintXrefType.Branch |
                                                       CustomRFSTables.PrintXrefType.ContainerType | CustomRFSTables.PrintXrefType.BlankCustomer | CustomRFSTables.PrintXrefType.BlankItem | CustomRFSTables.PrintXrefType.BlankLocation));

            _GenCartonPrintData = new GenCartonPrintData(DataAccess);
            _GenCartonPrintData.VariableNamePrefix = "ASR_";

            _addressBookTable = new F0101(UserSession.DataAccess, "");
            _branchTable = new F0006(UserSession.DataAccess, "");
            _salesOrderTable = new F4211(UserSession.DataAccess, "");
            _JDECartonTable = new CustomJDETables.F4620(UserSession.DataAccess, "");
            _RFSCartonTable = new F59RFCTL(UserSession.DataAccess, "");
            _warehouseConstantsTable = new F41001(UserSession.DataAccess, "");
            _itemMasterTable = new F4101(UserSession.DataAccess, "");
            _printersTable = new RFS_PrinterDefinition(UserSession.DataAccess, "");
            _vocabTable = new RFS_Vocabulary(UserSession.DataAccess, "");
            _itemBranchTable = new F4102(UserSession.DataAccess, "");
            _itemLocationTable = new F41021(UserSession.DataAccess, "");
            _uomConversionTable = new F41002(UserSession.DataAccess, "");
            _uomConversionTable2 = new F41003(UserSession.DataAccess, "");
            _lotTable = new F4108(UserSession.DataAccess, "");
            _dataManipulator = new RFS_DataManipulator(UserSession.DataAccess, "");
            _forceBranch = WFOptions.GetYesNoWFOptionValue("ForceBranch");
            _p4620Version = WFOptions.RetrieveOptionValue("P4620Version");
            _allowedStatus = WFOptions.RetrieveOptionValue("IncludeStatus").Split(new char[] {' '});
            _defaultDim = WFOptions.GetYesNoWFOptionValue("DefaultDim");
            _defaultWeight = WFOptions.GetYesNoWFOptionValue("DefaultWeight");
            _defaultUOM = WFOptions.RetrieveOptionValue("DefaultUOM");
            try
            {
                _maxLabels = int.Parse(WFOptions.RetrieveOptionValue("MaxLabels"));
            }
            catch
            {
                _maxLabels = 10;
            }

            try
            {
                _maxItemsPerLabel = int.Parse(WFOptions.RetrieveOptionValue("MaxItemsPerLabel"));
            }
            catch
            {
                _maxItemsPerLabel = 10;
            }

            _serialNumberType = WFOptions.RetrieveOptionValue("SerialNumberType");
            createPrintItemTable();
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            if (_GenCartonPrintData != null)
            {
                _GenCartonPrintData.Dispose();
            }

            if (_StoneridgePrinting != null)
            {
                _StoneridgePrinting.Dispose();
            }

            ScreenMap.ClearAllWorkflows();
            base.Dispose();
        }

        #endregion

        #region BranchPlant

        #region _collectBranch_EnterValidationEvent

        private void _collectBranch_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateBranch(e.UserInput.ToString().Trim());
        }

        #endregion

        #region validateBranch

        private void validateBranch(string branch)
        {
            //Query F0006 and F41001 to validate that the branch has open SO Lines. 
            if (!_branchTable.ValidateBranchWithSalesOrders(branch, _allowedStatus, null, UserSession.UserId, "0"))
            {
                if (!_branchTable.FetchBranchPlant(branch))
                {
                    throw new RFS_Exception("INVALIDBRANCH");
                }
                else
                {
                    throw new RFS_Exception("NoOpenOrders");
                }
            }

            _collectBranch.ScreenInput = _branchTable.MCMCU;
            _warehouseConstantsTable.DeterminePrimaryItem(_collectBranch.ScreenInput);
            Next(_collectPickMode);
        }

        #endregion

        #region _branch_List_LoadEvent

        private void _branch_List_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _branchTable.DisplayBranchesWithSalesOrders(_allowedStatus, null, UserSession.UserId, "0");
        }

        #endregion

        private void _branch_List_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _branchTable.UpdateCurrentRow();
            validateBranch(_branchTable.MCMCU);
        }

        #endregion

        #region PickMode

        #region _collectPickMode_EnterValidationEvent

        private void _collectPickMode_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validatePickMode(e.UserInput.ToString());
        }

        #endregion

        #region validatePickMode

        private void validatePickMode(string pickMode)
        {
            _collectPickMode.ScreenInput = pickMode;
            if (_collectPickMode.ScreenInput.Length == 0)
            {
                throw new RFS_Exception("INVALIDPICKMODE");
            }
            else
            {
                try
                {
                    int i = Convert.ToInt16(_collectPickMode.ScreenInput);
                    if ((i < 1) || (i > 2))
                    {
                        throw new RFS_Exception("INVALIDPICKMODE");
                    } // end if
                    else
                    {
                        decisionTable1();
                    } // end else
                } // end try
                catch
                {
                    throw new RFS_Exception("INVALIDPICKMODE");
                } // end catch
            } // end else
        }

        #endregion

        #endregion

        #region PickSlip

        #region _collectPickSlip_EnterValidationEvent

        private void _collectPickSlip_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validatePickSlip(e.UserInput.ToString().Trim());
        }

        #endregion

        #region validatePickSlip

        private void validatePickSlip(string pickSlip)
        {
            try
            {
                double dPickSlip = double.Parse(pickSlip);
            }
            catch
            {
                throw new RFS_Exception("NotNumeric");
            }

            if (!_salesOrderTable.ValidPickSlip(_collectBranch.ScreenInput, pickSlip, _allowedStatus, null, UserSession.UserId, "0"))
            {
                if (!_salesOrderTable.ValidPickSlip(_collectBranch.ScreenInput, pickSlip, null, null, UserSession.UserId, "0"))
                {
                    throw new RFS_Exception("INVALIDPickSlip");
                }
                else
                {
                    throw new RFS_Exception("NoOpenLines");
                }
            }

            _collectPickSlip.ScreenInput = _salesOrderTable.SDPSN.ToString();
            Next(_collectPrinter);
        }

        #endregion

        #region _pickSlipList_LoadEvent

        private void _pickSlipList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _salesOrderTable.FetchPickSlips(_collectBranch.ScreenInput, _allowedStatus, null, UserSession.UserId, "0");
        }

        #endregion

        #region _pickSlipList_EnterValidationEvent

        private void _pickSlipList_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _salesOrderTable.UpdateCurrentRow();
            validatePickSlip(_salesOrderTable.SDPSN.ToString());
        }

        #endregion

        #endregion

        #region Shipment

        #region validateShipment

        private void _collectShipment_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateShipment(e.UserInput.ToString().Trim());
        }

        #endregion

        #region validateShipment

        private void validateShipment(string shipment)
        {
            try
            {
                double dShipment = double.Parse(shipment);
            }
            catch
            {
                throw new RFS_Exception("NotNumeric");
            }

            if (!_salesOrderTable.ValidShipment(_collectBranch.ScreenInput, shipment, _allowedStatus, null, UserSession.UserId, "0"))
            {
                if (!_salesOrderTable.ValidShipment(_collectBranch.ScreenInput, shipment, null, null, UserSession.UserId, "0"))
                {
                    throw new RFS_Exception("INVALIDShipment");
                }
                else
                {
                    throw new RFS_Exception("NoOpenLines");
                }
            }

            _collectShipment.ScreenInput = _salesOrderTable.SDSHPN.ToString();
            Next(_collectPrinter);
        }

        #endregion

        #region _shipmentList_LoadEvent

        private void _shipmentList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _salesOrderTable.FetchShipments(_collectBranch.ScreenInput, _allowedStatus, null, UserSession.UserId, "0");
        }

        #endregion

        #region _shipmentList_EnterValidationEvent

        private void _shipmentList_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _salesOrderTable.UpdateCurrentRow();
            validateShipment(_salesOrderTable.SDSHPN.ToString());
        }

        #endregion

        #endregion

        #region decisionTable1

        private void decisionTable1()
        {
            switch (_collectPickMode.ScreenInput.Trim())
            {
                case "1": // by PickSlip
                    Next(_collectPickSlip);
                    break;
                case "2": // by Shipment
                    Next(_collectShipment);
                    break;
            } // end switch
        }

        #endregion

        #region decisionTable3

        private void decisionTable3()
        {
            switch (_sequence)
            {
                case BuildSequence.AddItem:
                    ScreenMap.Add("Title", RetrieveVoc("AddItem"));
                    RestartLoop(_collectItem);
                    break;
                case BuildSequence.AddCarton:
                    ScreenMap.Add("Title", RetrieveVoc("AddCarton"));
                    RestartLoop(_collectCarton);
                    break;
                //case BuildSequence.RemoveItem:
                //    ScreenMap.Add("Title", RetrieveVoc("RemoveItem"));
                //    RestartLoop(_collectItem);
                //    break;
                case BuildSequence.RemoveCarton:
                    ScreenMap.Add("Title", RetrieveVoc("RemoveCarton"));
                    RestartLoop(_collectCarton);
                    break;
                case BuildSequence.CartonTransfer:
                    ScreenMap.Add("Title", RetrieveVoc("CartonTransfer"));
                    RestartLoop(_collectCarton);
                    break;
                case BuildSequence.ItemTransfer:
                    ScreenMap.Add("Title", RetrieveVoc("ItemTransfer"));
                    RestartLoop(_collectItem);
                    break;
                case BuildSequence.DeletePallet:
                    ScreenMap.Add("Title", RetrieveVoc("DeletePallet"));
                    RestartLoop(_collectFromCarton);
                    break;
                case BuildSequence.WeightsAndDimensions:
                    ScreenMap.Add("Title", RetrieveVoc("WeightsAndDimensions"));
                    RestartLoop(_collectCarton);
                    break;
                case BuildSequence.MergePallet:
                    ScreenMap.Add("Title", RetrieveVoc("MergePallet"));
                    RestartLoop(_collectFromCarton);
                    break;
            } // end switch
        }

        #endregion

        #region Printer

        #region _collectPrinter_EnterValidationEvent

        private void _collectPrinter_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validatePrinter(e.UserInput.ToString().Trim());
        }

        #endregion

        #region validatePrinter

        private void validatePrinter(string printer)
        {
            if (!String.IsNullOrWhiteSpace(printer))
            {
                if (_StoneridgePrinting.ValidatePrinter(printer))
                {
                    _collectPrinter.ScreenInput = _StoneridgePrinting.PrinterName;
                    _printerAddress = _StoneridgePrinting.PrinterAddress;
                }
                else
                {
                    throw new RFS_Exception(_StoneridgePrinting.ErrorMessageVocabulary);
                }
            }
            else
            {
                _collectPrinter.ScreenInput = "";
            }
        }

        #endregion

        #region _printerList_LoadEvent

        private void _printerList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            //All the parameters for this should already be setup in the Load event for the 
            //CollectPrinter screen (xref params)
            _StoneridgePrinting.RetrievePrinters();
        }

        #endregion

        #region _printerList_EnterValidationEvent

        private void _printerList_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _printersTable.UpdateCurrentRow();
            validatePrinter(_printersTable.PrinterName);
        }

        #endregion

        #region _collectPrinter_LoadEvent

        private void _collectPrinter_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _StoneridgePrinting.ResetAll(); //note while we have SO information, we still do NOT have a specific item yet
            _StoneridgePrinting.Branch = _collectBranch.ScreenInput.Trim();
            _StoneridgePrinting.Customer = _salesOrderTable.SDAN8.ToString();
            _StoneridgePrinting.Item = _salesOrderTable.SDSHAN.ToString();
            _StoneridgePrinting.ContainerType = "M";

            //Below we check if the PrinterName is blank, if so, it means no records were found for the 
            //ContainerType, so try another (we use "PrinterName" as once we have a valid value it stores
            //it and doesn't try to keep requirying like DefaultPrinter does)
            if (String.IsNullOrWhiteSpace(_StoneridgePrinting.PrinterName))
            {
                _StoneridgePrinting.ContainerType = "X";
            }

            if (String.IsNullOrWhiteSpace(_StoneridgePrinting.PrinterName))
            {
                _StoneridgePrinting.ContainerType = "C";
            }

            if (!String.IsNullOrWhiteSpace(_StoneridgePrinting.PrinterName))
            {
                ScreenHandler.DefaultText = _StoneridgePrinting.PrinterName;
            }
            else
            {
                Error(_StoneridgePrinting.ErrorMessageVocabulary);
            }
        }

        #endregion

        #endregion

        #region To Carton

        private void _collectToCarton_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateToCarton(e.UserInput.ToString().Trim().ToUpper());
        }

        private void validateToCarton(string carton)
        {
            if (!this._JDECartonTable.GetCarton(_collectBranch.ScreenInput, carton))
            {
                if (!_RFSCartonTable.GetCarton(carton))
                {
                    throw new RFS_Exception("Carton");
                }

                if (_sequence == BuildSequence.MergePallet)
                {
                    throw new RFS_Exception("NotPallet");
                }

                if ((_forceBranch) && (_RFSCartonTable.CLMCU.Trim().ToUpper() != _collectBranch.ScreenInput.Trim().ToUpper()))
                {
                    throw new RFS_Exception("BranchMismatch");
                }

                _toCartonId = double.MinValue;
                _toCartonCode = _RFSCartonTable.CLEQTY;
                _toCartonSerial = _RFSCartonTable.CLSNTYPE.Trim() != "3" ? carton : "";
            }
            else
            {
                if (_salesOrderTable.SDSHPN != _JDECartonTable.CDSHPN)
                {
                    throw new RFS_Exception("CartonShipment");
                }

                if (_sequence == BuildSequence.MergePallet)
                {
                    if (!isPallet())
                        throw new RFS_Exception("NotPallet");
                }

                _toCartonId = _JDECartonTable.CDCRID;
                _toCartonCode = _JDECartonTable.CDEQTY;
                _toCartonSerial = _JDECartonTable.CDCTSN;
            }

            ScreenMap.Add("ToCarton", carton);
            _collectToCarton.ScreenInput = carton;
            toCartonNav();
        }

        private void toCartonNav()
        {
            switch (_sequence)
            {
                case BuildSequence.AddItem:
                case BuildSequence.ItemTransfer:
                    Next(_collectItem);
                    break;
                case BuildSequence.AddCarton:
                case BuildSequence.CartonTransfer:
                    Next(_collectCarton);
                    break;
                case BuildSequence.MergePallet:
                    SendToErp();
                    RestartLoop(_collectFromCarton);
                    break;
            } // end switch
        }

        private bool _collectToCarton_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            _RFSCartonTable.GetAvailableCartons(_collectBranch.ScreenInput);
            return true;
        }

        private void _toCarton_List_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _JDECartonTable.UpdateCurrentRow();
            //validateToCarton(_JDECartonTable.CDCEQT);
            validateToCarton(_JDECartonTable.CDURRF);
        }

        private void _toCarton_List_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _JDECartonTable.GetContainersOrPalletsInShipment(_salesOrderTable.SDSHPN);
        }

        private void _RFSCarton_List_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _RFSCartonTable.UpdateCurrentRow();
            validateToCarton(_RFSCartonTable.CLCEQT);
        }

        #endregion

        private void _collectSequence_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            clearTransaction();
            try
            {
                _sequence = (BuildSequence) int.Parse(e.UserInput);
            }
            catch
            {
                throw new RFS_Exception("Sequence");
            }

            if ((int) _sequence > 9 || (int) _sequence < 1)
            {
                throw new RFS_Exception("Sequence");
            }

            decisionTable2();
        }

        #region decisionTable2

        private void decisionTable2()
        {
            switch (_sequence)
            {
                case BuildSequence.AddItem:
                    ScreenMap.Add("Title", RetrieveVoc("AddItem"));
                    Next(_collectToCarton);
                    break;
                case BuildSequence.AddCarton:
                    ScreenMap.Add("Title", RetrieveVoc("AddCarton"));
                    Next(_collectToCarton);
                    break;
                case BuildSequence.RemoveCarton:
                    ScreenMap.Add("Title", RetrieveVoc("RemoveCarton"));
                    Next(_collectFromCarton);
                    break;
                case BuildSequence.CartonTransfer:
                    ScreenMap.Add("Title", RetrieveVoc("CartonTransfer"));
                    Next(_collectFromCarton);
                    break;
                case BuildSequence.ItemTransfer:
                    ScreenMap.Add("Title", RetrieveVoc("ItemTransfer"));
                    Next(_collectFromCarton);
                    break;
                case BuildSequence.DeletePallet:
                    ScreenMap.Add("Title", RetrieveVoc("DeletePallet"));
                    Next(_collectFromCarton);
                    break;
                case BuildSequence.WeightsAndDimensions:
                    ScreenMap.Add("Title", RetrieveVoc("WeightsAndDimensions"));
                    Next(_collectCarton);
                    break;
                case BuildSequence.MergePallet:
                    ScreenMap.Add("Title", RetrieveVoc("MergePallet"));
                    Next(_collectFromCarton);
                    break;
                case BuildSequence.UnpackPallet:
                    ScreenMap.Add("Title", RetrieveVoc("Unpack"));
                    Next(CollectUnpackPalletNumber);
                    break;
            } // end switch
        }

        #endregion

        #region RetrieveVoc

        //
        // load column headers in the user's native language
        //
        private string RetrieveVoc(string DataDictionaryItem)
        {
            if (this._vocabTable.RetrieveVocabularyOverride(DataDictionaryItem,
                    "S",
                    UserSession.ScreenHandler.Environment.ToString(),
                    UserSession.ScreenHandler.Language.ToString(),
                    this.WorkflowID,
                    this.ScreenHandler.CurrentScreen.ToString()))
            {
                return (this._vocabTable.DisplayText);
            }
            else
            {
                return (DataDictionaryItem);
            }
        }

        #endregion

        #region Carton

        private void _collectCarton_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateCarton(e.UserInput.ToString().Trim().ToUpper());
        }

        private void validateCarton(string carton)
        {
            if (!this._JDECartonTable.GetCarton(_collectBranch.ScreenInput, carton))
            {
                throw new RFS_Exception("Carton");
            }
            else
            {
                if (_salesOrderTable.SDSHPN != _JDECartonTable.CDSHPN)
                {
                    throw new RFS_Exception("CartonShipment");
                }

                //Throw error if From Carton and To Carton are the same
                if (_JDECartonTable.CDCRID == _toCartonId)
                {
                    throw new RFS_Exception("Carton");
                }

                _cartonId = _JDECartonTable.CDCRID;
                _collectCarton.ScreenInput = carton;
            }

            ScreenHandler.ScreenMap.Add("Carton", carton);

            switch (_sequence)
            {
                case BuildSequence.AddCarton:
                    if (_JDECartonTable.CDCROS != 0)
                        throw new RFS_Exception("CartonOnPallet");
                    //if (_toCartonCode == _JDECartonTable.CDCEQT)
                    if (_toCartonCode == _JDECartonTable.CDURRF)
                        throw new RFS_Exception("SameAsPallet");
                    SendToErp();
                    break;
                case BuildSequence.RemoveCarton:
                    if (_JDECartonTable.CDCROS != _fromCartonId)
                        throw new RFS_Exception("CartonNotOnPallet");
                    Next(_confirmRemoveCarton);
                    break;
                case BuildSequence.CartonTransfer:
                    if (_JDECartonTable.CDCROS != _fromCartonId)
                        throw new RFS_Exception("CartonNotOnPallet");
                    SendToErp();
                    break;
                case BuildSequence.WeightsAndDimensions:
                    if (!_JDECartonTable.GetCarton(_collectBranch.ScreenInput, _cartonId))
                    {
                        throw new RFS_Exception("InvalidButton");
                        break;
                    }

                    ScreenMap.Add("ContainerType", _JDECartonTable.CDEQTY);
                    //ScreenMap.Add("PalletId", _JDECartonTable.CDCEQT.ToString());
                    ScreenMap.Add("PalletId", _JDECartonTable.CDURRF.ToString());
                    Next(_collectWeight);
                    break;
            } // end switch
        }

        #endregion

        #region Send To Erp

        private void SendToErp()
        {
            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            switch (_sequence)
            {
                case BuildSequence.AddItem:
                    addItem();
                    break;
                case BuildSequence.AddCarton:
                    addCarton();
                    break;
                //case BuildSequence.RemoveItem:
                //    removeItem();
                //    break;
                case BuildSequence.RemoveCarton:
                    removeCarton();
                    break;
                case BuildSequence.CartonTransfer:
                    cartonTransfer();
                    break;
                case BuildSequence.ItemTransfer:
                    itemTransfer();
                    break;
                case BuildSequence.MergePallet:
                    mergePallet();
                    break;
                case BuildSequence.DeletePallet:
                    deletePallet();
                    decisionTable3();
                    return;
                    break;
            } // end switch

            decisionTable3();
        }

        private void mergePallet()
        {
            _maintainCarton = new ICS.ERPI.JDE.MaintainCarton();

            Transactions.Add(_maintainCarton);

            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            _maintainCarton.Initialize();
            setSoldTo();
            setShipTo();
            _maintainCarton.SerialNumberType = _serialNumberType;
            _maintainCarton.RFUserID = UserSession.CurrentUser.LoginName;
            _maintainCarton.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
            _maintainCarton.ShipmentNumber = _salesOrderTable.SDSHPN.ToString();
            _maintainCarton.BranchPlant = _collectBranch.ScreenInput.Trim();
            _maintainCarton.CompletedCartonID = _cartonId.ToString();
            _maintainCarton.FromCartonID = _fromCartonId.ToString();
            if (_toCartonId <= 0)
            {
                _maintainCarton.CartonCode = _toCartonCode.Trim();
                _maintainCarton.CreateNewCarton = "1";
                _maintainCarton.ToCartonID = string.Empty;
                _maintainCarton.CustomerCartonId = _collectToCarton.ScreenInput.Trim();
            }
            else
            {
                _maintainCarton.CartonCode = _toCartonCode.Trim();
                _maintainCarton.CreateNewCarton = string.Empty;
                _maintainCarton.ToCartonID = _toCartonId.ToString();
            }

            _maintainCarton.P4620Version = _p4620Version;
            string mode = "1";
            if (_JDECartonTable.GetCartonsOnPallet(_fromCartonId))
            {
                foreach (DataRow dr in _JDECartonTable.Rows)
                {
                    _JDECartonTable.CurrentRow = dr;
                    _maintainCarton.CompletedCartonID = _JDECartonTable.CDCRID.ToString();
                    _maintainCarton.Mode = mode;
                    mode = "P1";
                    _maintainCarton.AddToBoundaryWithData();
                }
            }

            _maintainCarton.CompletedCartonID = string.Empty;
            if (_JDECartonTable.GetItemsOnPallet(_collectBranch.ScreenInput, _fromCartonId))
            {
                foreach (DataRow dr in _JDECartonTable.Rows)
                {
                    _primaryItem = _itemMasterTable.FetchPrimaryItemNumber(_JDECartonTable.CDITM, _warehouseConstantsTable._DesignatedPrimaryItemNumber);
                    _itemMasterTable.RetrieveItem(_JDECartonTable.CDMCU, _primaryItem, _warehouseConstantsTable._DesignatedPrimaryItemNumber.ToString());
                    setItem();
                    _maintainCarton.LotNumber = _JDECartonTable.CDLOT.Trim();
                    _maintainCarton.Quantity = _JDECartonTable.CDTQTY.ToString();
                    _maintainCarton.UnitOfMeasure = _JDECartonTable.CDUOM.Trim();
                    _maintainCarton.Mode = mode;
                    mode = "P1";
                    _maintainCarton.AddToBoundaryWithData();
                }
            }

            _maintainCarton.Mode = "2";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "3";

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();
        }

        private void setSoldTo()
        {
            _maintainCarton.CustSoldTo = _salesOrderTable.SDAN8.ToString();
            _addressBookTable.RetrieveAddress(_salesOrderTable.SDAN8.ToString());
            _maintainCarton.CustSoldToDescr = _addressBookTable.ABALPH.Trim();
        }

        private void setShipTo()
        {
            _maintainCarton.CustShipTo = _salesOrderTable.SDSHAN.ToString();
            _addressBookTable.RetrieveAddress(_salesOrderTable.SDSHAN.ToString());
            _maintainCarton.CustShipToDescr = _addressBookTable.ABALPH.Trim();
        }

        private void deletePallet()
        {
            _maintainCarton = new ICS.ERPI.JDE.MaintainCarton();

            Transactions.Add(_maintainCarton);

            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            _maintainCarton.Initialize();
            string mode = "1";
            if (_JDECartonTable.GetCartonsOnPallet(_fromCartonId))
            {
                foreach (DataRow dr in _JDECartonTable.Rows)
                {
                    setSoldTo();
                    setShipTo();
                    _JDECartonTable.CurrentRow = dr;
                    _maintainCarton.RFUserID = UserSession.CurrentUser.LoginName;
                    _maintainCarton.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
                    _maintainCarton.ShipmentNumber = _salesOrderTable.SDSHPN.ToString();
                    _maintainCarton.BranchPlant = _collectBranch.ScreenInput.Trim();
                    _maintainCarton.CompletedCartonID = _JDECartonTable.CDCRID.ToString();
                    _maintainCarton.FromCartonID = _fromCartonId.ToString();
                    _maintainCarton.CartonCode = string.Empty;
                    _maintainCarton.CreateNewCarton = string.Empty;
                    _maintainCarton.ToCartonID = string.Empty;
                    _maintainCarton.P4620Version = _p4620Version;
                    _maintainCarton.Mode = mode;
                    mode = "P1";
                    _maintainCarton.AddToBoundaryWithData();
                }
            }

            if (_JDECartonTable.GetItemsOnPallet(_collectBranch.ScreenInput, _fromCartonId))
            {
                foreach (DataRow dr in _JDECartonTable.Rows)
                {
                    setSoldTo();
                    setShipTo();
                    _JDECartonTable.CurrentRow = dr;
                    _maintainCarton.RFUserID = UserSession.CurrentUser.LoginName;
                    _maintainCarton.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
                    _maintainCarton.ShipmentNumber = _salesOrderTable.SDSHPN.ToString();
                    _maintainCarton.BranchPlant = _collectBranch.ScreenInput.Trim();
                    _maintainCarton.CompletedCartonID = string.Empty;
                    _maintainCarton.FromCartonID = _fromCartonId.ToString();
                    _maintainCarton.CartonCode = string.Empty;
                    _maintainCarton.CreateNewCarton = string.Empty;
                    _maintainCarton.ToCartonID = string.Empty;
                    _primaryItem = _itemMasterTable.FetchPrimaryItemNumber(_JDECartonTable.CDITM, _warehouseConstantsTable._DesignatedPrimaryItemNumber);
                    _itemMasterTable.RetrieveItem(_JDECartonTable.CDMCU, _primaryItem, _warehouseConstantsTable._DesignatedPrimaryItemNumber.ToString());
                    setItem();
                    _maintainCarton.LotNumber = _JDECartonTable.CDLOT.Trim();
                    _maintainCarton.Quantity = _JDECartonTable.CDTQTY.ToString();
                    _maintainCarton.UnitOfMeasure = _JDECartonTable.CDUOM.Trim();
                    _maintainCarton.P4620Version = _p4620Version;
                    _maintainCarton.Mode = mode;
                    mode = "P1";
                    _maintainCarton.AddToBoundaryWithData();
                }
            }

            _maintainCarton.Mode = "2";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "3";

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();
        }

        private void removeCarton()
        {
            _maintainCarton = new ICS.ERPI.JDE.MaintainCarton();

            Transactions.Add(_maintainCarton);

            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            _maintainCarton.Initialize();
            setSoldTo();
            setShipTo();
            _maintainCarton.RFUserID = UserSession.CurrentUser.LoginName;
            _maintainCarton.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
            _maintainCarton.ShipmentNumber = _salesOrderTable.SDSHPN.ToString();
            _maintainCarton.BranchPlant = _collectBranch.ScreenInput.Trim();
            _maintainCarton.CompletedCartonID = _cartonId.ToString();
            _maintainCarton.FromCartonID = _fromCartonId.ToString();
            _maintainCarton.CartonCode = string.Empty;
            _maintainCarton.CreateNewCarton = string.Empty;
            _maintainCarton.ToCartonID = string.Empty;
            _maintainCarton.P4620Version = _p4620Version;
            _maintainCarton.Mode = "1";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "2";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "3";

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();
        }

        private void setItem()
        {
            _maintainCarton.ItemNumber = _primaryItem.Trim();
            _maintainCarton.PrimaryItemNumber = _primaryItem.Trim();
            _maintainCarton.SecondItemNumber = _itemMasterTable.IMLITM.Trim();
            _maintainCarton.ShortItemNumber = _itemMasterTable.IMITM.ToString();
            _maintainCarton.ThirdItemNumber = _itemMasterTable.IMAITM.Trim();
            _maintainCarton.ItemDescription1 = _itemMasterTable.IMDSC1.Trim();
            _maintainCarton.ItemDescription2 = _itemMasterTable.IMDSC2.Trim();
            if (_itemBranchTable.ValidateItemBranch(_salesOrderTable.SDMCU.Trim(), _itemMasterTable.IMITM))
            {
                _maintainCarton.ItemDimGroup = _itemBranchTable.IBPRP6.Trim();
                _maintainCarton.ItemLotProcType = _itemBranchTable.IBSRCE;
            }
        }

        private void removeItem()
        {
            _maintainCarton = new ICS.ERPI.JDE.MaintainCarton();

            Transactions.Add(_maintainCarton);

            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            _maintainCarton.Initialize();
            setSoldTo();
            setShipTo();
            setItem();
            _maintainCarton.RFUserID = UserSession.CurrentUser.LoginName;
            _maintainCarton.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
            _maintainCarton.ShipmentNumber = _salesOrderTable.SDSHPN.ToString();
            _maintainCarton.BranchPlant = _collectBranch.ScreenInput.Trim();
            _maintainCarton.LotNumber = _collectLot.ScreenInput.Trim();
            _maintainCarton.Quantity = _quantityInPrimary.ToString();
            _maintainCarton.UnitOfMeasure = _itemMasterTable.IMUOM1.Trim();
            setUOMtoCartonUOM();
            _maintainCarton.CompletedCartonID = string.Empty;
            _maintainCarton.FromCartonID = _fromCartonId.ToString();
            _maintainCarton.CartonCode = string.Empty;
            _maintainCarton.CreateNewCarton = string.Empty;
            _maintainCarton.ToCartonID = string.Empty;
            _maintainCarton.P4620Version = _p4620Version;
            _maintainCarton.Mode = "1";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "2";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "3";

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();
        }

        private void cartonTransfer()
        {
            _maintainCarton = new ICS.ERPI.JDE.MaintainCarton();

            Transactions.Add(_maintainCarton);

            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            _maintainCarton.Initialize();
            setSoldTo();
            setShipTo();
            _maintainCarton.SerialNumberType = _serialNumberType;
            _maintainCarton.RFUserID = UserSession.CurrentUser.LoginName;
            _maintainCarton.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
            _maintainCarton.ShipmentNumber = _salesOrderTable.SDSHPN.ToString();
            _maintainCarton.BranchPlant = _collectBranch.ScreenInput.Trim();
            _maintainCarton.CompletedCartonID = _cartonId.ToString();
            _maintainCarton.FromCartonID = _fromCartonId.ToString();
            _maintainCarton.CartonSerialNumber = _toCartonSerial;
            if (_toCartonId <= 0)
            {
                _maintainCarton.CartonCode = _toCartonCode.Trim();
                _maintainCarton.CreateNewCarton = "1";
                _maintainCarton.ToCartonID = string.Empty;
                _maintainCarton.CustomerCartonId = _collectToCarton.ScreenInput.Trim();
            }
            else
            {
                _maintainCarton.CartonCode = _toCartonCode.Trim();
                _maintainCarton.CreateNewCarton = string.Empty;
                _maintainCarton.ToCartonID = _toCartonId.ToString();
            }

            _maintainCarton.P4620Version = _p4620Version;
            _maintainCarton.Mode = "1";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "2";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "3";

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();
            if (_toCartonId <= 0)
            {
                if (this._JDECartonTable.GetCarton(_collectBranch.ScreenInput, _collectToCarton.ScreenInput.Trim()))
                {
                    _toCartonId = _JDECartonTable.CDCRID;
                    _toCartonCode = _JDECartonTable.CDEQTY;
                    _toCartonSerial = _JDECartonTable.CDCTSN;
                }
            }
        }

        private void itemTransfer()
        {
            _maintainCarton = new ICS.ERPI.JDE.MaintainCarton();

            Transactions.Add(_maintainCarton);

            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            _maintainCarton.Initialize();
            double qtyRemaining = ConvertToUOMQty(_itemMasterTable.IMUOM1, _JDECartonTable.CDUOM, _quantityInPrimary);
            bool firstPart = true;
            if (!this._JDECartonTable.GetLotInCarton(
                    _collectBranch.ScreenInput,
                    _fromCartonId,
                    _itemMasterTable.IMITM,
                    _collectLot.ScreenInput.Trim().Length == 0 ? null : _collectLot.ScreenInput.Trim()))
            {
                throw new RFS_Exception("Carton");
            }

            _maintainCarton.SerialNumberType = _serialNumberType;
            foreach (DataRow dr in _JDECartonTable.Rows)
            {
                _JDECartonTable.CurrentRow = dr;
                setSoldTo();
                setShipTo();
                setItem();
                _maintainCarton.OrderCompany = _JDECartonTable.CDKCOO.Trim();
                _maintainCarton.OrderLineNumber = _JDECartonTable.CDLNID.ToString();
                _maintainCarton.OrderNumber = _JDECartonTable.CDDOCO.ToString();
                _maintainCarton.OrderSuffix = _JDECartonTable.CDSFXO.Trim();
                _maintainCarton.OrderType = _JDECartonTable.CDDCTO.Trim();
                _maintainCarton.RFUserID = UserSession.CurrentUser.LoginName;
                _maintainCarton.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
                _maintainCarton.ShipmentNumber = _salesOrderTable.SDSHPN.ToString();
                _maintainCarton.BranchPlant = _collectBranch.ScreenInput.Trim();
                _maintainCarton.LotNumber = _collectLot.ScreenInput.Trim();
                if (qtyRemaining < _JDECartonTable.CDTQTY)
                {
                    _maintainCarton.Quantity = qtyRemaining.ToString();
                    qtyRemaining = 0;
                }
                else
                {
                    _maintainCarton.Quantity = _JDECartonTable.CDTQTY.ToString();
                    qtyRemaining -= _JDECartonTable.CDTQTY;
                }

                _maintainCarton.UnitOfMeasure = _JDECartonTable.CDUOM.Trim();
                _maintainCarton.CompletedCartonID = string.Empty;
                _maintainCarton.FromCartonID = _fromCartonId.ToString();
                _maintainCarton.CartonSerialNumber = _toCartonSerial;
                if (_toCartonId <= 0)
                {
                    _maintainCarton.CartonCode = _toCartonCode.Trim();
                    _maintainCarton.CreateNewCarton = "1";
                    _maintainCarton.ToCartonID = string.Empty;
                    _maintainCarton.CustomerCartonId = _collectToCarton.ScreenInput.Trim();
                }
                else
                {
                    _maintainCarton.CartonCode = _toCartonCode.Trim();
                    _maintainCarton.CreateNewCarton = string.Empty;
                    _maintainCarton.ToCartonID = _toCartonId.ToString();
                }

                _maintainCarton.P4620Version = _p4620Version;
                if (firstPart)
                {
                    _maintainCarton.Mode = "1";
                }
                else
                {
                    _maintainCarton.Mode = "P1";
                }

                _maintainCarton.AddToBoundaryWithData();
                firstPart = false;
                if (qtyRemaining == 0)
                {
                    break;
                }
            }

            _maintainCarton.Mode = "2";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "3";

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();
            if (_toCartonId <= 0)
            {
                if (this._JDECartonTable.GetCarton(_collectBranch.ScreenInput, _collectToCarton.ScreenInput.Trim()))
                {
                    _toCartonId = _JDECartonTable.CDCRID;
                    _toCartonCode = _JDECartonTable.CDEQTY;
                    _toCartonSerial = _JDECartonTable.CDCTSN;
                }
            }
        }

        private void setUOMtoCartonUOM()
        {
            // Must send the Quantity in UOM on F4620
            if (this._JDECartonTable.GetLotInCarton(_collectBranch.ScreenInput,
                    _fromCartonId,
                    _itemMasterTable.IMITM,
                    _collectLot.ScreenInput.Trim().Length == 0 ? null : _collectLot.ScreenInput.Trim()))
            {
                _maintainCarton.Quantity = ConvertToUOMQty(_itemMasterTable.IMUOM1.Trim(),
                    _JDECartonTable.CDUOM,
                    _quantityInPrimary).ToString();
                _maintainCarton.UnitOfMeasure = _JDECartonTable.CDUOM.Trim();
            }
        }

        private void addCarton()
        {
            _maintainCarton = new ICS.ERPI.JDE.MaintainCarton();

            Transactions.Add(_maintainCarton);

            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            _maintainCarton.Initialize();
            setSoldTo();
            setShipTo();
            _maintainCarton.SerialNumberType = _serialNumberType;
            _maintainCarton.RFUserID = UserSession.CurrentUser.LoginName;
            _maintainCarton.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
            _maintainCarton.ShipmentNumber = _salesOrderTable.SDSHPN.ToString();
            _maintainCarton.BranchPlant = _collectBranch.ScreenInput.Trim();
            _maintainCarton.CompletedCartonID = _cartonId.ToString();
            _maintainCarton.CartonSerialNumber = _toCartonSerial;
            if (_toCartonId <= 0)
            {
                _maintainCarton.CartonCode = _toCartonCode.Trim();
                _maintainCarton.CreateNewCarton = "1";
                _maintainCarton.ToCartonID = string.Empty;
                _maintainCarton.CustomerCartonId = _collectToCarton.ScreenInput.Trim();
            }
            else
            {
                _maintainCarton.CartonCode = _toCartonCode.Trim();
                _maintainCarton.CreateNewCarton = string.Empty;
                _maintainCarton.ToCartonID = _toCartonId.ToString();
            }

            _maintainCarton.P4620Version = _p4620Version;
            _maintainCarton.Mode = "1";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "2";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "3";

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();
            if (_toCartonId <= 0)
            {
                if (this._JDECartonTable.GetCarton(_collectBranch.ScreenInput, _collectToCarton.ScreenInput.Trim()))
                {
                    _toCartonId = _JDECartonTable.CDCRID;
                    _toCartonCode = _JDECartonTable.CDEQTY;
                    _toCartonSerial = _JDECartonTable.CDCTSN;
                }
            }
        }

        private void addItem()
        {
            _maintainCarton = new ICS.ERPI.JDE.MaintainCarton();

            Transactions.Add(_maintainCarton);

            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            _maintainCarton.Initialize();
            double qtyRemaining = _quantityInPrimary;
            bool firstPart = true;
            _maintainCarton.SerialNumberType = _serialNumberType;
            foreach (DataRow dr in _salesOrderTable.Rows)
            {
                _salesOrderTable.CurrentRow = dr;
                setSoldTo();
                setShipTo();
                setItem();
                _maintainCarton.OrderCompany = _salesOrderTable.SDKCOO.Trim();
                _maintainCarton.OrderLineNumber = _salesOrderTable.SDLNID.ToString();
                _maintainCarton.OrderNumber = _salesOrderTable.SDDOCO.ToString();
                _maintainCarton.OrderSuffix = _salesOrderTable.SDSFXO.Trim();
                _maintainCarton.OrderType = _salesOrderTable.SDDCTO.Trim();
                _maintainCarton.RFUserID = UserSession.CurrentUser.LoginName;
                _maintainCarton.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
                _maintainCarton.ShipmentNumber = _salesOrderTable.SDSHPN.ToString();
                _maintainCarton.BranchPlant = _collectBranch.ScreenInput.Trim();
                _maintainCarton.LotNumber = _collectLot.ScreenInput.Trim();
                if (qtyRemaining < double.Parse(_salesOrderTable["OpenInPrimary"].ToString()))
                {
                    _maintainCarton.Quantity = qtyRemaining.ToString();
                    qtyRemaining = 0;
                }
                else
                {
                    _maintainCarton.Quantity = _salesOrderTable["OpenInPrimary"].ToString();
                    qtyRemaining -= double.Parse(_salesOrderTable["OpenInPrimary"].ToString());
                }

                _maintainCarton.UnitOfMeasure = _itemMasterTable.IMUOM1.Trim();
                _maintainCarton.CompletedCartonID = string.Empty;
                _maintainCarton.CartonSerialNumber = _toCartonSerial;
                if (_toCartonId <= 0)
                {
                    _maintainCarton.CartonCode = _toCartonCode.Trim();
                    _maintainCarton.CreateNewCarton = "1";
                    _maintainCarton.ToCartonID = string.Empty;
                    _maintainCarton.CustomerCartonId = _collectToCarton.ScreenInput.Trim();
                }
                else
                {
                    _maintainCarton.CartonCode = _toCartonCode.Trim();
                    _maintainCarton.CreateNewCarton = string.Empty;
                    _maintainCarton.ToCartonID = _toCartonId.ToString();
                }

                _maintainCarton.P4620Version = _p4620Version;
                if (firstPart)
                {
                    _maintainCarton.Mode = "1";
                }
                else
                {
                    _maintainCarton.Mode = "P1";
                }

                _maintainCarton.AddToBoundaryWithData();
                firstPart = false;
                if (qtyRemaining == 0)
                {
                    break;
                }
            }

            _maintainCarton.Mode = "2";
            _maintainCarton.AddToBoundaryWithData();
            _maintainCarton.Mode = "3";

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();
            if (_toCartonId <= 0)
            {
                if (this._JDECartonTable.GetCarton(_collectBranch.ScreenInput, _collectToCarton.ScreenInput.Trim()))
                {
                    _toCartonId = _JDECartonTable.CDCRID;
                    _toCartonCode = _JDECartonTable.CDEQTY;
                    _toCartonSerial = _JDECartonTable.CDCTSN;
                }
            }
        }

        #endregion

        #region From Carton

        private bool _collectFromCarton_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (_sequence)
            {
                case BuildSequence.MergePallet:
                    if (!_JDECartonTable.GetCarton(_collectBranch.ScreenInput, _toCartonId))
                    {
                        throw new RFS_Exception("InvalidButton");
                        break;
                    }

                    ScreenMap.Add("ContainerType", _JDECartonTable.CDEQTY);
                    //ScreenMap.Add("PalletId", _JDECartonTable.CDCEQT.ToString());
                    ScreenMap.Add("PalletId", _JDECartonTable.CDURRF.ToString());
                    Next(_collectWeight);
                    break;
                case BuildSequence.AddItem:
                case BuildSequence.ItemTransfer:
                case BuildSequence.AddCarton:
                case BuildSequence.RemoveCarton:
                case BuildSequence.CartonTransfer:
                    throw new RFS_Exception("InvalidButton");
                    break;
            } // end switch

            return true;
        }

        private void _collectFromCarton_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateFromCarton(e.UserInput.ToString().Trim().ToUpper());
        }

        private void validateFromCarton(string carton)
        {
            if (!this._JDECartonTable.GetCartonPallet(_collectBranch.ScreenInput, carton))
            {
                throw new RFS_Exception("Carton");
            }
            else
            {
                if (_salesOrderTable.SDSHPN != _JDECartonTable.CDSHPN)
                {
                    throw new RFS_Exception("CartonShipment");
                }

                _fromCartonId = _JDECartonTable.CDCRID;
                _collectFromCarton.ScreenInput = carton;
            }

            ScreenMap.Add("FromCarton", carton);

            switch (_sequence)
            {
                //case BuildSequence.RemoveItem:
                //    Next(_collectItem);
                //    break;
                case BuildSequence.ItemTransfer:
                    Next(_collectToCarton);
                    break;
                case BuildSequence.CartonTransfer:
                    if (!isPallet())
                        throw new RFS_Exception("NotPallet");
                    Next(_collectToCarton);
                    break;
                case BuildSequence.RemoveCarton:
                    if (!isPallet())
                        throw new RFS_Exception("NotPallet");
                    Next(_collectCarton);
                    break;
                case BuildSequence.DeletePallet:
                    if (!isPallet())
                        throw new RFS_Exception("NotPallet");
                    if (hasLoose())
                        throw new RFS_Exception("LooseItems");
                    Next(_confirmDeletePallet);
                    break;
                case BuildSequence.MergePallet:
                    if (!isPallet())
                        throw new RFS_Exception("NotPallet");
                    Next(_collectToCarton);
                    break;
            } // end switch
        }

        #endregion

        private bool hasLoose()
        {
            F4620 palletTable = new F4620(UserSession.DataAccess, "LooseItems");
            return palletTable.GetItemsOnPallet(_collectBranch.ScreenInput, _JDECartonTable.CDCRID);
        }

        private bool isPallet()
        {
            F4620 palletTable = new F4620(UserSession.DataAccess, "Pallet");
            return palletTable.GetCartonsOnPallet(_JDECartonTable.CDCRID);
        }

        private void _confirmDeletePallet_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateConfirmDeletePallet(e.UserInput.ToBool());
        }

        private void validateConfirmDeletePallet(bool confirm)
        {
            if (confirm)
            {
                SendToErp();
            }
            else if (!confirm)
            {
                Back(_collectFromCarton);
            }
            else
            {
                throw new RFS_Exception("InvalidEntry");
            }
        }

        private void _collectItem_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateItem(e.UserInput.ToString().Trim());
        }

        private void validateItem(string item)
        {
            _itemMasterTable.ResetAll();

            // Use the returned result to display the proper item number
            _primaryItem = _itemMasterTable.RetrieveItem(_collectBranch.ScreenInput,
                item,
                _warehouseConstantsTable._DesignatedPrimaryItemNumber.ToString());
            if (_primaryItem == null)
            {
                throw new RFS_Exception("ItemNumber");
            }

            _shortItem = _itemMasterTable.IMITM;
            if (!isQuantityAvailable(null))
            {
                throw new RFS_Exception("NotAvailable");
            }

            _collectItem.ScreenInput = _primaryItem;

            _itemBranchTable.ValidateItemBranch(_collectBranch.ScreenInput, _shortItem);
        }

        private bool isLotItem()
        {
            //in this workflow Serial Items are treated as lot items.
            if ((_itemBranchTable.IBSRCE.Trim().Length > 0) &&
                (Convert.ToInt16(_itemBranchTable.IBSRCE) >= Convert.ToInt16(LotSerialDesignations.LotOptional)))
            {
                return (true);
            }
            else
            {
                return false;
            }
        }

        #region callRandomItemQuery

        private void callRandomItemQuery(double shortItem)
        {
            switch (_collectPickMode.ScreenInput.Trim())
            {
                case "1": // by PickSlip
                    if (!_salesOrderTable.ValidPickSlipItem(_collectBranch.ScreenInput, _collectPickSlip.ScreenInput, null, null, shortItem, UserSession.UserId, "0"))
                    {
                        throw new RFS_Exception("ItemNotOnPickSlip");
                    }

                    break;
                case "2": // by Shipment
                    if (!_salesOrderTable.ValidShipmentItem(_collectBranch.ScreenInput, _collectShipment.ScreenInput, null, null, shortItem, UserSession.UserId, "0"))
                    {
                        throw new RFS_Exception("ItemNotOnShipment");
                    }

                    break;
            }
        }

        #endregion

        private void _collectLot_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (!isLotItem())
            {
                _collectLot.CancelLoad = true;
                _collectLot.ScreenInput = string.Empty;
                ScreenMap.Add("LotNumber", "");
                lotNext();
                return;
            }
        }

        private void lotNext()
        {
            if (_availableQuantityInPrimary < 0 && _sequence == BuildSequence.AddItem)
            {
            }
            else
            {
                Next(_collectQuantity);
            }
        }

        private void _collectLot_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateLot(e.UserInput.ToString().Trim());
        }

        private void validateLot(string lot)
        {
            if (lot != string.Empty || Convert.ToInt16(_itemBranchTable.IBSRCE) == Convert.ToInt16(LotSerialDesignations.LotRequired))
            {
                if (!_lotTable.RetrieveLotMaster(_collectBranch.ScreenInput, _shortItem, lot))
                {
                    throw new RFS_Exception("LotNumber");
                }

                if (DateTimeUtils.JDEJulian(_lotTable.IOMMEJ) < System.DateTime.Today)
                {
                    throw new RFS_Exception("ExpiredLot");
                }
            }

            if (!isQuantityAvailable(lot))
            {
                throw new RFS_Exception("NotAvailable");
            }

            _collectLot.ScreenInput = lot;
            ScreenMap.Add("LotNumber", lot);
            lotNext();
        }

        private void _collectQuantity_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateQuantity(e.UserInput.ToString());
        }

        private void validateQuantity(string quantity)
        {
            // Call calculator function
            try
            {
                string calculatedQuantity = _itemLocationTable.CalcQty(quantity);

                if (calculatedQuantity == null)
                {
                    throw new RFS_Exception("InvalidQuantity");
                } // end if
                else if (double.Parse(calculatedQuantity) > 0)
                {
                    _quantity = double.Parse(calculatedQuantity);
                } // end if
                else if (double.Parse(calculatedQuantity) == 0)
                {
                    throw new RFS_Exception("ZeroEntry");
                }
                else
                {
                    throw new RFS_Exception("NegativeEntry");
                } // end else
            } // end try
            catch
            {
                throw new RFS_Exception("InvalidQuantity");
            } // end catch

            ScreenHandler.ScreenMap.Add("CollectedQuantity", _quantity.ToString());
            ScreenHandler.ScreenMap.Add("Quantity", _quantity.ToString());
        }

        private double ConvertToUOMQty(string fromUOM, string toUOM, double qty)
        {
            double factor = 0;
            if (fromUOM == toUOM)
            {
                factor = 1;
            }
            else
            {
                factor = _uomConversionTable.ConvertToPrimaryUOM(_collectBranch.ScreenInput,
                    _shortItem,
                    fromUOM,
                    toUOM);
                if (factor == 0)
                    factor = _uomConversionTable2.ConvertToPrimaryUOM(fromUOM, toUOM);
            }

            return (factor * qty);
        }

        /// <summary>
        /// Check to see if there is something to transfer, add or remove.
        /// </summary>
        /// <param name="lot">Set to null if not collected.</param>
        /// <returns></returns>
        private bool isQuantityAvailable(string lot)
        {
            switch (_sequence)
            {
                case BuildSequence.ItemTransfer:
                    //case BuildSequence.RemoveItem:
                    _JDECartonTable.GetLotInCarton(_collectBranch.ScreenInput, _fromCartonId, _shortItem, lot);
                    getCartonQuantity();
                    _pickedInPrimary = 0;
                    _availableQuantityInPrimary = _cartonQuanityInPrimary;
                    return _availableQuantityInPrimary > 0;
                    break;
                case BuildSequence.AddItem:
                    // quantity on order at status - carton quantity.
                    switch (_collectPickMode.ScreenInput.Trim())
                    {
                        case "1": // by PickSlip
                            if (_salesOrderTable.ValidPickslipLot(_collectBranch.ScreenInput,
                                    double.Parse(_collectPickSlip.ScreenInput),
                                    _allowedStatus,
                                    _shortItem,
                                    lot))
                            {
                                _JDECartonTable.GetLotOnPickslip(_collectBranch.ScreenInput,
                                    double.Parse(_collectPickSlip.ScreenInput),
                                    _shortItem,
                                    lot);
                                getAvailableQuantity();
                                getUnpackedlines();
                                return _availableQuantityInPrimary != 0;
                            }
                            else
                            {
                                return false;
                            }

                            break;
                        case "2": // by Shipment
                            if (_salesOrderTable.ValidShipmentLot(_collectBranch.ScreenInput,
                                    _collectShipment.ScreenInput,
                                    _allowedStatus,
                                    _shortItem,
                                    lot))
                            {
                                _JDECartonTable.GetLotOnShipment(_collectBranch.ScreenInput,
                                    double.Parse(_collectShipment.ScreenInput),
                                    _shortItem,
                                    lot);
                                getUnpackedlines();
                                return _availableQuantityInPrimary != 0;
                            }
                            else
                            {
                                return false;
                            }

                            break;
                    } // end switch

                    break;
            } // end switch

            return false;
        }

        private void getUnpackedlines()
        {
            if (!_salesOrderTable.ThisDataTable.Columns.Contains("OpenInPrimary"))
                _salesOrderTable.ThisDataTable.Columns.Add("OpenInPrimary", typeof(double));
            if (!_salesOrderTable.ThisDataTable.Columns.Contains("OpenInSOUOM"))
                _salesOrderTable.ThisDataTable.Columns.Add("OpenInSOUOM", typeof(double));
            foreach (DataRow dr in _salesOrderTable.Rows)
            {
                _salesOrderTable.CurrentRow = dr;
                _JDECartonTable.ThisDataTable.DefaultView.RowFilter =
                    string.Format(
                        "CDLNID = {0} AND CDDOCO = {1} AND CDDCTO = '{2}' AND CDKCOO = '{3}'",
                        this._dataManipulator.ManipulateDataForDisplay("F4211", "SDLNID", _salesOrderTable.SDLNID),
                        _salesOrderTable.SDDOCO,
                        _salesOrderTable.SDDCTO,
                        _salesOrderTable.SDKCOO);
                _cartonQuanityInPrimary = 0;
                _pickedInPrimary = ConvertToUOMQty(_salesOrderTable.SDUOM, _itemMasterTable.IMUOM1, _salesOrderTable.SDSOQS);
                if (_JDECartonTable.ThisDataTable.DefaultView.Count > 0)
                {
                    foreach (DataRowView cartonDr in _JDECartonTable.ThisDataTable.DefaultView)
                    {
                        _JDECartonTable.CurrentRow = cartonDr.Row;
                        _cartonQuanityInPrimary += ConvertToUOMQty(_JDECartonTable.CDUOM, _itemMasterTable.IMUOM1, _JDECartonTable.CDTQTY);
                    }
                }

                _JDECartonTable.ThisDataTable.DefaultView.RowFilter = "";
                Log.Debug("OpenQtyInPrimary = " + (_pickedInPrimary - _cartonQuanityInPrimary).ToString());
                _salesOrderTable["OpenInPrimary"] = _pickedInPrimary - _cartonQuanityInPrimary;
                _salesOrderTable["OpenInSOUOM"] = ConvertToUOMQty(_itemMasterTable.IMUOM1, _salesOrderTable.SDUOM, _pickedInPrimary - _cartonQuanityInPrimary);
            }

            _salesOrderTable.ThisDataTable.DefaultView.RowFilter = "OpenInPrimary > 0";
            if (_salesOrderTable.ThisDataTable.DefaultView.Count == 0)
            {
                _availableQuantityInPrimary = 0;
            }
            else if (_salesOrderTable.ThisDataTable.DefaultView.Count == 1)
            {
                _salesOrderTable.CurrentRow = _salesOrderTable.ThisDataTable.DefaultView[0].Row;
                _availableQuantityInPrimary = double.Parse(_salesOrderTable.ThisDataTable.DefaultView[0]["OpenInPrimary"].ToString());
            }
            else if (_salesOrderTable.ThisDataTable.DefaultView.Count > 1)
            {
                _availableQuantityInPrimary = 0;

                //Leave the current row at row 0
                _salesOrderTable.CurrentRow = _salesOrderTable.ThisDataTable.DefaultView[0].Row;

                for (int i = 0; i < _salesOrderTable.ThisDataTable.DefaultView.Count; i++)
                {
                    _availableQuantityInPrimary += double.Parse(_salesOrderTable.ThisDataTable.DefaultView[i]["OpenInPrimary"].ToString());
                }
            }
            else
            {
                _availableQuantityInPrimary = -1;
            }
        }

        private void getAvailableQuantity()
        {
            _pickedInPrimary = 0;
            foreach (DataRow dr in _salesOrderTable.Rows)
            {
                _salesOrderTable.CurrentRow = dr;
                _pickedInPrimary += ConvertToUOMQty(_salesOrderTable.SDUOM, _itemMasterTable.IMUOM1, _salesOrderTable.SDSOQS);
            }

            _salesOrderTable.CurrentRow = _salesOrderTable.Rows[0];
            getCartonQuantity();
            _availableQuantityInPrimary = _pickedInPrimary - _cartonQuanityInPrimary;
        }

        private void getCartonQuantity()
        {
            _cartonQuanityInPrimary = 0;
            if (_JDECartonTable.Rows.Count > 0)
            {
                foreach (DataRow dr in _JDECartonTable.Rows)
                {
                    _JDECartonTable.CurrentRow = dr;
                    _cartonQuanityInPrimary += ConvertToUOMQty(_JDECartonTable.CDUOM, _itemMasterTable.IMUOM1, _JDECartonTable.CDTQTY);
                }

                _JDECartonTable.CurrentRow = _JDECartonTable.Rows[0];
            }
        }

        #region UOM

        private void _collectUOM_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (_sequence)
            {
                case BuildSequence.AddItem:
                    ScreenHandler.DefaultText = _itemMasterTable.DefaultUOM(_defaultUOM);
                    break;
                case BuildSequence.ItemTransfer:
                    ScreenHandler.DefaultText = _JDECartonTable.CDUOM;
                    break;
            } // end switch
        }

        private void _collectUOM_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateUOM(e.UserInput.ToString());
        }

        private void validateUOM(string uom)
        {
            if (_sequence == BuildSequence.ItemTransfer && uom.ToUpper().Trim() != _JDECartonTable.CDUOM.ToUpper().Trim())
            {
                throw new RFS_Exception("CartonUnitOfMeasure");
            }

            _quantityInPrimary = ConvertToUOMQty(uom, _itemMasterTable.IMUOM1.ToString(), _quantity);
            if (_quantityInPrimary == 0)
            {
                throw new RFS_Exception("UnitOfMeasure");
            }

            if (_quantityInPrimary > _availableQuantityInPrimary)
            {
                throw new RFS_Exception("AvailableQuantity");
            }

            _collectUOM.ScreenInput = uom;
            SendToErp();
        }

        #endregion

        #region print

        private void _printBarCodeLabels_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            int numberOfLabels = 0;
            try
            {
                numberOfLabels = int.Parse(e.UserInput.ToString());
            }
            catch
            {
                throw new RFS_Exception("NotNumeric");
            }

            if (numberOfLabels < 0)
            {
                throw new RFS_Exception("NegativeNotAllowed");
            }

            if (numberOfLabels > _maxLabels)
            {
                throw new RFS_Exception("GreaterThanMax");
            }

            if (numberOfLabels > 0)
            {
                printBarCode(numberOfLabels);
            }

            RestartLoop(_collectSequence);
        }

        private void printBarCode(int numberOfLabels)
        {
            _StoneridgePrinting.ResetAll();
            _StoneridgePrinting.Branch = _collectBranch.ScreenInput.Trim();
            _StoneridgePrinting.Customer = _salesOrderTable.SDAN8.ToString();
            _StoneridgePrinting.Item = _salesOrderTable.SDSHAN.ToString();

            if (_sequence == BuildSequence.UnpackPallet)
            {
                switch (UnpackLabelType)
                {
                    case 1:
                        _StoneridgePrinting.ContainerType = "M"; //Master
                        break;
                    case 2:
                        _StoneridgePrinting.ContainerType = "X"; //Mixed
                        break;
                }

                if (!_StoneridgePrinting.ValidatePrinter(_collectPrinter.ScreenInput))
                {
                    throw new RFS_Exception(_StoneridgePrinting.ErrorMessageVocabulary);
                }

                //Now that we know we have a valid printer to print to, pull back the F4620 record to get the CDUKID 
                _JDECartonTable.GetCarton(_collectBranch.ScreenInput, UnpackPalletNumber);
            }
            else
            {
                if (_JDECartonTable.CDITM == 0) //if 0, then assume it's a pallet record
                {
                    //Check to see if the current Pallet is a Mixed or not, and set the ContainerType respectively
                    using (CustomJDETables.F4620 table = new CustomJDETables.F4620(DataAccess, "MixCheck_"))
                    {
                        if (table.IsMixedPallet(_JDECartonTable.CDCRID))
                        {
                            _StoneridgePrinting.ContainerType = "X";
                        }
                        else
                        {
                            _StoneridgePrinting.ContainerType = "M";
                        }
                    }
                }
                else //Carton Record
                {
                    _StoneridgePrinting.ContainerType = "C";
                    _StoneridgePrinting.Location = _JDECartonTable.CDITM.ToString();
                }

                if (!_StoneridgePrinting.ValidatePrinter(_collectPrinter.ScreenInput))
                {
                    throw new RFS_Exception(_StoneridgePrinting.ErrorMessageVocabulary);
                }
            }

            PrintSystem.Reset();
            PrintSystem.Branch = _StoneridgePrinting.Branch;
            PrintSystem.ContainerType = _StoneridgePrinting.ContainerType;
            PrintSystem.Customer = _StoneridgePrinting.Customer;
            PrintSystem.ItemNumber = _StoneridgePrinting.Item;
            PrintSystem.Location = _StoneridgePrinting.Location;

            PrintSystem.PrinterName = _collectPrinter.ScreenInput;
            PrintSystem.PrinterAddress = _printerAddress;
            PrintSystem.Copies = numberOfLabels;

            //At this point we know we have a valid printer record (as setup above), now simply gather the data and print
            _GenCartonPrintData.ResetAll();
            _GenCartonPrintData.GenerateData(_salesOrderTable.SDSHPN, _JDECartonTable.CDUKID);
            _GenCartonPrintData.TransferPrintData(DataAccess); //copies the data generated to the RFS_AdditionalPrintData DataTable

            if (!PrintSystem.Print())
            {
                DisplayMessage(PrintSystem.ErrorMessageVocabulary);
            }
        }

        #endregion

        private void _confirmRemoveCarton_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            validateRemoveCarton(e.UserInput.ToBool());
        }

        private void validateRemoveCarton(bool confirm)
        {
            if (confirm)
            {
                SendToErp();
            }
            else if (!confirm)
            {
                RestartLoop(_collectFromCarton);
            }
            else
            {
                throw new RFS_Exception("InvalidEntry");
            }
        }

        private void _collectWeight_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            double carton = 0;
            if (_defaultWeight)
            {
                ScreenHandler.DefaultText = _JDECartonTable.CDGRWT.ToString();
            }
        }

        private void _collectLength_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_defaultDim)
            {
                ScreenHandler.DefaultText = _JDECartonTable.CDLGTS.ToString();
            }
        }

        private void _collectWidth_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_defaultDim)
            {
                ScreenHandler.DefaultText = _JDECartonTable.CDWTHS.ToString();
            }
        }

        private void _collectHeight_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_defaultDim)
            {
                ScreenHandler.DefaultText = _JDECartonTable.CDHGTS.ToString();
            }
        }

        private void _collectWeight_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                _weight = double.Parse(e.UserInput.ToString());
                if (_weight < 0)
                {
                    Error("NegativeNotAllowed");
                    return;
                }
            }
            catch
            {
                Error("NotNumeric");
                return;
            }

            ScreenMap.Add("Weight", _weight.ToString() + " " + _JDECartonTable.CDUWUM.Trim());
            ScreenMap.Add("Dimensions", _JDECartonTable.CDLGTS.ToString() + "\\" + _JDECartonTable.CDWTHS.ToString() + "\\" + _JDECartonTable.CDHGTS.ToString() + " " + _JDECartonTable.CDLUOM.Trim());

            Next(_collectLength);
        }

        private void _collectLength_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                _length = double.Parse(e.UserInput.ToString());
                if (_length < 0)
                {
                    Error("NegativeNotAllowed");
                    return;
                }
            }
            catch
            {
                Error("NotNumeric");
                return;
            }

            ScreenMap.Add("Dimensions", _length.ToString() + "\\" + _JDECartonTable.CDWTHS.ToString() + "\\" + _JDECartonTable.CDHGTS.ToString() + " " + _JDECartonTable.CDLUOM.Trim());

            Next(_collectWidth);
        }

        private void _collectWidth_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                _width = double.Parse(e.UserInput.ToString());
                if (_width < 0)
                {
                    Error("NegativeNotAllowed");
                    return;
                }
            }
            catch
            {
                Error("NotNumeric");
                return;
            }

            ScreenMap.Add("Dimensions", _length.ToString() + "\\" + _width.ToString() + "\\" + _JDECartonTable.CDHGTS.ToString() + " " + _JDECartonTable.CDLUOM.Trim());

            Next(_collectHeight);
        }

        private void _collectHeight_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            try
            {
                _height = double.Parse(e.UserInput.ToString());
                if (_height < 0)
                {
                    Error("NegativeNotAllowed");
                    return;
                }
            }
            catch
            {
                Error("NotNumeric");
                return;
            }

            SendWeightDimToErp();
            ScreenMap.Add("PalletId", "");
            ScreenMap.Add("ContainerType", "");
            Next(_printBarCodeLabels);
        }

        private void SendWeightDimToErp()
        {
            _weightDim = new ICS.ERPI.JDE.UpdateCartonWtandDim();

            Transactions.Add(_weightDim);

            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            _weightDim.Initialize();

            _weightDim.RFUserID = UserSession.CurrentUser.LoginName;
            _weightDim.TransDate_YYYYMMDD = System.DateTime.Now.ToString("yyyyMMdd");
            _weightDim.ShipmentNumber = _JDECartonTable.CDSHPN.ToString();
            switch (_sequence)
            {
                case BuildSequence.AddItem:
                    _weightDim.CartonID = this._toCartonId.ToString().Trim();
                    break;
                case BuildSequence.AddCarton:
                    _weightDim.CartonID = this._toCartonId.ToString().Trim();
                    break;
                //case BuildSequence.RemoveItem:
                //    _weightDim.CartonID = this._fromCartonId.ToString().Trim();
                //    break;
                case BuildSequence.RemoveCarton:
                    _weightDim.CartonID = this._fromCartonId.ToString().Trim();
                    break;
                case BuildSequence.CartonTransfer:
                    _weightDim.CartonID = this._toCartonId.ToString().Trim();
                    break;
                case BuildSequence.ItemTransfer:
                    _weightDim.CartonID = this._toCartonId.ToString().Trim();
                    break;
                case BuildSequence.WeightsAndDimensions:
                    _weightDim.CartonID = this._cartonId.ToString().Trim();
                    break;
            } // end switch

            _weightDim.CartonGrossWeight = _weight.ToString();
            _weightDim.CartonLength = _length.ToString();
            _weightDim.CartonWidth = _width.ToString();
            _weightDim.CartonHeight = _height.ToString();

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();
        }

        private bool _collectItem_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (_sequence)
            {
                case BuildSequence.AddItem:
                case BuildSequence.ItemTransfer:
                    //case BuildSequence.RemoveItem:
                    if (!_JDECartonTable.GetCarton(_collectBranch.ScreenInput, _toCartonId))
                    {
                        throw new RFS_Exception("InvalidButton");
                        break;
                    }

                    ScreenMap.Add("ContainerType", _JDECartonTable.CDEQTY);
                    //ScreenMap.Add("PalletId", _JDECartonTable.CDCEQT.ToString());
                    ScreenMap.Add("PalletId", _JDECartonTable.CDURRF.ToString());
                    Next(_collectWeight);
                    break;
                case BuildSequence.AddCarton:
                case BuildSequence.RemoveCarton:
                case BuildSequence.CartonTransfer:
                    throw new RFS_Exception("InvalidButton");
                    break;
            } // end switch

            return true;
        }

        private bool _collectCarton_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            switch (_sequence)
            {
                case BuildSequence.AddItem:
                case BuildSequence.ItemTransfer:
                    //case BuildSequence.RemoveItem:
                    throw new RFS_Exception("InvalidButton");
                    break;
                case BuildSequence.RemoveCarton:
                    if (!_JDECartonTable.GetCarton(_collectBranch.ScreenInput, _fromCartonId))
                    {
                        RestartLoop(_collectSequence);
                        Error("EndOfPallet");
                        return true;
                    }

                    ScreenMap.Add("ContainerType", _JDECartonTable.CDEQTY);
                    //ScreenMap.Add("PalletId", _JDECartonTable.CDCEQT.ToString());
                    ScreenMap.Add("PalletId", _JDECartonTable.CDURRF.ToString());
                    Next(_collectWeight);
                    break;
                case BuildSequence.AddCarton:
                case BuildSequence.CartonTransfer:
                    if (!_JDECartonTable.GetCarton(_collectBranch.ScreenInput, _toCartonId))
                    {
                        throw new RFS_Exception("InvalidButton");
                        break;
                    }

                    ScreenMap.Add("ContainerType", _JDECartonTable.CDEQTY);
                    //ScreenMap.Add("PalletId", _JDECartonTable.CDCEQT.ToString());
                    ScreenMap.Add("PalletId", _JDECartonTable.CDURRF.ToString());
                    Next(_collectWeight);
                    break;
            } // end switch

            return true;
        }

        private void clearTransaction()
        {
            ScreenMap.Add("ContainerType", "");
            ScreenMap.Add("PalletId", "");
            ScreenHandler.ScreenMap.Add("CollectedQuantity", "");
            ScreenHandler.ScreenMap.Add("Carton", "");
            ScreenHandler.ScreenMap.Add("FromCarton", "");
            ScreenHandler.ScreenMap.Add("ToCarton", "");
        }

        private void _printBarCodeLabels_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            bool allowPrint = true;
            switch (_sequence)
            {
                case BuildSequence.AddItem:
                case BuildSequence.ItemTransfer:
                    if (!_JDECartonTable.GetCartonForPrint(_collectBranch.ScreenInput, _toCartonId))
                    {
                        allowPrint = false;
                        break;
                    }

                    break;
                case BuildSequence.RemoveCarton:
                    if (!_JDECartonTable.GetCartonForPrint(_collectBranch.ScreenInput, _fromCartonId))
                    {
                        allowPrint = false;
                        break;
                    }

                    break;
                case BuildSequence.AddCarton:
                case BuildSequence.CartonTransfer:
                    if (!_JDECartonTable.GetCartonForPrint(_collectBranch.ScreenInput, _toCartonId))
                    {
                        allowPrint = false;
                        break;
                    }

                    break;
            } // end switch

            if (!allowPrint || _collectPrinter.ScreenInput.Trim().Length == 0)
            {
                _printBarCodeLabels.CancelLoad = true;
                decisionTable3();
            }
        }

        private void _OrderDetail_List_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Next(_collectQuantity);
        }

        #region printItemtable

        private void createPrintItemTable()
        {
            _printItems = new DataTable("printItems");
            _printItems.Columns.Add("item", typeof(string));
            _printItems.Columns.Add("lot", typeof(string));
            _printItems.Columns.Add("UOM", typeof(string));
            _printItems.Columns.Add("desc", typeof(string));
            _printItems.Columns.Add("quantity", typeof(double));
            DataColumn[] keys = new DataColumn[3];
            keys[0] = _printItems.Columns[0];
            keys[1] = _printItems.Columns[1];
            keys[2] = _printItems.Columns[2];
            _printItems.PrimaryKey = keys;
            ;
        }

        private void addPrintItems()
        {
            string sPrimaryItem = "";
            switch (this._warehouseConstantsTable._DesignatedPrimaryItemNumber)
            {
                case PrimaryItemNumberDesignations.ShortNumber:
                    sPrimaryItem = _JDECartonTable.CurrentRow["IMITM"].ToString();
                    break;
                case PrimaryItemNumberDesignations.LongNumber:
                    sPrimaryItem = _JDECartonTable.CurrentRow["IMLITM"].ToString();
                    break;
                case PrimaryItemNumberDesignations.ThirdNumber:
                    sPrimaryItem = _JDECartonTable.CurrentRow["IMAITM"].ToString();
                    break;
            }

            DataRow ItemDR = _printItems.Rows.Find(new object[]
            {
                sPrimaryItem,
                _JDECartonTable.CDLOT,
                _JDECartonTable.CDUOM
            });
            if (ItemDR != null)
            {
                ItemDR["quantity"] = double.Parse(ItemDR["quantity"].ToString()) + _JDECartonTable.CDTQTY;
            }
            else
            {
                ItemDR = _printItems.NewRow();
                ItemDR["item"] = sPrimaryItem;
                ItemDR["lot"] = _JDECartonTable.CDLOT;
                ItemDR["UOM"] = _JDECartonTable.CDUOM;
                ItemDR["desc"] = _JDECartonTable.CurrentRow["IMDSC1"].ToString();
                ItemDR["quantity"] = _JDECartonTable.CDTQTY;
                _printItems.Rows.Add(ItemDR);
            }
        }

        #endregion

        #region Unpack Mode (Mode 9)

        #region Collect Unpack Pallet Number

        private bool CollectUnpackPalletNumber_ValidateMethod(string UnpackPalletNumberInput)
        {
            double UnpackParsedPalletNumber;

            if (double.TryParse(UnpackPalletNumberInput, out UnpackParsedPalletNumber))
            {
                if (_JDECartonTable.GetUnpackPalletNumber(_branchTable.MCMCU, _salesOrderTable.SDSHPN, UnpackPalletNumberInput))
                {
                    UnpackPalletNumber = _JDECartonTable.CDURRF;
                    UnpackPalletCrossReferenceNumber = _JDECartonTable.CDCRID;

                    ScreenMap.Add("UnpackPalletNumber", UnpackPalletNumber);

                    if (WFOptions.RetrieveOptionValue("UnpackPallet") == "Y")
                    {
                        if (_JDECartonTable.GetUnpackSerialNumberPallet(_branchTable.MCMCU, _salesOrderTable.SDSHPN, UnpackPalletNumber, AllowedUnpackSalesOrderStatuses))
                        {
                            ScreenMap.Add("UnpackSerialNumber", UnpackPalletNumber);
                            Next(WarnUnpackSerialNumbers);
                        }
                        else
                        {
                            throw new RFS_Exception("InvalidStatus");
                        }
                    }
                    else
                    {
                        Next(CollectUnpackSerialNumber);
                    }
                }
                else
                {
                    throw new RFS_Exception("InvalidPallet");
                }
            }
            else
            {
                throw new RFS_Exception("NotNumeric");
            }

            return true;
        }

        #endregion

        #region Collect Unpack Serial Number

        private bool CollectUnpackSerialNumber_ValidateMethod(string UnpackSerialNumberInput)
        {
            if (_JDECartonTable.GetUnpackSerialNumber(_branchTable.MCMCU, _salesOrderTable.SDSHPN, UnpackSerialNumberInput, -1))
            {
                if (_JDECartonTable.GetUnpackSerialNumber(_branchTable.MCMCU, _salesOrderTable.SDSHPN, UnpackSerialNumberInput, UnpackPalletCrossReferenceNumber))
                {
                    if (_JDECartonTable.GetUnpackSerialNumber(_branchTable.MCMCU, _salesOrderTable.SDSHPN, UnpackSerialNumberInput, UnpackPalletCrossReferenceNumber, AllowedUnpackSalesOrderStatuses))
                    {
                        UnpackUKID = _JDECartonTable.CDUKID;
                        UnpackSerialNumber = _JDECartonTable.CDPAK;

                        ScreenMap.Add("UnpackSerialNumber", UnpackSerialNumber);

                        Next(WarnUnpackSerialNumbers);
                    }
                    else
                    {
                        throw new RFS_Exception("InvalidStatus");
                    }
                }
                else
                {
                    throw new RFS_Exception("SerialNotOnPallet");
                }
            }
            else
            {
                throw new RFS_Exception("InvalidSerialNumber");
            }

            return true;
        }

        private bool CollectUnpackSerialNumber_F6Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            Next(ConfirmDoneUnpacking);
            return true;
        }

        #endregion

        #region Warn Unpack Serial Number

        private bool WarnUnpackSerialNumbers_ValidateMethod(string ConfirmationInput)
        {
            if (ConfirmationInput.GetYesNo())
            {
                //SendUnpackToERP();
                do
                {
                    UnpackUKID = _JDECartonTable.CDUKID;
                    UnpackSerialNumber = _JDECartonTable.CDPAK;
                    SendUnpackToERP();
                } while (_JDECartonTable.MoveNext());
            }

            RestartLoop(CollectUnpackSerialNumber);

            return true;
        }

        #endregion

        #region Confirm Done Unpacking

        private bool ConfirmDoneUnpacking_ValidateMethod(string ConfirmationInput)
        {
            if (ConfirmationInput.GetYesNo())
            {
                //Make sure a printer was collected, indicating they want to print as a whole. This was put into place, as it already exists
                //in the base, with some of the code expecting it (including the actual printing code itself). So honoring, just like the 
                //LoadEvent for PrintBarCodeLabes does
                if (!String.IsNullOrWhiteSpace(_collectPrinter.ScreenInput))
                {
                    Next(CollectUnpackLabelType);
                }
                else
                {
                    RestartLoop(CollectUnpackPalletNumber);
                }
            }
            else
            {
                RestartLoop(CollectUnpackSerialNumber);
            }

            return true;
        }

        #endregion

        #region Collect Unpack Label Type

        private bool CollectUnpackLabelType_ValidateMethod(string UnpackLabelTypeInput)
        {
            switch (UnpackLabelTypeInput.Trim().ToUpper())
            {
                case "M":
                    UnpackLabelType = 1;
                    ScreenMap.Add("UnpackLabelType", "Master");
                    Next(CollectNumberOfUnpackLabels);
                    break;
                case "X":
                    UnpackLabelType = 2;
                    ScreenMap.Add("UnpackLabelType", "Mixed Load");
                    Next(CollectNumberOfUnpackLabels);
                    break;
                case "N":
                    RestartLoop(CollectUnpackPalletNumber);
                    break;
                default:
                    throw new RFS_Exception("InvalidLabelType");
            }

            return true;
        }

        #endregion

        #region Collect Number of Unpack Labels

        private bool CollectNumberOfUnpackLabels_ValidateMethod(string NumberOfUnpackLabelsInput)
        {
            int ParsedNumberOfUnpackLabels;

            if (int.TryParse(NumberOfUnpackLabelsInput, out ParsedNumberOfUnpackLabels))
            {
                NumberOfUnpackLabels = ParsedNumberOfUnpackLabels;

                ScreenMap.Add("NumberOfUnpackLabels", NumberOfUnpackLabels.ToString());

                printBarCode(NumberOfUnpackLabels);
                RestartLoop(CollectUnpackPalletNumber);
                return true;
            }
            else
            {
                throw new RFS_Exception("NotANumber");
            }
        }

        #endregion

        #region Send Unpack To ERP

        public bool SendUnpackToERP()
        {
            _maintainCarton = new ICS.ERPI.JDE.MaintainCarton();
            Transactions.Add(_maintainCarton);
            Transactions.Branch = _collectBranch.ScreenInput.Trim().ToUpperInvariant();
            _maintainCarton.Initialize();

            _maintainCarton.UserInput01 = UnpackUKID.ToString();
            _maintainCarton.UserInput02 = UnpackedSalesOrderStatus;
            _maintainCarton.Mode = "U";
            _maintainCarton.RFUserID = UserSession.CurrentUser.LoginName;
            _maintainCarton.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");

            Transactions.Send(WFOptions.GetYesNoWFOptionValue("SyncTransaction") ? "Y" : "N");
            DisplayMessages();

            return !Transactions.Error;
        }

        #endregion

        #endregion
    }
}