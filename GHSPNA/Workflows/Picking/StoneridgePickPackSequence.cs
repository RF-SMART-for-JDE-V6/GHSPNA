namespace ICS.Stoneridge.Wireless.JDE.Workflow.Picking
{
    #region Usings

    using ICS.Display;
    using ICS.Utilities;
    using ICS.Workflow.Base.WorkflowDesign.Components;
    using Utilities;

    #endregion

    public partial class StoneridgePickPackSequence : RFSWorkflowDesigner
    {
        #region Business Connector

        private StoneridgePickPackSequenceConnector _connectorWorkflow;

        public StoneridgePickPackSequenceConnector WorkflowConnector
        {
            get { return _connectorWorkflow ?? (_connectorWorkflow = new StoneridgePickPackSequenceConnector(this)); }
        }

        #endregion

        #region Constructors and Overrides

        public StoneridgePickPackSequence(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgePickPackSequence()
        {
            InitializeComponent();
        }

        protected override void Initialize()
        {
            WorkflowConnector.Init();
        }

        public override void Dispose()
        {
            ScreenMap.ClearAllWorkflows();
            WorkflowConnector.Dispose();
            base.Dispose();
        }

        #endregion

        #region Screen Methods

        #region Collect Branch Plant

        private void CollectBranchPlant_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.ValidateWorkflowOptions();
        }

        private bool CollectBranchPlant_ValidateMethod(string BranchPlant)
        {
            WorkflowConnector.ValidateBranchPlant(BranchPlant);

            Next(CollectPrinter);

            return true;
        }

        private void CollectBranchPlant_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListBranchPlants_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadBranchPlantList(ListBranchPlants);
        }

        #endregion

        #region Collect Printer

        private void CollectPrinter_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadDefaultPrinter();
        }

        private bool CollectPrinter_ValidateMethod(string Printer)
        {
            WorkflowConnector.ValidatePrinter(Printer);

            //Next(CollectShipmentNumber);
            //Next(CollectNumberOfCartonLabels);
            Next(PrintCartonLabel);
            return true;
        }

        private void CollectPrinter_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListPrinters_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadPrinterList(ListPrinters);
        }

        #endregion

        #region Collect Shipment Number

        private bool CollectShipmentNumber_ValidateMethod(string ShipmentNumber)
        {
            WorkflowConnector.ValidateShipmentNumber(ShipmentNumber);

            Next(CollectPalletNumber);

            return true;
        }

        private void CollectShipmentNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListShipmentNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadShipmentNumberList(ListShipmentNumbers);
        }

        #endregion

        #region Collect Pallet Number

        private bool CollectPalletNumber_ValidateMethod(string PalletNumber)
        {
            WorkflowConnector.ValidatePalletNumber(PalletNumber);

            Next(CollectSerialNumber);

            return true;
        }

        private void CollectPalletNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListPalletNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadPalletNumberList(ListPalletNumbers);
        }

        #endregion

        #region Collect Serial Number

        private void CollectSerialNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadStandardPack();
        }

        private bool CollectSerialNumber_ValidateMethod(string SerialNumber)
        {
            int Warning = WorkflowConnector.ValidateSerialNumber(SerialNumber);

            if (Warning == 0)
            {
                //Check before sending to ERP
                WorkflowConnector.SendPackCartonToERPPreCheck();
                if (WorkflowConnector.SendPackCartonToERP())
                {
                    //Next(CollectNumberOfCartonLabels);
                    Log.Debug($"<***>Print Carton Labels");
                    if (WorkflowConnector.PrintCartonLabel)
                    {
                        WorkflowConnector.PrintCartonLabels();
                    }

                    if (WorkflowConnector.FullItemQuantityIsPacked())
                    {
                        WorkflowConnector.EnoughQuantityToPick();
                        if (WorkflowConnector.FullShipmentQuantityIsPacked())
                        {
                            if (WorkflowConnector.SendPickSalesOrderToERP())
                            {
                                Next(CollectPalletLabelType);
                            }
                            else
                            {
                                RestartLoop(CollectSerialNumber);
                            }
                        }
                        else
                        {
                            RestartLoop(CollectSerialNumber);
                        }
                    }
                    else
                    {
                        RestartLoop(CollectSerialNumber);
                    }
                }
                else
                {
                    RestartLoop(CollectSerialNumber);
                }
            }
            else if (Warning == 1)
            {
                Next(WarnInvalidSerial);
            }
            else
            {
                Next(WarnSerialConsumed);
            }

            return true;
        }

        private void CollectSerialNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListQuantityRemaining_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadQuantityRemainingList(ListQuantityRemaining);
        }

        private void ListQuantityRemaining_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Back(CollectSerialNumber);
        }

        private void ListQuantityRemaining_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private bool CollectSerialNumber_F6Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (WorkflowConnector.TransactionSentForShipment())
            {
                Next(ConfirmFullPallet);
            }
            else
            {
                throw new RFS_Exception("TxNotSentForShipment");
            }

            return true;
        }

        #endregion

        #region Warn Invalid Serial

        private bool WarnInvalidSerial_ValidateMethod(string Confirmation)
        {
            Back(CollectSerialNumber);

            return true;
        }

        #endregion

        #region Warn Serial Consumed

        private bool WarnSerialConsumed_ValidateMethod(string Confirmation)
        {
            Back(CollectSerialNumber);

            return true;
        }

        #endregion

        #region Confirm Full Pallet

        private bool ConfirmFullPallet_ValidateMethod(string Confirmation)
        {
            if (WorkflowConnector.ValidateYesNoConfirmation(Confirmation))
            {
                WorkflowConnector.EnoughQuantityToPick();
                if (WorkflowConnector.SendPickSalesOrderToERP())
                {
                    Next(CollectPalletLabelType);
                }
                else
                {
                    RestartLoop(CollectSerialNumber);
                }
            }
            else
            {
                RestartLoop(CollectSerialNumber);
            }

            return true;
        }

        #endregion

        #region Collect Number Of Carton Labels

        private bool CollectNumberOfCartonLabels_ValidateMethod(string NumberOfCartonLabels)
        {
            WorkflowConnector.ValidateNumberOfCartonLabels(NumberOfCartonLabels);
            /*
            WorkflowConnector.PrintCartonLabels();

            if (WorkflowConnector.FullItemQuantityIsPacked())
            {
                WorkflowConnector.EnoughQuantityToPick();
                if (WorkflowConnector.FullShipmentQuantityIsPacked())
                {
                    if (WorkflowConnector.SendPickSalesOrderToERP())
                    {
                        Next(CollectPalletLabelType);
                    }
                    else
                    {
                        RestartLoop(CollectSerialNumber);
                    }
                }
                else
                {
                    RestartLoop(CollectSerialNumber);
                }
            }
            else
            {
                RestartLoop(CollectSerialNumber);
            }
            */
            Next(CollectShipmentNumber);
            return true;
        }

        #endregion

        #region Collect Pallet Label Type

        private bool CollectPalletLabelType_ValidateMethod(string PalletLabelType)
        {
            WorkflowConnector.ValidatePalletLabelType(PalletLabelType);

            if (WorkflowConnector.PalletLabelType == 0)
            {
                if (WorkflowConnector.FullShipmentQuantityIsPacked())
                {
                    RestartLoop(CollectShipmentNumber);
                }
                else
                {
                    RestartLoop(CollectPalletNumber);
                }
            }
            else
            {
                Next(CollectNumberOfPalletLabels);
            }

            return true;
        }

        #endregion

        #region Collect Number Of Pallet Labels

        private bool CollectNumberOfPalletLabels_ValidateMethod(string NumberOfPalletLabels)
        {
            WorkflowConnector.ValidateNumberOfPalletLabels(NumberOfPalletLabels);
            WorkflowConnector.PrintPalletLabels();

            if (WorkflowConnector.FullShipmentQuantityIsPacked())
            {
                RestartLoop(CollectShipmentNumber);
            }
            else
            {
                RestartLoop(CollectPalletNumber);
            }

            return true;
        }

        #endregion

        #region PrintCartonLabel

        private bool PrintCartonLabels_ValidateMethod(string sInput)
        {
            WorkflowConnector.CartonLabelPrint(sInput);
            if (WorkflowConnector.PrintCartonLabel)
            {
                Next(CollectNumberOfCartonLabels);
            }
            else
            {
                Next(CollectShipmentNumber);
            }

            return true;
        }

        #endregion

        #endregion
    }
}