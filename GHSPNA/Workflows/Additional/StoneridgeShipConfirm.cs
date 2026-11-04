namespace ICS.Stoneridge.Wireless.JDE.Workflow.Additional
{
    #region Usings

    using ICS.Display;
    using ICS.Workflow.Base.WorkflowDesign.Components;

    #endregion

    public partial class StoneridgeShipConfirm : RFSWorkflowDesigner
    {
        #region Business Connector

        private StoneridgeShipConfirmConnector _connectorWorkflow;

        public StoneridgeShipConfirmConnector WorkflowConnector
        {
            get { return _connectorWorkflow ?? (_connectorWorkflow = new StoneridgeShipConfirmConnector(this)); }
        }

        #endregion

        #region Constructors and Overrides

        public StoneridgeShipConfirm(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeShipConfirm()
        {
            InitializeComponent();
        }

        protected override void Initialize()
        {
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

        private bool CollectBranchPlant_ValidateMethod(string BranchPlant)
        {
            WorkflowConnector.ValidateBranchPlant(BranchPlant);

            Next(CollectShipmentNumber);

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

        #region Collect Shipment Number

        private bool CollectShipmentNumber_ValidateMethod(string ShipmentNumber)
        {
            int Warning = WorkflowConnector.ValidateShipmentNumber(ShipmentNumber);

            if (Warning == 0)
            {
                Next(CollectSCAC);
            }
            else if (Warning == 1)
            {
                Next(WarnShipmentStatus);
            }
            else
            {
                Next(WarnSalesOrderStatus);
            }

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

        #region Warn Shipment Status

        private bool WarnShipmentStatus_ValidateMethod(string Confirmation)
        {
            RestartLoop(CollectShipmentNumber);

            return true;
        }

        #endregion

        #region Warn Sales Order Status

        private bool WarnSalesOrderStatus_ValidateMethod(string Confirmation)
        {
            RestartLoop(CollectShipmentNumber);

            return true;
        }

        #endregion

        #region Collect SCAC

        private void CollectSCAC_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadShipmentSCAC();
        }

        private bool CollectSCAC_ValidateMethod(string SCAC)
        {
            WorkflowConnector.ValidateSCAC(SCAC);

            Next(CollectTrailerNumber);

            return true;
        }

        private void CollectSCAC_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListSCACs_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadSCACList(ListSCACs);
        }

        #endregion

        #region Collect Trailer Number

        private void CollectTrailerNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadShipmentTrailerNumber();
        }

        private bool CollectTrailerNumber_ValidateMethod(string TrailerNumber)
        {
            WorkflowConnector.ValidateTrailerNumber(TrailerNumber);

            Next(CollectPRONumber);

            return true;
        }

        #endregion

        #region Collect PRO Number

        private void CollectPRONumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadShipmentPRONumber();
        }

        private bool CollectPRONumber_ValidateMethod(string PRONumber)
        {
            WorkflowConnector.ValidatePRONumber(PRONumber);

            Next(CollectSealNumber);

            return true;
        }

        #endregion

        #region Collect Seal Number

        private void CollectSealNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadShipmentSealNumber();
        }

        private bool CollectSealNumber_ValidateMethod(string SealNumber)
        {
            WorkflowConnector.ValidateSealNumber(SealNumber);
            WorkflowConnector.LoadOriginalShipmentWeight();

            Next(ConfirmShipment);

            return true;
        }

        #endregion

        #region Confirm Shipment

        private bool ConfirmShipment_ValidateMethod(string Confirmation)
        {
            if (WorkflowConnector.ValidateYesNoConfirmation(Confirmation))
            {
                if (WorkflowConnector.SendTrailerNumberUpdateToERP())
                {
                    if (WorkflowConnector.SendPRONumberUpdateToERP())
                    {
                        if (WorkflowConnector.SendSealNumberUpdateToERP())
                        {
                            if (WorkflowConnector.SendShipConfirmationToERP())
                            {
                                if (WorkflowConnector.QuickReturnLabelIsRequired())
                                {
                                    Next(CollectNumberOfLabels);
                                }
                                else
                                {
                                    RestartLoop(CollectShipmentNumber);
                                }
                            }
                            else
                            {
                                Log.Debug("*** Ship Confirmation Failed");
                                RestartLoop(CollectShipmentNumber);
                            }
                        }
                        else
                        {
                            Log.Debug("*** Seal Number Reference Update Failed");
                            RestartLoop(CollectShipmentNumber);
                        }
                    }
                    else
                    {
                        Log.Debug("*** PRO Number Reference Update Failed");
                        RestartLoop(CollectShipmentNumber);
                    }
                }
                else
                {
                    Log.Debug("*** Trailer Number Reference Update Failed");
                    RestartLoop(CollectShipmentNumber);
                }
            }
            else
            {
                RestartLoop(CollectShipmentNumber);
            }

            return true;
        }

        private bool ConfirmShipment_F2Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            Next(CollectShipmentWeight);

            return true;
        }

        #endregion

        #region Collect Shipment Weight

        private bool CollectShipmentWeight_ValidateMethod(string ShipmentWeight)
        {
            WorkflowConnector.ValidateShipmentWeight(ShipmentWeight);

            Next(CollectShipmentUnitOfMeasure);

            return true;
        }

        #endregion

        #region Collect Shipment Unit Of Measure

        private void CollectShipmentUnitOfMeasure_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            ScreenHandler.DefaultText = WorkflowConnector.LoadShipmentUnitOfMeasure();
        }

        private bool CollectShipmentUnitOfMeasure_ValidateMethod(string ShipmentUnitOfMeasure)
        {
            WorkflowConnector.ValidateShipmentUnitOfMeasure(ShipmentUnitOfMeasure);

            RestartLoop(ConfirmShipment);

            return true;
        }

        private void CollectShipmentUnitOfMeasure_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListShipmentUnitsOfMeasure_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadShipmentUnitOfMeasureList(ListShipmentUnitsOfMeasure);
        }

        #endregion

        #region Collect Number Of Labels

        private bool CollectNumberOfLabels_ValidateMethod(string NumberOfLabels)
        {
            WorkflowConnector.ValidateNumberOfLabels(NumberOfLabels);

            Next(CollectPrinter);

            return true;
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

            WorkflowConnector.PrintBarcodeLabels();

            RestartLoop(CollectShipmentNumber);

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

        #endregion
    }
}