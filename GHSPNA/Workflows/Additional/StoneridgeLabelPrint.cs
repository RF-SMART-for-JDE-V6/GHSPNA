namespace ICS.Stoneridge.Wireless.JDE.Workflow.Additional
{
    using ICS.Database.TableObjects.JDE;
    using ICS.Database.TableObjects.RFS;
    using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;

    #region Usings

    using ICS.Display;
    using ICS.Utilities;
    using ICS.Workflow.Base.WorkflowDesign.Components;
    using System;

    #endregion

    public partial class StoneridgeLabelPrint : RFSWorkflowDesigner
    {
        #region Class Level defs

        #region Tables

        private F0006 _BranchPlantTable;

        private F4101 _ItemMasterTable;

        private F4100 _LocationMasterTable;

        private F4104 _ItemCrossReferenceTable;

        private CustomJDETables.F4102 _ItemBranchTable;

        private F41001 _WarehouseConstants;

        private F41002 _UnitOfMeasureConversionTable1;

        private F41003 _UnitOfMeasureConversionTable2;

        private F41021 _ItemLocationTable;

        private F4311 _PurchaseOrderDetailTable;

        private F43090 _ReceiptRouteTable;

        private CustomJDETables.F5846001 _SerialTable;

        private CustomJDETables.F56PI001 _InventoryTageTable;

        private RFS_DataManipulator _DataManipulatorTable;

        private RFS_NextNumber _NextNumberTable;

        private RFS_PrinterDefinition _DefinedPrintersTable; // RFS Defined Printers

        #endregion

        #region Collected/Working Data

        private string _BranchPlant;

        private string _PrimaryItemNumber;

        private double _ShortItemNumber;

        private string _Location;

        private double _QtyOnLabel;

        private double _NumberOfLabels;

        private string _ExpRet;

        private string _UnitOfMeasure; // unit of measure

        private int _NumberOfCartons;

        private string _ReceivedDate;

        private string _Revision;

        private string _PO;

        private string _DeliverTo;

        private string _Inspection;

        private string _LotSerial;

        #region Workflow Options

        #endregion

        #endregion

        #endregion

        #region Business connector

        private LabelPrintConnector connectorWorkflow;

        public LabelPrintConnector _WorkflowConnector
        {
            get { return connectorWorkflow ?? (connectorWorkflow = new LabelPrintConnector(this)); }
        }

        #endregion

        #region Constructors and overrides

        public StoneridgeLabelPrint(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeLabelPrint()
        {
            InitializeComponent();
        }

        protected override void Initialize()
        {
            #region Table

            this._BranchPlantTable = new F0006(UserSession.DataAccess, "");
            this._LocationMasterTable = new F4100(UserSession.DataAccess, "");
            this._ItemMasterTable = new F4101(UserSession.DataAccess, "");
            this._ItemBranchTable = new CustomJDETables.F4102(UserSession.DataAccess, "");
            this._ItemCrossReferenceTable = new F4104(UserSession.DataAccess, "");
            this._WarehouseConstants = new F41001(UserSession.DataAccess, "");
            this._UnitOfMeasureConversionTable1 = new F41002(UserSession.DataAccess, "");
            this._UnitOfMeasureConversionTable2 = new F41003(UserSession.DataAccess, "");
            this._ItemLocationTable = new F41021(UserSession.DataAccess, "");
            _PurchaseOrderDetailTable = new F4311(UserSession.DataAccess, "");
            _ReceiptRouteTable = new F43090(UserSession.DataAccess, "");
            this._SerialTable = new CustomJDETables.F5846001(UserSession.DataAccess, "");
            this._InventoryTageTable = new CustomJDETables.F56PI001(UserSession.DataAccess, "");
            this._DataManipulatorTable = new RFS_DataManipulator(UserSession.DataAccess, "");
            this._NextNumberTable = new RFS_NextNumber(UserSession.DataAccess, "");
            this._DefinedPrintersTable = new RFS_PrinterDefinition(UserSession.DataAccess, "");

            #endregion
        }

        public override void Dispose()
        {
            ScreenMap.ClearAllWorkflows();
            base.Dispose();
        }

        #endregion

        #region Screen methods

        #region BranchPlant

        private bool CollectBranchPlant_ValidateMethod(string BranchPlant)
        {
            _WorkflowConnector.ValidateBranchPlant(BranchPlant);
            _BranchPlant = _WorkflowConnector.BranchPlant;
            //if (_BranchPlantTable.FetchBranchPlant(BranchPlant))
            //{
            //    _BranchPlant = _BranchPlantTable.MCMCU;
            //}
            //else
            //{
            //    throw new RFS_Exception("MCU");
            //}
            Next(CollectPrinter);

            return true;
        }

        private void BranchPlantList_LoadingMethod(ScreenEventArgs e)
        {
            BranchPlantList.Table = _WorkflowConnector._BranchPlantTable;
            _WorkflowConnector._BranchPlantTable.FetchBranchPlants();
        }

        private void BranchPlantList_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _BranchPlantTable.UpdateCurrentRow();
            _BranchPlant = _BranchPlantTable.MCMCU;
            _WorkflowConnector.ValidateBranchPlant(_BranchPlant);
        }

        #endregion

        #region Item

        private void CollectItemNumber_Enter(string ItemNumber)
        {
            //_PrimaryItemNumber = _ItemMasterTable.RetrieveItem(_BranchPlant, ItemNumber, _WarehouseConstants._DesignatedPrimaryItemNumber.ToString());
            _ItemMasterTable.ResetAll();
            _ItemMasterTable.IMLITM = ItemNumber;
            //_PrimaryItemNumber = _ItemMasterTable.IMITM.ToString();
            //Log.Debug($"<***> _PrimaryItemNumber = {_PrimaryItemNumber}");
            if (_ItemMasterTable.Fetch())
            {
                if (WFOptions.RetrieveOptionValue("LabelType") == "P")
                {
                    if (!_ItemBranchTable.GetItem(_BranchPlant, _ItemMasterTable.IMLITM, true))
                    {
                        throw new RFS_Exception("INVALIDPART");
                    }
                }

                if (_ItemBranchTable.GetItem(_BranchPlant, _ItemMasterTable.IMLITM))
                {
                    _ShortItemNumber = _ItemMasterTable.IMITM;
                    _UnitOfMeasure = _ItemMasterTable.IMUOM1;
                    switch (_WarehouseConstants._DesignatedPrimaryItemNumber)
                    {
                        case PrimaryItemNumberDesignations.ShortNumber:
                            UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ItemMasterTable.IMITM.ToString());
                            break;
                        case PrimaryItemNumberDesignations.LongNumber:
                            UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ItemMasterTable.IMLITM);
                            break;
                        case PrimaryItemNumberDesignations.ThirdNumber:
                            UserSession.ScreenHandler.ScreenMap.Add("ITEMNUMBER", _ItemMasterTable.IMAITM);
                            break;
                    }

                    if (PromptForLot() || IsItemSerialized())
                    {
                        Next(CollectLotSerial);
                    }
                    else
                    {
                        Next(CollectQtyOnLabel);
                    }
                    
                    return;
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

        private void _ItemMasterList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemMasterTable.ShowItemsInBranch(_BranchPlant, "");
        }

        private void _ItemMasterList_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ItemMasterTable.UpdateCurrentRow();
            CollectItemNumber_Enter(_ItemMasterTable.IMLITM.ToString());
        }

        private void CollectItemNumber_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToString().Trim().Length > 0)
            {
                CollectItemNumber_Enter(e.UserInput.ToString());
            }
            else
            {
                throw new RFS_Exception("BLANKSNOTALLOWED");
            }
        }

        private void CollectItemNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_ItemMasterTable.IMLITM != null)
            {
                ScreenHandler.DefaultText = _ItemMasterTable.IMLITM;
            }
        }

        #endregion

        #region Lot/Serial
        
        private bool CollectLotSerial_ValidateMethod(string sInput)
        {
            _LotSerial = sInput;

            if (string.IsNullOrWhiteSpace(_LotSerial))
            {
                if (LotSerialRequired())
                {
                    throw new RFS_Exception("Lot/Serial Required");
                }
            }
      
            return true;
        }

        #endregion

        #region Quantity on Label

        private bool CollectQtyOnLabel_ValidateMethod(string QtyOnLabel)
        {
            //if its serial, just ignore what the user input and always use 1
            if (IsItemSerialized())
            {
                QtyOnLabel = "1";
            }

            if (double.TryParse(QtyOnLabel, out _QtyOnLabel))
            {
                if (_QtyOnLabel > 0)
                {
                    UserSession.ScreenHandler.ScreenMap.Add("_QtyOnLabel", QtyOnLabel.ToString());
                }
                else
                {
                    throw new RFS_Exception("INVALIDQUANTITY");
                }
            }
            else
            {
                throw new RFS_Exception("INVALIDQUANTITY");
            }

            switch (WFOptions.RetrieveOptionValue("LabelType"))
            {
                case "I":
                    Next(CollectContainer);
                    break;
                case "R":
                    Next(CollectPurchaseOrder);
                    break;
                case "P":
                    Next(CollectLocation);
                    break;
            }

            //if (WFOptions.RetrieveOptionValue("LabelType") == "I")
            //{
            //    Next(CollectContainer);
            //}
            //else
            //{
            //    Next(CollectPurchaseOrder);
            //}
            return true;
        }

        private void CollectQtyOnLabel_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (IsItemSerialized())
            {
                ScreenHandler.DefaultText = "1";
            }
            else
            {
                ScreenHandler.DefaultText = _QtyOnLabel.ToString();
            }
        }

        #endregion

        #region Printing

        private void CollectPrinter_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = _WorkflowConnector.LoadDefaultPrinter();
        }

        private bool CollectPrinter_ValidateMethod(string Printer)
        {
            return true;
        }

        private void CollectPrinter_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _WorkflowConnector.SetPrinter(e.UserInput.ToString());
        }

        private void PrintLabels(bool _Next, string _LabelCount = "")
        {
            _ItemLocationTable.RetrievePrimaryLocation(_BranchPlant, _ShortItemNumber);
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
            PrintSystem.Branch = _BranchPlant.Trim();
            PrintSystem.ItemNumber = _PrimaryItemNumber;
            PrintSystem.PrinterAddress = _WorkflowConnector.PrinterAddress;
            PrintSystem.PrinterName = _WorkflowConnector.PrinterName;
            if (WFOptions.RetrieveOptionValue("LabelType") == "I")
            {
                PrintSystem.ContainerType = "C";
                PrintSystem.Copies = (int) _NumberOfLabels;
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity", _QtyOnLabel.ToString(), false);
            }
            else
            {
                DateTime ReceivedDate = DateTime.Parse(_ReceivedDate.Substring(0, 2) + "/" + _ReceivedDate.Substring(2, 2) + "/" + _ReceivedDate.Substring(4, 4));
                PrintSystem.Copies = 1;
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_DateReceived", _ReceivedDate, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MonthReceived", ReceivedDate.ToString("MMM"), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PO", _PO.ToUpper(), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Revision", _Revision.ToUpper(), false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Inspection", _Inspection, false);
                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_DeliverTo", _DeliverTo.ToUpper(), false);
                Log.Debug($"<***>QTY = {(Math.Abs(_QtyOnLabel % 1) <= double.Epsilon).ToString()}");
                if (Math.Abs(_QtyOnLabel % 1) <= double.Epsilon)
                {
                    PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity", _QtyOnLabel.ToString(), false);
                    PrintSystem.ContainerType = "R";
                }
                else
                {
                    PrintSystem.ContainerType = "D";
                }

                PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity_Barcode", _QtyOnLabel.ToString(), false);
            }

            //PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity", _QtyOnLabel.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SRIPartNumber", _ItemMasterTable.IMLITM.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PartDesc", _ItemMasterTable.IMDSC1.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_CartonType", _ExpRet, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateDay", PackDate.Day.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateMonth", PackDate.Month.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_MfgDateYear", PackDate.Year.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PackTime", PackDate.ToString("HH:mm:ss"), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Shift", "", false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Packer", UserSession.UserId, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_LabelCount", _LabelCount, false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Reprint", "REPRINT", false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PrimaryLocation", _ItemLocationTable.LILOCN, false);

            int NextNumber;
            string NextSerialNumber = string.Empty;
                      
            _NextNumberTable.GetNextNumber("InternalSerial", out NextNumber, out NextSerialNumber);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_SerialNumber", NextSerialNumber, false);

            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_LotSerialNumber", _LotSerial, false);
            
            //PrintSystem.Copies = (int)_NumberOfLabels;
            PrintSystem.ExtraPrintData.NewRow();

            if (!PrintSystem.Print())
            {
                DisplayMessage(PrintSystem.ErrorMessageVocabulary);
            }
            else
            {
                string _locins = "";
                
                if (_Inspection=="INSPECTION")
                {
                    _locins = "INSP";
                }
                if (PromptForLot() || IsItemSerialized())
                {
                    Log.Debug($"(***) IS SERIAL = {IsItemSerialized()}");
                    //agch 20240328
                    if (_locins == "")
                    {
                        Log.Debug($"(***) with lot serial = {_LotSerial}");
                        _SerialTable.InsertSerial(NextSerialNumber, _ItemMasterTable.IMLITM.ToString(), _ShortItemNumber.ToString(), _BranchPlant, _UnitOfMeasure, _QtyOnLabel,
                        _ExpRet, UserSession.CurrentUser.LoginName, WorkflowID, _BranchPlantTable.MCCO.ToString(), _PO, "", _LotSerial);
                    }
                    else
                    {
                        Log.Debug($"(***) with lot serial and ispect = {_LotSerial}");
                        _SerialTable.InsertSerial(NextSerialNumber, _ItemMasterTable.IMLITM.ToString(), _ShortItemNumber.ToString(), _BranchPlant, _UnitOfMeasure, _QtyOnLabel,
                        _ExpRet, UserSession.CurrentUser.LoginName, WorkflowID, _BranchPlantTable.MCCO.ToString(), _PO, "", _LotSerial, _locins);
                    }
                }
                else
                {
                    //agch 20240328
                    if (_locins == "")
                        
                    {
                        Log.Debug($"(***) without  lot serial ");
                        _SerialTable.InsertSerial(NextSerialNumber, _ItemMasterTable.IMLITM.ToString(), _ShortItemNumber.ToString(), _BranchPlant, _UnitOfMeasure, _QtyOnLabel,
                            _ExpRet, UserSession.CurrentUser.LoginName, WorkflowID, _BranchPlantTable.MCCO.ToString(), _PO, "", string.Empty);
                    }
                    else
                    {
                        Log.Debug($"(***) without  lot serial but with insp ");
                        _SerialTable.InsertSerial(NextSerialNumber, _ItemMasterTable.IMLITM.ToString(), _ShortItemNumber.ToString(), _BranchPlant, _UnitOfMeasure, _QtyOnLabel,
                        _ExpRet, UserSession.CurrentUser.LoginName, WorkflowID, _BranchPlantTable.MCCO.ToString(), _PO, "", string.Empty, _locins);
                    }
                }
            }

            if (!_Next)
            {
                RestartLoop(CollectItemNumber);
            }
        }

        private void PrintPILabels()
        {
            DateTime PackDate = new DateTime();
            DateTime ReceivedDate = new DateTime();
            if (_BranchPlant.Trim().ToUpper() == "73W01")
            {
                PackDate = DateTime.Now.AddHours(-2);
                ReceivedDate = DateTime.Now.AddHours(-2);
            }
            else
            {
                PackDate = DateTime.Now;
                ReceivedDate = DateTime.Now;
            }

            PrintSystem.Reset();
            PrintSystem.Branch = _BranchPlant.Trim();
            PrintSystem.ItemNumber = _PrimaryItemNumber;
            PrintSystem.PrinterAddress = _WorkflowConnector.PrinterAddress;
            PrintSystem.PrinterName = _WorkflowConnector.PrinterName;
            PrintSystem.ContainerType = "P";
            PrintSystem.Copies = 1;

            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity", _QtyOnLabel.ToString(), false);

            //DateTime ReceivedDate = DateTime.Now;// Parse(_ReceivedDate.Substring(0, 2) + "/" + _ReceivedDate.Substring(2, 2) + "/" + _ReceivedDate.Substring(4, 4));

            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_DateReceived", ReceivedDate.ToString("MM/dd/yy"), false);

            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Location", _Location.ToUpper(), false);

            //PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_BoxQuantity", _QtyOnLabel.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_LITM", _ItemMasterTable.IMLITM.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PartDesc", _ItemMasterTable.IMDSC1.ToString(), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_PackTime", PackDate.ToString("HH:mm:ss"), false);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("ASR_Packer", UserSession.UserId, false);

            int NextNumber;
            string NextSerialNumber;
            _NextNumberTable.GetNextNumber("InventoryTag", out NextNumber, out NextSerialNumber);
            PrintSystem.ExtraPrintData.AddColumnAndSetDefaultData("Stoneridge_Serial", NextSerialNumber, false);

            //PrintSystem.Copies = (int)_NumberOfLabels;
            PrintSystem.ExtraPrintData.NewRow();

            if (_InventoryTageTable.InsertPITag(_BranchPlant, _ShortItemNumber.ToString(), _ItemMasterTable.IMLITM, _Location, _QtyOnLabel.ToString(),
                NextSerialNumber, UserSession.UserId, "InvTag"))
            {
                if (!PrintSystem.Print())
                {
                    DisplayMessage(PrintSystem.ErrorMessageVocabulary);
                }
            }
            else
            {
                throw new RFS_Exception("INVTAGERROR");
            }

            //else
            //{
            //    _InventoryTageTable.InsertPITag(_BranchPlant, _ShortItemNumber.ToString(), _ItemMasterTable.IMLITM, _Location, _QtyOnLabel.ToString(),
            //                                    NextSerialNumber, UserSession.UserId);
            //    //_SerialTable.InsertSerial(NextSerialNumber, _ItemMasterTable.IMLITM.ToString(), _ShortItemNumber.ToString(), _BranchPlant, _UnitOfMeasure, _QtyOnLabel,
            //    //_ExpRet, UserSession.CurrentUser.LoginName, WorkflowID, _BranchPlantTable.MCCO.ToString(), _PO, "");
            //}
            RestartLoop(CollectItemNumber);
        }

        #endregion

        #region Number of Labels

        private void CollectNumberOfLabels_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (double.TryParse(e.UserInput.ToString(), out _NumberOfLabels))
            {
                if (_NumberOfLabels >= 0)
                {
                    if (_NumberOfLabels > double.Parse(WFOptions.RetrieveOptionValue("MaxLabels")))
                    {
                        throw new RFS_Exception("QtyTooLarge");
                    }

                    if (WFOptions.RetrieveOptionValue("MultipleBoxes") == "N")
                    {
                        PrintLabels(false);
                        RestartLoop(CollectItemNumber);
                    }
                    else
                    {
                        Next(CollectNumberOfCartons);
                    }
                }
                else
                {
                    throw new RFS_Exception("INVALIDQUANTITY");
                }
            }
            else
            {
                throw new RFS_Exception("INVALIDQUANTITY");
            }
        }

        #endregion

        #region Printer List

        private void _DefinedPrintersList_BackValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(CollectPrinter);
        }

        private void _DefinedPrintersList_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DefinedPrintersTable.UpdateCurrentRow();
            _WorkflowConnector.SetPrinter(_DefinedPrintersTable.PrinterName);
        }

        private void _DefinedPrintersList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DefinedPrintersTable.FetchPrintersByUserWorkflow(UserSession.UserId, WorkflowID, _BranchPlant.Trim(), "C");
        }

        #endregion

        #region Collect Container

        private bool CollectContainer_ValidateMethod(string ContainerType)
        {
            _WorkflowConnector.ValidateConainerType(ContainerType.ToUpper());
            switch (_WorkflowConnector.ContainerType)
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

            Next(CollectUOM);
            return true;
        }

        private void CollectContainer_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = _ExpRet;
        }

        #endregion

        #endregion

        private bool CollectUOM_ValidateMethod(string sUOM)
        {
            Next(CollectNumberOfLabels);
            return true;
        }

        private void UOMConversionList_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureConversionTable1.ResetAll();
            _UnitOfMeasureConversionTable1.UMITM = _ItemMasterTable.IMITM;
            _UnitOfMeasureConversionTable1.Fetch();
        }

        private void UOMConversionList_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasureConversionTable1.UpdateCurrentRow();
            _UnitOfMeasure = _UnitOfMeasureConversionTable1.UMUM.ToString();
        }

        private void CollectUOM_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _UnitOfMeasure = e.UserInput.ToString();
            Next(CollectNumberOfLabels);
        }

        private void CollectUOM_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = _ItemMasterTable.IMUOM1;
        }

        private void CollectNumberOfCartons_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (IsItemSerialized())
            {
                ScreenHandler.DefaultText = "1";
            }
        }

        private void CollectNumberOfCartons_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (int.TryParse(e.UserInput.ToString(), out _NumberOfCartons))
            {   
                //if its serial, just ignore what the user input and always use 1
                if (IsItemSerialized())
                {
                    _NumberOfCartons = 1;
                }

                if (_NumberOfCartons > 0)
                {
                    Log.Debug($"<***> MaxCartons {WFOptions.RetrieveOptionValue("MaxCartons")}");
                    if (int.Parse(e.UserInput.ToString()) > int.Parse(WFOptions.RetrieveOptionValue("MaxCartons")))
                    {
                        throw new RFS_Exception("QtyTooLarge");
                    }

                    for (int x = 0; x < _NumberOfCartons; x++)
                    {
                        string LabelCount = $"{x + 1} of {_NumberOfCartons}";
                        //Determine if we are on the last carton
                        if (x == _NumberOfCartons - 1)
                        {
                            PrintLabels(true, LabelCount);
                        }
                        else
                        {
                            PrintLabels(false, LabelCount);
                        }
                    }
                }
                else
                {
                    throw new RFS_Exception("INVALIDQUANTITY");
                }
            }
            else
            {
                throw new RFS_Exception("INVALIDQUANTITY");
            }
        }

        private void CollectDateReceived_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _ReceivedDate = e.UserInput.ToString();
            if (!DateTimeUtils.ValidateAndFormat(ref _ReceivedDate,
                WFOptions.RetrieveOptionValue("DateFormat"), "MMddyyyy"))
            {
                Error("RECDATE", e.UserInput.ToString());
                return;
            }

            //DateTime recdate = DateTime.Parse(_ReceivedDate);
            //_ReceivedDate = recdate.ToString("MMddyyyy");
            Next(CollectRevision);
        }

        private void CollectDateReceived_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenMap.Add("DateFormat", WFOptions.RetrieveOptionValue("DateFormat").ToString());
            ScreenHandler.DefaultText = System.DateTime.Now.ToString(WFOptions.RetrieveOptionValue("DateFormat", false).ToString());
        }

        private void CollectPurchaseOrder_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _PO = e.UserInput.ToString();
            ScreenMap.Add("_PO", _PO);

            double test;
            if (!double.TryParse(_PO, out test))
            {
                throw new RFS_Exception("INVALIDINPUT");
            }

            _PurchaseOrderDetailTable.ResetAll();
            _PurchaseOrderDetailTable.PDMCU = _BranchPlant;
            _PurchaseOrderDetailTable.PDDOCO = test;
            _PurchaseOrderDetailTable.PDLITM = _ItemMasterTable.IMLITM;
            _PurchaseOrderDetailTable.Fetch();
            if (_PurchaseOrderDetailTable.Rows.Count == 0)
            {
                ScreenMap.Add("Error", "Purchase Order Invalid");
            }

            Next(CollectDateReceived);
        }

        private void CollectRevision_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _Revision = e.UserInput.ToString();
            Next(CollectDeliverTo);
        }

        private void CollectDeliverTo_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _DeliverTo = e.UserInput.ToString();
            Next(CollectInspection);
        }

        private void CollectInspection_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (_PurchaseOrderDetailTable.Rows.Count == 0)
            {
                return;
            }

            _ReceiptRouteTable.ResetAll();
            // check to see if item is in receipt routing
            _ReceiptRouteTable.PCAN8 = _PurchaseOrderDetailTable.PDAN8;
            _ReceiptRouteTable.PCMCU = _BranchPlant;
            _ReceiptRouteTable.PCITM = _ShortItemNumber;
            if (_ReceiptRouteTable.Fetch())
            {
                if (_ReceiptRouteTable.PCNROU.Trim().ToUpper().Equals("INSP"))
                {
                    DateTime FromDate = ICS.Utilities.DateTimeUtils.JDEJulian(_ReceiptRouteTable.PCCEFJ);
                    DateTime ThruDate = ICS.Utilities.DateTimeUtils.JDEJulian(_ReceiptRouteTable.PCCXPJ);
                    if ((System.DateTime.Now.Date >= FromDate) && (System.DateTime.Now.Date <= ThruDate))
                    {
                        ScreenHandler.DefaultText = "Y";
                    }
                    else
                    {
                        ScreenHandler.DefaultText = "N";
                    }
                }
                else
                {
                    ScreenHandler.DefaultText = "N";
                }
            }
        }

        private void CollectInspection_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (e.UserInput.ToString().ToUpper() == "Y" || e.UserInput.ToString().ToUpper() == "N")
            {
                if (e.UserInput.ToString().ToUpper() == "Y")
                {
                    _Inspection = "INSPECTION";
                }
                else
                {
                    _Inspection = "";
                }

                Next(CollectNumberOfCartons);
            }
            else
            {
                throw new RFS_Exception("InvalidInput");
            }
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
                    PrintPILabels();
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

        private bool IsItemSerialized()
        {
            return !string.IsNullOrWhiteSpace(_ItemBranchTable.IBSRCE)
                   && _ItemBranchTable.IBSRCE.GetInt() >= Convert.ToInt32(LotSerialDesignations.SerialOptional)
                   && _ItemBranchTable.IBSRCE.GetInt() <= Convert.ToInt32(LotSerialDesignations.SerialRequired);
        }

        private bool PromptForLot()
        {
            int LotType;
            if (_ItemBranchTable.IBSRCE.Trim().Length > 0)
            {
                LotType = Convert.ToInt16(_ItemBranchTable.IBSRCE);
                if ((LotType >= Convert.ToInt16(LotSerialDesignations.LotOptional)) &&
                    (LotType <= Convert.ToInt16(LotSerialDesignations.LotRequired)))
                {
                        return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private bool LotSerialRequired()
        {
            int LotType;
            if (_ItemBranchTable.IBSRCE.Trim().Length > 0)
            {
                LotType = Convert.ToInt16(_ItemBranchTable.IBSRCE);
                if ((LotType != Convert.ToInt16(LotSerialDesignations.LotOptional)) &&
                    (LotType != Convert.ToInt16(LotSerialDesignations.SerialOptional)))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}