using ICS.Display;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using System;
using System.Data;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Picking
{
    public partial class StoneridgeLabelVerification : RFSWorkflowDesigner
    {
        #region Business Connector

        private StoneridgeLabelVerificationConnector _connectorWorkflow;

        public StoneridgeLabelVerificationConnector WorkflowConnector
        {
            get { return _connectorWorkflow ?? (_connectorWorkflow = new StoneridgeLabelVerificationConnector(this)); }
        }

        #endregion

        #region Constructors and Overrides

        public StoneridgeLabelVerification(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeLabelVerification()
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

        private void CollectBranchPlant_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Log.Debug("#**** CollectBranchPlant_LoadEvent");
            WorkflowConnector.ValidateWorkflowOptions();
        }

        private bool CollectBranchPlant_ValidateMethod(string BranchPlant)
        {
            Log.Debug("#**** CollectBranchPlant_ValidateMethod");
            WorkflowConnector.ValidateBranchPlant(BranchPlant);

            Next(CollectPrinter);

            return true;
        }

        private void CollectBranchPlant_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListBranchPlants_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Log.Debug("#**** ListBranchPlants_LoadEvent");
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
            Next(CollectNumberOfPalletLabels);
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
            Log.Debug("#**** CollectShipmentNumber_ValidateMethod");
            WorkflowConnector.ValidateShipmentNumber(ShipmentNumber);

            
            WorkflowConnector.PalletReadinShip = Array.Empty<string>();

            Next(CollectPalletNumber);

            return true;
        }

        private void CollectShipmentNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListShipmentNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Log.Debug("#**** ListShipmentNumbers_LoadEvent");
            WorkflowConnector.LoadShipmentNumberList(ListShipmentNumbers);
        }

        #endregion

        #region Collect Pallet Number

        private void CollectPartNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (WFOptions.RetrieveOptionValue("ValidationBy") == "SERPAR")
            {
                ScreenMap.Add("PalletNumber", WorkflowConnector.PalletNumber + ": " + WorkflowConnector.TotalReadSerialbyPallet.ToString() + " of " + WorkflowConnector.TotalSerialbyPallet.ToString());
                ScreenMap.Add("ShipmentNumber", WorkflowConnector.ShipmentNumber + ": " + WorkflowConnector.TotalReadSerialbyShipper.ToString() + " of " + WorkflowConnector.TotalSerialbyShipper.ToString());
                Log.Debug("#**** CollectSerialNumber_LoadEvent");
                ScreenMap.Add("ScreenMap.Error", WorkflowConnector.TotalReadSerialbyPallet.ToString() + " of " + WorkflowConnector.TotalSerialbyPallet.ToString());
            }
          
        }

        private bool CollectPalletNumber_ValidateMethod(string PalletNumber)
        {

            Log.Debug("#**** CollectPalletNumber_ValidateMethod");
            WorkflowConnector.ValidatePalletNumber(PalletNumber);
            
            if (Array.IndexOf(WorkflowConnector.PalletReadinShip, PalletNumber) > -1)
            {
                throw new RFS_Exception("Plt already Scan");
            }
            else
            {
                Array.Resize(ref WorkflowConnector.PalletReadinShip, WorkflowConnector.PalletReadinShip.Length + 1);

                WorkflowConnector.PalletReadinShip[WorkflowConnector.PalletReadinShip.Length - 1] = PalletNumber;
                Log.Debug("==#ValidationBy:"+WFOptions.RetrieveOptionValue("ValidationBy") );
                if (WFOptions.RetrieveOptionValue("ValidationBy") == "SERPAR")
                {
                    WorkflowConnector.JDECartonDetailInformationTable.GetCustomerPartNoOnPallet(WorkflowConnector.BranchPlant, WorkflowConnector.ShipmentNumber, PalletNumber);


                    foreach (DataRow CartonRow in WorkflowConnector.JDECartonDetailInformationTable.Rows)
                    {
                        Log.Debug("#****PART number in Paller");
                        WorkflowConnector.JDECartonDetailInformationTable.CurrentRow = CartonRow;
                        Array.Resize(ref WorkflowConnector.PartNumberinPallet, WorkflowConnector.PartNumberinPallet.Length + 1);
                        
                        WorkflowConnector.PartNumberinPallet[WorkflowConnector.PartNumberinPallet.Length - 1] = WorkflowConnector.JDECartonDetailInformationTable["ivcitm"].ToString().Trim();
                    }
                   
                    Next(CollectPartNumber);

                }
                else
                {
                    if (WFOptions.RetrieveOptionValue("ValidationBy").ToUpper() == "SERIAL")
                    {
                        WorkflowConnector.JDECartonDetailInformationTable.GetCustomerPartNoOnPallet(WorkflowConnector.BranchPlant, WorkflowConnector.ShipmentNumber, PalletNumber);


                        foreach (DataRow CartonRow in WorkflowConnector.JDECartonDetailInformationTable.Rows)
                        {
                            Log.Debug("==#****PART number in Paller & Serial");
                            WorkflowConnector.JDECartonDetailInformationTable.CurrentRow = CartonRow;

                            Array.Resize(ref WorkflowConnector.PartNumberinPallet, WorkflowConnector.PartNumberinPallet.Length + 1);
                            Array.Resize(ref WorkflowConnector.SerialNumberinPallet, WorkflowConnector.SerialNumberinPallet.Length + 1);

                            WorkflowConnector.PartNumberinPallet[WorkflowConnector.PartNumberinPallet.Length - 1] = WorkflowConnector.JDECartonDetailInformationTable["ivcitm"].ToString().Trim();
                            WorkflowConnector.SerialNumberinPallet[WorkflowConnector.SerialNumberinPallet.Length - 1] = WorkflowConnector.JDECartonDetailInformationTable["cdpak"].ToString().Trim();
                        }
                        Log.Debug("===#****PART number in Paller & Serial:collect serial");
                        Next(CollectSerialNumber);
                    }

                    if (WFOptions.RetrieveOptionValue("ValidationBy") == "PALLET")
                    {
                        Array.Resize(ref WorkflowConnector.SerialNumberinPallet, WorkflowConnector.SerialNumberinPallet.Length + 1);
                        WorkflowConnector.SerialNumberinPallet[WorkflowConnector.SerialNumberinPallet.Length - 1] = WorkflowConnector.JDECartonDetailInformationTable["cdpak"].ToString().Trim();

                        //WorkflowConnector.TotalReadSerialbyPallet++;


                        //if (WorkflowConnector.TotalSerialbyPallet == WorkflowConnector.TotalReadSerialbyPallet)
                        //{
                        

                            if (WorkflowConnector.TotalReadSerialbyShipper == WorkflowConnector.TotalSerialbyShipper)
                            {


                                
                                WorkflowConnector.PrintPalletLabels();
                                Next(CollectShipmentNumber);
                                ScreenMap.Add("Error", WorkflowConnector.ShipmentNumber + ": Success");
                            }
                            else
                            {
                                RestartLoop(CollectPalletNumber);
                           
                                ScreenMap.Add("ShipmentNumber", WorkflowConnector.ShipmentNumber + ": " + WorkflowConnector.TotalReadSerialbyShipper.ToString() + " of " + WorkflowConnector.TotalSerialbyShipper.ToString());
                                ScreenMap.Add("Error", PalletNumber + ": Succes");

                        }
                        //}
                    }
                }
            }
            return true;
        }

        private void CollectPalletNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            if (WFOptions.RetrieveOptionValue("ValidationBy") == "PALLET")
            {
                ScreenMap.Add("ShipmentNumber", WorkflowConnector.ShipmentNumber + ": " + WorkflowConnector.TotalReadSerialbyShipper.ToString() + " of " + WorkflowConnector.TotalSerialbyShipper.ToString());

            }
        }

        private void ListPalletNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Log.Debug("#**** ListPalletNumbers_LoadEvent");
            WorkflowConnector.LoadPalletNumberList(ListPalletNumbers);

            if (WFOptions.RetrieveOptionValue("ValidationBy") == "PALLET")
            {
                ScreenMap.Add("ShipmentNumber", WorkflowConnector.ShipmentNumber + ": " + WorkflowConnector.TotalReadSerialbyShipper.ToString() + " of " + WorkflowConnector.TotalSerialbyShipper.ToString());
                              
            }
        }

        #endregion

        #region Collect Serial Number

        private void CollectSerialNumber_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            // WorkflowConnector.LoadStandardPack();
            ScreenMap.Add("PalletNumber", WorkflowConnector.PalletNumber + ": " + WorkflowConnector.TotalReadSerialbyPallet.ToString() + " of " + WorkflowConnector.TotalSerialbyPallet.ToString());
            ScreenMap.Add("ShipmentNumber", WorkflowConnector.ShipmentNumber + ": " + WorkflowConnector.TotalReadSerialbyShipper.ToString() + " of " + WorkflowConnector.TotalSerialbyShipper.ToString());
            Log.Debug("#**** CollectSerialNumber_LoadEvent");
            ScreenMap.Add("ScreenMap.Error", WorkflowConnector.TotalReadSerialbyPallet.ToString()+" of "+WorkflowConnector.TotalSerialbyPallet.ToString());
        }

        private bool CollectSerialNumber_ValidateMethod(string SerialNumber)
        {
            Log.Debug("#**** CollectSerialNumber_ValidateMethod");
            Log.Debug("#**** SerialNumber:"+ SerialNumber);
            //agch 
            if (WorkflowConnector.ValidateSerialNumber(SerialNumber)==0)
            {
                Log.Debug("#**** CollectSerialNumber_LoadEvent --"+ WFOptions.RetrieveOptionValue("ValidationBy") + "--");
                
                if (WFOptions.RetrieveOptionValue("ValidationBy") == "SERPAR")
                {
                    Log.Debug("#**** CollectSerialNumber_LoadEvent SERPAR=TRUE");
                    Next(CollectPartNumber);
                }
                else
                {
                    Log.Debug("#**** CollectSerialNumber_LoadEvent SERPAR=False");
                    Next(CollectSerialNumbership);
                }

                    

            }
            else
            {
                Log.Debug("#**** CollectSerialNumber_ValidateMethod--back");
                
                Back(CollectSerialNumber);

            }

            return true;
        }



        private bool CollectPartNumber_ValidateMethod(string PartNumber)
        {
            Log.Debug("#**** CollectPartNumber_ValidateMethod");
            if (WFOptions.RetrieveOptionValue("ValidationBy") == "SERPAR")
            {
                if (Array.IndexOf(WorkflowConnector.PartNumberinPallet, PartNumber) < 0)
                {
                    throw new RFS_Exception("Invalid Part number Number");
                }
                else
                {
                    WorkflowConnector.TotalReadSerialbyPallet++;
                    WorkflowConnector.PartNumberinPallet[Array.IndexOf(WorkflowConnector.PartNumberinPallet, PartNumber)] = "@";

                    if (WorkflowConnector.TotalSerialbyPallet == WorkflowConnector.TotalReadSerialbyPallet)
                    {
                        WorkflowConnector.PrintPalletLabels();
                        if (WorkflowConnector.TotalReadSerialbyShipper == WorkflowConnector.TotalSerialbyShipper)
                        {
                            Next(CollectShipmentNumber);
                        }
                        else
                        {
                            RestartLoop(CollectPalletNumber);
                        }
                    }
                    else
                    {
                        Next(CollectPartNumber);

                        ScreenMap.Add("PalletNumber", WorkflowConnector.PalletNumber + ": " + WorkflowConnector.TotalReadSerialbyPallet.ToString() + " of " + WorkflowConnector.TotalSerialbyPallet.ToString());
                        ScreenMap.Add("ShipmentNumber", WorkflowConnector.ShipmentNumber + ": " + WorkflowConnector.TotalReadSerialbyShipper.ToString() + " of " + WorkflowConnector.TotalSerialbyShipper.ToString());
                        Log.Debug("#**** CollectSerialNumber_LoadEvent");
                        ScreenMap.Add("ScreenMap.Error", WorkflowConnector.TotalReadSerialbyPallet.ToString() + " of " + WorkflowConnector.TotalSerialbyPallet.ToString());


                    }

                }
            }
            else //by SERPAr
            {
                if (Array.IndexOf(WorkflowConnector.SerialNumberinPallet, WorkflowConnector.SerialNumber) < 0)
                {
                    throw new RFS_Exception("Invalid Part number Number");
                }
                else
                {
                    if (WorkflowConnector.PartNumberinPallet[Array.IndexOf(WorkflowConnector.SerialNumberinPallet, WorkflowConnector.SerialNumber)] == PartNumber)
                    {

                        WorkflowConnector.TotalReadSerialbyPallet++;
                        WorkflowConnector.PartNumberinPallet[Array.IndexOf(WorkflowConnector.SerialNumberinPallet, WorkflowConnector.SerialNumber)] = "@";

                        if (WorkflowConnector.TotalSerialbyPallet == WorkflowConnector.TotalReadSerialbyPallet)
                        {
                            WorkflowConnector.PrintPalletLabels();
                            if (WorkflowConnector.TotalReadSerialbyShipper == WorkflowConnector.TotalSerialbyShipper)
                            {
                                Next(CollectShipmentNumber);
                            }
                            else
                            {
                                RestartLoop(CollectPalletNumber);
                            }
                        }
                        else
                        {
                            Next(CollectSerialNumber);
                        } 
                    }
                    else
                    {

                        throw new RFS_Exception("Invalid Part number Number");

                    }

                }



            }
            
            
            
            
            
           

            return true;
        }




        //agch Label Verification Begin
        private bool CollectSerialNumbership_ValidateMethod(string SerialNumbership)
        {
            Log.Debug("#**** CollectSerialNumbership_ValidateMethod");
            string[] listprefix  =  WFOptions.RetrieveOptionValue("CartonPrefixRemoved").Split(' ');
            foreach (string s in listprefix)
            {
                if (SerialNumbership.StartsWith(s))
                {
                    SerialNumbership=SerialNumbership.Substring(s.Length);  
                }
            }



            if (WorkflowConnector.ValidateSerialNumbership(SerialNumbership) == 0)
            {
               
                    if (WorkflowConnector.TotalSerialbyPallet == WorkflowConnector.TotalReadSerialbyPallet)
                    {
                        WorkflowConnector.PrintPalletLabels();
                        if (WorkflowConnector.TotalReadSerialbyShipper == WorkflowConnector.TotalSerialbyShipper)
                        {
                            Next(CollectShipmentNumber);
                        }
                        else
                        {
                            RestartLoop(CollectPalletNumber);
                        }
                    }
                    else
                    {
                        Next(CollectSerialNumber);
                    }
                
            }
            else
            {
                Back(CollectSerialNumbership);

            }



            return true;
        }
        //agch Label Verification Begin


        private void CollectSerialNumber_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListQuantityRemaining_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Log.Debug("#**** ListQuantityRemaining_LoadEvent");
            WorkflowConnector.LoadQuantityRemainingList(ListQuantityRemaining);
        }

        private void ListQuantityRemaining_EnterValidationEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            Log.Debug("#**** ListQuantityRemaining_EnterValidationEvent");
            Back(CollectSerialNumber);
        }

        private void ListQuantityRemaining_LeavingEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private bool CollectSerialNumber_F6Method(RFSScreenControl sender, ScreenEventArgs e)
        {
            Log.Debug("#**** CollectSerialNumber_F6Method");
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
            Log.Debug("#**** WarnInvalidSerial_ValidateMethod");

            Back(CollectSerialNumber);

            return true;
        }

        #endregion

        #region Warn Serial Consumed

        private bool WarnSerialConsumed_ValidateMethod(string Confirmation)
        {
            Log.Debug("#**** WarnSerialConsumed_ValidateMethod");

            Back(CollectSerialNumber);

            return true;
        }

        #endregion

        #region Confirm Full Pallet

        private bool ConfirmFullPallet_ValidateMethod(string Confirmation)
        {
            Log.Debug("#**** ConfirmFullPallet_ValidateMethod");
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
            Log.Debug("#**** CollectNumberOfCartonLabels_ValidateMethod");

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
            Log.Debug("#**** CollectNumberOfPalletLabels_ValidateMethod");
            WorkflowConnector.ValidateNumberOfPalletLabels(NumberOfPalletLabels);
            Next(CollectShipmentNumber);
            return true;
        }

        #endregion

        #region ExpendablePackagingWarning

        private bool ExpendablePackagingWarning_ValidateMethod(string sInput)
        {
            Log.Debug("#**** ExpendablePackagingWarning_ValidateMethod");
            if (sInput.GetYesNo())
            {
                Log.Debug("<***>WorkflowConnector.SendPackCartonToERP()");
                if (WorkflowConnector.SendPackCartonToERP())
                {
                    //Next(CollectNumberOfCartonLabels);
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
                }
                else
                {
                    RestartLoop(CollectSerialNumber);
                }
            }
            else
            {
                Back();
            }

            return true;
        }

        #endregion

        #endregion
    }
}